using Azure.Messaging.ServiceBus;
using GorilloBoards.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Classes
{
    public class AzureBusEventPublisherService : IEventPublisher
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusSender _serviceBusSender;

        public AzureBusEventPublisherService(ServiceBusClient serviceBusClient, ServiceBusSender serviceBusSender)
        {
            _serviceBusClient = serviceBusClient;
            _serviceBusSender = serviceBusSender;
        }

        public async Task Publish<TEvent>(TEvent @event) where TEvent : class
        {
            var eventName = @event.GetType().Name;
            var jsonMessage = JsonSerializer.Serialize(@event, @event.GetType());
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            var message = new ServiceBusMessage()
            {
                MessageId = Guid.NewGuid().ToString(),
                Subject = eventName,
                Body = new BinaryData(body)
            };
            message.ApplicationProperties["event"] = eventName;
            await _serviceBusSender.SendMessageAsync(message);
        }
    }
}
