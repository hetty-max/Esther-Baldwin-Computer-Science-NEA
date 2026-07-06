namespace Esther_Baldwin_Computer_Science_NEA
{
    partial class Menu
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
            generate = new Button();
            solve = new Button();
            set = new Button();
            textBox1 = new TextBox();
            Info = new Button();
            SuspendLayout();
            // 
            // generate
            // 
            generate.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            generate.Location = new Point(322, 154);
            generate.Name = "generate";
            generate.Size = new Size(151, 59);
            generate.TabIndex = 0;
            generate.Text = "Generate";
            generate.UseVisualStyleBackColor = true;
            generate.Click += generate_Click;
            // 
            // solve
            // 
            solve.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            solve.Location = new Point(322, 219);
            solve.Name = "solve";
            solve.Size = new Size(151, 63);
            solve.TabIndex = 1;
            solve.Text = "Solve";
            solve.UseVisualStyleBackColor = true;
            solve.Click += solve_Click;
            // 
            // set
            // 
            set.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            set.Location = new Point(322, 288);
            set.Name = "set";
            set.Size = new Size(151, 61);
            set.TabIndex = 2;
            set.Text = "Set";
            set.UseVisualStyleBackColor = true;
            set.Click += set_Click;
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.Window;
            textBox1.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.Location = new Point(12, 25);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(235, 37);
            textBox1.TabIndex = 3;
            textBox1.Text = "Train Tracks Puzzle";
            // 
            // Info
            // 
            Info.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Info.Location = new Point(12, 68);
            Info.Name = "Info";
            Info.Size = new Size(151, 35);
            Info.TabIndex = 4;
            Info.Text = "Info";
            Info.UseVisualStyleBackColor = true;
            Info.Click += Info_Click;
            // 
            // Menu
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(838, 518);
            Controls.Add(Info);
            Controls.Add(textBox1);
            Controls.Add(set);
            Controls.Add(solve);
            Controls.Add(generate);
            Name = "Menu";
            Text = "Menu";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button generate;
        private Button solve;
        private Button set;
        private TextBox textBox1;
        private Button Info;
    }
}