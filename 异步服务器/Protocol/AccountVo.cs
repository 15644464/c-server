using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol
{
    [Serializable]
    public class AccountVo
    {
        public string username;
        public string password;

        public AccountVo(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public override string ToString()
        {
            base.ToString();
            return username+ password;
        }
    }
}
