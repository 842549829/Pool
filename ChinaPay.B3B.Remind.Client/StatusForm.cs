using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ChinaPay.B3B.Remind.Client {
    partial class StatusForm : Form {
        private StatusForm() {
            InitializeComponent();
            Winform.MoveHelper.SetMoveable(this, this.pnlTitle, this.pnlBody, this.pnlContent);
            m_statuses = null;
        }

        private static StatusForm m_instance = null;
        private static object m_locker = new object();
        public static StatusForm Instance {
            get {
                if(m_instance == null || m_instance.IsDisposed) {
                    lock(m_locker) {
                        if(m_instance == null || m_instance.IsDisposed) {
                            m_instance = new StatusForm();
                        }
                    }
                }
                return m_instance;
            }
        }

        private Dictionary<string, string> m_statuses = null;
        public Dictionary<string, string> Statuses {
            get {
                if(m_statuses == null) {
                    if(Program.LogonInfo != null) {
                        var request = new Command.QueryAllStatus(Settings.ServerHost, Settings.ServerPort, Program.LogonInfo.BatchNo);
                        var response = request.Execute();
                        if(response.Success) {
                            m_statuses = response.Response;
                        } else {
                            m_statuses = new Dictionary<string, string>();
                            MessageBox.Show("获取状态失败!" + Environment.NewLine + "错误原因:" + response.ErrorMessage);
                        }
                    } else {
                        m_statuses = new Dictionary<string, string>();
                    }
                }
                return m_statuses;
            }
        }

        private void StatusForm_Load(object sender, EventArgs e) {
            initializeStatuses();
            setSelectedStatuses();
        }

        private void btnSave_Click(object sender, EventArgs e) {
            var selectedStatuses = getSelectedStatuses();
            if(selectedStatuses.Count > 0) {
                var request = new Command.SaveStatus(Settings.ServerHost, Settings.ServerPort, Program.LogonInfo.BatchNo, selectedStatuses);
                var response = request.Execute();
                if(response.Success) {
                    MessageBox.Show("修改成功");
                    this.Close();
                } else {
                    MessageBox.Show("修改失败!" + Environment.NewLine + "错误原因:" + response.ErrorMessage);
                }
            } else {
                MessageBox.Show("至少选择一种状态");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Close();
        }

        void initializeStatuses() {
            int row = 0, col = 0, rowInterval = 30, colInterval = 100;
            foreach(var item in this.Statuses) {
                var x = 30 + col * colInterval;
                var y = 15 + row * rowInterval;
                this.pnlContent.Controls.Add(new CheckBox() {
                    Name = item.Key,
                    Text = item.Value,
                    Location = new Point(x, y),
                    AutoSize = true,
                    UseVisualStyleBackColor = true
                });
                col++;
                if(col > 1) {
                    row++;
                    col = 0;
                }
            }
        }
        void setSelectedStatuses() {
            var request = new Command.QueryStatus(Settings.ServerHost, Settings.ServerPort, Program.LogonInfo.BatchNo);
            var response = request.Execute();
            if(response.Success) {
                if(response.Response.Count == 0) {
                    setAllStatusCheckState(true);
                } else {
                    selectStatuses(response.Response);
                }
            } else {
                MessageBox.Show("获取设置信息失败!" + Environment.NewLine + "错误原因:" + response.ErrorMessage);
            }
        }
        IList<string> getSelectedStatuses() {
            var result = new List<string>();
            foreach(var item in this.pnlContent.Controls) {
                if(item is CheckBox) {
                    var cb = item as CheckBox;
                    if(cb.Checked) {
                        result.Add(cb.Name);
                    }
                }
            }
            return result;
        }
        void setAllStatusCheckState(bool isChecked) {
            foreach(var item in this.pnlContent.Controls) {
                if(item is CheckBox) {
                    (item as CheckBox).Checked = isChecked;
                }
            }
        }
        void selectStatuses(IList<string> statuses) {
            foreach(var item in this.pnlContent.Controls) {
                if(item is CheckBox) {
                    var cb = item as CheckBox;
                    if(statuses.Contains(cb.Name)) {
                        cb.Checked = true;
                    }
                }
            }
        }
    }
}