using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ChinaPay.B3B.Remind.Client {
    partial class CarrierForm : Form {
        private CarrierForm() {
            InitializeComponent();
            ChinaPay.Winform.MoveHelper.SetMoveable(this, this.pnlTitle, this.pnlBody, this.pnlContent);
        }

        private static CarrierForm m_instance = null;
        private static object m_locker = new object();
        public static CarrierForm Instance {
            get {
                if(m_instance == null || m_instance.IsDisposed) {
                    lock(m_locker) {
                        if(m_instance == null || m_instance.IsDisposed) {
                            m_instance = new CarrierForm();
                        }
                    }
                }
                return m_instance;
            }
        }

        private IList<Model.Carrier> m_carriers = null;
        public IEnumerable<Model.Carrier> Carriers {
            get {
                if(m_carriers == null) {
                    if(Program.LogonInfo != null) {
                        var request = new Command.QueryAllCarriers(Settings.ServerHost, Settings.ServerPort, Program.LogonInfo.BatchNo);
                        var response = request.Execute();
                        if(response.Success) {
                            m_carriers = response.Response;
                        } else {
                            m_carriers = new List<Model.Carrier>();
                            MessageBox.Show("获取乘运人失败!" + Environment.NewLine + "错误原因:" + response.ErrorMessage);
                        }
                    } else {
                        m_carriers = new List<Model.Carrier>();
                    }
                }
                return m_carriers;
            }
        }

        private void CarrierForm_Load(object sender, EventArgs e) {
            if(Program.LogonInfo != null) {
                initializeCarriers();
                setSelectedCarriers();
            }
        }

        private void btnSave_Click(object sender, EventArgs e) {
            var selectedCarriers = getSelectedCarriers();
            if(selectedCarriers.Count > 0) {
                var request = new Command.SaveCarrier(Settings.ServerHost, Settings.ServerPort, Program.LogonInfo.BatchNo, selectedCarriers);
                var response = request.Execute();
                if(response.Success) {
                    MessageBox.Show("修改成功");
                    this.Close();
                } else {
                    MessageBox.Show("修改失败!" + Environment.NewLine + "错误原因:" + response.ErrorMessage);
                }
            } else {
                MessageBox.Show("至少选择一个航空公司");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void chbAll_CheckedChanged(object sender, EventArgs e) {
            setAllCarrierCheckState(this.chbAll.Checked);
        }

        void initializeCarriers() {
            int row = 0, col = 0, rowInterval = 22, colInterval = 87;
            foreach(var item in Carriers) {
                var x = 28 + col * colInterval;
                var y = 9 + row * rowInterval;
                this.pnlContent.Controls.Add(new CheckBox() {
                    Name = item.Code,
                    Text = item.Name,
                    Location = new Point(x, y),
                    AutoSize = true,
                    UseVisualStyleBackColor = true
                });
                col++;
                if(col > 2) {
                    row++;
                    col = 0;
                }
            }
        }
        void setSelectedCarriers() {
            var request = new Command.QueryCarrier(Settings.ServerHost, Settings.ServerPort, Program.LogonInfo.BatchNo);
            var response = request.Execute();
            if(response.Success) {
                if(response.Response.Count == 0) {
                    setAllCarrierCheckState(true);
                } else {
                    selectCarriers(response.Response);
                }
            } else {
                MessageBox.Show("获取设置信息失败!" + Environment.NewLine + "错误原因:" + response.ErrorMessage);
            }
        }
        List<string> getSelectedCarriers() {
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
        void setAllCarrierCheckState(bool isChecked) {
            foreach(var item in this.pnlContent.Controls) {
                if(item is CheckBox) {
                    (item as CheckBox).Checked = isChecked;
                }
            }
        }
        void selectCarriers(IList<string> carriers) {
            foreach(var item in this.pnlContent.Controls) {
                if(item is CheckBox) {
                    var cb = item as CheckBox;
                    if(carriers.Contains(cb.Name)) {
                        cb.Checked = true;
                    }
                }
            }
        }
    }
}