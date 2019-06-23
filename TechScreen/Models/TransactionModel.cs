using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechScreen.Models
{
    public class TransactionModel
    {
        
        public int TransactionId { get; set; }
        public int? ScreeningId { get; set; }
        public int? AmountBilled { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }

        public ScreeningModel Screening { get; set; }
        
    }
}
