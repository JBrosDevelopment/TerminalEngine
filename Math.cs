using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace TerminalEngine
{
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    public struct float2(float? a, float? b) :
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
        IEquatable<float>, 
        IEquatable<float2>, 
        IComparer<float2>, 
        IIncrementOperators<float2>, 
        IDecrementOperators<float2>, 
        IComparable<float2>, 
        IMinMaxValue<float2>
    {
        public static float2 DefaultValue => new(0, 0);

        public static float2 MaxValue => new(float.MinValue, float.MaxValue);

        public static float2 MinValue => new(float.MinValue, float.MaxValue);

        public float a { get; set; } = a ?? 0f;
        public float b { get; set; } = b ?? 0f;

        public readonly int Compare(float2 x, float2 y)
        {
            return x < y ? -1 : x > y ? 1 : 0;
        }

        public readonly int CompareTo(float2 other)
        {
            return Compare(this, other);
        }

        public readonly bool Equals(float2 other)
        {
            return other.a == a && other.b == b;
        }

        public readonly override string ToString()
        {
            return string.Format("{0},{1}", a, b);
        }
        public readonly string ToString(string format = "{0},{1}")
        {
            return string.Format(format, a, b);
        }
        public readonly override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is float2 f2 && Equals(f2);
        }
        public readonly bool Equals(float other)
        {
            return a == other && b == other;
        }

        public static bool operator ==(float2 left, float2 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(float2 left, float2 right)
        {
            return !(left == right);
        }

        public static bool operator <(float2 left, float2 right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(float2 left, float2 right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(float2 left, float2 right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(float2 left, float2 right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static float2 operator ++(float2 value)
        {
            return new(value.a++, value.b++);
        }

        public static float2 operator --(float2 value)
        {
            return new(value.a--, value.b--);
        }

        public static float2 operator *(float2 left, float2 right)
        {
            return new(left.a * right.a, left.b * right.b);
        }

        public static float2 operator *(float2 left, float right)
        {
            return new(left.a * right, left.b * right);
        }

        public static float2 operator /(float2 left, float2 right)
        {
            return new(left.a / right.a, left.b / right.b);
        }

        public static float2 operator /(float2 left, float right)
        {
            return new(left.a * right, left.b * right);
        }

        public static float2 operator +(float2 left, float2 right)
        {
            return new(left.a + right.a, left.b + right.b);
        }

        public static float2 operator +(float2 left, float right)
        {
            return new(left.a + right, left.b + right);
        }

        public static float2 operator -(float2 left, float2 right)
        {
            return new(left.a - right.a, left.b - right.b);
        }

        public static float2 operator -(float2 left, float right)
        {
            return new(left.a - right, left.b - right);
        }

        public static float2 operator +(float2 value)
        {
            return new(+value.a, +value.b);
        }

        public static float2 operator -(float2 value)
        {
            return new(-value.a, -value.b);
        }

        public static implicit operator Position(float2 val)
        {
            return new((int)val.a, (int)val.b);
        }
        public static implicit operator float2(Position val)
        {
            return new(val.x, val.y);
        }
    }
#pragma warning restore IDE1006 // Naming Styles
}
