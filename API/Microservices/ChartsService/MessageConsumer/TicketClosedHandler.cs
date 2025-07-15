using Azure.Messaging.ServiceBus;
using BusinessLogicLayer.DTO.Ticket.Request;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace MessageConsumer;

public class TicketClosedHandler
{
    private readonly ILogger<TicketClosedHandler> _logger;
    private readonly ITicketProcessorService _ticketProcessorService;

    public TicketClosedHandler(ILogger<TicketClosedHandler> logger, ITicketProcessorService ticketProcessorService)
    {
        _logger = logger;
        _ticketProcessorService = ticketProcessorService;
    }

    [Function(nameof(TicketClosedHandler))]
    public async Task Run(
        [ServiceBusTrigger("workflow", "ticketclosed-subscription", Connection = "AzureConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        var json = message.Body.ToString();
        var dto = JsonSerializer.Deserialize<TicketCloseDTO>(json);

        await _ticketProcessorService.CloseTicket(dto);

        _logger.LogInformation($"Ticket closed: {dto.Id}");

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}