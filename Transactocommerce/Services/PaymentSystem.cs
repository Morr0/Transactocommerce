using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transactocommerce.Models;
using Amazon.SQS;
using Amazon.SQS.Model;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net;
using Transactocommerce.Services.Interfaces;

namespace Transactocommerce.Services
{
    public class PaymentSystem : IPaymentSystem
    {
        private static string queueURL = "https://sqs.ap-southeast-2.amazonaws.com/472971161478/transactocommerce";
        private AmazonSQSClient _sqsClient;
        public PaymentSystem()
        {
            _sqsClient = new AmazonSQSClient();
        }

        // Starts a payment that will be held in the AWS queue for 30 minutes or will be ignored
        // 
        public async void StartPayment(Order order)
        {
            SendMessageRequest sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = queueURL,
                MessageBody = JsonSerializer.Serialize(order),
                MessageAttributes = new Dictionary<string, MessageAttributeValue>()
                {
                    {
                        "id",
                        new MessageAttributeValue
                        {
                            DataType = "String",
                            StringValue = order.Id
                        }
                    }
                }
            };
            SendMessageResponse response = await _sqsClient.SendMessageAsync(sendMessageRequest);

            // Resend the message if was not successful
            if (response.HttpStatusCode != HttpStatusCode.OK)
                StartPayment(order);
        }

        // To be notified by the payment handler (e.g. Stripe Checkout) in the form of webhook
        // If the transaction ID is not the same then this payment will fail
        // If correct will carry on
        public async Task<Order> CompletePayment(string transactionId)
        {
            ReceiveMessageRequest receiveMessageRequest = new ReceiveMessageRequest
            {
                MaxNumberOfMessages = 1,
                QueueUrl = queueURL,
                MessageAttributeNames = new List<string> { "id" },
            };
            ReceiveMessageResponse response = await _sqsClient.ReceiveMessageAsync(receiveMessageRequest);

            //
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                // Even though requesting one item, just to reduce unnecessary checks
                foreach (var message in response.Messages)
                {
                    if (!message.MessageAttributes.ContainsKey("id"))
                        return null;

                    string id = message.MessageAttributes["id"].StringValue;
                    if (id == transactionId)
                    {
                        Order order = JsonSerializer.Deserialize<Order>(message.Body);

                        // Try deleting the message from the queue after successful consumption
                        try
                        {
                            await _sqsClient.DeleteMessageAsync(queueURL, message.ReceiptHandle);
                        } catch (AmazonSQSException e)
                        {
                            Console.WriteLine(e);
                        }

                        return order;
                    }
                } 
            }

            return null;
        }
    }
}
