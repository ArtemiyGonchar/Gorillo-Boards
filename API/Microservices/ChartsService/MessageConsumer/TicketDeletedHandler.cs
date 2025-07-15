using Azure.Messaging.ServiceBus;
using BusinessLogicLayer.DTO.Ticket.Request;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace MessageConsumer;

public class TicketDeletedHandler
{
    private readonly ILogger<TicketDeletedHandler> _logger;
    private readonly ITicketProcessorService _ticketProcessorService;

    public TicketDeletedHandler(ILogger<TicketDeletedHandler> logger, ITicketProcessorService ticketProcessorService)
    {
        _logger = logger;
        _ticketProcessorService = ticketProcessorService;
    }

    [Function(nameof(TicketDeletedHandler))]
    public async Task Run(
        [ServiceBusTrigger("workflow","ticketdeleted-subscription", Connection = "AzureConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        var json = message.Body.ToString();
        var dto = JsonSerializer.Deserialize<TicketDeleteDTO>(json);

        var isDeleted = await _ticketProcessorService.DeleteTicket(dto);
        _logger.LogInformation($"Ticket deleted ID: {dto.Id}");
        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}