using System;

namespace ChinaPay.B3B.Interface {
    public class InterfaceInvokeException : Exception {
        public InterfaceInvokeException(string code)
            : this(code, string.Empty) {
        }
        public InterfaceInvokeException(string code, string parameter) {
            Code = code;
            Parameter = parameter;
        }

        public string Code { get; private set; }

        public string Parameter { get; private set; }

        internal static void ThrowParameterMissException(string parameter) {
            ThrowException("8", parameter);
        }
        internal static void ThrowParameterErrorException(string parameter) {
            ThrowException("1", parameter);
        }
        internal static void ThrowCustomMsgException(string msg)
        {
            ThrowException("9", msg);
        }
        internal static void ThrowNotFindPolicyException()
        {
            ThrowException("11");
        }
        internal static void ThrowSignErrorException() {
            ThrowException("3");
        }
        internal static void ThrowNoAccessException() {
            ThrowException("10");
        }
        internal static void ThrowException(string code) {
            ThrowException(code, string.Empty);
        }
        internal static void ThrowException(string code, string parameter) {
            throw new InterfaceInvokeException(code, parameter);
        }
    }
}