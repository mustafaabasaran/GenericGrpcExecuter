using System;
using Customer.Model;

namespace Customer.Action
{
    public class Address
    {
        
        public string Doit(AddressRequest request)
        {
            return $"{request.CustomerId} idli müşteri adresi {request.ClearAdress}";
        }
    }
}