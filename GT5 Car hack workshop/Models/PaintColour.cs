using System;
using Avalonia.Media;

namespace GT5_Car_hack_workshop.Models
{
    /// <summary>
    /// One 8-bit RGB colour plus every colour calculation the app needs (HSV and WCAG relative
    /// luminance), so those formulas exist in exactly one place.
    ///
    /// It is deliberately a small immutable value type: three bytes, short-lived, and often held in
    /// a list of thousands - which is what the framework design guidelines call a struct.
    /// </summary>
    public readonly record struct PaintColour(byte R, byte G, byte B)
    {
        /// <summary>Unpacks the 0xRRGGBB form that PaintEntry.Rgb uses.</summary>
        public static PaintColour FromPacked(uint rgb)
            => new((byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);

        /// <summary>
        /// Unpacks a nullable 0xRRGGBB value. A colour the game did not record (null) becomes black,
        /// which keeps every formula below defined instead of throwing or returning NaN.
        /// </summary>
        public static PaintColour FromPacked(uint? rgb)
            => rgb is { } packed ? FromPacked(packed) : default;

        /// <summary>This colour as an Avalonia colour, for swatches.</summary>
        public Color ToColor() => Color.FromRgb(R, G, B);

        private double Max => Math.Max(R, Math.Max(G, B));

        private double Min => Math.Min(R, Math.Min(G, B));

        /// <summary>
        /// True when the colour has no hue at all (it is grey, white or black): every channel is the
        /// same, so the HSV hue is mathematically undefined. Such colours are grouped at the end of
        /// the hue and saturation sorts so the top of the list is always actual colour.
        /// </summary>
        public bool IsNeutral => Max == Min;

        // ---- HSV. Hue is in degrees 0..360; saturation and value are 0..1. ----

        public double Value => Max / 255.0;

        public double Saturation
        {
            get
            {
                var max = Max;
                return max == 0 ? 0 : (max - Min) / max;
            }
        }

        public double Hue
        {
            get
            {
                double r = R, g = G, b = B;
                var max = Max;
                var delta = max - Min;
                if (delta == 0) return 0; // no hue to speak of; IsNeutral says so

                var hue = max == r ? (g - b) / delta
                        : max == g ? (b - r) / delta + 2
                                   : (r - g) / delta + 4;
                hue *= 60.0;
                return hue < 0 ? hue + 360.0 : hue;
            }
        }

        public (double Hue, double Saturation, double Value) ToHsv() => (Hue, Saturation, Value);

        // ---- WCAG 2.x relative luminance (0 for black, 1 for white). ----

        /// <summary>
        /// The perceived brightness of the colour: the sRGB channels are linearised (undoing the
        /// gamma curve, which is why mid grey is about 0.216 rather than 0.5) and then weighted for
        /// the eye, which is far more sensitive to green than to blue.
        /// </summary>
        public double RelativeLuminance
        {
            get
            {
                static double Linear(byte channel)
                {
                    var c = channel / 255.0;
                    return c <= 0.04045 ? c / 12.92 : Math.Pow((c + 0.055) / 1.055, 2.4);
                }

                return 0.2126 * Linear(R) + 0.7152 * Linear(G) + 0.0722 * Linear(B);
            }
        }
    }
}
