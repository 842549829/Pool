using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.UserControls
{
    public enum Mode
    {
        /// <summary>
        /// 正常模式
        /// </summary>
        Normal,
        /// <summary>
        /// 申请退改签模式
        /// 即带有选择框
        /// </summary>
        Apply,
        /// <summary>
        /// 出票模式
        /// 即带有票号输入框
        /// </summary>
        ETDZ,
        /// <summary>
        /// 提供资源模式
        /// 即带有舱位输入框
        /// </summary>
        Supply,
        /// <summary>
        /// 行程告知单
        /// </summary>
        Itinerary,
    }
    public partial class PNRInfo : System.Web.UI.UserControl
    {
        private PNRPair m_pnrCode;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindPNRInfo();
            }
        }
        public void InitData(Service.Order.Domain.Order order, Service.Order.Domain.PNRInfo pnrInfo)
        {
            InitData(order, pnrInfo, Mode.Normal);
        }

        public void InitData(Service.Order.Domain.Order order, Service.Order.Domain.PNRInfo pnrInfo, Mode mode)
        {
            this.m_pnrCode = pnrInfo.Code;
            m_ETDZPNR = order.ETDZPNR;
            m_reservePNR = order.ReservationPNR;
            m_adultPNR = order.AssociatePNR;
            m_IsChildrenOrder = order.IsChildrenOrder;
            this.passenger.InitData(order, pnrInfo.Passengers, mode, pnrInfo.Flights);
            this.voyage.InitData(order, pnrInfo.Flights, mode);
            this.divOperation.Visible = mode == Mode.Apply;
            this.divPNRCodeInfo.Visible = mode != Mode.Supply;
            this.btnApplyUpgrade.Visible = order.Product.ProductType != ProductType.Special && order.Product.ProductType != ProductType.Team && (order.TripType == ItineraryType.OneWay || order.TripType == ItineraryType.Roundtrip);
            this.btnApplyPostpone.Visible =  order.Product.ProductType != ProductType.Special;
        }
        private void bindPNRInfo()
        {
            if (ShowPNR && m_pnrCode!=null)
            {
                lblPNRCode.Text = AppendPNR(m_pnrCode, string.Empty);
                lblPNRCode.Text += AppendPNR(m_ETDZPNR, "出票编码：");
                lblPNRCode.Text += AppendPNR(m_reservePNR, "订座编码：");
                if (m_IsChildrenOrder)
                {
                    lblPNRCode.Text += AppendPNR(m_adultPNR, "成人编码：");
                }
                else
                {
                    lblPNRCode.Text += AppendPNR(m_adultPNR, "关联编码：");
                }
            }

        }

        string AppendPNR(PNRPair pnr, string tip)
        {
            if (PNRPair.IsNullOrEmpty(pnr)) return string.Empty;
            if (RenderedPNR.Any(pnr.Equals)) return string.Empty;
            var result = new StringBuilder(" ");
            result.Append(tip);
            if (!string.IsNullOrWhiteSpace(pnr.PNR)) result.AppendFormat("{0}(小)", pnr.PNR.ToUpper());
            result.Append(" ");
            if (!string.IsNullOrWhiteSpace(pnr.BPNR)) result.AppendFormat("{0}(大)", pnr.BPNR.ToUpper());
            RenderedPNR.Add(pnr);
            return result.ToString();
        }

        List<PNRPair> RenderedPNR = new List<PNRPair>();



        private bool _showPnr = true;
        private PNRPair m_reservePNR;
        private PNRPair m_adultPNR;
        private bool m_IsChildrenOrder;
        private PNRPair m_ETDZPNR;

        public bool ShowPNR
        {
            get
            {
                return _showPnr;
            }
            set
            {
                _showPnr = value;
            }
        }
    }
}