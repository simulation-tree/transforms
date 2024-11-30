#pragma warning disable CS8981
using System;
using System.Diagnostics;
using Unmanaged;
using Worlds;

namespace Transforms.Components
{
    /// <summary>
    /// Overrides transformation logic for positioning elements relative
    /// or absolute to the parent element.
    /// </summary>
    [Component]
    public struct Anchor
    {
        public static readonly Anchor Centered = new(new(0.5f, false), new(0.5f, false), new(0f, false), new(0.5f, false), new(0.5f, false), new(0f, false));
        public static readonly Anchor BottomLeft = new(new(0f, false), new(0f, false), new(0f, false), new(0f, false), new(0f, false), new(0f, false));
        public static readonly Anchor TopRight = new(new(1f, false), new(1f, false), new(0f, false), new(1f, false), new(1f, false), new(0f, false));
        public static readonly Anchor BottomRight = new(new(1f, false), new(0f, false), new(0f, false), new(1f, false), new(0f, false), new(0f, false));
        public static readonly Anchor TopLeft = new(new(0f, false), new(1f, false), new(0f, false), new(0f, false), new(1f, false), new(0f, false));
        public static readonly Anchor Bottom = new(new(0.5f, false), new(0f, false), new(0f, false), new(0.5f, false), new(0f, false), new(0f, false));
        public static readonly Anchor Top = new(new(0.5f, false), new(1f, false), new(0f, false), new(0.5f, false), new(1f, false), new(0f, false));
        public static readonly Anchor Left = new(new(0f, false), new(0.5f, false), new(0f, false), new(0f, false), new(0.5f, false), new(0f, false));
        public static readonly Anchor Right = new(new(1f, false), new(0.5f, false), new(0f, false), new(1f, false), new(0.5f, false), new(0f, false));

        public value minX;
        public value minY;
        public value minZ;
        public value maxX;
        public value maxY;
        public value maxZ;

#if NET
        /// <summary>
        /// Creates a default anchor pointing towards bottom left.
        /// </summary>
        public Anchor()
        {
            minX = new(0f, false);
            minY = new(0f, false);
            minZ = new(0f, false);
            maxX = new(0f, false);
            maxY = new(0f, false);
            maxZ = new(0f, false);
        }
#endif

        public Anchor(value minX, value minY, value minZ, value maxX, value maxY, value maxZ)
        {
            this.minX = minX;
            this.minY = minY;
            this.minZ = minZ;
            this.maxX = maxX;
            this.maxY = maxY;
            this.maxZ = maxZ;
        }

        public struct value
        {
            public const int NumberRange = 65536;
            public const int MaxNumberValue = 32768;
            public const float Precision = 0.00003051757f;

            private int data;

            public bool FromEdge
            {
                readonly get
                {
                    int bitPos = 0;
                    return (data & (1 << bitPos)) == 0;
                }
                set
                {
                    if (value)
                    {
                        data &= ~1;
                    }
                    else
                    {
                        data |= 1;
                    }
                }
            }

            public float Number
            {
                readonly get
                {
                    int bitPos = 0;
                    int numberInt = data & ~(1 << bitPos);
                    return (numberInt << 1) / (float)NumberRange;
                }
                set
                {
                    bool absolute = FromEdge;
                    int valueInt = (int)(value * NumberRange) >> 1;
                    data = valueInt;
                    if (absolute)
                    {
                        data &= ~1;
                    }
                    else
                    {
                        data |= 1;
                    }
                }
            }

            public unsafe value(float number, bool fromEdge)
            {
                ThrowIfOutOfRange(number);
                data = (int)(number * NumberRange) >> 1;
                if (fromEdge)
                {
                    data &= ~1;
                }
                else
                {
                    data |= 1;
                }
            }

            public unsafe override string ToString()
            {
                USpan<char> buffer = stackalloc char[32];
                uint length = ToString(buffer);
                return buffer.Slice(0, length).ToString();
            }

            public readonly uint ToString(USpan<char> buffer)
            {
                bool isRelative = FromEdge;
                float number = Number;
                uint length = 0;
                if (isRelative)
                {
                    buffer[0] = 'r';
                    buffer[1] = ':';
                    length = 2;
                }
                else
                {
                    buffer[0] = 'a';
                    buffer[1] = ':';
                    length = 2;
                }

                length += number.ToString(buffer.Slice(length));
                return length;
            }

            public readonly override bool Equals(object? obj)
            {
                return obj is value value && Equals(value);
            }

            public readonly bool Equals(value other)
            {
                return data == other.data;
            }

            public readonly override int GetHashCode()
            {
                return data.GetHashCode();
            }

            public static bool operator ==(value left, value right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(value left, value right)
            {
                return !(left == right);
            }

            [Conditional("DEBUG")]
            private static void ThrowIfOutOfRange(float input)
            {
                if (input < -MaxNumberValue || input >= MaxNumberValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(input), $"Anchor value must be greater than {-MaxNumberValue} and below {MaxNumberValue}.");
                }
            }
        }
    }
}
