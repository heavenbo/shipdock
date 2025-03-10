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
        private enum DetectModel
        {
            nearDetect = 1,
            farDetect,
            allDetect
        };
        private DetectModel detectModel;
        //雷达波形参数
        private double StartFreq;
        private double Tidle;
        private double Fs;
        private double Tr;
        private double NearFrameT, FarFrameT;
        private double NearK;
        private double SampleNear;
        private double ChirpLoopNear;
        private double FarK;
        private double SampleFar;
        private double ChirpLoopFar;
        private double StartADC = 6;
        private double RxGain;
        private double TxStart = 1;
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
            this.detectModel = (DetectModel)Properties.Settings.Default.DetectModel;
            if (this.detectModel != 0)
            {
                this.btnNearDetect.Checked = detectModel == DetectModel.nearDetect;
                this.btnFarDetect.Checked = detectModel == DetectModel.farDetect;
                this.btnAllDetect.Checked = detectModel == DetectModel.allDetect;
            }
            this.tbTidle.Text = Properties.Settings.Default.Tidle.ToString();
            this.tbFs.Text = Properties.Settings.Default.Fs.ToString();
            this.tbTr.Text = Properties.Settings.Default.Tr.ToString();
            this.tbNearFrameT.Text = Properties.Settings.Default.NearFrameT.ToString();
            this.tbFarFrameT.Text = Properties.Settings.Default.FarFrameT.ToString();
            this.tbKnear.Text = Properties.Settings.Default.NearK.ToString();
            this.tbSampleNear.Text = Properties.Settings.Default.SampleNear.ToString();
            this.tbChirpLoopNear.Text = Properties.Settings.Default.ChirpLoopNear.ToString();
            this.tbKfar.Text = Properties.Settings.Default.FarK.ToString();
            this.tbSampleFar.Text = Properties.Settings.Default.SampleFar.ToString();
            this.tbChirpLoopFar.Text = Properties.Settings.Default.ChirpLoopFar.ToString();
            this.tbStartFreq.Text = Properties.Settings.Default.StartFreq.ToString();
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

        private void tbTr_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbTr.Text))
                return;
            char lastChar = tbTr.Text[tbTr.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            // 判断文本内容是否可以转换为有效的 double 类型
            if (!double.TryParse(this.tbTr.Text, out double result))
            {
                this.tbTr.Text = string.Empty;  // 清空文本
            }
        }

        private void tbNearFrameT_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbNearFrameT.Text))
                return;
            char lastChar = tbNearFrameT.Text[tbNearFrameT.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(this.tbNearFrameT.Text, out double result))
            {
                this.tbNearFrameT.Text = string.Empty;  // 清空文本
            }
        }
        private void tbFarFrameT_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbFarFrameT.Text))
                return;
            char lastChar = tbFarFrameT.Text[tbFarFrameT.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(this.tbFarFrameT.Text, out double result))
            {
                this.tbFarFrameT.Text = string.Empty;  // 清空文本
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

        private void tbChirpLoopNear_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbChirpLoopNear.Text))
                return;
            char lastChar = tbChirpLoopNear.Text[tbChirpLoopNear.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(this.tbChirpLoopNear.Text, out double result))
            {
                this.tbChirpLoopNear.Text = string.Empty;  // 清空文本
            }
        }

        private void tbChirpLoopFar_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbChirpLoopFar.Text))
                return;
            char lastChar = tbChirpLoopFar.Text[tbChirpLoopFar.Text.Length - 1];
            if (lastChar == 'e' || lastChar == '-')
                return;
            if (!double.TryParse(this.tbChirpLoopFar.Text, out double result))
            {
                this.tbChirpLoopFar.Text = string.Empty;  // 清空文本
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
                this.detectModel = (DetectModel)((this.btnNearDetect.Checked ? 1 : 0) + (this.btnFarDetect.Checked ? 2 : 0) + (this.btnAllDetect.Checked ? 3 : 0));
                this.Tidle = double.Parse(this.tbTidle.Text);
                this.Fs = double.Parse(this.tbFs.Text);
                this.Tr = double.Parse(this.tbTr.Text);
                this.NearFrameT = double.Parse(this.tbNearFrameT.Text);
                this.FarFrameT = double.Parse(this.tbFarFrameT.Text);
                this.NearK = double.Parse(this.tbKnear.Text);
                this.SampleNear = double.Parse(this.tbSampleNear.Text);
                this.ChirpLoopNear = double.Parse(this.tbChirpLoopNear.Text);
                this.FarK = double.Parse(this.tbKfar.Text);
                this.SampleFar = double.Parse(this.tbSampleFar.Text);
                this.ChirpLoopFar = double.Parse(this.tbChirpLoopFar.Text);
                this.StartFreq = double.Parse(this.tbStartFreq.Text);
                this.RxGain = double.Parse(this.tbRxGain.Text);
                //计算性能指标

                double EffBwNear = this.NearK * 1e12 * this.SampleNear / (Fs * 1e3);
                double lambda_near = SpeedOfLight / (StartFreq * 1e9 + EffBwNear / 2);
                this.NearB = this.NearK * this.Tr;
                this.RresNear = SpeedOfLight / (2 * EffBwNear);
                this.NearS = this.SampleNear * this.RresNear;

                double EffBwFar = this.FarK * 1e12 * this.SampleFar / (Fs * 1e3);
                double LambdaFar = SpeedOfLight / (StartFreq * 1e9 + EffBwFar / 2);
                this.FarB = this.FarK * this.Tr;
                this.RresFar = SpeedOfLight / (2 * EffBwFar);
                this.FarS = this.SampleFar * this.RresFar;
                if (this.btnNearDetect.Checked || this.btnAllDetect.Checked)
                {
                    this.VresNear = lambda_near / (ChirpLoopNear * (Tidle + Tr) * 1e-6 * 2);
                }
                else if (this.btnFarDetect.Checked || this.btnAllDetect.Checked)
                {
                    this.VresFar = LambdaFar / (ChirpLoopFar * (Tidle + Tr) * 1e-6 * 2);
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
            else if (this.FarFrameT * 1e3 <= (this.Tr + this.Tidle) * this.ChirpLoopFar && this.detectModel != DetectModel.nearDetect)
            {
                ParamsLog.Text = ("[Error] 帧时间小于（chirp时间+chirp间隔）*chirp数！");
                return;
            }
            else if (this.NearFrameT * 1e3 <= (this.Tr + this.Tidle) * this.ChirpLoopNear && this.detectModel != DetectModel.farDetect)
            {
                ParamsLog.Text = ("[Error] 帧时间小于（chirp时间+chirp间隔）*chirp数！");
                return;
            }
            else if (this.Tr <= this.SampleFar / this.Fs * 1e3 + this.StartADC && this.detectModel != DetectModel.farDetect)
            {
                ParamsLog.Text = ("[Error] chirp时间小于(chirp采样点数/采样频率)+ADC开始时间！");
                return;
            }
            else if (this.Tr <= this.SampleNear / this.Fs * 1e3 + this.StartADC && this.detectModel != DetectModel.nearDetect)
            {
                ParamsLog.Text = ("[Error] chirp时间小于(chirp采样点数/采样频率)+ADC开始时间！");
                return;
            }
            else if (this.StartFreq < 77 || this.StartFreq + this.FarB / 1e3 > 81 || this.StartFreq + this.NearB / 1e3 > 81)
            {
                ParamsLog.Text = ("[Error] 超出雷达频率范围（77GHz,81GHz）");
                return;
            }
            else if ((int)this.ChirpLoopNear % 2 == 1 || ChirpLoopNear >= 256 || ChirpLoopNear < 16 || (int)this.ChirpLoopFar % 2 == 1 || ChirpLoopFar >= 256 || ChirpLoopFar < 16)
            {
                ParamsLog.Text = ("[Error] chirp数应为偶数，并且应在（16,156）之中");
                return;
            }
            else if (this.detectModel == DetectModel.allDetect && (this.SampleFar * this.ChirpLoopFar + this.ChirpLoopNear * this.SampleNear) * 4 > 32 * 1024)
            {
                ParamsLog.Text = ("[Error] 运行内存超过32K");
                return;
            }
            //显示
            if (this.btnNearDetect.Checked || this.btnAllDetect.Checked)
            {
                this.tbBnear.Text = (this.NearB).ToString("F2");
                this.tbRresNear.Text = (this.RresNear * 1e2).ToString("F2");
                this.tbVresNear.Text = this.VresNear.ToString("F2");
                this.tbSnear.Text = this.NearS.ToString("F2");
            }
            if (this.btnFarDetect.Checked || this.btnAllDetect.Checked)
            {
                this.tbBfar.Text = (this.FarB).ToString("F2");
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
                Properties.Settings.Default.DetectModel = (int)this.detectModel;
                Properties.Settings.Default.Tidle = this.Tidle;
                Properties.Settings.Default.Fs = this.Fs;
                Properties.Settings.Default.Tr = this.Tr;
                Properties.Settings.Default.NearFrameT = this.NearFrameT;
                Properties.Settings.Default.FarFrameT = this.FarFrameT;
                Properties.Settings.Default.NearK = this.NearK;
                Properties.Settings.Default.SampleNear = this.SampleNear;
                Properties.Settings.Default.ChirpLoopNear = this.ChirpLoopNear;
                Properties.Settings.Default.FarK = this.FarK;
                Properties.Settings.Default.SampleFar = this.SampleFar;
                Properties.Settings.Default.ChirpLoopFar = this.ChirpLoopFar;
                Properties.Settings.Default.StartADC = this.StartADC;
                Properties.Settings.Default.StartFreq = this.StartFreq;
                Properties.Settings.Default.TxStart = this.TxStart;
                Properties.Settings.Default.RxGain = this.RxGain;
                Properties.Settings.Default.Save();
                //设置cfg文件
                Writecfg();
            }
        }
        private void Writecfg()
        {
            File.WriteAllText(Properties.Settings.Default.CfgPath, string.Empty);
            StreamWriter cfgwriter = new StreamWriter(Properties.Settings.Default.CfgPath, append: true);
            if (this.detectModel == DetectModel.allDetect)
            {
                cfgwriter.WriteLine("%这是用于兼顾模式下的参数配置 ");
                cfgwriter.WriteLine("sensorStop ");
                cfgwriter.WriteLine("flushCfg ");
                cfgwriter.WriteLine("dfeDataOutputMode " + "3 ");
                cfgwriter.Flush();
            }
            else
            {
                if (this.detectModel == DetectModel.farDetect)
                {
                    cfgwriter.WriteLine("%这是用于远距离模式下的参数配置 ");
                }
                else
                {
                    cfgwriter.WriteLine("%这是用于近距离模式下的参数配置 ");
                }
                cfgwriter.WriteLine("sensorStop ");
                cfgwriter.WriteLine("flushCfg ");
                cfgwriter.WriteLine("dfeDataOutputMode " + "1 ");
                cfgwriter.Flush();
            }

            cfgwriter.WriteLine("channelCfg 15 1 0 ");
            cfgwriter.WriteLine("adcCfg 2 1 ");
            cfgwriter.WriteLine("adcbufCfg -1 0 1 1 1 ");
            cfgwriter.WriteLine("lowPower 0 0 ");
            cfgwriter.Flush();
            if (this.detectModel != DetectModel.farDetect)
            {
                cfgwriter.WriteLine($"profileCfg 0 {this.StartFreq} {this.Tidle} {this.StartADC} {this.Tr} 0 0 {this.NearK} {this.TxStart} {this.SampleNear} {this.Fs} 0 0 {this.RxGain} ");
                cfgwriter.Flush();
                cfgwriter.WriteLine("chirpCfg 0 0 0 0 0 0 0 1 ");
                cfgwriter.Flush();
            }
            else
            {
                cfgwriter.WriteLine($"profileCfg 0 {this.StartFreq} {this.Tidle} {this.StartADC} {this.Tr} 0 0 {this.FarK} {this.TxStart} {this.SampleFar} {this.Fs} 0 0 {this.RxGain} ");
                cfgwriter.Flush();
                cfgwriter.WriteLine("chirpCfg 0 0 0 0 0 0 0 1 ");
                cfgwriter.Flush();
            }
            if (this.detectModel == DetectModel.allDetect)
            {
                cfgwriter.WriteLine($"profileCfg 1 {this.StartFreq} {this.Tidle} {this.StartADC} {this.Tr} 0 0 {this.FarK} {this.TxStart} {this.SampleFar} {this.Fs} 0 0 {this.RxGain} ");
                cfgwriter.WriteLine("chirpCfg 1 1 1 0 0 0 0 1 ");
                cfgwriter.WriteLine("advFrameCfg 2 0 0 1 0 ");
                cfgwriter.WriteLine($"subFrameCfg 0 0 0 1 {this.ChirpLoopNear} {this.NearFrameT} 0 1 1 {this.NearFrameT} ");
                cfgwriter.WriteLine($"subFrameCfg 1 0 1 1 {this.ChirpLoopFar} {this.FarFrameT} 0 1 1 {this.FarFrameT} ");
                cfgwriter.WriteLine("guiMonitor 0 1 1 0 0 0 0 ");
                cfgwriter.WriteLine("guiMonitor 1 1 1 0 0 0 0 ");
                cfgwriter.WriteLine("cfarCfg 0 0 2 8 4 3 0 15.0 1 ");
                cfgwriter.WriteLine("cfarCfg 0 1 0 4 2 3 1 15.0 1 ");
                cfgwriter.WriteLine("cfarCfg 1 0 2 8 4 3 0 15.0 1 ");
                cfgwriter.WriteLine("cfarCfg 1 1 0 4 2 3 1 15.0 1 ");
                cfgwriter.WriteLine("multiObjBeamForming 0 1 0.5 ");
                cfgwriter.WriteLine("multiObjBeamForming 1 1 0.5 ");
                //int binNear = Math.Min((int)(SampleNear * 0.05),16);
                //cfgwriter.WriteLine($"calibDcRangeSig 0 0 {-binNear} {binNear} 256 ");
                cfgwriter.WriteLine($"calibDcRangeSig 0 0 -5 8 256 ");
                //int binFar = Math.Min((int)(SampleFar * 0.05), 16);
                cfgwriter.WriteLine($"calibDcRangeSig 1 0 -5 8 256 ");
                cfgwriter.WriteLine("aoaFovCfg -1 -90 90 -90 90 ");
                cfgwriter.WriteLine($"cfarFovCfg 0 0 {NearS * 0.1:F2}  {NearS - NearS * 0.1:F2} ");
                cfgwriter.WriteLine($"cfarFovCfg 1 0 {FarS * 0.1:F2}  {FarS - FarS * 0.1:F2} ");
                cfgwriter.WriteLine($"cfarFovCfg 0 1 -40.58 40.58 ");
                cfgwriter.WriteLine($"cfarFovCfg 1 1 -40.58 40.58 ");
                cfgwriter.WriteLine("CQRxSatMonitor  0   3   4   19  0 ");
                cfgwriter.WriteLine("CQSigImgMonitor 0   31  4 ");
                cfgwriter.WriteLine("CQRxSatMonitor  1   3   4   19  0 ");
                cfgwriter.WriteLine("CQSigImgMonitor 1   31  4 ");
                cfgwriter.Flush();
            }
            else
            {
                int chirploop = detectModel == DetectModel.nearDetect ? (int)ChirpLoopNear : (int)ChirpLoopFar;
                double frameT = detectModel == DetectModel.nearDetect ? NearFrameT : FarFrameT;
                cfgwriter.WriteLine($"frameCfg 0 0 {chirploop} 0 {frameT:F2} 1 0 ");
                cfgwriter.WriteLine("guiMonitor -1 1 1 1 0 0 0 ");
                cfgwriter.WriteLine("cfarCfg -1 0 2 8 4 3 0 15.0 1 ");
                cfgwriter.WriteLine("cfarCfg -1 1 0 4 2 3 1 15.0 1 ");
                cfgwriter.WriteLine("multiObjBeamForming -1 1 0.5 ");
                int NumPoint = this.detectModel == DetectModel.nearDetect ? (int)this.SampleFar : (int)this.SampleNear;
                //int binNum = Math.Min((int)(NumPoint * 0.05), 16);
                //cfgwriter.WriteLine($"calibDcRangeSig -1 0 {-binNum} {binNum} 256 ");
                cfgwriter.WriteLine("calibDcRangeSig -1 0 -5 8 256 ");
                cfgwriter.WriteLine("aoaFovCfg -1 -90 90 -90 90 ");
                double S = this.detectModel == DetectModel.nearDetect ? NearS : FarS;
                cfgwriter.WriteLine($"cfarFovCfg -1 0 {S * 0.1:F2}  {S - S * 0.1:F2} ");
                cfgwriter.WriteLine($"cfarFovCfg -1 1 -40.58 40.58 ");
                int profileType = this.detectModel == DetectModel.nearDetect ? 0 : 1;
                cfgwriter.WriteLine($"CQRxSatMonitor  {profileType}   3   4   19  0 ");
                cfgwriter.WriteLine($"CQSigImgMonitor {profileType}   31  4 ");
                cfgwriter.Flush();
            }
            cfgwriter.WriteLine("analogMonitor 0 0 ");
            cfgwriter.WriteLine("lvdsStreamCfg -1 0 0 0 ");

            cfgwriter.WriteLine("clutterRemoval -1 0 ");

            cfgwriter.WriteLine("compRangeBiasAndRxChanPhase 0.0 1 0 1 0 1 0 1 0 1 0 1 0 1 0 1 0 1 0 1 0 1 0 1 0 ");
            cfgwriter.WriteLine("measureRangeBiasAndRxChanPhase 0 1.5 0.2 ");

            cfgwriter.WriteLine("extendedMaxVelocity -1 0 ");
            cfgwriter.WriteLine("calibData 0 0 0 ");
            cfgwriter.Flush();
            cfgwriter.Close();
        }
        private void btnClosedParams_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
