using System;
using System.Windows.Forms;

namespace ChinaPay.B3B.Remind.Client {
    partial class LogonForm : Form {
        private LogonForm() {
            InitializeComponent();
            setDefultLogonData();
            Winform.MoveHelper.SetMoveable(this, pnlTitle, pnlContent, lblUserName, lblPwd, lblFooter);
        }

        private static LogonForm m_instance = null;
        private static object m_locker = new object();
        public static LogonForm Instance {
            get {
                if(m_instance == null || m_instance.IsDisposed) {
                    lock(m_locker) {
                        if(m_instance == null || m_instance.IsDisposed) {
                            m_instance = new LogonForm();
                        }
                    }
                }
                return m_instance;
            }
        }

        private void btnLogon_Click(object sender, EventArgs e) {
            string userName = txtUserName.Text.Trim();
            string password = txtPwd.Text;
            if(userName.Length == 0) {
                MessageBox.Show("请输入用户名");
                txtUserName.Focus();
                return;
            }
            if(password.Length == 0) {
                MessageBox.Show("请输入密码");
                txtUserName.Focus();
                return;
            }
            var request = new Command.Logon(Settings.ServerHost, Settings.ServerPort, userName, password);
            var response = request.Execute();
            if(response.Success) {
                Program.LogonInfo = response.Response;
                RemindInfoListener.Instance.Start(response.Response.Connection);
                MessageBox.Show("登录成功");
                if(chbSavePassword.Checked) {
                    UserCenter.Instance.Save(userName, password);
                } else {
                    UserCenter.Instance.Remove(userName);
                }
                Visible = false;
                tray.Visible = true;
            } else {
                MessageBox.Show("登录失败!" + Environment.NewLine + "错误原因:" + response.ErrorMessage);
            }
        }

        private void setDefultLogonData() {
            var latestUser = UserCenter.Instance.QueryLatestUser();
            if(latestUser == null) {
                txtUserName.Text = string.Empty;
                txtPwd.Text = string.Empty;
                chbSavePassword.Checked = false;
            } else {
                txtUserName.Text = latestUser.UserName;
                txtPwd.Text = latestUser.Password;
                chbSavePassword.Checked = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            Close();
        }

        private void tsmiCarrier_Click(object sender, EventArgs e) {
            CarrierForm.Instance.Show();
            CarrierForm.Instance.Activate();
        }

        private void tsmiStatus_Click(object sender, EventArgs e) {
            StatusForm.Instance.Show();
            StatusForm.Instance.Activate();
        }

        private void tsmiChangeUser_Click(object sender, EventArgs e) {
            Program.Logoff();
            tray.Visible = false;
            CarrierForm.Instance.Dispose();
            StatusForm.Instance.Dispose();
            RemindForm.Instance.CustomerClose();
            Visible = true;
            setDefultLogonData();
        }

        private void tsmiExit_Click(object sender, EventArgs e) {
            tray.Visible = false;
            Program.Exit(true);
        }

        private void txtUserName_TextChanged(object sender, EventArgs e) {
            var userInfo = UserCenter.Instance.Query(txtUserName.Text.Trim());
            if(userInfo == null) {
                txtPwd.Text = string.Empty;
                chbSavePassword.Checked = false;
            } else {
                txtPwd.Text = userInfo.Password;
                chbSavePassword.Checked = true;
            }
        }
    }
}