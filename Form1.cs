using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Payment_system
{
    public partial class Form1 : Form
    {
        private const int WmNclButtonDown = 0xA1;
        private const int HtCaption = 0x2;

        private readonly Rectangle _cardBounds = new Rectangle(686, 60, 430, 640);
        private Bitmap _backdrop;
        private int _windowRadius = 8;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attribute, ref int value, int size);

        public Form1()
        {
            InitializeComponent();

            //link with email_valdating
            emailInput.InnerTextBox.Validating += emailInput_Validating;

            //link with password_validating
            passwordInput.InnerTextBox.Validating += passwordInput_Validating;

            //we prevent user to exit from the input field if the validation fails
            AutoValidate = AutoValidate.EnablePreventFocusChange;

            //we allow user to click on close and minimize buttons without triggering validation
            closeButton.CausesValidation = false;
            minimizeButton.CausesValidation = false;

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

        // ---------------------------------------------------------------------
        // The entire scene is pre-rendered once for perfectly smooth gradients.
        // ---------------------------------------------------------------------
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

                    // Base wash.
                    using (var baseWash = new LinearGradientBrush(
                        new Rectangle(0, 0, w, h),
                        LoginTheme.BackTop,
                        LoginTheme.BackBottom,
                        90f))
                    {
                        g.FillRectangle(baseWash, 0, 0, w, h);
                    }

                    // Soft ambient aurora.
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

                    // Vertical rhythm line between the story and the card.
                    using (var upper = new LinearGradientBrush(
                        new RectangleF(631f, 108f, 1.5f, 252f),
                        Color.FromArgb(0, 183, 158, 255),
                        Color.FromArgb(40, 183, 158, 255),
                        LinearGradientMode.Vertical))
                    using (var upperPen = new Pen(upper, 1f))
                    {
                        g.DrawLine(upperPen, 632f, 108f, 632f, 360f);
                    }

                    using (var lower = new LinearGradientBrush(
                        new RectangleF(631f, 360f, 1.5f, 292f),
                        Color.FromArgb(40, 183, 158, 255),
                        Color.FromArgb(0, 183, 158, 255),
                        LinearGradientMode.Vertical))
                    using (var lowerPen = new Pen(lower, 1f))
                    {
                        g.DrawLine(lowerPen, 632f, 360f, 632f, 652f);
                    }

                    // Soft vignette.
                    using (var topVignette = new LinearGradientBrush(
                        new Rectangle(0, 0, w, 150),
                        Color.FromArgb(86, 4, 2, 10),
                        Color.FromArgb(0, 4, 2, 10),
                        LinearGradientMode.Vertical))
                    {
                        g.FillRectangle(topVignette, 0, 0, w, 150);
                    }

                    using (var bottomVignette = new LinearGradientBrush(
                        new Rectangle(0, h - 190, w, 190),
                        Color.FromArgb(0, 2, 1, 6),
                        Color.FromArgb(92, 2, 1, 6),
                        LinearGradientMode.Vertical))
                    {
                        g.FillRectangle(bottomVignette, 0, h - 190, w, 190);
                    }

                    using (var leftVignette = new LinearGradientBrush(
                        new Rectangle(0, 0, 120, h),
                        Color.FromArgb(56, 3, 2, 8),
                        Color.FromArgb(0, 3, 2, 8),
                        LinearGradientMode.Horizontal))
                    {
                        g.FillRectangle(leftVignette, 0, 0, 120, h);
                    }

                    using (var rightVignette = new LinearGradientBrush(
                        new Rectangle(w - 110, 0, 110, h),
                        Color.FromArgb(0, 3, 2, 8),
                        Color.FromArgb(58, 3, 2, 8),
                        LinearGradientMode.Horizontal))
                    {
                        g.FillRectangle(rightVignette, w - 110, 0, 110, h);
                    }

                    DrawCard(g);
                    DrawWindowOutline(g, w, h);
                }

                // Downsample once so the gradients and glows stay smooth and free of dithering noise.
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
            DrawSparkle(g, 566f, 668f, 3.4f, Color.FromArgb(42, 214, 197, 255));
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

        private void DrawCard(Graphics g)
        {
            RectangleF card = _cardBounds;

            // Coloured light around the glass.
            LoginTheme.RadialGlow(g, new PointF(902f, 296f), 760f, 780f,
                Color.FromArgb(44, 111, 77, 255));
            LoginTheme.RadialGlow(g, new PointF(1046f, 654f), 430f, 380f,
                Color.FromArgb(16, 236, 150, 255));
            LoginTheme.RadialGlow(g, new PointF(754f, 134f), 310f, 250f,
                Color.FromArgb(11, 220, 206, 255));

            // Grounding shadow under the card.
            LoginTheme.RadialGlow(g, new PointF(900f, 722f), 500f, 190f,
                Color.FromArgb(112, 0, 0, 0));

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
                    new PointF(card.Left - 120f, card.Top - 80f),
                    new PointF(card.Left + 380f, card.Top + 430f),
                    Color.FromArgb(8, 255, 255, 255),
                    Color.FromArgb(0, 255, 255, 255)))
                {
                    g.FillRectangle(reflection, card);
                }

                LoginTheme.RadialGlow(g, new PointF(card.Right - 60f, card.Top + 50f),
                    430f, 340f, Color.FromArgb(14, 142, 104, 255));
                LoginTheme.RadialGlow(g, new PointF(card.Left + 40f, card.Bottom - 20f),
                    420f, 260f, Color.FromArgb(12, 84, 53, 255));

                using (var bottomShade = new LinearGradientBrush(
                    new RectangleF(card.Left, card.Bottom - 170f, card.Width, 170f),
                    Color.FromArgb(0, 3, 1, 8),
                    Color.FromArgb(48, 3, 1, 8),
                    LinearGradientMode.Vertical))
                {
                    g.FillRectangle(bottomShade, card.Left, card.Bottom - 170f, card.Width, 170f);
                }

                g.Restore(state);

                // Glass edge.
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

            // A polished highlight across the top edge.
            var highlightRect = new RectangleF(card.Left + 46f, card.Top + 1.4f, card.Width - 140f, 1.6f);
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


        //Program code===================================================================================


        private readonly UserStore _userStore = new UserStore();
        private readonly AccountStore _accountStroe = new AccountStore();

        private string email = "";
        private string password = "";

        private bool Authenticate()
        {
            UserAccount _userAccount =_userStore.FindByEmail(email); 
            if(_userAccount != null && _userAccount.Password == password)
            {
                return true;
            }

            return false;
        }

        private void emailInput_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(emailInput.InnerTextBox.Text))
            {
                errorProvider1.SetError(emailInput.InnerTextBox, "Email is required.");
                e.Cancel = true; // Prevent focus change if validation fails
            }
            else
            {
                errorProvider1.SetError(emailInput.InnerTextBox, string.Empty);
            }
        }

        private void passwordInput_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(passwordInput.InnerTextBox.Text))
            {
                errorProvider1.SetError(passwordInput.InnerTextBox, "Password is required.");
                e.Cancel = true; // Prevent focus change if validation fails
            }
            else
            {
                errorProvider1.SetError(passwordInput.InnerTextBox, string.Empty);
            }
        }

        private void signInButton_Click(object sender, EventArgs e)
        {
            if ((!string.IsNullOrWhiteSpace(passwordInput.InnerTextBox.Text) && !string.IsNullOrWhiteSpace(emailInput.InnerTextBox.Text)))
            {
                email = emailInput.InnerTextBox.Text.Trim();
                password = passwordInput.InnerTextBox.Text;
                if (Authenticate())
                {
                    //MessageBox.Show("Login successflly", "info", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    Form frm = new DashboardForm(_accountStroe.GetOrCreateAccount(email));
                    frm.ShowDialog();
                    return;
                    
                }
                else
                {
                    MessageBox.Show("password or email not correct", "error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Email or password is required", "error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
