using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MessageConsumer;

public class TicketCreatedHandler
{
    private readonly ILogger<TicketCreatedHandler> _logger;

    public TicketCreatedHandler(ILogger<TicketCreatedHandler> logger)
    {
        _logger = logger;
    }

    [Function(nameof(TicketCreatedHandler))]
    public async Task Run(
        [ServiceBusTrigger("workflow", "ticketcreated-subscription", Connection = "AzureConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}