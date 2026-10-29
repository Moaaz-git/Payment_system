using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Payment_system
{
    /// <summary>
    /// Central visual language for the Pay_WU sign-in screen.
    /// Every value here is purely presentational.
    /// </summary>
    internal static class LoginTheme
    {
        // Pay_WU palette ---------------------------------------------------------
        public static readonly Color Ink            = Color.FromArgb(7, 5, 15);
        public static readonly Color BackTop        = Color.FromArgb(8, 5, 18);
        public static readonly Color BackBottom     = Color.FromArgb(15, 9, 31);
        public static readonly Color Accent         = Color.FromArgb(109, 74, 255);    // 6D4AFF
        public static readonly Color AccentBright   = Color.FromArgb(140, 112, 255);
        public static readonly Color AccentSoft     = Color.FromArgb(178, 158, 255);
        public static readonly Color Lavender       = Color.FromArgb(207, 193, 255);
        public static readonly Color Pink           = Color.FromArgb(236, 150, 255);
        public static readonly Color Mint           = Color.FromArgb(96, 226, 192);

        public static readonly Color TextPrimary    = Color.FromArgb(248, 245, 255);
        public static readonly Color TextSecondary  = Color.FromArgb(175, 164, 207);
        public static readonly Color TextMuted      = Color.FromArgb(133, 121, 166);
        public static readonly Color TextDim        = Color.FromArgb(94, 84, 122);

        public static readonly Color FieldFill      = Color.FromArgb(24, 16, 42);
        public static readonly Color FieldBorder    = Color.FromArgb(52, 41, 85);
        public static readonly Color FieldBorderHot = Color.FromArgb(94, 75, 147);
        public static readonly Color Placeholder    = Color.FromArgb(132, 120, 159);
        public static readonly Color CardEdge       = Color.FromArgb(255, 255, 255);

        public static readonly Color Danger         = Color.FromArgb(255, 92, 122);

        // Typography --------------------------------------------------------------
        private static readonly string DisplayFamily = ResolveFamily("Segoe UI Variable Display", "Segoe UI");
        private static readonly string BodyFamily    = ResolveFamily("Segoe UI Variable Text", "Segoe UI");

        public static Font Display(float size, FontStyle style)
        {
            return new Font(DisplayFamily, size, style, GraphicsUnit.Point);
        }

        public static Font Body(float size, FontStyle style)
        {
            return new Font(BodyFamily, size, style, GraphicsUnit.Point);
        }

        private static string ResolveFamily(string preferred, string fallback)
        {
            try
            {
                using (var fonts = new InstalledFontCollection())
                {
                    foreach (FontFamily family in fonts.Families)
                    {
                        if (string.Equals(family.Name, preferred, StringComparison.OrdinalIgnoreCase))
                        {
                            return preferred;
                        }
                    }
                }
            }
            catch
            {
                // Fall through to the safe system font.
            }

            return fallback;
        }

        // Geometry helpers --------------------------------------------------------
        public static GraphicsPath Rounded(RectangleF bounds, float radius)
        {
            var path = new GraphicsPath();
            if (bounds.Width <= 0f || bounds.Height <= 0f)
            {
                return path;
            }

            float diameter = Math.Max(0f, Math.Min(radius * 2f, Math.Min(bounds.Width, bounds.Height)));
            if (diameter < 1f)
            {
                path.AddRectangle(bounds);
                path.CloseFigure();
                return path;
            }

            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180f, 90f);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270f, 90f);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0f, 90f);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90f, 90f);
            path.CloseFigure();
            return path;
        }

        public static void Fill(Graphics g, Brush brush, RectangleF bounds, float radius)
        {
            using (GraphicsPath path = Rounded(bounds, radius))
            {
                g.FillPath(brush, path);
            }
        }

        public static void Stroke(Graphics g, Pen pen, RectangleF bounds, float radius)
        {
            using (GraphicsPath path = Rounded(bounds, radius))
            {
                g.DrawPath(pen, path);
            }
        }

        public static void Configure(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.GammaCorrected;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        }

        public static void RadialGlow(Graphics g, PointF center, float width, float height, Color color)
        {
            var bounds = new RectangleF(center.X - (width / 2f), center.Y - (height / 2f), width, height);
            using (var path = new GraphicsPath())
            {
                path.AddEllipse(bounds);
                using (var brush = new PathGradientBrush(path))
                {
                    brush.CenterPoint = center;
                    brush.CenterColor = color;
                    brush.SurroundColors = new[]
                    {
                        Color.FromArgb(0, color.R, color.G, color.B)
                    };
                    brush.FocusScales = new PointF(0.12f, 0.12f);
                    g.FillPath(brush, path);
                }
            }
        }

        public static void TrackedText(Graphics g, string text, Font font, Color color, float x, float y, float tracking)
        {
            using (var brush = new SolidBrush(color))
            using (var format = (StringFormat)StringFormat.GenericTypographic.Clone())
            {
                format.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.NoClip;
                float cursor = x;
                foreach (char character in text)
                {
                    string piece = character.ToString();
                    g.DrawString(piece, font, brush, cursor, y, format);
                    cursor += g.MeasureString(piece, font, PointF.Empty, format).Width + tracking;
                }
            }
        }

        public static float MeasureTracked(Graphics g, string text, Font font, float tracking)
        {
            float width = 0f;
            using (var format = (StringFormat)StringFormat.GenericTypographic.Clone())
            {
                format.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.NoClip;
                foreach (char character in text)
                {
                    width += g.MeasureString(character.ToString(), font, PointF.Empty, format).Width + tracking;
                }
            }

            return width;
        }

        public static LinearGradientBrush HorizontalGradient(RectangleF bounds, Color left, Color right)
        {
            return new LinearGradientBrush(
                new PointF(bounds.Left, bounds.Top),
                new PointF(bounds.Right, bounds.Top),
                left,
                right);
        }
    }
}
