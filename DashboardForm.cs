using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Diagnostics;     // «·‰ﬁÿ… 5: ≈÷«›… «·„ﬂ »…
using System.ComponentModel;  // ÷—Ê—Ì… ·«” Œœ«„ Win32Exception

namespace Payment_system
{
    /// <summary>
    /// Presentational dashboard for the signed-in Pay_WU account.
    /// The three actions are visual only and intentionally contain no business logic.
    /// </summary>
    public partial class DashboardForm : Form
    {
        private const int WmNclButtonDown = 0xA1;
        private const int HtCaption = 0x2;

        private readonly Rectangle _cardBounds = new Rectangle(72, 332, 1036, 300);
        private Bitmap _backdrop;
        private int _windowRadius = 8;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attribute, ref int value, int size);

        public DashboardForm() : this(null)
        {
        }
        
        public DashboardForm(BankAccount account)
        {
            InitializeComponent();

            //this is program code too
            if(account != null)
            {
                _account = account;
                ApplySignedInAccount(_account.OwnerEmail);
                balanceAmount.Text = _account.Balance.ToString("C2");
            }

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true;
            Font = LoginTheme.Body(9f, FontStyle.Regular);

            closeButton.Click += delegate { Close(); };
            minimizeButton.Click += delegate { WindowState = FormWindowState.Minimized; };

            _windowRadius = Environment.OSVersion.Version.Build >= 22000 ? 8 : 18;
        }

        public string AvailableBalanceText
        {
            get { return balanceAmount.Text; }
            set { balanceAmount.Text = value ?? string.Empty; }
        }

        private void ApplySignedInAccount(string signedInEmail)
        {
            if (string.IsNullOrWhiteSpace(signedInEmail))
            {
                userChip.UserName = string.Empty;
                userChip.UserEmail = string.Empty;
                userChip.Initials = string.Empty;
                return;
            }

            string accountEmail = signedInEmail.Trim();
            int atIndex = accountEmail.IndexOf('@');
            string accountName = atIndex > 0
                ? accountEmail.Substring(0, atIndex)
                : accountEmail;

            if (string.IsNullOrWhiteSpace(accountName))
            {
                accountName = accountEmail;
            }

            userChip.UserName = accountName;
            userChip.UserEmail = accountEmail;
            userChip.Initials = CreateInitials(accountName);
        }

        private static string CreateInitials(string accountName)
        {
            string initials = string.Empty;

            foreach (char character in accountName)
            {
                if (!char.IsLetterOrDigit(character))
                {
                    continue;
                }

                initials += char.ToUpperInvariant(character);
                if (initials.Length == 2)
                {
                    break;
                }
            }

            return initials;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= 0x00020000; // CS_DROPSHADOW
                return cp;
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ApplyWindowShape();
        }

        private void ApplyWindowShape()
        {
            bool dwmApplied = false;

            if (Environment.OSVersion.Version.Build >= 22000)
            {
                try
                {
                    int preference = 2; // DWMWCP_ROUND
                    int result = DwmSetWindowAttribute(Handle, 33, ref preference, sizeof(int));
                    dwmApplied = result == 0;
                }
                catch
                {
                    dwmApplied = false;
                }
            }

            if (dwmApplied)
            {
                _windowRadius = 8;
                Region = null;
            }
            else
            {
                _windowRadius = 18;
                using (GraphicsPath path = LoginTheme.Rounded(
                    new RectangleF(0f, 0f, Width, Height), _windowRadius))
                {
                    Region = new Region(path);
                }
            }

            if (_backdrop != null)
            {
                _backdrop.Dispose();
                _backdrop = null;
            }

            Invalidate(true);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (_backdrop == null || _backdrop.Width != ClientSize.Width || _backdrop.Height != ClientSize.Height)
            {
                BuildBackdrop();
            }

            if (_backdrop != null)
            {
                e.Graphics.DrawImageUnscaled(_backdrop, 0, 0);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WmNclButtonDown, (IntPtr)HtCaption, IntPtr.Zero);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_backdrop != null)
            {
                _backdrop.Dispose();
                _backdrop = null;
            }

