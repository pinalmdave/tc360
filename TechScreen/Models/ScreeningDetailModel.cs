using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechScreen.Models
{
    public class ScreeningDetailModel
    {
        public int Id { get; set; }
        public int? ScreeningId { get; set; }
        public int? ReviewerId { get; set; }
        public int? TechStackId { get; set; }
        public int? OverallScore { get; set; }
        public int? CommunicationSkills { get; set; }
        public int? Attitude { get; set; }
        public int? ConfidenceLevel { get; set; }
        public int? FacialGesture { get; set; }
        public string VideoUrl { get; set; }
        public int? TechId { get; set; }
        public int? Score { get; set; }
        public string ReviewerComments { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
