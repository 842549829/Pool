using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ChinaPay.B3B.Remind.Client.Model;

namespace ChinaPay.B3B.Remind.Client {
    partial class RemindForm : Form {
        Timer m_movingTimer = null;
        Timer m_restingTimer = null;
        readonly int m_movingInterval = 20;
        readonly int m_restingTime = 5000;

        private RemindForm() {
            InitializeComponent();
        }

        private static RemindForm m_instatnce;
        public static RemindForm Instance {
            get {
                return m_instatnce;
            }
        }
        public static void Init() {
            if(m_instatnce == null) {
                m_instatnce = new RemindForm();
                m_instatnce.Show();
                m_instatnce.Visible = false;
                m_instatnce.TopMost = true;
            }
        }
        public void CustomerClose() {
            stopMoving();
            stopRestTimeWatcher();
            Visible = false;
        }

        void pnlClose_Click(object sender, EventArgs e) {
            CustomerClose();
        }
        void RemindForm_FormClosing(object sender, FormClosingEventArgs e) {
            if(m_movingTimer != null) {
                m_movingTimer.Dispose();
            }
            if(m_restingTimer != null) {
                m_restingTimer.Dispose();
            }
        }

        public void Show(IList<RemindRecord> records) {
            if(records != null && records.Count > 0) {
                if(InvokeRequired) {
                    var xx = new ShowRemindForm(showRemindForm);
                    Invoke(xx, records);
                } else {
                    showRemindForm(records);
                }
            }
        }
        void showRemindForm(IEnumerable<RemindRecord> records) {
            bindRecords(records);
            Winform.WindowHelper.ShowWindow(Handle, Winform.WindowHelper.SW_SHOWNOACTIVATE);
            Winform.WindowHelper.SetForegroundWindow((IntPtr)((ulong)Handle | 0x01));
            setInitialLocation();
            startMoving();
            stopRestTimeWatcher();
        }
        void bindRecords(IEnumerable<RemindRecord> records) {
            var controls = new List<Control>();
            int row = 0;
            int rowInterval = 22;
            foreach(var item in records) {
                if(row > 0) {
                    controls.Add(new PictureBox() {
                        Location = new Point(0, rowInterval * row),
                        Size = new Size(this.Size.Width, 1),
                        TabStop = false,
                        Image = Properties.Resources.crossband
                    });
                }
                controls.Add(new Label() {
                    BackColor = Color.Transparent,
                    Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134))),
                    Location = new Point(39, 3 + row * rowInterval),
                    AutoSize = true,
                    Text = item.Name,
                    TabStop = false
                });
                controls.Add(new Label() {
                    BackColor = Color.Transparent,
                    Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134))),
                    ForeColor = Color.White,
                    Image = Properties.Resources.countBackground,
                    Location = new Point(180, 3 + row * rowInterval),
                    Size = new Size(43, 16),
                    Text = item.Count.ToString(),
                    TextAlign = ContentAlignment.MiddleCenter
                });
                row++;
            }
            pnlContent.Controls.Clear();
            pnlContent.Controls.AddRange(controls.ToArray());
        }
        void startMoving() {
            if(m_movingTimer == null) {
                m_movingTimer = new Timer();
                m_movingTimer.Interval = m_movingInterval;
                m_movingTimer.Tick += new EventHandler(delegate(object sender, EventArgs e) {
                    move(0, -1);
                    if(isFinalLocation()) {
                        stopMoving();
                        startRestTimeWatcher();
                    }
                });
            }
            m_movingTimer.Start();
        }
        void stopMoving() {
            if(m_movingTimer != null) {
                m_movingTimer.Stop();
            }
        }
        void startRestTimeWatcher() {
            if(m_restingTimer == null) {
                m_restingTimer = new Timer();
                m_restingTimer.Interval = m_restingTime;
                m_restingTimer.Tick += new EventHandler(delegate(object sender, EventArgs e) {
                    Visible = false;
                    stopRestTimeWatcher();
                });
            }
            m_restingTimer.Start();
        }
        void stopRestTimeWatcher() {
            if(m_restingTimer != null) {
                m_restingTimer.Stop();
            }
        }
        void setInitialLocation() {
            var startX = Screen.PrimaryScreen.WorkingArea.Width - Size.Width - 20;
            var startY = Screen.PrimaryScreen.WorkingArea.Bottom;
            Location = new Point(startX, startY);
        }
        Point getFinalLocation() {
            var startX = Screen.PrimaryScreen.WorkingArea.Width - Size.Width - 20;
            var startY = Screen.PrimaryScreen.WorkingArea.Bottom - Size.Height;
            return new Point(startX, startY);
        }
        bool isFinalLocation() {
            return Location == getFinalLocation();
        }
        void move(int x, int y) {
            Location = new Point(Location.X + x, Location.Y + y);
        }

        delegate void ShowRemindForm(IEnumerable<RemindRecord> records);
    }
}