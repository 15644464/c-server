using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Protocol;

namespace 异步服务器
{
    /// <summary>
    /// 这是一个客户端对象
    /// </summary>
    public class ClienPeer
    {

        public Socket client = null;

        byte[] msg = new byte[10240];
        public ClienPeer(Socket cl)
        {
            this.client = cl;
            Console.WriteLine("开始接收数据");
            client.BeginReceive(msg, 0, msg.Length, SocketFlags.None, AsyncReceive, null);
        }
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="ar"></param>
        private void AsyncReceive(IAsyncResult ar)
        {
            try
            {
                Console.WriteLine(ar.AsyncState);
                int len = client.EndReceive(ar);
                if (len == 0)
                {
                    client.Close();
                    NetManager.Ins.RemoveClient(this);
                    return;
                }
                //简化消息
                byte[] byArr = new byte[len];
                Array.Copy(msg,0,byArr,0,len);
                //将消息添加到消息管理
                NetManager.Ins.AddMessage(byArr,this);
                //string message = Encoding.UTF8.GetString(msg,0,len);
                //NetManager.Ins.BoardCastMessage(message, this);
                //Console.WriteLine(message);
                client.BeginReceive(msg, 0, msg.Length, SocketFlags.None, AsyncReceive, null);
            }
            catch (Exception e)
            {
                client.Close();
                NetManager.Ins.RemoveClient(this);
                Console.WriteLine(e);
                return;
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="arr"></param>
        public void SendMessage(byte[] arr)
        {
            client.Send(arr);
        } 
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="str"></param>
        public void SendMessage(ProtocolEnum id, object str)
        {
            SocketMessage sm = new SocketMessage(id,str);
            client.Send(sm.PackMessage()) ;
        }
    }
}
