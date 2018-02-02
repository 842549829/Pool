using System;
using System.ServiceModel;
using ChinaPay.B3B.Service.Command.Interface;
using ChinaPay.B3B.Service.Command.XapiServiceReference;
using System.Text.RegularExpressions;
using System.ServiceModel.Description;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Command
{
    /// <summary>
    /// 航信指令父类
    /// </summary>
    public abstract class Command : IExecutable
    {
        /// <summary>
        /// 事务编号
        /// </summary>
        public string TransactonId
        {
            get { return transactionId; }
        }

        /// <summary>
        /// 命令字串
        /// </summary>
        public string CommandString { get { return this.commandString; } }

        /// <summary>
        /// 返回字串
        /// </summary>
        public string ReturnString
        {
            get { return RegexUtil.FormatReturnValue(this.returnString); }
        }

        /// <summary>
        /// 事务编号
        /// </summary>
        public CommandType CommandType
        {
            get { return commandType; }
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        public void Execute()
        {
            EndpointAddress address = new EndpointAddress(System.Configuration.ConfigurationManager.AppSettings["CommandServerAddress"]);
            //EndpointAddress address = new EndpointAddress("net.tcp://192.168.1.253:9010/XAPI/");
            //EndpointAddress address = new EndpointAddress("net.tcp://localhost:9010/XAPI/");

            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            using (ChannelFactory<IXapiService> channelFactory = new ChannelFactory<IXapiService>(binding,address))
            {
                IXapiService xapiService = channelFactory.CreateChannel();

                using (xapiService as IDisposable)
                {
                    try
                    {
                        this.returnString = WCFUtil.Invoke<IXapiService, string>(
                        xapiService,
                        proxy => proxy.GetMessage(commandString, (byte)commandType,  0, 0, (byte)returnType, transactionId, 1)
                        );
                    }
                    catch (CustomException ex)
                    {
                        this.returnString = ex.Message;
                    }
                }
            }
        }

        // 事务编号
        protected string transactionId;
        // 命令字串
        protected string commandString;
        // 结果字串
        protected string returnString;
        // 返回类型
        protected ReturnResultType returnType = ReturnResultType.Single;
        // 命令类型
        protected CommandType commandType;
    }
}
