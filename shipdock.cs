using System.Reflection.Metadata;
using System.Windows.Forms;
using System.IO.Ports;
using static System.Windows.Forms.DataFormats;
using System.Threading.Channels;
using System.Text;

namespace shipdock
{
    public partial class shipdock : Form
    {
        private static byte[] userbuffer = new byte[1024]; // 设置一个字节缓冲区
        private static int userbufferIndex = 0;
        private static byte[] databuffer = new byte[1024]; // 设置一个字节缓冲区
        private static int databufferIndex = 0;
        private static bool IsLogFolderExist = false;
        private StreamWriter logWriter;
        private bool isLogWriterOpen = false;
        private Bitmap traBitmap;
        private Graphics traGraphics;
        private PointF rightTop, leftBottom;
        private float axisX, axisY;
        private SerialPort userPort = new SerialPort();
        private SerialPort dataPort = new SerialPort();
        //端口相关变量
        private string userPortCLI;
        private int userPortWait = 100;
        //程序启动
        private bool IsChangeStart = false;
        private enum LogLevel
        {
            Info,
            Warning,
            Error
        }
        public shipdock()
        {
            InitializeComponent();
        }
        // 加载窗体
        private void shipdock_Load(object sender, EventArgs e)
        {
            //时间标注
            this.ProgramStart.Text = "系统启动时间为:  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //文件路径
            this.tbLogPath.Text = Properties.Settings.Default.LogPath;
            this.tbDataPath.Text = Properties.Settings.Default.DataPath;
            this.tbCfgPath.Text = Properties.Settings.Default.CfgPath;
            if (this.IsLog.Checked)
            {
                if (string.IsNullOrWhiteSpace(this.tbLogPath.Text))
                {
                    this.tbLogPath.Text = Path.GetFullPath("../log/");
                }
                try
                {
                    Directory.CreateDirectory(this.tbLogPath.Text);
                    IsLogFolderExist = true;
                    string LogPathName;
                    if (!this.tbLogPath.Text.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                    {
                        LogPathName = this.tbLogPath.Text + System.IO.Path.DirectorySeparatorChar + "Log" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                    }
                    else
                    {
                        LogPathName = this.tbLogPath.Text + "Log" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                    }
                    logWriter = new StreamWriter(LogPathName, true);  // true 表示追加写入
                    isLogWriterOpen = true;
                    UpdateLog("日志已启动", LogLevel.Info);
                }
                catch (Exception ex)
                {
                    // 捕获异常并输出错误信息
                    UpdateLog(ex.Message, LogLevel.Error);
                    IsLogFolderExist = false;
                    this.IsLog.Checked = false;
                    this.IsLog.ForeColor = System.Drawing.Color.Gray;
                }
            }
            if (string.IsNullOrWhiteSpace(this.tbDataPath.Text))
            {
                this.tbDataPath.Text = Path.GetFullPath("../../../data/");
                try
                {
                    if (!Directory.Exists(this.tbDataPath.Text))
                    {
                        // 创建文件夹
                        Directory.CreateDirectory(this.tbDataPath.Text);
                        UpdateLog("数据文件夹创建成功", LogLevel.Info);
                    }
                    else
                    {
                        UpdateLog("数据文件夹已存在", LogLevel.Info);
                    }

                }
                catch (Exception ex)
                {
                    // 捕获异常并输出错误信息
                    UpdateLog(ex.Message, LogLevel.Error);
                }
            }
            if (string.IsNullOrWhiteSpace(this.tbCfgPath.Text))
            { 
                this.tbCfgPath.Text = Path.GetFullPath("../../../Properties/default.cfg");
            }
            this.cbUserBaudRate.Text = "115200";
            this.cbDataBaudRate.Text = "921600";
            //添加中断函数
            userPort.DataReceived += new SerialDataReceivedEventHandler(UserPort_DataReceived);
            //相关按钮disabled
            this.btnSendParam.Enabled = false;
            this.btnSendParam.ForeColor = System.Drawing.Color.Gray;
            this.btnStartLadar.Enabled = false;
            this.btnStartLadar.ForeColor = System.Drawing.Color.Gray;
            this.btnStopLadar.Enabled = false;
            this.btnStopLadar.ForeColor = System.Drawing.Color.Gray;

            //画图初始化
            traBitmap = new Bitmap(pbTrajectory.Size.Width, pbTrajectory.Size.Height);
            traGraphics = Graphics.FromImage(traBitmap);
            traGraphics.Clear(Color.WhiteSmoke);

            leftBottom = new PointF(30, traBitmap.Height - 30);
            rightTop = new PointF(traBitmap.Width - 30, 30);
            PointF CenterP = new PointF((leftBottom.X + rightTop.X) / 2, leftBottom.Y);

            //画x轴
            traGraphics.DrawLine(Pens.Black, leftBottom.X, leftBottom.Y, rightTop.X, leftBottom.Y);
            PointF[] xpt = new PointF[3] { new PointF(rightTop.X + 8, leftBottom.Y), new PointF(rightTop.X, leftBottom.Y + 4), new PointF(rightTop.X, leftBottom.Y - 4) };//x轴三角形
            traGraphics.DrawPolygon(Pens.Black, xpt);
            traGraphics.FillPolygon(new SolidBrush(Color.Black), xpt);
            traGraphics.DrawString("切向/m", new Font("宋体", 10), Brushes.Black, new PointF(traBitmap.Width - 60, leftBottom.Y - 20));
            //画y轴
            traGraphics.DrawLine(Pens.Black, CenterP.X, CenterP.Y, CenterP.X, rightTop.Y);
            PointF[] ypt = new PointF[3] { new PointF(CenterP.X, rightTop.Y - 8), new PointF(CenterP.X - 4, rightTop.Y), new PointF(CenterP.X + 4, rightTop.Y) };//y轴三角形
            traGraphics.DrawPolygon(Pens.Black, ypt);
            traGraphics.FillPolygon(new SolidBrush(Color.Black), ypt);
            traGraphics.DrawString("径向/m", new Font("宋体", 10), Brushes.Black, new PointF(CenterP.X + 9, rightTop.Y - 10));
            //标记刻度
            axisX = (rightTop.X - leftBottom.X) / 1020;
            axisY = (leftBottom.Y - rightTop.Y) / 510;
            for (int i = 1; i <= 10; i++)
            {
                traGraphics.DrawString((i * 50).ToString(), new Font("宋体", 9), Brushes.Black, new PointF(CenterP.X - 25, CenterP.Y - i * 50 * axisY - 5));
                traGraphics.DrawLine(Pens.Black, CenterP.X - 3, CenterP.Y - i * 50 * axisY, CenterP.X, CenterP.Y - i * 50 * axisY);
            }
            for (int i = -5; i <= 5; i++)
            {
                if (i != 0)
                {
                    traGraphics.DrawString((i * 100).ToString(), new Font("宋体", 9), Brushes.Black, new PointF(leftBottom.X + (10 + (i + 5) * 100) * axisX - 10, leftBottom.Y + 6));
                    traGraphics.DrawLine(Pens.Black, leftBottom.X + (10 + (i + 5) * 100) * axisX, leftBottom.Y - 3, leftBottom.X + (10 + (i + 5) * 100) * axisX, leftBottom.Y);
                }
            }
            pbTrajectory.Image = traBitmap;
        }
        private void shipdock_Close(object sender, FormClosingEventArgs e)
        {
            var confirmForm = new Form();
            confirmForm.StartPosition = FormStartPosition.CenterParent;  // 设置在父窗体中央显示
            var result = MessageBox.Show(this, "确定要退出吗?", "退出确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                // 取消窗体关闭操作
                e.Cancel = true;
            }
        }
        private void IsLog_CheckedChanged(object sender, EventArgs e)
        {
            // 启用或禁用指定的控件
            tbLogPath.Enabled = !IsLog.Checked;
            btnLog.Enabled = !IsLog.Checked;

            // 可选：根据状态改变控件的样式或提示信息
            if (IsLog.Checked)
            {
                IsLog.ForeColor = System.Drawing.Color.Gray;
                tbLogPath.BackColor = System.Drawing.Color.LightGray;
                try
                {
                    // 创建文件夹
                    Directory.CreateDirectory(this.tbLogPath.Text);
                    UpdateLog("日志文件夹已创建，可进行日志的记录", LogLevel.Info);
                    IsLogFolderExist = true;
                }
                catch (Exception ex)
                {
                    // 捕获异常并输出错误信息
                    UpdateLog(ex.Message, LogLevel.Error);
                    IsLogFolderExist = false;
                    this.IsLog.Checked = false;
                    this.IsLog.ForeColor = System.Drawing.Color.Gray;
                    return;
                }
                string LogPathName;
                if (!this.tbLogPath.Text.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                {
                    LogPathName = this.tbLogPath.Text + System.IO.Path.DirectorySeparatorChar + "Log" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                }
                else
                {
                    LogPathName = this.tbLogPath.Text + "Log" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                }
                logWriter = new StreamWriter(LogPathName, true);  // true 表示追加写入
                isLogWriterOpen = true;
                UpdateLog("启动日志系统", LogLevel.Info);
            }
            else
            {
                // 禁用控件时，更新提示信息或控件样式
                tbLogPath.BackColor = System.Drawing.Color.White;
                IsLog.ForeColor = System.Drawing.Color.Black;
                UpdateLog("关闭日志系统", LogLevel.Info);
                if (isLogWriterOpen)
                {
                    isLogWriterOpen = false;
                    logWriter.Close();
                }
            }
        }

        //更新Log
        private void UpdateLog(string message, LogLevel loglevel)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            message = message.Trim() + $"{"     "}[{timestamp}]{Environment.NewLine}";
            message = $"[{loglevel}]   " + message;
            // 检查是否在 UI 线程
            if (Log.InvokeRequired)
            {
                // 如果在非 UI 线程，使用 Invoke 切换到 UI 线程
                Log.Invoke(new Action<string, LogLevel>(UpdateLog), new object[] { message, loglevel });
            }
            else
            {
                // 在 UI 线程中更新日志到 RichTextBox
                if (loglevel == LogLevel.Info)
                {
                    Log.SelectionColor = System.Drawing.Color.Black;
                    Log.AppendText(message);
                }
                if (loglevel == LogLevel.Error)
                {
                    Log.SelectionColor = System.Drawing.Color.Red;
                    Log.AppendText(message);
                }
                if (loglevel == LogLevel.Warning)
                {
                    Log.SelectionColor = System.Drawing.Color.Orange;
                    Log.AppendText(message);
                }
                // 将日志同时写入 txt 文件
                if (isLogWriterOpen && IsLogFolderExist)
                {
                    logWriter.WriteLine(message);
                    logWriter.Flush();  // 确保实时写入文件
                }
            }
            Log.ScrollToCaret();
        }
        //标记船只
        private void DrawChart(Graphics graphics, PointF TruePosition, float radius, Brush brush)
        {
            PointF center = new PointF((TruePosition.X + 500) * axisX + leftBottom.X, leftBottom.Y - TruePosition.Y * axisY);
            PointF[] points = new PointF[10];
            double angle = Math.PI / 5; // 36° in radians

            for (int i = 0; i < 5; i++)
            {
                // 外圈顶点
                points[i * 2] = new PointF(
                    center.X + (float)(radius * Math.Cos(i * 2 * angle)),
                    center.Y - (float)(radius * Math.Sin(i * 2 * angle))
                );

                // 内圈顶点
                points[i * 2 + 1] = new PointF(
                    center.X + (float)(radius / 2 * Math.Cos(i * 2 * angle + angle)),
                    center.Y - (float)(radius / 2 * Math.Sin(i * 2 * angle + angle))
                );
            }
            graphics.FillPolygon(brush, points); // 填充五角星
        }
        private void tbLogPath_TextChanged(object sender, EventArgs e)
        {
            IsLogFolderExist = false;
            Properties.Settings.Default.LogPath = this.tbLogPath.Text;
            Properties.Settings.Default.Save();
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            IsLogFolderExist = false;
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "请选择存放日志的文件夹";

            folderDialog.SelectedPath = @"C:\";

            // 显示文件夹选择对话框
            DialogResult result = folderDialog.ShowDialog();

            // 如果用户选择了文件夹
            if (result == DialogResult.OK)
            {
                this.tbLogPath.Text = folderDialog.SelectedPath + @"\";
            }
        }

        private void btnData_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "请选择存放数据的文件夹";
            folderDialog.SelectedPath = @"C:\";
            // 显示文件夹选择对话框
            DialogResult result = folderDialog.ShowDialog();

            // 如果用户选择了文件夹
            if (result == DialogResult.OK)
            {
                this.tbDataPath.Text = folderDialog.SelectedPath + @"\";
            }
        }

