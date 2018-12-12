namespace theHostComputer
{
    partial class EndingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EndingForm));
            this.label1 = new System.Windows.Forms.Label();
            this.back_to_desktop = new System.Windows.Forms.Button();
            this.close_pc = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 42F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(74, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(511, 56);
            this.label1.TabIndex = 9;
            this.label1.Text = "欢 迎 再 次 使 用";
            // 
            // back_to_desktop
            // 
            this.back_to_desktop.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.back_to_desktop.Location = new System.Drawing.Point(429, 176);
            this.back_to_desktop.Name = "back_to_desktop";
            this.back_to_desktop.Size = new System.Drawing.Size(106, 45);
            this.back_to_desktop.TabIndex = 10;
            this.back_to_desktop.Text = "返回桌面";
            this.back_to_desktop.UseVisualStyleBackColor = true;
            this.back_to_desktop.Click += new System.EventHandler(this.back_to_desktop_Click);
            // 
            // close_pc
            // 
            this.close_pc.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.close_pc.Location = new System.Drawing.Point(541, 274);
            this.close_pc.Name = "close_pc";
            this.close_pc.Size = new System.Drawing.Size(119, 45);
            this.close_pc.TabIndex = 11;
            this.close_pc.Text = "关   机";
            this.close_pc.UseVisualStyleBackColor = true;
            this.close_pc.Click += new System.EventHandler(this.close_pc_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(169, 176);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 45);
            this.button1.TabIndex = 12;
            this.button1.Text = "主菜单";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // EndingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 331);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.close_pc);
            this.Controls.Add(this.back_to_desktop);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EndingForm";
            this.Text = "弱磁管道非开挖检测系统";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button back_to_desktop;
        private System.Windows.Forms.Button close_pc;
        private System.Windows.Forms.Button button1;

    }
}