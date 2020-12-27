
namespace gRPCCommon
{
    public class ResponseBase
    {
        public ResponseBase(string message, bool ısSuccess, string errorMessage)
        {
            Message = message;
            IsSuccess = ısSuccess;
            ErrorMessage = errorMessage;
        }

        public ResponseBase()
        {
            
        }

        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}