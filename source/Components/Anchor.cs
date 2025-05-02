using System;

namespace Transforms.Components
{
    /// <summary>
    /// Overrides transformation logic for positioning elements relative
    /// or absolute to the parent element.
    /// </summary>
    public struct Anchor : IEquatable<Anchor>
    {
        public static readonly Anchor Centered = new(0.5f, 0.5f, 0f, 0.5f, 0.5f, 0f);
        public static readonly Anchor BottomLeft = new(0f, 0f, 0f, 0f, 0f, 0f);
        public static readonly Anchor TopRight = new(1f, 1f, 0f, 1f, 1f, 0f);
        public static readonly Anchor BottomRight = new(1f, 0f, 0f, 1f, 0f, 0f);
        public static readonly Anchor TopLeft = new(0f, 1f, 0f, 0f, 1f, 0f);
        public static readonly Anchor Bottom = new(0.5f, 0f, 0f, 0.5f, 0f, 0f);
        public static readonly Anchor Top = new(0.5f, 1f, 0f, 0.5f, 1f, 0f);
        public static readonly Anchor Left = new(0f, 0.5f, 0f, 0f, 0.5f, 0f);
        public static readonly Anchor Right = new(1f, 0.5f, 0f, 1f, 0.5f, 0f);
        public static readonly Anchor Default = default;

        public float minX;
        public float minY;
        public float minZ;
        public float maxX;
        public float maxY;
        public float maxZ;
        public Relativeness flags;

        public Anchor(float minX, float minY, float minZ, float maxX, float maxY, float maxZ, Relativeness flags = default)
        {
            this.minX = minX;
            this.minY = minY;
            this.minZ = minZ;
            this.maxX = maxX;
            this.maxY = maxY;
            this.maxZ = maxZ;
            this.flags = flags;
        }

        public Anchor(ReadOnlySpan<char> minX, ReadOnlySpan<char> minY, ReadOnlySpan<char> minZ, ReadOnlySpan<char> maxX, ReadOnlySpan<char> maxY, ReadOnlySpan<char> maxZ)
        {
            if (TryParse(minX, out this.minX, out bool relative) && relative)
            {
                flags |= Relativeness.MinX;
            }

            if (TryParse(minY, out this.minY, out relative) && relative)
            {
                flags |= Relativeness.MinY;
            }

            if (TryParse(minZ, out this.minZ, out relative) && relative)
            {
                flags |= Relativeness.MinZ;
            }

            if (TryParse(maxX, out this.maxX, out relative) && relative)
            {
                flags |= Relativeness.MaxX;
            }

            if (TryParse(maxY, out this.maxY, out relative) && relative)
            {
                flags |= Relativeness.MaxY;
            }

            if (TryParse(maxZ, out this.maxZ, out relative) && relative)
            {
                flags |= Relativeness.MaxZ;
            }
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is Anchor anchor && Equals(anchor);
        }

        public readonly bool Equals(Anchor other)
        {
            return minX.Equals(other.minX) && minY.Equals(other.minY) && minZ.Equals(other.minZ) && maxX.Equals(other.maxX) && maxY.Equals(other.maxY) && maxZ.Equals(other.maxZ) && flags == other.flags;
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(minX, minY, minZ, maxX, maxY, maxZ, flags);
        }

        public static bool TryParse(ReadOnlySpan<char> text, out float number, out bool relative)
        {
            if (text.Length > 0)
            {
                if (text[text.Length - 1] == '%')
                {
                    relative = true;
                    text = text.Slice(0, text.Length - 1);
                }
                else
                {
                    relative = false;
                }

                return float.TryParse(text, out number);
            }
            else
            {
                number = default;
                relative = false;
                return false;
            }
        }

        public static bool operator ==(Anchor left, Anchor right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Anchor left, Anchor right)
        {
            return !(left == right);
        }

        [Flags]
        public enum Relativeness : byte
        {
            None = 0,
            MinX = 1,
            MinY = 2,
            MinZ = 4,
            MaxX = 8,
            MaxY = 16,
            MaxZ = 32,
            X = MinX | MaxX,
            Y = MinY | MaxY,
            Z = MinZ | MaxZ,
            All = MinX | MinY | MinZ | MaxX | MaxY | MaxZ
        }
    }
}