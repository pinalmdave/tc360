using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechScreen.Models
{
    public class CustomScreeningQuestionsModel
    {
        public int Id { get; set; }
        public int? ScreeningId { get; set; }
        public int? TechStackId { get; set; }
        public int? TechId { get; set; }
        public string QuestionText { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
