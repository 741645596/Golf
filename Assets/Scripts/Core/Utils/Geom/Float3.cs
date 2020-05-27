using System;
using System.Collections.Generic;

namespace IGG.Core.Geom
{
    [System.Serializable]
    public struct Float3
    {
        public float x;
        public float y;
        public float z;

        public Float3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        private static readonly Float3 zeroFloat3 = new Float3(0, 0, 0);
        public static Float3 zero { get { return zeroFloat3; } }
    }
}
