using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Data;

namespace ChinaPay.B3B.Service.Foundation.Domain
{
    /// <summary>
    /// 值机
    /// </summary>
    public class Check_In
    {
        public Guid Id { get; set; }
        public string AirlineName { get; set; }
        public string AirlineCode { get; set; }
        public string Remark { get; set; }
        public string OperatingHref { get; set; }
        public string Opertor{ get; set; }
        public DateTime Time { get; set; }
    }
    public class Check_InCellection : RepositoryCache<Guid, Check_In>
    {
        private static Check_InCellection _instance;
        private static object _locker = new object();

        public static Check_InCellection Instance 
        {
            get 
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new Check_InCellection();
                        }
                    }
                }
                return _instance;
            }
        }
        private Check_InCellection()
            : base(Repository.Factory.CreateCheck_InRepository(),5 * 60 * 1000)
        { 
            
        }
    }
}

