using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Grpc.Core;
using gRPCServer.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace gRPCServer
{
    public class ExecuterService : Transporter.TransporterBase
    {
        
        private readonly ILogger<ExecuterService> _logger;
        public ExecuterService(ILogger<ExecuterService> logger)
        {
            _logger = logger;
        }


        public override Task<ResponseBase> Execute(RequestBase request, ServerCallContext context)
        {
            var response = DoSomething(request);
            
            return Task.FromResult(new ResponseBase
            {
                Message = response.Message
            });
        }

        public ResponseBase DoSomething(RequestBase requestBase)
        {
            var assemly = Assembly.Load(requestBase.AssemblyName);
            Type requestType = assemly.GetTypes().FirstOrDefault(x => x.FullName == requestBase.ClassName);
            var requestObj = JsonConvert.DeserializeObject(requestBase.Message, requestType);

            AssemblyData assemblyData = GetServerAssemblyData(requestType);
            var finalResponse = ExecuteFinal(assemblyData.Type, requestBase.MethodName, new object[1] {requestObj});

            return new ResponseBase() {Message = finalResponse.ToString()};
        }

        public string GetServerSideClass(Type type)
        {
            return type.Name.Replace("Request", "");
        }

        public object ExecuteFinal(Type type, string methodName, object[] args)
        {
            var classObject = Activator.CreateInstance(type);
            var response = type.InvokeMember(methodName, BindingFlags.Default | BindingFlags.InvokeMethod, null, classObject, args );
            return response;
        }

        public AssemblyData GetServerAssemblyData(Type type)
        {
            var data = new AssemblyData {ClassName = type.Namespace.Replace("Model", "Action") +"."+ GetServerSideClass(type) };
            data.DllName = type.Namespace.Replace("Model", "Action") + ".dll";
            data.Assembly =  Assembly.LoadFrom(Path.Combine(System.AppContext.BaseDirectory, data.DllName));
            data.Type = data.Assembly.GetType(data.ClassName);
            return data;
        }
    }
}