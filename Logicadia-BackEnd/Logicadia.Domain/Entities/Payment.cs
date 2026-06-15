using System;

namespace Logicadia.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SubscriptionId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EGP";
        public string Status { get; set; } = null!;         // paid | failed | refunded | pending
        public string Provider { get; set; } = null!;       // stripe | paymob | fawry
        public string? ProviderTxId { get; set; }
        public string? PaymentMethod { get; set; }          // card | wallet | cash
        public DateTime? PaidAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual Subscription Subscription { get; set; } = null!;
    }
}
