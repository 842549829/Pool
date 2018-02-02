using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Organization
{
  public class IncomeGroupListView
    {
      public Guid Id { get; set; }
      public Guid Company { get; set; }
      public string Name { get; set; }
      public string Description { get; set; }
      public DateTime CreateTime { get; set; }
      public int UserCount { get; set; }
    }
}
