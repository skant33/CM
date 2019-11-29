using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.Models
{
    public class Link
    {
        public int LinkID { get; set; }
        public Account Doctor = new Account();
        public Account Patient = new Account();
    }
}
