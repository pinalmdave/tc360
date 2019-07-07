using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechScreen.DBEntities;
using TechScreen.Models;

namespace TechScreen.ViewModels
{
    public class AdminVM
    {
        public List<Screening> lstScreening { get; set; }
    }
}
