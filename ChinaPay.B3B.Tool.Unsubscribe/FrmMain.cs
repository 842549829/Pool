using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Configuration;
using System.Threading;
using ChinaPay.XAPI.Service.Pid.Domain;
using ChinaPay.XAPI.Service.Pid;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service;
using log4net;
using CommandService = ChinaPay.B3B.Service.Command.CommandService;

namespace ChinaPay.B3B.Tool.Unsubscribe
{
    public partial class FrmMain : Form
    {
        private static readonly ILog exceptionLog = LogManager.GetLogger("Exception.Logging");
        private static readonly ILog runtimeLog = LogManager.GetLogger("Runtime.Logging");

        public FrmMain()
        {
            InitializeComponent();
        }

        private void tsmiToolOption_Click(object sender, EventArgs e)
        {
            FrmOption fo = new FrmOption();
            fo.MdiParent = this;
            fo.Show();
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsmiOpen_Click(object sender, EventArgs e)
        {
            FrmPNRQuery fpq = new FrmPNRQuery();
            fpq.MdiParent = this;
            fpq.Show();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            CheckPNR();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            Init();
        }

        /// <summary>
        /// 检查PNR信息；
        /// </summary>
        private void CheckPNR()
        {
            // 读取配置文件中的查询时间范围；
            decimal range = decimal.Parse(ConfigurationManager.AppSettings["Range"]);

            // 这里查询到所有状态为1的，
            IEnumerable<PNRHistory> histories = HistoryService.QueryBooking(DateTime.Now.AddHours(-(double)range));

            runtimeLog.Debug("Start Process.");
            foreach (var item in histories)
            {
                runtimeLog.InfoFormat("编码：{0}, 状态：{1}", item.PNRCode, item.Status == 1 ? "未取消" : "已取消");

                // 判断编码是否需要取消；
                if (item.Status == 1)
                {
                    bool needCancel = OrderProcessService.RequireCancelPNR(item.PNRCode, item.GenerateTime.AddMinutes(-5));
                    if (needCancel)
                    {
                        var isSucceeded =CommandService.CancelPNR(new PNRPair(item.PNRCode, null));
                        if (isSucceeded)
                        {
                            item.Status = 0;
                            HistoryService.Update(item);
                            runtimeLog.InfoFormat("编码{0}已被成功取消。", item.PNRCode);
                        }
                        else
                        {
                            runtimeLog.InfoFormat("编码{0}未被成功取消。", item.PNRCode);
                            exceptionLog.ErrorFormat("编码{0}未被成功取消。", item.PNRCode);
                        }
                    }
                    else
                    {
                        runtimeLog.InfoFormat("编码{0}不需要被取消。", item.PNRCode);
                    }
                }
                else
                {
                    runtimeLog.InfoFormat("编码{0}已处于取消状态。", item.PNRCode);
                }

                runtimeLog.Info("");
                Thread.Sleep(500);
            }

            runtimeLog.Debug("End Process.");
        }

        /// <summary>
        /// 根据配置文件初始化计时器；
        /// </summary>
        public void Init()
        {
            // 读取配置；
            decimal interval = decimal.Parse(ConfigurationManager.AppSettings["Interval"]);
            bool isAutoUnsubscribe = ConfigurationManager.AppSettings["IsAutoUnsubscribe"].ToString() == "True";

            this.timer.Interval = 1000 * 60 * (int)interval;
            this.timer.Enabled = true;

            if (isAutoUnsubscribe)
            {
                this.timer.Start();
            }
            else
            {
                this.timer.Stop();
            }
        }
    }
}
