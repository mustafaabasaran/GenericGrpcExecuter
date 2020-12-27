using System;
using gRPCCommonss;

namespace gRPCCommon.Interfaces
{
    public interface IPipeLine
    {
        public PipeType PipeType { get; set; }
        public string Name { get; set; }
        
        ResponseBase response { get; set; }

        ResponseBase Execute(RequestBase requestBase);
    }
}