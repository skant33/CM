using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.ViewModels
{
    public class NotificationDetailViewModel
    {
        public int AccountID { get; set; }
        public int TypeID { get; set; }
        public int TimeTillSend { get; set; }
    }
}
