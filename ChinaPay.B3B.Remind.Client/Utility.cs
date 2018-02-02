using System.Collections.Generic;
using System.Text;

namespace ChinaPay.B3B.Remind.Client {
    class Utility {
        public static string Join(IEnumerable<string> source, string seperator) {
            return Join<string>(source, seperator, item => item);
        }
        public static string Join<T>(IEnumerable<T> source, string seperator, Func<T> selector) {
            if(source == null) return string.Empty;
            var result = new StringBuilder();
            foreach(var item in source) {
                if(item != null) {
                    result.Append(selector(item));
                    result.Append(seperator);
                }
            }
            if(result.Length > 0) {
                result.Remove(result.Length - 1, 1);
            }
            return result.ToString();
        }
    }
    delegate string Func<in T>(T source);
}