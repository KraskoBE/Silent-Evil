namespace SilentEvil
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.nsTheme1 = new NSTheme();
            this.nsCheckBox1 = new NSCheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nsButton1 = new NSButton();
            this.nsTextBox2 = new NSTextBox();
            this.nsTextBox1 = new NSTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.nsControlButton1 = new NSControlButton();
            this.nsTheme1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // nsTheme1
            // 
            this.nsTheme1.AccentOffset = 0;
            this.nsTheme1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.nsTheme1.BorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.nsTheme1.Colors = new Bloom[0];
            this.nsTheme1.Controls.Add(this.nsCheckBox1);
            this.nsTheme1.Controls.Add(this.button1);
            this.nsTheme1.Controls.Add(this.label3);
            this.nsTheme1.Controls.Add(this.label2);
            this.nsTheme1.Controls.Add(this.label1);
            this.nsTheme1.Controls.Add(this.nsButton1);
            this.nsTheme1.Controls.Add(this.nsTextBox2);
            this.nsTheme1.Controls.Add(this.nsTextBox1);
            this.nsTheme1.Controls.Add(this.pictureBox1);
            this.nsTheme1.Controls.Add(this.nsControlButton1);
            this.nsTheme1.Customization = "";
            this.nsTheme1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nsTheme1.Font = new System.Drawing.Font("Verdana", 8F);
            this.nsTheme1.Image = null;
            this.nsTheme1.Location = new System.Drawing.Point(0, 0);
            this.nsTheme1.Movable = true;
            this.nsTheme1.Name = "nsTheme1";
            this.nsTheme1.NoRounding = false;
            this.nsTheme1.Sizable = false;
            this.nsTheme1.Size = new System.Drawing.Size(364, 287);
            this.nsTheme1.SmartBounds = true;
            this.nsTheme1.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.nsTheme1.TabIndex = 0;
            this.nsTheme1.Text = "Silent Evil";
            this.nsTheme1.TransparencyKey = System.Drawing.Color.Empty;
            this.nsTheme1.Transparent = false;
            this.nsTheme1.Click += new System.EventHandler(this.nsTheme1_Click);
            // 
            // nsCheckBox1
            // 
            this.nsCheckBox1.Checked = false;
            this.nsCheckBox1.Location = new System.Drawing.Point(252, 153);
            this.nsCheckBox1.Name = "nsCheckBox1";
            this.nsCheckBox1.Size = new System.Drawing.Size(100, 23);
            this.nsCheckBox1.TabIndex = 12;
            this.nsCheckBox1.Text = "Remember";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(42, 234);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(0, 0);
            this.button1.TabIndex = 11;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(3, 269);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(355, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "label3";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label3.Visible = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(122, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 25);
            this.label2.TabIndex = 9;
            this.label2.Text = "USERNAME";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(122, 183);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 25);
            this.label1.TabIndex = 8;
            this.label1.Text = "PASSWORD";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nsButton1
            // 
            this.nsButton1.Location = new System.Drawing.Point(130, 240);
            this.nsButton1.Name = "nsButton1";
            this.nsButton1.Size = new System.Drawing.Size(105, 23);
            this.nsButton1.TabIndex = 7;
            this.nsButton1.Text = "       LOGIN";
            this.nsButton1.Click += new System.EventHandler(this.nsButton1_Click);
            // 
            // nsTextBox2
            // 
            this.nsTextBox2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.nsTextBox2.Location = new System.Drawing.Point(121, 211);
            this.nsTextBox2.MaxLength = 32767;
            this.nsTextBox2.Multiline = false;
            this.nsTextBox2.Name = "nsTextBox2";
            this.nsTextBox2.ReadOnly = false;
            this.nsTextBox2.Size = new System.Drawing.Size(124, 23);
            this.nsTextBox2.TabIndex = 6;
            this.nsTextBox2.Text = "ljBlock4o98Krasen";
            this.nsTextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.nsTextBox2.UseSystemPasswordChar = true;
            // 
            // nsTextBox1
            // 
            this.nsTextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.nsTextBox1.Location = new System.Drawing.Point(121, 153);
            this.nsTextBox1.MaxLength = 32767;
            this.nsTextBox1.Multiline = false;
            this.nsTextBox1.Name = "nsTextBox1";
            this.nsTextBox1.ReadOnly = false;
            this.nsTextBox1.Size = new System.Drawing.Size(124, 23);
            this.nsTextBox1.TabIndex = 4;
            this.nsTextBox1.Text = "Block4o";
            this.nsTextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.nsTextBox1.UseSystemPasswordChar = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 32);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(340, 80);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // nsControlButton1
            // 
            this.nsControlButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nsControlButton1.ControlButton = NSControlButton.Button.Close;
            this.nsControlButton1.Location = new System.Drawing.Point(340, 5);
            this.nsControlButton1.Margin = new System.Windows.Forms.Padding(0);
            this.nsControlButton1.MaximumSize = new System.Drawing.Size(18, 20);
            this.nsControlButton1.MinimumSize = new System.Drawing.Size(18, 20);
            this.nsControlButton1.Name = "nsControlButton1";
            this.nsControlButton1.Size = new System.Drawing.Size(18, 20);
            this.nsControlButton1.TabIndex = 0;
            this.nsControlButton1.Text = "nsControlButton1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 287);
            this.Controls.Add(this.nsTheme1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Silent Evil";
            this.nsTheme1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private NSTheme nsTheme1;
        private NSControlButton nsControlButton1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private NSButton nsButton1;
        private NSTextBox nsTextBox2;
        private NSTextBox nsTextBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private NSCheckBox nsCheckBox1;


    }
}

