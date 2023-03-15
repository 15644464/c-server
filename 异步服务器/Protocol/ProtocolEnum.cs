using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol
{
    public enum ProtocolEnum
    {
        /// <summary>
        /// 登录消息
        /// </summary>
        logonMessage = 1,
        /// <summary>
        /// 注册消息
        /// </summary>
        regisMessage,
        /// <summary>
        /// 不需要消息队列处理的消息（有可能丢失数据），实时更新如：位置
        /// </summary>
        updataMessage,

        isInt,
        /// <summary>
        /// 字符串消息
        /// </summary>
        isString,
        isFloat,
        isV3,
        isObj,
        /// <summary>
        /// 视频信息
        /// </summary>
        isWebCam,

        chatBox,//聊天框
    }
}
