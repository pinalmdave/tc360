using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechScreen.Models
{
    public class ClientModel
    {

        public int ClientId { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
