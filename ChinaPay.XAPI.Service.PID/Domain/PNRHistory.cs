using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.XAPI.Service.Pid.Domain
{
    public class PNRHistory
    {
        public PNRHistory(int threadId, DateTime generateTime, string pnrCode, string officeNo, int status)
        {
            this.ThreadId = threadId;
            this.GenerateTime = generateTime;
            this.PNRCode = pnrCode;
            this.OfficeNo = officeNo;
            this.Status = status;
        }

        public int ThreadId { get; set; }
        public DateTime GenerateTime { get; set; }
        public string PNRCode { get; set; }
        public string OfficeNo { get; set; }
        public int Status { get; set; }
    }
}
