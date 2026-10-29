namespace Payment_system
{
    partial class DashboardForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.balanceCaption = new System.Windows.Forms.Label();
            this.balanceAmount = new System.Windows.Forms.Label();
            this.balanceNote = new System.Windows.Forms.Label();
            this.currencyCaption = new System.Windows.Forms.Label();
            this.currencyValue = new System.Windows.Forms.Label();
            this.cardTitle = new System.Windows.Forms.Label();
            this.cardSubtitle = new System.Windows.Forms.Label();
            this.amountLabel = new System.Windows.Forms.Label();
            this.amountHint = new System.Windows.Forms.Label();
            this.cardMeta = new System.Windows.Forms.Label();
            this.copyright = new System.Windows.Forms.Label();
            this.closeButton = new Payment_system.WindowButton();
            this.minimizeButton = new Payment_system.WindowButton();
            this.securityNote = new Payment_system.StatusNote();
            this.printButton = new Payment_system.PaymentActionButton();
            this.withdrawButton = new Payment_system.PaymentActionButton();
            this.depositButton = new Payment_system.PaymentActionButton();
            this.amountInput = new Payment_system.GlassInput();
            this.statusPill = new Payment_system.StatusPill();
            this.cardEyebrow = new Payment_system.Eyebrow();
            this.eyebrow = new Payment_system.Eyebrow();
            this.userChip = new Payment_system.UserChip();
            this.brand = new Payment_system.PayWUBrand();
            this.SuspendLayout();
            // 
            // balanceCaption
            // 
            this.balanceCaption.BackColor = System.Drawing.Color.Transparent;
            this.balanceCaption.Font = new System.Drawing.Font("Segoe UI Variable Text", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.balanceCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(173)))), ((int)(((byte)(212)))));
            this.balanceCaption.Location = new System.Drawing.Point(74, 178);
            this.balanceCaption.Name = "balanceCaption";
            this.balanceCaption.Size = new System.Drawing.Size(300, 16);
            this.balanceCaption.TabIndex = 3;
            this.balanceCaption.Text = "AVAILABLE BALANCE";
            this.balanceCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // balanceAmount
            // 
            this.balanceAmount.AutoEllipsis = true;
            this.balanceAmount.BackColor = System.Drawing.Color.Transparent;
            this.balanceAmount.Font = new System.Drawing.Font("Segoe UI Variable Display", 50F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.balanceAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(244)))), ((int)(((byte)(255)))));
            this.balanceAmount.Location = new System.Drawing.Point(64, 196);
            this.balanceAmount.Name = "balanceAmount";
            this.balanceAmount.Size = new System.Drawing.Size(740, 88);
            this.balanceAmount.TabIndex = 4;
            this.balanceAmount.Text = "$24,860.00";
            this.balanceAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // balanceNote
            // 
            this.balanceNote.BackColor = System.Drawing.Color.Transparent;
            this.balanceNote.Font = new System.Drawing.Font("Segoe UI Variable Text", 9.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.balanceNote.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(144)))), ((int)(((byte)(188)))));
            this.balanceNote.Location = new System.Drawing.Point(74, 288);
            this.balanceNote.Name = "balanceNote";
            this.balanceNote.Size = new System.Drawing.Size(520, 20);
            this.balanceNote.TabIndex = 5;
            this.balanceNote.Text = "Available for deposits and withdrawals";
            this.balanceNote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // currencyCaption
            // 
            this.currencyCaption.BackColor = System.Drawing.Color.Transparent;
            this.currencyCaption.Font = new System.Drawing.Font("Segoe UI Variable Text", 7.6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currencyCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(123)))), ((int)(((byte)(166)))));
            this.currencyCaption.Location = new System.Drawing.Point(956, 210);
            this.currencyCaption.Name = "currencyCaption";
            this.currencyCaption.Size = new System.Drawing.Size(152, 16);
            this.currencyCaption.TabIndex = 6;
            this.currencyCaption.Text = "CURRENCY";
            this.currencyCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // currencyValue
            // 
            this.currencyValue.BackColor = System.Drawing.Color.Transparent;
            this.currencyValue.Font = new System.Drawing.Font("Segoe UI Variable Display", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currencyValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(222)))), ((int)(((byte)(250)))));
            this.currencyValue.Location = new System.Drawing.Point(956, 228);
            this.currencyValue.Name = "currencyValue";
            this.currencyValue.Size = new System.Drawing.Size(152, 30);
            this.currencyValue.TabIndex = 7;
            this.currencyValue.Text = "USD";
            this.currencyValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cardTitle
            // 
            this.cardTitle.BackColor = System.Drawing.Color.Transparent;
            this.cardTitle.Font = new System.Drawing.Font("Segoe UI Variable Display", 23.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(244)))), ((int)(((byte)(255)))));
            this.cardTitle.Location = new System.Drawing.Point(100, 374);
            this.cardTitle.Name = "cardTitle";
            this.cardTitle.Size = new System.Drawing.Size(520, 38);
            this.cardTitle.TabIndex = 9;
            this.cardTitle.Text = "New transaction";
            this.cardTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cardSubtitle
            // 
            this.cardSubtitle.BackColor = System.Drawing.Color.Transparent;
            this.cardSubtitle.Font = new System.Drawing.Font("Segoe UI Variable Text", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(163)))), ((int)(((byte)(205)))));
            this.cardSubtitle.Location = new System.Drawing.Point(100, 414);
            this.cardSubtitle.Name = "cardSubtitle";
            this.cardSubtitle.Size = new System.Drawing.Size(600, 21);
            this.cardSubtitle.TabIndex = 10;
            this.cardSubtitle.Text = "Enter an amount, then choose an action.";
            this.cardSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // amountLabel
            // 
            this.amountLabel.BackColor = System.Drawing.Color.Transparent;
            this.amountLabel.Font = new System.Drawing.Font("Segoe UI Variable Text", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.amountLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(172)))), ((int)(((byte)(211)))));
            this.amountLabel.Location = new System.Drawing.Point(100, 460);
            this.amountLabel.Name = "amountLabel";
            this.amountLabel.Size = new System.Drawing.Size(240, 16);
            this.amountLabel.TabIndex = 12;
            this.amountLabel.Text = "AMOUNT (USD)";
            this.amountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // amountHint
            // 
            this.amountHint.BackColor = System.Drawing.Color.Transparent;
            this.amountHint.Font = new System.Drawing.Font("Segoe UI Variable Text", 8.4F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.amountHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(133)))), ((int)(((byte)(121)))), ((int)(((byte)(166)))));
            this.amountHint.Location = new System.Drawing.Point(102, 546);
            this.amountHint.Name = "amountHint";
            this.amountHint.Size = new System.Drawing.Size(366, 18);
            this.amountHint.TabIndex = 13;
            this.amountHint.Text = "Your available balance is shown above.";
            this.amountHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cardMeta
            // 
            this.cardMeta.BackColor = System.Drawing.Color.Transparent;
            this.cardMeta.Font = new System.Drawing.Font("Segoe UI Variable Text", 7.7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardMeta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(112)))), ((int)(((byte)(156)))));
            this.cardMeta.Location = new System.Drawing.Point(800, 652);
            this.cardMeta.Name = "cardMeta";
            this.cardMeta.Size = new System.Drawing.Size(308, 18);
            this.cardMeta.TabIndex = 18;
            this.cardMeta.Text = "Encrypted payments  •  Instant transfers";
            this.cardMeta.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // copyright
            // 
            this.copyright.BackColor = System.Drawing.Color.Transparent;
            this.copyright.Font = new System.Drawing.Font("Segoe UI Variable Text", 8.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyright.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(81)))), ((int)(((byte)(118)))));
            this.copyright.Location = new System.Drawing.Point(74, 686);
            this.copyright.Name = "copyright";
            this.copyright.Size = new System.Drawing.Size(300, 18);
            this.copyright.TabIndex = 19;
            this.copyright.Text = "© 2026 Pay_WU";
            // 
            // closeButton
            // 
            this.closeButton.BackColor = System.Drawing.Color.Transparent;
            this.closeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.closeButton.Kind = Payment_system.WindowButtonKind.Close;
            this.closeButton.Location = new System.Drawing.Point(1128, 16);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(36, 36);
            this.closeButton.TabIndex = 21;
            this.closeButton.TabStop = false;
            // 
            // minimizeButton
            // 
            this.minimizeButton.BackColor = System.Drawing.Color.Transparent;
            this.minimizeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.minimizeButton.Kind = Payment_system.WindowButtonKind.Minimize;
            this.minimizeButton.Location = new System.Drawing.Point(1086, 16);
            this.minimizeButton.Name = "minimizeButton";
            this.minimizeButton.Size = new System.Drawing.Size(36, 36);
            this.minimizeButton.TabIndex = 20;
            this.minimizeButton.TabStop = false;
            // 
            // securityNote
            // 
            this.securityNote.BackColor = System.Drawing.Color.Transparent;
            this.securityNote.Location = new System.Drawing.Point(74, 650);
            this.securityNote.Name = "securityNote";
            this.securityNote.Size = new System.Drawing.Size(500, 24);
            this.securityNote.TabIndex = 17;
            this.securityNote.TabStop = false;
            this.securityNote.Text = "Secure by design. Every transaction protected.";
            // 
            // printButton
            // 
            this.printButton.ActionIcon = Payment_system.PayActionIcon.Print;
            this.printButton.BackColor = System.Drawing.Color.Transparent;
            this.printButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.printButton.Location = new System.Drawing.Point(902, 458);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(176, 130);
            this.printButton.Style = Payment_system.PayActionStyle.Quiet;
            this.printButton.Subtitle = "Download PDF";
            this.printButton.TabIndex = 16;
            this.printButton.TabStop = false;
            this.printButton.Text = "Print statement";
            this.printButton.Click += new System.EventHandler(this.printButton_Click);
            // 
            // withdrawButton
            // 
            this.withdrawButton.ActionIcon = Payment_system.PayActionIcon.Withdraw;
            this.withdrawButton.BackColor = System.Drawing.Color.Transparent;
            this.withdrawButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.withdrawButton.Location = new System.Drawing.Point(712, 458);
            this.withdrawButton.Name = "withdrawButton";
            this.withdrawButton.Size = new System.Drawing.Size(176, 130);
            this.withdrawButton.Style = Payment_system.PayActionStyle.Glass;
            this.withdrawButton.Subtitle = "To your bank";
            this.withdrawButton.TabIndex = 15;
            this.withdrawButton.TabStop = false;
            this.withdrawButton.Text = "Withdraw";
            this.withdrawButton.Click += new System.EventHandler(this.withdrawButton_Click);
            // 
            // depositButton
            // 
            this.depositButton.ActionIcon = Payment_system.PayActionIcon.Deposit;
            this.depositButton.BackColor = System.Drawing.Color.Transparent;
            this.depositButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.depositButton.Location = new System.Drawing.Point(522, 458);
            this.depositButton.Name = "depositButton";
            this.depositButton.Size = new System.Drawing.Size(176, 130);
            this.depositButton.Style = Payment_system.PayActionStyle.Primary;
            this.depositButton.Subtitle = "Add funds";
            this.depositButton.TabIndex = 14;
            this.depositButton.TabStop = false;
            this.depositButton.Text = "Deposit";
            this.depositButton.Click += new System.EventHandler(this.depositButton_Click);
            // 
            // amountInput
            // 
            this.amountInput.AccessibleFieldName = "Amount";
            this.amountInput.AccessibleName = "Amount";
            this.amountInput.BackColor = System.Drawing.Color.Transparent;
            this.amountInput.Icon = Payment_system.FieldIcon.Amount;
            this.amountInput.IsPassword = false;
            this.amountInput.Location = new System.Drawing.Point(100, 482);
            this.amountInput.Name = "amountInput";
            this.amountInput.PlaceholderText = "0.00";
            this.amountInput.Size = new System.Drawing.Size(366, 54);
            this.amountInput.TabIndex = 0;
            this.amountInput.TabStop = false;
            // 
            // statusPill
            // 
            this.statusPill.BackColor = System.Drawing.Color.Transparent;
            this.statusPill.Location = new System.Drawing.Point(942, 366);
            this.statusPill.Name = "statusPill";
            this.statusPill.Size = new System.Drawing.Size(138, 24);
            this.statusPill.TabIndex = 11;
            this.statusPill.TabStop = false;
            this.statusPill.Text = "SECURE PAYMENT";
            // 
            // cardEyebrow
            // 
            this.cardEyebrow.BackColor = System.Drawing.Color.Transparent;
            this.cardEyebrow.Location = new System.Drawing.Point(100, 352);
            this.cardEyebrow.Name = "cardEyebrow";
            this.cardEyebrow.Size = new System.Drawing.Size(260, 22);
            this.cardEyebrow.TabIndex = 8;
            this.cardEyebrow.TabStop = false;
            this.cardEyebrow.Text = "QUICK ACTIONS";
            // 
            // eyebrow
            // 
            this.eyebrow.BackColor = System.Drawing.Color.Transparent;
            this.eyebrow.Location = new System.Drawing.Point(72, 148);
            this.eyebrow.Name = "eyebrow";
            this.eyebrow.Size = new System.Drawing.Size(280, 22);
            this.eyebrow.TabIndex = 2;
            this.eyebrow.TabStop = false;
            this.eyebrow.Text = "YOUR PAY_WU WALLET";
            // 
            // userChip
            // 
            this.userChip.BackColor = System.Drawing.Color.Transparent;
            this.userChip.Cursor = System.Windows.Forms.Cursors.Hand;
            this.userChip.Initials = "";
            this.userChip.Location = new System.Drawing.Point(866, 54);
            this.userChip.Name = "userChip";
            this.userChip.Size = new System.Drawing.Size(250, 52);
            this.userChip.TabIndex = 1;
            this.userChip.TabStop = false;
            this.userChip.UserEmail = "";
            this.userChip.UserName = "";
            // 
            // brand
            // 
            this.brand.BackColor = System.Drawing.Color.Transparent;
            this.brand.Location = new System.Drawing.Point(72, 50);
            this.brand.Name = "brand";
            this.brand.Size = new System.Drawing.Size(260, 46);
            this.brand.TabIndex = 0;
            this.brand.TabStop = false;
            // 
            // DashboardForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(5)))), ((int)(((byte)(18)))));
            this.ClientSize = new System.Drawing.Size(1180, 760);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.minimizeButton);
            this.Controls.Add(this.copyright);
            this.Controls.Add(this.cardMeta);
            this.Controls.Add(this.securityNote);
            this.Controls.Add(this.printButton);
            this.Controls.Add(this.withdrawButton);
            this.Controls.Add(this.depositButton);
            this.Controls.Add(this.amountHint);
            this.Controls.Add(this.amountInput);
            this.Controls.Add(this.amountLabel);
            this.Controls.Add(this.statusPill);
            this.Controls.Add(this.cardSubtitle);
            this.Controls.Add(this.cardTitle);
            this.Controls.Add(this.cardEyebrow);
            this.Controls.Add(this.currencyValue);
            this.Controls.Add(this.currencyCaption);
            this.Controls.Add(this.balanceNote);
            this.Controls.Add(this.balanceAmount);
            this.Controls.Add(this.balanceCaption);
            this.Controls.Add(this.eyebrow);
            this.Controls.Add(this.userChip);
            this.Controls.Add(this.brand);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DashboardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pay_WU - Dashboard";
            this.ResumeLayout(false);

        }

        #endregion

        private PayWUBrand brand;
        private UserChip userChip;
        private Eyebrow eyebrow;
        private System.Windows.Forms.Label balanceCaption;
        private System.Windows.Forms.Label balanceAmount;
        private System.Windows.Forms.Label balanceNote;
        private System.Windows.Forms.Label currencyCaption;
        private System.Windows.Forms.Label currencyValue;
        private Eyebrow cardEyebrow;
        private System.Windows.Forms.Label cardTitle;
        private System.Windows.Forms.Label cardSubtitle;
        private StatusPill statusPill;
        private System.Windows.Forms.Label amountLabel;
        private GlassInput amountInput;
        private System.Windows.Forms.Label amountHint;
        private PaymentActionButton depositButton;
        private PaymentActionButton withdrawButton;
        private PaymentActionButton printButton;
        private StatusNote securityNote;
        private System.Windows.Forms.Label cardMeta;
        private System.Windows.Forms.Label copyright;
        private WindowButton minimizeButton;
        private WindowButton closeButton;
    }
}
