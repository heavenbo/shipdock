namespace shipdock
{
    partial class ParamForm
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
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            label12 = new Label();
            label13 = new Label();
            label14 = new Label();
            label16 = new Label();
            label17 = new Label();
            label19 = new Label();
            label20 = new Label();
            label21 = new Label();
            label22 = new Label();
            tbKnear = new TextBox();
            tbSampleNear = new TextBox();
            tbTrNear = new TextBox();
            tbChirpLoop = new TextBox();
            tbKfar = new TextBox();
            tbSampleFar = new TextBox();
            tbTrFar = new TextBox();
            tbTidle = new TextBox();
            label15 = new Label();
            tbFrameT = new TextBox();
            label18 = new Label();
            tbFs = new TextBox();
            label24 = new Label();
            CommonParamsGroup = new GroupBox();
            label35 = new Label();
            label37 = new Label();
            tbTxStart = new TextBox();
            label25 = new Label();
            label27 = new Label();
            tbRxGain = new TextBox();
            label44 = new Label();
            tbStartFreq = new TextBox();
            label43 = new Label();
            label38 = new Label();
            label1 = new Label();
            tbStartADC = new TextBox();
            NearParamsGroup = new GroupBox();
            FarParamsGroup = new GroupBox();
            PerfStatsBox = new GroupBox();
            FarPerfStatsBox = new GroupBox();
            label41 = new Label();
            label42 = new Label();
            tbBfar = new TextBox();
            tbSfar = new TextBox();
            label30 = new Label();
            label31 = new Label();
            label32 = new Label();
            label33 = new Label();
            label34 = new Label();
            label36 = new Label();
            tbVresFar = new TextBox();
            tbRresFar = new TextBox();
            NearPerfStatsBox = new GroupBox();
            label39 = new Label();
            label40 = new Label();
            tbBnear = new TextBox();
            tbSnear = new TextBox();
            label28 = new Label();
            label29 = new Label();
            label2 = new Label();
            label11 = new Label();
            label23 = new Label();
            label26 = new Label();
            tbVresNear = new TextBox();
            tbRresNear = new TextBox();
            DetectModelGroup = new GroupBox();
            btnAllDetect = new RadioButton();
            btnFarDetect = new RadioButton();
            btnNearDetect = new RadioButton();
            btnSetParams = new Button();
            btnClosedParams = new Button();
            ParamsLog = new TextBox();
            CommonParamsGroup.SuspendLayout();
            NearParamsGroup.SuspendLayout();
            FarParamsGroup.SuspendLayout();
            PerfStatsBox.SuspendLayout();
            FarPerfStatsBox.SuspendLayout();
            NearPerfStatsBox.SuspendLayout();
            DetectModelGroup.SuspendLayout();
            SuspendLayout();
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.Black;
            label3.Location = new Point(48, 22);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(56, 17);
            label3.TabIndex = 2;
            label3.Text = "调频斜率";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(171, 22);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(54, 17);
            label4.TabIndex = 3;
            label4.Text = "MHz/μs";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(7, 54);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new Size(97, 17);
            label5.TabIndex = 4;
            label5.Text = "单chirp采样点数";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(19, 85);
            label6.Margin = new Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new Size(85, 17);
            label6.TabIndex = 5;
            label6.Text = "chirp持续时间";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(43, 23);
            label7.Margin = new Padding(2, 0, 2, 0);
            label7.Name = "label7";
            label7.Size = new Size(61, 17);
            label7.TabIndex = 6;
            label7.Text = "chirp间隔";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(299, 23);
            label8.Margin = new Padding(2, 0, 2, 0);
            label8.Name = "label8";
            label8.Size = new Size(73, 17);
            label8.TabIndex = 7;
            label8.Text = "单帧chirp数";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(315, 57);
            label9.Margin = new Padding(2, 0, 2, 0);
            label9.Name = "label9";
            label9.Size = new Size(56, 17);
            label9.TabIndex = 8;
            label9.Text = "采样频率";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(48, 57);
            label10.Margin = new Padding(2, 0, 2, 0);
            label10.Name = "label10";
            label10.Size = new Size(56, 17);
            label10.TabIndex = 9;
            label10.Text = "单帧时长";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(171, 54);
            label12.Margin = new Padding(2, 0, 2, 0);
            label12.Name = "label12";
            label12.Size = new Size(20, 17);
            label12.TabIndex = 11;
            label12.Text = "个";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(171, 85);
            label13.Margin = new Padding(2, 0, 2, 0);
            label13.Name = "label13";
            label13.Size = new Size(22, 17);
            label13.TabIndex = 12;
            label13.Text = "μs";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(441, 23);
            label14.Margin = new Padding(2, 0, 2, 0);
            label14.Name = "label14";
            label14.Size = new Size(20, 17);
            label14.TabIndex = 13;
            label14.Text = "个";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(168, 84);
            label16.Margin = new Padding(2, 0, 2, 0);
            label16.Name = "label16";
            label16.Size = new Size(22, 17);
            label16.TabIndex = 21;
            label16.Text = "μs";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(168, 52);
            label17.Margin = new Padding(2, 0, 2, 0);
            label17.Name = "label17";
            label17.Size = new Size(20, 17);
            label17.TabIndex = 20;
            label17.Text = "个";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(16, 85);
            label19.Margin = new Padding(2, 0, 2, 0);
            label19.Name = "label19";
            label19.Size = new Size(85, 17);
            label19.TabIndex = 18;
            label19.Text = "chirp持续时间";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(7, 54);
            label20.Margin = new Padding(2, 0, 2, 0);
            label20.Name = "label20";
            label20.Size = new Size(97, 17);
            label20.TabIndex = 17;
            label20.Text = "单chirp采样点数";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(169, 22);
            label21.Margin = new Padding(2, 0, 2, 0);
            label21.Name = "label21";
            label21.Size = new Size(54, 17);
            label21.TabIndex = 16;
            label21.Text = "MHz/μs";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(48, 23);
            label22.Margin = new Padding(2, 0, 2, 0);
            label22.Name = "label22";
            label22.Size = new Size(56, 17);
            label22.TabIndex = 15;
            label22.Text = "调频斜率";
            // 
            // tbKnear
            // 
            tbKnear.Location = new Point(108, 22);
            tbKnear.Margin = new Padding(2);
            tbKnear.Name = "tbKnear";
            tbKnear.Size = new Size(61, 23);
            tbKnear.TabIndex = 23;
            tbKnear.TextChanged += tbKnear_TextChanged;
            // 
            // tbSampleNear
            // 
            tbSampleNear.Location = new Point(108, 54);
            tbSampleNear.Margin = new Padding(2);
            tbSampleNear.Name = "tbSampleNear";
            tbSampleNear.Size = new Size(61, 23);
            tbSampleNear.TabIndex = 24;
            tbSampleNear.TextChanged += tbSampleNear_TextChanged;
            // 
            // tbTrNear
            // 
            tbTrNear.Location = new Point(108, 85);
            tbTrNear.Margin = new Padding(2);
            tbTrNear.Name = "tbTrNear";
            tbTrNear.Size = new Size(61, 23);
            tbTrNear.TabIndex = 25;
            tbTrNear.TextChanged += tbTrNear_TextChanged;
            // 
            // tbChirpLoop
            // 
            tbChirpLoop.Location = new Point(375, 19);
            tbChirpLoop.Margin = new Padding(2);
            tbChirpLoop.Name = "tbChirpLoop";
            tbChirpLoop.Size = new Size(61, 23);
            tbChirpLoop.TabIndex = 26;
            tbChirpLoop.TextChanged += tbChirpLoop_TextChanged;
            // 
            // tbKfar
            // 
            tbKfar.Location = new Point(105, 22);
            tbKfar.Margin = new Padding(2);
            tbKfar.Name = "tbKfar";
            tbKfar.Size = new Size(61, 23);
            tbKfar.TabIndex = 27;
            tbKfar.TextChanged += tbKfar_TextChanged;
            // 
            // tbSampleFar
            // 
            tbSampleFar.Location = new Point(105, 54);
            tbSampleFar.Margin = new Padding(2);
            tbSampleFar.Name = "tbSampleFar";
            tbSampleFar.Size = new Size(61, 23);
            tbSampleFar.TabIndex = 28;
            tbSampleFar.TextChanged += tbSampleFar_TextChanged;
            // 
            // tbTrFar
            // 
            tbTrFar.Location = new Point(105, 85);
            tbTrFar.Margin = new Padding(2);
            tbTrFar.Name = "tbTrFar";
            tbTrFar.Size = new Size(61, 23);
            tbTrFar.TabIndex = 29;
            tbTrFar.TextChanged += tbTrFar_TextChanged;
            // 
            // tbTidle
            // 
            tbTidle.Location = new Point(108, 19);
            tbTidle.Margin = new Padding(2);
            tbTidle.Name = "tbTidle";
            tbTidle.Size = new Size(61, 23);
            tbTidle.TabIndex = 30;
            tbTidle.TextChanged += tbTidle_TextChanged;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(171, 21);
            label15.Margin = new Padding(2, 0, 2, 0);
            label15.Name = "label15";
            label15.Size = new Size(22, 17);
            label15.TabIndex = 31;
            label15.Text = "μs";
            // 
            // tbFrameT
            // 
            tbFrameT.Location = new Point(108, 53);
            tbFrameT.Margin = new Padding(2);
            tbFrameT.Name = "tbFrameT";
            tbFrameT.Size = new Size(61, 23);
            tbFrameT.TabIndex = 32;
            tbFrameT.TextChanged += tbFrameT_TextChanged;
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(172, 57);
            label18.Margin = new Padding(2, 0, 2, 0);
            label18.Name = "label18";
            label18.Size = new Size(25, 17);
            label18.TabIndex = 33;
            label18.Text = "ms";
            // 
            // tbFs
            // 
            tbFs.Location = new Point(375, 53);
            tbFs.Margin = new Padding(2);
            tbFs.Name = "tbFs";
            tbFs.Size = new Size(61, 23);
            tbFs.TabIndex = 34;
            tbFs.TextChanged += tbFs_TextChanged;
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Location = new Point(441, 57);
            label24.Margin = new Padding(2, 0, 2, 0);
            label24.Name = "label24";
            label24.Size = new Size(30, 17);
            label24.TabIndex = 35;
            label24.Text = "kHz";
            // 
            // CommonParamsGroup
            // 
            CommonParamsGroup.Controls.Add(label35);
            CommonParamsGroup.Controls.Add(label37);
            CommonParamsGroup.Controls.Add(tbTxStart);
            CommonParamsGroup.Controls.Add(label25);
            CommonParamsGroup.Controls.Add(label27);
            CommonParamsGroup.Controls.Add(tbRxGain);
            CommonParamsGroup.Controls.Add(label44);
            CommonParamsGroup.Controls.Add(tbStartFreq);
            CommonParamsGroup.Controls.Add(label43);
            CommonParamsGroup.Controls.Add(label38);
            CommonParamsGroup.Controls.Add(label1);
            CommonParamsGroup.Controls.Add(tbStartADC);
            CommonParamsGroup.Controls.Add(tbFs);
            CommonParamsGroup.Controls.Add(label24);
            CommonParamsGroup.Controls.Add(label7);
            CommonParamsGroup.Controls.Add(label8);
            CommonParamsGroup.Controls.Add(label18);
            CommonParamsGroup.Controls.Add(label9);
            CommonParamsGroup.Controls.Add(tbFrameT);
            CommonParamsGroup.Controls.Add(label10);
            CommonParamsGroup.Controls.Add(label15);
            CommonParamsGroup.Controls.Add(label14);
            CommonParamsGroup.Controls.Add(tbTidle);
            CommonParamsGroup.Controls.Add(tbChirpLoop);
            CommonParamsGroup.Location = new Point(29, 74);
            CommonParamsGroup.Margin = new Padding(2);
            CommonParamsGroup.Name = "CommonParamsGroup";
            CommonParamsGroup.Padding = new Padding(2);
            CommonParamsGroup.Size = new Size(498, 155);
            CommonParamsGroup.TabIndex = 36;
            CommonParamsGroup.TabStop = false;
            CommonParamsGroup.Text = "基本参数";
            // 
            // label35
            // 
            label35.AutoSize = true;
            label35.Location = new Point(35, 125);
            label35.Margin = new Padding(2, 0, 2, 0);
            label35.Name = "label35";
            label35.Size = new Size(69, 17);
            label35.TabIndex = 48;
            label35.Text = "Tx开始时间";
            // 
            // label37
            // 
            label37.AutoSize = true;
            label37.Location = new Point(171, 125);
            label37.Margin = new Padding(2, 0, 2, 0);
            label37.Name = "label37";
            label37.Size = new Size(22, 17);
            label37.TabIndex = 47;
            label37.Text = "μs";
            // 
            // tbTxStart
            // 
            tbTxStart.Location = new Point(108, 122);
            tbTxStart.Margin = new Padding(2);
            tbTxStart.Name = "tbTxStart";
            tbTxStart.Size = new Size(61, 23);
            tbTxStart.TabIndex = 46;
            tbTxStart.TextChanged += tbTxStart_TextChanged;
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new Point(313, 125);
            label25.Margin = new Padding(2, 0, 2, 0);
            label25.Name = "label25";
            label25.Size = new Size(56, 17);
            label25.TabIndex = 45;
            label25.Text = "接收增益";
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Location = new Point(441, 125);
            label27.Margin = new Padding(2, 0, 2, 0);
            label27.Name = "label27";
            label27.Size = new Size(24, 17);
            label27.TabIndex = 44;
            label27.Text = "dB";
            // 
            // tbRxGain
            // 
            tbRxGain.Location = new Point(375, 122);
            tbRxGain.Margin = new Padding(2);
            tbRxGain.Name = "tbRxGain";
            tbRxGain.Size = new Size(61, 23);
            tbRxGain.TabIndex = 43;
            tbRxGain.TextChanged += tbRxGain_TextChanged;
            // 
            // label44
            // 
            label44.AutoSize = true;
            label44.Location = new Point(441, 91);
            label44.Margin = new Padding(2, 0, 2, 0);
            label44.Name = "label44";
            label44.Size = new Size(32, 17);
            label44.TabIndex = 42;
            label44.Text = "GHz";
            // 
            // tbStartFreq
            // 
            tbStartFreq.Location = new Point(375, 88);
            tbStartFreq.Margin = new Padding(2);
            tbStartFreq.Name = "tbStartFreq";
            tbStartFreq.Size = new Size(61, 23);
            tbStartFreq.TabIndex = 41;
            tbStartFreq.TextChanged += tbStartFreq_TextChanged;
            // 
            // label43
            // 
            label43.AutoSize = true;
            label43.Location = new Point(316, 91);
            label43.Margin = new Padding(2, 0, 2, 0);
            label43.Name = "label43";
            label43.Size = new Size(56, 17);
            label43.TabIndex = 40;
            label43.Text = "起始频率";
            // 
            // label38
            // 
            label38.AutoSize = true;
            label38.Location = new Point(24, 91);
            label38.Margin = new Padding(2, 0, 2, 0);
            label38.Name = "label38";
            label38.Size = new Size(81, 17);
            label38.TabIndex = 39;
            label38.Text = "ADC开始时间";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(171, 91);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(22, 17);
            label1.TabIndex = 38;
            label1.Text = "μs";
            // 
            // tbStartADC
            // 
            tbStartADC.Location = new Point(108, 88);
            tbStartADC.Margin = new Padding(2);
            tbStartADC.Name = "tbStartADC";
            tbStartADC.Size = new Size(61, 23);
            tbStartADC.TabIndex = 37;
            tbStartADC.TextChanged += tbStartADC_TextChanged;
            // 
            // NearParamsGroup
            // 
            NearParamsGroup.Controls.Add(tbTrNear);
            NearParamsGroup.Controls.Add(label3);
            NearParamsGroup.Controls.Add(label4);
            NearParamsGroup.Controls.Add(label5);
            NearParamsGroup.Controls.Add(label6);
            NearParamsGroup.Controls.Add(label12);
            NearParamsGroup.Controls.Add(tbSampleNear);
            NearParamsGroup.Controls.Add(label13);
            NearParamsGroup.Controls.Add(tbKnear);
            NearParamsGroup.Location = new Point(29, 233);
            NearParamsGroup.Margin = new Padding(2);
            NearParamsGroup.Name = "NearParamsGroup";
            NearParamsGroup.Padding = new Padding(2);
            NearParamsGroup.Size = new Size(230, 115);
            NearParamsGroup.TabIndex = 37;
            NearParamsGroup.TabStop = false;
            NearParamsGroup.Text = "近距离探测波形";
            // 
            // FarParamsGroup
            // 
            FarParamsGroup.Controls.Add(tbSampleFar);
            FarParamsGroup.Controls.Add(label22);
            FarParamsGroup.Controls.Add(label21);
            FarParamsGroup.Controls.Add(tbTrFar);
            FarParamsGroup.Controls.Add(label20);
            FarParamsGroup.Controls.Add(label19);
            FarParamsGroup.Controls.Add(tbKfar);
            FarParamsGroup.Controls.Add(label17);
            FarParamsGroup.Controls.Add(label16);
            FarParamsGroup.Location = new Point(297, 233);
            FarParamsGroup.Margin = new Padding(2);
            FarParamsGroup.Name = "FarParamsGroup";
            FarParamsGroup.Padding = new Padding(2);
            FarParamsGroup.Size = new Size(230, 115);
            FarParamsGroup.TabIndex = 38;
            FarParamsGroup.TabStop = false;
            FarParamsGroup.Text = "远距离探测波形";
            // 
            // PerfStatsBox
            // 
            PerfStatsBox.Controls.Add(FarPerfStatsBox);
            PerfStatsBox.Controls.Add(NearPerfStatsBox);
            PerfStatsBox.Location = new Point(29, 352);
            PerfStatsBox.Margin = new Padding(2);
            PerfStatsBox.Name = "PerfStatsBox";
            PerfStatsBox.Padding = new Padding(2);
            PerfStatsBox.Size = new Size(498, 193);
            PerfStatsBox.TabIndex = 39;
            PerfStatsBox.TabStop = false;
            PerfStatsBox.Text = "性能指标";
            // 
            // FarPerfStatsBox
            // 
            FarPerfStatsBox.Controls.Add(label41);
            FarPerfStatsBox.Controls.Add(label42);
            FarPerfStatsBox.Controls.Add(tbBfar);
            FarPerfStatsBox.Controls.Add(tbSfar);
            FarPerfStatsBox.Controls.Add(label30);
            FarPerfStatsBox.Controls.Add(label31);
            FarPerfStatsBox.Controls.Add(label32);
            FarPerfStatsBox.Controls.Add(label33);
            FarPerfStatsBox.Controls.Add(label34);
            FarPerfStatsBox.Controls.Add(label36);
            FarPerfStatsBox.Controls.Add(tbVresFar);
            FarPerfStatsBox.Controls.Add(tbRresFar);
            FarPerfStatsBox.Location = new Point(284, 21);
            FarPerfStatsBox.Margin = new Padding(2);
            FarPerfStatsBox.Name = "FarPerfStatsBox";
            FarPerfStatsBox.Padding = new Padding(2);
            FarPerfStatsBox.Size = new Size(189, 157);
            FarPerfStatsBox.TabIndex = 1;
            FarPerfStatsBox.TabStop = false;
            FarPerfStatsBox.Text = "远距离";
            // 
            // label41
            // 
            label41.AutoSize = true;
            label41.Location = new Point(39, 21);
            label41.Margin = new Padding(2, 0, 2, 0);
            label41.Name = "label41";
            label41.Size = new Size(32, 17);
            label41.TabIndex = 50;
            label41.Text = "带宽";
            // 
            // label42
            // 
            label42.AutoSize = true;
            label42.Location = new Point(147, 18);
            label42.Margin = new Padding(2, 0, 2, 0);
            label42.Name = "label42";
            label42.Size = new Size(35, 17);
            label42.TabIndex = 51;
            label42.Text = "MHz";
            // 
            // tbBfar
            // 
            tbBfar.Location = new Point(84, 16);
            tbBfar.Margin = new Padding(2);
            tbBfar.Name = "tbBfar";
            tbBfar.ReadOnly = true;
            tbBfar.Size = new Size(61, 23);
            tbBfar.TabIndex = 52;
            // 
            // tbSfar
            // 
            tbSfar.Location = new Point(85, 116);
            tbSfar.Margin = new Padding(2);
            tbSfar.Name = "tbSfar";
            tbSfar.ReadOnly = true;
            tbSfar.Size = new Size(61, 23);
            tbSfar.TabIndex = 49;
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.Location = new Point(17, 118);
            label30.Margin = new Padding(2, 0, 2, 0);
            label30.Name = "label30";
            label30.Size = new Size(56, 17);
            label30.TabIndex = 47;
            label30.Text = "探测距离";
            // 
            // label31
            // 
            label31.AutoSize = true;
            label31.Location = new Point(148, 52);
            label31.Margin = new Padding(2, 0, 2, 0);
            label31.Name = "label31";
            label31.Size = new Size(25, 17);
            label31.TabIndex = 48;
            label31.Text = "cm";
            // 
            // label32
            // 
            label32.AutoSize = true;
            label32.Location = new Point(28, 52);
            label32.Margin = new Padding(2, 0, 2, 0);
            label32.Name = "label32";
            label32.Size = new Size(44, 17);
            label32.TabIndex = 38;
            label32.Text = "距离元";
            // 
            // label33
            // 
            label33.AutoSize = true;
            label33.Location = new Point(148, 118);
            label33.Margin = new Padding(2, 0, 2, 0);
            label33.Name = "label33";
            label33.Size = new Size(19, 17);
            label33.TabIndex = 39;
            label33.Text = "m";
            // 
            // label34
            // 
            label34.AutoSize = true;
            label34.Location = new Point(28, 84);
            label34.Margin = new Padding(2, 0, 2, 0);
            label34.Name = "label34";
            label34.Size = new Size(44, 17);
            label34.TabIndex = 40;
            label34.Text = "速度元";
            // 
            // label36
            // 
            label36.AutoSize = true;
            label36.Location = new Point(147, 84);
            label36.Margin = new Padding(2, 0, 2, 0);
            label36.Name = "label36";
            label36.Size = new Size(30, 17);
            label36.TabIndex = 42;
            label36.Text = "m/s";
            // 
            // tbVresFar
            // 
            tbVresFar.Location = new Point(84, 77);
            tbVresFar.Margin = new Padding(2);
            tbVresFar.Name = "tbVresFar";
            tbVresFar.ReadOnly = true;
            tbVresFar.Size = new Size(61, 23);
            tbVresFar.TabIndex = 45;
            // 
            // tbRresFar
            // 
            tbRresFar.Location = new Point(84, 45);
            tbRresFar.Margin = new Padding(2);
            tbRresFar.Name = "tbRresFar";
            tbRresFar.ReadOnly = true;
            tbRresFar.Size = new Size(61, 23);
            tbRresFar.TabIndex = 44;
            // 
            // NearPerfStatsBox
            // 
            NearPerfStatsBox.Controls.Add(label39);
            NearPerfStatsBox.Controls.Add(label40);
            NearPerfStatsBox.Controls.Add(tbBnear);
            NearPerfStatsBox.Controls.Add(tbSnear);
            NearPerfStatsBox.Controls.Add(label28);
            NearPerfStatsBox.Controls.Add(label29);
            NearPerfStatsBox.Controls.Add(label2);
            NearPerfStatsBox.Controls.Add(label11);
            NearPerfStatsBox.Controls.Add(label23);
            NearPerfStatsBox.Controls.Add(label26);
            NearPerfStatsBox.Controls.Add(tbVresNear);
            NearPerfStatsBox.Controls.Add(tbRresNear);
            NearPerfStatsBox.Location = new Point(26, 21);
            NearPerfStatsBox.Margin = new Padding(2);
            NearPerfStatsBox.Name = "NearPerfStatsBox";
            NearPerfStatsBox.Padding = new Padding(2);
            NearPerfStatsBox.Size = new Size(189, 157);
            NearPerfStatsBox.TabIndex = 0;
            NearPerfStatsBox.TabStop = false;
            NearPerfStatsBox.Text = "近距离";
            // 
            // label39
            // 
            label39.AutoSize = true;
            label39.Location = new Point(38, 21);
            label39.Margin = new Padding(2, 0, 2, 0);
            label39.Name = "label39";
            label39.Size = new Size(32, 17);
            label39.TabIndex = 38;
            label39.Text = "带宽";
            // 
            // label40
            // 
            label40.AutoSize = true;
            label40.Location = new Point(151, 21);
            label40.Margin = new Padding(2, 0, 2, 0);
            label40.Name = "label40";
            label40.Size = new Size(35, 17);
            label40.TabIndex = 39;
            label40.Text = "MHz";
            // 
            // tbBnear
            // 
            tbBnear.Location = new Point(82, 16);
            tbBnear.Margin = new Padding(2);
            tbBnear.Name = "tbBnear";
            tbBnear.ReadOnly = true;
            tbBnear.Size = new Size(61, 23);
            tbBnear.TabIndex = 40;
            // 
            // tbSnear
            // 
            tbSnear.Location = new Point(82, 116);
            tbSnear.Margin = new Padding(2);
            tbSnear.Name = "tbSnear";
            tbSnear.ReadOnly = true;
            tbSnear.Size = new Size(61, 23);
            tbSnear.TabIndex = 37;
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Location = new Point(15, 116);
            label28.Margin = new Padding(2, 0, 2, 0);
            label28.Name = "label28";
            label28.Size = new Size(56, 17);
            label28.TabIndex = 35;
            label28.Text = "探测距离";
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.Location = new Point(146, 116);
            label29.Margin = new Padding(2, 0, 2, 0);
            label29.Name = "label29";
            label29.Size = new Size(19, 17);
            label29.TabIndex = 36;
            label29.Text = "m";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(26, 54);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(44, 17);
            label2.TabIndex = 26;
            label2.Text = "距离元";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(151, 54);
            label11.Margin = new Padding(2, 0, 2, 0);
            label11.Name = "label11";
            label11.Size = new Size(25, 17);
            label11.TabIndex = 27;
            label11.Text = "cm";
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Location = new Point(26, 87);
            label23.Margin = new Padding(2, 0, 2, 0);
            label23.Name = "label23";
            label23.Size = new Size(44, 17);
            label23.TabIndex = 28;
            label23.Text = "速度元";
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Location = new Point(151, 87);
            label26.Margin = new Padding(2, 0, 2, 0);
            label26.Name = "label26";
            label26.Size = new Size(30, 17);
            label26.TabIndex = 30;
            label26.Text = "m/s";
            // 
            // tbVresNear
            // 
            tbVresNear.Location = new Point(82, 85);
            tbVresNear.Margin = new Padding(2);
            tbVresNear.Name = "tbVresNear";
            tbVresNear.ReadOnly = true;
            tbVresNear.Size = new Size(61, 23);
            tbVresNear.TabIndex = 33;
            // 
            // tbRresNear
            // 
            tbRresNear.Location = new Point(82, 52);
            tbRresNear.Margin = new Padding(2);
            tbRresNear.Name = "tbRresNear";
            tbRresNear.ReadOnly = true;
            tbRresNear.Size = new Size(61, 23);
            tbRresNear.TabIndex = 32;
            // 
            // DetectModelGroup
            // 
            DetectModelGroup.Controls.Add(btnAllDetect);
            DetectModelGroup.Controls.Add(btnFarDetect);
            DetectModelGroup.Controls.Add(btnNearDetect);
            DetectModelGroup.Location = new Point(29, 18);
            DetectModelGroup.Margin = new Padding(2);
            DetectModelGroup.Name = "DetectModelGroup";
            DetectModelGroup.Padding = new Padding(2);
            DetectModelGroup.Size = new Size(498, 52);
            DetectModelGroup.TabIndex = 40;
            DetectModelGroup.TabStop = false;
            DetectModelGroup.Text = "探测模式";
            // 
            // btnAllDetect
            // 
            btnAllDetect.AutoSize = true;
            btnAllDetect.Checked = true;
            btnAllDetect.Location = new Point(373, 26);
            btnAllDetect.Margin = new Padding(2);
            btnAllDetect.Name = "btnAllDetect";
            btnAllDetect.Size = new Size(74, 21);
            btnAllDetect.TabIndex = 2;
            btnAllDetect.TabStop = true;
            btnAllDetect.Text = "兼顾模式";
            btnAllDetect.UseVisualStyleBackColor = true;
            btnAllDetect.CheckedChanged += btnAllDetect_CheckedChanged;
            // 
            // btnFarDetect
            // 
            btnFarDetect.AutoSize = true;
            btnFarDetect.Location = new Point(199, 26);
            btnFarDetect.Margin = new Padding(2);
            btnFarDetect.Name = "btnFarDetect";
            btnFarDetect.Size = new Size(86, 21);
            btnFarDetect.TabIndex = 1;
            btnFarDetect.Text = "远距离探测";
            btnFarDetect.UseVisualStyleBackColor = true;
            btnFarDetect.CheckedChanged += btnFarDetect_CheckedChanged;
            // 
            // btnNearDetect
            // 
            btnNearDetect.AutoSize = true;
            btnNearDetect.Location = new Point(26, 26);
            btnNearDetect.Margin = new Padding(2);
            btnNearDetect.Name = "btnNearDetect";
            btnNearDetect.Size = new Size(86, 21);
            btnNearDetect.TabIndex = 0;
            btnNearDetect.Text = "近距离探测";
            btnNearDetect.UseVisualStyleBackColor = true;
            btnNearDetect.CheckedChanged += btnNearDetect_CheckedChanged;
            // 
            // btnSetParams
            // 
            btnSetParams.Location = new Point(373, 574);
            btnSetParams.Margin = new Padding(2);
            btnSetParams.Name = "btnSetParams";
            btnSetParams.Size = new Size(71, 24);
            btnSetParams.TabIndex = 41;
            btnSetParams.Text = "应用";
            btnSetParams.UseVisualStyleBackColor = true;
            btnSetParams.Click += btnSetParams_Click;
            // 
            // btnClosedParams
            // 
            btnClosedParams.Location = new Point(459, 574);
            btnClosedParams.Margin = new Padding(2);
            btnClosedParams.Name = "btnClosedParams";
            btnClosedParams.Size = new Size(71, 24);
            btnClosedParams.TabIndex = 42;
            btnClosedParams.Text = "完成";
            btnClosedParams.UseVisualStyleBackColor = true;
            btnClosedParams.Click += ClosedParams_Click;
            // 
            // ParamsLog
            // 
            ParamsLog.Location = new Point(46, 579);
            ParamsLog.Margin = new Padding(2);
            ParamsLog.Name = "ParamsLog";
            ParamsLog.ReadOnly = true;
            ParamsLog.Size = new Size(205, 23);
            ParamsLog.TabIndex = 43;
            // 
            // ParamForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(544, 620);
            Controls.Add(ParamsLog);
            Controls.Add(btnClosedParams);
            Controls.Add(btnSetParams);
            Controls.Add(DetectModelGroup);
            Controls.Add(PerfStatsBox);
            Controls.Add(FarParamsGroup);
            Controls.Add(NearParamsGroup);
            Controls.Add(CommonParamsGroup);
            Margin = new Padding(2);
            Name = "ParamForm";
            Text = "参数设置";
            Load += ParamForm_Load;
            CommonParamsGroup.ResumeLayout(false);
            CommonParamsGroup.PerformLayout();
            NearParamsGroup.ResumeLayout(false);
            NearParamsGroup.PerformLayout();
            FarParamsGroup.ResumeLayout(false);
            FarParamsGroup.PerformLayout();
            PerfStatsBox.ResumeLayout(false);
            FarPerfStatsBox.ResumeLayout(false);
            FarPerfStatsBox.PerformLayout();
            NearPerfStatsBox.ResumeLayout(false);
            NearPerfStatsBox.PerformLayout();
            DetectModelGroup.ResumeLayout(false);
            DetectModelGroup.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label16;
        private Label label17;
        private Label label19;
        private Label label20;
        private Label label21;
        private Label label22;
        private TextBox tbKnear;
        private TextBox tbSampleNear;
        private TextBox tbTrNear;
        private TextBox tbChirpLoop;
        private TextBox tbKfar;
        private TextBox tbSampleFar;
        private TextBox tbTrFar;
        private TextBox tbTidle;
        private Label label15;
        private TextBox tbFrameT;
        private Label label18;
        private TextBox tbFs;
        private Label label24;
        private GroupBox CommonParamsGroup;
        private GroupBox NearParamsGroup;
        private GroupBox FarParamsGroup;
        private GroupBox PerfStatsBox;
        private GroupBox NearPerfStatsBox;
        private GroupBox FarPerfStatsBox;
        private Label label2;
        private Label label11;
        private Label label23;
        private Label label26;
        private TextBox tbVresNear;
        private TextBox tbRresNear;
        private TextBox tbSnear;
        private Label label28;
        private Label label29;
        private TextBox tbSfar;
        private Label label30;
        private Label label31;
        private Label label32;
        private Label label33;
        private Label label34;
        private Label label36;
        private TextBox tbVresFar;
        private TextBox tbRresFar;
        private GroupBox DetectModelGroup;
        private RadioButton btnNearDetect;
        private RadioButton btnAllDetect;
        private RadioButton btnFarDetect;
        private Button btnSetParams;
        private Button btnClosedParams;
        private TextBox ParamsLog;
        private TextBox tbStartADC;
        private Label label1;
        private Label label38;
        private Label label39;
        private Label label40;
        private TextBox tbBnear;
        private Label label41;
        private Label label42;
        private TextBox tbBfar;
        private Label label43;
        private Label label44;
        private TextBox tbStartFreq;
        private Label label35;
        private Label label37;
        private TextBox tbTxStart;
        private Label label25;
        private Label label27;
        private TextBox tbRxGain;
    }
}