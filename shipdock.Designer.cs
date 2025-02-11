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
            Log = new RichTextBox();
            btnLog = new Button();
            logPath = new TextBox();
            IsLog = new CheckBox();
            label3 = new Label();
            DataPath = new TextBox();
            btnData = new Button();
            label2 = new Label();
            cbUserPort = new ComboBox();
            btnStartLadar = new Button();
            btnSendParam = new Button();
            btnSetParam = new Button();
            btnStopLadar = new Button();
            PortGroups = new GroupBox();
            cbDataBaudRate = new ComboBox();
            label1 = new Label();
            label6 = new Label();
            cbDataPort = new ComboBox();
            btnConnectPort = new Button();
            cbUserBaudRate = new ComboBox();
            label4 = new Label();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            pbTrajectory = new PictureBox();
            ParamsGroup = new GroupBox();
            ProgramStart = new TextBox();
            label5 = new Label();
            btnClosePort = new Button();
            PortGroups.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbTrajectory).BeginInit();
            ParamsGroup.SuspendLayout();
            SuspendLayout();
            // 
            // Log
            // 
            Log.Location = new Point(15, 274);
            Log.Margin = new Padding(2);
            Log.Name = "Log";
            Log.ReadOnly = true;
            Log.Size = new Size(454, 286);
            Log.TabIndex = 4;
            Log.Text = "";
            // 
            // btnLog
            // 
            btnLog.AutoSize = true;
            btnLog.Enabled = false;
            btnLog.ForeColor = SystemColors.ControlText;
            btnLog.Location = new Point(267, 81);
            btnLog.Margin = new Padding(2);
            btnLog.Name = "btnLog";
            btnLog.Size = new Size(69, 27);
            btnLog.TabIndex = 7;
            btnLog.Text = "浏览";
            btnLog.UseVisualStyleBackColor = true;
            btnLog.Click += btnLog_Click;
            // 
            // logPath
            // 
            logPath.BackColor = SystemColors.ScrollBar;
            logPath.Enabled = false;
            logPath.ForeColor = SystemColors.ControlText;
            logPath.Location = new Point(77, 83);
            logPath.Margin = new Padding(2);
            logPath.Name = "logPath";
            logPath.Size = new Size(184, 23);
            logPath.TabIndex = 6;
            logPath.TextChanged += logPath_TextChanged;
            // 
            // IsLog
            // 
            IsLog.AutoSize = true;
            IsLog.ForeColor = SystemColors.GrayText;
            IsLog.Location = new Point(4, 81);
            IsLog.Margin = new Padding(2);
            IsLog.Name = "IsLog";
            IsLog.Size = new Size(75, 21);
            IsLog.TabIndex = 8;
            IsLog.Text = "日志系统";
            IsLog.UseVisualStyleBackColor = true;
            IsLog.CheckedChanged += IsLog_CheckedChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(17, 34);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(56, 17);
            label3.TabIndex = 10;
            label3.Text = "存储路径";
            // 
            // DataPath
            // 
            DataPath.Location = new Point(77, 31);
            DataPath.Margin = new Padding(2);
            DataPath.Name = "DataPath";
            DataPath.Size = new Size(184, 23);
            DataPath.TabIndex = 11;
            DataPath.TextChanged += DataPath_TextChanged;
            // 
            // btnData
            // 
            btnData.AutoSize = true;
            btnData.Location = new Point(267, 29);
            btnData.Margin = new Padding(2);
            btnData.Name = "btnData";
            btnData.Size = new Size(69, 27);
            btnData.TabIndex = 12;
            btnData.Text = "浏览";
            btnData.UseVisualStyleBackColor = true;
            btnData.Click += btnData_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(4, 22);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(59, 17);
            label2.TabIndex = 13;
            label2.Text = "User端口";
            // 
            // cbUserPort
            // 
            cbUserPort.FormattingEnabled = true;
            cbUserPort.Location = new Point(67, 19);
            cbUserPort.Margin = new Padding(2);
            cbUserPort.Name = "cbUserPort";
            cbUserPort.Size = new Size(67, 25);
            cbUserPort.TabIndex = 14;
            // 
            // btnStartLadar
            // 
            btnStartLadar.Location = new Point(11, 34);
            btnStartLadar.Margin = new Padding(2);
            btnStartLadar.Name = "btnStartLadar";
            btnStartLadar.Size = new Size(71, 27);
            btnStartLadar.TabIndex = 15;
            btnStartLadar.Text = "程序启动";
            btnStartLadar.UseVisualStyleBackColor = true;
            // 
            // btnSendParam
            // 
            btnSendParam.AutoSize = true;
            btnSendParam.Location = new Point(29, 86);
            btnSendParam.Margin = new Padding(2);
            btnSendParam.Name = "btnSendParam";
            btnSendParam.Size = new Size(69, 27);
            btnSendParam.TabIndex = 16;
            btnSendParam.Text = "传输参数";
            btnSendParam.UseVisualStyleBackColor = true;
            btnSendParam.Click += btnSendParam_Click;
            // 
            // btnSetParam
            // 
            btnSetParam.AutoSize = true;
            btnSetParam.Location = new Point(29, 39);
            btnSetParam.Margin = new Padding(2);
            btnSetParam.Name = "btnSetParam";
            btnSetParam.Size = new Size(69, 27);
            btnSetParam.TabIndex = 17;
            btnSetParam.Text = "设置参数";
            btnSetParam.UseVisualStyleBackColor = true;
            btnSetParam.Click += btnSetParam_Click;
            // 
            // btnStopLadar
            // 
            btnStopLadar.AutoSize = true;
            btnStopLadar.Location = new Point(11, 81);
            btnStopLadar.Margin = new Padding(2);
            btnStopLadar.Name = "btnStopLadar";
            btnStopLadar.Size = new Size(71, 27);
            btnStopLadar.TabIndex = 18;
            btnStopLadar.Text = "程序停止";
            btnStopLadar.UseVisualStyleBackColor = true;
            // 
            // PortGroups
            // 
            PortGroups.Controls.Add(btnClosePort);
            PortGroups.Controls.Add(cbDataBaudRate);
            PortGroups.Controls.Add(label1);
            PortGroups.Controls.Add(label6);
            PortGroups.Controls.Add(cbDataPort);
            PortGroups.Controls.Add(btnConnectPort);
            PortGroups.Controls.Add(cbUserBaudRate);
            PortGroups.Controls.Add(label4);
            PortGroups.Controls.Add(label2);
            PortGroups.Controls.Add(cbUserPort);
            PortGroups.Location = new Point(15, 20);
            PortGroups.Margin = new Padding(2);
            PortGroups.Name = "PortGroups";
            PortGroups.Padding = new Padding(2);
            PortGroups.Size = new Size(286, 123);
            PortGroups.TabIndex = 19;
            PortGroups.TabStop = false;
            PortGroups.Text = "端口连接";
            // 
            // cbDataBaudRate
            // 
            cbDataBaudRate.FormattingEnabled = true;
            cbDataBaudRate.Items.AddRange(new object[] { "4800", "9600", "19200", "38400", "57600", "115200", "921600" });
            cbDataBaudRate.Location = new Point(209, 52);
            cbDataBaudRate.Margin = new Padding(2);
            cbDataBaudRate.Name = "cbDataBaudRate";
            cbDataBaudRate.Size = new Size(67, 25);
            cbDataBaudRate.TabIndex = 21;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(161, 55);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(44, 17);
            label1.TabIndex = 20;
            label1.Text = "波特率";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(149, 22);
            label6.Margin = new Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new Size(56, 17);
            label6.TabIndex = 18;
            label6.Text = "数据端口";
            // 
            // cbDataPort
            // 
            cbDataPort.FormattingEnabled = true;
            cbDataPort.Location = new Point(209, 19);
            cbDataPort.Margin = new Padding(2);
            cbDataPort.Name = "cbDataPort";
            cbDataPort.Size = new Size(67, 25);
            cbDataPort.TabIndex = 19;
            // 
            // btnConnectPort
            // 
            btnConnectPort.Location = new Point(67, 84);
            btnConnectPort.Margin = new Padding(2);
            btnConnectPort.Name = "btnConnectPort";
            btnConnectPort.Size = new Size(71, 30);
            btnConnectPort.TabIndex = 17;
            btnConnectPort.Text = "连接";
            btnConnectPort.UseVisualStyleBackColor = true;
            btnConnectPort.Click += btnConnectPort_Click;
            // 
            // cbUserBaudRate
            // 
            cbUserBaudRate.FormattingEnabled = true;
            cbUserBaudRate.Items.AddRange(new object[] { "4800", "9600", "19200", "38400", "57600", "115200", "921600" });
            cbUserBaudRate.Location = new Point(67, 52);
            cbUserBaudRate.Margin = new Padding(2);
            cbUserBaudRate.Name = "cbUserBaudRate";
            cbUserBaudRate.Size = new Size(67, 25);
            cbUserBaudRate.TabIndex = 16;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(19, 55);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(44, 17);
            label4.TabIndex = 15;
            label4.Text = "波特率";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnStartLadar);
            groupBox1.Controls.Add(btnStopLadar);
            groupBox1.Location = new Point(15, 147);
            groupBox1.Margin = new Padding(2);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(2);
            groupBox1.Size = new Size(100, 123);
            groupBox1.TabIndex = 20;
            groupBox1.TabStop = false;
            groupBox1.Text = "程序控制";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(IsLog);
            groupBox2.Controls.Add(btnData);
            groupBox2.Controls.Add(btnLog);
            groupBox2.Controls.Add(DataPath);
            groupBox2.Controls.Add(logPath);
            groupBox2.Location = new Point(127, 147);
            groupBox2.Margin = new Padding(2);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(2);
            groupBox2.Size = new Size(340, 123);
            groupBox2.TabIndex = 21;
            groupBox2.TabStop = false;
            groupBox2.Text = "文件";
            // 
            // pbTrajectory
            // 
            pbTrajectory.BackColor = SystemColors.ButtonHighlight;
            pbTrajectory.Location = new Point(484, 65);
            pbTrajectory.Margin = new Padding(2);
            pbTrajectory.Name = "pbTrajectory";
            pbTrajectory.Size = new Size(410, 408);
            pbTrajectory.TabIndex = 23;
            pbTrajectory.TabStop = false;
            // 
            // ParamsGroup
            // 
            ParamsGroup.Controls.Add(btnSetParam);
            ParamsGroup.Controls.Add(btnSendParam);
            ParamsGroup.Location = new Point(340, 20);
            ParamsGroup.Margin = new Padding(2);
            ParamsGroup.Name = "ParamsGroup";
            ParamsGroup.Padding = new Padding(2);
            ParamsGroup.Size = new Size(123, 123);
            ParamsGroup.TabIndex = 22;
            ParamsGroup.TabStop = false;
            ParamsGroup.Text = "参数";
            // 
            // ProgramStart
            // 
            ProgramStart.Location = new Point(484, 501);
            ProgramStart.Margin = new Padding(2);
            ProgramStart.Name = "ProgramStart";
            ProgramStart.ReadOnly = true;
            ProgramStart.Size = new Size(410, 23);
            ProgramStart.TabIndex = 24;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Microsoft YaHei UI", 26F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(583, 11);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new Size(195, 46);
            label5.TabIndex = 25;
            label5.Text = "浮体轨迹图";
            // 
            // btnClosePort
            // 
            btnClosePort.Location = new Point(161, 84);
            btnClosePort.Margin = new Padding(2);
            btnClosePort.Name = "btnClosePort";
            btnClosePort.Size = new Size(71, 30);
            btnClosePort.TabIndex = 22;
            btnClosePort.Text = "关闭";
            btnClosePort.UseVisualStyleBackColor = true;
            btnClosePort.Click += btnClosePort_Click;
            // 
            // shipdock
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(915, 558);
            Controls.Add(label5);
            Controls.Add(ProgramStart);
            Controls.Add(pbTrajectory);
            Controls.Add(ParamsGroup);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(PortGroups);
            Controls.Add(Log);
            Margin = new Padding(2);
            Name = "shipdock";
            Text = "浮体检测程序";
            Load += shipdock_Load;
            PortGroups.ResumeLayout(false);
            PortGroups.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbTrajectory).EndInit();
            ParamsGroup.ResumeLayout(false);
            ParamsGroup.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private RichTextBox Log;
        private Button btnLog;
        private TextBox logPath;
        private CheckBox IsLog;
        private Label label3;
        private TextBox DataPath;
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
    }
}