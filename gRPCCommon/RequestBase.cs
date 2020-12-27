
namespace gRPCCommon
{

    public class RequestBase
    {
        public string MethodName { get; set; }
        public string RequestId { get; set; }
        public string ChannelId { get; set; }
    }
}
