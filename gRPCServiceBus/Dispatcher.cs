using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using gRPCCommon;
using gRPCCommon.Interfaces;
using gRPCCommonss;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


namespace gRPCServiceBus
{
    public static class Dispatcher
    {
        private static IConfiguration _configuration;
        
        static Dispatcher()
        {
            if (_configuration == null)
                _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }
        
        public static ResponseBase Dispatch(DispatcherData data)
        {
            ResponseBase returnData = null;
            try
            {
                if (data == null || string.IsNullOrEmpty(data.Message))
                    return returnData = new ResponseBase(null, false, "Request can not be empty!");

                data.Request = GetRequestFromMessage(data);

                var response = PipeManager.Run(data);
                
                returnData = response.Response;
            }
            catch (Exception exception)
            {
                returnData = new ResponseBase(null, false, exception.ToString());
            }

            return returnData;
        }

        private static RequestBase GetRequestFromMessage(DispatcherData data)
        {
            var assembly = Assembly.Load(data.AssemblyName);

            Type requestType = assembly.GetTypes().FirstOrDefault(x => x.FullName == data.ClassName);
            var requestObj = JsonConvert.DeserializeObject(data.Message, requestType);

            return (RequestBase) requestObj;
        }
    }
}