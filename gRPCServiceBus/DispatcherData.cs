using System;
using gRPCCommon;

namespace gRPCServiceBus
{
    public class DispatcherData
    {
        public DispatcherData(string message, string assemblyName, string className)
        {
            Message = message;
            AssemblyName = assemblyName;
            ClassName = className;
            StartTime = DateTime.Now;
        }
        
        public DispatcherData(RequestBase requestBase, ResponseBase responseBase, string assemblyName, string className, DateTime startTime) 
        {
            Request = requestBase;
            Response = responseBase;
            AssemblyName = assemblyName;
            ClassName = className;
            StartTime = startTime;
        }

        public string Message { get; set; }
        
        public RequestBase Request { get; set; }

        public ResponseBase Response { get;  set; }

        public DateTime StartTime { get; private set; }

        public string AssemblyName { get; private set; }

        public string ClassName { get; private set; }
    }
}