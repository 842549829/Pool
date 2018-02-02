using System.Collections.Generic;
using System.Text;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Remind.Server.Command {
    class Remind {
        Net.TcpProcessor m_processor = null;
        IEnumerable<DataSource.RemindData> m_datas = null;
        Encoding m_encoding = Encoding.GetEncoding("gb2312");

        public Remind(Net.TcpProcessor processor, IEnumerable<DataSource.RemindData> remindDatas) {
            this.m_processor = processor;
            this.m_datas = remindDatas;
        }

        public string Execute() {
            if(this.m_datas != null) {
                var dataString = this.m_datas.Join("|", item=>string.Format("{0}-{1}", (int)item.Status, item.Count));
                var data = this.m_encoding.GetBytes("REMIND/" + dataString);
                this.m_processor.AsyncSend(data, null, null);
                return dataString;
            }
            return string.Empty;
        }
    }
}