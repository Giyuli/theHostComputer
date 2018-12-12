namespace theHostComputer
{
    partial class DynamicForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DynamicForm));
            this.DyfushizedGraph = new ZedGraph.ZedGraphControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // DyfushizedGraph
            // 
            this.DyfushizedGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DyfushizedGraph.Location = new System.Drawing.Point(0, 1);
            this.DyfushizedGraph.Name = "DyfushizedGraph";
            this.DyfushizedGraph.ScrollGrace = 0D;
            this.DyfushizedGraph.ScrollMaxX = 0D;
            this.DyfushizedGraph.ScrollMaxY = 0D;
            this.DyfushizedGraph.ScrollMaxY2 = 0D;
            this.DyfushizedGraph.ScrollMinX = 0D;
            this.DyfushizedGraph.ScrollMinY = 0D;
            this.DyfushizedGraph.ScrollMinY2 = 0D;
            this.DyfushizedGraph.Size = new System.Drawing.Size(1035, 433);
            this.DyfushizedGraph.TabIndex = 16;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // DynamicForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1035, 435);
            this.Controls.Add(this.DyfushizedGraph);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DynamicForm";
            this.Text = "腐蚀动态曲线";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DynamicForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl DyfushizedGraph;
        private System.Windows.Forms.Timer timer1;
    }
}