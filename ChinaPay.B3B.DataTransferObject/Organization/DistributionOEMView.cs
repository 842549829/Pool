using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Organization
{
   public class DistributionOEMView
    {
       public string UserNo
       {
           get;
           set;
       }
       public string AbbreivateName
       {
           get;
           set;
       }
       public Guid CompanyId
       {
           get;
           set;
       }
    }
}
