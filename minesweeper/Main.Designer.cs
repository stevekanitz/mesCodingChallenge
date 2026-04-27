
namespace Minesweeper
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            MineField = new MineField();
            ResetButton = new Button();
            SuspendLayout();
            // 
            // MineField
            // 
            MineField.BackgroundImage = (Image)resources.GetObject("MineField.BackgroundImage");
            MineField.BackgroundImageLayout = ImageLayout.None;
            MineField.Location = new Point(9, 38);
            MineField.Margin = new Padding(0);
            MineField.Name = "MineField";
            MineField.Size = new Size(144, 144);
            MineField.TabIndex = 0;
            MineField.Text = "mineField1";
            MineField.CellClicked += MineField_CellClicked;
            // 
            // ResetButton
            // 
            ResetButton.Location = new Point(44, 12);
            ResetButton.Name = "ResetButton";
            ResetButton.Size = new Size(75, 23);
            ResetButton.TabIndex = 1;
            ResetButton.Text = "Reset";
            ResetButton.UseVisualStyleBackColor = true;
            ResetButton.Click += ResetButton_Click;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(164, 193);
            Controls.Add(ResetButton);
            Controls.Add(MineField);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Main";
            Text = "Minesweeper";
            ResumeLayout(false);
        }

        #endregion

        private MineField MineField;
        private Button ResetButton;
    }
}