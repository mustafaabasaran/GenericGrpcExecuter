using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using gRPCCommon;
using gRPCCommonss;
using gRPCServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace gRPCServer
{
    public class ExecuterService : Transporter.TransporterBase
    {
        
        private readonly ILogger<ExecuterService> _logger;
        private IOptions<List<PipeLineDefinition>> _pipes;

        public ExecuterService(ILogger<ExecuterService> logger, IOptions<List<PipeLineDefinition>> pipes)
        {
            _logger = logger;
            _pipes = pipes;
        }

        public override Task<Response> Execute(Request request, ServerCallContext context)
        {
            var dispatcherData = ToDispatcherData(request);
            var dispatcherResponse = Dispatcher.Dispatch(dispatcherData);
            return Task.FromResult(ToResponseData(dispatcherResponse));
        }

        static DispatcherData ToDispatcherData(Request request)
        {
            return new DispatcherData(request.Message, request.AssemblyName, request.ClassName);
        }

        static Response ToResponseData(ResponseBase responseBase)
        {
            return new Response()
            {
                Message = responseBase.Message,
                ErrorMessage = responseBase.ErrorMessage,
                IsSuccess = responseBase.IsSuccess
            };
        }

    }
}