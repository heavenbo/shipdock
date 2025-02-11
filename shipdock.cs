using System.Reflection.Metadata;
using System.Windows.Forms;
using System.IO.Ports;
using static System.Windows.Forms.DataFormats;

namespace shipdock
{
    public partial class shipdock : Form
    {
        private static byte[] userbuffer = new byte[1024]; // 设置一个字节缓冲区
        private static int userbufferIndex = 0;
        private static byte[] databuffer = new byte[1024]; // 设置一个字节缓冲区
        private static int databufferIndex = 0;
        private StreamWriter logWriter;
        private bool isLogWriterOpen = false;
        private Bitmap traBitmap;
        private Graphics traGraphics;
        private PointF rightTop, leftBottom;
        private float axisX, axisY;
        private SerialPort userPort = new SerialPort();
        private SerialPort dataPort = new SerialPort();
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
            this.logPath.Text = Properties.Settings.Default.LogPath;
            //this.firmwarePath.Text = Properties.Settings.Default.FirmwarePath;
            this.DataPath.Text = Properties.Settings.Default.DataPath;
            if (string.IsNullOrWhiteSpace(this.logPath.Text))
            {
                this.logPath.Text = Path.GetFullPath("../log/");
                try
                {
                    // 创建文件夹
                    Directory.CreateDirectory(this.logPath.Text);
                    UpdateLog("文件夹已创建: " + this.logPath.Text, LogLevel.Info);
                }
                catch (Exception ex)
                {
                    // 捕获异常并输出错误信息
                    UpdateLog(ex.Message, LogLevel.Error);
                }
            }
            if (string.IsNullOrWhiteSpace(this.DataPath.Text))
            {
                this.DataPath.Text = Path.GetFullPath("../../../data/");
                try
                {
                    // 创建文件夹
                    Directory.CreateDirectory(this.DataPath.Text);
                    UpdateLog("文件夹已创建: " + this.DataPath.Text, LogLevel.Info);
                }
                catch (Exception ex)
                {
                    // 捕获异常并输出错误信息
                    UpdateLog(ex.Message, LogLevel.Error);
                }
            }
            string[] ports = SerialPort.GetPortNames();

            // 清空之前的端口
            cbUserPort.Items.Clear();

            // 如果有可用的端口，添加到ComboBox
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
            this.cbUserBaudRate.Text = "115200";
            this.cbDataBaudRate.Text = "921600";
            this.ProgramStart.Text = "系统启动时间为:  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            userPort.DataReceived += new SerialDataReceivedEventHandler(UserPort_DataReceived);
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
        //private void btnfirmware_Click(object sender, EventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();

        //    // 设置对话框的标题和初始目录（可选）
        //    openFileDialog.Title = "选择固件文件";
        //    openFileDialog.InitialDirectory = @"C:\"; // 默认起始路径，可根据需要设置

        //    // 设置过滤器，只显示固件相关的文件（比如.bin，.hex等）
        //    openFileDialog.Filter = "固件文件|*.bin;*.hex|所有文件|*.*";

        //    // 如果用户选择了文件并点击了“打开”
        //    if (openFileDialog.ShowDialog() == DialogResult.OK)
        //    {
        //        // 获取所选文件的路径并设置到 TextBox 中
        //        firmwarePath.Text = openFileDialog.FileName;
        //    }
        //}

        //private void firmwarePath_TextChanged(object sender, EventArgs e)
        //{
        //    Properties.Settings.Default.FirmwarePath = this.firmwarePath.Text;
        //    Properties.Settings.Default.Save();
        //}

        private void IsLog_CheckedChanged(object sender, EventArgs e)
        {
            // 启用或禁用指定的控件
            logPath.Enabled = IsLog.Checked;
            btnLog.Enabled = IsLog.Checked;

            // 可选：根据状态改变控件的样式或提示信息
            if (IsLog.Checked)
            {
                // 启用控件时，更新提示信息或控件样式
                logPath.BackColor = System.Drawing.Color.White;
                IsLog.ForeColor = System.Drawing.Color.Black;
                string LogPathname = this.logPath.Text + "Log" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                logWriter = new StreamWriter(LogPathname, true);  // true 表示追加写入
                isLogWriterOpen = true;
                UpdateLog("启动日志系统", LogLevel.Info);
            }
            else
            {
                // 禁用控件时，更新提示信息或控件样式
                IsLog.ForeColor = System.Drawing.Color.Gray;
                logPath.BackColor = System.Drawing.Color.LightGray;
                UpdateLog("关闭日志系统", LogLevel.Info);
                isLogWriterOpen = false;
                logWriter.Close();

            }
        }

        //更新Log
        private void UpdateLog(string message, LogLevel loglevel)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            message = message + $"{"     "}[{timestamp}]{Environment.NewLine}";
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
                if(loglevel==LogLevel.Info)
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
                    Log.SelectionColor = System.Drawing.Color.Yellow;
                    Log.AppendText(message);
                }
                // 将日志同时写入 txt 文件
                if(isLogWriterOpen)
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
        private void logPath_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LogPath = this.logPath.Text;
            Properties.Settings.Default.Save();
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "请选择存放日志的文件夹";

            folderDialog.SelectedPath = @"C:\";

            // 显示文件夹选择对话框
            DialogResult result = folderDialog.ShowDialog();

            // 如果用户选择了文件夹
            if (result == DialogResult.OK)
            {
                this.logPath.Text = folderDialog.SelectedPath + @"\";
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


                this.DataPath.Text = folderDialog.SelectedPath + @"\";
            }
        }

        private void DataPath_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.DataPath = this.DataPath.Text;
            Properties.Settings.Default.Save();
        }

        private void btnSetParam_Click(object sender, EventArgs e)
        {
            // 创建一个新的窗体实例
            ParamForm paramForm = new ParamForm();

            // 显示新的窗体
            paramForm.Show();
        }

        private void btnSendParam_Click(object sender, EventArgs e)
        {

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
                userPort.Open();
                UpdateLog("用户端口已打开", LogLevel.Info);
                dataPort.Open();
                UpdateLog("数据端口已打开", LogLevel.Info);
                Thread CheckPortThread = new Thread(IsConnectPort);
                CheckPortThread.Start();
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
            for (int i = 0; i < 20; i++)
            {
                userPort.WriteLine("configDataPort " + this.cbDataBaudRate.Text + " 1");
                Thread.Sleep(50);
                if (userbufferIndex > 0)
                {
                    byte[] receiveData = new byte[userbufferIndex];
                    Array.Copy(userbuffer, receiveData, 10);
                    userbufferIndex = 0;
                    if (BitConverter.ToString(receiveData) == "done")
                    {
                        UpdateLog("端口可进行正常通信", LogLevel.Info);
                        this.btnConnectPort.Enabled = true;
                        this.btnConnectPort.ForeColor = System.Drawing.Color.Black;
                        break;
                    }
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
            sp.Read(userbuffer, 0, bytesToRead);
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
    }
}