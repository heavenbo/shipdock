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
using System.Reflection;
using System.Collections.Generic;
using System;
namespace shipdock
{
    public partial class shipdock : Form
    {
        //��ͼ����
        List<KalmanFilter> kalmanFilters = new List<KalmanFilter>();
        List<Trajectory> trajectories = new List<Trajectory>();
        private Bitmap axisBitmap, pointBitmap, finalBitmap;
        private Graphics axisGraphics, pointGraphics, finalGraphics;
        private PointF pbRightTop, pbLeftBottom;
        private PointF RealRightTop, RealLeftBottom;
        private float axisX, axisY;
        private float axisXAdjusted, axisYAdjusted;
        bool isMouseInside = false;
        private float scaleFactor = 1.0f; // ���ű���
        private const float zoomStep = 0.5f; // ÿ�����ű����仯
        private bool isDragging = false;
        private Point lastMousePosition;
        PointF[] PointX = new PointF[11];
        PointF[] PointY = new PointF[10];
        private static Mutex bmpmutex = new Mutex();
        //��־��ر���
        private static bool IsLogFolderExist = false;
        private StreamWriter logWriter;
        private static bool isLogWriterOpen = false;
        //user�˿���ر���
        private SerialPort userPort = new SerialPort();
        private static string userPortCLI;
        private static byte[] userbuffer = new byte[1024]; // ����һ���ֽڻ�����
        private static int userbufferIndex = 0;
        private int userPortWait = 150;
        private static Mutex Usermutex = new Mutex();
        //���ݶ˿���ر���
        private SerialPort dataPort = new SerialPort();
        static System.Timers.Timer datatimer;
        private static BinaryWriter dataWriter;
        private static byte[] databuffer = new byte[2 * 65536]; // ����һ���ֽڻ�����
        private static int startDataIndex = 0;
        private static int endDataIndex = 0;
        //�������ݵı���
        Task backgroundTask;
        private List<byte> framebuffer = new List<byte>();
        private List<byte> frame = new List<byte>();
        uint frameIndex = 0;
        //��������
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
        // ���ش���
        private void shipdock_Load(object sender, EventArgs e)
        {
            //ʱ���ע
            UpdateLog("ϵͳ����ʱ��Ϊ:  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LogLevel.Info);
            //�ļ�·��
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
                    logWriter = new StreamWriter(LogPathName, true);  // true ��ʾ׷��д��
                    isLogWriterOpen = true;
                    UpdateLog("��־������", LogLevel.Info);
                }
                catch (Exception ex)
                {
                    // �����쳣�����������Ϣ
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
                    // �����ļ���
                    Directory.CreateDirectory(this.tbDataPath.Text);
                    UpdateLog("�����ļ��д����ɹ�", LogLevel.Info);
                }
                else
                {
                    UpdateLog("�����ļ����Ѵ���", LogLevel.Info);
                }
            }
            catch (Exception ex)
            {
                // �����쳣�����������Ϣ
                UpdateLog(ex.Message, LogLevel.Error);
            }
            if (string.IsNullOrWhiteSpace(this.tbCfgPath.Text))
            {
                this.tbCfgPath.Text = Path.GetFullPath("../../../Properties/default.cfg");
            }
            this.cbUserBaudRate.Text = "115200";
            this.cbDataBaudRate.Text = "921600";
            //����жϺ���
            userPort.DataReceived += new SerialDataReceivedEventHandler(UserPort_DataReceived);
            dataPort.DataReceived += new SerialDataReceivedEventHandler(DataPort_DataReceived);
            //���ö�ʱ������ʱ����
            datatimer = new System.Timers.Timer(50);  // ��������Ϊ50���루0.1�룩
            datatimer.Elapsed += SaveData;  // ���ô����¼�
            //��ذ�ťdisabled
            this.btnSendParam.Enabled = false;
            this.btnSendParam.ForeColor = System.Drawing.Color.Gray;
            this.btnStartLadar.Enabled = false;
            this.btnStartLadar.ForeColor = System.Drawing.Color.Gray;
            this.btnStopLadar.Enabled = false;
            this.btnStopLadar.ForeColor = System.Drawing.Color.Gray;

            //��ͼ��ʼ��
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

