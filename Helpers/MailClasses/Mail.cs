using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CM.Helpers.MailClasses
{
    public class Mail
    {
        public Guid ID { get; set; }
        public Guid AccountID { get; set; }
        public Guid FromAddressID { get; set; }
        public string FromName { get; set; }
        public Guid? ReplyToAddressID { get; set; }
        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public MailAddress[] Bccs { get; set; }
        public MailAddress[] Ccs { get; set; }
        public string HtmlBody { get; set; }
        public string TextBody { get; set; }
        public Attachment[] Attachments { get; set; }
        public string CustomerReference { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
