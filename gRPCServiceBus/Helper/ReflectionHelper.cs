using System;
using System.IO;
using System.Reflection;
using gRPCServiceBus.Data;

namespace gRPCServiceBus.Helper
{
    public static class ReflectionHelper
    {
        public static string GetServerSideClass(Type type)
        {
            return type.Name.Replace("Request", "");
        }
        
        
        public static AssemblyData GetServerAssemblyData(Type type, string applicationDirectory)
        {
            var data = new AssemblyData
            {
                ClassName = type.Namespace.Replace("Model", "Action") + "." + GetServerSideClass(type),
                DllName = type.Namespace.Replace("Model", "Action") + ".dll"
            };

            if (string.IsNullOrEmpty(applicationDirectory))
                applicationDirectory = System.AppContext.BaseDirectory;
            
            data.Assembly =  Assembly.LoadFrom(Path.Combine(applicationDirectory, data.DllName));
            data.Type = data.Assembly.GetType(data.ClassName);
            return data;
        }
        
    }
}