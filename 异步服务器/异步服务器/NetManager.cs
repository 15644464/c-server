using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using Protocol;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace 异步服务器
{
    class NetManager
    {
        private static NetManager _ins = null;

        private NetManager()
        {
            //将消息监听放到多线程中
            Thread thread = new Thread(DealWithMessage);
            thread.Start();
        }

        

        public static NetManager Ins
        {
            get {
                if (_ins==null)
                {
                    _ins = new NetManager();
                }
                return _ins;
            }
        }
        //消息队列会逐条处理消息
        Queue<ClientMessage> msgQue = new Queue<ClientMessage>();
        /// <summary>
        /// 客户端对象
        /// </summary>
        List<ClienPeer> cpList = new List<ClienPeer>();

        /// <summary>
        /// 客户端连接
        /// </summary>
        /// <param name="cl"></param>
        public void AddClient(ClienPeer cl)
        {
            cpList.Add(cl);
            Console.WriteLine($"客户端{cl.client.RemoteEndPoint}连接");

        }

        /// <summary>
        /// 客户端断开
        /// </summary>
        /// <param name="cl"></param>
        public void RemoveClient(ClienPeer cl)
        {
            cpList.Remove(cl);
            Console.WriteLine($"客户端{cl}断开");
        }

        

        /// <summary>
        /// 消息管理器
        /// </summary>
        /// <param name="byArr">消息</param>
        /// <param name="cp">客户端</param>
        public void AddMessage(byte[] byArr, ClienPeer cp)
        {
            //解析消息
            SocketMessage sm = SocketMessage.Parse(byArr);
            Console.WriteLine($"收到来自{cp.client.RemoteEndPoint}的消息");
            var a = new ClientMessage(cp, sm);
            //如果是位置消息不通过队列处理
            if (sm.mesId == ProtocolEnum.updataMessage|| sm.mesId == ProtocolEnum.isWebCam)
            {
                switch (sm.mesId)
                {
                    case ProtocolEnum.updataMessage:
                        BoardCastMessage(ProtocolEnum.updataMessage, a.smg.message, a.client);
                        break;
                    case ProtocolEnum.isWebCam:
                        BoardCastMessage(ProtocolEnum.isWebCam, a.smg.message, a.client);
                        break;
                }
            }
            else
            {
                //通过队列来管理其他消息
                msgQue.Enqueue(a);
            }
        }


        /// <summary>
        /// 处理其他消息
        /// </summary>
        private void DealWithMessage()
        {
            while (true)
            {
                //如果队列里面有消息就解析
                if (msgQue.Count>0)
                {
                    //解析消息
                    ClientMessage cm = msgQue.Dequeue();
                    
                    Console.WriteLine($"开始解析{cm.client.client.RemoteEndPoint}的消息");

                    //返回消息
                    //SendMessage(cm.smg.mesId, cm.smg.message,cm.client);
                    Console.WriteLine(cm.smg.mesId);
                    switch (cm.smg.mesId)
                    {
                        case ProtocolEnum.logonMessage:
                            AccountVo av = cm.smg.message as AccountVo;
                            AccountManage.Ins.Login(av,cm.client);
                            Console.WriteLine(cm.smg.message);
                            break;
                        case ProtocolEnum.regisMessage:
                            AccountVo _av = cm.smg.message as AccountVo;
                            Console.WriteLine("建立连接");
                            break;
                        //case ProtocolEnum.updataChat:
                        //    Console.WriteLine("接收到消息");
                        //    BoardCastMessage(ProtocolEnum.updataChat,cm.smg.message,cm.client);
                        //    var b = cm.smg.message;
                        //    var a= DeserializeObject(b as byte[]) as SerTransform;
                        //        Console.WriteLine($"{a.rotation.x},{a.rotation.y},{a.rotation.z},{a.rotation.w}");
                            //cm.smg.DeserializeObject
                            //break;
                        case ProtocolEnum.isInt:
                            Console.WriteLine((int)(cm.smg.message));
                            break;
                        case ProtocolEnum.chatBox:
                            Console.WriteLine((int)(cm.smg.message));
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="id">消息号</param>
        /// <param name="str">消息</param>
        /// <param name="cp">客户端</param>
        public void BoardCastMessage(ProtocolEnum id, object str, ClienPeer cp=null)
        {
            Console.WriteLine("广播消息");
            SocketMessage sm = new SocketMessage(id ,str);
            BoardCastMessage(sm.PackMessage(),cp);
        }

        /// <summary>
        /// 将消息发送到除消息源以外的客户端
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="cp">消息源客户端</param>
        public void BoardCastMessage(byte[] arr, ClienPeer cp = null)
        {
            for (int i = 0; i < cpList.Count; i++)
            {
                if (cp != null && cpList[i] == cp)
                {
                    continue;
                }
                cpList[i].SendMessage(arr);
            }
        }

        /// <summary>
        /// 将消息发送到目标客户端
        /// </summary>
        /// <param name="id"></param>
        /// <param name="str"></param>
        /// <param name="cp"></param>
        public void SendMessage(ProtocolEnum id,object str ,ClienPeer cp)
        {
            cp.SendMessage(id,str);
        }

        public void SendMessage(byte[] arr, ClienPeer cp) 
        {
            cp.SendMessage(arr);
        }

    }
}
