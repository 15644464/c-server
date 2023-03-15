using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace 异步服务器
{
    public class Test
    {
        /// <summary>
        /// 序列化obj对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        static byte[] SerializableObject(object obj)
        {
            using (MemoryStream ms=new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms,obj);
                byte[] byArr = new byte[(int)ms.Length];
                Buffer.BlockCopy(ms.GetBuffer(),0,byArr,0,byArr.Length);
                return byArr;
            }        
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        static object DeserializeObject(byte[] by)
        {
            using (MemoryStream ms=new MemoryStream(by))
            {
                BinaryFormatter bf = new BinaryFormatter();
                object obj = bf.Deserialize(ms);
                return obj;
            }
        }
    }
}
