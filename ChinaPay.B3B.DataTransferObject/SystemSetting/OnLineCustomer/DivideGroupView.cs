using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.SystemSetting.OnLineCustomer
{
    /// <summary>
    /// 分组信息
    /// </summary>
  public class DivideGroupView
    {
      public DivideGroupView()
          : this(Guid.NewGuid())
      {
      }
      public DivideGroupView(Guid id)
      {
          this.Id = id;
      }
      public Guid Id
      {
          get;
          set;
      }
      /// <summary>
      /// 分组名称
      /// </summary>
      public string Name
      {
          get;
          set;
      }
      /// <summary>
      /// 分组描述
      /// </summary>
      public string Description
      {
          get;
          set;
      }
      /// <summary>
      /// 分组排序
      /// </summary>
      public int SortLevel
      {
          get;
          set;
      }
    }
}
