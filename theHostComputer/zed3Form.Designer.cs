namespace theHostComputer
{
    partial class zed3Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(zed3Form));
            this.DyfushizedGraph = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // DyfushizedGraph
            // 
            this.DyfushizedGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DyfushizedGraph.Location = new System.Drawing.Point(2, 2);
            this.DyfushizedGraph.Name = "DyfushizedGraph";
            this.DyfushizedGraph.ScrollGrace = 0D;
            this.DyfushizedGraph.ScrollMaxX = 0D;
            this.DyfushizedGraph.ScrollMaxY = 0D;
            this.DyfushizedGraph.ScrollMaxY2 = 0D;
            this.DyfushizedGraph.ScrollMinX = 0D;
            this.DyfushizedGraph.ScrollMinY = 0D;
            this.DyfushizedGraph.ScrollMinY2 = 0D;
            this.DyfushizedGraph.Size = new System.Drawing.Size(1020, 419);
            this.DyfushizedGraph.TabIndex = 17;
            // 
            // zed3Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 423);
            this.Controls.Add(this.DyfushizedGraph);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "zed3Form";
            this.Text = "处理信号";
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.zed3Form_MouseMove);
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl DyfushizedGraph;
    }
}