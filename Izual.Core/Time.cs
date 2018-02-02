#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Core.Time.cs
// description：
// 
// create by：Izual ,2012/06/11
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Izual {
    using System.ComponentModel;

    /// <summary>
    /// 用于表示时间的结构
    /// </summary>
    public struct Time2 : IComparable<Time2>, IEquatable<Time2>, ISerializable
    {
        private const long TicksPerMillisecond = 10000L;
        private const long TicksPerSecond = 10000000L;
        private const long TicksPerMinute = 600000000L;
        private const long TicksPerHour = 36000000000L;
        private const long MaxTicks = 863999999999L;
        private const long MinTicks = 0L;
        private static readonly Regex timeFormat;
        private static readonly Regex[] parts;
        private static readonly Func<Time2, string, string>[] partPickers;

        /// <summary>
        /// Time 结构能接受的最大值
        /// </summary>
        public static readonly Time2 MaxValue = new Time2(23, 59, 59, 999, 9999);

        /// <summary>
        /// Time 结构能接受的最小值
        /// </summary>
        public static readonly Time2 MinValue = new Time2(0, 0, 0);

        private int billisecond;
        private int hour;
        private int millisecond;
        private int minute;
        private int second;

        static Time2() {
            // 匹配的格式：
            // 23:59:59
            // 23:59:59 999
            // 23:59:59 999 9999
            timeFormat = new Regex(@"^(am|pm)?\s+(?<hour>\d{1,2})(:|：)(?<minute>\d{1,2})(:|：)(?<second>\d{1,2})((\+|\s+)(?<millisecond>\d{1,3})(\s+(?<billisecond>\d{1,4}))?)?$",
                                   RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
            // [\u2E80-\u9FFF]+$
            parts = new[] {
                new Regex(@"h+", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled),
                new Regex(@"m+", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled),
                new Regex(@"s+", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled),
                new Regex(@"f+", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled),
                new Regex(@"b+", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled)
            };

            //
            partPickers = new Func<Time2, string, string>[] {
                (time, part) => string.Format(GetFormat(part), part.All(char.IsUpper) ? time.hour : (time.hour > 12 ? time.hour - 12 : time.hour)),
                (time, part) => string.Format(GetFormat(part), time.minute), (time, part) => string.Format(GetFormat(part), time.second),
                (time, part) => string.Format(GetFormat(part), time.millisecond), (time, part) => string.Format(GetFormat(part), time.billisecond)
            };
        }

        /// <summary>
        /// 用指定的 ticks 初始化 Time 结构的新实例
        /// </summary>
        /// <param name="ticks"> </param>
        public Time2(long ticks) {
            if(ticks > MaxTicks || ticks < 0)
                throw new ArgumentOutOfRangeException("ticks");

            hour = (int)(ticks / TicksPerHour);
            minute = (int)((ticks - hour * TicksPerHour) / TicksPerMinute);
            second = (int)((ticks - hour * TicksPerHour - minute * TicksPerMinute) / TicksPerSecond);
            millisecond = (int)((ticks - hour * TicksPerHour - minute * TicksPerMinute - second * TicksPerSecond) / TicksPerMillisecond);
            billisecond = (int)(ticks - hour * TicksPerHour - minute * TicksPerMinute - second * TicksPerSecond - millisecond * TicksPerMillisecond);
        }

        /// <summary>
        /// 用指定的 小时，分钟，秒钟 初始化 Time 结构的新实例
        /// </summary>
        /// <param name="hour"> 小时 </param>
        /// <param name="minute"> 分钟 </param>
        /// <param name="second"> 秒 </param>
        public Time2(int hour, int minute, int second) {
            if(hour > 23 || hour < 0) throw new ArgumentOutOfRangeException("hour", "小时取值超出范围。");
            if(minute > 59 || minute < 0) throw new ArgumentOutOfRangeException("minute", "分钟取值超出范围。");
            if(second > 59 || second < 0) throw new ArgumentOutOfRangeException("second", "秒钟取值超出范围。");

            this.hour = hour;
            this.minute = minute;
            this.second = second;
            millisecond = 0;
            billisecond = 0;
        }

        /// <summary>
        /// 用指定的 小时，分钟，秒钟，毫秒 初始化 Time 结构的新实例
        /// </summary>
        /// <param name="hour"> 小时 </param>
        /// <param name="minute"> 分钟 </param>
        /// <param name="second"> 秒 </param>
        /// <param name="millisecond"> 毫秒 </param>
        public Time2(int hour, int minute, int second, int millisecond) {
            if(hour > 23 || hour < 0) throw new ArgumentOutOfRangeException("hour", "小时取值超出范围。");
            if(minute > 59 || minute < 0) throw new ArgumentOutOfRangeException("minute", "分钟取值超出范围。");
            if(second > 59 || second < 0) throw new ArgumentOutOfRangeException("second", "秒钟取值超出范围。");
            if(millisecond > 999 || second < 0) throw new ArgumentOutOfRangeException("millisecond", "毫秒取值超出范围。");

            this.hour = hour;
            this.minute = minute;
            this.second = second;
            this.millisecond = millisecond;
            billisecond = 0;
        }

        /// <summary>
        /// 用指定的 小时，分钟，秒钟，毫秒，毫微秒 初始化 Time 结构的新实例
        /// </summary>
        /// <param name="hour"> 小时 </param>
        /// <param name="minute"> 分钟 </param>
        /// <param name="second"> 秒 </param>
        /// <param name="millisecond"> 毫秒 </param>
        /// <param name="billisecond"> 毫微秒 </param>
        public Time2(int hour, int minute, int second, int millisecond, int billisecond) {
            if(hour > 23 || hour < 0) throw new ArgumentOutOfRangeException("hour", "小时取值超出范围。");
            if(minute > 59 || minute < 0) throw new ArgumentOutOfRangeException("minute", "分钟取值超出范围。");
            if(second > 59 || second < 0) throw new ArgumentOutOfRangeException("second", "秒钟取值超出范围。");
            if(millisecond > 999 || millisecond < 0) throw new ArgumentOutOfRangeException("millisecond", "毫秒取值超出范围。");
            if(billisecond > 9999 || billisecond < 0) throw new ArgumentOutOfRangeException("billisecond", "毫微秒钟取值超出范围。");

            this.hour = hour;
            this.minute = minute;
            this.second = second;
            this.millisecond = millisecond;
            this.billisecond = billisecond;
        }

        /// <summary>
        /// 获取或设置小时
        /// </summary>
        public int Hour {
            get { return hour; }
            set {
                if(value > 23 || value < 0)
                    throw new ArgumentOutOfRangeException("value", "小时取值超出范围。");
                hour = value;
            }
        }

        /// <summary>
        /// 获取或设置分钟
        /// </summary>
        public int Minute {
            get { return minute; }
            set {
                if(value > 59 || value < 0)
                    throw new ArgumentOutOfRangeException("value", "分钟取值超出范围。");
                minute = value;
            }
        }

        /// <summary>
        /// 获取或设置秒钟
        /// </summary>
        public int Second {
            get { return second; }
            set {
                if(value > 59 || value < 0)
                    throw new ArgumentOutOfRangeException("value", "秒钟取值超出范围。");
                second = value;
            }
        }

        /// <summary>
        /// 获取或设置毫秒
        /// </summary>
        public int Millisecond {
            get { return second; }
            set {
                if(value > 999 || value < 0)
                    throw new ArgumentOutOfRangeException("value", "毫秒取值超出范围。");
                millisecond = value;
            }
        }

        /// <summary>
        /// 获取或设置毫微秒
        /// </summary>
        public int Billisecond {
            get { return billisecond; }
            set {
                if(value > 9999 || value < 0)
                    throw new ArgumentOutOfRangeException("value", "毫微秒钟取值超出范围。");
                billisecond = value;
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        public long Ticks {
            get { return hour * TicksPerHour + minute * TicksPerMinute + second * TicksPerSecond + millisecond * TicksPerMillisecond + billisecond; }
        }

        /// <summary>
        /// 获取当前时间
        /// </summary>
        public static Time2 Now {
            get {
                DateTime now = DateTime.Now;
                var tmp = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);
                return new Time2(now.Hour, now.Minute, now.Second, now.Millisecond, (int)(now.Ticks - tmp.Ticks));
            }
        }

        #region IComparable<Time2> Members

        /// <summary>
        /// 比较当前与参数指定的时间
        /// </summary>
        /// <param name="other"> 与当前时间做比较的时间 </param>
        /// <returns> 如果当前时间，大于 other 返回 1；小于 other 返回 -1；等于 other 返回 0 </returns>
        public int CompareTo(Time2 other) {
            return Ticks.CompareTo(other.Ticks);
        }

        #endregion

        #region IEquatable<Time2> Members

        /// <summary>
        /// 比较当前时间与参数指定的时间是否相等
        /// </summary>
        /// <param name="other"> 与当前时间做比较的时间 </param>
        /// <returns> 相等返回 true，否则返回 false </returns>
        public bool Equals(Time2 other) {
            return Ticks.Equals(other.Ticks);
        }

        #endregion

        #region ISerializable Members

        /// <summary>
        /// 获取对象数据
        /// </summary>
        /// <param name="info"> 序列化信息 </param>
        /// <param name="context"> </param>
        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            if(info == null)
                throw new ArgumentNullException("info");

            if(info == null)
                throw new ArgumentNullException("info");
            info.AddValue("ticks", Ticks);
            info.AddValue("timeData", Ticks);
            //info.AddValue("dateData", this.dateData);
        }

        #endregion

        private static string GetFormat(string part) {
            return "{0:d" + part.Length + "}";
        }

        /// <summary>
        /// 将指定的字符串转换为 Time 结构的实例
        /// </summary>
        /// <param name="str"> 要转换的字符串 </param>
        /// <returns> 返回成功转换的实例 </returns>
        public static Time2 Parse(string str) {
            if(string.IsNullOrWhiteSpace(str))
                throw new ArgumentNullException("str");

            Match match = timeFormat.Match(str);
            if(!match.Success)
                throw new ArgumentException("时间格式不匹配。");

            int hour, minute, second, millisecond = 0, billisecond = 0;

            if(int.TryParse(match.Groups["hour"].Value, out hour) && int.TryParse(match.Groups["minute"].Value, out minute) && int.TryParse(match.Groups["second"].Value, out second)) {
                if(hour > 23 || hour < 0)
                    throw new InvalidOperationException("小时取值超出范围。");
                if(minute > 59 || minute < 0)
                    throw new InvalidOperationException("分钟取值超出范围。");
                if(second > 59 || second < 0)
                    throw new InvalidOperationException("秒钟取值超出范围。");

                string ms = match.Groups["millisecond"].Value;
                if(string.IsNullOrWhiteSpace(ms) || int.TryParse(ms, out millisecond)) {
                    if(millisecond > 999 || millisecond < 0)
                        throw new InvalidOperationException("毫秒取值超出范围。");
                }

                string bs = match.Groups["billisecond"].Value;
                if(string.IsNullOrWhiteSpace(bs) || int.TryParse(bs, out billisecond)) {
                    if(billisecond > 9999 || billisecond < 0)
                        throw new InvalidOperationException("毫微秒钟取值超出范围。");
                }

                return new Time2(hour, minute, second, millisecond, billisecond);
            }
            throw new ArgumentException("时间格式不匹配。");
        }

        /// <summary>
        /// 将指定的字符串转换为 Time 结构的实例
        /// </summary>
        /// <param name="str"> 要转换的字符串 </param>
        /// <param name="time"> 输出参数，转换得到的 Time 结构实例 </param>
        /// <returns> 返回转换是否成功 </returns>
        public static bool TryParse(string str, out Time2 time) {
            time = new Time2();
            if(string.IsNullOrWhiteSpace(str))
                return false;

            Match match = timeFormat.Match(str);
            if(!match.Success)
                return false;

            int hour, minute, second, millisecond = 0, billisecond = 0;

            if(int.TryParse(match.Groups["hour"].Value, out hour) && int.TryParse(match.Groups["minute"].Value, out minute) && int.TryParse(match.Groups["second"].Value, out second)) {
                if(hour > 23 || hour < 0 || minute > 59 || minute < 0 || second > 59 || second < 0)
                    return false;

                string ms = match.Groups["millisecond"].Value;
                if(string.IsNullOrWhiteSpace(ms) && int.TryParse(ms, out millisecond)) {
                    if(millisecond > 999 || millisecond < 0)
                        return false;
                }

                string bs = match.Groups["billisecond"].Value;
                if(string.IsNullOrWhiteSpace(bs) || int.TryParse(bs, out billisecond)) {
                    if(billisecond > 9999 || billisecond < 0)
                        return false;
                }

                time = new Time2(hour, minute, second, millisecond, billisecond);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 定义 DateTime 结构到 Time 结构的隐式转换
        /// </summary>
        /// <param name="dt"> 要转换到 Time 的 DateTime </param>
        /// <returns> 返回与 DateTime 表示的时间部分相等的 Time 结构实例 </returns>
        public static implicit operator Time2(DateTime dt) {
            var tmp = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);

            return new Time2(dt.Hour, dt.Minute, dt.Second, dt.Millisecond, (int)(dt.Ticks - tmp.Ticks));
        }

        /// <summary>
        /// 重载二元运算符 "+" ，将时间加上一个时间间隔
        /// </summary>
        /// <param name="time"> Time 结构操作数 </param>
        /// <param name="span"> TimeSpan 结构操作数 </param>
        /// <returns> 返回相加后的时间 </returns>
        public static Time2 operator +(Time2 time, TimeSpan span) {
            long ticks = time.Ticks + span.Ticks;
            if(ticks > MaxTicks)
                throw new OverflowException();

            return new Time2(ticks);
        }

        /// <summary>
        /// 重载二元运算符 "-" ，将时间减去一个时间间隔
        /// </summary>
        /// <param name="time"> Time 结构操作数 </param>
        /// <param name="span"> TimeSpan 结构操作数 </param>
        /// <returns> 返回相减后的时间 </returns>
        public static Time2 operator -(Time2 time, TimeSpan span) {
            long ticks = time.Ticks - span.Ticks;
            if(ticks < MinTicks)
                throw new OverflowException();

            return new Time2(ticks);
        }

        /// <summary>
        /// 重载二元运算符 "=="，判断两个时间相等
        /// </summary>
        /// <param name="time1"> 参与比较的第一个 Time 类型操作数 </param>
        /// <param name="time2"> 参与比较的第二个 Time 类型操作数 </param>
        /// <returns> 相等返回 true，否则返回 false </returns>
        public static bool operator ==(Time2 time1, Time2 time2) {
            return time1.Ticks == time2.Ticks;
        }

        /// <summary>
        /// 重载二元运算符 "!="，判断两个时间相等
        /// </summary>
        /// <param name="time1"> 参与比较的第一个 Time 类型操作数 </param>
        /// <param name="time2"> 参与比较的第二个 Time 类型操作数 </param>
        /// <returns> 不相等返回 true，否则返回 false </returns>
        public static bool operator !=(Time2 time1, Time2 time2) {
            return time1.Ticks != time2.Ticks;
        }

        /// <summary>
        /// 重载二元运算符 "&gt;"，判断第一个时间是否大于第二个时间
        /// </summary>
        /// <param name="time1"> 参与比较的第一个 Time 类型操作数 </param>
        /// <param name="time2"> 参与比较的第二个 Time 类型操作数 </param>
        /// <returns> 如果第一个 Time 大于第二个 Time 返回 true，否则返回 false </returns>
        public static bool operator >(Time2 time1, Time2 time2) {
            return time1.Ticks > time2.Ticks;
        }

        /// <summary>
        /// 重载二元运算符 "&lt;"，判断第一个时间是否小于第二个时间
        /// </summary>
        /// <param name="time1"> 参与比较的第一个 Time 类型操作数 </param>
        /// <param name="time2"> 参与比较的第二个 Time 类型操作数 </param>
        /// <returns> 如果第一个 Time 小于第二个 Time 返回 true，否则返回 false </returns>
        public static bool operator <(Time2 time1, Time2 time2) {
            return time1.Ticks < time2.Ticks;
        }

        /// <summary>
        /// 重载二元运算符 "&gt;="，判断第一个时间是否大于等于第二个时间
        /// </summary>
        /// <param name="time1"> 参与比较的第一个 Time 类型操作数 </param>
        /// <param name="time2"> 参与比较的第二个 Time 类型操作数 </param>
        /// <returns> 如果第一个 Time 大于等于第二个 Time 返回 true，否则返回 false </returns>
        public static bool operator >=(Time2 time1, Time2 time2) {
            return time1.Ticks >= time2.Ticks;
        }

        /// <summary>
        /// 重载二元运算符 "&lt;="，判断第一个时间是否小于等于第二个时间
        /// </summary>
        /// <param name="time1"> 参与比较的第一个 Time 类型操作数 </param>
        /// <param name="time2"> 参与比较的第二个 Time 类型操作数 </param>
        /// <returns> 如果第一个 Time 小于等于第二个 Time 返回 true，否则返回 false </returns>
        public static bool operator <=(Time2 time1, Time2 time2) {
            return time1.Ticks <= time2.Ticks;
        }

        /// <summary>
        /// 计算当前 Time 结构的 Hash 值
        /// </summary>
        /// <returns> 返回 Ticks 的 HashCode </returns>
        public override int GetHashCode() {
            return Ticks.GetHashCode();
        }

        /// <summary>
        /// 比较指定对象是否与当前时间相等
        /// </summary>
        /// <param name="obj"> 要比较的对象 </param>
        /// <returns> </returns>
        public override bool Equals(object obj) {
            return obj is Time2 && Ticks == ((Time2)obj).Ticks;
        }

        /// <summary>
        /// 按指定格式输出时间字符串
        /// </summary>
        /// <returns> </returns>
        public string ToString(string format) {
            if(parts.Length == 0 || partPickers.Length == 0)
                return base.ToString();

            Time2 self = this;
            for(int i = 0; i < parts.Length; i++) {
                format = parts[i].Replace(format, m => partPickers[i](self, m.Value));
            }
            return format;
        }

        /// <summary>
        /// 输出 "HH:mm:ss" 格式的时间字符串
        /// </summary>
        /// <returns> </returns>
        public override string ToString() {
            return ToString("HH:mm:ss");
        }
    }

    /// <summary>
    /// 用于表示时间的结构
    /// </summary>
    public struct Time : IComparable<Time>, IEquatable<Time>, ISerializable,IConvertible {
        private const int TicksPerSecond = 1000;
        private const int TicksPerMinute = 60000;
        private const int TicksPerHour = 3600000;
        private const int MaxTicks = 86399999;
        private const int MinTicks = 0;
        private static readonly Regex[] parts;
        private static readonly Func<Time, string, string>[] partPickers;

        private static readonly Regex timeFormat = new Regex(@"^(?<hour>\d{1,2}):(?<minute>\d{1,2}):(?<second>\d{1,2})(\s+(?<millisecond>\d{1,3}))?$",
                                                             RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

        /// <summary>
        /// Time 结构能接受的最大值
        /// </summary>
        public static readonly Time MaxValue = new Time(23, 59, 59, 999);

        /// <summary>
        /// Time 结构能接受的最小值
        /// </summary>
        public static readonly Time MinValue = new Time(0, 0, 0);

        private int hour;
        private int millisecond;
        private int minute;
        private int second;

        static Time() {
            //    // 匹配的格式：
            //    // 23:59:59
            //    // 23:59:59 999
            //    timeFormat = new Regex(@"^(?<hour>\d{1,2}):(?<minute>\d{1,2}):(?<second>\d{1,2})(\s+(?<millisecond>\d{1,3})(\s+(?<billisecond>\d{1,4}))?)?$", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
            parts = new[] {
                new Regex(@"h+", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled),
                new Regex(@"m+", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled),
                new Regex(@"s+", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled),
                new Regex(@"f+", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled),
            };

            //
            partPickers = new Func<Time, string, string>[] {
                (time, part) => string.Format(GetFormat(part), part.All(char.IsUpper) ? time.hour : (time.hour > 12 ? time.hour - 12 : time.hour)),
                (time, part) => string.Format(GetFormat(part), time.minute), (time, part) => string.Format(GetFormat(part), time.second),
                (time, part) => string.Format(GetFormat(part), time.millisecond),
            };
        }

        /// <summary>
        /// 用指定的 ticks 初始化 Time 结构的新实例
        /// </summary>
        /// <param name="ticks"> </param>
        public Time(int ticks) {
            if(ticks > MaxTicks || ticks < 0)
                throw new ArgumentOutOfRangeException("ticks");

            hour = (int)(ticks / TicksPerHour);
            minute = (int)((ticks - hour * TicksPerHour) / TicksPerMinute);
            second = (int)((ticks - hour * TicksPerHour - minute * TicksPerMinute) / TicksPerSecond);
            millisecond = (int)(ticks - hour * TicksPerHour - minute * TicksPerMinute - second * TicksPerSecond);
        }

        /// <summary>
        /// 用指定的 小时，分钟，秒钟 初始化 Time 结构的新实例
        /// </summary>
        /// <param name="hour"> 小时 </param>
        /// <param name="minute"> 分钟 </param>
        /// <param name="second"> 秒 </param>
        public Time(int hour, int minute, int second) {
            if(hour > 23 || hour < 0) throw new ArgumentOutOfRangeException("hour", "小时取值超出范围。");
            if(minute > 59 || minute < 0) throw new ArgumentOutOfRangeException("minute", "分钟取值超出范围。");
            if(second > 59 || second < 0) throw new ArgumentOutOfRangeException("second", "秒取值超出范围。");

            this.hour = hour;
            this.minute = minute;
            this.second = second;
            millisecond = 0;
        }

        /// <summary>
        /// 用指定的 小时，分钟，秒钟，毫秒 初始化 Time 结构的新实例
        /// </summary>
        /// <param name="hour"> 小时 </param>
        /// <param name="minute"> 分钟 </param>
        /// <param name="second"> 秒 </param>
        /// <param name="millisecond"> 毫秒 </param>
        public Time(int hour, int minute, int second, int millisecond) {
            if(hour > 23 || hour < 0) throw new ArgumentOutOfRangeException("hour", "小时取值超出范围。");
            if(minute > 59 || minute < 0) throw new ArgumentOutOfRangeException("minute", "分钟取值超出范围。");
            if(second > 59 || second < 0) throw new ArgumentOutOfRangeException("second", "秒取值超出范围。");
            if(millisecond > 999 || second < 0) throw new ArgumentOutOfRangeException("millisecond", "毫秒取值超出范围。");

            this.hour = hour;
            this.minute = minute;
            this.second = second;
            this.millisecond = millisecond;
        }

        /// <summary>
        /// 获取或设置小时
        /// </summary>
        public int Hour {
            get { return hour; }
            set {
                if(value > 23 || value < 0)
                    throw new ArgumentOutOfRangeException("value", "小时取值超出范围。");
                hour = value;
            }
        }

        /// <summary>
        /// 获取或设置分钟
        /// </summary>
        public int Minute {
            get { return minute; }
            set {
                if(value > 59 || value < 0)
                    throw new ArgumentOutOfRangeException("value", "分钟取值超出范围。");
                minute = value;
            }
        }

        /// <summary>
        /// 获取或设置秒钟
        /// </summary>
        public int Second {
            get { return second; }
            set {
                if(value > 59 || value < 0)
                    throw new ArgumentOutOfRangeException("value", "秒取值超出范围。");
                second = value;
            }
        }

        /// <summary>
        /// 获取或设置毫秒
        /// </summary>
        public int Millisecond {
            get { return second; }
            set {
                if(value > 999 || value < 0)
                    throw new ArgumentOutOfRangeException("value", "毫秒取值超出范围。");
                millisecond = value;
            }
        }

        /// <summary>
        /// 获取表示当前 Time 的 tick 数
        /// </summary>
        public int Ticks {
            get { return hour * TicksPerHour + minute * TicksPerMinute + second * TicksPerSecond + millisecond; }
        }

        /// <summary>
        /// 获取当前时间
        /// </summary>
        public static Time Now {
            get {
                DateTime now = DateTime.Now;
                return new Time(now.Hour, now.Minute, now.Second, now.Millisecond);
            }
        }

        #region IComparable<Time> Members

        /// <summary>
        /// 比较当前与参数指定的时间
        /// </summary>
        /// <param name="other"> 与当前时间做比较的时间 </param>
        /// <returns> 如果当前时间，大于 other 返回 1；小于 other 返回 -1；等于 other 返回 0 </returns>
        public int CompareTo(Time other) {
            return Ticks.CompareTo(other.Ticks);
        }

        #endregion

        #region IEquatable<Time> Members

        /// <summary>
        /// 比较当前时间与参数指定的时间是否相等
        /// </summary>
        /// <param name="other"> 与当前时间做比较的时间 </param>
        /// <returns> 相等返回 true，否则返回 false </returns>
        public bool Equals(Time other) {
            return Ticks.Equals(other.Ticks);
        }

        #endregion

        #region ISerializable Members

        /// <summary>
        /// 获取对象数据
        /// </summary>
        /// <param name="info"> 序列化信息 </param>
        /// <param name="context"> </param>
        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            if(info == null)
                throw new ArgumentNullException("info");

            if(info == null)
                throw new ArgumentNullException("info");
            info.AddValue("timeDate", Ticks);
            //info.AddValue("dateData", this.dateData);
        }

        #endregion

        private static string GetFormat(string part) {
            return "{0:d" + part.Length + "}";
        }

        /// <summary>
        /// 将指定的字符串转换为 Time 结构的实例
        /// </summary>
        /// <param name="str"> 要转换的字符串 </param>
        /// <returns> 返回成功转换的实例 </returns>
        public static Time Parse(string str) {
            if(string.IsNullOrWhiteSpace(str))
                throw new ArgumentNullException("str");

            Match match = timeFormat.Match(str);
            if(!match.Success)
                throw new ArgumentException("时间格式不匹配。");

            int hour, minute, second, millisecond = 0;

            if(int.TryParse(match.Groups["hour"].Value, out hour) && int.TryParse(match.Groups["minute"].Value, out minute) && int.TryParse(match.Groups["second"].Value, out second)) {
                if(hour > 23 || hour < 0)
                    throw new InvalidOperationException("小时取值超出范围。");
                if(minute > 59 || minute < 0)
                    throw new InvalidOperationException("分钟取值超出范围。");
                if(second > 59 || second < 0)
                    throw new InvalidOperationException("秒钟取值超出范围。");

                string ms = match.Groups["millisecond"].Value;
                if(string.IsNullOrWhiteSpace(ms) || int.TryParse(ms, out millisecond)) {
                    if(millisecond > 999 || millisecond < 0)
                        throw new InvalidOperationException("毫秒取值超出范围。");
                }

                return new Time(hour, minute, second, millisecond);
            }
            throw new ArgumentException("时间格式不匹配。");
        }

        /// <summary>
        /// 将指定的字符串转换为 Time 结构的实例
        /// </summary>
        /// <param name="str"> 要转换的字符串 </param>
        /// <param name="time"> 输出参数，转换得到的 Time 结构实例 </param>
        /// <returns> 返回转换是否成功 </returns>
        public static bool TryParse(string str, out Time time) {
            time = new Time();
            if(string.IsNullOrWhiteSpace(str))
                return false;

            Match match = timeFormat.Match(str);
            if(!match.Success)
                return false;

            int hour, minute, second, millisecond = 0;

            if(int.TryParse(match.Groups["hour"].Value, out hour) && int.TryParse(match.Groups["minute"].Value, out minute) && int.TryParse(match.Groups["second"].Value, out second)) {
                if(hour > 23 || hour < 0 || minute > 59 || minute < 0 || second > 59 || second < 0)
                    return false;

                string ms = match.Groups["millisecond"].Value;
                if(string.IsNullOrWhiteSpace(ms) && int.TryParse(ms, out millisecond)) {
                    if(millisecond > 999 || millisecond < 0)
                        return false;
                }

                time = new Time(hour, minute, second, millisecond);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 定义 DateTime 结构到 Time 结构的强制转换
        /// </summary>
        /// <param name="dt"> 要转换到 Time 的 DateTime </param>
        /// <returns> 返回与 DateTime 表示的时间部分相等的 Time 结构实例 </returns>
        public static explicit operator Time(DateTime dt) {
            return new Time(dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
        }

        /// <summary>
        /// Time 结构到 TimeSpan 结构的强制转换
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static explicit  operator TimeSpan(Time time) {
            return new TimeSpan(0, time.hour, time.minute, time.second, time.millisecond);
        }
        /// <summary>
        /// Time 结构到 Time2 结构的隐式转换
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static implicit operator Time2(Time time) {
            return new Time2(time.Ticks);
        }

        /// <summary>
        /// 重载二元运算符 "+" ，将时间加上一个时间间隔
        /// </summary>
        /// <param name="time"> Time 结构操作数 </param>
        /// <param name="span"> TimeSpan 结构操作数 </param>
        /// <returns> 返回相加后的时间 </returns>
        public static Time operator +(Time time, TimeSpan span) {
            var ticks = time.Ticks + span.Ticks;
            if(ticks > MaxTicks)
                throw new OverflowException();

            return new Time((int)ticks);
        }

        /// <summary>
        /// 重载二元运算符 "-" ，将时间减去一个时间间隔
        /// </summary>
        /// <param name="time"> Time 结构操作数 </param>
        /// <param name="span"> TimeSpan 结构操作数 </param>
        /// <returns> 返回相减后的时间 </returns>
        public static Time operator -(Time time, TimeSpan span) {
            long ticks = time.Ticks - span.Ticks;
            if(ticks < MinTicks)
                throw new OverflowException();

            return new Time((int)ticks);
        }

        /// <summary>
        /// 重载二元运算符 "=="，判断两个时间相等
        /// </summary>
        /// <param name="time1"> 参与比较的第一个 Time 类型操作数 </param>
        /// <param name="time2"> 参与比较的第二个 Time 类型操作数 </param>
        /// <returns> 相等返回 true，否则返回 false </returns>
        public static bool operator ==(Time time1, Time time2) {
            return time1.Ticks == time2.Ticks;
        }

        /// <summary>
        /// 重载二元运算符 "!="，判断两个时间相等
        /// </summary>
        /// <param name="time1"> 参与比较的第一个 Time 类型操作数 </param>
        /// <param name="time2"> 参与比较的第二个 Time 类型操作数 </param>
        /// <returns> 不相等返回 true，否则返回 false </returns>
        public static bool operator !=(Time time1, Time time2) {
            return time1.Ticks != time2.Ticks;
        }

        /// <summary>
        /// 重载二元运算符 "&gt;"，判断第一个时间是否大于第二个时间
        /// </summary>
        /// <param name="time1"> 参与比较的第一个 Time 类型操作数 </param>
        /// <param name="time2"> 参与比较的第二个 Time 类型操作数 </param>
        /// <returns> 如果第一个 Time 大于第二个 Time 返回 true，否则返回 false </returns>
        public static bool operator >(Time time1, Time time2) {
            return time1.Ticks > time2.Ticks;
        }

        /// <summary>
        /// 重载二元运算符 "&lt;"，判断第一个时间是否小于第二个时间
        /// </summary>
        /// <param name="time1"> 参与比较的第一个 Time 类型操作数 </param>
        /// <param name="time2"> 参与比较的第二个 Time 类型操作数 </param>
        /// <returns> 如果第一个 Time 小于第二个 Time 返回 true，否则返回 false </returns>
        public static bool operator <(Time time1, Time time2) {
            return time1.Ticks < time2.Ticks;
        }

        /// <summary>
        /// 重载二元运算符 "&gt;="，判断第一个时间是否大于等于第二个时间
        /// </summary>
        /// <param name="time1"> 参与比较的第一个 Time 类型操作数 </param>
        /// <param name="time2"> 参与比较的第二个 Time 类型操作数 </param>
        /// <returns> 如果第一个 Time 大于等于第二个 Time 返回 true，否则返回 false </returns>
        public static bool operator >=(Time time1, Time time2) {
            return time1.Ticks >= time2.Ticks;
        }

        /// <summary>
        /// 重载二元运算符 "&lt;="，判断第一个时间是否小于等于第二个时间
        /// </summary>
        /// <param name="time1"> 参与比较的第一个 Time 类型操作数 </param>
        /// <param name="time2"> 参与比较的第二个 Time 类型操作数 </param>
        /// <returns> 如果第一个 Time 小于等于第二个 Time 返回 true，否则返回 false </returns>
        public static bool operator <=(Time time1, Time time2) {
            return time1.Ticks <= time2.Ticks;
        }

        /// <summary>
        /// 计算当前 Time 结构的 Hash 值
        /// </summary>
        /// <returns> 返回 Ticks 的 HashCode </returns>
        public override int GetHashCode() {
            return Ticks.GetHashCode();
        }

        /// <summary>
        /// 比较指定对象是否与当前时间相等
        /// </summary>
        /// <param name="obj"> 要比较的对象 </param>
        /// <returns> </returns>
        public override bool Equals(object obj) {
            return obj is Time && Ticks == ((Time)obj).Ticks;
        }

        /// <summary>
        /// 按指定格式输出时间字符串
        /// </summary>
        /// <returns> </returns>
        public string ToString(string format) {
            if(parts.Length == 0 || partPickers.Length == 0)
                return base.ToString();

            Time self = this;
            for(int i = 0; i < parts.Length; i++) {
                format = parts[i].Replace(format, m => partPickers[i](self, m.Value));
            }
            return format;
        }

        /// <summary>
        /// 输出 "HH:mm:ss" 格式的时间字符串
        /// </summary>
        /// <returns> </returns>
        public override string ToString() {
            return ToString("HH:mm:ss");
        }

        #region Implementation of IConvertible
        /// <summary>
        /// 获取类型代码
        /// </summary>
        /// <returns></returns>
        public TypeCode GetTypeCode() {
            return TypeCode.Object;
        }
        /// <summary>
        /// 转换为 bool 类型
        /// </summary>
        /// <param name="provider">格式化提供程序</param>
        /// <returns>始终抛出异常 InvalidCastException</returns>
        public bool ToBoolean(IFormatProvider provider) {
            throw new InvalidCastException();
        }
        /// <summary>
        /// 转换为 char 类型
        /// </summary>
        /// <param name="provider">格式化提供程序</param>
        /// <returns>始终抛出异常 InvalidCastException</returns>
        public char ToChar(IFormatProvider provider) {
            throw new InvalidCastException();
        }
        /// <summary>
        /// 转换为 sbyte 类型
        /// </summary>
        /// <param name="provider">格式化提供程序</param>
        /// <returns>始终抛出异常 InvalidCastException</returns>
        public sbyte ToSByte(IFormatProvider provider) {
            throw new InvalidCastException();
        }
        /// <summary>
        /// 转换为 byte 类型
        /// </summary>
        /// <param name="provider">格式化提供程序</param>
        /// <returns>始终抛出异常 InvalidCastException</returns>
        public byte ToByte(IFormatProvider provider) {
            throw new InvalidCastException();
        }
        /// <summary>
        /// 转换为 short 类型
        /// </summary>
        /// <param name="provider">格式化提供程序</param>
        /// <returns>始终抛出异常 InvalidCastException</returns>
        public short ToInt16(IFormatProvider provider) {
            throw new InvalidCastException();
        }
        /// <summary>
        /// 转换为 ushort 类型
        /// </summary>
        /// <param name="provider">格式化提供程序</param>
        /// <returns>始终抛出异常 InvalidCastException</returns>
        public ushort ToUInt16(IFormatProvider provider) {
            throw new InvalidCastException();
        }
        /// <summary>
        /// 转换为 int 类型
        /// </summary>
        /// <param name="provider">格式化提供程序</param>
        /// <returns>Time 表示的时间的毫秒数</returns>
        public int ToInt32(IFormatProvider provider) {
            return Ticks;
        }
        /// <summary>
        /// 转换为 uint 类型
        /// </summary>
        /// <param name="provider">格式化提供程序</param>
        /// <returns>Time 表示的时间的毫秒数</returns>
        public uint ToUInt32(IFormatProvider provider) {
            return (uint)Ticks;
        }
        /// <summary>
        /// 转换为 long 类型
        /// </summary>
        /// <param name="provider">格式化提供程序</param>
        /// <returns>Time 表示的时间的毫秒数</returns>
        public long ToInt64(IFormatProvider provider) {
            return Ticks;
        }
        /// <summary>
        /// 转换为 long 类型
        /// </summary>
        /// <param name="provider">格式化提供程序</param>
        /// <returns>Time 表示的时间的毫秒数</returns>
        public ulong ToUInt64(IFormatProvider provider) {
            return (ulong)Ticks;
        }
        /// <summary>
        /// 转换为 float 类型
        /// </summary>
        /// <param name="provider">格式化提供程序</param>
        /// <returns>Time 表示的时间的毫秒数</returns>
        public float ToSingle(IFormatProvider provider) {
            return Ticks;
        }
        /// <summary>
        /// 转换为 double 类型
        /// </summary>
        /// <param name="provider">格式化提供程序</param>
        /// <returns>Time 表示的时间的毫秒数</returns>
        public double ToDouble(IFormatProvider provider) {
            return Ticks;
        }
        /// <summary>
        /// 转换为 decimal 类型
        /// </summary>
        /// <param name="provider">格式化提供程序</param>
        /// <returns>Time 表示的时间的毫秒数</returns>
        public decimal ToDecimal(IFormatProvider provider) {
            return Ticks;
        }
        /// <summary>
        /// 转换为 DateTime 类型
        /// </summary>
        /// <param name="provider">格式化提供程序</param>
        /// <returns>当天日期，并且时间部分为由 Time 表示的时间</returns>
        public DateTime ToDateTime(IFormatProvider provider) {
            var now = DateTime.Now;
            return new DateTime(now.Year, now.Month, now.Day, hour, minute, second, millisecond);
        }
        /// <summary>
        /// 转换为 string 类型
        /// </summary>
        /// <param name="provider">格式化提供程序</param>
        /// <returns>Time 表示的时间的字符串。</returns>
        public string ToString(IFormatProvider provider) {
            return ToString();
        }

        /// <summary>
        /// 转换为指定类型
        /// </summary>
        /// <param name="conversionType">转换的目标类型</param>
        /// <param name="provider">格式化提供程序</param>
        /// <returns>Time 表示的时间的毫秒数</returns>
        public object ToType(Type conversionType, IFormatProvider provider) {
            if(conversionType == typeof(TimeSpan)) {
                return (TimeSpan)this;
            }
            if(conversionType == typeof(Time2)) {
                return this;
            }
            return Convert.ChangeType(this, conversionType);
        }

        #endregion
    }

    /// <summary>
    /// 用于表示一天之内的一个时间范围
    /// </summary>
    public struct TimeScope : IEquatable<TimeScope> {
        /// <summary>
        /// 时间范围的最小值
        /// </summary>
        public static readonly TimeScope MinScope = new TimeScope(Time.MinValue, Time.MinValue);

        /// <summary>
        /// 时间范围的最大值
        /// </summary>
        public static readonly TimeScope MaxScope = new TimeScope(Time.MinValue, Time.MaxValue);

        private Time end;
        private Time start;

        /// <summary>
        /// 用指定的起始和结束时间初始化 TimeScope 类型的新实例
        /// </summary>
        /// <param name="start"> 起始时间 </param>
        /// <param name="end"> 结束时间 </param>
        public TimeScope(Time start, Time end) {
            this.start = start;
            this.end = end;
        }

        /// <summary>
        /// 起始时间
        /// </summary>
        public Time Start {
            get { return start; }
            set { start = value; }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public Time End {
            get { return end; }
            set { end = value; }
        }

        #region Implementation of IEquatable<TimeScope>

        /// <summary>
        /// 判断两个时间范围是否相等
        /// </summary>
        /// <param name="other"> </param>
        /// <returns> </returns>
        public bool Equals(TimeScope other) {


            return (end.Ticks - start.Ticks) == (other.end.Ticks - other.start.Ticks);
        }

        #endregion
    }
}