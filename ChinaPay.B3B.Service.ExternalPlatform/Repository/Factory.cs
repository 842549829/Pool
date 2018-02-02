using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.ExternalPlatform.Repository
{
   static class Factory
    {
       public static ISettingReposity CreateSettingReposity()
       {
           return new Service.ExternalPlatform.Repository.SqlServer.SettingReposity(ConnectionManager.B3BConnectionString);
       }
    }
}
