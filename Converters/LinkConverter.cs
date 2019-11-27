using CM.Models;
using CM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.Converters
{
    public class LinkConverter
    {
        public Link ViewModelToLink(LinkDetailViewModel LDVM)
        {
            Link link = new Link()
            {
                LinkID = LDVM.LinkID,
                DoctorID = LDVM.DoctorID,
                PatientID = LDVM.PatientID
            };
            return link;
        }

        public LinkDetailViewModel ViewModelFromLink(Link link)
        {
            LinkDetailViewModel LDVM = new LinkDetailViewModel()
            {
                LinkID = link.LinkID,
                DoctorID = link.DoctorID,
                PatientID = link.PatientID
            };
            return LDVM;
        }
    }
}
