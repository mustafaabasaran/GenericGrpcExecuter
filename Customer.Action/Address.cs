using System;
using Customer.Model;
using gRPCCommon;

namespace Customer.Action
{
    public class Address
    {
        
        public ResponseBase Doit(AddressRequest request)
        {
            ResponseBase returnVal = new ResponseBase()
            {
                Message = $"{request.CustomerId} idli müşteri adresi {request.ClearAdress}",
                ErrorMessage = string.Empty,
                IsSuccess = true
            };
            
            return returnVal;
        }
    }
}