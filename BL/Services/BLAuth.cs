using System;
using System.Threading.Tasks;
using System.Linq;
using FamilyTree.Core;
using FamilyTree.Data;
using FamilyTree.DB.Interfaces;
using FamilyTree.Models.Common;
using FamilyTree.Models.Master;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Logging;

namespace FamilyTree.BL.Services
{
    public class BLAuth : BLCommon<User>, IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IOtpService _otpService;
        private readonly IEmailService _emailService;
        private readonly DbSet<User> _userSet;
        private readonly ITokenService _tokenService;
        private readonly ILogger<BLAuth> _logger;

        public BLAuth(IAuthRepository authRepository, DataContext context, IOtpService otpService, IEmailService emailService, ITokenService tokenService, ILogger<BLAuth> logger)
            : base(authRepository)
        {
            _authRepository = authRepository;
            _otpService = otpService;
            _emailService = emailService;
            _tokenService = tokenService;
            _userSet = context.Set<User>();
            _logger = logger;
        }


        public Response ValidateUser(LoginRequest request)
        {
            Response response = _authRepository.ValidateUser(request);

            if (!response.IsError)
            {
                string token = _tokenService.GenerateToken(response.Id ?? 0, request.UserRoleId);

                LoginValidate objLV = new LoginValidate
                {
                    Username = request.Username,
                    UserRoleId = request.UserRoleId,
                    UserId = response.Id ?? 0,
                    Token = token
                };

                response.DataModel = objLV;
            }

            return response;
        }

        public async Task<Response> RegisterUser(RegisterRequest request)
        {
            Response requestValidation = ValidateRegisterUserRequest(request);
            if (requestValidation.IsError)
            {
                return requestValidation;
            }

            User newUser = BuildNewUser(request);

            Response entityValidation = ValidationBeforePreSave(newUser);
            if (entityValidation.IsError)
            {
                return entityValidation;
            }

            // Attempt to send OTP first
            Response otpDispatchResponse = await DispatchOtp(newUser.Email);
            if (otpDispatchResponse.IsError)
            {
                return otpDispatchResponse;
            }

            // If OTP sent successfully, save the user
            EntryType = enmEntryType.A;
            Presave(newUser);
            Response saveResponse = AddOrUpdate();

            if (saveResponse.IsError)
            {
                return saveResponse;
            }

            // Construct success response
            saveResponse.Message = "Registration successful. OTP sent to the registered email address.";
            saveResponse.DataModel = new
            {
                Username = newUser.Username,
                Email = newUser.Email
            };

            return saveResponse;
        }

        public Response VerifyOtp(VerifyOtpRequest request)
        {
            Response requestValidation = ValidateVerifyOtpRequest(request);
            if (requestValidation.IsError)
            {
                return requestValidation;
            }

            string normalizedEmail = NormalizeEmail(request.Email);
            string sanitizedOtp = request.OtpCode.Trim();

            User? user = FindUserByEmail(normalizedEmail);
            if (user == null)
            {
                return CreateErrorResponse(MessageCode.E010);
            }

            Response otpValidation = ValidateAndConsumeOtp(normalizedEmail, sanitizedOtp);
            if (otpValidation.IsError)
            {
                return otpValidation;
            }

            Response persistenceResponse = PersistExistingUser(user);
            if (persistenceResponse.IsError)
            {
                return persistenceResponse;
            }

            return BuildOtpVerificationResponse(user.UserId, user.Username ?? string.Empty, normalizedEmail, user.UserRoleId);
        }

