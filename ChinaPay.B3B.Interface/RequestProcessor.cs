using System;
using System.Linq;
using System.Xml;
using ChinaPay.B3B.Interface.Processor;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.Core;

namespace ChinaPay.B3B.Interface
{
    internal abstract class RequestProcessor
    {
        private DataTransferObject.Organization.EmployeeDetailInfo _employee;
        private DataTransferObject.Organization.CompanyDetailInfo _company;
        private Service.Organization.Domain.ExternalInterfaceSetting _interfaceSetting;

        /// <summary>
        /// 请求的上下文信息
        /// </summary>
        protected RequestContext Context { get; private set; }
        /// <summary>
        /// 号工信息
        /// </summary>
        protected DataTransferObject.Organization.EmployeeDetailInfo Employee
        {
            get
            {
                if (_employee == null)
                {
                    if (Context == null) throw new InvalidOperationException("缺少上下文信息");
                    if (string.IsNullOrWhiteSpace(Context.UserName)) InterfaceInvokeException.ThrowParameterMissException("用户名");
                    _employee = Cache.EmployeeCenter.Instance[Context.UserName];
                    if (_employee == null) { throw new InterfaceInvokeException("2"); }
                    if (!_employee.Enabled) { throw new InterfaceInvokeException("4"); }
                }
                return _employee;
            }
        }

        /// <summary>
        /// 单位信息
        /// </summary>
        protected DataTransferObject.Organization.CompanyDetailInfo Company
        {
            get
            {
                if (_company == null)
                {
                    _company = Cache.OrganizationCenter.Instance[Employee.Owner];
                    // 单位是否存在
                    if (_company == null) throw new InterfaceInvokeException("2");
                    // 单位是否启用
                    if (!_company.Enabled) throw new InterfaceInvokeException("5");
                    // 是否开通接口
                    if (!_company.IsOpenExternalInterface) throw new InterfaceInvokeException("6");
                }
                return _company;
            }
        }
        /// <summary>
        /// 接口设置信息
        /// </summary>
        protected Service.Organization.Domain.ExternalInterfaceSetting InterfaceSetting
        {
            get
            {
                if (_interfaceSetting == null)
                {
                    _interfaceSetting = Cache.InterfaceSettingCenter.Instance[Company.CompanyId];
                    if (_interfaceSetting == null) throw new InterfaceInvokeException("7");
                }
                return _interfaceSetting;
            }
        }

        /// <summary>
        /// 执行处理
        /// </summary>
        public XmlDocument Execute()
        {
            XmlDocument result;
            Guid? organization = null;
            var organizationName = string.Empty;
            try
            {
                organization = Company.CompanyId;
                organizationName = Company.AbbreviateName;

                validateSign();
                InvokeStatistic.Instance.Save(Context);
                validatePermission();
                //存储请求接口的域名
                ChinaPay.B3B.Service.LogService.SaveTextLog("客户端信息：IP " + System.Web.HttpContext.Current.Request.UserHostAddress + " 域名 " + System.Web.HttpContext.Current.Request.UserHostName);
                var execResult = ExecuteCore();
                result = Responser.Format(execResult);
            }
            catch (InterfaceInvokeException iie)
            {
                result = Responser.Format(iie);
            }
            catch (CustomException ex)
            {
                result = Responser.Format(ex);
            }
            catch (Exception ex)
            {
                Service.LogService.SaveExceptionLog(ex, "调用接口" + Context.Service);
                result = Responser.Format(ex);
            }
            // 记录访问日志
            var invokeLog = new Service.Log.Domain.InterfaceInvokeLog
            {
                InterfaceName = Context.Service,
                UserName = Context.UserName,
                Organization = organization,
                OrganizationName = organizationName,
                IPAddress = Context.ClientIP,
                Request = Context.Original,
                Response = result.InnerXml,
                Time = DateTime.Now
            };
            Service.LogService.SaveInterfaceInvokeLog(invokeLog);
            return result;
        }

        /// <summary>
        /// 执行请求
        /// </summary>
        protected abstract string ExecuteCore();

        /// <summary>
        /// 验证签名
        /// </summary>
        private void validateSign()
        {
            var signContent = Context.UserName + InterfaceSetting.SecurityCode + Context.Service + (Context.Params == null ? string.Empty : Context.Params.InnerXml);
            var signValue = Utility.MD5EncryptorService.MD5(signContent).ToUpper();
            if (signValue != Context.Sign) InterfaceInvokeException.ThrowSignErrorException();
        }
        /// <summary>
        /// 验证访问权限
        /// </summary>
        private void validatePermission()
        {
            // 是否有权限访问
            if (InterfaceSetting.BindIP != Context.ClientIP) InterfaceInvokeException.ThrowNoAccessException();
            if (InterfaceSetting.InterfaceInvokeMethod == null || !InterfaceSetting.InterfaceInvokeMethod.Contains(Context.Service)) InterfaceInvokeException.ThrowNoAccessException();
        }

        /// <summary>
        /// 创建请求处理器
        /// </summary>
        public static RequestProcessor CreateProcessor(RequestContext context)
        {
            RequestProcessor processor = null;
            if (context.Service == "PNRImportWithoutPat")
            {
                processor = new PNRImportWithoutPat();
            }
            else if (context.Service == "PNRImport")
            {
                processor = new PNRImport();
            }
            else if (context.Service == "ProduceOrder")
            {
                processor = new ProduceOrder();
            }
            else if (context.Service == "ProduceOrder2")
            {
                processor = new ProduceOrder2();
            }
            else if (context.Service == "ApplyRefund")
            {
                processor = new ApplyRefund();
            }
            else if (context.Service == "ApplyPostpone")
            {
                processor = new ApplyPostpone();
            }
            else if (context.Service == "OrderPay")
            {
                processor = new PayOrder();
            }
            else if (context.Service == "PayOrderByPayType")
            {
                processor = new PayOrderByPayType();
            }
            else if (context.Service == "PayApplyform")
            {
                processor = new PayApplyform();
            }
            else if (context.Service == "PayApplyformByPayType")
            {
                processor = new PayApplyformByPayType();
            }
            else if (context.Service == "QueryOrder")
            {
                processor = new QueryOrder();
            }
            else if (context.Service == "QueryApplyform")
            {
                processor = new QueryApplyform();
            }
            else if (context.Service == "QueryFlights")
            {
                processor = new QueryFlights();
            }
            else if (context.Service == "QueryFlightStop")
            {
                processor = new QueryFlightStop();
            }
            else if (context.Service == "QueryFlight")
            {
                processor = new QueryFlight();
            }
            else if (context.Service == "AutoPay")
            {
                processor = new AutoPay();
            }
            else if (context.Service == "ManualPay")
            {
                processor = new ManualPay();
            }
            if (processor != null)
            {
                processor.Context = context;
            }
            return processor;
        }
    }
}