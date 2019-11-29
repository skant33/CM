using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.Models
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public int AccountID { get; set; }
        public int TypeID { get; set; }
        public int TimeTillSend { get; set; }
    }
}
