namespace THAWFontWinForm
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBoxFont = new PictureBox();
            OpenFont = new Button();
            comboBoxGameVersion = new ComboBox();
            convertToThawButton = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBoxFont).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxFont
            // 
            pictureBoxFont.Location = new Point(-1, 0);
            pictureBoxFont.Name = "pictureBoxFont";
            pictureBoxFont.Size = new Size(472, 256);
            pictureBoxFont.TabIndex = 0;
            pictureBoxFont.TabStop = false;
            pictureBoxFont.Click += pictureBox1_Click;
            // 
            // OpenFont
            // 
            OpenFont.Location = new Point(688, 12);
            OpenFont.Name = "OpenFont";
            OpenFont.Size = new Size(100, 40);
            OpenFont.TabIndex = 1;
            OpenFont.Text = "Open Font";
            OpenFont.UseVisualStyleBackColor = true;
            OpenFont.Click += OpenFont_Click;
            // 
            // comboBoxGameVersion
            // 
            comboBoxGameVersion.FormattingEnabled = true;
            comboBoxGameVersion.Location = new Point(477, 22);
            comboBoxGameVersion.Name = "comboBoxGameVersion";
            comboBoxGameVersion.Size = new Size(205, 23);
            comboBoxGameVersion.TabIndex = 2;
            // 
            // convertToThawButton
            // 
            convertToThawButton.Location = new Point(688, 58);
            convertToThawButton.Name = "convertToThawButton";
            convertToThawButton.Size = new Size(100, 39);
            convertToThawButton.TabIndex = 3;
            convertToThawButton.Text = "Convert To THAW";
            convertToThawButton.UseVisualStyleBackColor = true;
            convertToThawButton.Click += convertToThawButton_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(convertToThawButton);
            Controls.Add(comboBoxGameVersion);
            Controls.Add(OpenFont);
            Controls.Add(pictureBoxFont);
            Name = "MainForm";
            Text = "THAWFont";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBoxFont).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBoxFont;
        private Button OpenFont;
        private ComboBox comboBoxGameVersion;
        private Button convertToThawButton;
    }
}