        public Response UpdateProfile(UpdateProfileRequest request)
        {
            User? user = _userSet.FirstOrDefault(u => u.UserId == request.UserId);
            if (user == null)
            {
                return CreateErrorResponse(MessageCode.E013); // User not found
            }

            // Update fields
            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                user.Username = NormalizeUsername(request.Username);
            }
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                user.Email = NormalizeEmail(request.Email);
            }
            // Mobile number can be null or empty
            user.MobileNumber = request.MobileNumber;

            // Persist (ValidationBeforePreSave will handle dupe checks)
            return PersistExistingUser(user);
        }

        public async Task<Response> ChangePassword(ChangePasswordRequest request)
        {
            string normalizedEmail = NormalizeEmail(request.Email);
            User? user = FindUserByEmail(normalizedEmail);
            if (user == null)
            {
                return CreateErrorResponse(MessageCode.E010);
            }

            // Verify OTP
            Response otpValidation = ValidateAndConsumeOtp(normalizedEmail, request.Otp);
            if (otpValidation.IsError)
            {
                return otpValidation;
            }

            // Update Password
            user.Password = request.NewPassword;

            // Persist
            return PersistExistingUser(user);
        }

        public async Task<Response> SendOtp(SendOtpRequest request)
        {
            return await DispatchOtp(request.Email);
        }

        public override Response ValidationBeforePreSave(User user)
        {
            Response validationResponse = new Response();
            if (user == null)
            {
                validationResponse.IsError = true;
                validationResponse.MessageCode = MessageCode.E013.ToString();
                return validationResponse;
            }

            bool usernameExists = _userSet
                .AsNoTracking()
                .Any(existingUser => existingUser.Username == user.Username && existingUser.UserId != user.UserId);

            if (usernameExists)
            {
                validationResponse.IsError = true;
                validationResponse.MessageCode = MessageCode.E011.ToString();
                return validationResponse;
            }

            bool emailExists = _userSet
                .AsNoTracking()
                .Any(existingUser => existingUser.Email == user.Email && existingUser.UserId != user.UserId);

            if (emailExists)
            {
                validationResponse.IsError = true;
                validationResponse.MessageCode = MessageCode.E007.ToString();
            }

            return validationResponse;
        }

        public override void Presave(User entity)
        {
            base.Presave(entity);

            DateTime utcNow = DateTime.UtcNow;

            if (EntryType == enmEntryType.A)
            {
                _entity.CreationDatetime = utcNow;
            }

            _entity.ModificationDatetime = utcNow;

            if (!string.IsNullOrWhiteSpace(entity.Password))
            {
                string sanitizedPassword = entity.Password.Trim();
                _entity.PasswordHash = PasswordEncryptionDecryption.HashPassword(sanitizedPassword);
                _entity.Password = null;
            }
        }

        private Response ValidateRegisterUserRequest(RegisterRequest? request)
        {
            if (request == null)
            {
                return CreateErrorResponse(MessageCode.E016);
            }

            Response validationResponse = new Response();
            return validationResponse;
        }

        private async Task<Response> DispatchOtp(string email)
        {
            _logger.LogInformation("[BLAuth] Dispatching OTP to {Email}", email);
            Response otpResponse = new Response();

            string sanitizedEmail = NormalizeEmail(email);
            if (string.IsNullOrWhiteSpace(sanitizedEmail))
            {
                otpResponse.IsError = true;
                otpResponse.MessageCode = MessageCode.E015.ToString();
                return otpResponse;
            }

            try
            {
                string otpCode = _otpService.GenerateOtp(sanitizedEmail);
                _logger.LogInformation("[BLAuth] OTP Generated. Sending email...");
                await _emailService.SendOtpEmailAsync(sanitizedEmail, otpCode, 5);
                _logger.LogInformation("[BLAuth] Email sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[BLAuth] Error dispatching OTP");
                otpResponse.IsError = true;
                otpResponse.MessageCode = ex.Message + MessageCode.E015.ToString();
            }

            return otpResponse;
        }

        private Response PersistExistingUser(User user)
        {
            response = new Response();
            EntryType = enmEntryType.E;

            Presave(user);
            return AddOrUpdate();
        }

        private Response BuildOtpVerificationResponse(int userId, string username, string normalizedEmail, int userRoleId)
        {
            string token = _tokenService.GenerateToken(userId, userRoleId);

            Response verificationResponse = new Response
            {
                Id = userId,
                Message = "OTP verified successfully.",
                DataModel = new
                {
                    Username = username,
                    Email = normalizedEmail,
                    UserRoleId = userRoleId,
                    Token = token,
                    VerifiedAt = DateTime.UtcNow
                }
            };
            return verificationResponse;
        }

        private Response ValidateVerifyOtpRequest(VerifyOtpRequest? request)
        {
            if (request == null)
            {
                return CreateErrorResponse(MessageCode.E017);
            }

            Response validationResponse = new Response();
            return validationResponse;
        }

        private User? FindUserByEmail(string normalizedEmail)
        {
            return _userSet.FirstOrDefault(existingUser => existingUser.Email == normalizedEmail);
        }

        private Response ValidateAndConsumeOtp(string email, string otpCode)
        {
            OtpVerification? otpRecord = _otpService.GetLatestOtp(email);
            if (otpRecord == null)
            {
                return CreateErrorResponse(MessageCode.E009);
            }

            if (otpRecord.IsUsed)
            {
                return CreateErrorResponse(MessageCode.E018);
            }

            if (otpRecord.ExpirationTime < DateTime.UtcNow)
            {
                return CreateErrorResponse(MessageCode.E008);
            }

            if (!string.Equals(otpRecord.OtpCode, otpCode, StringComparison.Ordinal))
            {
                return CreateErrorResponse(MessageCode.E009);
            }

            _otpService.MarkOtpAsUsed(otpRecord);

            Response validationResponse = new Response();
            return validationResponse;
        }

        private static User BuildNewUser(RegisterRequest request)
        {
            string normalizedUsername = NormalizeUsername(request.Username);
            string normalizedEmail = request.Email;
            User newUser = new User
            {
                Username = normalizedUsername,
                Email = normalizedEmail,
                MobileNumber = null,
                Password = request.Password.Trim(),
                UserRoleId = request.UserRoleId
            };
            return newUser;
        }

        private static string NormalizeEmail(string? email)
        {
            return email?.Trim().ToLowerInvariant() ?? string.Empty;
        }

        private static string NormalizeUsername(string? username)
        {
            return username?.Trim() ?? string.Empty;
        }

        private static Response CreateErrorResponse(MessageCode code)
        {
            Response errorResponse = new Response
            {
                IsError = true,
                MessageCode = code.ToString()
            };
            return errorResponse;
        }
    }
}
