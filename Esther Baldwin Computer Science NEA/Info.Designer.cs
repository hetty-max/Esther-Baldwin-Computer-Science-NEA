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
            titleLabel = new Label();
            subtitleLabel = new Label();
            whatIsCard = new Panel();
            whatIsBody = new Label();
            whatIsHeading = new Label();
            howToCard = new Panel();
            howToBody = new Label();
            howToHeading = new Label();
            practiceCard = new Panel();
            gridButton3 = new Button();
            gridButton4 = new Button();
            gridButton2 = new Button();
            gridButton1 = new Button();
            practiceHint = new Label();
            practiceHeading = new Label();
            algorithmsCard = new Panel();
            algorithmsBody = new Label();
            algorithmsHeading = new Label();
            exitButton = new Button();
            whatIsCard.SuspendLayout();
            howToCard.SuspendLayout();
            practiceCard.SuspendLayout();
            algorithmsCard.SuspendLayout();
            SuspendLayout();
            //
            // titleLabel
            //
            titleLabel.AutoSize = true;
            titleLabel.BackColor = Color.Transparent;
            titleLabel.Font = new Font("Franklin Gothic Book", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.MidnightBlue;
            titleLabel.Location = new Point(40, 28);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(240, 45);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "HOW TO PLAY";
            //
            // subtitleLabel
            //
            subtitleLabel.AutoSize = true;
            subtitleLabel.BackColor = Color.Transparent;
            subtitleLabel.Font = new Font("Segoe UI", 11F, FontStyle.Italic, GraphicsUnit.Point, 0);
            subtitleLabel.ForeColor = Color.FromArgb(107, 140, 174);
            subtitleLabel.Location = new Point(44, 80);
            subtitleLabel.Name = "subtitleLabel";
            subtitleLabel.Size = new Size(380, 30);
            subtitleLabel.TabIndex = 1;
            subtitleLabel.Text = "Everything you need to know about Train Tracks";
            //
            // whatIsCard
            //
            whatIsCard.BackColor = Color.White;
            whatIsCard.Controls.Add(whatIsBody);
            whatIsCard.Controls.Add(whatIsHeading);
            whatIsCard.Location = new Point(40, 130);
            whatIsCard.Name = "whatIsCard";
            whatIsCard.Size = new Size(280, 360);
            whatIsCard.TabIndex = 2;
            whatIsCard.Paint += Card_Paint;
            //
            // whatIsBody
            //
            whatIsBody.AutoSize = true;
            whatIsBody.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            whatIsBody.ForeColor = Color.FromArgb(44, 62, 80);
            whatIsBody.Location = new Point(16, 52);
            whatIsBody.MaximumSize = new Size(248, 0);
            whatIsBody.Name = "whatIsBody";
            whatIsBody.Size = new Size(248, 262);
            whatIsBody.TabIndex = 1;
            whatIsBody.Text = "Train Tracks is a logic puzzle: lay one continuous railway track from the start tile to the end tile.\r\n\r\nEach row and column has a number showing exactly how many track pieces it must contain.\r\n\r\nThe finished track must be a single connected path with no dead ends or stray pieces.";
            //
            // whatIsHeading
            //
            whatIsHeading.AutoSize = true;
            whatIsHeading.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            whatIsHeading.ForeColor = Color.FromArgb(21, 67, 96);
            whatIsHeading.Location = new Point(16, 14);
            whatIsHeading.Name = "whatIsHeading";
            whatIsHeading.Size = new Size(99, 25);
            whatIsHeading.TabIndex = 0;
            whatIsHeading.Text = "What is it?";
            //
            // howToCard
            //
            howToCard.BackColor = Color.White;
            howToCard.Controls.Add(howToBody);
            howToCard.Controls.Add(howToHeading);
            howToCard.Location = new Point(340, 130);
            howToCard.Name = "howToCard";
            howToCard.Size = new Size(280, 360);
            howToCard.TabIndex = 3;
            howToCard.Paint += Card_Paint;
            //
            // howToBody
            //
            howToBody.AutoSize = true;
            howToBody.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            howToBody.ForeColor = Color.FromArgb(44, 62, 80);
            howToBody.Location = new Point(16, 52);
            howToBody.MaximumSize = new Size(248, 0);
            howToBody.Name = "howToBody";
            howToBody.Size = new Size(248, 262);
            howToBody.TabIndex = 1;
            howToBody.Text = "Click a tile to cycle through its track piece - try it on the practice grid to the right.\r\n\r\n1. Check how many tracks each row and column needs.\r\n2. Place pieces to satisfy those counts.\r\n3. Keep going until a single path connects start to end.\r\n\r\nEvery piece should connect to another - no dead ends.";
            //
            // howToHeading
            //
            howToHeading.AutoSize = true;
            howToHeading.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            howToHeading.ForeColor = Color.FromArgb(21, 67, 96);
            howToHeading.Location = new Point(16, 14);
            howToHeading.Name = "howToHeading";
            howToHeading.Size = new Size(133, 25);
            howToHeading.TabIndex = 0;
            howToHeading.Text = "How to Solve";
            //
            // practiceCard
            //
            practiceCard.BackColor = Color.White;
            practiceCard.Controls.Add(gridButton3);
            practiceCard.Controls.Add(gridButton4);
            practiceCard.Controls.Add(gridButton2);
            practiceCard.Controls.Add(gridButton1);
            practiceCard.Controls.Add(practiceHint);
            practiceCard.Controls.Add(practiceHeading);
            practiceCard.Location = new Point(640, 130);
            practiceCard.Name = "practiceCard";
            practiceCard.Size = new Size(220, 360);
            practiceCard.TabIndex = 4;
            practiceCard.Paint += Card_Paint;
            //
            // gridButton3
            //
            gridButton3.Cursor = Cursors.Hand;
            gridButton3.FlatAppearance.BorderColor = Color.FromArgb(133, 193, 233);
            gridButton3.FlatAppearance.BorderSize = 2;
            gridButton3.FlatStyle = FlatStyle.Flat;
            gridButton3.Location = new Point(25, 190);
            gridButton3.Name = "gridButton3";
            gridButton3.Size = new Size(80, 80);
            gridButton3.TabIndex = 3;
            gridButton3.UseVisualStyleBackColor = true;
            gridButton3.Click += gridButton3_Click;
            //
            // gridButton4
            //
            gridButton4.Cursor = Cursors.Hand;
            gridButton4.FlatAppearance.BorderColor = Color.FromArgb(133, 193, 233);
            gridButton4.FlatAppearance.BorderSize = 2;
            gridButton4.FlatStyle = FlatStyle.Flat;
            gridButton4.Location = new Point(115, 190);
            gridButton4.Name = "gridButton4";
            gridButton4.Size = new Size(80, 80);
            gridButton4.TabIndex = 4;
            gridButton4.UseVisualStyleBackColor = true;
            gridButton4.Click += gridButton4_Click;
            //
            // gridButton2
            //
            gridButton2.Cursor = Cursors.Hand;
            gridButton2.FlatAppearance.BorderColor = Color.FromArgb(133, 193, 233);
            gridButton2.FlatAppearance.BorderSize = 2;
            gridButton2.FlatStyle = FlatStyle.Flat;
            gridButton2.Location = new Point(115, 100);
            gridButton2.Name = "gridButton2";
            gridButton2.Size = new Size(80, 80);
            gridButton2.TabIndex = 2;
            gridButton2.UseVisualStyleBackColor = true;
            gridButton2.Click += gridButton2_Click;
            //
            // gridButton1
            //
            gridButton1.Cursor = Cursors.Hand;
            gridButton1.FlatAppearance.BorderColor = Color.FromArgb(133, 193, 233);
            gridButton1.FlatAppearance.BorderSize = 2;
            gridButton1.FlatStyle = FlatStyle.Flat;
            gridButton1.Location = new Point(25, 100);
            gridButton1.Name = "gridButton1";
            gridButton1.Size = new Size(80, 80);
            gridButton1.TabIndex = 1;
            gridButton1.UseVisualStyleBackColor = true;
            gridButton1.Click += gridButton1_Click;
            //
            // practiceHint
            //
            practiceHint.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            practiceHint.ForeColor = Color.FromArgb(107, 140, 174);
            practiceHint.Location = new Point(16, 48);
            practiceHint.Name = "practiceHint";
            practiceHint.Size = new Size(188, 44);
            practiceHint.TabIndex = 1;
            practiceHint.Text = "Click a tile to cycle through its track piece.";
            //
            // practiceHeading
            //
            practiceHeading.AutoSize = true;
            practiceHeading.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            practiceHeading.ForeColor = Color.FromArgb(21, 67, 96);
            practiceHeading.Location = new Point(16, 14);
            practiceHeading.Name = "practiceHeading";
            practiceHeading.Size = new Size(140, 25);
            practiceHeading.TabIndex = 0;
            practiceHeading.Text = "Try it Yourself";
            //
            // algorithmsCard
            //
            algorithmsCard.BackColor = Color.White;
            algorithmsCard.Controls.Add(algorithmsBody);
            algorithmsCard.Controls.Add(algorithmsHeading);
            algorithmsCard.Location = new Point(40, 510);
            algorithmsCard.Name = "algorithmsCard";
            algorithmsCard.Size = new Size(820, 130);
            algorithmsCard.TabIndex = 5;
            algorithmsCard.Paint += Card_Paint;
            //
            // algorithmsBody
            //
            algorithmsBody.AutoSize = true;
            algorithmsBody.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            algorithmsBody.ForeColor = Color.FromArgb(44, 62, 80);
            algorithmsBody.Location = new Point(16, 44);
            algorithmsBody.MaximumSize = new Size(788, 0);
            algorithmsBody.Name = "algorithmsBody";
            algorithmsBody.Size = new Size(788, 74);
            algorithmsBody.TabIndex = 1;
            algorithmsBody.Text = "Head to the Solve menu to compare three approaches side by side: Depth First Search, Breadth First Search, and a constraint-based solver built with Google OR-Tools' CP-SAT. Each one shows its search process directly on the grid as it runs.";
            //
            // algorithmsHeading
            //
            algorithmsHeading.AutoSize = true;
            algorithmsHeading.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            algorithmsHeading.ForeColor = Color.FromArgb(21, 67, 96);
            algorithmsHeading.Location = new Point(16, 12);
            algorithmsHeading.Name = "algorithmsHeading";
            algorithmsHeading.Size = new Size(190, 25);
            algorithmsHeading.TabIndex = 0;
            algorithmsHeading.Text = "Solving Algorithms";
            //
            // exitButton
            //
            exitButton.BackColor = Color.FromArgb(52, 152, 219);
            exitButton.Cursor = Cursors.Hand;
            exitButton.FlatAppearance.BorderSize = 0;
            exitButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(93, 173, 226);
            exitButton.FlatStyle = FlatStyle.Flat;
            exitButton.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            exitButton.ForeColor = Color.White;
            exitButton.Location = new Point(700, 670);
            exitButton.Name = "exitButton";
            exitButton.Size = new Size(160, 48);
            exitButton.TabIndex = 6;
            exitButton.Text = "Close";
            exitButton.UseVisualStyleBackColor = false;
            exitButton.Click += Exit_Click;
            //
            // Info
            //
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(235, 244, 250);
            ClientSize = new Size(900, 760);
            Controls.Add(exitButton);
            Controls.Add(algorithmsCard);
            Controls.Add(practiceCard);
            Controls.Add(howToCard);
            Controls.Add(whatIsCard);
            Controls.Add(subtitleLabel);
            Controls.Add(titleLabel);
            Name = "Info";
            Text = "Info";
            whatIsCard.ResumeLayout(false);
            whatIsCard.PerformLayout();
            howToCard.ResumeLayout(false);
            howToCard.PerformLayout();
            practiceCard.ResumeLayout(false);
            algorithmsCard.ResumeLayout(false);
            algorithmsCard.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label titleLabel;
        private Label subtitleLabel;
        private Panel whatIsCard;
        private Label whatIsHeading;
        private Label whatIsBody;
        private Panel howToCard;
        private Label howToHeading;
        private Label howToBody;
        private Panel practiceCard;
        private Label practiceHeading;
        private Label practiceHint;
        private Button gridButton1;
        private Button gridButton2;
        private Button gridButton3;
        private Button gridButton4;
        private Panel algorithmsCard;
        private Label algorithmsHeading;
        private Label algorithmsBody;
        private Button exitButton;
    }
}
