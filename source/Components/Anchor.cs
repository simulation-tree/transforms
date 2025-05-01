#pragma warning disable CS8981
using System;
using System.Diagnostics;

namespace Transforms.Components
{
    /// <summary>
    /// Overrides transformation logic for positioning elements relative
    /// or absolute to the parent element.
    /// </summary>
    public struct Anchor : IEquatable<Anchor>
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
        public static readonly Anchor Default = default;

        public Number minX;
        public Number minY;
        public Number minZ;
        public Number maxX;
        public Number maxY;
        public Number maxZ;

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

        public Anchor(Number minX, Number minY, Number minZ, Number maxX, Number maxY, Number maxZ)
        {
            this.minX = minX;
            this.minY = minY;
            this.minZ = minZ;
            this.maxX = maxX;
            this.maxY = maxY;
            this.maxZ = maxZ;
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is Anchor anchor && Equals(anchor);
        }

        public readonly bool Equals(Anchor other)
        {
            return minX.Equals(other.minX) && minY.Equals(other.minY) && minZ.Equals(other.minZ) && maxX.Equals(other.maxX) && maxY.Equals(other.maxY) && maxZ.Equals(other.maxZ);
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(minX, minY, minZ, maxX, maxY, maxZ);
        }

        public static bool operator ==(Anchor left, Anchor right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Anchor left, Anchor right)
        {
            return !(left == right);
        }

        public struct Number : IEquatable<Number>
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

            public float Value
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

            public Number(float value, bool absolute)
            {
                ThrowIfOutOfRange(value);

                data = (int)(value * NumberRange) >> 1;
                if (absolute)
                {
                    data &= ~1;
                }
                else
                {
                    data |= 1;
                }
            }

            public Number(ReadOnlySpan<char> text)
            {
                bool negative = false;
                int index = 0;
                int startIndex = 0;
                bool foundNumber = false;
                bool absolute = true;
                int endIndex = 0;
                while (index < text.Length)
                {
                    char c = text[index];
                    if (c == '-')
                    {
                        negative = true;
                    }
                    else if (c == '.' || char.IsDigit(c))
                    {
                        if (!foundNumber)
                        {
                            startIndex = index;
                        }

                        foundNumber = true;
                        if (index == text.Length - 1)
                        {
                            endIndex = index + 1;
                            break;
                        }
                    }
                    else
                    {
                        endIndex = index;

                        if (c == '%')
                        {
                            absolute = false;
                            break;
                        }
                    }

                    index++;
                }

                if (!foundNumber)
                {
                    throw new FormatException($"No number found in text input `{text.ToString()}`");
                }

                float number = float.Parse(text.Slice(startIndex, endIndex - startIndex));
                if (negative)
                {
                    number = -number;
                }

                if (absolute)
                {
                    data &= ~1;
                }
                else
                {
                    data |= 1;
                }

                if (absolute)
                {
                    Value = number;
                }
                else
                {
                    Value = number / 100f;
                }
            }

            public readonly override string ToString()
            {
                Span<char> buffer = stackalloc char[32];
                int length = ToString(buffer);
                return buffer.Slice(0, length).ToString();
            }

            public readonly int ToString(Span<char> destination)
            {
                bool isRelative = IsAbsolute;
                float number = Value;
                int length = 0;
                if (isRelative)
                {
                    destination[0] = 'r';
                    destination[1] = ':';
                    length = 2;
                }
                else
                {
                    destination[0] = 'a';
                    destination[1] = ':';
                    length = 2;
                }

                length += number.ToString(destination.Slice(length));
                return length;
            }

            public readonly override bool Equals(object? obj)
            {
                return obj is Number value && Equals(value);
            }

            public readonly bool Equals(Number other)
            {
                return data == other.data;
            }

            public readonly override int GetHashCode()
            {
                return data.GetHashCode();
            }

            public static bool operator ==(Number left, Number right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(Number left, Number right)
            {
                return !(left == right);
            }

            public static implicit operator float(Number value)
            {
                return value.Value;
            }

            public static implicit operator Number(ReadOnlySpan<char> text)
            {
                return new(text);
            }

            public static implicit operator Number(string text)
            {
                return new(text.AsSpan());
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
