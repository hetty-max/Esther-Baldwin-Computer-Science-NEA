namespace Esther_Baldwin_Computer_Science_NEA
{
    partial class Info
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Info));
            button3 = new Button();
            gridButton1 = new Button();
            button2 = new Button();
            textBox1 = new TextBox();
            gridButton3 = new Button();
            gridButton4 = new Button();
            gridButton2 = new Button();
            button1 = new Button();
            button4 = new Button();
            SuspendLayout();
            // 
            // button3
            // 
            button3.Location = new Point(652, 467);
            button3.Name = "button3";
            button3.Size = new Size(161, 48);
            button3.TabIndex = 14;
            button3.Text = "Exit";
            button3.UseVisualStyleBackColor = true;
            button3.Click += Exit_Click;
            // 
            // gridButton1
            // 
            gridButton1.Location = new Point(649, 15);
            gridButton1.Name = "gridButton1";
            gridButton1.Size = new Size(80, 80);
            gridButton1.TabIndex = 15;
            gridButton1.UseVisualStyleBackColor = true;
            gridButton1.Click += gridButton1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(12, 55);
            button2.Name = "button2";
            button2.Size = new Size(275, 443);
            button2.TabIndex = 16;
            button2.Text = resources.GetString("button2.Text");
            button2.TextAlign = ContentAlignment.TopLeft;
            button2.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.Window;
            textBox1.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBox1.Location = new Point(12, 12);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(113, 37);
            textBox1.TabIndex = 17;
            textBox1.Text = "Info";
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // gridButton3
            // 
            gridButton3.Location = new Point(649, 101);
            gridButton3.Name = "gridButton3";
            gridButton3.Size = new Size(80, 80);
            gridButton3.TabIndex = 18;
            gridButton3.UseVisualStyleBackColor = true;
            gridButton3.Click += gridButton3_Click;
            // 
            // gridButton4
            // 
            gridButton4.Location = new Point(735, 101);
            gridButton4.Name = "gridButton4";
            gridButton4.Size = new Size(80, 80);
            gridButton4.TabIndex = 19;
            gridButton4.UseVisualStyleBackColor = true;
            gridButton4.Click += gridButton4_Click;
            // 
            // gridButton2
            // 
            gridButton2.Location = new Point(735, 15);
            gridButton2.Name = "gridButton2";
            gridButton2.Size = new Size(80, 80);
            gridButton2.TabIndex = 20;
            gridButton2.UseVisualStyleBackColor = true;
            gridButton2.Click += gridButton2_Click;
            // 
            // button1
            // 
            button1.Location = new Point(293, 15);
            button1.Name = "button1";
            button1.Size = new Size(350, 394);
            button1.TabIndex = 21;
            button1.Text = resources.GetString("button1.Text");
            button1.TextAlign = ContentAlignment.TopLeft;
            button1.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Location = new Point(293, 415);
            button4.Name = "button4";
            button4.Size = new Size(350, 100);
            button4.TabIndex = 22;
            button4.Text = "In the solving menu depth first search and breadth first search are used to solve the puzzle this process is shown in the UI.\r\n";
            button4.TextAlign = ContentAlignment.TopLeft;
            button4.UseVisualStyleBackColor = true;
            // 
            // Info
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(825, 534);
            Controls.Add(button4);
            Controls.Add(button1);
            Controls.Add(gridButton2);
            Controls.Add(gridButton4);
            Controls.Add(gridButton3);
            Controls.Add(textBox1);
            Controls.Add(button2);
            Controls.Add(gridButton1);
            Controls.Add(button3);
            Name = "Info";
            Text = "Info";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button3;
        private Button gridButton1;
        private Button button2;
        private TextBox textBox1;
        private Button gridButton3;
        private Button gridButton4;
        private Button gridButton2;
        private Button button1;
        private Button button4;
    }
}