namespace theHostComputer
{
    partial class PointForm
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
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label17 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.battery_power = new System.Windows.Forms.Button();
            this.keyboard = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.linkButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.browseButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.runButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.MjtextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ZtextBox = new System.Windows.Forms.TextBox();
            this.PointButton = new System.Windows.Forms.Button();
            this.XtextBox = new System.Windows.Forms.TextBox();
            this.YtextBox = new System.Windows.Forms.TextBox();
            this.Ylabel = new System.Windows.Forms.Label();
            this.Xlabel = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.TLBox = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.qccdtext = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cdtext = new System.Windows.Forms.TextBox();
            this.bfgstext = new System.Windows.Forms.TextBox();
            this.yztext = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.NoText = new System.Windows.Forms.TextBox();
            this.TText = new System.Windows.Forms.TextBox();
            this.DText = new System.Windows.Forms.TextBox();
            this.RatecomboBox = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.DisText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.FileText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TimeLabel = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Location = new System.Drawing.Point(12, 12);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(760, 306);
            this.zedGraphControl1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label17);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.trackBar2);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.trackBar1);
            this.panel1.Location = new System.Drawing.Point(9, 338);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(760, 70);
            this.panel1.TabIndex = 30;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(408, 41);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(35, 12);
            this.label17.TabIndex = 5;
            this.label17.Text = "(000)";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(34, 41);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(35, 12);
            this.label15.TabIndex = 4;
            this.label15.Text = "(000)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 14F);
            this.label7.Location = new System.Drawing.Point(398, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 19);
            this.label7.TabIndex = 3;
            this.label7.Text = "增益：";
            // 
            // trackBar2
            // 
            this.trackBar2.Location = new System.Drawing.Point(461, 13);
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(270, 45);
            this.trackBar2.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 14F);
            this.label6.Location = new System.Drawing.Point(27, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 19);
            this.label6.TabIndex = 1;
            this.label6.Text = "音量：";
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(99, 13);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(270, 45);
            this.trackBar1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Location = new System.Drawing.Point(5, 414);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(775, 145);
            this.panel2.TabIndex = 31;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.battery_power);
            this.panel3.Controls.Add(this.keyboard);
            this.panel3.Controls.Add(this.exitButton);
            this.panel3.Controls.Add(this.linkButton);
            this.panel3.Controls.Add(this.startButton);
            this.panel3.Controls.Add(this.browseButton);
            this.panel3.Controls.Add(this.SaveButton);
            this.panel3.Controls.Add(this.runButton);
            this.panel3.Location = new System.Drawing.Point(604, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(166, 137);
            this.panel3.TabIndex = 19;
            // 
            // battery_power
            // 
            this.battery_power.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.battery_power.Location = new System.Drawing.Point(44, 105);
            this.battery_power.Name = "battery_power";
            this.battery_power.Size = new System.Drawing.Size(41, 27);
            this.battery_power.TabIndex = 25;
            this.battery_power.Text = "电池";
            this.battery_power.UseVisualStyleBackColor = true;
            // 
            // keyboard
            // 
            this.keyboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.keyboard.Font = new System.Drawing.Font("宋体", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.keyboard.Location = new System.Drawing.Point(3, 105);
            this.keyboard.Name = "keyboard";
            this.keyboard.Size = new System.Drawing.Size(41, 27);
            this.keyboard.TabIndex = 24;
            this.keyboard.Text = "软键盘";
            this.keyboard.UseVisualStyleBackColor = true;
            // 
            // exitButton
            // 
            this.exitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.exitButton.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.exitButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.exitButton.Location = new System.Drawing.Point(89, 105);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(72, 27);
            this.exitButton.TabIndex = 23;
            this.exitButton.Text = "退出";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // linkButton
            // 
            this.linkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkButton.Location = new System.Drawing.Point(89, 3);
            this.linkButton.Name = "linkButton";
            this.linkButton.Size = new System.Drawing.Size(72, 46);
            this.linkButton.TabIndex = 21;
            this.linkButton.Text = "连接";
            this.linkButton.UseVisualStyleBackColor = true;
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.startButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.startButton.Location = new System.Drawing.Point(89, 55);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(72, 46);
            this.startButton.TabIndex = 22;
            this.startButton.Text = "开始";
            this.startButton.UseVisualStyleBackColor = true;
            // 
            // browseButton
            // 
            this.browseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.browseButton.Location = new System.Drawing.Point(3, 3);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(80, 32);
            this.browseButton.TabIndex = 18;
            this.browseButton.Text = "预览数据";
            this.browseButton.UseVisualStyleBackColor = true;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(3, 38);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(82, 32);
            this.SaveButton.TabIndex = 19;
            this.SaveButton.Text = "保存数据";
            this.SaveButton.UseVisualStyleBackColor = true;
            // 
            // runButton
            // 
            this.runButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.runButton.Location = new System.Drawing.Point(3, 72);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(82, 32);
            this.runButton.TabIndex = 20;
            this.runButton.Text = "数据处理";
            this.runButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.MjtextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.ZtextBox);
            this.groupBox1.Controls.Add(this.PointButton);
            this.groupBox1.Controls.Add(this.XtextBox);
            this.groupBox1.Controls.Add(this.YtextBox);
            this.groupBox1.Controls.Add(this.Ylabel);
            this.groupBox1.Controls.Add(this.Xlabel);
            this.groupBox1.ForeColor = System.Drawing.Color.Red;
            this.groupBox1.Location = new System.Drawing.Point(447, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(152, 119);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "缺陷参数";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 26;
            this.label1.Text = "损伤面积当量：";
            // 
            // MjtextBox
            // 
            this.MjtextBox.Location = new System.Drawing.Point(95, 92);
            this.MjtextBox.Name = "MjtextBox";
            this.MjtextBox.Size = new System.Drawing.Size(51, 21);
            this.MjtextBox.TabIndex = 25;
            this.MjtextBox.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 24;
            this.label2.Text = "损伤深度当量：";
            // 
            // ZtextBox
            // 
            this.ZtextBox.Enabled = false;
            this.ZtextBox.Location = new System.Drawing.Point(95, 64);
            this.ZtextBox.Name = "ZtextBox";
            this.ZtextBox.Size = new System.Drawing.Size(51, 21);
            this.ZtextBox.TabIndex = 23;
            this.ZtextBox.TabStop = false;
            // 
            // PointButton
            // 
            this.PointButton.Enabled = false;
            this.PointButton.ForeColor = System.Drawing.Color.Black;
            this.PointButton.Location = new System.Drawing.Point(2, 23);
            this.PointButton.Name = "PointButton";
            this.PointButton.Size = new System.Drawing.Size(47, 33);
            this.PointButton.TabIndex = 18;
            this.PointButton.Text = "缺陷定位";
            this.PointButton.UseVisualStyleBackColor = true;
            // 
            // XtextBox
            // 
            this.XtextBox.Location = new System.Drawing.Point(85, 12);
            this.XtextBox.Name = "XtextBox";
            this.XtextBox.Size = new System.Drawing.Size(61, 21);
            this.XtextBox.TabIndex = 15;
            this.XtextBox.TabStop = false;
            // 
            // YtextBox
            // 
            this.YtextBox.Location = new System.Drawing.Point(85, 38);
            this.YtextBox.Name = "YtextBox";
            this.YtextBox.Size = new System.Drawing.Size(61, 21);
            this.YtextBox.TabIndex = 16;
            this.YtextBox.TabStop = false;
            // 
            // Ylabel
            // 
            this.Ylabel.AutoSize = true;
            this.Ylabel.Location = new System.Drawing.Point(50, 43);
            this.Ylabel.Name = "Ylabel";
            this.Ylabel.Size = new System.Drawing.Size(41, 12);
            this.Ylabel.TabIndex = 17;
            this.Ylabel.Text = "Y ↑：";
            // 
            // Xlabel
            // 
            this.Xlabel.AutoSize = true;
            this.Xlabel.Location = new System.Drawing.Point(51, 15);
            this.Xlabel.Name = "Xlabel";
            this.Xlabel.Size = new System.Drawing.Size(41, 12);
            this.Xlabel.TabIndex = 18;
            this.Xlabel.Text = "X →：";
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.TLBox);
            this.panel4.Controls.Add(this.label19);
            this.panel4.Controls.Add(this.label16);
            this.panel4.Controls.Add(this.qccdtext);
            this.panel4.Controls.Add(this.label14);
            this.panel4.Controls.Add(this.cdtext);
            this.panel4.Controls.Add(this.bfgstext);
            this.panel4.Controls.Add(this.yztext);
            this.panel4.Controls.Add(this.label13);
            this.panel4.Controls.Add(this.label12);
            this.panel4.Controls.Add(this.label11);
            this.panel4.Controls.Add(this.NoText);
            this.panel4.Controls.Add(this.TText);
            this.panel4.Controls.Add(this.DText);
            this.panel4.Controls.Add(this.RatecomboBox);
            this.panel4.Controls.Add(this.label20);
            this.panel4.Controls.Add(this.DisText);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.FileText);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.TimeLabel);
            this.panel4.Controls.Add(this.label10);
            this.panel4.Controls.Add(this.label9);
            this.panel4.Controls.Add(this.label8);
            this.panel4.Location = new System.Drawing.Point(3, 7);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(438, 126);
            this.panel4.TabIndex = 3;
            // 
            // TLBox
            // 
            this.TLBox.Location = new System.Drawing.Point(252, 100);
            this.TLBox.Name = "TLBox";
            this.TLBox.Size = new System.Drawing.Size(65, 21);
            this.TLBox.TabIndex = 5;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(172, 104);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(89, 12);
            this.label19.TabIndex = 58;
            this.label19.Text = "管道埋深(mm)：";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(561, 11);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(0, 12);
            this.label16.TabIndex = 55;
            // 
            // qccdtext
            // 
            this.qccdtext.Location = new System.Drawing.Point(374, 100);
            this.qccdtext.Name = "qccdtext";
            this.qccdtext.Size = new System.Drawing.Size(60, 21);
            this.qccdtext.TabIndex = 9;
            this.qccdtext.Text = "200";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(318, 104);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 12);
            this.label14.TabIndex = 53;
            this.label14.Text = "去除长度：";
            // 
            // cdtext
            // 
            this.cdtext.Location = new System.Drawing.Point(374, 67);
            this.cdtext.Name = "cdtext";
            this.cdtext.Size = new System.Drawing.Size(60, 21);
            this.cdtext.TabIndex = 8;
            this.cdtext.Text = "10";
            // 
            // bfgstext
            // 
            this.bfgstext.Enabled = false;
            this.bfgstext.Location = new System.Drawing.Point(374, 40);
            this.bfgstext.Name = "bfgstext";
            this.bfgstext.Size = new System.Drawing.Size(60, 21);
            this.bfgstext.TabIndex = 7;
            this.bfgstext.Text = "2";
            // 
            // yztext
            // 
            this.yztext.Location = new System.Drawing.Point(374, 9);
            this.yztext.Name = "yztext";
            this.yztext.Size = new System.Drawing.Size(60, 21);
            this.yztext.TabIndex = 6;
            this.yztext.Text = "1.96";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(318, 72);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 12);
            this.label13.TabIndex = 49;
            this.label13.Text = "判断次数：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(318, 42);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 12);
            this.label12.TabIndex = 48;
            this.label12.Text = "波峰个数：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(325, 14);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(59, 12);
            this.label11.TabIndex = 47;
            this.label11.Text = "灵敏度 ：";
            // 
            // NoText
            // 
            this.NoText.Location = new System.Drawing.Point(61, 41);
            this.NoText.Name = "NoText";
            this.NoText.Size = new System.Drawing.Size(106, 21);
            this.NoText.TabIndex = 1;
            // 
            // TText
            // 
            this.TText.Location = new System.Drawing.Point(252, 41);
            this.TText.Name = "TText";
            this.TText.Size = new System.Drawing.Size(65, 21);
            this.TText.TabIndex = 3;
            // 
            // DText
            // 
            this.DText.Location = new System.Drawing.Point(252, 72);
            this.DText.Name = "DText";
            this.DText.Size = new System.Drawing.Size(65, 21);
            this.DText.TabIndex = 4;
            // 
            // RatecomboBox
            // 
            this.RatecomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RatecomboBox.Enabled = false;
            this.RatecomboBox.FormattingEnabled = true;
            this.RatecomboBox.Items.AddRange(new object[] {
            "6.25",
            "12.5",
            "25",
            "50",
            "100",
            "200",
            "400",
            "800",
            "1000"});
            this.RatecomboBox.Location = new System.Drawing.Point(63, 75);
            this.RatecomboBox.Name = "RatecomboBox";
            this.RatecomboBox.Size = new System.Drawing.Size(65, 20);
            this.RatecomboBox.TabIndex = 40;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(3, 78);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(65, 12);
            this.label20.TabIndex = 39;
            this.label20.Text = "采样频率：";
            // 
            // DisText
            // 
            this.DisText.Location = new System.Drawing.Point(252, 7);
            this.DisText.Name = "DisText";
            this.DisText.Size = new System.Drawing.Size(65, 21);
            this.DisText.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(172, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 23;
            this.label3.Text = "管道壁厚(mm)：";
            // 
            // FileText
            // 
            this.FileText.Location = new System.Drawing.Point(62, 7);
            this.FileText.Name = "FileText";
            this.FileText.Size = new System.Drawing.Size(105, 21);
            this.FileText.TabIndex = 22;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 21;
            this.label4.Text = "数据文件：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(172, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 19;
            this.label5.Text = "管道外径(mm)：";
            // 
            // TimeLabel
            // 
            this.TimeLabel.AutoSize = true;
            this.TimeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.TimeLabel.Location = new System.Drawing.Point(57, 106);
            this.TimeLabel.Name = "TimeLabel";
            this.TimeLabel.Size = new System.Drawing.Size(59, 12);
            this.TimeLabel.TabIndex = 15;
            this.TimeLabel.Text = "Timeofnow";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 106);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 14;
            this.label10.Text = "检测时间：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1, 44);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 12;
            this.label9.Text = "工件编号：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(172, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 12);
            this.label8.TabIndex = 10;
            this.label8.Text = "扫描距离(mm)：";
            // 
            // PointForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.zedGraphControl1);
            this.Name = "PointForm";
            this.Text = "频谱分析";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl zedGraphControl1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button battery_power;
        private System.Windows.Forms.Button keyboard;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button linkButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button runButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox MjtextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ZtextBox;
        private System.Windows.Forms.Button PointButton;
        private System.Windows.Forms.TextBox XtextBox;
        private System.Windows.Forms.TextBox YtextBox;
        private System.Windows.Forms.Label Ylabel;
        private System.Windows.Forms.Label Xlabel;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox TLBox;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox qccdtext;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox cdtext;
        private System.Windows.Forms.TextBox bfgstext;
        private System.Windows.Forms.TextBox yztext;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox NoText;
        private System.Windows.Forms.TextBox TText;
        private System.Windows.Forms.TextBox DText;
        private System.Windows.Forms.ComboBox RatecomboBox;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox DisText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox FileText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label TimeLabel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
    }
}