using System;

namespace IGG.Core.Geom
{
    [System.Serializable]
    public struct Int3
    {
        public int x;
        public int y;
        public int z;

        public Int3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }


        private static Int3 zeroInt3 = new Int3(0, 0, 0);
        public static Int3 zero { get { return zeroInt3; } }

        public int sqrMagnitude
        {
            get
            {
                return this.x * this.x + this.y * this.y + this.z * this.z;
            }
        }

        public long sqrMagnitudeLong
        {
            get
            {
                return (long)this.x * (long)this.x + (long)this.y * (long)this.y + (long)this.z * (long)this.z;
            }
        }

        public static Int3 operator *(Int3 a, int scale)
        {
            return new Int3(a.x * scale, a.y * scale, a.z * scale);
        }

        public static Int3 operator +(Int3 a, Int3 b)
        {
            return new Int3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Int3 operator -(Int3 a, Int3 b)
        {
            return new Int3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static bool operator ==(Int3 a, Int3 b)
        {
            return ((a.x.Equals(b.x)) && (a.y.Equals(b.y)) && (a.z.Equals(b.z)));
        }

        public static Int3 operator /(Int3 a, int d)
        {
            return new Int3(a.x / d, a.y / d, a.z / d);
        }

        public static bool operator !=(Int3 a, Int3 b)
        {
            return !(a == b);
        }

        public static int Dot(Int3 a, Int3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static long DotLong(Int3 a, Int3 b)
        {
            return (long)a.x * (long)b.x + (long)a.y * (long)b.y + (long)a.z * (long)b.z;
        }

        //º∆À„æ‡¿Î
        public static int Distance(Int3 a, Int3 b)
        {
            return Distance(a.x, a.y, a.z, b.x, b.y, b.z);
        }

        public static int Distance(int ax, int ay, int az, int bx, int by, int bz)
        {
            int dx = ax - bx;
            int dy = ay - by;
            int dz = az - bz;
            return (int)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        /*public static float DistanceFloat(Int2 a, Int2 b)
        {
            float dx = a.x - b.x;
            float dy = a.y - b.y;
            return (float)(Math.Sqrt(dx * dx + dy * dy));
        }*/

        public static long DistanceNoSquare(Int3 a, Int3 b)
        {
            long dx = a.x - b.x;
            long dy = a.y - b.y;
            long dz = a.z - b.z;

            return dx * dx + dy * dy + dz * dz;
        }

        public override bool Equals(System.Object o)
        {
            if (o == null) return false;
            Int3 rhs = (Int3)o;

            return x == rhs.x && y == rhs.y && z == rhs.z;
        }

        public override int GetHashCode()
        {
            return x * 49157 + y * 98317 + z * 1474527;
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
        /*private static readonly int[] Rotations = {
             1, 0, //Identity matrix
			 0, 1,

             0, 1,
            -1, 0,

            -1, 0,
             0,-1,

             0,-1,
             1, 0
        };*/

        /** Returns a new Int2 rotated 90*r degrees around the origin. */
        /*public static Int2 Rotate(Int2 v, int r)
        {
            r = r % 4;
            return new Int2(v.x * Rotations[r * 4 + 0] + v.y * Rotations[r * 4 + 1], v.x * Rotations[r * 4 + 2] + v.y * Rotations[r * 4 + 3]);
        }*/

        public static Int3 Min(Int3 a, Int3 b)
        {
            return new Int3(System.Math.Min(a.x, b.x), System.Math.Min(a.y, b.y), System.Math.Min(a.z, b.z));
        }

        public static Int3 Max(Int3 a, Int3 b)
        {
            return new Int3(System.Math.Max(a.x, b.x), System.Math.Max(a.y, b.y), System.Math.Max(a.z, b.z));
        }

        public Int3 Normalize()
        {
            int dis = this.x * this.x + this.y * this.y + this.z * this.z;
            this.x = this.x * 100 / (int)System.Math.Sqrt(dis);
            this.y = this.y * 100 / (int)System.Math.Sqrt(dis);
            this.z = this.z * 100 / (int)System.Math.Sqrt(dis);

            return this;
        }

        public int[] ToArray()
        {
            return new int[3] {x * 100, y * 100, z * 100};
        }

        public override string ToString()
        {
            return "(" + x + ", " + y + ", " + z + ")";
        }
    }
}