using System;
using System.Collections.Specialized;
using System.Xml;
using System.Text;
using System.Linq;
using ChinaPay.Core;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.Interface
{
    internal abstract class BaseProcessor
    {
        protected System.Web.HttpContext _context;
        private string _sign;
        private string _userName;
        private DataTransferObject.Organization.EmployeeDetailInfo _employee;
        private DataTransferObject.Organization.CompanyDetailInfo _company;
        private Service.Organization.Domain.ExternalInterfaceSetting _interfaceSetting;

        protected BaseProcessor(string userName, string sign)
        {
            _context = System.Web.HttpContext.Current;
            _userName = userName;
            _sign = sign;
        }

        /// <summary>
        /// 号工信息
        /// </summary>
        public DataTransferObject.Organization.EmployeeDetailInfo Employee
        {
            get
            {
                if (_employee == null)
                {
                    if (string.IsNullOrWhiteSpace(_userName)) throw new InterfaceInvokeException("1", "用户名");
                    _employee = Cache.EmployeeCenter.Instance[_userName];
                    if (_employee == null) { throw new InterfaceInvokeException("2"); }
                    if (!_employee.Enabled) { throw new InterfaceInvokeException("4"); }
                }
                return _employee;
            }
        }

        /// <summary>
        /// 单位信息
        /// </summary>
        public DataTransferObject.Organization.CompanyDetailInfo Company
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
        public Service.Organization.Domain.ExternalInterfaceSetting InterfaceSetting
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

        public XmlDocument Execute()
        {
            XmlDocument result;
            var interfaceName = GetType().Name;
            var remoteIP = AddressLocator.IPAddressLocator.GetRequestIP(_context.Request).ToString();
            Guid? organization = null;
            var organizationName = "";
            try
            {
                organization = Company.CompanyId;
                organizationName = Company.AbbreviateName;
                // 控制访问频率
                InvokeStatistic.Instance.Save(interfaceName, _userName, remoteIP);
                var externalInterface = ExternalInterfaceService.Query(Company.CompanyId);
                // 是否有权限访问
                if (externalInterface.BindIP != remoteIP) throw new InterfaceInvokeException("10");
                if (externalInterface.InterfaceInvokeMethod == null || !externalInterface.InterfaceInvokeMethod.Any()) throw new InterfaceInvokeException("10");
                if (!externalInterface.InterfaceInvokeMethod.Contains(interfaceName)) throw new InterfaceInvokeException("10");
                // 验证签名
                ValidateSign();
                // 验证业务参数
                ValidateBusinessParameters();
                //存储请求接口的域名 
                ChinaPay.B3B.Service.LogService.SaveTextLog("客户端信息：IP " + _context.Request.UserHostAddress + " 域名 " + _context.Request.UserHostName);
                // 执行接口处理
                result = FormatInvokeResult("0", string.Empty, ExecuteCore());
            }
            catch (InterfaceInvokeException iie)
            {
                result = FormatInvokeResult(iie.Code, iie.Parameter, string.Empty);
            }
            catch (CustomException ex)
            {
                result = FormatInvokeResult("9", ex.Message, string.Empty);
            }
            catch (Exception ex)
            {
                Service.LogService.SaveExceptionLog(ex, "调用接口" + interfaceName);
                result = FormatInvokeResult("99", string.Empty, string.Empty);
            }
            var businessParametersText = new StringBuilder();
            var businessParameters = GetBusinessParameterCollection();
            foreach (var key in businessParameters.AllKeys)
            {
                businessParametersText.AppendFormat("&{0}={1}", key, businessParameters.Get(key));
            }
            // 记录访问日志
            var invokeLog = new Service.Log.Domain.InterfaceInvokeLog
            {
                InterfaceName = interfaceName,
                UserName = _userName,
                Organization = organization,
                OrganizationName = organizationName,
                IPAddress = remoteIP,
                Request = string.Format("userName={0}&sign={1}{2}", _userName, _sign, businessParametersText.ToString()),
                Response = result.InnerXml,
                Time = DateTime.Now
            };
            Service.LogService.SaveInterfaceInvokeLog(invokeLog);
            return result;
        }
        private void ValidateSign()
        {
            var basicParameters = GetBasicParameters();
            var businessParameters = GetBusinessParameters();
            //发布在外网上需要打开这个验证
            if (Utility.MD5EncryptorService.MD5(basicParameters + businessParameters).ToUpper() != _sign) throw new InterfaceInvokeException("3");
        }
        private XmlDocument FormatInvokeResult(string code, string parameter, string result)
        {
            var doc = new XmlDocument();
            var message = Message.Get(code, parameter);
            doc.LoadXml(string.Format("<b3b><code>{0}</code><message>{1}</message>{2}</b3b>", code, message, result));
            return doc;
        }

        protected virtual string GetBasicParameters()
        {
            return _userName + InterfaceSetting.SecurityCode;
        }
        protected string GetBusinessParameters()
        {
            var result = new StringBuilder();
            var nonBusinessKey = new[] { "userName", "sign" };
            var parameterCollection = GetBusinessParameterCollection();
            foreach (var pKey in parameterCollection.AllKeys)
            {
                if (!nonBusinessKey.Contains(pKey))
                {
                    result.Append(parameterCollection.Get(pKey));
                }
            }
            return result.ToString();
        }

        protected abstract NameValueCollection GetBusinessParameterCollection();

        protected abstract void ValidateBusinessParameters();
        protected abstract string ExecuteCore();
    }
}