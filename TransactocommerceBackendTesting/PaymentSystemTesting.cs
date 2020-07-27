using System;
using System.Collections.Generic;
using Transactocommerce.Models;
using Transactocommerce.Services;
using Transactocommerce.Services.Interfaces;
using Xunit;

namespace TransactocommerceBackendTesting
{
    public class PaymentSystemTesting
    {
        private static Random _random = new Random();

        private IPaymentSystem _paymentSystem;

        // Testing data
        private static readonly List<string> _productIds = new List<string>();

        public PaymentSystemTesting ()
        {
            _random = new Random();
            _paymentSystem = new PaymentSystem();

            SetInitialProducts();
        }

        private void SetInitialProducts()
        {
            _productIds.Add("dkgfhghg");
            _productIds.Add("uuyiyiy");
        }

        [Theory]
        [InlineData(new Order
        {
            Address = "Lachlan St",
            Email = "rami@ramihikmat.net",
            Name = "Rami",
            OrderNo = 30,
            ProductsIds = _productIds
        })]
        public void PaymentWorkflow(Order _order)
        {
            _paymentSystem.StartPayment(_order);
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                new Order
                {

                },
                new Order 
                {
                    Address = "Lachlan St",
                    Email = "rami@ramihikmat.net",
                    Name = "Rami",
                    OrderNo = 30,
                    ProductsIds = _productIds
                }
            };
        }
    }
}
