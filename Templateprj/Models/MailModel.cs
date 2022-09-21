using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Templateprj.Models
{
    public class MailModel
    {
        public string Subject { get; set; }
        public string Emailbody { get; set; }
        public string MailTo { get; set; }
        public string MailCC { get; set; }
    }
}