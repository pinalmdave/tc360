using System;
using System.Collections.Generic;

namespace TechScreen.DBEntities
{
    public partial class Client
    {
        public Client()
        {
            User = new HashSet<User>();
        }

        public int ClientId { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}
