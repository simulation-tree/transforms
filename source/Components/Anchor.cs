#pragma warning disable CS8981
using System;
using System.Diagnostics;

namespace Transforms.Components
{
    /// <summary>
    /// Overrides transformation logic for positioning elements relative
    /// or absolute to the parent element.
    /// </summary>
    public struct Anchor
    {
        public static readonly Anchor Centered = new(new(0.5f, false), new(0.5f, false), new(0f, false), new(0.5f, false), new(0.5f, false), new(0f, false));
        public static readonly Anchor BottomLeft = new(new(0f, false), new(0f, false), new(0f, false), new(0f, false), new(0f, false), new(0f, false));
        public static readonly Anchor TopRight = new(new(1f, false), new(1f, false), new(0f, false), new(1f, false), new(1f, false), new(0f, false));

        public value minX;
        public value minY;
        public value minZ;
        public value maxX;
        public value maxY;
        public value maxZ;

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

            public bool IsAbsolute
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
                    bool absolute = IsAbsolute;
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

            public unsafe value(float number, bool absolute)
            {
                ThrowIfOutOfRange(number);
                data = (int)(number * NumberRange) >> 1;
                if (absolute)
                {
                    data &= ~1;
                }
                else
                {
                    data |= 1;
                }
            }

            [Conditional("DEBUG")]
            private readonly void ThrowIfOutOfRange(float input)
            {
                if (input < -MaxNumberValue || input >= MaxNumberValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(input), $"Anchor value must be greater than {-MaxNumberValue} and below {MaxNumberValue}.");
                }
            }

            public readonly float Evaluate(float context)
            {
                float n = Number;
                if (IsAbsolute)
                {
                    return context + n;
                }
                else
                {
                    return context * n;
                }
            }
        }
    }
}
