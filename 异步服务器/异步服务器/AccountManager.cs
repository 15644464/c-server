using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol;
using MySql.Data.MySqlClient;

namespace 异步服务器
{
    /// <summary>
    /// 注册，登录操作
    /// </summary>
    public class AccountManage
    {
        
        private static AccountManage ins;

        public static AccountManage Ins 
        {
            get 
            {
                if (ins == null)
                {
                    ins = new AccountManage();
                }
                return ins;
            }
        }

        private AccountManage()
        { }
        public void Login(AccountVo av,ClienPeer cp)
        {
            int result = isLoginSuccess(av) ? 1 : 0;
            cp.SendMessage(ProtocolEnum.logonMessage,new MyString(result.ToString()));
        }
        /// <summary>
        /// 注册函数
        /// </summary>
        /// <param name="av"></param>
        /// <param name="cp"></param>
        //public void Register(ClienPeer cp)
        public void Register()
        {
            MySqlHelper msq = new MySqlHelper("name", "userinfor");
            MySqlConnection ms = msq.Open();
            string cha = msq.sqlCheck("username","123");

            MySqlDataReader reader = msq.Command(cha, ms);
            //MySqlCommand command = new MySqlCommand(msq.sqlAdd(new string[] {"100001","xunasm", },ms), ms);

            //MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Console.WriteLine(reader.GetString("password"));
                reader.Close();
            }
            else 
            {
                Console.WriteLine("没有用户信息");
            }
        }

        /// <summary>
        /// 在数据库中读取数据
        /// </summary>
        /// <param name="av"></param>
        /// <returns></returns>
        bool isLoginSuccess(AccountVo av)
        {
            MySqlHelper msq = new MySqlHelper("name", "account");
            MySqlConnection ms = msq.Open();
            bool key = false;
            MySqlCommand command = new MySqlCommand($"select * form account where username='{av.username}'", ms);
            
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                string password = reader.GetString("password");
                key = av.password == password;
            }
            reader.Close();
            return key;
        }
    }
}
