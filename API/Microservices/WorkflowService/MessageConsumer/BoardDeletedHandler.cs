using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;

namespace MessageConsumer
{
    public class BoardDeletedHandler
    {
        private readonly ILogger<BoardDeletedHandler> _logger;

        public BoardDeletedHandler(ILogger<BoardDeletedHandler> logger)
        {
            _logger = logger;
        }

        [Function(nameof(BoardDeletedHandler))]
        public void Run([ServiceBusTrigger("identity-events", "boarddeleted-subscription", Connection = "AzureConnection")] ServiceBusReceivedMessage message)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);
        }
    }
}
