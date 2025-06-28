using Azure.Messaging.ServiceBus;
using BusinessLogicLayer.DTO.Board;
using BusinessLogicLayer.Services.Classes;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;

namespace MessageConsumer
{
    public class BoardCreatedHandler
    {
        private readonly ILogger<BoardCreatedHandler> _logger;
        private readonly IBoardManagementService _boardManagementService;
        public BoardCreatedHandler(ILogger<BoardCreatedHandler> logger, IBoardManagementService boardManagmentService)
        {
            _logger = logger;
            _boardManagementService = boardManagmentService;
        }

        [Function(nameof(BoardCreatedHandler))]
        public async Task Run([ServiceBusTrigger("identity-events", "boardcreated-subscription", Connection = "AzureConnection")] ServiceBusReceivedMessage message)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

            var json = message.Body.ToString();
            var dto = JsonSerializer.Deserialize<BoardCreatedDTO>(json);

            _logger.LogInformation($"Board created: {dto.Title}");

            await _boardManagementService.BoardCreate(dto);
        }
    }
}