            //��x��
            axisGraphics.DrawLine(Pens.Black, pbLeftBottom.X, pbLeftBottom.Y, pbRightTop.X, pbLeftBottom.Y);
            PointF[] xpt = new PointF[3] { new PointF(pbRightTop.X + 8, pbLeftBottom.Y), new PointF(pbRightTop.X, pbLeftBottom.Y + 4), new PointF(pbRightTop.X, pbLeftBottom.Y - 4) };//x��������
            axisGraphics.DrawPolygon(Pens.Black, xpt);
            axisGraphics.FillPolygon(new SolidBrush(Color.Black), xpt);
            axisGraphics.DrawString("����/m", new Font("����", 10), Brushes.Black, new PointF(pointBitmap.Width - 60, pbLeftBottom.Y - 20));
            //��y��
            axisGraphics.DrawLine(Pens.Black, CenterP.X, CenterP.Y, CenterP.X, pbRightTop.Y);
            PointF[] ypt = new PointF[3] { new PointF(CenterP.X, pbRightTop.Y - 8), new PointF(CenterP.X - 4, pbRightTop.Y), new PointF(CenterP.X + 4, pbRightTop.Y) };//y��������
            axisGraphics.DrawPolygon(Pens.Black, ypt);
            axisGraphics.FillPolygon(new SolidBrush(Color.Black), ypt);
            axisGraphics.DrawString("����/m", new Font("����", 10), Brushes.Black, new PointF(CenterP.X + 9, pbRightTop.Y - 10));
            //��ǿ̶�
            axisX = (RealRightTop.X - RealLeftBottom.X) / (pbRightTop.X - pbLeftBottom.X);
            axisY = (RealRightTop.Y - RealLeftBottom.Y) / (pbLeftBottom.Y - pbRightTop.Y);
            axisXAdjusted = axisX;
            axisYAdjusted = axisY;
            for (int i = 1; i <= 10; i++)
            {
                PointY[i - 1] = new PointF(CenterP.X + 5, CenterP.Y - i * 50 / axisY);
                axisGraphics.DrawString((i * 50).ToString(), new Font("����", 9), Brushes.Black, new PointF(PointY[i - 1].X, PointY[i - 1].Y - 5));
                axisGraphics.DrawLine(Pens.Black, CenterP.X - 3, CenterP.Y - i * 50 / axisY, CenterP.X, CenterP.Y - i * 50 / axisY);
            }
            for (int i = -5; i <= 5; i++)
            {
                PointX[i + 5] = new PointF(pbLeftBottom.X + (10 + (i + 5) * 100) / axisX, pbLeftBottom.Y + 6);
                if (i != 0)
                {
                    axisGraphics.DrawString((i * 100).ToString(), new Font("����", 9), Brushes.Black, new PointF(PointX[i + 5].X - 10, PointX[i + 5].Y));
                    axisGraphics.DrawLine(Pens.Black, pbLeftBottom.X + (10 + (i + 5) * 100) / axisX, pbLeftBottom.Y - 3, pbLeftBottom.X + (10 + (i + 5) * 100) / axisX, pbLeftBottom.Y);
                }
            }
            MergeLayers();
            //���Ź���
            pbTrajectory.MouseEnter += (s, e) => isMouseInside = true;
            pbTrajectory.MouseLeave += (s, e) => isMouseInside = false;
            pbTrajectory.MouseWheel += PictureBox_MouseWheel;
            pbTrajectory.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    isDragging = true;
                    lastMousePosition = e.Location; // ��¼����ʼλ��
                }
            };
            pbTrajectory.MouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    isDragging = false; // �ͷ���ֹ꣬ͣ�϶�
                }
            };
            pbTrajectory.MouseMove += PictureBox_MouseMove;
        }
        private void shipdock_Close(object sender, FormClosingEventArgs e)
        {
            //�ر�Ӧ��
            datatimer.Stop();
            userPort.Close();
            dataPort.Close();
            var confirmForm = new Form();
            confirmForm.StartPosition = FormStartPosition.CenterParent;  // �����ڸ�����������ʾ
            var result = MessageBox.Show(this, "ȷ��Ҫ�˳���?", "�˳�ȷ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                // ȡ������رղ���
                e.Cancel = true;
            }
        }
        private void IsLog_CheckedChanged(object sender, EventArgs e)
        {
            // ���û����ָ���Ŀؼ�
            tbLogPath.Enabled = !IsLog.Checked;
            btnLog.Enabled = !IsLog.Checked;

            // ��ѡ������״̬�ı�ؼ�����ʽ����ʾ��Ϣ
            if (IsLog.Checked)
            {
                IsLog.ForeColor = System.Drawing.Color.Gray;
                tbLogPath.BackColor = System.Drawing.Color.LightGray;
                try
                {
                    // �����ļ���
                    Directory.CreateDirectory(this.tbLogPath.Text);
                    UpdateLog("��־�ļ����Ѵ������ɽ�����־�ļ�¼", LogLevel.Info);
                    IsLogFolderExist = true;
                }
                catch (Exception ex)
                {
                    // �����쳣�����������Ϣ
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
                logWriter = new StreamWriter(LogPathName, true);  // true ��ʾ׷��д��
                isLogWriterOpen = true;
                UpdateLog("������־ϵͳ", LogLevel.Info);
            }
            else
            {
                // ���ÿؼ�ʱ��������ʾ��Ϣ��ؼ���ʽ
                tbLogPath.BackColor = System.Drawing.Color.White;
                IsLog.ForeColor = System.Drawing.Color.Black;
                UpdateLog("�ر���־ϵͳ", LogLevel.Info);
                if (isLogWriterOpen)
                {
                    isLogWriterOpen = false;
                    logWriter.Close();
                }
            }
        }

        //����Log
        private void UpdateLog(string message, LogLevel loglevel)
        {
            // ����Ƿ���UI�߳�
            if (RtbLog.InvokeRequired)
            {
                // ����ڷ� UI �̣߳�ʹ�� Invoke �л��� UI �߳�
                RtbLog.Invoke(new Action<string, LogLevel>(UpdateLog), new object[] { message, loglevel });
            }
            else
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                message = message.Trim() + $"{"     "}[{timestamp}]{Environment.NewLine}";
                message = $"[{loglevel}]   " + message;
                // �� UI �߳��и�����־�� RichTextBox
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
                // ����־ͬʱд�� txt �ļ�
                if (isLogWriterOpen && IsLogFolderExist)
                {
                    logWriter.WriteLine(message);
                    logWriter.Flush();  // ȷ��ʵʱд���ļ�
                }
                RtbLog.ScrollToCaret();
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
            folderDialog.Description = "��ѡ������־���ļ���";

            folderDialog.SelectedPath = @"C:\";

            // ��ʾ�ļ���ѡ��Ի���
            DialogResult result = folderDialog.ShowDialog();

            // ����û�ѡ�����ļ���
            if (result == DialogResult.OK)
            {
                this.tbLogPath.Text = folderDialog.SelectedPath + @"\";
            }
        }

        private void btnData_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "��ѡ�������ݵ��ļ���";
            folderDialog.SelectedPath = @"C:\";
            // ��ʾ�ļ���ѡ��Ի���
            DialogResult result = folderDialog.ShowDialog();

            // ����û�ѡ�����ļ���
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
                UpdateLog("�����ļ�·��Ϊ��", LogLevel.Error);
                return;
            }
            else if (!this.tbCfgPath.Text.EndsWith(".cfg", StringComparison.OrdinalIgnoreCase) && !this.tbCfgPath.Text.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                UpdateLog("ָ�������ļ�����,������ .cfg�� .txt", LogLevel.Error);
                return;
            }
            // ����һ���µĴ���ʵ��
            ParamForm paramForm = new ParamForm();
            // ��ʾ�µĴ���
            paramForm.Show();
        }

        private void btnSendParam_Click(object sender, EventArgs e)
        {
            if (!this.tbCfgPath.Text.EndsWith(".cfg", StringComparison.OrdinalIgnoreCase) && !this.tbCfgPath.Text.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                UpdateLog("ָ�������ļ�����,������ .cfg�� .txt", LogLevel.Error);
                return;
            }
            using (StreamReader reader = new StreamReader(this.tbCfgPath.Text))
            {
                int NumDone = 0;
                int NumCLI = 0;
                IsChangeStart = true;
                // ���ж�ȡ�ļ�����
                while ((userPortCLI = reader.ReadLine()) != null)
                {
                    userPortCLI = userPortCLI.Trim();
                    // ��������� "%"������������
                    if (userPortCLI.StartsWith("%") || string.IsNullOrWhiteSpace(userPortCLI))
                    {
                        continue;
                    }
                    NumCLI++;
                    // ������Ч���У�����%��ͷ��
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
                    UpdateLog("ָ��ͳɹ�����������" + NumCLI.ToString() + "��ָ�" + "��������" + NumDone.ToString() + "��Done", LogLevel.Info);
                }
                else
                {
                    UpdateLog("ָ���ʧ�ܣ���������" + NumCLI.ToString() + "��ָ�" + "��������" + NumDone.ToString() + "��Done", LogLevel.Error);
                }
            }
        }

        private void btnConnectPort_Click(object sender, EventArgs e)
        {
            //�����û��˿ں����ݶ˿�
            try
            {
                userPort.PortName = this.cbUserPort.Text;
                userPort.BaudRate = int.Parse(this.cbUserBaudRate.Text);
                dataPort.PortName = this.cbDataPort.Text;
                dataPort.BaudRate = int.Parse(this.cbDataBaudRate.Text);
                userPort.DtrEnable = false;
                userPort.RtsEnable = false;
                userPort.Open();
                UpdateLog("�û��˿��Ѵ�", LogLevel.Info);
                dataPort.DtrEnable = false;
                dataPort.RtsEnable = false;
                dataPort.Open();
                UpdateLog("���ݶ˿��Ѵ�", LogLevel.Info);
                IsConnectPort();
                //datatimer.Start();
            }
            catch (Exception ex)
            {
                UpdateLog(ex.Message, LogLevel.Error);
            }
        }
        //����Ƿ񴮿��Ƿ���Խ���ͨ��
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
                    UpdateLog("�˿ڿɽ�������ͨ��", LogLevel.Info);
                    return;
                }
            }
            UpdateLog("�˿ڲ��ɽ�������ͨ��", LogLevel.Error);
            this.btnConnectPort.Enabled = true;
            this.btnConnectPort.ForeColor = System.Drawing.Color.Black;
        }
        //�û��˿ڽ����ж�
        private static void UserPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            // ��ȡ���յ������ݵĳ���
            int bytesToRead = sp.BytesToRead;
            Usermutex.WaitOne();
            sp.Read(userbuffer, userbufferIndex, bytesToRead);
            userbufferIndex = userbufferIndex + bytesToRead;
            Usermutex.ReleaseMutex();
        }
        private static void DataPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            // ��ȡ���յ������ݵĳ���
            int bytesToRead = sp.BytesToRead;
            if (endDataIndex + bytesToRead > databuffer.Length)
            {
                sp.Read(databuffer, endDataIndex, databuffer.Length - endDataIndex);
                sp.Read(databuffer, 0, endDataIndex + bytesToRead - databuffer.Length);
            }
            else
            {
                sp.Read(databuffer, endDataIndex, bytesToRead);
            }
            endDataIndex = (endDataIndex + bytesToRead) % (databuffer.Length);
        }
        private void btnClosePort_Click(object sender, EventArgs e)
        {
            try
            {
                userPort.Close();
                dataPort.Close();
                UpdateLog("�����ѹر�", LogLevel.Info);
            }
            catch (Exception ex)
            {
                UpdateLog(ex.Message, LogLevel.Error);
            }
        }

        private void btncfgPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // �����ļ����͹�����
            openFileDialog.Filter = "�����ļ� (*.cfg)|*.cfg|�ı��ļ� (*.txt)|*.txt";

            // ֻ����ѡ��һ���ļ�
            openFileDialog.Multiselect = false;

            // ���ļ�ѡ��Ի���
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
                UpdateLog("�״�ɹ��ر�", LogLevel.Info);
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
                UpdateLog("�״�ɹ�����", LogLevel.Info);
                datatimer.Start();
                //��ʼ��¼����
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
                UpdateLog("�˿ڿ�������:" + ports.Length.ToString(), LogLevel.Info);
            }
            else
            {
                UpdateLog("û�п��õĶ˿ڣ�", LogLevel.Warning);
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
            int retryCount = 0;  // ���ڼ������Դ���
            int lastIndex = 0;
            bool IsUpdateLog = false;
            while (retryCount <= WaitTime / SingleWait)
            {
                Thread.Sleep(SingleWait);  // �ȴ�һ��ʱ�����
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
                UpdateLog(command + "---�޻ظ�", LogLevel.Error);
                isReceived = false;
            }
            return isReceived;
        }
        //�жϷ�����Ϣ
        private string JudgeReceive(string receivedata)
        {
            receivedata = receivedata.Replace("mmwDemo:/>", string.Empty); // ɾ���ض��ַ���
            // ���ջ��з��������
            string[] lines = receivedata.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            // �� "---" ƴ��ÿһ��
            string result = string.Join("---", lines);
            // ȥ��ǰ��հף������ؽ��
            result = result.Trim();
            return result;
        }
        //data������
        private void SaveData(object? sender, ElapsedEventArgs e)
        {
            datatimer.Stop(); //�ȹرն�ʱ��
            string DataPathName;
            //����datawriter�Ĵ���
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
                UpdateLog(DataPathName, LogLevel.Info);
            }

            DataPathName = DataPathName + "\\Data" + DateTime.Now.ToString("yyyyMMddHH") + ".dat";

            if (dataWriter != null)
            {
                if (dataWriter.BaseStream is FileStream fs)
                {
                    if (fs.Name != DataPathName)
                    {
                        dataWriter.Close();
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
                    for (int j = 0; j < 4; j++)
                    {
                        subbuffer[j] = databuffer[(startDataIndex + 12 + j) % databuffer.Length];//��ǰ�Ǽ�12��
                    }
                    frameIndex = BinaryPrimitives.ReadUInt32LittleEndian(subbuffer);
                }
                if (frameIndex > 0)
                {
                    framebuffer.Add(databuffer[startDataIndex]);
                    frameIndex--;
                }
                startDataIndex = (startDataIndex + 1) % databuffer.Length;
                if (framebuffer.Count != 0 && frameIndex == 0)
                {
                    if (backgroundTask != null) backgroundTask.Wait();
                    frame = new List<byte>(framebuffer);
                    backgroundTask = Task.Run(() => ProcessData(frame));
                    framebuffer.Clear();
                }

            }
            datatimer.Start(); //ִ����Ϻ��ٿ�����
        }
        int[] notdetect = new int[100];
        private void ProcessData(List<byte> frame)
        {
            if (frame.Count == 0)
            {
                return;
            }
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            byte[] timestampBytes = BitConverter.GetBytes(timestamp);
            //�����ڴ����,����8��20λ�Ĵ洢
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
            dataWriter.Flush();
            index = 20;
            numList = frame.GetRange(index, 4);
            uint framenumber = BinaryPrimitives.ReadUInt32LittleEndian(numList.ToArray());
            index = 28;
            numList = frame.GetRange(index, 4);
            uint numDetectedObj = BinaryPrimitives.ReadUInt32LittleEndian(numList.ToArray());
            index = index + 4;
            numList = frame.GetRange(index, 4);
            uint numTLVs = BinaryPrimitives.ReadUInt32LittleEndian(numList.ToArray());
            index = index + 4;
            numList = frame.GetRange(index, 4);
            uint subFrameNumber = BinaryPrimitives.ReadUInt32LittleEndian(numList.ToArray());
            index = index + 4;
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
            UInt16[] PointInfo = new UInt16[numDetectedObj];
            for (int i = 0; i < numDetectedObj; i++)
            {
                numList = frame.GetRange(index, 2);
                index = index + 4;
                PointInfo[i] = BinaryPrimitives.ReadUInt16LittleEndian(numList.ToArray());
            }
            //����ѡ��
            if (framenumber % 10 != 0)
            {
                return;
            }
            double snrAverage = 0;
            for (int i = 0; i < numDetectedObj; i++)
            {
                snrAverage = snrAverage + PointInfo[i] / numDetectedObj;
            }
            if (framenumber % 800 == 0)
            {
                trajectories.Clear();
                trajectories.Capacity = 0;
                pointGraphics.Clear(Color.Transparent);

            }
            var PointList = new List<(PointF point, double snr)>();
            for (int i = 0; i < numDetectedObj; i++)
            {
                if (PointInfo[i] < snrAverage || (Math.Abs(axisPoint[2 * i]) < 1e-6 && Math.Abs(axisPoint[2 * i + 1]) < 1e-6))
                {
                    continue;
                }
                PointF newpoint = new PointF(axisPoint[2 * i], axisPoint[2 * i + 1]);
                if (PointList.Count == 0)
                {
                    PointList.Add((newpoint, PointInfo[i]));
                    UpdateLog("point:" + PointList[^1].point.ToString() + "snr: " + PointInfo[i].ToString(), LogLevel.Info);
                }
                else
                {
                    bool canAdd = true;
                    foreach (var p in PointList)
                    {
                        if ((p.point.X - newpoint.X) * (p.point.X - newpoint.X) + (p.point.Y - newpoint.Y) * (p.point.Y - newpoint.Y) <= 9)
                        {
                            canAdd = false;
                            break;
                        }
                    }
                    if (canAdd)
                    {
                        PointList.Add((newpoint, PointInfo[i]));
                        UpdateLog("point:" + PointList[^1].point.ToString() + "snr: " + PointInfo[i].ToString(), LogLevel.Info);
                    }
                }
            }
            //���л�ͼ
            //ѡ��������ź���ǿ����Ϊ�źŵ�
            if (subFrameNumber == 0 && PointList.Count != 0)
            {
                if (trajectories.Count == 0)
                {

                    var maxItem = PointList.OrderByDescending(item => item.snr).First();
                    PointF target = maxItem.point;//ѡ������ĵ�
                    PointF pixelPoint = new PointF((target.X - RealLeftBottom.X) / axisXAdjusted + pbLeftBottom.X, pbLeftBottom.Y - (target.Y - RealLeftBottom.Y) / axisYAdjusted);
                    trajectories.Add(new Trajectory(Color.Blue, Color.Red, pbLeftBottom, pbRightTop, target, q: 1, r: 10));
                    trajectories[0].AddPoint(pbTrajectory, pixelPoint, pointGraphics);
                    trajectories[0].realPoint.Add(target);
                }
                else
                {

                    PointF lastPoint = trajectories[0].realPoint[^1];
                    PointF target = PointList.OrderBy(p => Trajectory.Distance(p.point, lastPoint)).First().point;//ѡ������ĵ�
                    if (Trajectory.Distance(lastPoint, target) < 7)
                    {
                        trajectories[0].realPoint.Add(target);
                        PointF pixelPoint = new PointF((target.X - RealLeftBottom.X) / axisXAdjusted + pbLeftBottom.X, pbLeftBottom.Y - (target.Y - RealLeftBottom.Y) / axisYAdjusted);
                        trajectories[0].AddPoint(pbTrajectory, pixelPoint, pointGraphics);
                    }
                    else
                    {
                        notdetect[0] = notdetect[0] + 1;
                        if (notdetect[0] > 3)
                        {
                            //trajectories[0].Exists=false;
                            trajectories.RemoveAt(0);
                            //UpdateLog("�޹켣", LogLevel.Info);
                        }
                    }
                }
            }
            bmpmutex.WaitOne();
            MergeLayers();
            bmpmutex.ReleaseMutex();
        }
        private void PictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!isMouseInside || ModifierKeys != Keys.Control) return; // ֻ���� PictureBox ���Ұ��� Ctrl �Ŵ�������
            if (e.X < pbLeftBottom.X || e.Y > pbLeftBottom.Y || e.X > pbRightTop.X || e.Y < pbRightTop.Y) return;//����Ƿ�����Ч����Χ��
            // ��������� PictureBox �ڵĶ�Ӧ������
            PointF mouse = new PointF((e.X - pbLeftBottom.X) * axisXAdjusted + RealLeftBottom.X, (pbLeftBottom.Y - e.Y) * axisYAdjusted + RealLeftBottom.Y);
            // ���ݹ��ַ���������ű���
            float factor = 0;
            if (e.Delta < 0) factor = 1 + zoomStep;  // �Ŵ�
            else if (e.Delta > 0) factor = scaleFactor > 0.05 ? 1 / (1 + zoomStep) : 1; // ��С��������С�� zoomStep
            scaleFactor = scaleFactor * factor;
            // ������ʵ���귶Χ�;���
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
            // ���»��ƿ̶�
            PointF CenterP = new PointF((pbLeftBottom.X + pbRightTop.X) / 2, pbLeftBottom.Y);
            for (int i = 1; i <= 10; i++)
            {
                int text = (int)(RealRightTop.Y - (PointY[i - 1].Y - pbRightTop.Y) * axisYAdjusted);
                // **����ı�����**
                SizeF textSize = axisGraphics.MeasureString("999", new Font("����", 9));
                RectangleF textRect = new RectangleF(new PointF(PointY[i - 1].X, PointY[i - 1].Y - 5), textSize);
                Rectangle invalidateRect = Rectangle.Ceiling(textRect);
                axisGraphics.FillRectangle(new SolidBrush(Color.White), textRect);
                // �����ı�����
                axisGraphics.DrawString(text.ToString(), new Font("����", 9), Brushes.Black, new PointF(PointY[i - 1].X, PointY[i - 1].Y - 5));
            }
            for (int i = -5; i <= 5; i++)
            {
                if (i != 0)
                {
                    int text = (int)(RealLeftBottom.X + (PointX[i + 5].X - pbLeftBottom.X) * axisXAdjusted);
                    // ����ı�����
                    SizeF textSize = axisGraphics.MeasureString("-999", new Font("����", 9));
                    RectangleF textRect = new RectangleF(new PointF(PointX[i + 5].X - 10, PointX[i + 5].Y), textSize);
                    Rectangle invalidateRect = Rectangle.Ceiling(textRect);
                    axisGraphics.FillRectangle(new SolidBrush(Color.White), textRect);
                    // �����ı�����
                    axisGraphics.DrawString(text.ToString(), new Font("����", 9), Brushes.Black, new PointF(PointX[i + 5].X - 10, PointX[i + 5].Y));
                }
            }
            bmpmutex.WaitOne();
            MergeLayers();
            bmpmutex.ReleaseMutex();
        }
        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isMouseInside || !isDragging) return; // ֻ���� PictureBox ���Ұ��� Ctrl �Ŵ�������
            if (e.X < pbLeftBottom.X || e.Y > pbLeftBottom.Y || e.X > pbRightTop.X || e.Y < pbRightTop.Y) return;//����Ƿ�����Ч����Χ��
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
            // ���»��ƿ̶�
            PointF CenterP = new PointF((pbLeftBottom.X + pbRightTop.X) / 2, pbLeftBottom.Y);
            for (int i = 1; i <= 10; i++)
            {
                int text = (int)(RealRightTop.Y - (PointY[i - 1].Y - pbRightTop.Y) * axisYAdjusted);
                // ����ı�����
                SizeF textSize = axisGraphics.MeasureString("999", new Font("����", 9));
                RectangleF textRect = new RectangleF(new PointF(PointY[i - 1].X, PointY[i - 1].Y - 5), textSize);
                Rectangle invalidateRect = Rectangle.Ceiling(textRect);
                axisGraphics.FillRectangle(new SolidBrush(Color.White), textRect);
                // �����ı�����
                axisGraphics.DrawString(text.ToString(), new Font("����", 9), Brushes.Black, new PointF(PointY[i - 1].X, PointY[i - 1].Y - 5));
            }
            for (int i = -5; i <= 5; i++)
            {
                if (i != 0)
                {
                    int text = (int)(RealLeftBottom.X + (PointX[i + 5].X - pbLeftBottom.X) * axisXAdjusted);
                    // ����ı�����
                    SizeF textSize = axisGraphics.MeasureString("-999", new Font("����", 9));
                    RectangleF textRect = new RectangleF(new PointF(PointX[i + 5].X - 10, PointX[i + 5].Y), textSize);
                    Rectangle invalidateRect = Rectangle.Ceiling(textRect);
                    axisGraphics.FillRectangle(new SolidBrush(Color.White), textRect);
                    // �����ı�����
                    axisGraphics.DrawString(text.ToString(), new Font("����", 9), Brushes.Black, new PointF(PointX[i + 5].X - 10, PointX[i + 5].Y));
                }
            }
            bmpmutex.WaitOne();
            MergeLayers();
            bmpmutex.ReleaseMutex();
        }
        private void MergeLayers()
        {
            finalGraphics.Clear(Color.Transparent);
            finalGraphics.DrawImage(axisBitmap, 0, 0); // ����������
            finalGraphics.DrawImage(pointBitmap, 0, 0); // ����Ŀ���
            // ���� PictureBox ��ʾ
            pbTrajectory.Image = finalBitmap;
        }
    }
}