using System;
using System.Web.Services;
using System.Xml;
using System.Web;

namespace ChinaPay.B3B.Interface {
    /// <summary>
    /// InterfaceService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class InterfaceService : WebService {
        [WebMethod(Description = "统一接口方法")]
        public XmlDocument Execute(string request)
        {
            var context = RequestContext.Parse(request);
            if (context == null) return Responser.ErrorContextResponse;
            var processor = RequestProcessor.CreateProcessor(context);
            if (processor == null) return Responser.NoneProcessorResponse;
            return processor.Execute();
        }

        /// <summary>
        /// 编码内容导入
        /// </summary>
        /// <param name="pnrContext"></param>
        /// <param name="userName"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        [WebMethod(Description = "编码内容导入")]
        public XmlDocument PNRImport(string pnrContext, string userName, string sign) {
            var processor = new InterfaceProcessor.PNRImport(pnrContext, userName, sign);
            return processor.Execute();
        }

        /// <summary>
        /// 生成订单
        /// </summary> 
        /// <param name="associatePNR">关联编码</param>
        /// <param name="contact">联系信息</param>
        /// <param name="policyId">政策编号</param>
        /// <param name="userName">用户名</param>
        /// <param name="sign">签名</param>
        /// <returns>生成的订单信息</returns>
        [WebMethod(Description = "生成订单")]
        public XmlDocument ProduceOrder(string pnrContext, string associatePNR, string contact, string policyId, string batchNo, string userName, string sign) {
            var processor = new InterfaceProcessor.ProduceOrder(pnrContext, associatePNR, contact, policyId, batchNo, userName, sign);
            return processor.Execute();
        }

        /// <summary>
        /// 订单支付
        /// </summary>
        /// <param name="id">订单号</param>
        /// <param name="userName">用户名</param>
        /// <param name="sign">签名</param>
        /// <returns>支付地址</returns>
        [WebMethod(Description = "订单支付")]
        public XmlDocument OrderPay(string id, string userName, string sign) {
            var processor = new InterfaceProcessor.OrderPay(id, userName, sign);
            return processor.Execute();
        }

        /// <summary>
        /// 查询订单详情
        /// </summary>
        /// <param name="id">订单编号</param>
        /// <returns>订单详情</returns>
        [WebMethod(Description = "查询订单详情")]
        public XmlDocument QueryOrder(string id, string userName, string sign) {
            var processor = new InterfaceProcessor.QueryOrder(id, userName, sign);
            return processor.Execute();
        }

        /// <summary>
        /// 查询申请详情
        /// </summary>
        /// <param name="id">申请编号</param>
        /// <returns>申请单信息</returns>
        [WebMethod(Description = "查询申请详情")]
        public XmlDocument QueryApplyform(string id, string userName, string sign) {
            var processor = new InterfaceProcessor.QueryApplyform(id, userName, sign);
            return processor.Execute();
        }
        /// <summary>
        /// 申请改期
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="passengers">乘机人信息</param>
        /// <param name="voyages">航段信息</param>
        /// <param name="reason">申请原因</param>
        /// <returns>申请单信息</returns>
        [WebMethod(Description = "申请改期")]
        public XmlDocument ApplyPostpone(string orderId, string passengers, string voyages, string reason, string userName, string sign) {
            var processor = new InterfaceProcessor.ApplyPostpone(orderId, passengers, voyages, reason, userName, sign);
            return processor.Execute();
        }
        /// <summary>
        /// 申请退废票
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="passengers">乘机人信息</param>
        /// <param name="voyages">航段信息</param>
        /// <param name="refundType">申请类型</param>
        /// <param name="reason">申请原因</param>
        /// <returns>申请单信息</returns>
        [WebMethod(Description = "申请退废票")]
        public XmlDocument ApplyRefund(string orderId, string passengers, string voyages, string refundType, string reason, string userName, string sign) {
            var processor = new InterfaceProcessor.ApplyRefund(orderId, passengers, voyages, refundType, reason, userName, sign);
            return processor.Execute();
        }

        /// <summary>
        /// 改期支付
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <returns>支付地址</returns>
        [WebMethod(Description = "改期支付")]
        public XmlDocument PayApplyform(string orderId, string userName, string sign) {
            var processor = new InterfaceProcessor.PayApplyform(orderId, userName, sign);
            return processor.Execute();
        }

        /// <summary>
        /// 编码内容导入(不需要PAT)
        /// </summary>
        /// <param name="pnrContext"></param>
        /// <param name="userName"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        [WebMethod(Description = "编码内容导入(不需要PAT)")]
        public XmlDocument PNRImportWithoutPat(string pnrContext, string userName, string sign) {
            var processor = new InterfaceProcessor.PNRImportWithoutPat(pnrContext, userName, sign);
            return processor.Execute();
        }

        /// <summary>
        /// 订单改期在线支付
        /// </summary>
        /// <param name="id">订单号</param>
        /// <param name="payType">类型</param>
        /// <param name="userName">用户名</param>
        /// <param name="sign">签名</param>
        /// <returns>支付地址</returns>
        [WebMethod(Description = "改期在线支付")]
        public XmlDocument PayApplyformByPayType(string id, string payType, string userName, string sign) {
            var processor = new InterfaceProcessor.PayApplyformByPayType(id, userName, payType, sign);
            return processor.Execute();
        }
        /// <summary>
        ///订单在线支付 
        /// </summary>
        /// <param name="id">订单号</param>
        /// <param name="userName">账号</param>
        /// <param name="payType">银行信息</param>
        /// <returns>支付地址</returns>
        [WebMethod(Description = "订单在线支付")]
        public XmlDocument PayOrderByPayType(string id, string payType, string userName, string sign) {
            var processor = new InterfaceProcessor.PayOrderByPayType(id, userName, payType, sign);
            return processor.Execute();
        }
    }
}