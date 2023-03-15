using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Protocol
{
    /// <summary>
    /// 这是消息编号和内容
    /// </summary>
    public class SocketMessage
    {
        public ProtocolEnum mesId;//消息号
        public object message;//内容

        public SocketMessage(ProtocolEnum mesId, object message)
        {
            this.mesId = mesId;
            this.message = message;
        }

        /// <summary>
        /// 将消息序列化返回可以发送的消息
        /// </summary>
        /// <returns></returns>
        public byte[] PackMessage()
        {
            byte[] idArr = BitConverter.GetBytes((int)mesId);
            byte[] msgArr = SerializableObject(message);
            //拼接数组
            byte[] data = idArr.Concat(msgArr).ToArray();
            return data;
        }
        /// <summary>
        /// 解析消息
        /// </summary>
        /// <param name="msgArr"></param>
        /// <returns></returns>
        public static SocketMessage Parse(byte[] msgArr)
        {
            //取出数据编号
            ProtocolEnum _id = (ProtocolEnum)BitConverter.ToInt32(msgArr,0);
            //string _msg = Encoding.UTF8.GetString(msgArr,4,msgArr.Length-4);
            byte[] byArr = new byte[msgArr.Length-4];
            Buffer.BlockCopy(msgArr, 4, byArr, 0, byArr.Length);
            //Array.Copy(msgArr,4, byArr,0, byArr.Length);
            //for (int i = 0; i < byArr.Length; i++)
            //{
            //    byArr[i] = msgArr[i + 4];
            //    Console.WriteLine("nu");
            //}
            //msgArr.CopyTo(byArr, 4);
            object obj = DeserializeObject(byArr);
            Console.WriteLine("反序列化完成"+obj.ToString());
            return new SocketMessage(_id,obj);
        }

        /// <summary>
        /// 序列化obj对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        static byte[] SerializableObject(object obj)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, obj);
                    byte[] byArr = new byte[(int)ms.Length];
                    Buffer.BlockCopy(ms.GetBuffer(), 0, byArr, 0, byArr.Length);
                    return byArr;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("序列化失败");
                throw;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        static object DeserializeObject(byte[] by)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(by))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    object obj = bf.Deserialize(ms);
                    return obj;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("反序列化失败");
                throw;
            }
        }
    }
}
