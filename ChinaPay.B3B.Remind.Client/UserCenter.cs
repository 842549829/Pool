using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ChinaPay.B3B.Remind.Client {
    internal class UserCenter {
        private static UserCenter _instance = null;
        private static object _locker = new object();
        public static UserCenter Instance {
            get {
                if(_instance == null) {
                    lock(_locker) {
                        if(_instance == null) {
                            _instance = new UserCenter();
                        }
                    }
                }
                return _instance;
            }
        }

        private string _userDataFile = "user.db";
        private Encoding _encoding = Encoding.UTF8;
        private string _splitor = "|";
        private List<UserInfo> _users = null;
        private UserCenter() {
            load();
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        public void Save(string userName, string password) {
            UserInfo userInfo = Query(userName);
            if(userInfo == null) {
                var newUser = new UserInfo();
                newUser.UserName = userName;
                newUser.Password = password;
                newUser.Time = DateTime.Now;
                _users.Add(newUser);
            } else {
                userInfo.Password = password;
                userInfo.Time = DateTime.Now;
            }
            flush();
        }
        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        public void Remove(string userName) {
            if(_users.RemoveAll(item => string.Compare(item.UserName, userName, true) == 0) > 0) {
                flush();
            }
        }
        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="userName">用户名</param>
        public UserInfo Query(string userName) {
            foreach(var item in _users) {
                if(string.Compare(item.UserName, userName, true) == 0) {
                    return item;
                }
            }
            return null;
        }
        /// <summary>
        /// 查询上一次保存的用户信息
        /// </summary>
        public UserInfo QueryLatestUser() {
            UserInfo latestUser = null;
            foreach(var userInfo in _users) {
                if(latestUser == null || latestUser.Time < userInfo.Time) {
                    latestUser = userInfo;
                }
            }
            return latestUser;
        }

        /// <summary>
        /// 加载用户信息
        /// </summary>
        private void load() {
            _users = new List<UserInfo>();
            var content = loadFile(_userDataFile);
            if(content.Length > 0) {
                var userItems = content.Split(Environment.NewLine.ToCharArray());
                foreach(var item in userItems) {
                    if(item.Length > 0) {
                        var items = item.Split(_splitor.ToCharArray());
                        if(items.Length == 3) {
                            UserInfo userInfo = new UserInfo();
                            userInfo.UserName = decode(items[0]);
                            userInfo.Password = decode(items[1]);
                            DateTime time;
                            if(!DateTime.TryParse(decode(items[2]), out time)) {
                                time = DateTime.MinValue;
                            }
                            userInfo.Time = time;
                            _users.Add(userInfo);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 保存用户信息
        /// </summary>
        private void flush() {
            string content = string.Empty;
            foreach(var item in _users) {
                content += encode(item.UserName) + _splitor + encode(item.Password) + _splitor + encode(item.Time.ToString("yyyy-MM-dd HH:mm:ss")) + Environment.NewLine;
            }
            saveFile(_userDataFile, content.TrimEnd(Environment.NewLine.ToCharArray()));
        }
        private string loadFile(string fileName) {
            if(File.Exists(fileName)) {
                using(FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read)) {
                    byte[] datas = new byte[stream.Length];
                    stream.Read(datas, 0, datas.Length);
                    return _encoding.GetString(datas);
                }
            }
            return string.Empty;
        }
        private void saveFile(string fileName, string content) {
            using(FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write)) {
                byte[] datas = _encoding.GetBytes(content);
                stream.SetLength(datas.Length);
                stream.Write(datas, 0, datas.Length);
            }
        }
        private string encode(string content) {
            string result = string.Empty;
            foreach(var item in content) {
                int ascii = item;
                char ch = (char)((ascii + result.Length + 1) << 8);
                result += ch;
            }
            return result;
        }
        private string decode(string content) {
            var result = string.Empty;
            foreach(var item in content) {
                int ascii = item;
                char ch = (char)((ascii >> 8) - result.Length - 1);
                result += ch;
            }
            return result;
        }
    }
    class UserInfo {
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime Time { get; set; }
    }
}