using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Transactocommerce.Models;
using Transactocommerce.Services.Interfaces;

namespace Transactocommerce.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private IPaymentSystem _system;
        public PaymentController(IPaymentSystem system)
        {
            _system = system;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartPayment([FromHeader] string transaction_id, [FromHeader] string name,
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
                TransactionId = transaction_id,
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

            // Add to payment manager waiting for the payment processor's webhook
            _system.StartPayment(_order);
            return Ok();
        }
    }
}
