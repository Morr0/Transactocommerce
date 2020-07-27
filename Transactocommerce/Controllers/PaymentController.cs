using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Transactocommerce.Models;
using Transactocommerce.Services.Interfaces;
using Transactocommerce.Utilities;

namespace Transactocommerce.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private IPaymentSystem _system;
        private DataContext _context;
        public PaymentController(IPaymentSystem system, DataContext context)
        {
            _system = system;
            _context = context;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartPayment([FromHeader] string id, [FromHeader] string name,
            [FromHeader] string email, [FromHeader] string address, [FromBody] JsonElement data)
        {
            // data should have an array of orders
            if (data.ValueKind != JsonValueKind.Array)
                return BadRequest();

            JsonElement.ArrayEnumerator enumerator = data.EnumerateArray();
            if (enumerator.Count() < 1)
                return BadRequest();

            Order _order = new Order 
            { 
                TransactionId = id,
                Name = name,
                Email = email,
                Address = address
            };

            // Loop through each order
            foreach (JsonElement product in enumerator)
            {
                if (product.ValueKind != JsonValueKind.String)
                    return BadRequest();

                _order.ProductsIds.Add(product.GetString());
            }

            // Add to DB
            await _context.Order.AddAsync(_order);
            await _context.SaveChangesAsync();

            // Add to payment manager waiting for the payment processor's webhook
            _system.StartPayment(_order);
            // Returns id of order so that it can be used to track the order when a webhook is recieved
            return Ok(_order.Id);
        }

        [HttpPost("webhook")]
        // This is sent from the front end as a response from the front end's payment handler
        public async Task<IActionResult> Webhook([FromHeader] string id)
        {
            Order _order = await _system.CompletePayment(id);
            if (_order == null)
                return BadRequest();
            Console.WriteLine("Here");

            Order order = await _context.Order.FirstOrDefaultAsync(p => p.Id == id);
            if (order == null)
                return NotFound();

            // Set values to mark a complete order
            order.Complete = true;
            order.Failed = false;
            order.OrderConfirmTime = DateTime.UtcNow;

            // Aggregate order ids into a map holding count of each time to order
            Dictionary<string, int> productsIdsAlongWithAmount = new Dictionary<string, int>();
            foreach (string orderId in order.ProductsIds)
            {
                if (!productsIdsAlongWithAmount.ContainsKey(orderId))
                    productsIdsAlongWithAmount.Add(orderId, 1);
                else
                    productsIdsAlongWithAmount[orderId] += 1;
            }

            // Buy each item
            foreach (KeyValuePair<string, int> entry in productsIdsAlongWithAmount)
            {
                Product product = await _context.Product.FirstOrDefaultAsync(p => p.Id == entry.Key);
                if (product == null)
                    continue;

                product.Stock -= entry.Value;
            }

            await _context.SaveChangesAsync();

            return Ok(order);
        }
    }
}
