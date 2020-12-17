using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Grpc.Core;
using gRPCServer.Factory;
using gRPCServer.Helper;
using gRPCServer.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace gRPCServer
{
    public class ExecuterService : Transporter.TransporterBase
    {
        
        private readonly ILogger<ExecuterService> _logger;
        public ExecuterService(ILogger<ExecuterService> logger, IServiceCollection collection)
        {
            _logger = logger;
        }


        public override Task<ResponseBase> Execute(RequestBase request, ServerCallContext context)
        {
            var response = Execute(request);
            
            return Task.FromResult(new ResponseBase
            {
                Message = response.Message
            });
        }

        public ResponseBase Execute(RequestBase requestBase)
        {
            var assemly = Assembly.Load(requestBase.AssemblyName);
            
            Type requestType = assemly.GetTypes().FirstOrDefault(x => x.FullName == requestBase.ClassName);
            var requestObj = JsonConvert.DeserializeObject(requestBase.Message, requestType);

            AssemblyData assemblyData = ReflectionHelper.GetServerAssemblyData(requestType);
            var finalResponse = ExecuteFinal(assemblyData.Type, requestBase.MethodName, new object[1] {requestObj});

            return new ResponseBase() {Message = finalResponse.ToString()};
        }

        public object ExecuteFinal(Type type, string methodName, object[] args)
        {
            using var scope = ServiceActivator.GetScope();
            var classObject = Activator.CreateInstance(type);
            var response = type.InvokeMember(methodName, BindingFlags.Default | BindingFlags.InvokeMethod, null, classObject, args );
            return response;
        }

       
    }
}