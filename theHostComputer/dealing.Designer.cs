namespace theHostComputer
{
    partial class dealing
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dealing));
            this.tabControl_2 = new System.Windows.Forms.TabControl();
            this.tabPage1_2 = new System.Windows.Forms.TabPage();
            this.Channel = new System.Windows.Forms.ComboBox();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.tabPage3_2 = new System.Windows.Forms.TabPage();
            this.Channel_2 = new System.Windows.Forms.ComboBox();
            this.zedGraphControl2 = new ZedGraph.ZedGraphControl();
            this.NoText_3 = new System.Windows.Forms.TextBox();
            this.FileText_3 = new System.Windows.Forms.TextBox();
            this.label2_2 = new System.Windows.Forms.Label();
            this.label9_2 = new System.Windows.Forms.Label();
            this.TLBox_3 = new System.Windows.Forms.TextBox();
            this.label19_2 = new System.Windows.Forms.Label();
            this.TText_3 = new System.Windows.Forms.TextBox();
            this.DText_3 = new System.Windows.Forms.TextBox();
            this.DisText_3 = new System.Windows.Forms.TextBox();
            this.label3_2 = new System.Windows.Forms.Label();
            this.label1_2 = new System.Windows.Forms.Label();
            this.label8_2 = new System.Windows.Forms.Label();
            this.qccdtext_3 = new System.Windows.Forms.TextBox();
            this.label14_2 = new System.Windows.Forms.Label();
            this.cdtext_3 = new System.Windows.Forms.TextBox();
            this.bfgstext_3 = new System.Windows.Forms.TextBox();
            this.yztext_3 = new System.Windows.Forms.TextBox();
            this.label13_2 = new System.Windows.Forms.Label();
            this.label12_2 = new System.Windows.Forms.Label();
            this.label11_2 = new System.Windows.Forms.Label();
            this.browseButton_3 = new System.Windows.Forms.Button();
            this.SaveButton_3 = new System.Windows.Forms.Button();
            this.runButton_3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.browsetimeBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.length_all = new System.Windows.Forms.TextBox();
            this.tabControl_2.SuspendLayout();
            this.tabPage1_2.SuspendLayout();
            this.tabPage3_2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl_2
            // 
            this.tabControl_2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl_2.Controls.Add(this.tabPage1_2);
            this.tabControl_2.Controls.Add(this.tabPage3_2);
            this.tabControl_2.Location = new System.Drawing.Point(12, 12);
            this.tabControl_2.Name = "tabControl_2";
            this.tabControl_2.SelectedIndex = 0;
            this.tabControl_2.Size = new System.Drawing.Size(763, 412);
            this.tabControl_2.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl_2.TabIndex = 19;
            // 
            // tabPage1_2
            // 
            this.tabPage1_2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tabPage1_2.Controls.Add(this.Channel);
            this.tabPage1_2.Controls.Add(this.zedGraphControl1);
            this.tabPage1_2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabPage1_2.Location = new System.Drawing.Point(4, 22);
            this.tabPage1_2.Name = "tabPage1_2";
            this.tabPage1_2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1_2.Size = new System.Drawing.Size(755, 386);
            this.tabPage1_2.TabIndex = 0;
            this.tabPage1_2.Tag = "";
            this.tabPage1_2.Text = "总览";
            this.tabPage1_2.UseVisualStyleBackColor = true;
            // 
            // Channel
            // 
            this.Channel.FormattingEnabled = true;
            this.Channel.Items.AddRange(new object[] {
            "X",
            "Y",
            "Z"});
            this.Channel.Location = new System.Drawing.Point(20, 13);
            this.Channel.Name = "Channel";
            this.Channel.Size = new System.Drawing.Size(59, 23);
            this.Channel.TabIndex = 83;
            this.Channel.Text = "X";
            this.Channel.SelectedIndexChanged += new System.EventHandler(this.Channel_SelectedIndexChanged);
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.zedGraphControl1.IsShowPointValues = true;
            this.zedGraphControl1.Location = new System.Drawing.Point(6, 6);
            this.zedGraphControl1.Margin = new System.Windows.Forms.Padding(2);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(742, 375);
            this.zedGraphControl1.TabIndex = 15;
            this.zedGraphControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dealing_MouseMove);
            // 
            // tabPage3_2
            // 
            this.tabPage3_2.Controls.Add(this.Channel_2);
            this.tabPage3_2.Controls.Add(this.zedGraphControl2);
            this.tabPage3_2.Location = new System.Drawing.Point(4, 22);
            this.tabPage3_2.Name = "tabPage3_2";
            this.tabPage3_2.Size = new System.Drawing.Size(755, 386);
            this.tabPage3_2.TabIndex = 2;
            this.tabPage3_2.Text = "原始信号";
            this.tabPage3_2.UseVisualStyleBackColor = true;
            // 
            // Channel_2
            // 
            this.Channel_2.FormattingEnabled = true;
            this.Channel_2.Items.AddRange(new object[] {
            "Channel1",
            "Channel2",
            "Channel3",
            "Channel4",
            "Channel5",
            "Channel6",
            "Channel7",
            "Channel8",
            "Channel9",
            "Channel10",
            "Channel11",
            "Channel12",
            "X全选",
            "Z全选"});
            this.Channel_2.Location = new System.Drawing.Point(654, 321);
            this.Channel_2.Name = "Channel_2";
            this.Channel_2.Size = new System.Drawing.Size(79, 20);
            this.Channel_2.TabIndex = 11;
            this.Channel_2.Text = "Channel1";
            // 
            // zedGraphControl2
            // 
            this.zedGraphControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.zedGraphControl2.Location = new System.Drawing.Point(3, 2);
            this.zedGraphControl2.Name = "zedGraphControl2";
            this.zedGraphControl2.ScrollGrace = 0D;
            this.zedGraphControl2.ScrollMaxX = 0D;
            this.zedGraphControl2.ScrollMaxY = 0D;
            this.zedGraphControl2.ScrollMaxY2 = 0D;
            this.zedGraphControl2.ScrollMinX = 0D;
            this.zedGraphControl2.ScrollMinY = 0D;
            this.zedGraphControl2.ScrollMinY2 = 0D;
            this.zedGraphControl2.Size = new System.Drawing.Size(749, 338);
            this.zedGraphControl2.TabIndex = 1;
            // 
            // NoText_3
            // 
            this.NoText_3.Location = new System.Drawing.Point(75, 464);
            this.NoText_3.Name = "NoText_3";
            this.NoText_3.Size = new System.Drawing.Size(106, 21);
            this.NoText_3.TabIndex = 47;
            // 
            // FileText_3
            // 
            this.FileText_3.Location = new System.Drawing.Point(76, 430);
            this.FileText_3.Name = "FileText_3";
            this.FileText_3.Size = new System.Drawing.Size(105, 21);
            this.FileText_3.TabIndex = 46;
            // 
            // label2_2
            // 
            this.label2_2.AutoSize = true;
            this.label2_2.Location = new System.Drawing.Point(17, 433);
            this.label2_2.Name = "label2_2";
            this.label2_2.Size = new System.Drawing.Size(65, 12);
            this.label2_2.TabIndex = 45;
            this.label2_2.Text = "数据文件：";
            // 
            // label9_2
            // 
            this.label9_2.AutoSize = true;
            this.label9_2.Location = new System.Drawing.Point(15, 467);
            this.label9_2.Name = "label9_2";
            this.label9_2.Size = new System.Drawing.Size(65, 12);
            this.label9_2.TabIndex = 44;
            this.label9_2.Text = "工件编号：";
            // 
            // TLBox_3
            // 
            this.TLBox_3.Location = new System.Drawing.Point(267, 523);
            this.TLBox_3.Name = "TLBox_3";
            this.TLBox_3.Size = new System.Drawing.Size(65, 21);
            this.TLBox_3.TabIndex = 67;
            // 
            // label19_2
            // 
            this.label19_2.AutoSize = true;
            this.label19_2.Location = new System.Drawing.Point(187, 527);
            this.label19_2.Name = "label19_2";
            this.label19_2.Size = new System.Drawing.Size(89, 12);
            this.label19_2.TabIndex = 66;
            this.label19_2.Text = "管道埋深(mm)：";
            // 
            // TText_3
            // 
            this.TText_3.Location = new System.Drawing.Point(267, 464);
            this.TText_3.Name = "TText_3";
            this.TText_3.Size = new System.Drawing.Size(65, 21);
            this.TText_3.TabIndex = 65;
            // 
            // DText_3
            // 
            this.DText_3.Location = new System.Drawing.Point(267, 495);
            this.DText_3.Name = "DText_3";
            this.DText_3.Size = new System.Drawing.Size(65, 21);
            this.DText_3.TabIndex = 64;
            // 
            // DisText_3
            // 
            this.DisText_3.Location = new System.Drawing.Point(267, 430);
            this.DisText_3.Name = "DisText_3";
            this.DisText_3.Size = new System.Drawing.Size(65, 21);
            this.DisText_3.TabIndex = 63;
            // 
            // label3_2
            // 
            this.label3_2.AutoSize = true;
            this.label3_2.Location = new System.Drawing.Point(187, 468);
            this.label3_2.Name = "label3_2";
            this.label3_2.Size = new System.Drawing.Size(89, 12);
            this.label3_2.TabIndex = 62;
            this.label3_2.Text = "管道壁厚(mm)：";
            // 
            // label1_2
            // 
            this.label1_2.AutoSize = true;
            this.label1_2.Location = new System.Drawing.Point(187, 498);
            this.label1_2.Name = "label1_2";
            this.label1_2.Size = new System.Drawing.Size(89, 12);
            this.label1_2.TabIndex = 61;
            this.label1_2.Text = "管道外径(mm)：";
            // 
            // label8_2
            // 
            this.label8_2.AutoSize = true;
            this.label8_2.Location = new System.Drawing.Point(187, 433);
            this.label8_2.Name = "label8_2";
            this.label8_2.Size = new System.Drawing.Size(89, 12);
            this.label8_2.TabIndex = 60;
            this.label8_2.Text = "扫描距离(mm)：";
            // 
            // qccdtext_3
            // 
            this.qccdtext_3.Location = new System.Drawing.Point(396, 525);
            this.qccdtext_3.Name = "qccdtext_3";
            this.qccdtext_3.Size = new System.Drawing.Size(60, 21);
            this.qccdtext_3.TabIndex = 75;
            this.qccdtext_3.Text = "200";
            // 
            // label14_2
            // 
            this.label14_2.AutoSize = true;
            this.label14_2.Location = new System.Drawing.Point(340, 529);
            this.label14_2.Name = "label14_2";
            this.label14_2.Size = new System.Drawing.Size(65, 12);
            this.label14_2.TabIndex = 74;
            this.label14_2.Text = "去除长度：";
            // 
            // cdtext_3
            // 
            this.cdtext_3.Location = new System.Drawing.Point(396, 492);
            this.cdtext_3.Name = "cdtext_3";
            this.cdtext_3.Size = new System.Drawing.Size(60, 21);
            this.cdtext_3.TabIndex = 73;
            this.cdtext_3.Text = "300";
            // 
            // bfgstext_3
            // 
            this.bfgstext_3.Enabled = false;
            this.bfgstext_3.Location = new System.Drawing.Point(396, 465);
            this.bfgstext_3.Name = "bfgstext_3";
            this.bfgstext_3.Size = new System.Drawing.Size(60, 21);
            this.bfgstext_3.TabIndex = 72;
            this.bfgstext_3.Text = "2";
            // 
            // yztext_3
            // 
            this.yztext_3.Location = new System.Drawing.Point(396, 434);
            this.yztext_3.Name = "yztext_3";
            this.yztext_3.Size = new System.Drawing.Size(60, 21);
            this.yztext_3.TabIndex = 71;
            this.yztext_3.Text = "1.96";
            // 
            // label13_2
            // 
            this.label13_2.AutoSize = true;
            this.label13_2.Location = new System.Drawing.Point(340, 497);
            this.label13_2.Name = "label13_2";
            this.label13_2.Size = new System.Drawing.Size(65, 12);
            this.label13_2.TabIndex = 70;
            this.label13_2.Text = "判断长度：";
            // 
            // label12_2
            // 
            this.label12_2.AutoSize = true;
            this.label12_2.Location = new System.Drawing.Point(340, 467);
            this.label12_2.Name = "label12_2";
            this.label12_2.Size = new System.Drawing.Size(65, 12);
            this.label12_2.TabIndex = 69;
            this.label12_2.Text = "波峰个数：";
            // 
            // label11_2
            // 
            this.label11_2.AutoSize = true;
            this.label11_2.Location = new System.Drawing.Point(347, 439);
            this.label11_2.Name = "label11_2";
            this.label11_2.Size = new System.Drawing.Size(59, 12);
            this.label11_2.TabIndex = 68;
            this.label11_2.Text = "灵敏度 ：";
            // 
            // browseButton_3
            // 
            this.browseButton_3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.browseButton_3.Location = new System.Drawing.Point(462, 430);
            this.browseButton_3.Name = "browseButton_3";
            this.browseButton_3.Size = new System.Drawing.Size(66, 32);
            this.browseButton_3.TabIndex = 76;
            this.browseButton_3.Text = "预览数据";
            this.browseButton_3.UseVisualStyleBackColor = true;
            this.browseButton_3.Click += new System.EventHandler(this.browseButton_3_Click);
            // 
            // SaveButton_3
            // 
            this.SaveButton_3.Location = new System.Drawing.Point(462, 468);
            this.SaveButton_3.Name = "SaveButton_3";
            this.SaveButton_3.Size = new System.Drawing.Size(66, 32);
            this.SaveButton_3.TabIndex = 78;
            this.SaveButton_3.Text = "保存数据";
            this.SaveButton_3.UseVisualStyleBackColor = true;
            this.SaveButton_3.Click += new System.EventHandler(this.SaveButton_3_Click);
            // 
            // runButton_3
            // 
            this.runButton_3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.runButton_3.Location = new System.Drawing.Point(462, 510);
            this.runButton_3.Name = "runButton_3";
            this.runButton_3.Size = new System.Drawing.Size(66, 32);
            this.runButton_3.TabIndex = 77;
            this.runButton_3.Text = "数据处理";
            this.runButton_3.UseVisualStyleBackColor = true;
            this.runButton_3.Click += new System.EventHandler(this.runButton_3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 497);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 79;
            this.label1.Text = "已载入数据量：";
            // 
            // browsetimeBox
            // 
            this.browsetimeBox.Location = new System.Drawing.Point(106, 494);
            this.browsetimeBox.Name = "browsetimeBox";
            this.browsetimeBox.Size = new System.Drawing.Size(75, 21);
            this.browsetimeBox.TabIndex = 80;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 523);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 81;
            this.label2.Text = "已载入全长：";
            // 
            // length_all
            // 
            this.length_all.Location = new System.Drawing.Point(90, 521);
            this.length_all.Name = "length_all";
            this.length_all.Size = new System.Drawing.Size(91, 21);
            this.length_all.TabIndex = 82;
            // 
            // dealing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.length_all);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.browsetimeBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SaveButton_3);
            this.Controls.Add(this.runButton_3);
            this.Controls.Add(this.browseButton_3);
            this.Controls.Add(this.qccdtext_3);
            this.Controls.Add(this.label14_2);
            this.Controls.Add(this.cdtext_3);
            this.Controls.Add(this.bfgstext_3);
            this.Controls.Add(this.yztext_3);
            this.Controls.Add(this.label13_2);
            this.Controls.Add(this.label12_2);
            this.Controls.Add(this.label11_2);
            this.Controls.Add(this.TLBox_3);
            this.Controls.Add(this.label19_2);
            this.Controls.Add(this.TText_3);
            this.Controls.Add(this.DText_3);
            this.Controls.Add(this.DisText_3);
            this.Controls.Add(this.label3_2);
            this.Controls.Add(this.label1_2);
            this.Controls.Add(this.label8_2);
            this.Controls.Add(this.NoText_3);
            this.Controls.Add(this.FileText_3);
            this.Controls.Add(this.label2_2);
            this.Controls.Add(this.label9_2);
            this.Controls.Add(this.tabControl_2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "dealing";
            this.Text = "dealing";
            this.tabControl_2.ResumeLayout(false);
            this.tabPage1_2.ResumeLayout(false);
            this.tabPage3_2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl_2;
        private System.Windows.Forms.TabPage tabPage1_2;
        private ZedGraph.ZedGraphControl zedGraphControl1;
        private System.Windows.Forms.TabPage tabPage3_2;
        private System.Windows.Forms.ComboBox Channel_2;
        private ZedGraph.ZedGraphControl zedGraphControl2;
        private System.Windows.Forms.TextBox NoText_3;
        private System.Windows.Forms.TextBox FileText_3;
        private System.Windows.Forms.Label label2_2;
        private System.Windows.Forms.Label label9_2;
        private System.Windows.Forms.TextBox TLBox_3;
        private System.Windows.Forms.Label label19_2;
        private System.Windows.Forms.TextBox TText_3;
        private System.Windows.Forms.TextBox DText_3;
        private System.Windows.Forms.TextBox DisText_3;
        private System.Windows.Forms.Label label3_2;
        private System.Windows.Forms.Label label1_2;
        private System.Windows.Forms.Label label8_2;
        private System.Windows.Forms.TextBox qccdtext_3;
        private System.Windows.Forms.Label label14_2;
        private System.Windows.Forms.TextBox cdtext_3;
        private System.Windows.Forms.TextBox bfgstext_3;
        private System.Windows.Forms.TextBox yztext_3;
        private System.Windows.Forms.Label label13_2;
        private System.Windows.Forms.Label label12_2;
        private System.Windows.Forms.Label label11_2;
        private System.Windows.Forms.Button browseButton_3;
        private System.Windows.Forms.Button SaveButton_3;
        private System.Windows.Forms.Button runButton_3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox browsetimeBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox length_all;
        private System.Windows.Forms.ComboBox Channel;
    }
}