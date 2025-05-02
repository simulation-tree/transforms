using System;

namespace Transforms.Components
{
    /// <summary>
    /// Overrides transformation logic for positioning elements relative
    /// or absolute to the parent element.
    /// </summary>
    public struct Anchor : IEquatable<Anchor>
    {
        /// <summary>
        /// Centers the transform on the X and Y axes.
        /// </summary>
        public static readonly Anchor Centered = new(0.5f, 0.5f, 0f, 0.5f, 0.5f, 0f);

        /// <summary>
        /// Anchors the transform to the bottom left (also the <see langword="default"/>).
        /// </summary>
        public static readonly Anchor BottomLeft = new(0f, 0f, 0f, 0f, 0f, 0f);

        /// <summary>
        /// Anchors the transform to the top right.
        /// </summary>
        public static readonly Anchor TopRight = new(1f, 1f, 0f, 1f, 1f, 0f);

        /// <summary>
        /// Anchors the transform to the bottom right.
        /// </summary>
        public static readonly Anchor BottomRight = new(1f, 0f, 0f, 1f, 0f, 0f);

        /// <summary>
        /// Anchors the transform to the top left.
        /// </summary>
        public static readonly Anchor TopLeft = new(0f, 1f, 0f, 0f, 1f, 0f);

        /// <summary>
        /// Anchors the transform to the center bottom.
        /// </summary>
        public static readonly Anchor Bottom = new(0.5f, 0f, 0f, 0.5f, 0f, 0f);

        /// <summary>
        /// Anchors the transform to the center top.
        /// </summary>
        public static readonly Anchor Top = new(0.5f, 1f, 0f, 0.5f, 1f, 0f);

        /// <summary>
        /// Anchors the transform to the left middle.
        /// </summary>
        public static readonly Anchor Left = new(0f, 0.5f, 0f, 0f, 0.5f, 0f);

        /// <summary>
        /// Anchors the transform to the right middle.
        /// </summary>
        public static readonly Anchor Right = new(1f, 0.5f, 0f, 1f, 0.5f, 0f);

        /// <summary>
        /// Default value.
        /// </summary>
        public static readonly Anchor Default = default;

        /// <summary>
        /// Minimum X anchor value.
        /// </summary>
        public float minX;

        /// <summary>
        /// Minimum Y anchor value.
        /// </summary>
        public float minY;

        /// <summary>
        /// Minimum Z anchor value.
        /// </summary>
        public float minZ;

        /// <summary>
        /// Maximum X anchor value.
        /// </summary>
        public float maxX;

        /// <summary>
        /// Maximum Y anchor value.
        /// </summary>
        public float maxY;

        /// <summary>
        /// Maximum Z anchor value.
        /// </summary>
        public float maxZ;

        /// <summary>
        /// Flags defining which anchor values are relative.
        /// </summary>
        public Relativeness flags;

        /// <summary>
        /// Creates an instance of the anchor component.
        /// </summary>
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

        /// <summary>
        /// Creates an instance of the anchor component from the parsed values.
        /// </summary>
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

        /// <inheritdoc/>
        public readonly override bool Equals(object? obj)
        {
            return obj is Anchor anchor && Equals(anchor);
        }

        /// <inheritdoc/>
        public readonly bool Equals(Anchor other)
        {
            return minX.Equals(other.minX) && minY.Equals(other.minY) && minZ.Equals(other.minZ) && maxX.Equals(other.maxX) && maxY.Equals(other.maxY) && maxZ.Equals(other.maxZ) && flags == other.flags;
        }

        /// <inheritdoc/>
        public readonly override int GetHashCode()
        {
            return HashCode.Combine(minX, minY, minZ, maxX, maxY, maxZ, flags);
        }

        /// <summary>
        /// Tries to parse the given <paramref name="text"/> as a single anchor number.
        /// </summary>
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

        /// <inheritdoc/>
        public static bool operator ==(Anchor left, Anchor right)
        {
            return left.Equals(right);
        }

        /// <inheritdoc/>
        public static bool operator !=(Anchor left, Anchor right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Settings for describing which anchor numbers are relative to the parent bounds.
        /// </summary>
        [Flags]
        public enum Relativeness : byte
        {
            /// <summary>
            /// No values are relative to the parent.
            /// </summary>
            None = 0,

            /// <summary>
            /// The minimum X number is relative to the parent's left edge.
            /// </summary>
            MinX = 1,

            /// <summary>
            /// The minimum Y number is relative to the parent's bottom edge.
            /// </summary>
            MinY = 2,

            /// <summary>
            /// The minimum Z number is relative to the parent's back edge.
            /// </summary>
            MinZ = 4,

            /// <summary>
            /// The maximum X number is relative to the parent's right edge.
            /// </summary>
            MaxX = 8,

            /// <summary>
            /// The maximum Y number is relative to the parent's top edge.
            /// </summary>
            MaxY = 16,

            /// <summary>
            /// The maximum Z number is relative to the parent's front edge.
            /// </summary>
            MaxZ = 32,

            /// <summary>
            /// Both minimum and maximum X numbers are relative to the parent's left and right edges.
            /// </summary>
            X = MinX | MaxX,

            /// <summary>
            /// Both minimum and maximum Y numbers are relative to the parent's bottom and top edges.
            /// </summary>
            Y = MinY | MaxY,

            /// <summary>
            /// Both minimum and maximum Z numbers are relative to the parent's back and front edges.
            /// </summary>
            Z = MinZ | MaxZ,

            /// <summary>
            /// All minimum and maximum numbers are relative to the parent's edges.
            /// </summary>
            All = MinX | MinY | MinZ | MaxX | MaxY | MaxZ
        }
    }
}