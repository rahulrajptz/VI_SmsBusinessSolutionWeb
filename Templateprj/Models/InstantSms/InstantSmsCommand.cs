using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Templateprj.Models.InstantSms
{
    public class InstantSmsCommand
    {
        public int SmsTypeId { get; set; }

        public int SenderId { get; set; }

        public int TemplateId { get; set; }

        public List<MessageContent> Messages { get; set; }

        public List<Variable> Variables { get; set; }
        public int reportStatus { get; set; }

    }
}