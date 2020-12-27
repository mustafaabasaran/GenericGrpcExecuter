using gRPCCommon;

namespace Customer.Model
{
    public class AddressRequest : RequestBase
    {
        public string CustomerId { get; set; }
        public string CityName { get; set; }
        public string ClearAdress { get; set; }
    }
}