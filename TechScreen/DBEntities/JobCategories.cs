using System;
using System.Collections.Generic;

namespace TechScreen.DBEntities
{
    public partial class JobCategories
    {
        public JobCategories()
        {
            Screening = new HashSet<Screening>();
        }

        public int JobCatId { get; set; }
        public string JobCatDesc { get; set; }

        public virtual ICollection<Screening> Screening { get; set; }
    }
}
