using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using MySql.Data.MySqlClient;
using Protocol;

namespace 异步服务器
{
    class Program
    {
        static void Main(string[] args)
        {
            AccountManage.Ins.Register();
            try
            {
                Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Bind(new IPEndPoint(IPAddress.Loopback, ProtocolConfig.port));
                server.Listen(10);
                Console.WriteLine($"启动了.....{IPAddress.Loopback}");
                //使用异步监测客户端连接情况
                server.BeginAccept(AsyncAccept, server);

                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// 如果有客户端连接
        /// </summary>
        /// <param name="ar"></param>
        private static void AsyncAccept(IAsyncResult ar)
        {
            Socket server = ar.AsyncState as Socket;
            if (server==null)
            {
                return;
            }
            Socket clien = server.EndAccept(ar);
            //list.Add(clien);

            //将每一个客户端单独作为一个类有自己的数据（byte数组）
            ClienPeer client = new ClienPeer(clien);
            NetManager.Ins.AddClient(client);
            //监听消息
            server.BeginAccept(AsyncAccept,server);
        }
    }
}
