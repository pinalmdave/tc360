using System;
using System.Collections.Generic;

namespace TechScreen.DBEntities
{
    public partial class Transaction
    {
        public int TransactionId { get; set; }
        public int? ScreeningId { get; set; }
        public int? AmountBilled { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }

        public virtual Screening Screening { get; set; }
    }
}
