namespace Esther_Baldwin_Computer_Science_NEA
{
    partial class Generate
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
            panelGrid = new Panel();
            GenerateGridButton = new Button();
            difficultyDropDown = new ComboBox();
            dfs = new Button();
            ConfirmPuzzle = new Button();
            randWalk = new Button();
            reset = new Button();
            problemview = new Button();
            solveproblem = new Button();
            checksolution = new Button();
            button3 = new Button();
            SuspendLayout();
            // 
            // panelGrid
            // 
            panelGrid.Location = new Point(29, 27);
            panelGrid.Name = "panelGrid";
            panelGrid.Size = new Size(600, 600);
            panelGrid.TabIndex = 9;
            // 
            // GenerateGridButton
            // 
            GenerateGridButton.Location = new Point(648, 77);
            GenerateGridButton.Name = "GenerateGridButton";
            GenerateGridButton.Size = new Size(153, 72);
            GenerateGridButton.TabIndex = 10;
            GenerateGridButton.Text = "Generate Grid";
            GenerateGridButton.UseVisualStyleBackColor = true;
            GenerateGridButton.Click += GenerateButton_Click;
            // 
            // difficultyDropDown
            // 
            difficultyDropDown.FormattingEnabled = true;
            difficultyDropDown.Items.AddRange(new object[] { "Easy (6x6)", "Medium (8x8) ", "Hard (10x10)" });
            difficultyDropDown.Location = new Point(648, 38);
            difficultyDropDown.Name = "difficultyDropDown";
            difficultyDropDown.Size = new Size(182, 33);
            difficultyDropDown.TabIndex = 11;
            // 
            // dfs
            // 
            dfs.Location = new Point(648, 155);
            dfs.Name = "dfs";
            dfs.Size = new Size(103, 55);
            dfs.TabIndex = 12;
            dfs.Text = "DFS";
            dfs.UseVisualStyleBackColor = true;
            dfs.Click += dfs_Click_1;
            // 
            // ConfirmPuzzle
            // 
            ConfirmPuzzle.Location = new Point(807, 77);
            ConfirmPuzzle.Name = "ConfirmPuzzle";
            ConfirmPuzzle.Size = new Size(173, 72);
            ConfirmPuzzle.TabIndex = 13;
            ConfirmPuzzle.Text = "Generate Problem";
            ConfirmPuzzle.UseVisualStyleBackColor = true;
            ConfirmPuzzle.Click += ConfirmPuzzle_Click;
            // 
            // randWalk
            // 
            randWalk.Location = new Point(757, 155);
            randWalk.Name = "randWalk";
            randWalk.Size = new Size(154, 55);
            randWalk.TabIndex = 14;
            randWalk.Text = "Random Walk";
            randWalk.UseVisualStyleBackColor = true;
            randWalk.Click += RandomRecursion_Click_1;
            // 
            // reset
            // 
            reset.Cursor = Cursors.No;
            reset.Location = new Point(835, 644);
            reset.Name = "reset";
            reset.Size = new Size(161, 48);
            reset.TabIndex = 15;
            reset.Text = "Reset";
            reset.UseVisualStyleBackColor = true;
            reset.Click += reset_Click;
            // 
            // problemview
            // 
            problemview.Location = new Point(648, 216);
            problemview.Name = "problemview";
            problemview.Size = new Size(153, 60);
            problemview.TabIndex = 16;
            problemview.Text = "Problem View";
            problemview.UseVisualStyleBackColor = true;
            problemview.Click += problemview_Click;
            // 
            // solveproblem
            // 
            solveproblem.Location = new Point(648, 77);
            solveproblem.Name = "solveproblem";
            solveproblem.Size = new Size(173, 60);
            solveproblem.TabIndex = 17;
            solveproblem.Text = "Solve Problem";
            solveproblem.UseVisualStyleBackColor = true;
            solveproblem.Click += solveproblem_Click;
            // 
            // checksolution
            // 
            checksolution.Location = new Point(648, 143);
            checksolution.Name = "checksolution";
            checksolution.Size = new Size(173, 60);
            checksolution.TabIndex = 18;
            checksolution.Text = "Check Solution";
            checksolution.UseVisualStyleBackColor = true;
            checksolution.Click += checksolution_Click;
            // 
            // button3
            // 
            button3.Location = new Point(668, 644);
            button3.Name = "button3";
            button3.Size = new Size(161, 48);
            button3.TabIndex = 19;
            button3.Text = "Exit";
            button3.UseVisualStyleBackColor = true;
            button3.Click += Exit;
            // 
            // Generate
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1008, 704);
            Controls.Add(button3);
            Controls.Add(checksolution);
            Controls.Add(solveproblem);
            Controls.Add(problemview);
            Controls.Add(reset);
            Controls.Add(randWalk);
            Controls.Add(ConfirmPuzzle);
            Controls.Add(dfs);
            Controls.Add(difficultyDropDown);
            Controls.Add(GenerateGridButton);
            Controls.Add(panelGrid);
            Name = "Generate";
            Text = "Generate";
            ResumeLayout(false);
        }

        #endregion

        private Panel panelGrid;
        private Button GenerateGridButton;
        private ComboBox difficultyDropDown;
        private Button dfs;
        private Button ConfirmPuzzle;
        private Button randWalk;
        private Button reset;
        private Button problemview;
        private Button solveproblem;
        private Button checksolution;
        private Button button3;
    }
}