using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Command.PNR
{
    public class MLCommand : Command
    {
        public MLCommand()
        {
            this.commandString = string.Format("ML:{0}/");
            Initialize();
        }
        
        private void Initialize()
        {
            this.commandType = CommandType.PNRModification;
        }

        //private char option;
        //private string flightNo;
        //private DateTime date;
    }
}
