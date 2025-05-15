using Azure.Identity;
using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService
{
    public class ServiceBusPublisher
    {
        public ServiceBusPublisher() { }

        public async Task PublishMessage(string messgae)
        {
            string fullyQualifiedNamespace = "mehrfarservicebus.servicebus.windows.net";
            string queueName = "pre-process-notifications";

            // Use DefaultAzureCredential (uses managed identity in Azure)
            var client = new ServiceBusClient(fullyQualifiedNamespace, new DefaultAzureCredential());

            var sender = client.CreateSender(queueName);

            var message = new ServiceBusMessage(messgae);

            await sender.SendMessageAsync(message);

            Console.WriteLine("Message sent with managed identity.");
        }
    }
}
