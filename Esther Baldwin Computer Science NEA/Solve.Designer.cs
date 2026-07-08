namespace Esther_Baldwin_Computer_Science_NEA
{
    partial class Solve
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
            difficultyDropDown = new ComboBox();
            bfs = new Button();
            dfs = new Button();
            sat = new Button();
            nextSolution = new Button();
            solutionInfo = new TextBox();
            generategrid = new Button();
            solveprob = new Button();
            steps = new Button();
            panelGrid = new Panel();
            Startmessage = new TextBox();
            reset = new Button();
            button3 = new Button();
            OptimisationPick = new ComboBox();
            SuspendLayout();
            // 
            // difficultyDropDown
            // 
            difficultyDropDown.FormattingEnabled = true;
            difficultyDropDown.Items.AddRange(new object[] { "Easy (6x6)", "Medium (8x8) ", "Hard (10x10)" });
            difficultyDropDown.Location = new Point(630, 25);
            difficultyDropDown.Name = "difficultyDropDown";
            difficultyDropDown.Size = new Size(182, 33);
            difficultyDropDown.TabIndex = 1;
            // 
            // bfs
            // 
            bfs.Location = new Point(818, 137);
            bfs.Name = "bfs";
            bfs.Size = new Size(182, 57);
            bfs.TabIndex = 3;
            bfs.Text = "Using BFS";
            bfs.UseVisualStyleBackColor = true;
            bfs.Click += bfs_Click_1;
            // 
            // dfs
            // 
            dfs.Location = new Point(818, 74);
            dfs.Name = "dfs";
            dfs.Size = new Size(182, 57);
            dfs.TabIndex = 4;
            dfs.Text = "Using DFS";
            dfs.UseVisualStyleBackColor = true;
            dfs.Click += dfs_Click;
            //
            // sat
            //
            sat.Location = new Point(818, 200);
            sat.Name = "sat";
            sat.Size = new Size(182, 57);
            sat.TabIndex = 15;
            sat.Text = "Using SAT";
            sat.UseVisualStyleBackColor = true;
            sat.Click += sat_Click;
            //
            // nextSolution
            //
            nextSolution.Location = new Point(818, 263);
            nextSolution.Name = "nextSolution";
            nextSolution.Size = new Size(182, 57);
            nextSolution.TabIndex = 16;
            nextSolution.Text = "Next Solution";
            nextSolution.UseVisualStyleBackColor = true;
            nextSolution.Click += nextSolution_Click;
            //
            // solutionInfo
            //
            solutionInfo.Location = new Point(630, 263);
            solutionInfo.Name = "solutionInfo";
            solutionInfo.ReadOnly = true;
            solutionInfo.Size = new Size(182, 31);
            solutionInfo.TabIndex = 17;
            solutionInfo.TextAlign = HorizontalAlignment.Center;
            solutionInfo.Visible = false;
            //
            // generategrid
            // 
            generategrid.Location = new Point(630, 74);
            generategrid.Name = "generategrid";
            generategrid.Size = new Size(182, 57);
            generategrid.TabIndex = 5;
            generategrid.Text = "Create Grid";
            generategrid.UseVisualStyleBackColor = true;
            generategrid.Click += GernerateGrid_Click;
            // 
            // solveprob
            // 
            solveprob.Location = new Point(630, 137);
            solveprob.Name = "solveprob";
            solveprob.Size = new Size(182, 57);
            solveprob.TabIndex = 6;
            solveprob.Text = "Solve Problem";
            solveprob.UseVisualStyleBackColor = true;
            solveprob.Click += solveprob_Click;
            // 
            // steps
            // 
            steps.Location = new Point(839, 374);
            steps.Name = "steps";
            steps.Size = new Size(161, 48);
            steps.TabIndex = 7;
            steps.Text = "Steps: 0";
            steps.UseVisualStyleBackColor = true;
            // 
            // panelGrid
            // 
            panelGrid.Location = new Point(24, 25);
            panelGrid.Name = "panelGrid";
            panelGrid.Size = new Size(600, 600);
            panelGrid.TabIndex = 8;
            //
            // Startmessage
            // 
            Startmessage.Location = new Point(630, 337);
            Startmessage.Name = "Startmessage";
            Startmessage.Size = new Size(301, 31);
            Startmessage.TabIndex = 10;
            Startmessage.Text = "Please enter a puzzle to be solved.";
            // 
            // reset
            // 
            reset.Location = new Point(835, 644);
            reset.Name = "reset";
            reset.Size = new Size(161, 48);
            reset.TabIndex = 12;
            reset.Text = "Reset";
            reset.UseVisualStyleBackColor = true;
            reset.Click += Reset_Click;
            // 
            // button3
            // 
            button3.Location = new Point(668, 644);
            button3.Name = "button3";
            button3.Size = new Size(161, 48);
            button3.TabIndex = 13;
            button3.Text = "Exit";
            button3.UseVisualStyleBackColor = true;
            button3.Click += Exit;
            // 
            // OptimisationPick
            // 
            OptimisationPick.FormattingEnabled = true;
            OptimisationPick.Items.AddRange(new object[] { "Distance Away", "Row and Column", "Combined" });
            OptimisationPick.Location = new Point(630, 200);
            OptimisationPick.Name = "OptimisationPick";
            OptimisationPick.Size = new Size(182, 33);
            OptimisationPick.TabIndex = 14;
            OptimisationPick.Text = "Optimisation";
            // 
            // Solve
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1008, 704);
            Controls.Add(OptimisationPick);
            Controls.Add(button3);
            Controls.Add(reset);
            Controls.Add(Startmessage);
            Controls.Add(panelGrid);
            Controls.Add(steps);
            Controls.Add(solveprob);
            Controls.Add(generategrid);
            Controls.Add(solutionInfo);
            Controls.Add(nextSolution);
            Controls.Add(sat);
            Controls.Add(dfs);
            Controls.Add(bfs);
            Controls.Add(difficultyDropDown);
            Name = "Solve";
            Text = "Solve";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox difficultyDropDown;
        private Button bfs;
        private Button dfs;
        private Button sat;
        private Button nextSolution;
        private TextBox solutionInfo;
        private Button generategrid;
        private Button solveprob;
        private Button steps;
        private Panel panelGrid;
        private TextBox Startmessage;
        private Button reset;
        private Button button3;
        private ComboBox OptimisationPick;
    }
}