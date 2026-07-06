namespace Esther_Baldwin_Computer_Science_NEA
{
    partial class Set
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
            difficultyDropDown = new ComboBox();
            generateblankGrid = new Button();
            panelGrid = new Panel();
            reset = new Button();
            finish = new Button();
            Startmessage = new TextBox();
            valid = new TextBox();
            invalid = new TextBox();
            problemview = new Button();
            exit = new Button();
            solveproblem = new Button();
            checksolution = new Button();
            SuspendLayout();
            // 
            // difficultyDropDown
            // 
            difficultyDropDown.FormattingEnabled = true;
            difficultyDropDown.Items.AddRange(new object[] { "Easy (6x6)", "Medium (8x8) ", "Hard (10x10)" });
            difficultyDropDown.Location = new Point(664, 55);
            difficultyDropDown.Name = "difficultyDropDown";
            difficultyDropDown.Size = new Size(182, 33);
            difficultyDropDown.TabIndex = 0;
            //
            // generateblankGrid
            // 
            generateblankGrid.Location = new Point(664, 94);
            generateblankGrid.Name = "generateblankGrid";
            generateblankGrid.Size = new Size(133, 46);
            generateblankGrid.TabIndex = 1;
            generateblankGrid.Text = "Create Grid";
            generateblankGrid.UseVisualStyleBackColor = true;
            generateblankGrid.Click += generateGrid_Click;
            // 
            // panelGrid
            // 
            panelGrid.Location = new Point(37, 34);
            panelGrid.Name = "panelGrid";
            panelGrid.Size = new Size(600, 600);
            panelGrid.TabIndex = 2;
            //
            // reset
            // 
            reset.Location = new Point(925, 586);
            reset.Name = "reset";
            reset.Size = new Size(161, 48);
            reset.TabIndex = 6;
            reset.Text = "Reset";
            reset.UseVisualStyleBackColor = true;
            reset.Click += reset_Click;
            // 
            // finish
            // 
            finish.Location = new Point(664, 183);
            finish.Name = "finish";
            finish.Size = new Size(161, 48);
            finish.TabIndex = 7;
            finish.Text = "Finish";
            finish.UseVisualStyleBackColor = true;
            finish.Click += finish_Click;
            // 
            // Startmessage
            // 
            Startmessage.Location = new Point(664, 146);
            Startmessage.Name = "Startmessage";
            Startmessage.Size = new Size(432, 31);
            Startmessage.TabIndex = 9;
            Startmessage.Text = "Please select a start and end point before beginning!";
            // 
            // valid
            // 
            valid.Location = new Point(664, 237);
            valid.Name = "valid";
            valid.Size = new Size(161, 31);
            valid.TabIndex = 10;
            valid.Text = "Valid Grid!";
            // 
            // invalid
            // 
            invalid.Location = new Point(664, 237);
            invalid.Name = "invalid";
            invalid.Size = new Size(182, 31);
            invalid.TabIndex = 11;
            invalid.Text = "Invalid grid, try again.";
            //
            // problemview
            // 
            problemview.Location = new Point(664, 274);
            problemview.Name = "problemview";
            problemview.Size = new Size(161, 48);
            problemview.TabIndex = 13;
            problemview.Text = "Problem View";
            problemview.UseVisualStyleBackColor = true;
            problemview.Click += problemview_Click;
            // 
            // exit
            // 
            exit.Location = new Point(925, 532);
            exit.Name = "exit";
            exit.Size = new Size(161, 48);
            exit.TabIndex = 8;
            exit.Text = "Exit";
            exit.UseVisualStyleBackColor = true;
            exit.Click += exit_Click;
            // 
            // solveproblem
            // 
            solveproblem.Location = new Point(664, 94);
            solveproblem.Name = "solveproblem";
            solveproblem.Size = new Size(173, 60);
            solveproblem.TabIndex = 18;
            solveproblem.Text = "Solve Problem";
            solveproblem.UseVisualStyleBackColor = true;
            solveproblem.Click += solveproblem_Click;
            // 
            // checksolution
            // 
            checksolution.Location = new Point(664, 160);
            checksolution.Name = "checksolution";
            checksolution.Size = new Size(173, 60);
            checksolution.TabIndex = 19;
            checksolution.Text = "Check Solution";
            checksolution.UseVisualStyleBackColor = true;
            checksolution.Click += checksolution_Click;
            // 
            // Set
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1108, 704);
            Controls.Add(checksolution);
            Controls.Add(solveproblem);
            Controls.Add(problemview);
            Controls.Add(invalid);
            Controls.Add(valid);
            Controls.Add(Startmessage);
            Controls.Add(exit);
            Controls.Add(finish);
            Controls.Add(reset);
            Controls.Add(panelGrid);
            Controls.Add(generateblankGrid);
            Controls.Add(difficultyDropDown);
            Name = "Set";
            Text = "Set";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox difficultyDropDown;
        private Button generateblankGrid;
        private Panel panelGrid;
        private Button reset;
        private Button finish;
        private TextBox Startmessage;
        private TextBox valid;
        private TextBox invalid;
        private Button problemview;
        private Button exit;
        private Button solveproblem;
        private Button checksolution;
    }
}
