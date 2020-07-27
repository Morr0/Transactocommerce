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
                QueueUrl = "https://sqs.ap-southeast-2.amazonaws.com/472971161478/transactocommerce",
                MessageBody = JsonSerializer.Serialize(order)
            };
            SendMessageResponse response = await _sqsClient.SendMessageAsync(sendMessageRequest);

            // Resend the message if was not successful
            if (response.HttpStatusCode != HttpStatusCode.OK)
                StartPayment(order);
        }

        // To be notified by the payment handler (Stripe) in the form of webhook
        // If the transaction ID is not the same then this payment will fail
        // If correct will carry on
        public void CompletePayment(string transactionId, out Order order)
        {
            order = new Order();
        }
    }
}
