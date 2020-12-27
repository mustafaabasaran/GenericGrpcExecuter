using System;
using gRPCCommon;
using gRPCCommon.Interfaces;
using gRPCCommonss;

namespace gRPCPipeLine
{
    public class TransactionLoggerBefore : IPipeLine
    {
        

        public PipeType PipeType { get; set; }
        public string Name { get; set; }
        public ResponseBase response { get; set; }
        public ResponseBase Execute(RequestBase requestBase)
        {
            Console.WriteLine("Before log worked");
            return new ResponseBase() {IsSuccess = true};
        }
    }
}