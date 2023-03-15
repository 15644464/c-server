using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol
{
    [Serializable]//使类型可以序列化
    public class MyString
    {

        public string str;
        [NonSerialized]//使参数不能被序列化
        public int num;

        public MyString(string str)
        {
            this.str = str;
        }

        public MyString(string str, int num)
        {
            this.str = str;
            this.num = num;
        }
    }
}
