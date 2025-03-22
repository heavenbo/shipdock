using System.Reflection.Metadata;
using System.Windows.Forms;
using System.IO.Ports;
using static System.Windows.Forms.DataFormats;
using System.Threading.Channels;
using System.Text;
using System.Timers;
using System.Threading;
using System.Diagnostics.Metrics;
using System.Buffers.Binary;
using System.Threading.Tasks;
using System.Collections;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using Microsoft.VisualBasic.Devices;
namespace shipdock
{
    public partial class shipdock : Form
    {
        //画图变量
        private Bitmap axisBitmap, pointBitmap, finalBitmap;
        private Graphics axisGraphics, pointGraphics, finalGraphics;
        private PointF pbRightTop, pbLeftBottom;
        private PointF RealRightTop, RealLeftBottom;
        private float axisX, axisY;
        private float axisXAdjusted, axisYAdjusted;
        bool isMouseInside = false;
        private float scaleFactor = 1.0f; // 缩放比例
        private const float zoomStep = 0.5f; // 每次缩放比例变化
        private bool isDragging = false;
        private Point lastMousePosition;
        PointF[] PointX = new PointF[11];
        PointF[] PointY = new PointF[10];
        private static Mutex bmpmutex = new Mutex();
        //日志相关变量
        private static bool IsLogFolderExist = false;
        private StreamWriter logWriter;
        private static bool isLogWriterOpen = false;
        //user端口相关变量
        private SerialPort userPort = new SerialPort();
        private static string userPortCLI;
        private static byte[] userbuffer = new byte[1024]; // 设置一个字节缓冲区
        private static int userbufferIndex = 0;
        private int userPortWait = 150;
        private static Mutex Usermutex = new Mutex();
        //数据端口相关变量
        private SerialPort dataPort = new SerialPort();
        static System.Timers.Timer datatimer;
        private static BinaryWriter dataWriter;
        private static byte[] databuffer = new byte[2 * 65536]; // 设置一个字节缓冲区
        private static int startDataIndex = 0;
        private static int endDataIndex = 0;
        //处理数据的变量
        Task backgroundTask;
        private List<byte> framebuffer = new List<byte>();
        private List<byte> frame = new List<byte>();
        List<PointF> drawPoint = new List<PointF>();
        uint frameIndex = 0;
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
            UpdateLog("系统启动时间为:  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LogLevel.Info);
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
            }
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
            if (string.IsNullOrWhiteSpace(this.tbCfgPath.Text))
            {
                this.tbCfgPath.Text = Path.GetFullPath("../../../Properties/default.cfg");
            }
            this.cbUserBaudRate.Text = "115200";
            this.cbDataBaudRate.Text = "921600";
            //添加中断函数
            userPort.DataReceived += new SerialDataReceivedEventHandler(UserPort_DataReceived);
            dataPort.DataReceived += new SerialDataReceivedEventHandler(DataPort_DataReceived);
            //设置定时器，定时触发
            datatimer = new System.Timers.Timer(50);  // 设置周期为50毫秒（0.1秒）
            datatimer.Elapsed += SaveData;  // 设置触发事件
            //相关按钮disabled
            this.btnSendParam.Enabled = false;
            this.btnSendParam.ForeColor = System.Drawing.Color.Gray;
            this.btnStartLadar.Enabled = false;
            this.btnStartLadar.ForeColor = System.Drawing.Color.Gray;
            this.btnStopLadar.Enabled = false;
            this.btnStopLadar.ForeColor = System.Drawing.Color.Gray;

            //画图初始化
            pointBitmap = new Bitmap(pbTrajectory.Size.Width, pbTrajectory.Size.Height);
            axisBitmap = new Bitmap(pbTrajectory.Size.Width, pbTrajectory.Size.Height);
            finalBitmap = new Bitmap(pbTrajectory.Size.Width, pbTrajectory.Size.Height);
            pointGraphics = Graphics.FromImage(pointBitmap);
            axisGraphics = Graphics.FromImage(axisBitmap);
            finalGraphics = Graphics.FromImage(finalBitmap);

