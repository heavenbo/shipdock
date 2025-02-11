using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace shipdock
{
    public partial class ParamForm : Form
    {
        //常数
        public const int SpeedOfLight = 299792458;
        private const double dx = 1.8161e-3;
        private int Nant = 4;
        double pi = 3.1415926;
        //雷达波形参数
        private double StartFreq;
        private double Tidle;
        private double Fs;
        private double ChirpLoop;
        private double FrameT;
        private double K_near;
        private double Sample_near;
        private double Tr_near;
        private double K_far;
        private double Sample_far;
        private double Tr_far;
        private double ADC_start_time;
        //性能指标
        private double B_near;
        private double R_res_near;
        private double V_res_near;
        private double S_near;
        private double B_far;
        private double R_res_far;
        private double V_res_far;
        private double S_far;
        public ParamForm()
        {
            InitializeComponent();
        }

        private void ParamForm_Load(object sender, EventArgs e)
        {
            this.TidleBox.Text = Properties.Settings.Default.Tidle.ToString();
            this.FsBox.Text = Properties.Settings.Default.Fs.ToString();
            this.ChirpLoopBox.Text = Properties.Settings.Default.ChirpLoop.ToString();
            this.FrameTBox.Text = Properties.Settings.Default.FrameT.ToString();
            this.K_nearBox.Text = Properties.Settings.Default.K_near.ToString();
            this.Sample_nearBox.Text = Properties.Settings.Default.Sample_near.ToString();
            this.Tr_nearBox.Text = Properties.Settings.Default.Tr_near.ToString();
            this.K_farBox.Text = Properties.Settings.Default.K_far.ToString();
            this.Sample_farBox.Text = Properties.Settings.Default.Sample_far.ToString();
            this.Tr_farBox.Text = Properties.Settings.Default.Tr_far.ToString();
            this.ADC_start_timeBox.Text = Properties.Settings.Default.ADC_start_time.ToString();
            this.StartFreqBox.Text = Properties.Settings.Default.StartFreq.ToString();
        }
        private void BtnNearDetect_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.BtnNearDetect.Checked)
            {
                return;
            }
            foreach (Control control in this.FarParamsGroup.Controls)
            {
                if (control is Label)
                {
                    control.ForeColor = System.Drawing.Color.Gray;
                }
                else if (control is System.Windows.Forms.TextBox)
                {
                    control.BackColor = System.Drawing.Color.LightGray;
                    control.Enabled = false;
                }
            }
            foreach (Control control in this.NearParamsGroup.Controls)
            {
                if (control is Label)
                {
                    control.ForeColor = System.Drawing.Color.Black;
                }
                else if (control is System.Windows.Forms.TextBox)
                {
                    control.BackColor = System.Drawing.Color.White;
                    control.Enabled = true;
                }
            }
            foreach (Control control in this.NearPerfStatsBox.Controls)
            {
                if (control is Label)
                {
                    control.ForeColor = System.Drawing.Color.Black;
                }
                else if (control is System.Windows.Forms.TextBox)
                {
                    control.BackColor = System.Drawing.Color.White;
                    control.Enabled = true;
                }
            }
            foreach (Control control in this.FarPerfStatsBox.Controls)
            {
                if (control is Label)
                {
                    control.ForeColor = System.Drawing.Color.Gray;
                }
                else if (control is System.Windows.Forms.TextBox)
                {
                    control.BackColor = System.Drawing.Color.LightGray;
                    control.Enabled = false;
                }
            }
        }

        private void BtnFarDetect_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.BtnFarDetect.Checked)
            {
                return;
            }
            foreach (Control control in this.FarParamsGroup.Controls)
            {
                if (control is Label)
                {
                    control.ForeColor = System.Drawing.Color.Black;
                }
                else if (control is System.Windows.Forms.TextBox)
                {
                    control.BackColor = System.Drawing.Color.White;
                    control.Enabled = true;
                }
            }
            foreach (Control control in this.NearParamsGroup.Controls)
            {
                if (control is Label)
                {
                    control.ForeColor = System.Drawing.Color.Gray;
                }
                else if (control is System.Windows.Forms.TextBox)
                {
                    control.BackColor = System.Drawing.Color.LightGray;
                    control.Enabled = false;
                }
            }
            foreach (Control control in this.NearPerfStatsBox.Controls)
            {
                if (control is Label)
                {
                    control.ForeColor = System.Drawing.Color.Gray;
                }
                else if (control is System.Windows.Forms.TextBox)
                {
                    control.BackColor = System.Drawing.Color.LightGray;
                    control.Enabled = false;
                }
            }
            foreach (Control control in this.FarPerfStatsBox.Controls)
            {
                if (control is Label)
                {
                    control.ForeColor = System.Drawing.Color.Black;
                }
                else if (control is System.Windows.Forms.TextBox)
                {
                    control.BackColor = System.Drawing.Color.White;
                    control.Enabled = true;
                }
            }
        }

        private void btnAllDetect_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.btnAllDetect.Checked)
            {
                return;
            }
            foreach (Control control in this.FarParamsGroup.Controls)
            {
                if (control is Label)
                {
                    control.ForeColor = System.Drawing.Color.Black;
                }
                else if (control is System.Windows.Forms.TextBox)
                {
                    control.BackColor = System.Drawing.Color.White;
                    control.Enabled = true;
                }
            }
            foreach (Control control in this.NearParamsGroup.Controls)
            {
                if (control is Label)
                {
                    control.ForeColor = System.Drawing.Color.Black;
                }
                else if (control is System.Windows.Forms.TextBox)
                {
                    control.BackColor = System.Drawing.Color.White;
                    control.Enabled = true;
                }
            }
            foreach (Control control in this.NearPerfStatsBox.Controls)
            {
                if (control is Label)
                {
                    control.ForeColor = System.Drawing.Color.Black;
                }
                else if (control is System.Windows.Forms.TextBox)
                {
                    control.BackColor = System.Drawing.Color.White;
                    control.Enabled = true;
                }
            }
            foreach (Control control in this.FarPerfStatsBox.Controls)
            {
                if (control is Label)
                {
                    control.ForeColor = System.Drawing.Color.Black;
                }
                else if (control is System.Windows.Forms.TextBox)
                {
                    control.BackColor = System.Drawing.Color.White;
                    control.Enabled = true;
                }
            }
        }
        private void TidleBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TidleBox.Text))
                return;
            char lastChar = TidleBox.Text[TidleBox.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return; 
            if (!double.TryParse(TidleBox.Text, out double result))
            {
                this.TidleBox.Text = string.Empty;  // 清空文本
            }
        }

        private void ChirpLoopBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ChirpLoopBox.Text))
                return;
            char lastChar = ChirpLoopBox.Text[ChirpLoopBox.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            // 判断文本内容是否可以转换为有效的 double 类型
            if (!double.TryParse(this.ChirpLoopBox.Text, out double result))
            {
                this.ChirpLoopBox.Text = string.Empty;  // 清空文本
            }
        }

        private void FrameTBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FrameTBox.Text))
                return;
            char lastChar = FrameTBox.Text[FrameTBox.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(this.FrameTBox.Text, out double result))
            {
                this.FrameTBox.Text = string.Empty;  // 清空文本
            }
        }

        private void FsBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FsBox.Text))
                return;
            char lastChar = FsBox.Text[FsBox.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(this.FsBox.Text, out double result))
            {
                this.FsBox.Text = string.Empty;  // 清空文本
            }
        }
        private void StartFreqBoxBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(StartFreqBox.Text))
                return;
            char lastChar = StartFreqBox.Text[StartFreqBox.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(this.StartFreqBox.Text, out double result))
            {
                this.StartFreqBox.Text = string.Empty;  // 清空文本
            }
        }
        private void K_nearBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(K_nearBox.Text))
                return;
            char lastChar = K_nearBox.Text[K_nearBox.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(this.K_nearBox.Text, out double result))
            {
                this.K_nearBox.Text = string.Empty;  // 清空文本
            }
        }

        private void K_farBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(K_farBox.Text))
                return;
            char lastChar = K_farBox.Text[K_farBox.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(this.K_farBox.Text, out double result))
            {
                this.K_farBox.Text = string.Empty;  // 清空文本
            }
        }

        private void Sample_nearBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sample_nearBox.Text))
                return;
            char lastChar = Sample_nearBox.Text[Sample_nearBox.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(this.Sample_nearBox.Text, out double result))
            {
                this.Sample_nearBox.Text = string.Empty;  // 清空文本
            }
        }

        private void Sample_farBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sample_farBox.Text))
                return;
            char lastChar = Sample_farBox.Text[Sample_farBox.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(this.Sample_farBox.Text, out double result))
            {
                this.Sample_farBox.Text = string.Empty;  // 清空文本
            }
        }

        private void Tr_nearBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Tr_nearBox.Text))
                return;
            char lastChar = Tr_nearBox.Text[Tr_nearBox.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(this.Tr_nearBox.Text, out double result))
            {
                this.Tr_nearBox.Text = string.Empty;  // 清空文本
            }
        }

        private void Tr_farBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Tr_farBox.Text))
                return;
            char lastChar = Tr_farBox.Text[Tr_farBox.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(this.Tr_farBox.Text, out double result))
            {
                this.Tr_farBox.Text = string.Empty;  // 清空文本
            }
        }
        private void ADC_start_timeBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ADC_start_timeBox.Text))
                return;
            char lastChar = ADC_start_timeBox.Text[ADC_start_timeBox.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(this.ADC_start_timeBox.Text, out double result))
            {
                this.ADC_start_timeBox.Text = string.Empty;  // 清空文本
            }
        }
        private void SetParams_Click(object sender, EventArgs e)
        {
            try
            {
                this.Tidle = double.Parse(this.TidleBox.Text);
                this.Fs = double.Parse(this.FsBox.Text);
                this.ChirpLoop = double.Parse(this.ChirpLoopBox.Text);
                this.FrameT = double.Parse(this.FrameTBox.Text);
                this.K_near = double.Parse(this.K_nearBox.Text);
                this.Sample_near = double.Parse(this.Sample_nearBox.Text);
                this.Tr_near = double.Parse(this.Tr_nearBox.Text);
                this.K_far = double.Parse(this.K_farBox.Text);
                this.Sample_far = double.Parse(this.Sample_farBox.Text);
                this.Tr_far = double.Parse(this.Tr_farBox.Text);
                this.ADC_start_time = double.Parse(this.ADC_start_timeBox.Text);
                this.StartFreq = double.Parse(this.StartFreqBox.Text);
                //计算性能指标

                double EffBw_near = this.K_near * 1e12 * this.Sample_near / Fs;
                double lambda_near = SpeedOfLight / (StartFreq * 1e9 + EffBw_near / 2);
                this.B_near = this.K_near * this.Tr_near * 1e6;
                this.R_res_near = SpeedOfLight / (2 * EffBw_near);
                this.S_near = this.Sample_near * this.R_res_near;

                double EffBw_far = this.K_far * 1e12 * this.Sample_far / Fs;
                double lambda_far = SpeedOfLight / (StartFreq * 1e9 + EffBw_far / 2);
                this.B_far = this.K_far * this.Tr_far * 1e6;
                this.R_res_far = SpeedOfLight / (2 * EffBw_far);
                this.S_far = this.Sample_far * this.R_res_far;
                if (this.BtnNearDetect.Checked)
                {
                    this.V_res_near = lambda_near / (ChirpLoop * (Tidle + Tr_near) * 1e-6 * 2);
                }
                else if (this.BtnFarDetect.Checked)
                {
                    this.V_res_far = lambda_far / (ChirpLoop * (Tidle + Tr_far) * 1e-6 * 2);
                }
                else if (this.btnAllDetect.Checked)
                {
                    this.V_res_near = lambda_near / (ChirpLoop * (Tidle + Tr_near) * 1e-6 * 2 * 2);
                    this.V_res_far = lambda_far / (ChirpLoop * (Tidle + Tr_far) * 1e-6 * 2 * 2);
                }
                ParamsLog.Text = "参数格式正确";
            }
            catch (FormatException)
            {
                ParamsLog.Text = ("参数格式不正确！");
            }
            //显示
            if (this.BtnNearDetect.Checked || this.btnAllDetect.Checked)
            {
                this.B_nearBox.Text = (this.B_near / 1e6).ToString("F2");
                this.R_res_nearBox.Text = (this.R_res_near * 1e2).ToString("F2");
                this.V_res_nearBox.Text = this.V_res_near.ToString("F2");
                this.S_nearBox.Text = this.S_near.ToString("F2");
            }
            if (this.BtnFarDetect.Checked || this.btnAllDetect.Checked)
            {
                this.B_farBox.Text = (this.B_far / 1e6).ToString("F2");
                this.R_res_farBox.Text = (this.R_res_far * 1e2).ToString("F2");
                this.V_res_farBox.Text = this.V_res_far.ToString("F2");
                this.S_farBox.Text = this.S_far.ToString("F2");
            }
            //存储
            Properties.Settings.Default.Tidle = this.Tidle;
            Properties.Settings.Default.Fs = this.Fs;
            Properties.Settings.Default.ChirpLoop = this.ChirpLoop;
            Properties.Settings.Default.FrameT = this.FrameT;
            Properties.Settings.Default.K_near = this.K_near;
            Properties.Settings.Default.Sample_near = this.Sample_near;
            Properties.Settings.Default.Tr_near = this.Tr_near;
            Properties.Settings.Default.K_far = this.K_far;
            Properties.Settings.Default.Sample_far = this.Sample_far;
            Properties.Settings.Default.Tr_far = this.Tr_far;
            Properties.Settings.Default.ADC_start_time = this.ADC_start_time;
            Properties.Settings.Default.StartFreq = this.StartFreq;
            Properties.Settings.Default.Save();
        }

        private void ClosedParams_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
