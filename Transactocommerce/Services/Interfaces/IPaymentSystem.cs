﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transactocommerce.Models;

namespace Transactocommerce.Services.Interfaces
{
    public interface IPaymentSystem
    {
        void StartPayment(Order order);
        Task<Order> CompletePayment(string transactionId);
    }
}