        private void tbDataPath_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.DataPath = this.tbDataPath.Text;
            Properties.Settings.Default.Save();
        }

        private void btnSetParam_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.LogPath = this.tbLogPath.Text;
            Properties.Settings.Default.DataPath = this.tbDataPath.Text;
            Properties.Settings.Default.CfgPath = this.tbCfgPath.Text;
            Properties.Settings.Default.Save();
            if (string.IsNullOrEmpty(this.tbCfgPath.Text))
            {
                UpdateLog("参数文件路径为空", LogLevel.Error);
                return;
            }
            else if (!this.tbCfgPath.Text.EndsWith(".cfg", StringComparison.OrdinalIgnoreCase) && !this.tbCfgPath.Text.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                UpdateLog("指定参数文件错误,仅限于 .cfg或 .txt", LogLevel.Error);
                return;
            }
            // 创建一个新的窗体实例
            ParamForm paramForm = new ParamForm();
            // 显示新的窗体
            paramForm.Show();
        }

        private void btnSendParam_Click(object sender, EventArgs e)
        {
            if (!this.tbCfgPath.Text.EndsWith(".cfg", StringComparison.OrdinalIgnoreCase) && !this.tbCfgPath.Text.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                UpdateLog("指定参数文件错误,仅限于 .cfg或 .txt", LogLevel.Error);
                return;
            }
            using (StreamReader reader = new StreamReader(this.tbCfgPath.Text))
            {
                int NumDone = 0;
                int NumCLI = 0;
                IsChangeStart = true;
                // 逐行读取文件内容
                while ((userPortCLI = reader.ReadLine()) != null)
                {
                    // 如果行首是 "%"，则跳过这行
                    if (userPortCLI.StartsWith("%"))
                    {
                        continue;
                    }
                    NumCLI++;
                    // 发送有效的行（非以%开头）
                    if (SendCheck(userPort, userPortCLI, userPortWait, "Done"))
                    {                        
                        NumDone++;
                        if (userPortCLI == "sensorStart")
                        {
                            IsChangeStart = false;
                        }
                    }
                    else
                    {
                        if (userPortCLI == "sensorStart")
                        {
                            IsChangeStart = true;
                        }
                    }
                }
                if ((float)NumDone / NumCLI > 0.5)
                {
                    UpdateLog("指令发送成功，共发送了" + NumCLI.ToString() + "条指令，" + "共接收了" + NumDone.ToString() + "条Done", LogLevel.Info);
                }
                else
                {
                    UpdateLog("指令发送失败，共发送了" + NumCLI.ToString() + "条指令，" + "共接收了" + NumDone.ToString() + "条Done", LogLevel.Error);
                }
            }
        }

        private void btnConnectPort_Click(object sender, EventArgs e)
        {
            //设置用户端口和数据端口
            try
            {
                userPort.PortName = this.cbUserPort.Text;
                userPort.BaudRate = int.Parse(this.cbUserBaudRate.Text);
                dataPort.PortName = this.cbDataPort.Text;
                dataPort.BaudRate = int.Parse(this.cbDataBaudRate.Text);
                userPort.DtrEnable = false;
                userPort.RtsEnable = false;
                userPort.Open();
                UpdateLog("用户端口已打开", LogLevel.Info);
                dataPort.DtrEnable = false;
                dataPort.RtsEnable = false;
                dataPort.Open();
                UpdateLog("数据端口已打开", LogLevel.Info);
                //Thread CheckPortThread = new Thread(IsConnectPort);
                //CheckPortThread.Start();
                IsConnectPort();
            }
            catch (Exception ex)
            {
                UpdateLog(ex.Message, LogLevel.Error);
            }
        }
        //检测是否串口是否可以进行通信
        private void IsConnectPort()
        {
            this.btnConnectPort.Enabled = false;
            this.btnConnectPort.ForeColor = System.Drawing.Color.Gray;
            userPortCLI = "configDataPort " + this.cbDataBaudRate.Text + " 1";
            for (int i = 0; i < 5; i++)
            {
                if (SendCheck(userPort, userPortCLI, userPortWait, "Done"))
                {
                    this.btnConnectPort.Enabled = true;
                    this.btnConnectPort.ForeColor = System.Drawing.Color.Black;
                    this.btnSendParam.Enabled = true;
                    this.btnSendParam.ForeColor = System.Drawing.Color.Black;
                    this.btnStartLadar.Enabled = true;
                    this.btnStartLadar.ForeColor = System.Drawing.Color.Black;
                    this.btnStopLadar.Enabled = true;
                    this.btnStopLadar.ForeColor = System.Drawing.Color.Black;
                    UpdateLog("端口可进行正常通信", LogLevel.Info);
                    return;
                }
            }
            UpdateLog("端口不可进行正常通信", LogLevel.Error);
            this.btnConnectPort.Enabled = true;
            this.btnConnectPort.ForeColor = System.Drawing.Color.Black;
        }
        //用户端口接收中断
        private static void UserPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            // 获取接收到的数据的长度
            int bytesToRead = sp.BytesToRead;
            sp.Read(userbuffer, userbufferIndex, bytesToRead);
            userbufferIndex = userbufferIndex + bytesToRead;
        }
        private void btnClosePort_Click(object sender, EventArgs e)
        {
            try
            {
                userPort.Close();
                dataPort.Close();
                UpdateLog("串口已关闭", LogLevel.Info);
            }
            catch (Exception ex)
            {
                UpdateLog(ex.Message, LogLevel.Error);
            }
        }

        private void btncfgPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // 设置文件类型过滤器
            openFileDialog.Filter = "配置文件 (*.cfg)|*.cfg|文本文件 (*.txt)|*.txt";

            // 只允许选择一个文件
            openFileDialog.Multiselect = false;

            // 打开文件选择对话框
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.tbCfgPath.Text = openFileDialog.FileName;
            }
        }

        private void btnStopLadar_Click(object sender, EventArgs e)
        {
            userPortCLI = "sensorStop";
            if (SendCheck(userPort, userPortCLI, userPortWait, "Done"))
            {
                UpdateLog("雷达成功关闭", LogLevel.Info);
            }
        }

        private void tbCfgPath_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.CfgPath = this.tbCfgPath.Text;
            Properties.Settings.Default.Save();
        }

        private void btnStartLadar_Click(object sender, EventArgs e)
        {
            if(IsChangeStart)
            {
                userPortCLI = "sensorStart";
                IsChangeStart = false;
            }
            else
            {
                userPortCLI = "sensorStart 0";
            }
            if (SendCheck(userPort, userPortCLI, userPortWait, "Done"))
            {
                UpdateLog("雷达成功开启", LogLevel.Info);
            }
        }

        private void btnRefreshPort_Click(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            cbUserPort.Items.Clear();
            cbDataPort.Items.Clear();
            if (ports.Length > 0)
            {
                cbUserPort.Items.AddRange(ports);

                cbDataPort.Items.AddRange(ports);
                UpdateLog("端口可用数量:" + ports.Length.ToString(), LogLevel.Info);
            }
            else
            {
                UpdateLog("没有可用的端口！", LogLevel.Warning);
            }
        }
        private bool SendCheck(SerialPort port, string command, int WaitTime, string expectedReply)
        {
            bool result = false;
            if (!port.IsOpen)
            {
                return result;
            }
            port.WriteLine(command + " \n");
            Thread.Sleep(WaitTime);
            //如果没有收到信息，再次进行等待
            if (userbufferIndex == 0)
            {
                Thread.Sleep(WaitTime);
            }
            if (userbufferIndex > 0)
            {
                byte[] receiveData = new byte[userbufferIndex];
                Array.Copy(userbuffer, receiveData, userbufferIndex);
                string reveive = JudgeReceive(Encoding.ASCII.GetString(receiveData)).Trim();
                if (reveive.Contains(expectedReply))
                {
                    result = true;
                }
                else
                {
                    UpdateLog(reveive, LogLevel.Error);
                    //UpdateLog(Encoding.ASCII.GetString(receiveData), LogLevel.Error);
                    result = false;
                }
                userbufferIndex = 0;
            }
            else
            {
                UpdateLog(command + "---无回复", LogLevel.Error);
                result = false;
            }
            return result;
        }
        //判断返回信息
        private string JudgeReceive(string receivedata)
        {
            receivedata = receivedata.Replace("mmwDemo:/>", string.Empty); // 删除特定字符串
            // 按照换行符拆分数据
            string[] lines = receivedata.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            // 用 "---" 拼接每一行
            string result = string.Join("---", lines);
            // 去掉前后空白，并返回结果
            result = result.Trim();
            return result;
        }
    }
}