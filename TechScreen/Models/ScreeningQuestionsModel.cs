using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechScreen.Models
{
    public class ScreeningQuestionsModel
    {
        public int Id { get; set; }
        public int? ScreeningId { get; set; }
        public int? QuestionId { get; set; }
        public string QuestionText { get; set; }
    }
}
