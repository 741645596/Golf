using System;

namespace IGG.Core.Geom
{
    [Serializable]
    public struct Float2
    {
        public float x;
        public float y;

        public Float2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        private static readonly Float2 zeroFloat2 = new Float2(0, 0);
        public static Float2 zero { get { return zeroFloat2; } }
    }
}
