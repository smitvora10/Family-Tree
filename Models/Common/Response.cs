using FamilyTree.Core;
using System.Data;

namespace FamilyTree.Models.Common
{
    public class Response
    {
        private string _messageCode;
        public object DataModel { get; set; }
        public DataTable Data { get; set; }

        public int? Id { get; set; } = 0;
        public bool IsError { get; set; } = false;
        public string Message { get; set; } = "";
        public string MessageCode
        {
            get
            {
                return _messageCode;
            }
            set
            {
                _messageCode = value;
                Message = Messages.MessageByMessageCode[(MessageCode)Enum.Parse(typeof(MessageCode), _messageCode)];
            }
        }

        public IList<ModelError>? ModelErrors { get; set; }

        public int? TotalRecords { get; set; }
    }

    public class ModelError
    {
        public string PropertyName { get; set; } = "";
        public string ErrorMessage { get; set; } = "";
    }
}
