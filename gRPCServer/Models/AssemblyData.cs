using System;
using System.Reflection;

namespace gRPCServer.Models
{
    public class AssemblyData
    {
        public string ClassName { get; set; }
        public string DllName { get; set; }
        public Assembly Assembly { get; set; }

        public Type Type { get; set; }
    }
}