using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;


public class DV
{
    [System.Serializable]
    public struct dv2
    {
        public double x;
        public double y;

        public dv2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static dv2 operator +(dv2 a, dv2 b)
        {
            return new dv2(a.x + b.x, a.y + b.y);
        }

        public static dv2 operator -(dv2 a, dv2 b)
        {
            return new dv2(a.x - b.x, a.y - b.y);
        }

        public static dv2 operator *(dv2 v, double scalar)
        {
            return new dv2(v.x * scalar, v.y * scalar);
        }

        public static dv2 operator /(dv2 v, double divisor)
        {
            return new dv2(v.x / divisor, v.y / divisor);
        }
    }

    [System.Serializable]
    public struct dv3
    {
        public double x;
        public double y;
        public double z;

        public dv3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static dv3 operator +(dv3 a, dv3 b)
        {
            return new dv3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static dv3 operator -(dv3 a, dv3 b)
        {
            return new dv3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static dv3 operator *(dv3 v, double scalar)
        {
            return new dv3(v.x * scalar, v.y * scalar, v.z * scalar);
        }

        public static dv3 operator /(dv3 v, double divisor)
        {
            return new dv3(v.x / divisor, v.y / divisor, v.z / divisor);
        }
    }
}
