using System;

namespace IGG.Core.Geom
{
    /// <summary>
    /// Author  zhulin
    /// Date    2018.9.3
    /// Desc    Double2, 保持接口名与unity一至
    /// </summary>
    public struct Double2
    {
        public static Double2 Zero = new Double2();

        public double X;
        public double Y;

        public Double2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Double2(float xyz)
        {
            X = xyz;
            Y = xyz;
        }

        public Double2(Double2 v)
        {
            X = v.X;
            Y = v.Y;
        }

        /// <summary>
        /// 长度平方根
        /// </summary>
        public double SqrMagnitude
        {
            get
            {
                return X*X + Y*Y;
            }
        }

        /// <summary>
        /// 长度
        /// </summary>
        public double Magnitude
        {
            get { return Math.Sqrt(SqrMagnitude); }
        }

        /// <summary>
        /// 返回一个归一化对象
        /// </summary>
        public Double2 Normalized
        {
            get
            {
                Double2 d = new Double2(this);
                d.Normalize();
                return d;
            }
        }

        /// <summary>
        /// 将自己归一化
        /// </summary>
        public void Normalize()
        {
            double size = Magnitude;
            X /= size;
            Y /= size;
        }

        /// <summary>
        /// 用外部传入的magnitude将自己归一化
        /// 通常调用这个都是为了优化计算
        /// </summary>
        /// <param name="magnitude"></param>
        public void Normalize(double magnitude)
        {
            X /= magnitude;
            Y /= magnitude;
        }

        public double SqrDistanceTo(double targetX, double targetY)
        {
            double dx = targetX - X;
            double dy = targetY - Y;
            return dx * dx + dy * dy;
        }

        public override bool Equals(Object o)
        {                                           
            if (o is Double2)
            {
                Double2 dis = (Double2) o - this;
                return Math.Abs(dis.SqrMagnitude) < 0.0000000000000000000000000000000000001;
            }
            return false;
        }

        public Double2 Clone()
        {
            return new Double2(this);
        }

        public override int GetHashCode()
        {
            double ret = X*49157 + Y*196613;
            return (int)ret;
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }

        public static Double2 operator -(Double2 a, Double2 b)
        {
            return new Double2(a.X - b.X, a.Y - b.Y);
        }

        public static Double2 operator *(Double2 a, double scale)
        {
            return new Double2(a.X * scale, a.Y * scale);
        }
    }
}
