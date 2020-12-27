namespace gRPCCommon.Entity
{
    public class MessageEntity : BaseEntity
    {
        public string InOrOut { get; set; }
        public string Message { get; set; }
    }
}