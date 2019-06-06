using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechScreen.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public int? ClientId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public string Designation { get; set; }
        public string IsAdmin { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
