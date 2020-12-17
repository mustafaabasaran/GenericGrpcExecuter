using System;
using System.IO;
using System.Reflection;
using gRPCServer.Models;

namespace gRPCServer.Helper
{
    public static class ReflectionHelper
    {
        public static string GetServerSideClass(Type type)
        {
            return type.Name.Replace("Request", "");
        }
        
        
        public static AssemblyData GetServerAssemblyData(Type type)
        {
            var data = new AssemblyData
            {
                ClassName = type.Namespace.Replace("Model", "Action") + "." + GetServerSideClass(type),
                DllName = type.Namespace.Replace("Model", "Action") + ".dll"
            };
            data.Assembly =  Assembly.LoadFrom(Path.Combine(System.AppContext.BaseDirectory, data.DllName));
            data.Type = data.Assembly.GetType(data.ClassName);
            return data;
        }
        
    }
}