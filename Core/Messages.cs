namespace FamilyTree.Core
{
    public class Messages
    {
        public static Dictionary<MessageCode, string> MessageByMessageCode = new Dictionary<MessageCode, string>()
        {
            {MessageCode.E001,"Erro: No Records Found"},
            {MessageCode.E002,"Erro: Duplicate {~handler~} entry"},
            {MessageCode.E003,"Erro: No Request Exists for the corresponding Id."},
            {MessageCode.E004,"Erro: Request already Approved"},
            {MessageCode.E005,"Erro: Request already Rejected"},
            {MessageCode.E006,"Erro: Invalid Login Credentials"},
            {MessageCode.E007,"Erro: Mobile number already registered."},
            {MessageCode.E008,"Erro: OTP has expired."},
            {MessageCode.E009,"Erro: Invalid OTP provided."},
            {MessageCode.E010,"Erro: User not found for the provided mobile number."},
            {MessageCode.E011,"Erro: Username already registered."},
            {MessageCode.E013,"Erro: User details are required."},
            {MessageCode.E014,"Erro: Unable to complete user registration."},
            {MessageCode.E015,"Erro: Unable to dispatch OTP for the provided mobile number."},
            {MessageCode.E016,"Erro: Registration request payload is missing."},
            {MessageCode.E017,"Erro: OTP verification request payload is missing."},
            {MessageCode.E018,"Erro: OTP has already been used."},
        };
    }

    public enum MessageCode
    {
        /// <summary>
        /// No Records Found
        /// </summary>
        E001,

        /// <summary>
        /// Duplicate {~handler~} entry
        /// </summary>
        E002,

        /// <summary>
        /// No Request Exists for the corresponding Id.
        /// </summary>
        E003,

        /// <summary>
        /// Request already Approved
        /// </summary>
        E004,  
        
        /// <summary>
        /// Request already Rejected
        /// </summary>
        E005,

        /// <summary>
        /// Invalid Login Credentials
        /// </summary>
        E006,

        /// <summary>
        /// Mobile number already registered.
        /// </summary>
        E007,

        /// <summary>
        /// OTP has expired.
        /// </summary>
        E008,

        /// <summary>
        /// Invalid OTP provided.
        /// </summary>
        E009,

        /// <summary>
        /// User not found for the provided mobile number.
        /// </summary>
        E010,

        /// <summary>
        /// Username already registered.
        /// </summary>
        E011,

        /// <summary>
        /// User details are required.
        /// </summary>
        E013,

        /// <summary>
        /// Unable to complete user registration.
        /// </summary>
        E014,

        /// <summary>
        /// Unable to dispatch OTP for the provided mobile number.
        /// </summary>
        E015,

        /// <summary>
        /// Registration request payload is missing.
        /// </summary>
        E016,

        /// <summary>
        /// OTP verification request payload is missing.
        /// </summary>
        E017,

        /// <summary>
        /// OTP has already been used.
        /// </summary>
        E018,
    }
}
