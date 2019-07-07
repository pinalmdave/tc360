using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechScreen.ViewModels
{
    public class EmailViewModel
    {
        public string FromEmail { get; set; }
        public string Subject { get; set; }

        public List<EmailRecipient> lstEmailRecipient { get; set; }

        public string EmailTemplate { get; set; }
    }

    public class EmailRecipient
    {
        public string RecipientName { get; set; }
        public string RecipientEmail { get; set; }

        public string SignInCode { get; set; }
        public string HiringCompanyName { get; set; }

    }
}
