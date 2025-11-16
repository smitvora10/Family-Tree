using System;
using System.Linq;
using FamilyTree.Core;
using FamilyTree.Data;
using FamilyTree.DB.Interfaces;
using FamilyTree.Models.Common;
using FamilyTree.Models.Master;
using Microsoft.EntityFrameworkCore;

namespace FamilyTree.BL.Services
{
    public class BLAuth : BLCommon<User>, IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IOtpService _otpService;
        private readonly DbSet<User> _userSet;

        public BLAuth(IAuthRepository authRepository, DataContext context, IOtpService otpService)
            : base(authRepository)
        {
            _authRepository = authRepository;
            _otpService = otpService;
            _userSet = context.Set<User>();
        }

        public Response ValidateUser(LoginRequest request)
        {
            return _authRepository.ValidateUser(request);
        }

        public Response RegisterUser(RegisterRequest request)
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

            Response persistenceResponse = PersistNewUser(newUser);
            if (persistenceResponse.IsError)
            {
                return persistenceResponse;
            }

            return FinalizeRegistration(persistenceResponse, newUser);
        }

        public Response VerifyOtp(VerifyOtpRequest request)
        {
            Response requestValidation = ValidateVerifyOtpRequest(request);
            if (requestValidation.IsError)
            {
                return requestValidation;
            }

            string normalizedMobile = NormalizeMobile(request.MobileNumber);
            string sanitizedOtp = request.OtpCode.Trim();

            User? user = FindUserByMobile(normalizedMobile);
            if (user == null)
            {
                return CreateErrorResponse(MessageCode.E010);
            }

            Response otpValidation = ValidateAndConsumeOtp(normalizedMobile, sanitizedOtp);
            if (otpValidation.IsError)
            {
                return otpValidation;
            }

            Response persistenceResponse = PersistExistingUser(user);
            if (persistenceResponse.IsError)
            {
                return persistenceResponse;
            }

            return BuildOtpVerificationResponse(user.UserId, user.Username ?? string.Empty, normalizedMobile);
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

            bool mobileExists = _userSet
                .AsNoTracking()
                .Any(existingUser => existingUser.MobileNumber == user.MobileNumber && existingUser.UserId != user.UserId);

            if (mobileExists)
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

        private Response PersistNewUser(User user)
        {
            response = new Response();
            EntryType = enmEntryType.A;

            Presave(user);
            return AddOrUpdate();
        }

        private Response FinalizeRegistration(Response persistenceResponse, User pendingUser)
        {
            User? createdUser = ExtractCreatedUser(persistenceResponse, pendingUser);
            if (createdUser == null)
            {
                Response failureResponse = new Response
                {
                    IsError = true,
                    MessageCode = MessageCode.E014.ToString()
                };
                return failureResponse;
            }

            Response otpDispatchResponse = DispatchOtp(createdUser.MobileNumber);
            if (otpDispatchResponse.IsError)
            {
                return otpDispatchResponse;
            }

            persistenceResponse.Id = createdUser.UserId;
            persistenceResponse.Message = "Registration successful. OTP sent to the registered mobile number.";
            persistenceResponse.DataModel = new
            {
                Username = createdUser.Username,
                MobileNumber = createdUser.MobileNumber
            };

            return persistenceResponse;
        }

        private static User? ExtractCreatedUser(Response persistenceResponse, User pendingUser)
        {
            if (persistenceResponse.DataModel is User createdUserFromResponse)
            {
                return createdUserFromResponse;
            }

            if (pendingUser != null && pendingUser.UserId > 0)
            {
                return pendingUser;
            }

            return null;
        }

        private Response DispatchOtp(string mobileNumber)
        {
            Response otpResponse = new Response();

            string sanitizedMobileNumber = NormalizeMobile(mobileNumber);
            if (string.IsNullOrWhiteSpace(sanitizedMobileNumber))
            {
                otpResponse.IsError = true;
                otpResponse.MessageCode = MessageCode.E015.ToString();
                return otpResponse;
            }

            try
            {
                _otpService.GenerateOtp(sanitizedMobileNumber);
            }
            catch (ArgumentException)
            {
                otpResponse.IsError = true;
                otpResponse.MessageCode = MessageCode.E015.ToString();
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

        private Response BuildOtpVerificationResponse(int userId, string username, string normalizedMobile)
        {
            Response verificationResponse = new Response
            {
                Id = userId,
                Message = "OTP verified successfully.",
                DataModel = new
                {
                    Username = username,
                    MobileNumber = normalizedMobile,
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

        private User? FindUserByMobile(string normalizedMobile)
        {
            return _userSet.FirstOrDefault(existingUser => existingUser.MobileNumber == normalizedMobile);
        }

        private Response ValidateAndConsumeOtp(string mobileNumber, string otpCode)
        {
            OtpVerification? otpRecord = _otpService.GetLatestOtp(mobileNumber);
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
            string normalizedMobile = NormalizeMobile(request.MobileNumber);
            User newUser = new User
            {
                Username = normalizedUsername,
                MobileNumber = normalizedMobile,
                Password = request.Password.Trim(),
                UserRoleId = request.UserRoleId
            };
            return newUser;
        }

        private static string NormalizeMobile(string? mobileNumber)
        {
            return mobileNumber?.Trim() ?? string.Empty;
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
