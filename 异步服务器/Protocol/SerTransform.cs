using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol
{
    /// <summary>
    /// 序列化transform
    /// </summary>
    [Serializable]
    public class SerTransform
    {
        public SerVector position, rotation,scale;
        public SerTransform(SerVector position, SerVector rotation, SerVector scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }

        public SerTransform(float[] tran)
        {
            float[] arr= {tran[0], tran[1], tran[2] };
            this.position = new SerVector(arr);
            arr =new float[] { tran[3], tran[4], tran[5] , tran[6] };
            this.rotation = new SerVector(arr);
            arr = new float[] { tran[7], tran[8], tran[9] };
            this.scale = new SerVector(arr);
        }
    }
}
