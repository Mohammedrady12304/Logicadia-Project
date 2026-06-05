using System;


namespace LOGICADIA.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PlanName { get; set; } = null!;
        public string Status { get; set; } = null!;         // active | cancelled | expired
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? LastPaymentId { get; set; }

        // Navigation
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual Payment? LastPayment { get; set; }
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
