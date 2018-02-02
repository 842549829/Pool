using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChinaPay.B3B.Service.Command.Domain;
using ChinaPay.XAPI.Service.Pid.Domain;
using ChinaPay.XAPI.Service.Pid;
using ChinaPay.B3B.Service.Command;
using ChinaPay.B3B.DataTransferObject.Common;
using System.Threading;
using CommandService = ChinaPay.B3B.Service.Command.CommandService;

namespace ChinaPay.B3B.Tool.Unsubscribe
{
    public partial class FrmPNRQuery : Form
    {
        public FrmPNRQuery()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnQueryAll_Click(object sender, EventArgs e)
        {
            IEnumerable<PNRHistory> result = HistoryService.QueryAll();
            this.dgvPNRHistories.DataSource = result;
        }

        private void btnCancelAll_Click(object sender, EventArgs e)
        {
            IEnumerable<PNRHistory> histories = HistoryService.QueryAll();

            foreach (var item in histories)
            {
                // 若状态为未取消；
                if (item.Status == 1)
                {
                    // 取消编码；
                    var isSucceeded = CommandService.CancelPNR(new PNRPair(item.PNRCode, null));

                    if (isSucceeded)
                    {
                        item.Status = 0;
                        HistoryService.Update(item);
                    }

                    Thread.Sleep(500);
                }
            }
        }

        private void FrmPNRQuery_Load(object sender, EventArgs e)
        {

        }
    }
}
