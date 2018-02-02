using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ChinaPay.B3B.Service.CommandBuilder.Domain
{
    /// <summary>
    /// 指令集合类
    /// </summary>
    public class CommandSet
    {
        /// <summary>
        /// 命令字串
        /// </summary>
        public string CommandString
        {
            get
            {
                IEnumerable<string> temp = (from c in Commands
                                            select c.CommandString);
                return string.Join(_separator, temp);
            }
        }

        /// <summary>
        /// 命令集合
        /// </summary>
        public List<Command> Commands { get; private set; }

        public CommandSet()
        {
            Initialize();
        }

        public void Add(Command command)
        {
            Commands.Add(command);
        }

        private void Initialize()
        {
            _separator = "\r";
            Commands = new List<Command>();
        }

        private string _separator;
    }
}
