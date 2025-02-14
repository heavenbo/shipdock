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
        int DetectModel;
        //雷达波形参数
        private double StartFreq;
        private double Tidle;
        private double Fs;
        private double ChirpLoop;
        private double FrameT;
        private double NearK;
        private double SampleNear;
        private double TrNear;
        private double FarK;
        private double SampleFar;
        private double TrFar;
        private double StartADC;
        private double RxGain;
        private double TxStart;
        //性能指标
        private double NearB, FarB;
        private double RresNear, RresFar;
        private double VresNear, VresFar;
        private double NearS, FarS;
        public ParamForm()
        {
            InitializeComponent();
        }

        private void ParamForm_Load(object sender, EventArgs e)
        {
            this.DetectModel = Properties.Settings.Default.DetectModel;
            if (this.DetectModel != 0)
            {
                this.btnNearDetect.Checked = DetectModel == 1;
                this.btnFarDetect.Checked = DetectModel == 2;
                this.btnAllDetect.Checked = DetectModel == 3;
            }
            this.tbTidle.Text = Properties.Settings.Default.Tidle.ToString();
            this.tbFs.Text = Properties.Settings.Default.Fs.ToString();
            this.tbChirpLoop.Text = Properties.Settings.Default.ChirpLoop.ToString();
            this.tbFrameT.Text = Properties.Settings.Default.FrameT.ToString();
            this.tbKnear.Text = Properties.Settings.Default.NearK.ToString();
            this.tbSampleNear.Text = Properties.Settings.Default.SampleNear.ToString();
            this.tbTrNear.Text = Properties.Settings.Default.TrNear.ToString();
            this.tbKfar.Text = Properties.Settings.Default.FarK.ToString();
            this.tbSampleFar.Text = Properties.Settings.Default.SampleFar.ToString();
            this.tbTrFar.Text = Properties.Settings.Default.TrFar.ToString();
            this.tbStartADC.Text = Properties.Settings.Default.StartADC.ToString();
            this.tbStartFreq.Text = Properties.Settings.Default.StartFreq.ToString();
            this.tbTxStart.Text = Properties.Settings.Default.TxStart.ToString();
            this.tbRxGain.Text = Properties.Settings.Default.RxGain.ToString();
        }
        private void btnNearDetect_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.btnNearDetect.Checked)
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
                    control.Text = "";
                    control.BackColor = System.Drawing.Color.LightGray;
                    control.Enabled = false;
                }
            }
        }

        private void btnFarDetect_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.btnFarDetect.Checked)
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
                    control.Text = "";
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
        private void tbTidle_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbTidle.Text))
                return;
            char lastChar = tbTidle.Text[tbTidle.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(tbTidle.Text, out double result))
            {
                this.tbTidle.Text = string.Empty;  // 清空文本
            }
        }

        private void tbChirpLoop_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbChirpLoop.Text))
                return;
            char lastChar = tbChirpLoop.Text[tbChirpLoop.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            // 判断文本内容是否可以转换为有效的 double 类型
            if (!double.TryParse(this.tbChirpLoop.Text, out double result))
            {
                this.tbChirpLoop.Text = string.Empty;  // 清空文本
            }
        }

        private void tbFrameT_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbFrameT.Text))
                return;
            char lastChar = tbFrameT.Text[tbFrameT.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(this.tbFrameT.Text, out double result))
            {
                this.tbFrameT.Text = string.Empty;  // 清空文本
            }
        }

        private void tbFs_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbFs.Text))
                return;
            char lastChar = tbFs.Text[tbFs.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(this.tbFs.Text, out double result))
            {
                this.tbFs.Text = string.Empty;  // 清空文本
            }
        }
        private void tbStartFreq_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbStartFreq.Text))
                return;
            char lastChar = tbStartFreq.Text[tbStartFreq.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(this.tbStartFreq.Text, out double result))
            {
                this.tbStartFreq.Text = string.Empty;  // 清空文本
            }
        }
        private void tbKnear_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbKnear.Text))
                return;
            char lastChar = tbKnear.Text[tbKnear.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(this.tbKnear.Text, out double result))
            {
                this.tbKnear.Text = string.Empty;  // 清空文本
            }
        }

        private void tbKfar_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbKfar.Text))
                return;
            char lastChar = tbKfar.Text[tbKfar.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(this.tbKfar.Text, out double result))
            {
                this.tbKfar.Text = string.Empty;  // 清空文本
            }
        }

        private void tbSampleNear_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbSampleNear.Text))
                return;
            char lastChar = tbSampleNear.Text[tbSampleNear.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(this.tbSampleNear.Text, out double result))
            {
                this.tbSampleNear.Text = string.Empty;  // 清空文本
            }
        }

        private void tbSampleFar_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbSampleFar.Text))
                return;
            char lastChar = tbSampleFar.Text[tbSampleFar.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(this.tbSampleFar.Text, out double result))
            {
                this.tbSampleFar.Text = string.Empty;  // 清空文本
            }
        }

        private void tbTrNear_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbTrNear.Text))
                return;
            char lastChar = tbTrNear.Text[tbTrNear.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(this.tbTrNear.Text, out double result))
            {
                this.tbTrNear.Text = string.Empty;  // 清空文本
            }
        }

        private void tbTrFar_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbTrFar.Text))
                return;
            char lastChar = tbTrFar.Text[tbTrFar.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(this.tbTrFar.Text, out double result))
            {
                this.tbTrFar.Text = string.Empty;  // 清空文本
            }
        }
        private void tbStartADC_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbStartADC.Text))
                return;
            char lastChar = tbStartADC.Text[tbStartADC.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(this.tbStartADC.Text, out double result))
            {
                this.tbStartADC.Text = string.Empty;  // 清空文本
            }
        }
        private void tbTxStart_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbTxStart.Text))
                return;
            char lastChar = tbTxStart.Text[tbTxStart.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(this.tbTxStart.Text, out double result))
            {
                this.tbTxStart.Text = string.Empty;  // 清空文本
            }
        }
        private void tbRxGain_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbRxGain.Text))
                return;
            char lastChar = tbRxGain.Text[tbRxGain.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(this.tbRxGain.Text, out double result))
            {
                this.tbRxGain.Text = string.Empty;  // 清空文本
            }
        }
        private void btnSetParams_Click(object sender, EventArgs e)
        {
            try
            {
                this.DetectModel = (this.btnNearDetect.Checked ? 1 : 0) + (this.btnFarDetect.Checked ? 2 : 0) + (this.btnAllDetect.Checked ? 3 : 0);
                this.Tidle = double.Parse(this.tbTidle.Text);
                this.Fs = double.Parse(this.tbFs.Text);
                this.ChirpLoop = double.Parse(this.tbChirpLoop.Text);
                this.FrameT = double.Parse(this.tbFrameT.Text);
                this.NearK = double.Parse(this.tbKnear.Text);
                this.SampleNear = double.Parse(this.tbSampleNear.Text);
                this.TrNear = double.Parse(this.tbTrNear.Text);
                this.FarK = double.Parse(this.tbKfar.Text);
                this.SampleFar = double.Parse(this.tbSampleFar.Text);
                this.TrFar = double.Parse(this.tbTrFar.Text);
                this.StartADC = double.Parse(this.tbStartADC.Text);
                this.StartFreq = double.Parse(this.tbStartFreq.Text);
                this.RxGain = double.Parse(this.tbRxGain.Text);
                this.TxStart = double.Parse(this.tbTxStart.Text);
                //计算性能指标

                double EffBwNear = this.NearK * 1e12 * this.SampleNear / (Fs * 1e3);
                double lambda_near = SpeedOfLight / (StartFreq * 1e9 + EffBwNear / 2);
                this.NearB = this.NearK * this.TrNear;
                this.RresNear = SpeedOfLight / (2 * EffBwNear);
                this.NearS = this.SampleNear * this.RresNear;

                double EffBwFar = this.FarK * 1e12 * this.SampleFar / (Fs * 1e3);
                double LambdaFar = SpeedOfLight / (StartFreq * 1e9 + EffBwFar / 2);
                this.FarB = this.FarK * this.TrFar;
                this.RresFar = SpeedOfLight / (2 * EffBwFar);
                this.FarS = this.SampleFar * this.RresFar;
                if (this.btnNearDetect.Checked || this.btnAllDetect.Checked)
                {
                    this.VresNear = lambda_near / (ChirpLoop * (Tidle + TrNear) * 1e-6 * 2);
                }
                else if (this.btnFarDetect.Checked || this.btnAllDetect.Checked)
                {
                    this.VresFar = LambdaFar / (ChirpLoop * (Tidle + TrFar) * 1e-6 * 2);
                }
            }
            catch (FormatException)
            {
                ParamsLog.Text = ("[Error] 参数格式不正确！");
            }
            //进行参数设置的检验
            if (this.Tidle < this.TxStart + 6)
            {
                ParamsLog.Text = ("[Error] chirp间隔应至少大于Tx开始时间6μs！");
                return;
            }
            else if (this.FrameT * 1e3 <= (this.TrFar + this.Tidle) * this.ChirpLoop && this.DetectModel == 2)
            {
                ParamsLog.Text = ("[Error] 帧时间小于（chirp时间+chirp间隔）*chirp数！");
                return;
            }
            else if (this.FrameT * 1e3 <= (this.TrNear + this.Tidle) * this.ChirpLoop && this.DetectModel == 1)
            {
                ParamsLog.Text = ("[Error] 帧时间小于（chirp时间+chirp间隔）*chirp数！");
                return;
            }
            else if (this.FrameT * 1e3 <= (this.TrFar + this.Tidle + this.TrNear + this.Tidle) * this.ChirpLoop / 2 && this.btnAllDetect.Checked)
            {
                ParamsLog.Text = ("[Error] 帧时间小于（chirp时间+chirp间隔）*chirp数！");
                return;
            }
            else if (this.TrFar <= this.SampleFar / this.Fs * 1e3 + this.StartADC && this.DetectModel != 1)
            {
                ParamsLog.Text = ("[Error] chirp时间小于(chirp采样点数/采样频率)+ADC开始时间！");
                return;
            }
            else if (this.TrNear <= this.SampleNear / this.Fs * 1e3 + this.StartADC && this.DetectModel != 2)
            {
                ParamsLog.Text = ("[Error] chirp时间小于(chirp采样点数/采样频率)+ADC开始时间！");
                return;
            }
            else if (this.StartFreq < 77 || this.StartFreq + this.FarB / 1e3 > 81 || this.StartFreq + this.NearB / 1e3 > 81)
            {
                ParamsLog.Text = ("[Error] 超出雷达频率范围（77GHz,81GHz）");
                return;
            }
            else if ((int)this.ChirpLoop % 2 == 1 || ChirpLoop > 256 || ChirpLoop < 0)
            {
                ParamsLog.Text = ("[Error] chirp数应为偶数，并且应小于256");
                return;
            }
            //显示
            if (this.btnNearDetect.Checked || this.btnAllDetect.Checked)
            {
                this.tbBnear.Text = (this.NearB / 1e6).ToString("F2");
                this.tbRresNear.Text = (this.RresNear * 1e2).ToString("F2");
                this.tbVresNear.Text = this.VresNear.ToString("F2");
                this.tbSnear.Text = this.NearS.ToString("F2");
            }
            if (this.btnFarDetect.Checked || this.btnAllDetect.Checked)
            {
                this.tbBfar.Text = (this.FarB / 1e6).ToString("F2");
                this.tbRresFar.Text = (this.RresFar * 1e2).ToString("F2");
                this.tbVresFar.Text = this.VresFar.ToString("F2");
                this.tbSfar.Text = this.FarS.ToString("F2");
            }
            bool Iscorrect = true;
            foreach (Control control in this.NearPerfStatsBox.Controls)
            {
                if (control is System.Windows.Forms.TextBox && control.Text == "NaN")
                {
                    Iscorrect = false;
                    break;
                }
            }
            foreach (Control control in this.FarPerfStatsBox.Controls)
            {
                if (control is System.Windows.Forms.TextBox && control.Text == "NaN")
                {
                    Iscorrect = false;
                    break;
                }
            }
            if (!Iscorrect)
            {
                ParamsLog.Text = ("性能指标计算错误！");
            }
            else
            {
                ParamsLog.Text = ("参数设置成功！");
                //存储
                Properties.Settings.Default.DetectModel = this.DetectModel;
                Properties.Settings.Default.Tidle = this.Tidle;
                Properties.Settings.Default.Fs = this.Fs;
                Properties.Settings.Default.ChirpLoop = this.ChirpLoop;
                Properties.Settings.Default.FrameT = this.FrameT;
                Properties.Settings.Default.NearK = this.NearK;
                Properties.Settings.Default.SampleNear = this.SampleNear;
                Properties.Settings.Default.TrNear = this.TrNear;
                Properties.Settings.Default.FarK = this.FarK;
                Properties.Settings.Default.SampleFar = this.SampleFar;
                Properties.Settings.Default.TrFar = this.TrFar;
                Properties.Settings.Default.StartADC = this.StartADC;
                Properties.Settings.Default.StartFreq = this.StartFreq;
                Properties.Settings.Default.Save();
                //设置cfg文件
                Writecfg();
            }
        }
        private void Writecfg()
        {
            File.WriteAllText(Properties.Settings.Default.CfgPath, string.Empty);
            StreamWriter cfgwriter = new StreamWriter(Properties.Settings.Default.CfgPath, append: true);
            cfgwriter.WriteLine("sensorStop ");
            cfgwriter.Flush();
            cfgwriter.WriteLine("flushCfg ");
            cfgwriter.Flush();
            cfgwriter.WriteLine("dfeDataOutputMode " + "1 ");
            cfgwriter.Flush();
            cfgwriter.WriteLine("channelCfg 15 1 0 ");
            cfgwriter.Flush();
            cfgwriter.WriteLine("adcCfg 2 1 ");
            cfgwriter.Flush();
            cfgwriter.WriteLine("adcbufCfg -1 0 1 1 1 ");
            cfgwriter.Flush();
            cfgwriter.WriteLine("lowPower 0 1 ");
            cfgwriter.Flush();
            cfgwriter.WriteLine($"profileCfg 0 {this.StartFreq} {this.Tidle} {this.StartADC} {this.TrNear} 0 0 {this.NearK} {this.TxStart} {this.SampleNear} {this.Fs} 0 0 {this.RxGain} ");
            cfgwriter.Flush();
            cfgwriter.WriteLine($"profileCfg 1 {this.StartFreq} {this.Tidle} {this.StartADC} {this.TrFar} 0 0 {this.FarK} {this.TxStart} {this.SampleFar} {this.Fs} 0 0 {this.RxGain} ");
            cfgwriter.Flush();
            cfgwriter.WriteLine("chirpCfg 0 0 0 0 0 0 0 1 ");
            cfgwriter.Flush();
            cfgwriter.WriteLine("chirpCfg 1 1 1 0 0 0 0 1 ");
            cfgwriter.Flush();
            if (this.DetectModel==1)
            {
                cfgwriter.WriteLine($"frameCfg 0 0 32 0 {this.FrameT} 1 0 ");
                cfgwriter.Flush();
            }
            else if (this.DetectModel == 2)
            {
                cfgwriter.WriteLine($"frameCfg 1 1 32 0 {this.FrameT} 1 0 ");
                cfgwriter.Flush();
            }
            else if (this.DetectModel == 3)
            {
                cfgwriter.WriteLine($"frameCfg 0 1 32 0 {this.FrameT} 1 0 ");
                cfgwriter.Flush();
            }
            cfgwriter.WriteLine("guiMonitor -1 1 1 0 0 0 0 ");
            cfgwriter.Flush();
            cfgwriter.WriteLine("cfarCfg -1 0 2 8 4 3 0 15.0 0 ");
            cfgwriter.Flush();
            cfgwriter.WriteLine("cfarCfg -1 1 0 4 2 3 1 15.0 0 ");
            cfgwriter.Flush();
            cfgwriter.WriteLine("multiObjBeamForming -1 1 0.5 ");
            cfgwriter.Flush();
            cfgwriter.WriteLine("calibDcRangeSig -1 0 -5 8 256 ");
            cfgwriter.Flush();
            cfgwriter.WriteLine("clutterRemoval -1 0 ");
            cfgwriter.Flush();
            cfgwriter.WriteLine("compRangeBiasAndRxChanPhase 0.0 1 0 1 0 1 0 1 0 1 0 1 0 1 0 1 0 1 0 1 0 1 0 1 0 ");
            cfgwriter.Flush();
            cfgwriter.WriteLine("measureRangeBiasAndRxChanPhase 0 1. 0.2 ");
            cfgwriter.Flush();
            cfgwriter.WriteLine("aoaFovCfg -1 -90 90 -90 90 ");
            cfgwriter.Flush();
            //前后删除10%的点
            cfgwriter.WriteLine("cfarFovCfg -1 8 72.84 ");
            cfgwriter.Flush();
            //前后删除10%的点
            cfgwriter.WriteLine("cfarFovCfg -1 1 -10.59 10.59 ");
            cfgwriter.Flush();
            cfgwriter.WriteLine("extendedMaxVelocity -1 0 ");
            cfgwriter.Flush();
            //cfgwriter.WriteLine("CQRxSatMonitor 0 3 11 121 0 ");
            //cfgwriter.Flush();
            //cfgwriter.WriteLine("CQSigImgMonitor 0 127 8 ");
            //cfgwriter.Flush();
            cfgwriter.WriteLine("analogMonitor 0 0 ");
            cfgwriter.Flush();
            cfgwriter.WriteLine("lvdsStreamCfg -1 0 0 0 ");
            cfgwriter.Flush();
            cfgwriter.WriteLine("calibData 0 0 0 ");
            cfgwriter.Flush();
            cfgwriter.Close();
        }
        private void ClosedParams_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
