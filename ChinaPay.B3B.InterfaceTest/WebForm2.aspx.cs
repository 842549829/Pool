using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace ChinaPay.B3B.InterfaceTest
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            //执行方法
            //ws.InterfaceService client = new ws.InterfaceService();
            WebReference.InterfaceService client = new WebReference.InterfaceService();
            //localhost.InterfaceService client = new localhost.InterfaceService();
            if (PNRImport.Visible)
            {
                string p = "<pnrContent>" + HttpUtility.UrlEncode(pnrContext.Text) + "</pnrContent><patContent>" + HttpUtility.UrlEncode(patContext.Text) + "</patContent>";
                string signs = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + "PNRImport" + p, "MD5").ToUpper();
                string str = getStr(signs, "PNRImport", p);
                //拼接签名
                // string str1 = "<b3b><userName>" + userName.Text + "</userName><sign>" + FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + "PNRImport" + HttpUtility.UrlEncode(pnrContext.Text) + HttpUtility.UrlEncode(patContext.Text), "MD5").ToUpper() + "</sign><service>PNRImport</service><params><pnrContent>" + HttpUtility.UrlEncode(pnrContext.Text) + "</pnrContent><patContent>" + HttpUtility.UrlEncode(patContext.Text) + "</patContent></params></b3b>";
                txtMsg.Text = client.Execute(str).OuterXml;
                ////拼接签名
                //string str = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + HttpUtility.UrlEncode(pnrContext.Text), "MD5").ToUpper();
                //txtMsg.Text = client.PNRImport(HttpUtility.UrlEncode(pnrContext.Text), userName.Text, str).OuterXml;
            }
            if (ProduceOrder.Visible)
            {
                string p = "<pnrContent>" + HttpUtility.UrlEncode(pnrContext1.Text) + "</pnrContent><patContent>" + HttpUtility.UrlEncode(patContext1.Text) + "</patContent><associatePNR>" + (associatePNR.Text) + "</associatePNR><contact>" + (contact.Text) + "</contact><policyId>" + (policyId.Text) + "</policyId><batchNo>" + batchNo.Text + "</batchNo>";
                string signs = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + "ProduceOrder" + p, "MD5").ToUpper();
                string str = getStr(signs, "ProduceOrder", p);
                txtMsg.Text = client.Execute(str).OuterXml;
            }
            if (OrderPay.Visible)
            {
                string signs = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + "OrderPay" + "<id>" + orderId.Text + "</id>", "MD5").ToUpper();
                string str = getStr(signs, "OrderPay", "<id>" + orderId.Text + "</id>");
                //拼接签名
                //string str = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + orderId.Text, "MD5").ToUpper();
                txtMsg.Text = client.Execute(str).OuterXml;
            }
            if (QueryOrder.Visible)
            {
                string signs = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + "QueryOrder" + "<id>" + orderDId.Text + "</id>", "MD5").ToUpper();
                string str = getStr(signs, "QueryOrder", "<id>" + orderDId.Text + "</id>");
                txtMsg.Text = client.Execute(str).OuterXml;
            }
            if (Jiexi.Visible)
            {
                txtMsg.Text = HttpUtility.UrlDecode(zhifu.Text);
            }
            if (ApplyRefund.Visible)
            {
                string p = string.Format("<orderId>{0}</orderId><passengers>{1}</passengers><voyages>{2}</voyages><refundType>{3}</refundType><reason>{4}</reason>",
                   txtOrderId2.Text, txtPersons.Text, txtFlights.Text, txtRefundType.Text, txtReason.Text);
                string signs = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + "ApplyRefund" + p, "MD5").ToUpper();
                string str = getStr(signs, "ApplyRefund", p);
                txtMsg.Text = client.Execute(str).OuterXml;
                // txtMsg.Text = client.ApplyRefund(txtOrderId2.Text, txtPersons.Text, txtFlights.Text, txtRefundType.Text, txtReason.Text, userName.Text, str).OuterXml;
            }
            if (ApplyPostpone.Visible)
            {
                string p = string.Format("<orderId>{0}</orderId><passengers>{1}</passengers><voyages>{2}</voyages><reason>{3}</reason>",
                   txtOrderId3.Text, txtPassengers.Text, txtVoyages.Text, txtReason1.Text);
                string signs = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + "ApplyPostpone" + p, "MD5").ToUpper();
                string str = getStr(signs, "ApplyPostpone", p);
                txtMsg.Text = client.Execute(str).OuterXml;
                ////拼接签名
                //string str = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + txtOrderId3.Text + txtPassengers.Text + txtVoyages.Text + txtReason1.Text, "MD5").ToUpper();
                //txtMsg.Text = client.ApplyPostpone(txtOrderId3.Text, txtPassengers.Text, txtVoyages.Text, txtReason1.Text, userName.Text, str).OuterXml;
            }

            if (PayApplyform.Visible)
            {
                string p = string.Format("<id>{0}</id>", txtOrderId4.Text);
                string signs = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + "PayApplyform" + p, "MD5").ToUpper();
                string str = getStr(signs, "PayApplyform", p);
                txtMsg.Text = client.Execute(str).OuterXml;
                //拼接签名
                //string str = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + txtOrderId4.Text, "MD5").ToUpper();
                //txtMsg.Text = client.PayApplyform(txtOrderId4.Text, userName.Text, str).OuterXml;
            }


            if (QueryApplyform.Visible)
            {
                string p = string.Format("<applyformId>{0}</applyformId>", txtOrderId5.Text);
                string signs = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + "QueryApplyform" + p, "MD5").ToUpper();
                string str = getStr(signs, "QueryApplyform", p);
                txtMsg.Text = client.Execute(str).OuterXml;
                //拼接签名
                //string str = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + txtOrderId5.Text, "MD5").ToUpper();
                //txtMsg.Text = client.QueryApplyform(txtOrderId5.Text, userName.Text, str).OuterXml;
            }

            if (OnLineOrderPay.Visible)
            {
                string p = string.Format("<id>{0}</id><payType>{1}</payType>", txtOrderId6.Text, txtBankInfo.Text);
                string signs = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + "PayOrderByPayType" + p, "MD5").ToUpper();
                string str = getStr(signs, "PayOrderByPayType", p);
                txtMsg.Text = client.Execute(str).OuterXml;
                //拼接签名
                //string str = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + txtOrderId6.Text + txtBankInfo.Text, "MD5").ToUpper();
                //txtMsg.Text = client.PayOrderByPayType(txtOrderId6.Text, txtBankInfo.Text, userName.Text, str).OuterXml;
            }
            if (PayApplyformByPayType.Visible)
            {
                string p = string.Format("<id>{0}</id><payType>{1}</payType>", txtOrderId7.Text, txtPayType.Text);
                string signs = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + "PayApplyformByPayType" + p, "MD5").ToUpper();
                string str = getStr(signs, "PayApplyformByPayType", p);
                txtMsg.Text = client.Execute(str).OuterXml;
                //拼接签名
                //string str = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + txtOrderId7.Text + txtPayType.Text, "MD5").ToUpper();
                //txtMsg.Text = client.PayApplyformByPayType(txtOrderId7.Text, txtPayType.Text, userName.Text, str).OuterXml;
            }
            if (PNRImportNoNeedPat.Visible)
            {
                string p = string.Format("<pnrContent>{0}</pnrContent>", HttpUtility.UrlEncode(pnrContext3.Text));
                string signs = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + "PNRImportWithoutPat" + p, "MD5").ToUpper();
                string str = getStr(signs, "PNRImportWithoutPat", p);
                txtMsg.Text = client.Execute(str).OuterXml;
                //拼接签名
                //string str = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + HttpUtility.UrlEncode(pnrContext3.Text), "MD5").ToUpper();
                //txtMsg.Text = client.PNRImportWithoutPat(HttpUtility.UrlEncode(pnrContext3.Text), userName.Text, str).OuterXml;
            }

            //if (PNRImportNoNeedPat.Visible)
            //{
            //    //拼接签名
            //    string str = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + HttpUtility.UrlEncode(pnrContext3.Text), "MD5").ToUpper();
            //    txtMsg.Text = client.PNRImportWithoutPat(HttpUtility.UrlEncode(pnrContext3.Text), userName.Text, str).OuterXml;
            //}
            if (AutoPay.Visible)
            {
                string p = "<id>" + txtOrderId8.Text + "</id><payType>" + txtPayType1.Text + "</payType><businessType>" + txtOrderType.Text + "</businessType>";
                string signs = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + "AutoPay" + p, "MD5").ToUpper();
                string str = getStr(signs, "AutoPay", p);
                txtMsg.Text = client.Execute(str).OuterXml;
            }
            if (QueryFlights.Visible)
            {
                string p = "<departure>" + txtDeparture.Text + "</departure><arrival>" + txtArrival.Text + "</arrival><flightDate>" + txtFlightDate.Text + "</flightDate>";
                string signs = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + "QueryFlights" + p, "MD5").ToUpper();
                string str = getStr(signs, "QueryFlights", p);
                //拼接签名
                //string str = "<b3b><userName>" + userName.Text + "</userName><sign>" + FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + "QueryFlights" + txtDeparture.Text + txtArrival.Text + txtFlightDate.Text, "MD5").ToUpper() + "</sign><service>QueryFlights</service><params><departure>" + txtDeparture.Text + "</departure><arrival>" + txtArrival.Text + "</arrival><flightDate>" + txtFlightDate.Text + "</flightDate></params></b3b>";
                txtMsg.Text = client.Execute(str).OuterXml;
            }

            if (QueryFlight.Visible)
            {
                string p = "<batchNo>" + txtBatchNo.Text + "</batchNo><airlineCode>" + txtAirlineCode.Text + "</airlineCode><flightNo>" + txtFlightNo.Text + "</flightNo>";
                string signs = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + "QueryFlight" + p, "MD5").ToUpper();
                string str = getStr(signs, "QueryFlight", p);
                //拼接签名
                //string str = "<b3b><userName>" + userName.Text + "</userName><sign>" + FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + "QueryFlight" + txtBatchNo.Text + txtAirlineCode.Text + txtFlightNo.Text, "MD5").ToUpper() + "</sign><service>QueryFlight</service><params><batchNo>" + txtBatchNo.Text + "</batchNo><airlineCode>" + txtAirlineCode.Text + "</airlineCode><flightNo>" + txtFlightNo.Text + "</flightNo></params></b3b>";
                txtMsg.Text = client.Execute(str).OuterXml;
            }

            if (QueryFlightStop.Visible)
            {
                string p = "<airlineCode>" + txtAirlineCode1.Text + "</airlineCode><flightNo>" + txtFlightNo1.Text + "</flightNo><flightDate>" + txtFlightDate1.Text + "</flightDate>";
                string signs = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + "QueryFlightStop" + p, "MD5").ToUpper();
                string str = getStr(signs, "QueryFlightStop", p);
                //拼接签名
                //string str = "<b3b><userName>" + userName.Text + "</userName><sign>" + FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + "QueryFlightStop" + txtAirlineCode1.Text + txtFlightNo1.Text + txtFlightDate1.Text, "MD5").ToUpper() + "</sign><service>QueryFlightStop</service><params><airlineCode>" + txtAirlineCode1.Text + "</airlineCode><flightNo>" + txtFlightNo1.Text + "</flightNo><flightDate>" + txtFlightDate1.Text + "</flightDate></params></b3b>";
                txtMsg.Text = client.Execute(str).OuterXml;
            }
            if (ProduceOrder2.Visible)
            {
                string p = "<flights>" + txtFlights1.Text + "</flights><passengers>" + txtPassengers1.Text + "</passengers><contact>" + txtContact.Text + "</contact><policyType>" + txtPolicyType.Text + "</policyType>";
                string signs = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + "ProduceOrder2" + p, "MD5").ToUpper();
                string str = getStr(signs, "ProduceOrder2", p);
                
                txtMsg.Text = client.Execute(str).OuterXml;
            }
            if (ManualPay.Visible)
            {
                string p = string.Format("<id>{0}</id><payType>{1}</payType><businessType>{2}</businessType>", txtId.Text, txtPayType5.Text, txtBusinessType.Text);
                string signs = FormsAuthentication.HashPasswordForStoringInConfigFile(userName.Text + sign.Text + "ManualPay" + p, "MD5").ToUpper();
                string str = getStr(signs, "ManualPay", p);
                txtMsg.Text = client.Execute(str).OuterXml;
            }
        }

        private string getStr(string sign, string service, string param)
        {
            return string.Format("<b3b><userName>{0}</userName><sign>{1}</sign><service>{2}</service><params>{3}</params></b3b>", userName.Text, sign, service, param);
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
            ManualPay.Visible = false;
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
            ManualPay.Visible = false;
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
            ManualPay.Visible = false;
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
            ManualPay.Visible = false;
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
            ManualPay.Visible = false;
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
            ManualPay.Visible = false;
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
            ManualPay.Visible = false;
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
            ManualPay.Visible = false;
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
            ManualPay.Visible = false;
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
            ManualPay.Visible = false;
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
            ManualPay.Visible = false;
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
            ManualPay.Visible = false;
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
            ManualPay.Visible = false;
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
            ManualPay.Visible = false;
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
            ManualPay.Visible = false;

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
            ManualPay.Visible = false;

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
            ManualPay.Visible = false;
            ProduceOrder2.Visible = true;
        }

        protected void btnManualPay_Click(object sender, EventArgs e)
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
            ProduceOrder2.Visible = false;
            ManualPay.Visible = true;
        }


    }
}