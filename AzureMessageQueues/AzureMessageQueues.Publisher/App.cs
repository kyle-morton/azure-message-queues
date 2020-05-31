using Azure.Core;
using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AzureMessageQueues.Publisher
{
    public class App
    {

        private IConfiguration _config;

        public App(IConfiguration config)
        {
            _config = config;
        }

        public async Task Run()
        {
            //await PublishMessage();
            await ReadMessages();
        }

        private async Task ReadMessages()
        {
            // will need to use a loop or timer to continually get new messages

            // Get the connection string from app settings
            var connectionString = _config["Azure:ConnectionString"];

            // Instantiate a QueueClient which will be used to create and manipulate the queue
            var queueClient = new QueueClient(connectionString, "video-queue");

            var newMessagesResponse = await queueClient.ReceiveMessagesAsync();
            var newMessages = newMessagesResponse.Value;

            if (newMessages.Length == 0)
            {
                Console.WriteLine("No messages...");
                return;
            }
            
            // DO SOMETHING W/ MESSAGE HERE....
            foreach(var message in newMessages)
            {
                Console.WriteLine("Message Id - Text: " + message.MessageId + " - " + message.MessageText);
                Console.ReadLine();
                await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
            }
            
        }

        private async Task PublishMessage()
        {
            // Get the connection string from app settings
            var connectionString = _config["Azure:ConnectionString"];

            // Instantiate a QueueClient which will be used to create and manipulate the queue
            var queueClient = new QueueClient(connectionString, "video-queue");

            // Send a message to the queue
            var testMessage = new Core.Models.Request
            {
                Url = "youtube.com/1231233abasd",
                ExtractAudio = true
            };
            queueClient.SendMessage(JsonSerializer.Serialize(testMessage));
        }
    }
}
