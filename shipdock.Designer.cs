namespace shipdock
{
    partial class shipdock
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
            this.Log = new System.Windows.Forms.RichTextBox();
            this.btnLog = new System.Windows.Forms.Button();
            this.tbLogPath = new System.Windows.Forms.TextBox();
            this.IsLog = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbDataPath = new System.Windows.Forms.TextBox();
            this.btnData = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbUserPort = new System.Windows.Forms.ComboBox();
            this.btnStartLadar = new System.Windows.Forms.Button();
            this.btnSendParam = new System.Windows.Forms.Button();
            this.btnSetParam = new System.Windows.Forms.Button();
            this.btnStopLadar = new System.Windows.Forms.Button();
            this.PortGroups = new System.Windows.Forms.GroupBox();
            this.btnRefreshPort = new System.Windows.Forms.Button();
            this.btnClosePort = new System.Windows.Forms.Button();
            this.cbDataBaudRate = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cbDataPort = new System.Windows.Forms.ComboBox();
            this.btnConnectPort = new System.Windows.Forms.Button();
            this.cbUserBaudRate = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btncfgPath = new System.Windows.Forms.Button();
            this.tbCfgPath = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.pbTrajectory = new System.Windows.Forms.PictureBox();
            this.ParamsGroup = new System.Windows.Forms.GroupBox();
            this.ProgramStart = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.PortGroups.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTrajectory)).BeginInit();
            this.ParamsGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // Log
            // 
            this.Log.Location = new System.Drawing.Point(24, 387);
            this.Log.Name = "Log";
            this.Log.ReadOnly = true;
            this.Log.Size = new System.Drawing.Size(711, 402);
            this.Log.TabIndex = 4;
            this.Log.Text = "";
            // 
            // btnLog
            // 
            this.btnLog.AutoSize = true;
            this.btnLog.Enabled = false;
            this.btnLog.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnLog.Location = new System.Drawing.Point(420, 118);
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new System.Drawing.Size(90, 40);
            this.btnLog.TabIndex = 7;
            this.btnLog.Text = "浏览";
            this.btnLog.UseVisualStyleBackColor = true;
            this.btnLog.Click += new System.EventHandler(this.btnLog_Click);
            // 
            // tbLogPath
            // 
            this.tbLogPath.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.tbLogPath.Enabled = false;
            this.tbLogPath.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tbLogPath.Location = new System.Drawing.Point(130, 123);
            this.tbLogPath.Name = "tbLogPath";
            this.tbLogPath.Size = new System.Drawing.Size(281, 30);
            this.tbLogPath.TabIndex = 6;
            this.tbLogPath.TextChanged += new System.EventHandler(this.tbLogPath_TextChanged);
            // 
            // IsLog
            // 
            this.IsLog.AutoSize = true;
            this.IsLog.Checked = true;
            this.IsLog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IsLog.ForeColor = System.Drawing.SystemColors.GrayText;
            this.IsLog.Location = new System.Drawing.Point(6, 126);
            this.IsLog.Name = "IsLog";
            this.IsLog.Size = new System.Drawing.Size(108, 28);
            this.IsLog.TabIndex = 8;
            this.IsLog.Text = "日志系统";
            this.IsLog.UseVisualStyleBackColor = true;
            this.IsLog.CheckedChanged += new System.EventHandler(this.IsLog_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 24);
            this.label3.TabIndex = 10;
            this.label3.Text = "数据文件";
            // 
            // tbDataPath
            // 
            this.tbDataPath.Location = new System.Drawing.Point(130, 21);
            this.tbDataPath.Name = "tbDataPath";
            this.tbDataPath.Size = new System.Drawing.Size(277, 30);
            this.tbDataPath.TabIndex = 11;
            this.tbDataPath.TextChanged += new System.EventHandler(this.tbDataPath_TextChanged);
            // 
            // btnData
            // 
            this.btnData.AutoSize = true;
            this.btnData.Location = new System.Drawing.Point(420, 17);
            this.btnData.Name = "btnData";
            this.btnData.Size = new System.Drawing.Size(90, 40);
            this.btnData.TabIndex = 12;
            this.btnData.Text = "浏览";
            this.btnData.UseVisualStyleBackColor = true;
            this.btnData.Click += new System.EventHandler(this.btnData_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 24);
            this.label2.TabIndex = 13;
            this.label2.Text = "User端口";
            // 
            // cbUserPort
            // 
            this.cbUserPort.FormattingEnabled = true;
            this.cbUserPort.Location = new System.Drawing.Point(105, 27);
            this.cbUserPort.Name = "cbUserPort";
            this.cbUserPort.Size = new System.Drawing.Size(103, 32);
            this.cbUserPort.TabIndex = 14;
            // 
            // btnStartLadar
            // 
            this.btnStartLadar.Location = new System.Drawing.Point(17, 48);
            this.btnStartLadar.Name = "btnStartLadar";
            this.btnStartLadar.Size = new System.Drawing.Size(112, 38);
            this.btnStartLadar.TabIndex = 15;
            this.btnStartLadar.Text = "程序启动";
            this.btnStartLadar.UseVisualStyleBackColor = true;
            this.btnStartLadar.Click += new System.EventHandler(this.btnStartLadar_Click);
            // 
            // btnSendParam
            // 
            this.btnSendParam.AutoSize = true;
            this.btnSendParam.Location = new System.Drawing.Point(31, 113);
            this.btnSendParam.Name = "btnSendParam";
            this.btnSendParam.Size = new System.Drawing.Size(145, 48);
            this.btnSendParam.TabIndex = 16;
            this.btnSendParam.Text = "传输参数";
            this.btnSendParam.UseVisualStyleBackColor = true;
            this.btnSendParam.Click += new System.EventHandler(this.btnSendParam_Click);
            // 
            // btnSetParam
            // 
            this.btnSetParam.AutoSize = true;
            this.btnSetParam.Location = new System.Drawing.Point(31, 43);
            this.btnSetParam.Name = "btnSetParam";
            this.btnSetParam.Size = new System.Drawing.Size(145, 48);
            this.btnSetParam.TabIndex = 17;
            this.btnSetParam.Text = "设置参数";
            this.btnSetParam.UseVisualStyleBackColor = true;
            this.btnSetParam.Click += new System.EventHandler(this.btnSetParam_Click);
            // 
            // btnStopLadar
            // 
            this.btnStopLadar.AutoSize = true;
            this.btnStopLadar.Location = new System.Drawing.Point(17, 117);
            this.btnStopLadar.Name = "btnStopLadar";
            this.btnStopLadar.Size = new System.Drawing.Size(112, 36);
            this.btnStopLadar.TabIndex = 18;
            this.btnStopLadar.Text = "程序停止";
            this.btnStopLadar.UseVisualStyleBackColor = true;
            this.btnStopLadar.Click += new System.EventHandler(this.btnStopLadar_Click);
            // 
            // PortGroups
            // 
            this.PortGroups.Controls.Add(this.btnRefreshPort);
            this.PortGroups.Controls.Add(this.btnClosePort);
            this.PortGroups.Controls.Add(this.cbDataBaudRate);
            this.PortGroups.Controls.Add(this.label1);
            this.PortGroups.Controls.Add(this.label6);
            this.PortGroups.Controls.Add(this.cbDataPort);
            this.PortGroups.Controls.Add(this.btnConnectPort);
            this.PortGroups.Controls.Add(this.cbUserBaudRate);
            this.PortGroups.Controls.Add(this.label4);
            this.PortGroups.Controls.Add(this.label2);
            this.PortGroups.Controls.Add(this.cbUserPort);
            this.PortGroups.Location = new System.Drawing.Point(24, 28);
            this.PortGroups.Name = "PortGroups";
            this.PortGroups.Size = new System.Drawing.Size(449, 174);
            this.PortGroups.TabIndex = 19;
            this.PortGroups.TabStop = false;
            this.PortGroups.Text = "端口连接";
            // 
            // btnRefreshPort
            // 
            this.btnRefreshPort.Location = new System.Drawing.Point(46, 119);
            this.btnRefreshPort.Name = "btnRefreshPort";
            this.btnRefreshPort.Size = new System.Drawing.Size(112, 42);
            this.btnRefreshPort.TabIndex = 23;
            this.btnRefreshPort.Text = "刷新";
            this.btnRefreshPort.UseVisualStyleBackColor = true;
            this.btnRefreshPort.Click += new System.EventHandler(this.btnRefreshPort_Click);
            // 
            // btnClosePort
            // 
            this.btnClosePort.Location = new System.Drawing.Point(310, 119);
            this.btnClosePort.Name = "btnClosePort";
            this.btnClosePort.Size = new System.Drawing.Size(112, 42);
            this.btnClosePort.TabIndex = 22;
            this.btnClosePort.Text = "关闭";
            this.btnClosePort.UseVisualStyleBackColor = true;
            this.btnClosePort.Click += new System.EventHandler(this.btnClosePort_Click);
            // 
            // cbDataBaudRate
            // 
            this.cbDataBaudRate.FormattingEnabled = true;
            this.cbDataBaudRate.Items.AddRange(new object[] {
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200",
            "921600"});
            this.cbDataBaudRate.Location = new System.Drawing.Point(328, 73);
            this.cbDataBaudRate.Name = "cbDataBaudRate";
            this.cbDataBaudRate.Size = new System.Drawing.Size(103, 32);
            this.cbDataBaudRate.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(253, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 24);
            this.label1.TabIndex = 20;
            this.label1.Text = "波特率";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(234, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 24);
            this.label6.TabIndex = 18;
            this.label6.Text = "数据端口";
            // 
            // cbDataPort
            // 
            this.cbDataPort.FormattingEnabled = true;
            this.cbDataPort.Location = new System.Drawing.Point(328, 27);
            this.cbDataPort.Name = "cbDataPort";
            this.cbDataPort.Size = new System.Drawing.Size(103, 32);
            this.cbDataPort.TabIndex = 19;
            // 
            // btnConnectPort
            // 
            this.btnConnectPort.Location = new System.Drawing.Point(176, 119);
            this.btnConnectPort.Name = "btnConnectPort";
            this.btnConnectPort.Size = new System.Drawing.Size(112, 42);
            this.btnConnectPort.TabIndex = 17;
            this.btnConnectPort.Text = "连接";
            this.btnConnectPort.UseVisualStyleBackColor = true;
            this.btnConnectPort.Click += new System.EventHandler(this.btnConnectPort_Click);
            // 
            // cbUserBaudRate
            // 
            this.cbUserBaudRate.FormattingEnabled = true;
            this.cbUserBaudRate.Items.AddRange(new object[] {
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200",
            "921600"});
            this.cbUserBaudRate.Location = new System.Drawing.Point(105, 73);
            this.cbUserBaudRate.Name = "cbUserBaudRate";
            this.cbUserBaudRate.Size = new System.Drawing.Size(103, 32);
            this.cbUserBaudRate.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 24);
            this.label4.TabIndex = 15;
            this.label4.Text = "波特率";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnStartLadar);
            this.groupBox1.Controls.Add(this.btnStopLadar);
            this.groupBox1.Location = new System.Drawing.Point(24, 208);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(157, 174);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "程序控制";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btncfgPath);
            this.groupBox2.Controls.Add(this.tbCfgPath);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.IsLog);
            this.groupBox2.Controls.Add(this.btnData);
            this.groupBox2.Controls.Add(this.btnLog);
            this.groupBox2.Controls.Add(this.tbDataPath);
            this.groupBox2.Controls.Add(this.tbLogPath);
            this.groupBox2.Location = new System.Drawing.Point(200, 208);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(534, 174);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "文件";
            // 
            // btncfgPath
            // 
            this.btncfgPath.AutoSize = true;
            this.btncfgPath.Location = new System.Drawing.Point(420, 67);
            this.btncfgPath.Name = "btncfgPath";
            this.btncfgPath.Size = new System.Drawing.Size(90, 40);
            this.btncfgPath.TabIndex = 15;
            this.btncfgPath.Text = "浏览";
            this.btncfgPath.UseVisualStyleBackColor = true;
            this.btncfgPath.Click += new System.EventHandler(this.btncfgPath_Click);
            // 
            // tbCfgPath
            // 
            this.tbCfgPath.Location = new System.Drawing.Point(134, 72);
            this.tbCfgPath.Name = "tbCfgPath";
            this.tbCfgPath.Size = new System.Drawing.Size(277, 30);
            this.tbCfgPath.TabIndex = 14;
            this.tbCfgPath.TextChanged += new System.EventHandler(this.tbCfgPath_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(27, 76);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 24);
            this.label7.TabIndex = 13;
            this.label7.Text = "参数文件";
            // 
            // pbTrajectory
            // 
            this.pbTrajectory.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pbTrajectory.Location = new System.Drawing.Point(761, 92);
            this.pbTrajectory.Name = "pbTrajectory";
            this.pbTrajectory.Size = new System.Drawing.Size(644, 576);
            this.pbTrajectory.TabIndex = 23;
            this.pbTrajectory.TabStop = false;
            // 
            // ParamsGroup
            // 
            this.ParamsGroup.Controls.Add(this.btnSetParam);
            this.ParamsGroup.Controls.Add(this.btnSendParam);
            this.ParamsGroup.Location = new System.Drawing.Point(534, 28);
            this.ParamsGroup.Name = "ParamsGroup";
            this.ParamsGroup.Size = new System.Drawing.Size(193, 174);
            this.ParamsGroup.TabIndex = 22;
            this.ParamsGroup.TabStop = false;
            this.ParamsGroup.Text = "参数";
            // 
            // ProgramStart
            // 
            this.ProgramStart.Location = new System.Drawing.Point(761, 707);
            this.ProgramStart.Name = "ProgramStart";
            this.ProgramStart.ReadOnly = true;
            this.ProgramStart.Size = new System.Drawing.Size(642, 30);
            this.ProgramStart.TabIndex = 24;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei UI", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(916, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(289, 67);
            this.label5.TabIndex = 25;
            this.label5.Text = "浮体轨迹图";
            // 
            // shipdock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1438, 788);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ProgramStart);
            this.Controls.Add(this.pbTrajectory);
            this.Controls.Add(this.ParamsGroup);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.PortGroups);
            this.Controls.Add(this.Log);
            this.Name = "shipdock";
            this.Text = "浮体检测程序";
            this.Load += new System.EventHandler(this.shipdock_Load);
            this.FormClosing += shipdock_Close;
            this.PortGroups.ResumeLayout(false);
            this.PortGroups.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTrajectory)).EndInit();
            this.ParamsGroup.ResumeLayout(false);
            this.ParamsGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private RichTextBox Log;
        private Button btnLog;
        private TextBox tbLogPath;
        private CheckBox IsLog;
        private Label label3;
        private TextBox tbDataPath;
        private Button btnData;
        private Label label2;
        private ComboBox cbUserPort;
        private Button btnStartLadar;
        private Button btnSendParam;
        private Button btnSetParam;
        private Button btnStopLadar;
        private GroupBox PortGroups;
        private Label label4;
        private Button btnConnectPort;
        private ComboBox cbUserBaudRate;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox ParamsGroup;
        private PictureBox pbTrajectory;
        private TextBox ProgramStart;
        private Label label5;
        private ComboBox cbDataBaudRate;
        private Label label1;
        private Label label6;
        private ComboBox cbDataPort;
        private Button btnClosePort;
        private Button btncfgPath;
        private TextBox tbCfgPath;
        private Label label7;
        private Button btnRefreshPort;
    }
}