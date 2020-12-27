using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using gRPCCommon;
using gRPCCommon.Db;
using gRPCCommon.Entity;
using gRPCCommonss;
using gRPCServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace gRPCServer
{
    public class ExecuterService : Transporter.TransporterBase
    {
        
        private readonly ILogger<ExecuterService> _logger;
        private readonly PostgreSqlDbContext _context;
        private IOptions<List<PipeLineDefinition>> _pipes;

        public ExecuterService(ILogger<ExecuterService> logger, PostgreSqlDbContext context, IOptions<List<PipeLineDefinition>> pipes)
        {
            _logger = logger;
            _context = context;
            _pipes = pipes;
            
        }


        public override Task<Response> Execute(Request request, ServerCallContext context)
        {
            var dispatcherData = ToDispatcherData(request);
            var dispatcherResponse = Dispatcher.Dispatch(dispatcherData);
            return Task.FromResult(ToResponseData(dispatcherResponse));
        }

        DispatcherData ToDispatcherData(Request request)
        {
            MessageEntity message = new MessageEntity()
            {
                InOrOut = "I",
                IsActive = true,
                Message = request.Message,
                CreatedTime = DateTime.Now
            };
            _context.Add(message);

            return new DispatcherData(request.Message, request.AssemblyName, request.ClassName);
        }

        Response ToResponseData(ResponseBase responseBase)
        {
            MessageEntity message = new MessageEntity()
            {
                InOrOut = "O",
                IsActive = true,
                Message = JsonConvert.SerializeObject(responseBase),
                CreatedTime = DateTime.Now
            };
            _context.Add(message);
            _context.SaveChangesAsync();

            return new Response()
            {
                Message = responseBase.Message,
                ErrorMessage = responseBase.ErrorMessage,
                IsSuccess = responseBase.IsSuccess
            };
        }

    }
}