            base.OnFormClosed(e);
        }

        private void BuildBackdrop()
        {
            int w = Math.Max(1, ClientSize.Width);
            int h = Math.Max(1, ClientSize.Height);
            const int supersample = 2;
            Bitmap bitmap;

            using (var scene = new Bitmap(w * supersample, h * supersample, PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(scene))
                {
                    g.CompositingMode = CompositingMode.SourceOver;
                    g.CompositingQuality = CompositingQuality.GammaCorrected;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.ScaleTransform(supersample, supersample);

                    using (var baseWash = new LinearGradientBrush(
                        new Rectangle(0, 0, w, h),
                        LoginTheme.BackTop,
                        LoginTheme.BackBottom,
                        90f))
                    {
                        g.FillRectangle(baseWash, 0, 0, w, h);
                    }

                    // Same signature aurora as the sign-in screen.
                    LoginTheme.RadialGlow(g, new PointF(w * 0.82f, h * 0.03f), 940f, 640f,
                        Color.FromArgb(62, 105, 68, 255));
                    LoginTheme.RadialGlow(g, new PointF(w * 0.25f, h * 0.14f), 720f, 520f,
                        Color.FromArgb(30, 120, 84, 255));
                    LoginTheme.RadialGlow(g, new PointF(w * 0.02f, h * 0.92f), 860f, 640f,
                        Color.FromArgb(46, 50, 31, 165));
                    LoginTheme.RadialGlow(g, new PointF(w * 0.59f, h * 0.54f), 620f, 620f,
                        Color.FromArgb(18, 74, 48, 245));
                    LoginTheme.RadialGlow(g, new PointF(w * 1.02f, h * 0.64f), 620f, 560f,
                        Color.FromArgb(26, 226, 113, 255));
                    LoginTheme.RadialGlow(g, new PointF(w * 0.45f, h * 1.02f), 700f, 420f,
                        Color.FromArgb(22, 120, 84, 255));

                    DrawOrbit(g, new PointF(128f, 748f), 310f, Color.FromArgb(13, 163, 142, 255));
                    DrawOrbit(g, new PointF(128f, 748f), 412f, Color.FromArgb(8, 163, 142, 255));
                    DrawOrbit(g, new PointF(520f, 58f), 250f, Color.FromArgb(7, 183, 160, 255));

                    DrawStarField(g, w, h);

                    // Header rule.
                    var rule = new RectangleF(72f, 124f, 1044f, 1f);
                    using (var ruleBrush = new LinearGradientBrush(rule,
                        Color.FromArgb(0, 255, 255, 255),
                        Color.FromArgb(0, 255, 255, 255),
                        LinearGradientMode.Horizontal))
                    {
                        var blend = new ColorBlend(3);
                        blend.Colors = new[]
                        {
                            Color.FromArgb(0, 255, 255, 255),
                            Color.FromArgb(34, 255, 255, 255),
                            Color.FromArgb(0, 255, 255, 255)
                        };
                        blend.Positions = new[] { 0f, 0.35f, 1f };
                        ruleBrush.InterpolationColors = blend;
                        using (var pen = new Pen(ruleBrush, 1f))
                        {
                            g.DrawLine(pen, rule.Left, rule.Top, rule.Right, rule.Top);
                        }
                    }

                    DrawVerticalFadeLine(g, 932f, 204f, 262f);

                    DrawVignette(g, w, h);
                    DrawUserChipShadow(g);
                    DrawMainCard(g);
                    DrawWindowOutline(g, w, h);
                }

                bitmap = new Bitmap(w, h, PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CompositingMode = CompositingMode.SourceCopy;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.DrawImage(scene,
                        new Rectangle(0, 0, w, h),
                        0, 0, scene.Width, scene.Height,
                        GraphicsUnit.Pixel);
                }
            }

            if (_backdrop != null)
            {
                _backdrop.Dispose();
            }

            _backdrop = bitmap;
        }

        private void DrawVignette(Graphics g, int w, int h)
        {
            using (var top = new LinearGradientBrush(
                new Rectangle(0, 0, w, 150),
                Color.FromArgb(86, 4, 2, 10),
                Color.FromArgb(0, 4, 2, 10),
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(top, 0, 0, w, 150);
            }

            using (var bottom = new LinearGradientBrush(
                new Rectangle(0, h - 190, w, 190),
                Color.FromArgb(0, 2, 1, 6),
                Color.FromArgb(92, 2, 1, 6),
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(bottom, 0, h - 190, w, 190);
            }

            using (var left = new LinearGradientBrush(
                new Rectangle(0, 0, 120, h),
                Color.FromArgb(56, 3, 2, 8),
                Color.FromArgb(0, 3, 2, 8),
                LinearGradientMode.Horizontal))
            {
                g.FillRectangle(left, 0, 0, 120, h);
            }

            using (var right = new LinearGradientBrush(
                new Rectangle(w - 110, 0, 110, h),
                Color.FromArgb(0, 3, 2, 8),
                Color.FromArgb(58, 3, 2, 8),
                LinearGradientMode.Horizontal))
            {
                g.FillRectangle(right, w - 110, 0, 110, h);
            }
        }

        private static void DrawOrbit(Graphics g, PointF center, float radius, Color color)
        {
            using (var pen = new Pen(color, 1f))
            {
                g.DrawEllipse(pen,
                    center.X - radius,
                    center.Y - radius,
                    radius * 2f,
                    radius * 2f);
            }
        }

        private void DrawStarField(Graphics g, int width, int height)
        {
            var random = new Random(613);
            for (int i = 0; i < 54; i++)
            {
                float x = (float)(random.NextDouble() * width);
                float y = (float)(random.NextDouble() * height);
                if (x > _cardBounds.Left - 30 && x < _cardBounds.Right + 30 &&
                    y > _cardBounds.Top - 30 && y < _cardBounds.Bottom + 30)
                {
                    continue;
                }

                float diameter = 0.7f + (float)(random.NextDouble() * 1.0f);
                int alpha = 9 + random.Next(0, 38);
                using (var brush = new SolidBrush(Color.FromArgb(alpha, 224, 214, 255)))
                {
                    g.FillEllipse(brush, x, y, diameter, diameter);
                }
            }

            DrawSparkle(g, 404f, 114f, 4.5f, Color.FromArgb(60, 214, 197, 255));
            DrawSparkle(g, 566f, 700f, 3.4f, Color.FromArgb(42, 214, 197, 255));
            DrawSparkle(g, 1060f, 58f, 3.0f, Color.FromArgb(48, 255, 222, 255));
        }

        private static void DrawSparkle(Graphics g, float x, float y, float size, Color color)
        {
            using (var pen = new Pen(color, 1f))
            {
                g.DrawLine(pen, x - size, y, x + size, y);
                g.DrawLine(pen, x, y - size, x, y + size);
            }

            using (var brush = new SolidBrush(Color.FromArgb(Math.Min(255, color.A + 30),
                color.R, color.G, color.B)))
            {
                g.FillEllipse(brush, x - 1.1f, y - 1.1f, 2.2f, 2.2f);
            }
        }

        private static void DrawVerticalFadeLine(Graphics g, float x, float top, float bottom)
        {
            var line = new RectangleF(x, top, 1f, bottom - top);
            using (var brush = new LinearGradientBrush(
                line,
                Color.FromArgb(0, 255, 255, 255),
                Color.FromArgb(0, 255, 255, 255),
                LinearGradientMode.Vertical))
            {
                var blend = new ColorBlend(3);
                blend.Colors = new[]
                {
                    Color.FromArgb(0, 255, 255, 255),
                    Color.FromArgb(30, 255, 255, 255),
                    Color.FromArgb(0, 255, 255, 255)
                };
                blend.Positions = new[] { 0f, 0.5f, 1f };
                brush.InterpolationColors = blend;
                using (var pen = new Pen(brush, 1f))
                {
                    g.DrawLine(pen, x, top, x, bottom);
                }
            }
        }

        private void DrawUserChipShadow(Graphics g)
        {
            var chip = new RectangleF(866f, 54f, 250f, 52f);
            for (int i = 3; i >= 1; i--)
            {
                var shadow = RectangleF.Inflate(chip, i * 2f, i * 2f);
                shadow.Y += 4f + i;
                using (GraphicsPath path = LoginTheme.Rounded(shadow, 16f + i))
                using (var brush = new SolidBrush(Color.FromArgb(5, 0, 0, 0)))
                {
                    g.FillPath(brush, path);
                }
            }
        }

        private void DrawMainCard(Graphics g)
        {
            RectangleF card = _cardBounds;

            LoginTheme.RadialGlow(g, new PointF(590f, 492f), 1320f, 720f,
                Color.FromArgb(34, 111, 77, 255));
            LoginTheme.RadialGlow(g, new PointF(1050f, 610f), 500f, 380f,
                Color.FromArgb(14, 236, 150, 255));
            LoginTheme.RadialGlow(g, new PointF(220f, 380f), 430f, 310f,
                Color.FromArgb(10, 220, 206, 255));

            for (int i = 9; i >= 1; i--)
            {
                float grow = 3f + (i * 2.4f);
                var shadowBounds = RectangleF.Inflate(card, grow, grow);
                shadowBounds.Y += 9f + i;
                using (GraphicsPath shadowPath = LoginTheme.Rounded(shadowBounds, 26f + grow))
                using (var shadow = new SolidBrush(Color.FromArgb(7, 0, 0, 0)))
                {
                    g.FillPath(shadow, shadowPath);
                }
            }

            using (GraphicsPath cardPath = LoginTheme.Rounded(card, 26f))
            {
                using (var glass = new LinearGradientBrush(
                    new PointF(card.Left, card.Top),
                    new PointF(card.Right, card.Bottom),
                    Color.FromArgb(236, 36, 24, 70),
                    Color.FromArgb(216, 15, 11, 30)))
                {
                    g.FillPath(glass, cardPath);
                }

                GraphicsState state = g.Save();
                g.SetClip(cardPath);

                using (var reflection = new LinearGradientBrush(
                    card,
                    Color.FromArgb(9, 255, 255, 255),
                    Color.FromArgb(0, 255, 255, 255),
                    66f))
                {
                    g.FillRectangle(reflection, card);
                }

                LoginTheme.RadialGlow(g, new PointF(card.Right - 120f, card.Top + 40f),
                    560f, 360f, Color.FromArgb(13, 142, 104, 255));
                LoginTheme.RadialGlow(g, new PointF(card.Left + 90f, card.Bottom - 10f),
                    520f, 260f, Color.FromArgb(11, 84, 53, 255));

                using (var bottomShade = new LinearGradientBrush(
                    new RectangleF(card.Left, card.Bottom - 130f, card.Width, 130f),
                    Color.FromArgb(0, 3, 1, 8),
                    Color.FromArgb(44, 3, 1, 8),
                    LinearGradientMode.Vertical))
                {
                    g.FillRectangle(bottomShade, card.Left, card.Bottom - 130f, card.Width, 130f);
                }

                g.Restore(state);

                using (var edge = new LinearGradientBrush(
                    new PointF(card.Left, card.Top),
                    new PointF(card.Right, card.Bottom),
                    Color.FromArgb(122, 255, 255, 255),
                    Color.FromArgb(76, 150, 122, 255)))
                {
                    var blend = new ColorBlend(4);
                    blend.Colors = new[]
                    {
                        Color.FromArgb(132, 255, 255, 255),
                        Color.FromArgb(50, 255, 255, 255),
                        Color.FromArgb(35, 255, 255, 255),
                        Color.FromArgb(92, 150, 122, 255)
                    };
                    blend.Positions = new[] { 0f, 0.38f, 0.68f, 1f };
                    edge.InterpolationColors = blend;
                    using (var pen = new Pen(edge, 1.3f))
                    {
                        g.DrawPath(pen, cardPath);
                    }
                }

                using (GraphicsPath innerPath = LoginTheme.Rounded(
                    RectangleF.Inflate(card, -1.6f, -1.6f), 24.5f))
                using (var innerPen = new Pen(Color.FromArgb(18, 255, 255, 255), 1f))
                {
                    g.DrawPath(innerPen, innerPath);
                }
            }

            var highlightRect = new RectangleF(card.Left + 150f, card.Top + 1.4f, card.Width - 300f, 1.6f);
            using (var highlight = new LinearGradientBrush(highlightRect,
                Color.FromArgb(0, 255, 255, 255),
                Color.FromArgb(0, 255, 255, 255),
                LinearGradientMode.Horizontal))
            {
                var blend = new ColorBlend(3);
                blend.Colors = new[]
                {
                    Color.FromArgb(0, 255, 255, 255),
                    Color.FromArgb(148, 255, 255, 255),
                    Color.FromArgb(0, 255, 255, 255)
                };
                blend.Positions = new[] { 0f, 0.46f, 1f };
                highlight.InterpolationColors = blend;
                using (var pen = new Pen(highlight, 1.25f))
                {
                    g.DrawLine(pen, highlightRect.Left, highlightRect.Top,
                        highlightRect.Right, highlightRect.Top);
                }
            }
        }

        private void DrawWindowOutline(Graphics g, int width, int height)
        {
            var outline = new RectangleF(0.5f, 0.5f, width - 1f, height - 1f);
            using (GraphicsPath path = LoginTheme.Rounded(outline, _windowRadius))
            {
                using (var border = new LinearGradientBrush(
                    new PointF(0f, 0f),
                    new PointF(width, height),
                    Color.FromArgb(66, 255, 255, 255),
                    Color.FromArgb(52, 118, 86, 255)))
                {
                    using (var pen = new Pen(border, 1.1f))
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }
        }

        //===Program code====================================================================================================
        private BankAccount _account ;
        private readonly AccountStore _storeAccount = new AccountStore();
        private readonly string _transactionsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Transactions.txt");

        //private bool validateInputBalance()
        //{
        //    if(string.IsNullOrWhiteSpace(amountInput.Text))
        //    {
        //        MessageBox.Show("Balance amount cannot be empty.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return false;
        //    }
        //    return true;
        //}


        private void depositButton_Click(object sender, EventArgs e)
        {
            if(decimal.TryParse(amountInput.Text,out decimal finalAmount) && finalAmount > 0)
            {
                if(MessageBox.Show("Are u sure u want to do this transaction ?", "info",MessageBoxButtons.OKCancel,MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _account.Deposit(finalAmount);
                    _storeAccount.SaveAccount(_account);
                    MessageBox.Show("Done Successfuly !", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    balanceAmount.Text = _account.Balance.ToString("C2");
                    amountInput.Text = "";
                }
                else
                {
                    MessageBox.Show("Operation canseled !");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Erorr invalid type input","Erorr",MessageBoxButtons.OKCancel,MessageBoxIcon.Error);
            }
        }

        private void printButton_Click(object sender, EventArgs e)
        {
            File.WriteAllLines(_transactionsFilePath, _account.GetStatementLines());
            if(MessageBox.Show("Done Successfuly !", "info", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
            {
                try{
                    if (File.Exists(_transactionsFilePath))
                    {
                        //≈⁄œ«œ Œ’«∆’ ⁄„·Ì… «· ‘€Ì· 
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            FileName = _transactionsFilePath,
                            UseShellExecute = true
                        };
                        Process.Start(startInfo);
                    }
                    else
                    {
                        MessageBox.Show("File doesn't exsist !", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                catch (FileNotFoundException ex)
                {

                }
                catch(Win32Exception ex)
                {

                }
            }

        }

        private void withdrawButton_Click(object sender, EventArgs e)
        {
            if(decimal.TryParse(amountInput.Text,out decimal finalAmount) && finalAmount > 0 && finalAmount <= _account.Balance)
            {
                if(MessageBox.Show("Are u sure u want to do this transaction ?","info",MessageBoxButtons.OKCancel,MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _account.Withdraw(finalAmount);
                    _storeAccount.SaveAccount(_account);
                    MessageBox.Show("Done Successfuly !", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    balanceAmount.Text = _account.Balance.ToString("C2");
                    amountInput.Text = "";
                }
                else
                {
                    MessageBox.Show("Operation canseled !");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Erorr invalid type input or amount is more than your balance", "Erorr", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }
    }
}
