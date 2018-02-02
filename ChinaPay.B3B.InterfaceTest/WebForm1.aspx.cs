using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Web.Security;
using System.Text;

namespace ChinaPay.B3B.InterfaceTest
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        { 
        }

        protected void btnPnrImport_Click(object sender, EventArgs e)
        {
            PNRImport.Visible = true;
            ProduceOrder.Visible = false;
            OrderPay.Visible = false;
            QueryOrder.Visible = false;
            Jiexi.Visible = false;
            ApplyRefund.Visible = false;
            ApplyPostpone.Visible = false;
            PayApplyform.Visible = false;
            QueryApplyform.Visible = false;
            OnLineOrderPay.Visible = false;
            PayApplyformByPayType.Visible = false;
            PNRImportNoNeedPat.Visible = false;
            AutoPay.Visible = false;
            QueryFlights.Visible = false;
            QueryFlight.Visible = false;
            QueryFlightStop.Visible = false;
            ProduceOrder2.Visible = false;
        }

        protected void btnProductOrder_Click(object sender, EventArgs e)
        {
            PNRImport.Visible = false;
            ProduceOrder.Visible = true;
            OrderPay.Visible = false;
            QueryOrder.Visible = false;
            Jiexi.Visible = false;
            ApplyRefund.Visible = false;
            ApplyPostpone.Visible = false;
            PayApplyform.Visible = false;
            QueryApplyform.Visible = false;
            OnLineOrderPay.Visible = false;
            PNRImportNoNeedPat.Visible = false;
            PayApplyformByPayType.Visible = false;
            AutoPay.Visible = false;
            QueryFlights.Visible = false;
            QueryFlight.Visible = false;
            QueryFlightStop.Visible = false;
            ProduceOrder2.Visible = false;
        }

        protected void btnPayOrder_Click(object sender, EventArgs e)
        {
            PNRImport.Visible = false;
            ProduceOrder.Visible = false;
            OrderPay.Visible = true;
            QueryOrder.Visible = false;
            Jiexi.Visible = false;
            ApplyRefund.Visible = false;
            ApplyPostpone.Visible = false;
            PayApplyform.Visible = false;
            QueryApplyform.Visible = false;
            OnLineOrderPay.Visible = false;
            PNRImportNoNeedPat.Visible = false;
            PayApplyformByPayType.Visible = false;
            AutoPay.Visible = false;
            QueryFlights.Visible = false;
            QueryFlight.Visible = false;
            QueryFlightStop.Visible = false;
            ProduceOrder2.Visible = false;
        }

        protected void btnOrderDateil_Click(object sender, EventArgs e)
        {
            PNRImport.Visible = false;
            ProduceOrder.Visible = false;
            OrderPay.Visible = false;
            QueryOrder.Visible = true;
            Jiexi.Visible = false;
            ApplyRefund.Visible = false;
            ApplyPostpone.Visible = false;
            PayApplyform.Visible = false;
            QueryApplyform.Visible = false;
            PNRImportNoNeedPat.Visible = false;
            PayApplyformByPayType.Visible = false;
            OnLineOrderPay.Visible = false;
            AutoPay.Visible = false;
            QueryFlights.Visible = false;
            QueryFlight.Visible = false;
            QueryFlightStop.Visible = false;
            ProduceOrder2.Visible = false;
        }

        protected void btnJiexi_Click(object sender, EventArgs e)
        {
            PNRImport.Visible = false;
            ProduceOrder.Visible = false;
            OrderPay.Visible = false;
            QueryOrder.Visible = false;
            PayApplyformByPayType.Visible = false;
            ApplyRefund.Visible = false;
            ApplyPostpone.Visible = false;
            PayApplyform.Visible = false;
            Jiexi.Visible = true;
            QueryApplyform.Visible = false;
            OnLineOrderPay.Visible = false;
            PNRImportNoNeedPat.Visible = false;
            AutoPay.Visible = false;
            QueryFlights.Visible = false;
            QueryFlight.Visible = false;
            QueryFlightStop.Visible = false;
            ProduceOrder2.Visible = false;
        }
        protected void btnApplyRefund_Click(object sender, EventArgs e)
        {
            PNRImport.Visible = false;
            ProduceOrder.Visible = false;
            OrderPay.Visible = false;
            QueryOrder.Visible = false;
            PayApplyformByPayType.Visible = false;
            Jiexi.Visible = false;
            ApplyRefund.Visible = true;
            ApplyPostpone.Visible = false;
            PayApplyform.Visible = false;
            OnLineOrderPay.Visible = false;
            QueryApplyform.Visible = false;
            AutoPay.Visible = false;
            QueryFlights.Visible = false;
            QueryFlight.Visible = false;
            QueryFlightStop.Visible = false;
            ProduceOrder2.Visible = false;
        }

        protected void btnApplyPostpone_Click(object sender, EventArgs e)
        {
            PNRImport.Visible = false;
            ProduceOrder.Visible = false;
            OrderPay.Visible = false;
            QueryOrder.Visible = false;
            Jiexi.Visible = false;
            PayApplyformByPayType.Visible = false;
            ApplyRefund.Visible = false;
            ApplyPostpone.Visible = true;
            PayApplyform.Visible = false;
            QueryApplyform.Visible = false;
            OnLineOrderPay.Visible = false;
            PNRImportNoNeedPat.Visible = false;
            AutoPay.Visible = false;
            QueryFlights.Visible = false;
            QueryFlight.Visible = false;
            QueryFlightStop.Visible = false;
            ProduceOrder2.Visible = false;
        }

        protected void btnPayApplyform_Click(object sender, EventArgs e)
        {
            PNRImport.Visible = false;
            ProduceOrder.Visible = false;
            OrderPay.Visible = false;
            QueryOrder.Visible = false;
            Jiexi.Visible = false;
            ApplyRefund.Visible = false;
            PayApplyformByPayType.Visible = false;
            ApplyPostpone.Visible = false;
            PayApplyform.Visible = true;
            QueryApplyform.Visible = false;
            OnLineOrderPay.Visible = false;
            PNRImportNoNeedPat.Visible = false;
            AutoPay.Visible = false;
            QueryFlights.Visible = false;
            QueryFlight.Visible = false;
            QueryFlightStop.Visible = false;
            ProduceOrder2.Visible = false;
        }

        protected void btnQueryApplyform_Click(object sender, EventArgs e)
        {
            PNRImport.Visible = false;
            PayApplyformByPayType.Visible = false;
            ProduceOrder.Visible = false;
            OrderPay.Visible = false;
            QueryOrder.Visible = false;
            Jiexi.Visible = false;
            ApplyRefund.Visible = false;
            ApplyPostpone.Visible = false;
            PayApplyform.Visible = false;
            QueryApplyform.Visible = true;
            OnLineOrderPay.Visible = false;
            PNRImportNoNeedPat.Visible = false;
            AutoPay.Visible = false;
            QueryFlights.Visible = false;
            QueryFlight.Visible = false;
            QueryFlightStop.Visible = false;
            ProduceOrder2.Visible = false;
        }

        protected void btnOnLineOrderPay_Click(object sender, EventArgs e)
        {
            OnLineOrderPay.Visible = true;
            PNRImport.Visible = false;
            ProduceOrder.Visible = false;
            OrderPay.Visible = false;
            QueryOrder.Visible = false;
            Jiexi.Visible = false;
            ApplyRefund.Visible = false;
            ApplyPostpone.Visible = false;
            PayApplyform.Visible = false;
            QueryApplyform.Visible = false;
            PayApplyformByPayType.Visible = false;
            PNRImportNoNeedPat.Visible = false;
            AutoPay.Visible = false;
            QueryFlights.Visible = false;
            QueryFlight.Visible = false;
            QueryFlightStop.Visible = false;
            ProduceOrder2.Visible = false;
        }

        protected void btnPayApplyformByPayType_Click(object sender, EventArgs e)
        {
            PayApplyformByPayType.Visible = true;
            OnLineOrderPay.Visible = false;
            PNRImport.Visible = false;
            ProduceOrder.Visible = false;
            OrderPay.Visible = false;
            QueryOrder.Visible = false;
            Jiexi.Visible = false;
            ApplyRefund.Visible = false;
            ApplyPostpone.Visible = false;
            PayApplyform.Visible = false;
            QueryApplyform.Visible = false;
            PNRImportNoNeedPat.Visible = false;
            AutoPay.Visible = false;
            QueryFlights.Visible = false;
            QueryFlight.Visible = false;
            QueryFlightStop.Visible = false;
            ProduceOrder2.Visible = false;
        }

        protected void btnPNRImportNoNeedPat_Click(object sender, EventArgs e)
        {
            PayApplyformByPayType.Visible = false;
            OnLineOrderPay.Visible = false;
            PNRImport.Visible = false;
            ProduceOrder.Visible = false;
            OrderPay.Visible = false;
            QueryOrder.Visible = false;
            Jiexi.Visible = false;
            ApplyRefund.Visible = false;
            ApplyPostpone.Visible = false;
            PayApplyform.Visible = false;
            QueryApplyform.Visible = false;
            PNRImportNoNeedPat.Visible = true;
            AutoPay.Visible = false;
            QueryFlights.Visible = false;
            QueryFlight.Visible = false;
            QueryFlightStop.Visible = false;
            ProduceOrder2.Visible = false;
        }
        protected void btnAutoPay_Click(object sender, EventArgs e)
        {
            PayApplyformByPayType.Visible = false;
            OnLineOrderPay.Visible = false;
            PNRImport.Visible = false;
            ProduceOrder.Visible = false;
            OrderPay.Visible = false;
            QueryOrder.Visible = false;
            Jiexi.Visible = false;
            ApplyRefund.Visible = false;
            ApplyPostpone.Visible = false;
            PayApplyform.Visible = false;
            QueryApplyform.Visible = false;
            PNRImportNoNeedPat.Visible = false;
            AutoPay.Visible = true;
            QueryFlights.Visible = false;
            QueryFlight.Visible = false;
            QueryFlightStop.Visible = false;
            ProduceOrder2.Visible = false;
        }

        protected void btnQueryFlights_Click(object sender, EventArgs e)
        {
            PayApplyformByPayType.Visible = false;
            OnLineOrderPay.Visible = false;
            PNRImport.Visible = false;
            ProduceOrder.Visible = false;
            OrderPay.Visible = false;
            QueryOrder.Visible = false;
            Jiexi.Visible = false;
            ApplyRefund.Visible = false;
            ApplyPostpone.Visible = false;
            PayApplyform.Visible = false;
            QueryApplyform.Visible = false;
            PNRImportNoNeedPat.Visible = false;
            AutoPay.Visible = false;
            QueryFlights.Visible = true;
            QueryFlight.Visible = false;
            QueryFlightStop.Visible = false;
            ProduceOrder2.Visible = false;
        }

        protected void btnQueryFlightStop_Click(object sender, EventArgs e)
        {
            PayApplyformByPayType.Visible = false;
            OnLineOrderPay.Visible = false;
            PNRImport.Visible = false;
            ProduceOrder.Visible = false;
            OrderPay.Visible = false;
            QueryOrder.Visible = false;
            Jiexi.Visible = false;
            ApplyRefund.Visible = false;
            ApplyPostpone.Visible = false;
            PayApplyform.Visible = false;
            QueryApplyform.Visible = false;
            PNRImportNoNeedPat.Visible = false;
            AutoPay.Visible = false;
            QueryFlights.Visible = false;
            QueryFlight.Visible = false;
            QueryFlightStop.Visible = true;
            ProduceOrder2.Visible = false;

        }

        protected void btnQueryFlight_Click(object sender, EventArgs e)
        {
            PayApplyformByPayType.Visible = false;
            OnLineOrderPay.Visible = false;
            PNRImport.Visible = false;
            ProduceOrder.Visible = false;
            OrderPay.Visible = false;
            QueryOrder.Visible = false;
            Jiexi.Visible = false;
            ApplyRefund.Visible = false;
            ApplyPostpone.Visible = false;
            PayApplyform.Visible = false;
            QueryApplyform.Visible = false;
            PNRImportNoNeedPat.Visible = false;
            AutoPay.Visible = false;
            QueryFlights.Visible = false;
            QueryFlight.Visible = true;
            QueryFlightStop.Visible = false;
            ProduceOrder2.Visible = false;

        }

        protected void btnProduceOrder2_Click(object sender, EventArgs e)
        {
            PayApplyformByPayType.Visible = false;
            OnLineOrderPay.Visible = false;
            PNRImport.Visible = false;
            ProduceOrder.Visible = false;
            OrderPay.Visible = false;
            QueryOrder.Visible = false;
            Jiexi.Visible = false;
            ApplyRefund.Visible = false;
            ApplyPostpone.Visible = false;
            PayApplyform.Visible = false;
            QueryApplyform.Visible = false;
            PNRImportNoNeedPat.Visible = false;
            AutoPay.Visible = false;
            QueryFlights.Visible = false;
            QueryFlight.Visible = false;
            QueryFlightStop.Visible = false;
            ProduceOrder2.Visible = true;
        }


        protected void btnOk_Click(object sender, EventArgs e)
        {
            //执行方法
            //ws.InterfaceService client = new ws.InterfaceService();
            WebReference.InterfaceService client = new WebReference.InterfaceService();
            //localhost.InterfaceService client = new localhost.InterfaceService();
            if (PNRImport.Visible)
            {
                //拼接签名
                string str = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + HttpUtility.UrlEncode(pnrContext.Text), "MD5").ToUpper();
                txtMsg.Text = client.PNRImport(HttpUtility.UrlEncode(pnrContext.Text), userName.Text, str).OuterXml;
            }
            if (ProduceOrder.Visible)
            {
                //拼接签名
                string str = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + HttpUtility.UrlEncode(pnrContext1.Text) + associatePNR.Text + contact.Text + policyId.Text + batchNo.Text, "MD5").ToUpper();
                txtMsg.Text = client.ProduceOrder(HttpUtility.UrlEncode(pnrContext1.Text), associatePNR.Text, contact.Text, policyId.Text, batchNo.Text, userName.Text, str).OuterXml;
            }
            if (OrderPay.Visible)
            {//拼接签名
                string str = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + orderId.Text, "MD5").ToUpper();
                txtMsg.Text = client.OrderPay(orderId.Text, userName.Text, str).OuterXml;
            }
            if (QueryOrder.Visible)
            {//拼接签名
                string str = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + orderDId.Text, "MD5").ToUpper();
                txtMsg.Text = client.QueryOrder(orderDId.Text, userName.Text, str).OuterXml;
            }
            if (Jiexi.Visible)
            {
                txtMsg.Text = HttpUtility.UrlDecode(zhifu.Text);
            }
            if (ApplyRefund.Visible)
            {
                //拼接签名
                string str = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + txtOrderId2.Text + txtPersons.Text + txtFlights.Text + txtRefundType.Text + txtReason.Text, "MD5").ToUpper();
                txtMsg.Text = client.ApplyRefund(txtOrderId2.Text, txtPersons.Text, txtFlights.Text, txtRefundType.Text, txtReason.Text, userName.Text, str).OuterXml;
            }
            if (ApplyPostpone.Visible)
            {
                //拼接签名
                string str = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + txtOrderId3.Text + txtPassengers.Text + txtVoyages.Text + txtReason1.Text, "MD5").ToUpper();
                txtMsg.Text = client.ApplyPostpone(txtOrderId3.Text, txtPassengers.Text, txtVoyages.Text, txtReason1.Text, userName.Text, str).OuterXml;
            }

            if (PayApplyform.Visible)
            {
                //拼接签名
                string str = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + txtOrderId4.Text, "MD5").ToUpper();
                txtMsg.Text = client.PayApplyform(txtOrderId4.Text, userName.Text, str).OuterXml;
            }


            if (QueryApplyform.Visible)
            {
                //拼接签名
                string str = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + txtOrderId5.Text, "MD5").ToUpper();
                txtMsg.Text = client.QueryApplyform(txtOrderId5.Text, userName.Text, str).OuterXml;
            }

            if (OnLineOrderPay.Visible)
            {
                //拼接签名
                string str = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + txtOrderId6.Text + txtBankInfo.Text, "MD5").ToUpper();
                txtMsg.Text = client.PayOrderByPayType(txtOrderId6.Text, txtBankInfo.Text, userName.Text, str).OuterXml;
            }
            if (PayApplyformByPayType.Visible)
            {
                //拼接签名
                string str = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + txtOrderId7.Text + txtPayType.Text, "MD5").ToUpper();
                txtMsg.Text = client.PayApplyformByPayType(txtOrderId7.Text, txtPayType.Text, userName.Text, str).OuterXml;
            }
            if (PNRImportNoNeedPat.Visible)
            {
                //拼接签名
                string str = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + HttpUtility.UrlEncode(pnrContext3.Text), "MD5").ToUpper();
                txtMsg.Text = client.PNRImportWithoutPat(HttpUtility.UrlEncode(pnrContext3.Text), userName.Text, str).OuterXml;
            }

            if (PNRImportNoNeedPat.Visible)
            {
                //拼接签名
                string str = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + HttpUtility.UrlEncode(pnrContext3.Text), "MD5").ToUpper();
                txtMsg.Text = client.PNRImportWithoutPat(HttpUtility.UrlEncode(pnrContext3.Text), userName.Text, str).OuterXml;
            }
            //if (AutoPay.Visible)
            //{
            //    //拼接签名
            //    string str = "<b3b><userName>" + userName.Text + "</userName><sign>" + FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + "AutoPay" + txtOrderId8.Text + txtPayType1.Text + txtOrderType.Text, "MD5").ToUpper() + "</sign><service>AutoPay</service><params><orderId>" + txtOrderId8.Text + "</orderId><payType>" + txtPayType1.Text + "</payType><orderType>" + txtOrderType.Text + "</orderType></params></b3b>";
            //    txtMsg.Text = client.Execute(str).OuterXml;
            //}
            //if (QueryFlights.Visible)
            //{
            //    //拼接签名
            //    string str = "<b3b><userName>" + userName.Text + "</userName><sign>" + FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + "QueryFlights" + txtDeparture.Text + txtArrival.Text + txtFlightDate.Text, "MD5").ToUpper() + "</sign><service>QueryFlights</service><params><departure>" + txtDeparture.Text + "</departure><arrival>" + txtArrival.Text + "</arrival><flightDate>" + txtFlightDate.Text + "</flightDate></params></b3b>";
            //    txtMsg.Text = client.Execute(str).OuterXml;
            //}

            //if (QueryFlight.Visible)
            //{
            //    //拼接签名
            //    string str = "<b3b><userName>" + userName.Text + "</userName><sign>" + FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + "QueryFlight" + txtBatchNo.Text + txtAirlineCode.Text + txtFlightNo.Text, "MD5").ToUpper() + "</sign><service>QueryFlight</service><params><batchNo>" + txtBatchNo.Text + "</batchNo><airlineCode>" + txtAirlineCode.Text + "</airlineCode><flightNo>" + txtFlightNo.Text + "</flightNo></params></b3b>";
            //    txtMsg.Text = client.Execute(str).OuterXml;
            //}

            //if (QueryFlightStop.Visible)
            //{
            //    //拼接签名
            //    string str = "<b3b><userName>" + userName.Text + "</userName><sign>" + FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + "QueryFlightStop" + txtAirlineCode1.Text + txtFlightNo1.Text + txtFlightDate1.Text, "MD5").ToUpper() + "</sign><service>QueryFlightStop</service><params><airlineCode>" + txtAirlineCode1.Text + "</airlineCode><flightNo>" + txtFlightNo1.Text + "</flightNo><flightDate>" + txtFlightDate1.Text + "</flightDate></params></b3b>";
            //    txtMsg.Text = client.Execute(str).OuterXml;
            //}
            //if (ProduceOrder2.Visible)
            //{
            //    //拼接签名
            //    string str = "<b3b><userName>" + userName.Text + "</userName><sign>" + FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + "ProduceOrder2" + txtFlights1.Text + txtPassengers1.Text + txtContact.Text + txtPolicyType.Text, "MD5").ToUpper() + "</sign><service>ProduceOrder2</service><params><flights>" + txtFlights1.Text + "</flights><passengers>" + txtPassengers1.Text + "</passengers><contact>" + txtContact.Text + "</contact><policyType>" + txtPolicyType.Text + "</policyType></params></b3b>";
            //    txtMsg.Text = client.Execute(str).OuterXml;
            //}
        }



    }
}