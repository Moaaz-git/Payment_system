using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Payment_system
{
    // -------------------------------------------------------------------------
    // Compact signed-in account capsule
    // -------------------------------------------------------------------------
    internal sealed class UserChip : Control
    {
        private readonly Font _nameFont = LoginTheme.Body(9.6f, FontStyle.Bold);
        private readonly Font _mailFont = LoginTheme.Body(8.1f, FontStyle.Regular);
        private readonly Font _initialFont = LoginTheme.Display(9.2f, FontStyle.Bold);
        private string _userName = string.Empty;
        private string _userEmail = string.Empty;
        private string _initials = string.Empty;
        private bool _hovered;

        public UserChip()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;
            Size = new Size(250, 52);
            Cursor = Cursors.Hand;
            MouseEnter += delegate { _hovered = true; Invalidate(); };
            MouseLeave += delegate { _hovered = false; Invalidate(); };
        }

        public string UserName
        {
            get { return _userName; }
            set { _userName = value ?? string.Empty; Invalidate(); }
        }

        public string UserEmail
        {
            get { return _userEmail; }
            set { _userEmail = value ?? string.Empty; Invalidate(); }
        }

        public string Initials
        {
            get { return _initials; }
            set { _initials = (value ?? string.Empty).Trim(); Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LoginTheme.Configure(g);

            string displayName = string.IsNullOrWhiteSpace(UserName)
                ? "Not signed in"
                : UserName;
            string accountLine = string.IsNullOrWhiteSpace(UserEmail)
                ? "No active account"
                : UserEmail;
            bool hasAccount = !string.IsNullOrWhiteSpace(UserEmail);

            var bounds = new RectangleF(0.5f, 0.5f, Width - 1f, Height - 1f);
            using (GraphicsPath path = LoginTheme.Rounded(bounds, 16f))
            {
                using (var fill = new LinearGradientBrush(bounds,
                    Color.FromArgb(_hovered ? 22 : 14, 255, 255, 255),
                    Color.FromArgb(_hovered ? 11 : 7, 255, 255, 255),
                    70f))
                {
                    g.FillPath(fill, path);
                }

                using (var border = new Pen(
                    Color.FromArgb(_hovered ? 52 : 32, 255, 255, 255), 1f))
                {
                    g.DrawPath(border, path);
                }
            }

            var avatar = new RectangleF(7f, 7f, 38f, 38f);
            using (var avatarFill = new LinearGradientBrush(avatar,
                Color.FromArgb(146, 116, 255), Color.FromArgb(85, 54, 216), 55f))
            {
                g.FillEllipse(avatarFill, avatar);
            }

            using (var avatarBorder = new Pen(Color.FromArgb(82, 255, 255, 255), 1.1f))
            {
                g.DrawEllipse(avatarBorder, avatar);
            }

            if (string.IsNullOrWhiteSpace(Initials))
            {
                using (var pen = new Pen(Color.FromArgb(224, 216, 255), 1.4f))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    g.DrawEllipse(pen, avatar.Left + 13f, avatar.Top + 9f, 12f, 12f);
                    g.DrawArc(pen, avatar.Left + 8f, avatar.Top + 23f, 22f, 17f, 198f, 144f);
                }
            }
            else
            {
                using (var brush = new SolidBrush(Color.FromArgb(255, 252, 255)))
                using (var format = (StringFormat)StringFormat.GenericTypographic.Clone())
                {
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    g.DrawString(Initials, _initialFont, brush, avatar, format);
                }
            }

            if (hasAccount)
            {
                using (var ring = new SolidBrush(LoginTheme.BackTop))
                {
                    g.FillEllipse(ring, 35.5f, 35.5f, 12.5f, 12.5f);
                }

                using (var online = new SolidBrush(LoginTheme.Mint))
                {
                    g.FillEllipse(online, 38f, 38f, 7.5f, 7.5f);
                }
            }

            float textWidth = Width - 56f - 30f;

            using (var brush = new SolidBrush(Color.FromArgb(242, 237, 253)))
            using (var format = new StringFormat(StringFormat.GenericTypographic))
            {
                format.Trimming = StringTrimming.EllipsisCharacter;
                format.FormatFlags |= StringFormatFlags.NoWrap;
                format.LineAlignment = StringAlignment.Center;
                g.DrawString(displayName, _nameFont, brush,
                    new RectangleF(56f, 8f, textWidth, 17f), format);
            }

            using (var brush = new SolidBrush(Color.FromArgb(142, 129, 173)))
            using (var format = new StringFormat(StringFormat.GenericTypographic))
            {
                format.Trimming = StringTrimming.EllipsisCharacter;
                format.FormatFlags |= StringFormatFlags.NoWrap;
                format.LineAlignment = StringAlignment.Center;
                g.DrawString(accountLine, _mailFont, brush,
                    new RectangleF(56f, 26f, textWidth, 16f), format);
            }

            using (var pen = new Pen(Color.FromArgb(_hovered ? 218 : 164, 193, 179, 222), 1.35f))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                float cx = Width - 19f;
                float cy = Height / 2f - 1f;
                g.DrawLines(pen, new[]
                {
                    new PointF(cx - 3.6f, cy - 2f),
                    new PointF(cx, cy + 1.8f),
                    new PointF(cx + 3.6f, cy - 2f)
                });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _nameFont.Dispose();
                _mailFont.Dispose();
                _initialFont.Dispose();
            }

            base.Dispose(disposing);
        }
    }

    // -------------------------------------------------------------------------
    // Primary action tile used by the dashboard console
    // -------------------------------------------------------------------------
    internal enum PayActionStyle
    {
        Primary,
        Glass,
        Quiet
    }

    internal enum PayActionIcon
    {
        Deposit,
        Withdraw,
        Print
    }

    internal sealed class PaymentActionButton : Control
    {
        private readonly Font _titleFont = LoginTheme.Body(10.2f, FontStyle.Bold);
        private readonly Font _subtitleFont = LoginTheme.Body(8.1f, FontStyle.Regular);
        private readonly Font _pdfFont = LoginTheme.Body(6.7f, FontStyle.Bold);
        private string _subtitle = string.Empty;
        private bool _hovered;
        private bool _pressed;

        public PaymentActionButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;
            Size = new Size(176, 130);
            TabStop = false;
            Cursor = Cursors.Hand;
            Style = PayActionStyle.Glass;
            ActionIcon = PayActionIcon.Deposit;
            Text = "Action";
            MouseEnter += delegate { _hovered = true; Invalidate(); };
            MouseLeave += delegate { _hovered = false; _pressed = false; Invalidate(); };
            MouseDown += delegate { _pressed = true; Invalidate(); };
            MouseUp += delegate { _pressed = false; Invalidate(); };
        }

        public PayActionStyle Style { get; set; }

        public PayActionIcon ActionIcon { get; set; }

        public string Subtitle
        {
            get { return _subtitle; }
            set { _subtitle = value ?? string.Empty; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LoginTheme.Configure(g);

            if (Style == PayActionStyle.Primary)
            {
                for (int i = 4; i >= 1; i--)
                {
                    var halo = new RectangleF(3f + i, 5f + i, Width - 6f - (i * 2f), Height - 7f - (i * 2f));
                    using (GraphicsPath haloPath = LoginTheme.Rounded(halo, 18f))
                    using (var pen = new Pen(Color.FromArgb(9, 120, 82, 255), 5f))
                    {
                        g.DrawPath(pen, haloPath);
                    }
                }
            }

            var tile = new RectangleF(0.5f, 0.5f, Width - 1f, Height - 2f);
            using (GraphicsPath path = LoginTheme.Rounded(tile, 18f))
            {
                if (Style == PayActionStyle.Primary)
                {
                    using (var fill = new LinearGradientBrush(tile,
                        Color.FromArgb(133, 100, 255), Color.FromArgb(92, 57, 222), 32f))
                    {
                        g.FillPath(fill, path);
                    }

                    using (var shine = new LinearGradientBrush(
                        new RectangleF(tile.Left, tile.Top, tile.Width, tile.Height * 0.55f),
                        Color.FromArgb(30, 255, 255, 255),
                        Color.FromArgb(0, 255, 255, 255),
                        LinearGradientMode.Vertical))
                    {
                        GraphicsState state = g.Save();
                        g.SetClip(path);
                        g.FillRectangle(shine, tile.Left, tile.Top, tile.Width, tile.Height * 0.55f);
                        g.Restore(state);
                    }
                }
                else
                {
                    int fillAlpha = Style == PayActionStyle.Quiet ? 7 : 13;
                    if (_hovered)
                    {
                        fillAlpha += 7;
                    }

                    using (var fill = new LinearGradientBrush(tile,
                        Color.FromArgb(fillAlpha, 255, 255, 255),
                        Color.FromArgb(Math.Max(3, fillAlpha - 6), 255, 255, 255),
                        70f))
                    {
                        g.FillPath(fill, path);
                    }
                }

                if (_hovered)
                {
                    using (var overlay = new SolidBrush(Color.FromArgb(10, 255, 255, 255)))
                    {
                        g.FillPath(overlay, path);
                    }
                }

                if (_pressed)
                {
                    using (var overlay = new SolidBrush(Color.FromArgb(24, 8, 5, 20)))
                    {
                        g.FillPath(overlay, path);
                    }
                }

                Color borderColor = Style == PayActionStyle.Primary
                    ? Color.FromArgb(_hovered ? 84 : 52, 255, 255, 255)
                    : Color.FromArgb(_hovered ? 56 : 32, 255, 255, 255);
                using (var border = new Pen(borderColor, 1.05f))
                {
                    g.DrawPath(border, path);
                }
            }

            float offset = _pressed ? 1f : 0f;
            var iconCircle = new RectangleF(18f, 18f + offset, 38f, 38f);
            using (var iconFill = new SolidBrush(Style == PayActionStyle.Primary
                ? Color.FromArgb(30, 255, 255, 255)
                : Color.FromArgb(20, 180, 157, 255)))
            {
                g.FillEllipse(iconFill, iconCircle);
            }

            using (var iconBorder = new Pen(Style == PayActionStyle.Primary
                ? Color.FromArgb(58, 255, 255, 255)
                : Color.FromArgb(40, 211, 195, 255), 1f))
            {
                g.DrawEllipse(iconBorder, iconCircle);
            }

            Color iconColor = Style == PayActionStyle.Primary
                ? Color.FromArgb(255, 252, 255)
                : Color.FromArgb(205, 187, 255);

            switch (ActionIcon)
            {
                case PayActionIcon.Deposit:
                    DrawDeposit(g, iconCircle, iconColor);
                    break;
                case PayActionIcon.Withdraw:
                    DrawWithdraw(g, iconCircle, iconColor);
                    break;
                default:
                    DrawPrinter(g, iconCircle, iconColor);
                    break;
            }

            Color titleColor;
            Color subtitleColor;
            if (Style == PayActionStyle.Primary)
            {
                titleColor = Color.FromArgb(255, 253, 255);
                subtitleColor = Color.FromArgb(210, 199, 247);
            }
            else if (Style == PayActionStyle.Quiet)
            {
                titleColor = Color.FromArgb(218, 207, 241);
                subtitleColor = Color.FromArgb(136, 124, 160);
            }
            else
            {
                titleColor = Color.FromArgb(237, 230, 251);
                subtitleColor = Color.FromArgb(154, 141, 180);
            }

            using (var brush = new SolidBrush(titleColor))
            using (var format = new StringFormat(StringFormat.GenericTypographic))
            {
                format.Trimming = StringTrimming.EllipsisCharacter;
                format.FormatFlags |= StringFormatFlags.NoWrap;
                format.LineAlignment = StringAlignment.Center;
                g.DrawString(Text, _titleFont, brush,
                    new RectangleF(18f, 70f + offset, Width - 36f, 22f), format);
            }

            using (var brush = new SolidBrush(subtitleColor))
            using (var format = new StringFormat(StringFormat.GenericTypographic))
            {
                format.Trimming = StringTrimming.EllipsisCharacter;
                format.FormatFlags |= StringFormatFlags.NoWrap;
                format.LineAlignment = StringAlignment.Center;
                g.DrawString(Subtitle, _subtitleFont, brush,
                    new RectangleF(18f, 93f + offset, Width - 36f, 18f), format);
            }

            if (ActionIcon == PayActionIcon.Print)
            {
                DrawPdfPill(g, new RectangleF(Width - 56f, 27f + offset, 37f, 20f));
            }
            else
            {
                using (var pen = new Pen(Style == PayActionStyle.Primary
                    ? Color.FromArgb(200, 255, 253, 255)
                    : Color.FromArgb(150, 197, 179, 222), 1.3f))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    float cx = Width - 26f;
                    float cy = 37f + offset;
                    g.DrawLines(pen, new[]
                    {
                        new PointF(cx - 2.2f, cy - 3.6f),
                        new PointF(cx + 1.8f, cy),
                        new PointF(cx - 2.2f, cy + 3.6f)
                    });
                }
            }
        }

        private static void DrawDeposit(Graphics g, RectangleF circle, Color color)
        {
            using (var pen = new Pen(color, 1.45f))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                float cx = circle.Left + (circle.Width / 2f);
                float cy = circle.Top + (circle.Height / 2f);
                g.DrawLine(pen, cx, cy - 7f, cx, cy + 3.5f);
                g.DrawLines(pen, new[]
                {
                    new PointF(cx - 3.8f, cy - 0.2f),
                    new PointF(cx, cy + 3.6f),
                    new PointF(cx + 3.8f, cy - 0.2f)
                });
                g.DrawLine(pen, cx - 7f, cy + 7f, cx + 7f, cy + 7f);
            }
        }

        private static void DrawWithdraw(Graphics g, RectangleF circle, Color color)
        {
            using (var pen = new Pen(color, 1.45f))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                float cx = circle.Left + (circle.Width / 2f);
                float cy = circle.Top + (circle.Height / 2f);
                g.DrawLine(pen, cx, cy + 7f, cx, cy - 3.5f);
                g.DrawLines(pen, new[]
                {
                    new PointF(cx - 3.8f, cy + 0.2f),
                    new PointF(cx, cy - 3.6f),
                    new PointF(cx + 3.8f, cy + 0.2f)
                });
                g.DrawLine(pen, cx - 7f, cy + 7f, cx + 7f, cy + 7f);
            }
        }

        private static void DrawPrinter(Graphics g, RectangleF circle, Color color)
        {
            using (var pen = new Pen(color, 1.35f))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                pen.LineJoin = LineJoin.Round;
                float cx = circle.Left + (circle.Width / 2f);
                float cy = circle.Top + (circle.Height / 2f);

                using (GraphicsPath paper = LoginTheme.Rounded(
                    new RectangleF(cx - 4.5f, cy - 9f, 9f, 7f), 1.3f))
                {
                    g.DrawPath(pen, paper);
                }

                using (GraphicsPath body = LoginTheme.Rounded(
                    new RectangleF(cx - 8f, cy - 3.8f, 16f, 8.6f), 2f))
                {
                    g.DrawPath(pen, body);
                }

                using (GraphicsPath output = LoginTheme.Rounded(
                    new RectangleF(cx - 5f, cy + 2.8f, 10f, 5.2f), 1.1f))
                {
                    g.DrawPath(pen, output);
                }

                g.DrawLine(pen, cx + 4f, cy - 1f, cx + 5.6f, cy - 1f);
            }
        }

        private void DrawPdfPill(Graphics g, RectangleF bounds)
        {
            using (GraphicsPath path = LoginTheme.Rounded(bounds, 9f))
            {
                using (var fill = new SolidBrush(Color.FromArgb(12, 255, 255, 255)))
                {
                    g.FillPath(fill, path);
                }

                using (var border = new Pen(Color.FromArgb(26, 255, 255, 255), 1f))
                {
                    g.DrawPath(border, path);
                }
            }

            using (var brush = new SolidBrush(Color.FromArgb(166, 151, 196)))
            using (var format = (StringFormat)StringFormat.GenericTypographic.Clone())
            {
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                g.DrawString("PDF", _pdfFont, brush, bounds, format);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _titleFont.Dispose();
                _subtitleFont.Dispose();
                _pdfFont.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
