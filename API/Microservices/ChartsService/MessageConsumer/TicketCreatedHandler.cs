using Azure.Messaging.ServiceBus;
using BusinessLogicLayer.DTO.Ticket.Request;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace MessageConsumer;

public class TicketCreatedHandler
{
    private readonly ILogger<TicketCreatedHandler> _logger;
    private readonly ITicketProcessorService _ticketProcessorService;
    public TicketCreatedHandler(ILogger<TicketCreatedHandler> logger, ITicketProcessorService ticketProcessorService)
    {
        _logger = logger;
        _ticketProcessorService = ticketProcessorService;
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

        var json = message.Body.ToString();
        var dto = JsonSerializer.Deserialize<TicketCreateDTO>(json);

        await _ticketProcessorService.CreateTicket(dto);

        _logger.LogInformation($"Ticket created: {dto.Id}");

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}