﻿namespace Resturant.Services.OrderApi.Messages
{
    public class UpdatePaymentResult
    {
        public int OrderId { get; set; }
        public bool Status { get; set; }
        public string? Email { get; set; }
    }
}
