using Azure.Storage.Blobs;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Text.Json;

namespace TimeTrigger;

public class TicketArchiveTimeTrigger
{
    private readonly ILogger _logger;
    private const string ContainerName = "ticketsarchive";
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobContainerClient _containerClient;
    private readonly ITicketManagementService _ticketService;

    public TicketArchiveTimeTrigger(ILoggerFactory loggerFactory, BlobServiceClient blobServiceClient, ITicketManagementService ticketManagementService)
    {
        _logger = loggerFactory.CreateLogger<TicketArchiveTimeTrigger>();
        _blobServiceClient = blobServiceClient;
        _containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
        _containerClient.CreateIfNotExists();
        _ticketService = ticketManagementService;
    }

    [Function("TicketArchiveTimeTrigger")]
    public async Task Run([TimerTrigger("0 0 0 * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation("Ticket archive time trigger executed at: {executionTime}", DateTime.Now);
        var closedTickets = await _ticketService.GetClosedTickets();

        if (closedTickets != null)
        {
            string json = JsonSerializer.Serialize(closedTickets, new JsonSerializerOptions
            {
                WriteIndented = true, //norm temka
            });
            string fileName = $"tickets_{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}.json";
            BlobClient blobClient = _containerClient.GetBlobClient(fileName);

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            await blobClient.UploadAsync(stream, overwrite: true);

            var isDeleted = await _ticketService.DeleteClosedTickets();
            _logger.LogInformation($"Ticket was deleted: {isDeleted}");
        }
        if (myTimer.ScheduleStatus is not null)
        {
            _logger.LogInformation("Next timer schedule at: {nextSchedule}", myTimer.ScheduleStatus.Next);

        }
    }
}