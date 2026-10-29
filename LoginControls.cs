using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Payment_system
{
    /// <summary>
    /// Small vector icon factory used by the custom painted controls.
    /// </summary>
    internal static class LoginArt
    {
        public static GraphicsPath Shield(RectangleF r)
        {
            float cx = r.Left + (r.Width / 2f);
            var path = new GraphicsPath();
            path.StartFigure();
            path.AddBezier(cx, r.Top,
                cx + (r.Width * 0.34f), r.Top + (r.Height * 0.05f),
                r.Right - (r.Width * 0.07f), r.Top + (r.Height * 0.14f),
                r.Right - (r.Width * 0.07f), r.Top + (r.Height * 0.36f));
            path.AddLine(r.Right - (r.Width * 0.07f), r.Top + (r.Height * 0.36f),
                r.Right - (r.Width * 0.07f), r.Top + (r.Height * 0.55f));
            path.AddBezier(r.Right - (r.Width * 0.07f), r.Top + (r.Height * 0.55f),
                r.Right - (r.Width * 0.10f), r.Bottom - (r.Height * 0.14f),
                cx + (r.Width * 0.22f), r.Bottom - (r.Height * 0.03f),
                cx, r.Bottom);
            path.AddBezier(cx, r.Bottom,
                cx - (r.Width * 0.22f), r.Bottom - (r.Height * 0.03f),
                r.Left + (r.Width * 0.10f), r.Bottom - (r.Height * 0.14f),
                r.Left + (r.Width * 0.07f), r.Top + (r.Height * 0.55f));
            path.AddLine(r.Left + (r.Width * 0.07f), r.Top + (r.Height * 0.55f),
                r.Left + (r.Width * 0.07f), r.Top + (r.Height * 0.36f));
            path.AddBezier(r.Left + (r.Width * 0.07f), r.Top + (r.Height * 0.14f),
                cx - (r.Width * 0.34f), r.Top + (r.Height * 0.05f),
                cx, r.Top,
                cx, r.Top);
            path.CloseFigure();
            return path;
        }

        public static void ShieldWithKeyhole(Graphics g, RectangleF bounds, Color top, Color bottom, Color keyhole)
        {
            using (GraphicsPath shield = Shield(bounds))
            using (var fill = new LinearGradientBrush(bounds, top, bottom, LinearGradientMode.Vertical))
            {
                g.FillPath(fill, shield);
            }

            float cx = bounds.Left + (bounds.Width / 2f);
            float cy = bounds.Top + (bounds.Height * 0.42f);
            float lockSize = bounds.Width * 0.20f;
            using (var dot = new SolidBrush(keyhole))
            {
                g.FillEllipse(dot, cx - (lockSize / 2f), cy - (lockSize / 2f), lockSize, lockSize);
                var stem = new GraphicsPath();
                float stemTop = cy + (lockSize * 0.15f);
                float stemBottom = cy + (lockSize * 1.25f);
                stem.AddPolygon(new[]
                {
                    new PointF(cx - (lockSize * 0.26f), stemTop),
                    new PointF(cx + (lockSize * 0.26f), stemTop),
                    new PointF(cx + (lockSize * 0.13f), stemBottom),
                    new PointF(cx - (lockSize * 0.13f), stemBottom)
                });
                g.FillPath(dot, stem);
                stem.Dispose();
            }
        }

        public static void Envelope(Graphics g, RectangleF r, Color color)
        {
            using (var pen = new Pen(color, 1.35f))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                pen.LineJoin = LineJoin.Round;
                using (GraphicsPath body = LoginTheme.Rounded(r, 2.4f))
                {
                    g.DrawPath(pen, body);
                }

                g.DrawLines(pen, new[]
                {
                    new PointF(r.Left + 2f, r.Top + 3.2f),
                    new PointF(r.Left + (r.Width / 2f), r.Top + 7.4f),
                    new PointF(r.Right - 2f, r.Top + 3.2f)
                });
            }
        }

        public static void Lock(Graphics g, RectangleF r, Color color)
        {
            using (var pen = new Pen(color, 1.35f))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                pen.LineJoin = LineJoin.Round;
                float bodyTop = r.Top + (r.Height * 0.42f);
                using (GraphicsPath body = LoginTheme.Rounded(
                    new RectangleF(r.Left + 1f, bodyTop, r.Width - 2f, r.Bottom - bodyTop), 2.6f))
                {
                    g.DrawPath(pen, body);
                }

                float shackleWidth = r.Width * 0.52f;
                float shackleHeight = r.Height * 0.46f;
                var shackle = new RectangleF(
                    r.Left + ((r.Width - shackleWidth) / 2f),
                    r.Top + 1.1f,
                    shackleWidth,
                    shackleHeight);
                g.DrawArc(pen, shackle, 180f, 180f);
                g.DrawLine(pen, shackle.Left, shackle.Top + (shackle.Height / 2f),
                    shackle.Left, bodyTop + 0.6f);
                g.DrawLine(pen, shackle.Right, shackle.Top + (shackle.Height / 2f),
                    shackle.Right, bodyTop + 0.6f);

                using (var dot = new SolidBrush(color))
                {
                    g.FillEllipse(dot, r.Left + (r.Width / 2f) - 1.4f, bodyTop + 3.8f, 2.8f, 2.8f);
                }
            }
        }

        public static void Key(Graphics g, RectangleF r, Color color)
        {
            using (var pen = new Pen(color, 1.45f))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                pen.LineJoin = LineJoin.Round;
                float head = r.Height * 0.46f;
                g.DrawEllipse(pen, r.Left, r.Top + ((r.Height - head) / 2f), head, head);
                float shaftY = r.Top + (r.Height / 2f);
                g.DrawLine(pen, r.Left + head, shaftY, r.Right, shaftY);
                g.DrawLine(pen, r.Right - 3.2f, shaftY, r.Right - 3.2f, shaftY + 4.2f);
                g.DrawLine(pen, r.Right - 7.2f, shaftY, r.Right - 7.2f, shaftY + 3.2f);
            }
        }

        public static void Eye(Graphics g, RectangleF r, Color color, bool off)
        {
            using (var pen = new Pen(color, 1.35f))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                var eye = new GraphicsPath();
                eye.AddBezier(r.Left, r.Top + (r.Height / 2f),
                    r.Left + (r.Width * 0.30f), r.Top,
                    r.Left + (r.Width * 0.70f), r.Top,
                    r.Right, r.Top + (r.Height / 2f));
                eye.AddBezier(r.Right, r.Top + (r.Height / 2f),
                    r.Left + (r.Width * 0.70f), r.Bottom,
                    r.Left + (r.Width * 0.30f), r.Bottom,
                    r.Left, r.Top + (r.Height / 2f));
                g.DrawPath(pen, eye);
                eye.Dispose();

                float cx = r.Left + (r.Width / 2f);
                float cy = r.Top + (r.Height / 2f);
                using (var dot = new SolidBrush(color))
                {
                    g.FillEllipse(dot, cx - 2.2f, cy - 2.2f, 4.4f, 4.4f);
                }

                if (off)
                {
                    g.DrawLine(pen, r.Left + 1f, r.Bottom - 0.5f, r.Right - 1f, r.Top + 0.5f);
                }
            }
        }

        public static void Check(Graphics g, RectangleF r, Color color, float width)
        {
            using (var pen = new Pen(color, width))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                pen.LineJoin = LineJoin.Round;
                g.DrawLines(pen, new[]
                {
                    new PointF(r.Left + (r.Width * 0.22f), r.Top + (r.Height * 0.53f)),
                    new PointF(r.Left + (r.Width * 0.43f), r.Top + (r.Height * 0.73f)),
                    new PointF(r.Left + (r.Width * 0.80f), r.Top + (r.Height * 0.27f))
                });
            }
        }

        public static void ArrowRight(Graphics g, PointF start, Color color)
        {
            using (var pen = new Pen(color, 1.7f))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                pen.LineJoin = LineJoin.Round;
                g.DrawLine(pen, start.X, start.Y, start.X + 14f, start.Y);
                g.DrawLines(pen, new[]
                {
                    new PointF(start.X + 9f, start.Y - 4.2f),
                    new PointF(start.X + 14f, start.Y),
                    new PointF(start.X + 9f, start.Y + 4.2f)
                });
            }
        }
    }

    // -------------------------------------------------------------------------
    // Brand mark
    // -------------------------------------------------------------------------
    internal sealed class PayWUBrand : Control
    {
        private readonly Font _payFont = LoginTheme.Display(18.5f, FontStyle.Bold);
        private readonly Font _wuFont = LoginTheme.Body(18.5f, FontStyle.Regular);
        private readonly Font _markFont = LoginTheme.Display(26f, FontStyle.Bold);

        public PayWUBrand()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;
            Size = new Size(260, 46);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LoginTheme.Configure(g);

            var tile = new RectangleF(0f, 0f, 46f, 46f);
            using (GraphicsPath tilePath = LoginTheme.Rounded(tile, 14f))
            {
                using (var outerGlow = new Pen(Color.FromArgb(40, 151, 124, 255), 5f))
                {
                    g.DrawPath(outerGlow, tilePath);
                }

                using (var fill = new LinearGradientBrush(tile,
                    Color.FromArgb(126, 92, 255), Color.FromArgb(81, 48, 219), 62f))
                {
                    g.FillPath(fill, tilePath);
                }

                using (var innerShine = new LinearGradientBrush(
                    new RectangleF(0f, 0f, 46f, 25f),
                    Color.FromArgb(55, 255, 255, 255),
                    Color.FromArgb(0, 255, 255, 255),
                    LinearGradientMode.Vertical))
                {
                    g.FillPath(innerShine, tilePath);
                }

                using (var border = new Pen(Color.FromArgb(100, 255, 255, 255), 1.2f))
                {
                    g.DrawPath(border, tilePath);
                }
            }

            using (var markBrush = new LinearGradientBrush(
                new RectangleF(12f, 7f, 22f, 32f),
                Color.FromArgb(255, 251, 255),
                Color.FromArgb(199, 181, 255),
                90f))
            using (var markFormat = (StringFormat)StringFormat.GenericTypographic.Clone())
            {
                markFormat.Alignment = StringAlignment.Center;
                markFormat.LineAlignment = StringAlignment.Center;
                markFormat.FormatFlags |= StringFormatFlags.NoClip;
                g.DrawString("P", _markFont, markBrush, tile, markFormat);
            }

            float textY = (Height - _payFont.GetHeight(g)) / 2f - 1.5f;
            using (var brush = new SolidBrush(LoginTheme.TextPrimary))
            {
                g.DrawString("Pay", _payFont, brush, 59f, textY,
                    StringFormat.GenericTypographic);
            }

            float payWidth = g.MeasureString("Pay", _payFont, PointF.Empty,
                StringFormat.GenericTypographic).Width;

            using (var brush = new SolidBrush(Color.FromArgb(184, 167, 226)))
            {
                g.DrawString("_WU", _wuFont, brush, 59f + payWidth, textY,
                    StringFormat.GenericTypographic);
            }
        }
    }

    // -------------------------------------------------------------------------
    // Tiny overline above the hero headline
    // -------------------------------------------------------------------------
    internal sealed class Eyebrow : Control
    {
        private readonly Font _font = LoginTheme.Body(7.9f, FontStyle.Bold);

        public Eyebrow()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;
            Size = new Size(260, 22);
            Text = "PAYMENTS, SIMPLIFIED";
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LoginTheme.Configure(g);

            using (var line = new LinearGradientBrush(
                new RectangleF(0f, 0f, 28f, 2f),
                Color.FromArgb(230, 152, 126, 255),
                Color.FromArgb(0, 152, 126, 255),
                LinearGradientMode.Horizontal))
            {
                g.FillRectangle(line, 0f, 10f, 28f, 1.4f);
            }

            LoginTheme.TrackedText(g, Text, _font,
                Color.FromArgb(190, 170, 216), 38f, 3.2f, 1.45f);
        }
    }

    // -------------------------------------------------------------------------
    // Hero headline with a gradient second line
    // -------------------------------------------------------------------------
    internal sealed class GradientHeadline : Control
    {
        private readonly Font _font = LoginTheme.Display(41f, FontStyle.Bold);

        public GradientHeadline()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;
            Size = new Size(555, 138);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LoginTheme.Configure(g);
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            using (var format = (StringFormat)StringFormat.GenericTypographic.Clone())
            {
                format.FormatFlags |= StringFormatFlags.NoClip;

                using (var brush = new SolidBrush(Color.FromArgb(247, 243, 255)))
                {
                    g.DrawString("Payments that", _font, brush, 0f, -4f, format);
                }

                string second = "move with you.";
                float width = g.MeasureString(second, _font, PointF.Empty, format).Width;
                var gradientRect = new RectangleF(-2f, 0f, width + 10f, _font.GetHeight(g));
                using (var brush = new LinearGradientBrush(
                    gradientRect,
                    Color.FromArgb(205, 186, 255),
                    Color.FromArgb(238, 159, 255),
                    LinearGradientMode.Horizontal))
                {
                    var blend = new ColorBlend(3);
                    blend.Colors = new[]
                    {
                        Color.FromArgb(214, 199, 255),
                        Color.FromArgb(156, 122, 255),
                        Color.FromArgb(238, 157, 255)
                    };
                    blend.Positions = new[] { 0f, 0.48f, 1f };
                    brush.InterpolationColors = blend;
                    g.DrawString(second, _font, brush, -2f, 60f, format);
                }
            }
        }
    }

    // -------------------------------------------------------------------------
    // Left column feature row
    // -------------------------------------------------------------------------
    internal sealed class FeatureRow : Control
    {
        private readonly Font _font = LoginTheme.Body(10f, FontStyle.Regular);

        public FeatureRow()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;
            Size = new Size(360, 34);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LoginTheme.Configure(g);

            var ring = new RectangleF(0.5f, 5.5f, 22f, 22f);
            using (var fill = new SolidBrush(Color.FromArgb(16, 255, 255, 255)))
            using (GraphicsPath path = LoginTheme.Rounded(ring, 11f))
            {
                g.FillPath(fill, path);
                using (var border = new Pen(Color.FromArgb(48, 255, 255, 255), 1f))
                {
                    g.DrawPath(border, path);
                }
            }

            LoginArt.Check(g, new RectangleF(5f, 10f, 13f, 13f),
                Color.FromArgb(194, 171, 255), 1.55f);

            using (var brush = new SolidBrush(Color.FromArgb(205, 195, 228)))
            using (var format = (StringFormat)StringFormat.GenericTypographic.Clone())
            {
                format.LineAlignment = StringAlignment.Center;
                g.DrawString(Text, _font, brush,
                    new RectangleF(34f, 0f, Width - 34f, Height), format);
            }
        }
    }

    // -------------------------------------------------------------------------
    // Bottom-left security note
    // -------------------------------------------------------------------------
    internal sealed class StatusNote : Control
    {
        private readonly Font _font = LoginTheme.Body(9.1f, FontStyle.Regular);

        public StatusNote()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;
            Size = new Size(450, 24);
            Text = "Secure payments. Trusted worldwide.";
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LoginTheme.Configure(g);

            LoginArt.Lock(g, new RectangleF(0f, 4.5f, 12f, 14f),
                Color.FromArgb(137, 123, 170));

            using (var brush = new SolidBrush(Color.FromArgb(142, 129, 176)))
            using (var format = (StringFormat)StringFormat.GenericTypographic.Clone())
            {
                format.LineAlignment = StringAlignment.Center;
                g.DrawString(Text, _font, brush,
                    new RectangleF(20f, 0f, Width - 20f, Height), format);
            }
        }
    }

    // -------------------------------------------------------------------------
    // Glass badge with the Pay_WU mark
    // -------------------------------------------------------------------------
    internal sealed class SecurityBadge : Control
    {
        private readonly Font _markFont = LoginTheme.Display(26f, FontStyle.Bold);

        public SecurityBadge()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;
            Size = new Size(46, 46);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LoginTheme.Configure(g);

            var tile = new RectangleF(0.5f, 0.5f, Width - 1f, Height - 1f);
            using (GraphicsPath path = LoginTheme.Rounded(tile, 14f))
            {
                using (var glow = new Pen(Color.FromArgb(34, 151, 123, 255), 4f))
                {
                    g.DrawPath(glow, path);
                }

                using (var fill = new LinearGradientBrush(tile,
                    Color.FromArgb(55, 38, 92), Color.FromArgb(28, 18, 51), 55f))
                {
                    g.FillPath(fill, path);
                }

                using (var shine = new LinearGradientBrush(
                    new RectangleF(0f, 0f, Width, Height * 0.55f),
                    Color.FromArgb(28, 255, 255, 255),
                    Color.FromArgb(0, 255, 255, 255),
                    LinearGradientMode.Vertical))
                {
                    g.FillPath(shine, path);
                }

                using (var border = new LinearGradientBrush(tile,
                    Color.FromArgb(100, 213, 197, 255), Color.FromArgb(28, 255, 255, 255), 50f))
                using (var pen = new Pen(border, 1.1f))
                {
                    g.DrawPath(pen, path);
                }
            }

            using (var markBrush = new LinearGradientBrush(
                new RectangleF(12f, 7f, 22f, 32f),
                Color.FromArgb(255, 251, 255),
                Color.FromArgb(199, 181, 255),
                90f))
            using (var markFormat = (StringFormat)StringFormat.GenericTypographic.Clone())
            {
                markFormat.Alignment = StringAlignment.Center;
                markFormat.LineAlignment = StringAlignment.Center;
                markFormat.FormatFlags |= StringFormatFlags.NoClip;
                g.DrawString("P", _markFont, markBrush, tile, markFormat);
            }
        }
    }

    // -------------------------------------------------------------------------
    // SECURE PAYMENT capsule
    // -------------------------------------------------------------------------
    internal sealed class StatusPill : Control
    {
        private readonly Font _font = LoginTheme.Body(7.1f, FontStyle.Bold);

        public StatusPill()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;
            Size = new Size(118, 24);
            Text = "SECURE PAYMENT";
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LoginTheme.Configure(g);

            var pill = new RectangleF(0.5f, 0.5f, Width - 1f, Height - 1f);
            using (GraphicsPath path = LoginTheme.Rounded(pill, 12f))
            {
                using (var fill = new SolidBrush(Color.FromArgb(13, 255, 255, 255)))
                {
                    g.FillPath(fill, path);
                }

                using (var border = new Pen(Color.FromArgb(30, 255, 255, 255), 1f))
                {
                    g.DrawPath(border, path);
                }
            }

            float textWidth = LoginTheme.MeasureTracked(g, Text, _font, 1.25f);
            const float dot = 5.2f;
            const float gap = 7f;
            float total = dot + gap + textWidth;
            float x = (Width - total) / 2f;
            float cy = Height / 2f;

            using (var halo = new SolidBrush(Color.FromArgb(28, 96, 226, 192)))
            {
                g.FillEllipse(halo, x - 2f, cy - (dot / 2f) - 2f, dot + 4f, dot + 4f);
            }

            using (var dotBrush = new SolidBrush(LoginTheme.Mint))
            {
                g.FillEllipse(dotBrush, x, cy - (dot / 2f), dot, dot);
            }

            LoginTheme.TrackedText(g, Text, _font,
                Color.FromArgb(180, 169, 207), x + dot + gap, (Height - _font.GetHeight(g)) / 2f - 0.5f, 1.25f);
        }
    }

    // -------------------------------------------------------------------------
    // Text field with a real (borderless) textbox inside
    // -------------------------------------------------------------------------
    internal enum FieldIcon
    {
        Mail,
        Lock,
        Amount
    }

    internal sealed class GlassInput : Control
    {
        private readonly TextBox _textBox;
        private readonly Label _placeholder;
        private readonly Font _valueFont = LoginTheme.Body(10.6f, FontStyle.Regular);
        private readonly Font _hintFont = LoginTheme.Body(10.2f, FontStyle.Regular);
        private readonly Font _amountFont = LoginTheme.Display(12.2f, FontStyle.Bold);
        private bool _focused;
        private bool _hovered;
        private bool _reveal;
        private FieldIcon _icon = FieldIcon.Mail;

        public GlassInput()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;
            Size = new Size(366, 50);
            TabStop = false;

            _textBox = new TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor = LoginTheme.FieldFill,
                ForeColor = Color.FromArgb(246, 242, 255),
                Font = _valueFont,
                AutoSize = false,
                TabStop = true,
                TabIndex = 0,
                ShortcutsEnabled = true
            };
            _textBox.GotFocus += OnInnerFocus;
            _textBox.LostFocus += OnInnerBlur;
            _textBox.TextChanged += OnInnerTextChanged;
            _textBox.MouseEnter += (s, e) => SetHover(true);
            _textBox.MouseLeave += (s, e) => SetHover(false);
            Controls.Add(_textBox);

            _placeholder = new Label
            {
                AutoSize = false,
                BackColor = LoginTheme.FieldFill,
                ForeColor = LoginTheme.Placeholder,
                Font = _hintFont,
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.IBeam
            };
            _placeholder.Click += (s, e) =>
            {
                _textBox.Focus();
                _textBox.SelectionStart = _textBox.TextLength;
            };
            Controls.Add(_placeholder);

            MouseEnter += (s, e) => SetHover(true);
            MouseLeave += (s, e) => SetHover(false);
            MouseDown += OnFieldMouseDown;
            MouseMove += OnFieldMouseMove;
        }

        public string PlaceholderText
        {
            get { return _placeholder.Text; }
            set
            {
                _placeholder.Text = value ?? string.Empty;
                PerformLayout();
                Invalidate();
            }
        }

        public bool IsPassword { get; set; }

        public FieldIcon Icon
        {
            get { return _icon; }
            set { _icon = value; Invalidate(); }
        }

        public TextBox InnerTextBox
        {
            get { return _textBox; }
        }

        public override string Text
        {
            get { return _textBox.Text; }
            set { _textBox.Text = value ?? string.Empty; }
        }

        public string AccessibleFieldName
        {
            get { return AccessibleName; }
            set
            {
                AccessibleName = value;
                _textBox.AccessibleName = value;
            }
        }

        private void OnInnerFocus(object sender, EventArgs e)
        {
            _focused = true;
            UpdatePlaceholder();
            Invalidate();
        }

        private void OnInnerBlur(object sender, EventArgs e)
        {
            _focused = false;
            UpdatePlaceholder();
            Invalidate();
        }

        private void OnInnerTextChanged(object sender, EventArgs e)
        {
            UpdatePlaceholder();
            Invalidate();
        }

        private void SetHover(bool hovered)
        {
            if (_hovered == hovered)
            {
                return;
            }

            _hovered = hovered;
            Invalidate();
        }

        private void OnFieldMouseDown(object sender, MouseEventArgs e)
        {
            if (IsPassword && e.Button == MouseButtons.Left && GetEyeBounds().Contains(e.Location))
            {
                _reveal = !_reveal;
                _textBox.UseSystemPasswordChar = !_reveal;
                _textBox.Focus();
                _textBox.SelectionStart = _textBox.TextLength;
                Cursor = Cursors.Hand;
                Invalidate();
                return;
            }

            Cursor = Cursors.IBeam;
            _textBox.Focus();
        }

        private void OnFieldMouseMove(object sender, MouseEventArgs e)
        {
            Cursor = IsPassword && GetEyeBounds().Contains(e.Location)
                ? Cursors.Hand
                : Cursors.IBeam;
        }

        private Rectangle GetEyeBounds()
        {
            return new Rectangle(Width - 40, 0, 30, Height);
        }

        private void UpdatePlaceholder()
        {
            bool show = string.IsNullOrEmpty(_textBox.Text) && !_textBox.Focused;
            _placeholder.Visible = show;
            if (show)
            {
                _placeholder.BringToFront();
            }
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);

            int left = 50;
            int right = IsPassword ? 46 : 22;
            int height = 26;
            int top = (Height - height) / 2;

            if (_textBox == null || _placeholder == null)
            {
                return;
            }

            _textBox.SetBounds(left, top, Math.Max(20, Width - left - right), height);
            _textBox.UseSystemPasswordChar = IsPassword && !_reveal;
            _textBox.Font = _valueFont;

            _placeholder.Font = _hintFont;
            _placeholder.SetBounds(left + 1, top, Math.Max(20, Width - left - right - 2), height);

            UpdatePlaceholder();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LoginTheme.Configure(g);

            var rect = new RectangleF(1f, 1f, Width - 2f, Height - 2f);
            using (GraphicsPath path = LoginTheme.Rounded(rect, 12f))
            {
                using (var fill = new SolidBrush(LoginTheme.FieldFill))
                {
                    g.FillPath(fill, path);
                }

                using (var topLight = new LinearGradientBrush(
                    new RectangleF(rect.Left, rect.Top, rect.Width, 18f),
                    Color.FromArgb(16, 255, 255, 255),
                    Color.FromArgb(0, 255, 255, 255),
                    LinearGradientMode.Vertical))
                {
                    var clip = g.Clip;
                    g.SetClip(path);
                    g.FillRectangle(topLight, rect.Left, rect.Top, rect.Width, 18f);
                    g.Clip = clip;
                }

                Color borderColor = LoginTheme.FieldBorder;
                float borderWidth = 1.15f;
                if (_focused)
                {
                    borderColor = LoginTheme.AccentBright;
                    borderWidth = 1.55f;
                }
                else if (_hovered)
                {
                    borderColor = LoginTheme.FieldBorderHot;
                }

                if (_focused)
                {
                    var glowRect = RectangleF.Inflate(rect, -1.1f, -1.1f);
                    using (GraphicsPath glowPath = LoginTheme.Rounded(glowRect, 11f))
                    using (var glowPen = new Pen(Color.FromArgb(34, 140, 110, 255), 3f))
                    {
                        g.DrawPath(glowPen, glowPath);
                    }
                }

                using (var pen = new Pen(borderColor, borderWidth))
                {
                    g.DrawPath(pen, path);
                }
            }

            Color iconColor = _focused
                ? Color.FromArgb(216, 202, 255)
                : Color.FromArgb(132, 117, 170);

            if (_icon == FieldIcon.Mail)
            {
                LoginArt.Envelope(g, new RectangleF(18f, (Height - 13f) / 2f, 16f, 13f), iconColor);
            }
            else if (_icon == FieldIcon.Amount)
            {
                using (var brush = new SolidBrush(iconColor))
                using (var format = (StringFormat)StringFormat.GenericTypographic.Clone())
                {
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    format.FormatFlags |= StringFormatFlags.NoClip;
                    g.DrawString("$", _amountFont, brush,
                        new RectangleF(14f, 0f, 22f, Height), format);
                }
            }
            else
            {
                LoginArt.Lock(g, new RectangleF(19f, (Height - 16f) / 2f, 14f, 16f), iconColor);
            }

            if (IsPassword)
            {
                bool overEye = ClientRectangle.Contains(PointToClient(Cursor.Position)) &&
                               GetEyeBounds().Contains(PointToClient(Cursor.Position));
                Color eyeColor = overEye
                    ? Color.FromArgb(228, 216, 255)
                    : Color.FromArgb(128, 114, 161);
                LoginArt.Eye(g, new RectangleF(Width - 32f, (Height - 12f) / 2f, 16f, 12f), eyeColor, _reveal);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _valueFont.Dispose();
                _hintFont.Dispose();
                _amountFont.Dispose();
            }

            base.Dispose(disposing);
        }
    }

    // -------------------------------------------------------------------------
    // Remember-me check box
    // -------------------------------------------------------------------------
    internal sealed class GlassCheckBox : Control
    {
        private readonly Font _font = LoginTheme.Body(9.2f, FontStyle.Regular);
        private bool _checked;
        private bool _hovered;

        public GlassCheckBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;
            Size = new Size(150, 22);
            Cursor = Cursors.Hand;
            Text = "Remember me";
            MouseEnter += (s, e) => { _hovered = true; Invalidate(); };
            MouseLeave += (s, e) => { _hovered = false; Invalidate(); };
        }

        public bool Checked
        {
            get { return _checked; }
            set { _checked = value; Invalidate(); }
        }

        protected override void OnClick(EventArgs e)
        {
            _checked = !_checked;
            Invalidate();
            base.OnClick(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LoginTheme.Configure(g);

            var box = new RectangleF(0.5f, (Height - 19f) / 2f, 19f, 19f);
            using (GraphicsPath path = LoginTheme.Rounded(box, 6f))
            {
                if (_checked)
                {
                    using (var fill = new LinearGradientBrush(box,
                        Color.FromArgb(137, 106, 255), Color.FromArgb(93, 60, 229), 60f))
                    {
                        g.FillPath(fill, path);
                    }

                    using (var border = new Pen(Color.FromArgb(95, 255, 255, 255), 1f))
                    {
                        g.DrawPath(border, path);
                    }
                }
                else
                {
                    using (var fill = new SolidBrush(Color.FromArgb(12, 255, 255, 255)))
                    {
                        g.FillPath(fill, path);
                    }

                    using (var border = new Pen(
                        _hovered ? Color.FromArgb(110, 92, 160) : Color.FromArgb(55, 255, 255, 255), 1.1f))
                    {
                        g.DrawPath(border, path);
                    }
                }
            }

            if (_checked)
            {
                LoginArt.Check(g, new RectangleF(4.5f, (Height - 14f) / 2f, 11f, 14f),
                    Color.White, 1.9f);
            }

            using (var brush = new SolidBrush(_hovered
                ? Color.FromArgb(226, 217, 248)
                : Color.FromArgb(190, 180, 216)))
            using (var format = (StringFormat)StringFormat.GenericTypographic.Clone())
            {
                format.LineAlignment = StringAlignment.Center;
                g.DrawString(Text, _font, brush,
                    new RectangleF(29f, -1f, Width - 29f, Height), format);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _font.Dispose();
            }

            base.Dispose(disposing);
        }
    }

    // -------------------------------------------------------------------------
    // Gradient primary action
    // -------------------------------------------------------------------------
    internal sealed class PrimaryButton : Control
    {
        private readonly Font _font = LoginTheme.Body(10.8f, FontStyle.Bold);
        private bool _hovered;
        private bool _pressed;

        public PrimaryButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;
            Size = new Size(366, 52);
            Cursor = Cursors.Hand;
            Text = "Sign in securely";
            MouseEnter += (s, e) => { _hovered = true; Invalidate(); };
            MouseLeave += (s, e) => { _hovered = false; _pressed = false; Invalidate(); };
            MouseDown += (s, e) => { if (e.Button == MouseButtons.Left) { _pressed = true; Invalidate(); } };
            MouseUp += (s, e) => { _pressed = false; Invalidate(); };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LoginTheme.Configure(g);

            // soft coloured shadow
            for (int i = 4; i >= 1; i--)
            {
                var halo = new RectangleF(2f + i, 4f + i, Width - 4f - (i * 2f), Height - 4f - (i * 2f));
                using (GraphicsPath haloPath = LoginTheme.Rounded(halo, 13f))
                using (var pen = new Pen(Color.FromArgb(10, 120, 82, 255), 4f))
                {
                    g.DrawPath(pen, haloPath);
                }
            }

            var button = new RectangleF(0.5f, 0.5f, Width - 1f, Height - 2.2f);
            using (GraphicsPath path = LoginTheme.Rounded(button, 14f))
            {
                using (var fill = new LinearGradientBrush(button,
                    Color.FromArgb(132, 99, 255), Color.FromArgb(92, 57, 222), 28f))
                {
                    g.FillPath(fill, path);
                }

                using (var topShine = new LinearGradientBrush(
                    new RectangleF(button.Left, button.Top, button.Width, button.Height * 0.58f),
                    Color.FromArgb(36, 255, 255, 255),
                    Color.FromArgb(0, 255, 255, 255),
                    LinearGradientMode.Vertical))
                {
                    GraphicsState state = g.Save();
                    g.SetClip(path);
                    g.FillRectangle(topShine, button.Left, button.Top, button.Width, button.Height * 0.58f);
                    g.Restore(state);
                }

                if (_hovered)
                {
                    using (var hoverFill = new SolidBrush(Color.FromArgb(15, 255, 255, 255)))
                    {
                        g.FillPath(hoverFill, path);
                    }
                }

                if (_pressed)
                {
                    using (var pressFill = new SolidBrush(Color.FromArgb(28, 18, 8, 50)))
                    {
                        g.FillPath(pressFill, path);
                    }
                }

                using (var border = new Pen(Color.FromArgb(_hovered ? 80 : 50, 255, 255, 255), 1.15f))
                {
                    g.DrawPath(border, path);
                }
            }

            float offset = _pressed ? 1f : 0f;
            float height = _font.GetHeight(g);
            SizeF textSize = g.MeasureString(Text, _font, PointF.Empty, StringFormat.GenericTypographic);
            const float gap = 11f;
            const float arrowWidth = 15f;
            float total = textSize.Width + gap + arrowWidth;
            float startX = (Width - total) / 2f;
            float textY = ((Height - height) / 2f) + offset - 1.2f;

            using (var brush = new SolidBrush(Color.FromArgb(255, 253, 255)))
            using (var format = (StringFormat)StringFormat.GenericTypographic.Clone())
            {
                format.FormatFlags |= StringFormatFlags.NoClip;
                g.DrawString(Text, _font, brush, startX, textY, format);
            }

            LoginArt.ArrowRight(g,
                new PointF(startX + textSize.Width + gap + 1f, (Height / 2f) + offset),
                Color.FromArgb(255, 253, 255));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _font.Dispose();
            }

            base.Dispose(disposing);
        }
    }

    // -------------------------------------------------------------------------
    // Glass secondary action
    // -------------------------------------------------------------------------
    internal sealed class SecondaryButton : Control
    {
        private readonly Font _font = LoginTheme.Body(10.1f, FontStyle.Regular);
        private bool _hovered;
        private bool _pressed;

        public SecondaryButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;
            Size = new Size(366, 50);
            Cursor = Cursors.Hand;
            Text = "Sign in with a passkey";
            MouseEnter += (s, e) => { _hovered = true; Invalidate(); };
            MouseLeave += (s, e) => { _hovered = false; _pressed = false; Invalidate(); };
            MouseDown += (s, e) => { if (e.Button == MouseButtons.Left) { _pressed = true; Invalidate(); } };
            MouseUp += (s, e) => { _pressed = false; Invalidate(); };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LoginTheme.Configure(g);

            var button = new RectangleF(0.5f, 0.5f, Width - 1f, Height - 1.4f);
            using (GraphicsPath path = LoginTheme.Rounded(button, 12f))
            {
                using (var fill = new LinearGradientBrush(button,
                    Color.FromArgb(_hovered ? 26 : 17, 255, 255, 255),
                    Color.FromArgb(_hovered ? 13 : 8, 255, 255, 255),
                    65f))
                {
                    g.FillPath(fill, path);
                }

                if (_pressed)
                {
                    using (var pressFill = new SolidBrush(Color.FromArgb(30, 8, 5, 18)))
                    {
                        g.FillPath(pressFill, path);
                    }
                }

                using (var border = new Pen(
                    Color.FromArgb(_hovered ? 56 : 34, 255, 255, 255), 1.05f))
                {
                    g.DrawPath(border, path);
                }
            }

            float offset = _pressed ? 1f : 0f;
            float textHeight = _font.GetHeight(g);
            SizeF textSize = g.MeasureString(Text, _font, PointF.Empty, StringFormat.GenericTypographic);
            const float iconWidth = 16f;
            const float gap = 10f;
            float total = iconWidth + gap + textSize.Width;
            float startX = (Width - total) / 2f;
            float cy = (Height / 2f) + offset;

            LoginArt.Key(g, new RectangleF(startX, cy - 8f, iconWidth, 16f),
                Color.FromArgb(196, 179, 238));

            using (var brush = new SolidBrush(Color.FromArgb(235, 228, 250)))
            using (var format = (StringFormat)StringFormat.GenericTypographic.Clone())
            {
                format.FormatFlags |= StringFormatFlags.NoClip;
                g.DrawString(Text, _font, brush,
                    startX + iconWidth + gap, ((Height - textHeight) / 2f) + offset - 1.1f, format);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _font.Dispose();
            }

            base.Dispose(disposing);
        }
    }

    // -------------------------------------------------------------------------
    // Hover link
    // -------------------------------------------------------------------------
    internal sealed class HoverLink : Control
    {
        private readonly Font _font = LoginTheme.Body(9.1f, FontStyle.Regular);
        private bool _hovered;

        public HoverLink()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;
            Size = new Size(140, 22);
            Cursor = Cursors.Hand;
            Text = "Forgot password?";
            TextAlign = ContentAlignment.MiddleRight;
            MouseEnter += (s, e) => { _hovered = true; Invalidate(); };
            MouseLeave += (s, e) => { _hovered = false; Invalidate(); };
        }

        public ContentAlignment TextAlign { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LoginTheme.Configure(g);

            Color color = _hovered
                ? Color.FromArgb(229, 219, 255)
                : Color.FromArgb(181, 160, 255);

            SizeF size = g.MeasureString(Text, _font, PointF.Empty, StringFormat.GenericTypographic);
            float x;
            if (TextAlign == ContentAlignment.MiddleRight || TextAlign == ContentAlignment.TopRight ||
                TextAlign == ContentAlignment.BottomRight)
            {
                x = Width - size.Width;
            }
            else if (TextAlign == ContentAlignment.MiddleCenter || TextAlign == ContentAlignment.TopCenter ||
                     TextAlign == ContentAlignment.BottomCenter)
            {
                x = (Width - size.Width) / 2f;
            }
            else
            {
                x = 0f;
            }

            float y = (Height - size.Height) / 2f - 0.8f;
            using (var brush = new SolidBrush(color))
            using (var format = (StringFormat)StringFormat.GenericTypographic.Clone())
            {
                format.FormatFlags |= StringFormatFlags.NoClip;
                g.DrawString(Text, _font, brush, x, y, format);
            }

            if (_hovered)
            {
                using (var pen = new Pen(Color.FromArgb(125, 205, 186, 255), 1f))
                {
                    g.DrawLine(pen, x, y + size.Height - 2.5f, x + size.Width, y + size.Height - 2.5f);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _font.Dispose();
            }

            base.Dispose(disposing);
        }
    }

    // -------------------------------------------------------------------------
    // "or continue with" divider
    // -------------------------------------------------------------------------
    internal sealed class DividerLabel : Control
    {
        private readonly Font _font = LoginTheme.Body(8.2f, FontStyle.Regular);

        public DividerLabel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;
            Size = new Size(366, 18);
            Text = "or continue with";
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LoginTheme.Configure(g);

            SizeF size = g.MeasureString(Text, _font, PointF.Empty, StringFormat.GenericTypographic);
            float cy = Height / 2f;
            float textX = (Width - size.Width) / 2f;
            float gap = 12f;

            using (var pen = new Pen(Color.FromArgb(27, 255, 255, 255), 1f))
            {
                g.DrawLine(pen, 0f, cy, Math.Max(0f, textX - gap), cy);
                g.DrawLine(pen, textX + size.Width + gap, cy, Width, cy);
            }

            using (var brush = new SolidBrush(Color.FromArgb(140, 127, 170)))
            using (var format = (StringFormat)StringFormat.GenericTypographic.Clone())
            {
                format.FormatFlags |= StringFormatFlags.NoClip;
                g.DrawString(Text, _font, brush, textX, (Height - size.Height) / 2f - 0.4f, format);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _font.Dispose();
            }

            base.Dispose(disposing);
        }
    }

    // -------------------------------------------------------------------------
    // Footer line with an inline action
    // -------------------------------------------------------------------------
    internal sealed class SignupFooter : Control
    {
        private readonly Font _font = LoginTheme.Body(9.1f, FontStyle.Regular);
        private readonly Font _linkFont = LoginTheme.Body(9.1f, FontStyle.Bold);
        private RectangleF _linkBounds;
        private bool _hovered;

        public SignupFooter()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;
            Size = new Size(366, 22);
            MouseMove += OnFooterMouseMove;
            MouseLeave += (s, e) => { _hovered = false; Cursor = Cursors.Default; Invalidate(); };
        }

        private void OnFooterMouseMove(object sender, MouseEventArgs e)
        {
            bool over = _linkBounds.Contains(e.Location);
            if (over != _hovered)
            {
                _hovered = over;
                Invalidate();
            }

            Cursor = over ? Cursors.Hand : Cursors.Default;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LoginTheme.Configure(g);

            const string question = "New to Pay_WU?";
            const string action = "Create account";
            const float gap = 7f;

            SizeF qSize = g.MeasureString(question, _font, PointF.Empty, StringFormat.GenericTypographic);
            SizeF aSize = g.MeasureString(action, _linkFont, PointF.Empty, StringFormat.GenericTypographic);
            float total = qSize.Width + gap + aSize.Width;
            float x = (Width - total) / 2f;
            float y = (Height - qSize.Height) / 2f - 0.6f;

            using (var brush = new SolidBrush(Color.FromArgb(161, 148, 190)))
            using (var format = (StringFormat)StringFormat.GenericTypographic.Clone())
            {
                format.FormatFlags |= StringFormatFlags.NoClip;
                g.DrawString(question, _font, brush, x, y, format);
            }

            float linkX = x + qSize.Width + gap;
            using (var brush = new SolidBrush(_hovered
                ? Color.FromArgb(233, 224, 255)
                : Color.FromArgb(185, 164, 255)))
            using (var format = (StringFormat)StringFormat.GenericTypographic.Clone())
            {
                format.FormatFlags |= StringFormatFlags.NoClip;
                g.DrawString(action, _linkFont, brush, linkX, y, format);
            }

            _linkBounds = new RectangleF(linkX, 0f, aSize.Width, Height);
            if (_hovered)
            {
                using (var pen = new Pen(Color.FromArgb(115, 199, 180, 255), 1f))
                {
                    g.DrawLine(pen, linkX, y + aSize.Height - 2.4f,
                        linkX + aSize.Width, y + aSize.Height - 2.4f);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _font.Dispose();
                _linkFont.Dispose();
            }

            base.Dispose(disposing);
        }
    }

    // -------------------------------------------------------------------------
    // Frameless window buttons
    // -------------------------------------------------------------------------
    internal enum WindowButtonKind
    {
        Minimize,
        Close
    }

    internal sealed class WindowButton : Control
    {
        private bool _hovered;
        private bool _pressed;

        public WindowButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;
            Size = new Size(36, 36);
            Cursor = Cursors.Hand;
            Kind = WindowButtonKind.Close;
            MouseEnter += (s, e) => { _hovered = true; Invalidate(); };
            MouseLeave += (s, e) => { _hovered = false; _pressed = false; Invalidate(); };
            MouseDown += (s, e) => { if (e.Button == MouseButtons.Left) { _pressed = true; Invalidate(); } };
            MouseUp += (s, e) => { _pressed = false; Invalidate(); };
        }

        public WindowButtonKind Kind { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LoginTheme.Configure(g);

            if (_hovered)
            {
                var hover = new RectangleF(2f, 2f, Width - 4f, Height - 4f);
                Color fill = Kind == WindowButtonKind.Close
                    ? Color.FromArgb(_pressed ? 232 : 212, 239, 76, 108)
                    : Color.FromArgb(_pressed ? 30 : 22, 255, 255, 255);
                using (GraphicsPath path = LoginTheme.Rounded(hover, 10f))
                using (var brush = new SolidBrush(fill))
                {
                    g.FillPath(brush, path);
                }
            }

            Color icon = _hovered && Kind == WindowButtonKind.Close
                ? Color.FromArgb(255, 248, 251)
                : Color.FromArgb(184, 172, 211);
            float cx = Width / 2f;
            float cy = Height / 2f;

            using (var pen = new Pen(icon, 1.35f))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                if (Kind == WindowButtonKind.Minimize)
                {
                    g.DrawLine(pen, cx - 5f, cy + 4f, cx + 5f, cy + 4f);
                }
                else
                {
                    g.DrawLine(pen, cx - 4.6f, cy - 4.6f, cx + 4.6f, cy + 4.6f);
                    g.DrawLine(pen, cx + 4.6f, cy - 4.6f, cx - 4.6f, cy + 4.6f);
                }
            }
        }
    }
}
