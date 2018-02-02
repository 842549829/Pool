namespace ChinaPay.B3B.Service.Foundation.Domain {
    public abstract class Location {
        protected Location(string code) {
            this.Code = code;
        }

        /// <summary>
        /// 代码
        /// </summary>
        public string Code {
            get;
            private set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name {
            get;
            internal set;
        }
        /// <summary>
        /// 全拼
        /// </summary>
        public string Spelling {
            get;
            internal set;
        }
        /// <summary>
        /// 简拼
        /// </summary>
        public string ShortSpelling {
            get;
            internal set;
        }
        /// <summary>
        /// 热点级别
        /// </summary>
        public int HotLevel {
            get;
            internal set;
        }

        public override string ToString() {
            return string.Format("代码:{0} 名称:{1} 全拼:{2} 简拼:{3} 热点级别:{4}", Code, Name, Spelling, ShortSpelling, HotLevel);
        }
    }
}
