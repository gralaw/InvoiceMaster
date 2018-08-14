namespace InvoiceMaker
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.FileExplorerBtn = new System.Windows.Forms.Button();
            this.OpencsvFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 48);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(680, 29);
            this.textBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "CSV File to process";
            // 
            // FileExplorerBtn
            // 
            this.FileExplorerBtn.Location = new System.Drawing.Point(717, 48);
            this.FileExplorerBtn.Name = "FileExplorerBtn";
            this.FileExplorerBtn.Size = new System.Drawing.Size(75, 29);
            this.FileExplorerBtn.TabIndex = 2;
            this.FileExplorerBtn.Text = "File";
            this.FileExplorerBtn.UseVisualStyleBackColor = true;
            this.FileExplorerBtn.Click += new System.EventHandler(this.FileExplorerBtn_Click);
            // 
            // OpencsvFileDialog
            // 
            this.OpencsvFileDialog.FileName = "openFileDialog1";
            this.OpencsvFileDialog.Filter = "\"csv|*.csv\"";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 619);
            this.Controls.Add(this.FileExplorerBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Font = new System.Drawing.Font("Corbel", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "InvoiceMaker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button FileExplorerBtn;
        private System.Windows.Forms.OpenFileDialog OpencsvFileDialog;
    }
}

