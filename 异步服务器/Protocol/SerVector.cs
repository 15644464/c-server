using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol
{
    [Serializable]
    public class SerVector
    {
        public float x,y,z,w;

        public SerVector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public SerVector(float[] v)
        {
            this.x = v[0];
            this.y = v[1];
            this.z = v[2];
            if (v.Length>3)
            {
                this.w = v[3];
            }
        }

        public SerVector(float x, float y, float z, float w=1)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

    }
}
