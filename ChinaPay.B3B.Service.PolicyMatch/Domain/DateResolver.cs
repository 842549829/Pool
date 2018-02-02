using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ChinaPay.B3B.Service.PolicyMatch.Domain {
    internal static class DateResolver {
        private const string WeekPattern = @"[1-7]";
        private const string DatePattern = @"\d{4}[-/年]?\d{2}[-/月]?\d{2}日?";

        //private static readonly Regex Week = new Regex("^" + WeekPattern + "$", RegexOptions.Compiled);
        //private static readonly Regex WeekScope = new Regex("^(?<start>" + WeekPattern + ")-(?<end>" + WeekPattern + ")$");
        private static readonly Regex Date = new Regex("^" + DatePattern + "$", RegexOptions.Compiled);
        private static readonly Regex DateScope = new Regex("^(?<start>" + DatePattern + ")-(?<end>" + DatePattern + ")$", RegexOptions.Compiled);
        private static IEnumerable<T> MakeSequence<T>(T start, T end, Func<T, T> increasor) where T : IComparable<T> {
            if(start.CompareTo(end) >= 0) {
                var tmp = start;
                start = end;
                end = tmp;
            }

            do {
                yield return start;
            } while(end.CompareTo(start = increasor(start)) >= 0);
        }
        //private static IEnumerable<int> ResolveWeekScope(string weekScope) {
        //    var m = WeekScope.Match(weekScope);
        //    if(m.Success) {
        //        var start = int.Parse(m.Groups["start"].Value);
        //        var end = int.Parse(m.Groups["end"].Value);
        //        return MakeSequence(start, end, w => w + 1);
        //    }
        //    return new int[] { };
        //}
        private static IEnumerable<DateTime> ResolveDateScope(string dateScope) {
            var m = DateScope.Match(dateScope);
            if(m.Success) {
                var start = ResoleDate(m.Groups["start"].Value);
                var end = ResoleDate(m.Groups["end"].Value);
                return MakeSequence(start, end, d => d.AddDays(1));
            }
            return new DateTime[] { };
        }
        private static DateTime ResoleDate(string dateString) {
            if(dateString.Length == 8)
                return DateTime.Parse(string.Format("{0}-{1}-{2}", dateString.Substring(0, 4), dateString.Substring(4, 2), dateString.Substring(6, 2)));
            return DateTime.Parse(dateString);
        }

        /// <summary>
        /// 获取日期列表
        /// </summary>
        public static IEnumerable<DateTime> GetDates(DateTime start, DateTime end, string dateFilter, string filterSeparator) {
            var result = new List<DateTime>();
            if(!string.IsNullOrWhiteSpace(dateFilter)) {
                var dates = MakeSequence(start, end, d => d.AddDays(1));
                var filters = dateFilter.Split(new[] { filterSeparator }, StringSplitOptions.RemoveEmptyEntries);
                foreach(var filter in filters) {
                    try {
                        if(DateScope.Match(filter).Success) {
                            var accepts = ResolveDateScope(filter);
                            result.AddRange(dates.Where(accepts.Contains));
                        } else if(Date.Match(filter).Success) {
                            var accept = ResoleDate(filter);
                            result.AddRange(dates.Where(d => d == accept));
                        }
                    } catch { }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取星期列表
        /// </summary>
        public static IEnumerable<DayOfWeek> GetWeeks(string weekFilter, string filterSeparator) {
            var result = new List<DayOfWeek>();
            var weeks = weekFilter.Split(new[] { filterSeparator }, StringSplitOptions.RemoveEmptyEntries);
            foreach(var week in weeks) {
                try {
                    result.Add((DayOfWeek)(int.Parse(week) % 7));
                } catch { }
            }
            return result;
        }
    }
}
