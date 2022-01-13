using System;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dnPW
{
    public  class dnUsers
    {
        private List<User> users = new List<User>();

        private  const string fileName = @"E:\DN.sec";

        public delegate void UserLogInChangedEventHandler(object sender, UserLogChangedEventArgs e);
        public event UserLogInChangedEventHandler UserLogInChanged;

        private int waitTime = 60;

        private int userLevel = 0;

        private Timer WaitTimer = new Timer();

        private int TickCount = 0;

        private bool isAutoLogout = true;

        #region Properties
        public string LogInUserName
        {
            get
            {
                if (this.userLevel < users.Count)
                {
                    return users[userLevel].UserName;
                }
                else
                {
                    return users[0].UserName;
                }
            }
        }

        public int UserLevel
        {
            get
            {
                return this.userLevel;
            }
        }

        internal List<string> UserNames
        {
            get
            {
                List<string> names = new List<string>();
                foreach(User user in users)
                {
                    names.Add(user.UserName);
                }
                return names;
            }
        }

        public int WaitTime
        {
            set
            {
                this.waitTime = value;
            }
            get
            {
                return this.waitTime;
            }
        }

        internal bool IsAutoLogout
        {
            set
            {
                this.isAutoLogout = value;
            }
            get
            {
                return this.isAutoLogout;
            }
        }
        #endregion

        #region Methods

        public void LoginDialog()
        {
            LoginDialog dialog = new LoginDialog(this);
            dialog.ShowDialog();
        }

        internal bool LogIn(string userName,string password)
        {
            User mUser = null;
            foreach (User user in users)
            {
                if (user.UserName == userName)
                {
                    mUser = user;
                    break;
                }
            }
            if(mUser == null)
            {
                MessageBox.Show("未找到指定的用户名，请确认该用户是否存在！","登陆通知...",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return false;
            }
            if (mUser.UserPassword != password)
            {
                MessageBox.Show("输入的密码错误，请重新输入！", "登陆通知...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                this.userLevel = mUser.Level;
                DoUserLogInChangedEvent();
                if (this.userLevel > 0 && IsAutoLogout)
                {
                    TickCount = 0;
                    WaitTimer = new Timer();
                    WaitTimer.Interval = 1000;
                    WaitTimer.Tick += new EventHandler(WaitTimer_Tick);
                    WaitTimer.Stop();
                    WaitTimer.Start();
                }
                
                return true;
            }
        }

        internal bool LogIn(int level, string password)
        {
            if (level >= users.Count)
            {
                MessageBox.Show("未找到指定的用户名，请确认该用户是否存在！", "登陆通知...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (users[level].UserPassword == password)
            {
                this.userLevel = level;
                DoUserLogInChangedEvent();
                if (level > 0 && IsAutoLogout)
                {
                    TickCount = 0;
                    WaitTimer = new Timer();
                    WaitTimer.Interval = 1000;
                    WaitTimer.Tick += new EventHandler(WaitTimer_Tick);
                    WaitTimer.Stop();
                    WaitTimer.Start();
                }
                return true;
            }
            else
            {
                MessageBox.Show("输入的密码错误，请重新输入！", "登陆通知...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void LogOut()
        {
            this.userLevel = 0;
            DoUserLogInChangedEvent();
        }

        internal bool AdminConfirm(string adminPassword)
        {
            return users[users.Count - 1].UserPassword == adminPassword;
        }

        internal bool UserConfirm(string userName, string password)
        {
            User mUser = null;
            foreach (User user in users)
            {
                if (user.UserName == userName)
                {
                    mUser = user;
                    break;
                }
            }
            if (mUser == null)
            {
                return false;
            }
            else 
            {
                return mUser.UserPassword == password;
            }
        }

        internal void SetPwDialog(string name)
        {
            new ChangePW(name,this).ShowDialog();
        }

        internal void ChangePW(string userName, string userPassword)
        {
            try
            {
                User mUser = null;
                foreach (User user in users)
                {
                    if (user.UserName == userName)
                    {
                        mUser = user;
                        break;
                    }
                }
                if (mUser == null)
                {
                    MessageBox.Show("未找到指定的用户名，请确认该用户是否存在！", "登陆通知...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return ;
                }

                mUser.UserPassword = userPassword;

                SaveUsers();

                MessageBox.Show("密码修改成功！", "修改密码...");
            }
            catch(Exception ex)
            {
                MessageBox.Show("修改密码时出现以下错误！\r\n " + ex.Message,"修改密码...");
            }
        }

        private  void  IniUsers()
        {
            try
            {
                if (System.IO.File.Exists(fileName))
                {
                    try
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                        this.users = bf.Deserialize(fs) as List<User>;
                        fs.Flush();
                        fs.Close();
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show("读取用户数据失败！\r\n" + ex.Message, "初始化用户...");
                    }
                }
                else
                {
                    users.Clear();
                    users.Add(new User { Level = 0, UserName = "Operator", UserPassword = "" });
                    users.Add(new User { Level = 1, UserName = "Engineer", UserPassword = "" });
                    users.Add(new User { Level = 2, UserName = "Admin", UserPassword = "dn@nimdA" });
                }
                this.userLevel = 0;
                DoUserLogInChangedEvent();
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("初始化用户信息失败！\r\n" + ex.Message, "初始化用户...");
            }
        }

        private void SaveUsers()
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                bf.Serialize(fs, this.users);
                fs.Flush();
                fs.Close();
                //System.Windows.Forms.MessageBox.Show("用户数据保存成功！", "用户保存...");

                System.IO.FileInfo fi = new System.IO.FileInfo(fileName);
                if (fi.Exists)
                {
                    fi.Attributes = System.IO.FileAttributes.Hidden;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("用户数据保存失敗！\r\n " + ex.Message, "用户保存...");
            }
           
        }

        private void DoUserLogInChangedEvent()
        {
            if (UserLogInChanged != null)
            {
                UserLogChangedEventArgs e = new UserLogChangedEventArgs { UserLevel = this.UserLevel, UserName = this.LogInUserName };
                UserLogInChanged(this, e);
            }
        }

        private void WaitTimer_Tick(object sender, EventArgs e)
        {
            if (TickCount > this.waitTime)
            {
                this.LogOut();
                if (WaitTimer != null)
                {
                    WaitTimer.Tick -= this.WaitTimer_Tick;
                    WaitTimer.Dispose();
                    WaitTimer = null;
                }
            }
            else
            {
                TickCount++;
            }
        }

        public dnUsers()
        {
            IniUsers();
            WaitTimer.Interval = 1000;
            WaitTimer.Tick += new EventHandler(WaitTimer_Tick);
            SaveUsers();
        }

        #endregion


        [Serializable]
        internal class User
        {
            public int Level
            {
                set;
                get;
            }
            public string UserName
            {
                set;
                get;
            }
            public string UserPassword
            {
                get;
                set;
            }

        }

        public class UserLogChangedEventArgs : EventArgs
        {
            public string UserName
            {
                internal set;
                get;
            }
            public int UserLevel
            {
                internal set;
                get;
            }
        }
    }


}
