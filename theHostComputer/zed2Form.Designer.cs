namespace theHostComputer
{
    partial class zed2Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(zed2Form));
            this.DyfushizedGraph = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // DyfushizedGraph
            // 
            this.DyfushizedGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DyfushizedGraph.Location = new System.Drawing.Point(1, 2);
            this.DyfushizedGraph.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.DyfushizedGraph.Name = "DyfushizedGraph";
            this.DyfushizedGraph.ScrollGrace = 0D;
            this.DyfushizedGraph.ScrollMaxX = 0D;
            this.DyfushizedGraph.ScrollMaxY = 0D;
            this.DyfushizedGraph.ScrollMaxY2 = 0D;
            this.DyfushizedGraph.ScrollMinX = 0D;
            this.DyfushizedGraph.ScrollMinY = 0D;
            this.DyfushizedGraph.ScrollMinY2 = 0D;
            this.DyfushizedGraph.Size = new System.Drawing.Size(1079, 456);
            this.DyfushizedGraph.TabIndex = 17;
            this.DyfushizedGraph.Load += new System.EventHandler(this.DyfushizedGraph_Load);
            // 
            // zed2Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1081, 459);
            this.Controls.Add(this.DyfushizedGraph);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "zed2Form";
            this.Text = "缺陷信号";
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.zed2Form_MouseMove);
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl DyfushizedGraph;
    }
}