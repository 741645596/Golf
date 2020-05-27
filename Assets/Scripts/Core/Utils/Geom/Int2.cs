#region Namespace

using System;

#endregion

namespace IGG.Core.Geom
{
    /** Two Dimensional Integer Coordinate Pair */
    [Serializable]
    public struct Int2
    {
        public static readonly Int2 zero = new Int2(0, 0);
        public static readonly Int2 Default = new Int2(int.MinValue + 1, int.MinValue + 1);

        public int x;
        public int y;

        public Int2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool IsDefault
        {
            get { return this == Default; }
        }

        /// <summary>
        ///   <para>Returns the squared length of this vector (Read Only).</para>
        /// </summary>
        public int sqrMagnitude
        {
            get { return x * x + y * y; }
        }

        public long sqrMagnitudeLong
        {
            get { return (long) x * (long) x + (long) y * (long) y; }
        }

        public static Int2 operator *(Int2 a, int scale)
        {
            return new Int2(a.x * scale, a.y * scale);
        }

        public static Int2 operator +(Int2 a, Int2 b)
        {
            return new Int2(a.x + b.x, a.y + b.y);
        }

        public static Int2 operator -(Int2 a, Int2 b)
        {
            return new Int2(a.x - b.x, a.y - b.y);
        }

        public static bool operator ==(Int2 a, Int2 b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Int2 a, Int2 b)
        {
            return a.x != b.x || a.y != b.y;
        }

        public static int Dot(Int2 a, Int2 b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public static long DotLong(Int2 a, Int2 b)
        {
            return (long) a.x * (long) b.x + (long) a.y * (long) b.y;
        }

        //计算距离
        public static double Distance(Int2 a, Int2 b)
        {
            return Distance(a.x, a.y, b.x, b.y);
        }

        public static double Distance(int ax, int ay, int bx, int by)
        {
            double dx = ax - bx;
            double dy = ay - by;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static float DistanceFloat(Int2 a, Int2 b)
        {
            float dx = a.x - b.x;
            float dy = a.y - b.y;
            return (float) Math.Sqrt(dx * dx + dy * dy);
        }

        public static int DistanceNoSquare(Int2 a, Int2 b)
        {
            int dx = a.x - b.x;
            int dy = a.y - b.y;

            return dx * dx + dy * dy;
        }

        /// <summary>
        ///   <para>Returns true if the given Int2 is exactly equal to this Int2.</para>
        /// </summary>
        /// <param name="other"></param>
        public override bool Equals(object other)
        {
            bool result;
            if (!(other is Int2))
            {
                result = false;
            }
            else
            {
                Int2 rhs = (Int2) other;
                result = x.Equals(rhs.x) && y.Equals(rhs.y);
            }

            return result;
        }

        public override int GetHashCode()
        {
            return x * 49157 + y * 98317;
        }

        /** Matrices for rotation.
		 * Each group of 4 elements is a 2x2 matrix.
		 * The XZ position is multiplied by this.
		 * So
		 * \code
		 * //A rotation by 90 degrees clockwise, second matrix in the array
		 * (5,2) * ((0, 1), (-1, 0)) = (2,-5)
		 * \endcode
		 */
        private static readonly int[] Rotations =
        {
            1, 0, //Identity matrix
            0, 1,

            0, 1,
            -1, 0,

            -1, 0,
            0, -1,

            0, -1,
            1, 0
        };

        /** Returns a new Int2 rotated 90*r degrees around the origin. */
        public static Int2 Rotate(Int2 v, int r)
        {
            r = r % 4;
            return new Int2(v.x * Rotations[r * 4 + 0] + v.y * Rotations[r * 4 + 1],
                            v.x * Rotations[r * 4 + 2] + v.y * Rotations[r * 4 + 3]);
        }

        public static Int2 Min(Int2 a, Int2 b)
        {
            return new Int2(Math.Min(a.x, b.x), Math.Min(a.y, b.y));
        }

        public static Int2 Max(Int2 a, Int2 b)
        {
            return new Int2(Math.Max(a.x, b.x), Math.Max(a.y, b.y));
        }

        public override string ToString()
        {
            return "(" + x + ", " + y + ")";
        }
    }
}