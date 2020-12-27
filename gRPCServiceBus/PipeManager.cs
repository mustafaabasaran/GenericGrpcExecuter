using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using gRPCCommon;
using gRPCCommon.Interfaces;
using gRPCCommonss;
using gRPCServiceBus.Data;
using gRPCServiceBus.Helper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace gRPCServiceBus
{
    public static class PipeManager
    {
        private static List<PipeLineDefinition> _pipes;
        private static ConcurrentDictionary<string, IPipeLine> dicPipeLines;
        private static object lockObject = new object();
        private static IConfiguration _configuration;
        
        static PipeManager()
        {
            if (_configuration == null)
                _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            
            if (dicPipeLines == null)
                LoadPipeDefinitions();
        }

        private static void LoadPipeDefinitions()
        {
            lock (lockObject)
            {
               
                _pipes = _configuration.GetSection("PipeLine").Get<List<PipeLineDefinition>>();
                dicPipeLines = new ConcurrentDictionary<string, IPipeLine>();
                foreach (var item in _pipes)
                {
                    IPipeLine pipe = CreatePipeInstance(item);
                    dicPipeLines.TryAdd(item.Name, pipe);
                }
            }
        }
        
        static IPipeLine CreatePipeInstance(PipeLineDefinition pipeDefinition)
        {
            var assemly = Assembly.Load(pipeDefinition.AssemblyName);
            var type =  assemly.GetTypes().FirstOrDefault(x => x.Name == pipeDefinition.Name);
            IPipeLine pipe = Activator.CreateInstance(type) as IPipeLine;
            pipe.Name = pipeDefinition.Name;
            pipe.PipeType = Enum.Parse<PipeType>(pipeDefinition.Type);
            return pipe;
        }

        public static DispatcherData Run(DispatcherData data)
        {
            var beforePipe = dicPipeLines.Values.Where(x => x.PipeType == PipeType.Before);
            var afterPipe = dicPipeLines.Values.Where(x => x.PipeType == PipeType.After);

            foreach (IPipeLine pipeLine in beforePipe)
            {
                ResponseBase resp = pipeLine.Execute(data.Request);
                if (resp.IsSuccess) continue;
                data.Response = resp;
                return data;
            }

            Execute(data);
           
            foreach (IPipeLine pipeLine in afterPipe)
            {
                ResponseBase resp = pipeLine.Execute(data.Request);
                if (resp.IsSuccess) continue;
                data.Response = resp;
                return data;
            }
            
            return data;
        }
        
        
        public static void Execute(DispatcherData data)
        {
            string applicationdDirectory = _configuration.GetValue<string>("ApplicationDirectory");
            var assemly = Assembly.Load(data.AssemblyName);
            Type requestType = assemly.GetTypes().FirstOrDefault(x => x.FullName == data.ClassName);
            AssemblyData assemblyData = ReflectionHelper.GetServerAssemblyData(requestType, applicationdDirectory);
            data.Response = ExecuteAction(assemblyData.Type, data.Request.MethodName, new object[1] {data.Request});
        }

        public static ResponseBase ExecuteAction(Type type, string methodName, object[] args)
        {
            var classObject = Activator.CreateInstance(type);
            var response = type.InvokeMember(methodName, BindingFlags.Default | BindingFlags.InvokeMethod, null, classObject, args ) as ResponseBase;
            return response;
        }
        
    }
}