            pbLeftBottom = new PointF(30, pointBitmap.Height - 30);
            pbRightTop = new PointF(pointBitmap.Width - 30, 30);
            RealRightTop = new PointF(510, 510);
            RealLeftBottom = new PointF(-510, 0);
            PointF CenterP = new PointF((pbLeftBottom.X + pbRightTop.X) / 2, pbLeftBottom.Y);

            //画x轴
            axisGraphics.DrawLine(Pens.Black, pbLeftBottom.X, pbLeftBottom.Y, pbRightTop.X, pbLeftBottom.Y);
            PointF[] xpt = new PointF[3] { new PointF(pbRightTop.X + 8, pbLeftBottom.Y), new PointF(pbRightTop.X, pbLeftBottom.Y + 4), new PointF(pbRightTop.X, pbLeftBottom.Y - 4) };//x轴三角形
            axisGraphics.DrawPolygon(Pens.Black, xpt);
            axisGraphics.FillPolygon(new SolidBrush(Color.Black), xpt);
            axisGraphics.DrawString("切向/m", new Font("宋体", 10), Brushes.Black, new PointF(pointBitmap.Width - 60, pbLeftBottom.Y - 20));
            //画y轴
            axisGraphics.DrawLine(Pens.Black, CenterP.X, CenterP.Y, CenterP.X, pbRightTop.Y);
            PointF[] ypt = new PointF[3] { new PointF(CenterP.X, pbRightTop.Y - 8), new PointF(CenterP.X - 4, pbRightTop.Y), new PointF(CenterP.X + 4, pbRightTop.Y) };//y轴三角形
            axisGraphics.DrawPolygon(Pens.Black, ypt);
            axisGraphics.FillPolygon(new SolidBrush(Color.Black), ypt);
            axisGraphics.DrawString("径向/m", new Font("宋体", 10), Brushes.Black, new PointF(CenterP.X + 9, pbRightTop.Y - 10));
            //标记刻度
            axisX = (RealRightTop.X - RealLeftBottom.X) / (pbRightTop.X - pbLeftBottom.X);
            axisY = (RealRightTop.Y - RealLeftBottom.Y) / (pbLeftBottom.Y - pbRightTop.Y);
            axisXAdjusted = axisX;
            axisYAdjusted = axisY;
            for (int i = 1; i <= 10; i++)
            {
                PointY[i - 1] = new PointF(CenterP.X + 5, CenterP.Y - i * 50 / axisY);
                axisGraphics.DrawString((i * 50).ToString(), new Font("宋体", 9), Brushes.Black, new PointF(PointY[i - 1].X, PointY[i - 1].Y - 5));
                axisGraphics.DrawLine(Pens.Black, CenterP.X - 3, CenterP.Y - i * 50 / axisY, CenterP.X, CenterP.Y - i * 50 / axisY);
            }
            for (int i = -5; i <= 5; i++)
            {
                PointX[i + 5] = new PointF(pbLeftBottom.X + (10 + (i + 5) * 100) / axisX, pbLeftBottom.Y + 6);
                if (i != 0)
                {
                    axisGraphics.DrawString((i * 100).ToString(), new Font("宋体", 9), Brushes.Black, new PointF(PointX[i + 5].X - 10, PointX[i + 5].Y));
                    axisGraphics.DrawLine(Pens.Black, pbLeftBottom.X + (10 + (i + 5) * 100) / axisX, pbLeftBottom.Y - 3, pbLeftBottom.X + (10 + (i + 5) * 100) / axisX, pbLeftBottom.Y);
                }
            }
            MergeLayers();
            //缩放功能
            pbTrajectory.MouseEnter += (s, e) => isMouseInside = true;
            pbTrajectory.MouseLeave += (s, e) => isMouseInside = false;
            pbTrajectory.MouseWheel += PictureBox_MouseWheel;
            pbTrajectory.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    isDragging = true;
                    lastMousePosition = e.Location; // 记录鼠标初始位置
                }
            };
            pbTrajectory.MouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    isDragging = false; // 释放鼠标，停止拖动
                }
            };
            pbTrajectory.MouseMove += PictureBox_MouseMove;
        }
        private void shipdock_Close(object sender, FormClosingEventArgs e)
        {
            //关闭应用
            datatimer.Stop();
            userPort.Close();
            dataPort.Close();
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
            // 检查是否在UI线程
            if (RtbLog.InvokeRequired)
            {
                // 如果在非 UI 线程，使用 Invoke 切换到 UI 线程
                RtbLog.Invoke(new Action<string, LogLevel>(UpdateLog), new object[] { message, loglevel });
            }
            else
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                message = message.Trim() + $"{"     "}[{timestamp}]{Environment.NewLine}";
                message = $"[{loglevel}]   " + message;
                // 在 UI 线程中更新日志到 RichTextBox
                if (loglevel == LogLevel.Info)
                {
                    RtbLog.SelectionColor = System.Drawing.Color.Black;
                    RtbLog.AppendText(message);
                }
                if (loglevel == LogLevel.Error)
                {
                    RtbLog.SelectionColor = System.Drawing.Color.Red;
                    RtbLog.AppendText(message);
                }
                if (loglevel == LogLevel.Warning)
                {
                    RtbLog.SelectionColor = System.Drawing.Color.Orange;
                    RtbLog.AppendText(message);
                }
                // 将日志同时写入 txt 文件
                if (isLogWriterOpen && IsLogFolderExist)
                {
                    logWriter.WriteLine(message);
                    logWriter.Flush();  // 确保实时写入文件
                }
                RtbLog.ScrollToCaret();
            }
        }
        //标记船只
        private void DrawChart(PictureBox pb, Graphics graphics, PointF center, float radius, Brush brush)
        {
            if (pb.InvokeRequired)
            {
                // 如果在非 UI 线程，使用 Invoke 切换到 UI 线程
                pb.Invoke(new Action<PictureBox, Graphics, PointF, float, Brush>(DrawChart), new object[] { pb, graphics, center, radius, brush });
            }
            else
            {
                // 检查圆心是否在 PictureBox 的有效区域内
                if (center.X > pbLeftBottom.X && center.Y < pbLeftBottom.Y && center.X < pbRightTop.X && center.Y > pbRightTop.Y)
                {
                    // 计算圆的外接矩形
                    RectangleF rect = new RectangleF(
                        center.X - radius, // 左上角 X
                        center.Y - radius, // 左上角 Y
                        radius * 2,        // 宽度
                        radius * 2         // 高度
                    );
                    // 计算需要刷新的区域
                    Rectangle updateRect = new Rectangle(
                        (int)(rect.X),      // 更新区域的左上角 X
                        (int)(rect.Y),      // 更新区域的左上角 Y
                        (int)(rect.Width),  // 更新区域的宽度
                        (int)(rect.Height)  // 更新区域的高度
                    );
                    
                    graphics.FillEllipse(brush, rect);
                    // 加锁确保线程安全
                    bmpmutex.WaitOne();
                    MergeLayers();
                    bmpmutex.ReleaseMutex();
                }
            }
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
                    userPortCLI = userPortCLI.Trim();
                    // 如果行首是 "%"，则跳过这行
                    if (userPortCLI.StartsWith("%") || string.IsNullOrWhiteSpace(userPortCLI))
                    {
                        continue;
                    }
                    NumCLI++;
                    // 发送有效的行（非以%开头）
                    if (userPortCLI == "sensorStart")
                    {
                        if (UserSendCheck(userPort, userPortCLI, 1000, 200, "Done"))
                        {
                            NumDone++;
                            IsChangeStart = false;
                        }
                        else
                        {
                            IsChangeStart = true;
                        }
                    }
                    else if (UserSendCheck(userPort, userPortCLI, userPortWait, 7, "Done"))
                    {
                        NumDone++;
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
                IsConnectPort();
                //datatimer.Start();
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
                if (UserSendCheck(userPort, userPortCLI, userPortWait, 7, "Done"))
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
            Usermutex.WaitOne();
            sp.Read(userbuffer, userbufferIndex, bytesToRead);
            userbufferIndex = userbufferIndex + bytesToRead;
            Usermutex.ReleaseMutex();
        }
        private static void DataPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            // 获取接收到的数据的长度
            int bytesToRead = sp.BytesToRead;
            if (endDataIndex + bytesToRead > 2 * 65536)
            {
                sp.Read(databuffer, endDataIndex, 2 * 65536 - endDataIndex);
                sp.Read(databuffer, 0, endDataIndex + bytesToRead - 2 * 65536);
            }
            else
            {
                sp.Read(databuffer, endDataIndex, bytesToRead);
            }
            endDataIndex = (endDataIndex + bytesToRead) % (2 * 65536);
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
            if (UserSendCheck(userPort, userPortCLI, 1000, 200, "Done"))
            {
                UpdateLog("雷达成功关闭", LogLevel.Info);
                datatimer.Stop();
            }
        }

        private void tbCfgPath_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.CfgPath = this.tbCfgPath.Text;
            Properties.Settings.Default.Save();
        }

        private void btnStartLadar_Click(object sender, EventArgs e)
        {
            if (IsChangeStart)
            {
                userPortCLI = "sensorStart";
                IsChangeStart = false;
            }
            else
            {
                userPortCLI = "sensorStart 0";
            }
            if (UserSendCheck(userPort, userPortCLI, 1000, 200, "Done"))
            {
                UpdateLog("雷达成功开启", LogLevel.Info);
                datatimer.Start();
                //开始记录数据
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
        private bool UserSendCheck(SerialPort port, string command, int WaitTime, int SingleWait, string expectedReply)
        {
            bool isReceived = false;
            if (!port.IsOpen)
            {
                return isReceived;
            }
            port.WriteLine(command + " \r\n");
            int retryCount = 0;  // 用于计数重试次数
            int lastIndex = 0;
            bool IsUpdateLog = false;
            while (retryCount <= WaitTime / SingleWait)
            {
                Thread.Sleep(SingleWait);  // 等待一定时间后检查
                retryCount++;
                if (userbufferIndex > 0)
                {
                    byte[] receiveData = new byte[userbufferIndex];
                    Array.Copy(userbuffer, receiveData, userbufferIndex);
                    string reveive = JudgeReceive(Encoding.ASCII.GetString(receiveData)).Trim();
                    if (reveive.Contains(expectedReply))
                    {
                        //UpdateLog(reveive, LogLevel.Info);
                        isReceived = true;
                        IsUpdateLog = true;
                        Usermutex.WaitOne();
                        userbufferIndex = 0;
                        Usermutex.ReleaseMutex();
                        break;
                    }
                    if (lastIndex == userbufferIndex)
                    {
                        UpdateLog(reveive, LogLevel.Error);
                        Usermutex.WaitOne();
                        userbufferIndex = 0;
                        Usermutex.ReleaseMutex();
                        isReceived = false;
                        IsUpdateLog = true;
                        break;
                    }
                    lastIndex = userbufferIndex;
                }
            }
            if (!IsUpdateLog)
            {
                UpdateLog(command + "---无回复", LogLevel.Error);
                isReceived = false;
            }
            return isReceived;
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
        //data处理函数
        private void SaveData(object sender, ElapsedEventArgs e)
        {
            datatimer.Stop(); //先关闭定时器
            string DataPathName;
            //进行datawriter的创建
            if (!this.tbDataPath.Text.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
            {
                DataPathName = this.tbDataPath.Text + System.IO.Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyyMMdd");
            }
            else
            {
                DataPathName = this.tbDataPath.Text + DateTime.Now.ToString("yyyyMMdd");
            }
            if (!Directory.Exists(DataPathName))
            {
                Directory.CreateDirectory(DataPathName);
            }
            DataPathName = DataPathName + "\\Data" + DateTime.Now.ToString("yyyyMMddHH") + ".dat";
            if (dataWriter != null)
            {
                if (dataWriter.BaseStream is FileStream fs)
                {
                    if (fs.Name != DataPathName)
                    {
                        dataWriter = new BinaryWriter(File.Open(DataPathName, FileMode.Append));
                    }
                }
            }
            else
            {
                try
                {
                    dataWriter = new BinaryWriter(File.Open(DataPathName, FileMode.Append));
                }
                catch (Exception ex)
                {
                    UpdateLog(ex.Message, LogLevel.Error);
                    return;
                }
            }
            int writeLength = (endDataIndex + databuffer.Length - startDataIndex) % databuffer.Length;
            for (int i = 0; i < writeLength - 7; i++)
            {
                if (framebuffer.Count == 0 && frameIndex == 0 && databuffer[startDataIndex] == 2 && databuffer[(startDataIndex + 1) % databuffer.Length] == 1
                    && databuffer[(startDataIndex + 2) % databuffer.Length] == 4 && databuffer[(startDataIndex + 3) % databuffer.Length] == 3
                    && databuffer[(startDataIndex + 4) % databuffer.Length] == 6 && databuffer[(startDataIndex + 5) % databuffer.Length] == 5
                    && databuffer[(startDataIndex + 6) % databuffer.Length] == 8 && databuffer[(startDataIndex + 7) % databuffer.Length] == 7)
                {
                    byte[] subbuffer = new byte[4];
                    for (int j = 12; j < 16; j++)
                    {
                        subbuffer[j - 12] = databuffer[(startDataIndex + j) % databuffer.Length];
                    }
                    frameIndex = BinaryPrimitives.ReadUInt32LittleEndian(subbuffer);
                }
                if (frameIndex > 0)
                {
                    framebuffer.Add(databuffer[startDataIndex]);
                    frameIndex--;
                }
                if (framebuffer.Count != 0 && frameIndex == 0)
                {
                    if (backgroundTask != null) backgroundTask.Wait();
                    frame = new List<byte>(framebuffer);
                    backgroundTask = Task.Run(() => ProcessData(frame));
                    framebuffer.Clear();
                }
                startDataIndex = (startDataIndex + 1) % databuffer.Length;
            }
            //if (wirtebufferindex > 0)
            //{
            //    dataWriter.Write(databuffer, 0, wirtebufferindex);
            //    dataWriter.Flush();
            //}
            datatimer.Start(); //执行完毕后再开启器
        }
        private void ProcessData(List<byte> frame)
        {
            if (frame.Count == 0)
            {
                return;
            }
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            byte[] timestampBytes = BitConverter.GetBytes(timestamp);
            //进行内存调整,调整8到20位的存储
            for (int i = 8; i < 20; i++)
            {
                if (i < 12)
                {
                    frame[i] = frame[i + 4];
                }
                else
                {
                    frame[i] = timestampBytes[i - 12];
                }
            }
            if (frame.Count < 40) return;
            int index = 8;
            List<byte> numList = frame.GetRange(index, 4);
            uint packageLength = BinaryPrimitives.ReadUInt32LittleEndian(numList.ToArray());
            if (frame.Count < packageLength)
            {
                return;
            }
            dataWriter.Write(frame.ToArray());
            index = 20;
            numList = frame.GetRange(index, 4);
            uint framenumber = BinaryPrimitives.ReadUInt32LittleEndian(numList.ToArray());
            index = 28;
            numList = frame.GetRange(index, 4);
            index = index + 4;
            uint numDetectedObj = BinaryPrimitives.ReadUInt32LittleEndian(numList.ToArray());
            numList = frame.GetRange(index + 4, 4);
            index = index + 4;
            uint numTLVs = BinaryPrimitives.ReadUInt32LittleEndian(numList.ToArray());
            numList = frame.GetRange(index + 4, 4);
            index = index + 4;
            uint subFrameNumber = BinaryPrimitives.ReadUInt32LittleEndian(numList.ToArray());
            numList = frame.GetRange(index, 4);
            index = index + 4;
            uint tlvType1 = BinaryPrimitives.ReadUInt32LittleEndian(numList.ToArray());
            if (tlvType1 != 1)
            {
                return;
            }
            numList = frame.GetRange(index, 4);
            index = index + 4;
            uint tlvLength = BinaryPrimitives.ReadUInt32LittleEndian(numList.ToArray());
            float[] axisPoint = new float[4 * numDetectedObj];
            for (int i = 0; i < 4 * numDetectedObj; i++)
            {
                numList = frame.GetRange(index, 4);
                index = index + 4;
                axisPoint[i] = BitConverter.ToSingle(numList.ToArray(), 0);
            }
            numList = frame.GetRange(index, 4);
            index = index + 4;
            uint tlvType2 = BinaryPrimitives.ReadUInt32LittleEndian(numList.ToArray());
            if (tlvType2 != 7)
            {
                return;
            }
            numList = frame.GetRange(index, 4);
            index = index + 4;
            uint tlvLength2 = BinaryPrimitives.ReadUInt32LittleEndian(numList.ToArray());
            UInt16[] PointInfo = new UInt16[2 * numDetectedObj];
            for (int i = 0; i < 2 * numDetectedObj; i++)
            {
                numList = frame.GetRange(index, 2);
                index = index + 2;
                PointInfo[i] = BinaryPrimitives.ReadUInt16LittleEndian(numList.ToArray());
            }
            //进行绘图
            if (framenumber % 10 != 0)
            {
                return;
            }
            double snrAverage = 0;
            for (int i = 0; i < numDetectedObj; i++)
            {
                snrAverage = snrAverage + PointInfo[2 * i] / numDetectedObj;
            }
            //进行清空
            //for (int j = 0; j < drawPoint.Count; j++)
            //{
            //    DrawChart(pbTrajectory, pointGraphics, drawPoint[j], 3, new SolidBrush(((Bitmap)pbTrajectory.Image).GetPixel(0, 0)));
            //}
            pointGraphics.Clear(Color.Transparent);
            drawPoint.Clear();
            for (int i = 0; i < numDetectedObj; i++)
            {
                if (PointInfo[2 * i] < snrAverage||(Math.Abs(axisPoint[2 * i]) < 1e-6 && Math.Abs(axisPoint[2 * i + 1]) < 1e-6))
                {
                    continue;
                }
                PointF pixelPoint = new PointF((axisPoint[2 * i] - RealLeftBottom.X) / axisXAdjusted + pbLeftBottom.X, pbLeftBottom.Y - (axisPoint[2 * i + 1] - RealLeftBottom.Y) / axisYAdjusted);
                if (drawPoint.Count == 0)
                {
                    drawPoint.Add(pixelPoint);
                    DrawChart(pbTrajectory, pointGraphics, pixelPoint, 3, Brushes.Black);
                    //UpdateLog("point; " + (new PointF(axisPoint[2 * i], axisPoint[2 * i + 1])).ToString(), LogLevel.Info);
                }
                else
                {
                    bool canAdd = true;
                    foreach (var p in drawPoint)
                    {
                        if ((p.X - pixelPoint.X) * (p.X - pixelPoint.X) + (p.Y - pixelPoint.Y) * (p.Y - pixelPoint.Y) <= 9)
                        {
                            canAdd = false;
                            break;
                        }
                    }
                    if (canAdd)
                    {
                        drawPoint.Add(pixelPoint);
                        DrawChart(pbTrajectory, pointGraphics, pixelPoint, 3, Brushes.Black);
                        //UpdateLog("point; " + (new PointF(axisPoint[2 * i], axisPoint[2 * i + 1])).ToString(), LogLevel.Info);
                    }
                }
            }
        }
        private void PictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!isMouseInside || ModifierKeys != Keys.Control) return; // 只有在 PictureBox 内且按下 Ctrl 才触发缩放
            if (e.X < pbLeftBottom.X || e.Y > pbLeftBottom.Y || e.X > pbRightTop.X || e.Y < pbRightTop.Y) return;//检测是否在有效果范围内
            // 计算鼠标在 PictureBox 内的对应的坐标
            PointF mouse = new PointF((e.X - pbLeftBottom.X) * axisXAdjusted + RealLeftBottom.X, (pbLeftBottom.Y - e.Y) * axisYAdjusted + RealLeftBottom.Y);
            // 根据滚轮方向调整缩放比例
            float factor = 0;
            if (e.Delta < 0) factor = 1 + zoomStep;  // 放大
            else if (e.Delta > 0) factor = scaleFactor > 0.05 ? 1 / (1 + zoomStep) : 1; // 缩小，但不会小于 zoomStep
            scaleFactor = scaleFactor * factor;
            // 计算真实坐标范围和精度
            axisXAdjusted = axisX * scaleFactor;
            axisYAdjusted = axisY * scaleFactor;
            float RealLeftBottomX = mouse.X - axisXAdjusted * (e.X - pbLeftBottom.X);
            float RealLeftBottomY = mouse.Y - axisYAdjusted * (pbLeftBottom.Y - e.Y);
            float RealRightTopX = mouse.X + axisXAdjusted * (pbRightTop.X - e.X);
            float RealRightTopY = mouse.Y + axisYAdjusted * (e.Y - pbRightTop.Y);
            if (RealLeftBottomX < -510 || RealLeftBottomY < 0 || RealRightTopX > 510 || RealRightTopY > 510)
            {
                scaleFactor = 1;
                axisXAdjusted = axisX * scaleFactor;
                axisYAdjusted = axisY * scaleFactor;
                RealLeftBottom = new PointF(-510, 0);
                RealRightTop = new PointF(510, 510);
            }
            else
            {
                RealLeftBottom.X = RealLeftBottomX;
                RealLeftBottom.Y = RealLeftBottomY;
                RealRightTop.X = RealRightTopX;
                RealRightTop.Y = RealRightTopY;
            }
            // 重新绘制刻度
            PointF CenterP = new PointF((pbLeftBottom.X + pbRightTop.X) / 2, pbLeftBottom.Y);
            for (int i = 1; i <= 10; i++)
            {
                int text = (int)(RealRightTop.Y - (PointY[i - 1].Y - pbRightTop.Y) * axisYAdjusted);
                // **清空文本区域**
                SizeF textSize = axisGraphics.MeasureString("999", new Font("宋体", 9));
                RectangleF textRect = new RectangleF(new PointF(PointY[i - 1].X, PointY[i - 1].Y - 5), textSize);
                Rectangle invalidateRect = Rectangle.Ceiling(textRect);
                axisGraphics.FillRectangle(new SolidBrush(Color.White), textRect);
                // 更新文本区域
                axisGraphics.DrawString(text.ToString(), new Font("宋体", 9), Brushes.Black, new PointF(PointY[i - 1].X, PointY[i - 1].Y - 5));
            }
            for (int i = -5; i <= 5; i++)
            {
                if (i != 0)
                {
                    int text = (int)(RealLeftBottom.X + (PointX[i + 5].X - pbLeftBottom.X) * axisXAdjusted);
                    // 清空文本区域
                    SizeF textSize = axisGraphics.MeasureString("-999", new Font("宋体", 9));
                    RectangleF textRect = new RectangleF(new PointF(PointX[i + 5].X - 10, PointX[i + 5].Y), textSize);
                    Rectangle invalidateRect = Rectangle.Ceiling(textRect);
                    axisGraphics.FillRectangle(new SolidBrush(Color.White), textRect);
                    // 更新文本区域
                    axisGraphics.DrawString(text.ToString(), new Font("宋体", 9), Brushes.Black, new PointF(PointX[i + 5].X - 10, PointX[i + 5].Y));
                }
            }
            MergeLayers();
        }
        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isMouseInside || !isDragging) return; // 只有在 PictureBox 内且按下 Ctrl 才触发缩放
            if (e.X < pbLeftBottom.X || e.Y > pbLeftBottom.Y || e.X > pbRightTop.X || e.Y < pbRightTop.Y) return;//检测是否在有效果范围内
            int offsetX = e.X - lastMousePosition.X;
            int offsetY = e.Y - lastMousePosition.Y;
            lastMousePosition = e.Location;
            //UpdateLog("X: " + offsetX.ToString() + "Y: " + offsetY.ToString(), LogLevel.Info);
            float RealLeftBottomX = RealLeftBottom.X - offsetX * axisXAdjusted;
            float RealLeftBottomY = RealLeftBottom.Y + offsetY * axisYAdjusted;
            float RealRightTopX = RealRightTop.X - offsetX * axisXAdjusted;
            float RealRightTopY = RealRightTop.Y + offsetY * axisYAdjusted;
            if (RealLeftBottomX < -510 || RealLeftBottomY < 0 || RealRightTopX > 510 || RealRightTopY > 510)
            {
                return;
            }
            else
            {
                RealLeftBottom.X = RealLeftBottomX;
                RealLeftBottom.Y = RealLeftBottomY;
                RealRightTop.X = RealRightTopX;
                RealRightTop.Y = RealRightTopY;
            }
            // 重新绘制刻度
            PointF CenterP = new PointF((pbLeftBottom.X + pbRightTop.X) / 2, pbLeftBottom.Y);
            for (int i = 1; i <= 10; i++)
            {
                int text = (int)(RealRightTop.Y - (PointY[i - 1].Y - pbRightTop.Y) * axisYAdjusted);
                // 清空文本区域
                SizeF textSize = axisGraphics.MeasureString("999", new Font("宋体", 9));
                RectangleF textRect = new RectangleF(new PointF(PointY[i - 1].X, PointY[i - 1].Y - 5), textSize);
                Rectangle invalidateRect = Rectangle.Ceiling(textRect);
                axisGraphics.FillRectangle(new SolidBrush(Color.White), textRect);
                // 更新文本区域
                axisGraphics.DrawString(text.ToString(), new Font("宋体", 9), Brushes.Black, new PointF(PointY[i - 1].X, PointY[i - 1].Y - 5));
            }
            for (int i = -5; i <= 5; i++)
            {
                if (i != 0)
                {
                    int text = (int)(RealLeftBottom.X + (PointX[i + 5].X - pbLeftBottom.X) * axisXAdjusted);
                    // 清空文本区域
                    SizeF textSize = axisGraphics.MeasureString("-999", new Font("宋体", 9));
                    RectangleF textRect = new RectangleF(new PointF(PointX[i + 5].X - 10, PointX[i + 5].Y), textSize);
                    Rectangle invalidateRect = Rectangle.Ceiling(textRect);
                    axisGraphics.FillRectangle(new SolidBrush(Color.White), textRect);
                    // 更新文本区域
                    axisGraphics.DrawString(text.ToString(), new Font("宋体", 9), Brushes.Black, new PointF(PointX[i + 5].X - 10, PointX[i + 5].Y));
                }
            }
            MergeLayers();
        }
        private void MergeLayers()
        {
            finalGraphics.Clear(Color.Transparent);
            finalGraphics.DrawImage(axisBitmap, 0, 0); // 绘制坐标轴
            finalGraphics.DrawImage(pointBitmap, 0, 0); // 绘制目标点

            // 更新 PictureBox 显示
            pbTrajectory.Image = finalBitmap;
        }
    }
}