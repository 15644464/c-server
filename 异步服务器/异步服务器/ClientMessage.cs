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
    /// 客户端消息管理类，包含客户端和消息
    /// </summary>
    public class ClientMessage
    {

        public ClienPeer client;
        public SocketMessage smg;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client">消息源客户端</param>
        /// <param name="smg">消息</param>
        public ClientMessage(ClienPeer client, SocketMessage smg)
        {
            this.client = client;
            this.smg = smg;
        }
    }
}
