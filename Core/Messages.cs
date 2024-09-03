namespace FamilyTree.Core
{
    public class Messages
    {
        public static Dictionary<MessageCode, string> MessageByMessageCode = new Dictionary<MessageCode, string>()
        {
            {MessageCode.E001,"No Records Found"},
            {MessageCode.E002,"Duplicate {~handler~} entry"},
            {MessageCode.E003,"No Request Exists for the corresponding Id."},
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
    }
}
