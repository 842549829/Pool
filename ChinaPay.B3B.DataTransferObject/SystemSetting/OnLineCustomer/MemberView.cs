using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.SystemSetting.OnLineCustomer
{
    /// <summary>
    /// 成员管理
    /// </summary>
  public class MemberView
    {
      public MemberView(Guid id)
      {
          this.Id = id;
      }
      public MemberView()
          : this(Guid.NewGuid()){
    }
      public Guid Id
      {
          get;
          set;
      }
      /// <summary>
      /// 成员说明
      /// </summary>
      public string Remark
      {
          get;
          set;
      }
      /// <summary>
      /// 排序字段
      /// </summary>
      public int SortLevel
      {
          get;
          set;
      }
      /// <summary>
      /// QQ
      /// </summary>
      public IEnumerable<string> QQ
      {
          get;
          set;
      }
    }
}
