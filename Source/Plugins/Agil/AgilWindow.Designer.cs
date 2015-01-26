namespace Cardinal.Plugins
{
    partial class AgilWindow
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
            this.SettingsButton = new System.Windows.Forms.PictureBox();
            this.WeightsButton = new System.Windows.Forms.PictureBox();
            this.ReloadButton = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.SettingsButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WeightsButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReloadButton)).BeginInit();
            this.SuspendLayout();
            // 
            // SettingsButton
            // 
            this.SettingsButton.Location = new System.Drawing.Point(90, 230);
            this.SettingsButton.Margin = new System.Windows.Forms.Padding(0);
            this.SettingsButton.MaximumSize = new System.Drawing.Size(250, 100);
            this.SettingsButton.MinimumSize = new System.Drawing.Size(250, 100);
            this.SettingsButton.Name = "SettingsButton";
            this.SettingsButton.Size = new System.Drawing.Size(250, 100);
            this.SettingsButton.TabIndex = 0;
            this.SettingsButton.TabStop = false;
            this.SettingsButton.Click += new System.EventHandler(this.SettingsButtonClick);
            this.SettingsButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SettingsButtonPressed);
            this.SettingsButton.MouseEnter += new System.EventHandler(this.SettingsButonHoverEnter);
            this.SettingsButton.MouseLeave += new System.EventHandler(this.SettingsButonHoverExit);
            this.SettingsButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SettingsButtonReleased);
            // 
            // WeightsButton
            // 
            this.WeightsButton.Location = new System.Drawing.Point(90, 360);
            this.WeightsButton.Margin = new System.Windows.Forms.Padding(0);
            this.WeightsButton.MaximumSize = new System.Drawing.Size(250, 100);
            this.WeightsButton.MinimumSize = new System.Drawing.Size(250, 100);
            this.WeightsButton.Name = "WeightsButton";
            this.WeightsButton.Size = new System.Drawing.Size(250, 100);
            this.WeightsButton.TabIndex = 1;
            this.WeightsButton.TabStop = false;
            this.WeightsButton.Click += new System.EventHandler(this.WeightsButtonClick);
            this.WeightsButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WeightsButtonPressed);
            this.WeightsButton.MouseEnter += new System.EventHandler(this.WeightsButonHoverEnter);
            this.WeightsButton.MouseLeave += new System.EventHandler(this.WeightsButonHoverExit);
            this.WeightsButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.WeightsButtonReleased);
            // 
            // ReloadButton
            // 
            this.ReloadButton.Location = new System.Drawing.Point(90, 490);
            this.ReloadButton.Margin = new System.Windows.Forms.Padding(0);
            this.ReloadButton.MaximumSize = new System.Drawing.Size(250, 100);
            this.ReloadButton.MinimumSize = new System.Drawing.Size(250, 100);
            this.ReloadButton.Name = "ReloadButton";
            this.ReloadButton.Size = new System.Drawing.Size(250, 100);
            this.ReloadButton.TabIndex = 2;
            this.ReloadButton.TabStop = false;
            this.ReloadButton.Click += new System.EventHandler(this.ReloadButtonClick);
            this.ReloadButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ReloadButtonPressed);
            this.ReloadButton.MouseEnter += new System.EventHandler(this.ReloadButonHoverEnter);
            this.ReloadButton.MouseLeave += new System.EventHandler(this.ReloadButonHoverExit);
            this.ReloadButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ReloadButtonReleased);
            // 
            // AgilWindow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(802, 773);
            this.Controls.Add(this.ReloadButton);
            this.Controls.Add(this.WeightsButton);
            this.Controls.Add(this.SettingsButton);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(820, 820);
            this.MinimumSize = new System.Drawing.Size(820, 820);
            this.Name = "AgilWindow";
            this.ShowIcon = false;
            this.Text = "Agil Settings";
            ((System.ComponentModel.ISupportInitialize)(this.SettingsButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WeightsButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReloadButton)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox SettingsButton;
        private System.Windows.Forms.PictureBox WeightsButton;
        private System.Windows.Forms.PictureBox ReloadButton;
    }
}