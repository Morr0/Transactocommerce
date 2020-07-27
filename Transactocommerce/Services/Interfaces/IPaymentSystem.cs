using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transactocommerce.Models;

namespace Transactocommerce.Services.Interfaces
{
    interface IPaymentSystem
    {
        void StartPayment(Order order);
        void CompletePayment(string transactionId, out Order order);
    }
}
