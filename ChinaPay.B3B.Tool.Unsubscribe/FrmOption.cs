using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace ChinaPay.B3B.Tool.Unsubscribe
{
    public partial class FrmOption : Form
    {
        public FrmOption()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // 获取界面设置的值；
            bool isAutoUnsubscribe = this.chkAutoUnsubscribe.Checked;
            decimal interval = this.nudInterval.Value;
            decimal range = this.nudRange.Value;
            
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            AppSettingsSection appSetting = config.AppSettings;

            appSetting.Settings["Interval"].Value = interval.ToString();
            appSetting.Settings["Range"].Value = range.ToString();
            appSetting.Settings["IsAutoUnsubscribe"].Value = isAutoUnsubscribe.ToString();

            config.Save();
            ConfigurationManager.RefreshSection("appSettings");

            // 重新加载系统参数；
            ((FrmMain)this.MdiParent).Init();

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // 退出
            this.Close();
        }

        private void FrmOption_Load(object sender, EventArgs e)
        {
            // 读取配置文件
            decimal interval = decimal.Parse(ConfigurationManager.AppSettings["Interval"]);
            decimal range = decimal.Parse(ConfigurationManager.AppSettings["Range"]);
            bool isAutoUnsubscribe = ConfigurationManager.AppSettings["IsAutoUnsubscribe"].ToString() == "True";

            // 赋值
            this.nudInterval.Value = interval;
            this.nudRange.Value = range;
            this.chkAutoUnsubscribe.Checked = isAutoUnsubscribe;
        }
    }
}
