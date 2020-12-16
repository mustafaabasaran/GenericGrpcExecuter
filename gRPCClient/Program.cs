using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Customer.Model;
using Grpc.Net.Client;
using gRPCServer;
using Newtonsoft.Json;

namespace gRPCClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Transporter.TransporterClient(channel);

            AddressRequest req = new AddressRequest() {CustomerId = "503980"};

            RequestBase requestBase = new RequestBase()
            {
                Message = JsonConvert.SerializeObject(req),
                MethodName = "Doit",
                ClassName = req.GetType().ToString(),
                AssemblyName = Assembly.GetAssembly(req.GetType()).ToString()
            };

            var response = await client.ExecuteAsync(requestBase);

            Console.WriteLine(response.Message);
        }
    }
}
