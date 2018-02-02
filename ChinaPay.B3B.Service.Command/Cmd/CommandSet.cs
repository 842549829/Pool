using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Service.Command.Interface;
using ChinaPay.B3B.Service.Command.XapiServiceReference;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Command
{
    /// <summary>
    /// 指令集合类
    /// </summary>
    public class CommandSet : IExecutable
    {
        /// <summary>
        /// 命令字串
        /// </summary>
        public string CommandString { get { return this.commandString; } }

        /// <summary>
        /// 返回字串
        /// </summary>
        public string ReturnString { get { return RegexUtil.FormatReturnValue(this.returnString); } }

        /// <summary>
        /// 执行命令
        /// </summary>
        public void Execute()
        {
            if (this.commands.Count == 0)
            {
                this.returnString = "执行失败，指令集中未包含任何指令。";
            }

            this.commandString = this.commandString.TrimEnd(this.separator.ToCharArray());

            EndpointAddress address = new EndpointAddress(System.Configuration.ConfigurationManager.AppSettings["CommandServerAddress"]);
            //EndpointAddress address = new EndpointAddress("net.tcp://116.55.243.38:9010/XAPI/");
            //EndpointAddress address = new EndpointAddress("net.tcp://localhost:9010/XAPI/");
            
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            using (ChannelFactory<IXapiService> channelFactory = new ChannelFactory<IXapiService>(binding, address))
            {
                IXapiService xapiService = channelFactory.CreateChannel();

                using (xapiService as IDisposable)
                {
                    // this.returnString = proxy.GetMessage(commandString, lockfirst, flag, returnType != ReturnResultType.Single ? 1 : 0, 1, transactionId);
                    try
                    {
                        this.returnString = WCFUtil.Invoke<IXapiService, string>(
                        xapiService,
                        proxy => proxy.GetMessage(commandString, (byte)commandType, lockfirst, flag, (byte)returnType, transactionId, 1)
                            
                        );
                    }
                    catch (CustomException ex)
                    {
                        this.returnString = ex.Message;
                    }
                }
            }
        }
        
        /// <summary>
        /// 用于执行不带有事务性的多指令操作。
        /// </summary>
        /// <param name="commands"></param>
        public CommandSet(CommandType commandType, ExecuteMothodType commandSetType, ReturnResultType returnType)
        {
            this.commandType = commandType;
            this.executeType = commandSetType;
            this.returnType = returnType;
            Initialize();
        }

        public CommandSet(CommandType commandType, ExecuteMothodType commandSetType, ReturnResultType returnType, string transactionId)
        {
            this.commandType = commandType;
            this.executeType = commandSetType;
            this.returnType = returnType;
            this.transactionId = transactionId;
            Initialize();
        }
        
        public void Add(Command command)
        {
            this.commands.Add(command);
            this.commandString += command.CommandString + this.separator;
        }
        
        private void Initialize()
        {
            this.commands = new List<Command>();
            this.commandString = "";
            switch (executeType)
            {
                case ExecuteMothodType.SingleBatch:
                    this.separator = "|";
                    break;
                case ExecuteMothodType.MultipleBatch:
                    this.separator = ";";
                    break;
                default:
                    break;
            }
            this.lockfirst = 1;
            this.flag = 0;
        }

        // 命令集合；
        private List<Command> commands;
        // 命令字串；
        private string commandString;
        // 返回值字串；
        private string returnString;
        // 命令类型；
        private ExecuteMothodType executeType;
        // 是否返回所有结果；
        private ReturnResultType returnType;
        // 是否先锁定；
        private byte lockfirst;
        // 标识行程单的取消和作废；
        private byte flag;
        // 事务编号；
        private string transactionId;
        // 分隔符；
        private string separator;
        // 命令集类型
        private CommandType commandType;
    }
}
