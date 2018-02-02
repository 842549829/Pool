using System;
using System.Collections.Generic;
using System.Text;

namespace ChinaPay.B3B.Remind.Client.Command {
    class CommandResult<TResponse> {
        public bool Success {
            get;
            internal set;
        }

        public string ErrorMessage {
            get;
            internal set;
        }

        public TResponse Response {
            get;
            internal set;
        }
    }
}