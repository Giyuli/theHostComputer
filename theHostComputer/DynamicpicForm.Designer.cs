namespace theHostComputer
{
    partial class DynamicpicForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DynamicpicForm));
            this.Dynamicpic = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.Dynamicpic)).BeginInit();
            this.SuspendLayout();
            // 
            // Dynamicpic
            // 
            this.Dynamicpic.BackColor = System.Drawing.Color.White;
            this.Dynamicpic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Dynamicpic.Location = new System.Drawing.Point(0, 0);
            this.Dynamicpic.Name = "Dynamicpic";
            this.Dynamicpic.Size = new System.Drawing.Size(745, 323);
            this.Dynamicpic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Dynamicpic.TabIndex = 0;
            this.Dynamicpic.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // DynamicpicForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(745, 323);
            this.Controls.Add(this.Dynamicpic);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DynamicpicForm";
            this.Text = "动态腐蚀云图";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DynamicpicForm_FormClosed);
            this.Load += new System.EventHandler(this.DynamicpicForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Dynamicpic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Dynamicpic;
        private System.Windows.Forms.Timer timer1;
    }
}