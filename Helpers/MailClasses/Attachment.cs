using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.Helpers.MailClasses
{
    public class Attachment
    {
        public string FileName { get; set; }
        public string Content { get; set; }
        public string ContentID { get; set; }
    }
}
