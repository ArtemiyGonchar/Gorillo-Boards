using Azure.Messaging.ServiceBus;
using BusinessLogicLayer.DTO.Board;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;

namespace MessageConsumer
{
    public class BoardDeletedHandler
    {
        private readonly ILogger<BoardDeletedHandler> _logger;
        private readonly IBoardManagementService _boardManagementService;
        public BoardDeletedHandler(ILogger<BoardDeletedHandler> logger, IBoardManagementService boardManagmentService)
        {
            _logger = logger;
            _boardManagementService = boardManagmentService;
        }

        [Function(nameof(BoardDeletedHandler))]
        public async Task RunAsync([ServiceBusTrigger("identity-events", "boarddeleted-subscription", Connection = "AzureConnection")] ServiceBusReceivedMessage message)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

            var json = message.Body.ToString();
            var dto = JsonSerializer.Deserialize<BoardCreatedDTO>(json);

            _logger.LogInformation($"Board created: {dto.Title}");

            await _boardManagementService.BoardDelete(dto.Title);
        }
    }
}
