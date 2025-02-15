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
        private static byte[] userbuffer = new byte[1024]; // ����һ���ֽڻ�����
        private static int userbufferIndex = 0;
        private static byte[] databuffer = new byte[1024]; // ����һ���ֽڻ�����
        private static int databufferIndex = 0;
        private StreamWriter logWriter;
        private bool isLogWriterOpen = false;
        private bool IsChangeStart = false;
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
        // ���ش���
        private void shipdock_Load(object sender, EventArgs e)
        {
            //ʱ���ע
            this.ProgramStart.Text = "ϵͳ����ʱ��Ϊ:  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //�ļ�·��
            this.tbLogPath.Text = Properties.Settings.Default.LogPath;
            this.tbDataPath.Text = Properties.Settings.Default.DataPath;
            this.tbCfgPath.Text = Properties.Settings.Default.CfgPath;
            if (string.IsNullOrWhiteSpace(this.tbLogPath.Text))
            {
                this.tbLogPath.Text = Path.GetFullPath("../log/");
                try
                {
                    // �����ļ���
                    Directory.CreateDirectory(this.tbLogPath.Text);
                    UpdateLog("�ļ����Ѵ���: " + this.tbLogPath.Text, LogLevel.Info);
                }
                catch (Exception ex)
                {
                    // �����쳣�����������Ϣ
                    UpdateLog(ex.Message, LogLevel.Error);
                }
            }
            if (string.IsNullOrWhiteSpace(this.tbDataPath.Text))
            {
                this.tbDataPath.Text = Path.GetFullPath("../../../data/");
                try
                {
                    // �����ļ���
                    Directory.CreateDirectory(this.tbDataPath.Text);
                    UpdateLog("�ļ����Ѵ���: " + this.tbDataPath.Text, LogLevel.Info);
                }
                catch (Exception ex)
                {
                    // �����쳣�����������Ϣ
                    UpdateLog(ex.Message, LogLevel.Error);
                }
            }
            if (string.IsNullOrWhiteSpace(this.tbCfgPath.Text))
            {
                this.tbCfgPath.Text = Path.GetFullPath("../../../Properties/default.cfg");
            }
            
            this.cbUserBaudRate.Text = "115200";
            this.cbDataBaudRate.Text = "921600";
            //����жϺ���
            userPort.DataReceived += new SerialDataReceivedEventHandler(UserPort_DataReceived);
            //��ذ�ťdisabled
            this.btnSendParam.Enabled = false;
            this.btnSendParam.ForeColor = System.Drawing.Color.Gray;
            this.btnStartLadar.Enabled = false;
            this.btnStartLadar.ForeColor = System.Drawing.Color.Gray;
            this.btnStopLadar.Enabled = false;
            this.btnStopLadar.ForeColor = System.Drawing.Color.Gray;

            //��ͼ��ʼ��
            traBitmap = new Bitmap(pbTrajectory.Size.Width, pbTrajectory.Size.Height);
            traGraphics = Graphics.FromImage(traBitmap);
            traGraphics.Clear(Color.WhiteSmoke);

            leftBottom = new PointF(30, traBitmap.Height - 30);
            rightTop = new PointF(traBitmap.Width - 30, 30);
            PointF CenterP = new PointF((leftBottom.X + rightTop.X) / 2, leftBottom.Y);

            //��x��
            traGraphics.DrawLine(Pens.Black, leftBottom.X, leftBottom.Y, rightTop.X, leftBottom.Y);
            PointF[] xpt = new PointF[3] { new PointF(rightTop.X + 8, leftBottom.Y), new PointF(rightTop.X, leftBottom.Y + 4), new PointF(rightTop.X, leftBottom.Y - 4) };//x��������
            traGraphics.DrawPolygon(Pens.Black, xpt);
            traGraphics.FillPolygon(new SolidBrush(Color.Black), xpt);
            traGraphics.DrawString("����/m", new Font("����", 10), Brushes.Black, new PointF(traBitmap.Width - 60, leftBottom.Y - 20));
            //��y��
            traGraphics.DrawLine(Pens.Black, CenterP.X, CenterP.Y, CenterP.X, rightTop.Y);
            PointF[] ypt = new PointF[3] { new PointF(CenterP.X, rightTop.Y - 8), new PointF(CenterP.X - 4, rightTop.Y), new PointF(CenterP.X + 4, rightTop.Y) };//y��������
            traGraphics.DrawPolygon(Pens.Black, ypt);
            traGraphics.FillPolygon(new SolidBrush(Color.Black), ypt);
            traGraphics.DrawString("����/m", new Font("����", 10), Brushes.Black, new PointF(CenterP.X + 9, rightTop.Y - 10));
            //��ǿ̶�
            axisX = (rightTop.X - leftBottom.X) / 1020;
            axisY = (leftBottom.Y - rightTop.Y) / 510;
            for (int i = 1; i <= 10; i++)
            {
                traGraphics.DrawString((i * 50).ToString(), new Font("����", 9), Brushes.Black, new PointF(CenterP.X - 25, CenterP.Y - i * 50 * axisY - 5));
                traGraphics.DrawLine(Pens.Black, CenterP.X - 3, CenterP.Y - i * 50 * axisY, CenterP.X, CenterP.Y - i * 50 * axisY);
            }
            for (int i = -5; i <= 5; i++)
            {
                if (i != 0)
                {
                    traGraphics.DrawString((i * 100).ToString(), new Font("����", 9), Brushes.Black, new PointF(leftBottom.X + (10 + (i + 5) * 100) * axisX - 10, leftBottom.Y + 6));
                    traGraphics.DrawLine(Pens.Black, leftBottom.X + (10 + (i + 5) * 100) * axisX, leftBottom.Y - 3, leftBottom.X + (10 + (i + 5) * 100) * axisX, leftBottom.Y);
                }
            }
            pbTrajectory.Image = traBitmap;
        }
        private void shipdock_Close(object sender, FormClosingEventArgs e)
        {
            var confirmForm = new Form();
            confirmForm.StartPosition = FormStartPosition.CenterParent;  // �����ڸ�����������ʾ
            var result = MessageBox.Show(this, "ȷ��Ҫ�˳���?", "�˳�ȷ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                // ȡ������رղ���
                e.Cancel = true;
            }
        }
        //private void btnfirmware_Click(object sender, EventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();

        //    // ���öԻ���ı���ͳ�ʼĿ¼����ѡ��
        //    openFileDialog.Title = "ѡ��̼��ļ�";
        //    openFileDialog.InitialDirectory = @"C:\"; // Ĭ����ʼ·�����ɸ�����Ҫ����

        //    // ���ù�������ֻ��ʾ�̼���ص��ļ�������.bin��.hex�ȣ�
        //    openFileDialog.Filter = "�̼��ļ�|*.bin;*.hex|�����ļ�|*.*";

        //    // ����û�ѡ�����ļ�������ˡ��򿪡�
        //    if (openFileDialog.ShowDialog() == DialogResult.OK)
        //    {
        //        // ��ȡ��ѡ�ļ���·�������õ� TextBox ��
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
            // ���û����ָ���Ŀؼ�
            tbLogPath.Enabled = IsLog.Checked;
            btnLog.Enabled = IsLog.Checked;

            // ��ѡ������״̬�ı�ؼ�����ʽ����ʾ��Ϣ
            if (IsLog.Checked)
            {
                // ���ÿؼ�ʱ��������ʾ��Ϣ��ؼ���ʽ
                tbLogPath.BackColor = System.Drawing.Color.White;
                IsLog.ForeColor = System.Drawing.Color.Black;
                string LogPathname = this.tbLogPath.Text + "Log" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                logWriter = new StreamWriter(LogPathname, true);  // true ��ʾ׷��д��
                isLogWriterOpen = true;
                UpdateLog("������־ϵͳ", LogLevel.Info);
            }
            else
            {
                // ���ÿؼ�ʱ��������ʾ��Ϣ��ؼ���ʽ
                IsLog.ForeColor = System.Drawing.Color.Gray;
                tbLogPath.BackColor = System.Drawing.Color.LightGray;
                UpdateLog("�ر���־ϵͳ", LogLevel.Info);
                isLogWriterOpen = false;
                logWriter.Close();

            }
        }

        //����Log
        private void UpdateLog(string message, LogLevel loglevel)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            message = message.Trim() + $"{"     "}[{timestamp}]{Environment.NewLine}";
            message = $"[{loglevel}]   " + message;
            // ����Ƿ��� UI �߳�
            if (Log.InvokeRequired)
            {
                // ����ڷ� UI �̣߳�ʹ�� Invoke �л��� UI �߳�
                Log.Invoke(new Action<string, LogLevel>(UpdateLog), new object[] { message, loglevel });
            }
            else
            {
                // �� UI �߳��и�����־�� RichTextBox
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
                // ����־ͬʱд�� txt �ļ�
                if (isLogWriterOpen)
                {
                    logWriter.WriteLine(message);
                    logWriter.Flush();  // ȷ��ʵʱд���ļ�
                }
            }
            Log.ScrollToCaret();
        }
        //��Ǵ�ֻ
        private void DrawChart(Graphics graphics, PointF TruePosition, float radius, Brush brush)
        {
            PointF center = new PointF((TruePosition.X + 500) * axisX + leftBottom.X, leftBottom.Y - TruePosition.Y * axisY);
            PointF[] points = new PointF[10];
            double angle = Math.PI / 5; // 36�� in radians

            for (int i = 0; i < 5; i++)
            {
                // ��Ȧ����
                points[i * 2] = new PointF(
                    center.X + (float)(radius * Math.Cos(i * 2 * angle)),
                    center.Y - (float)(radius * Math.Sin(i * 2 * angle))
                );

                // ��Ȧ����
                points[i * 2 + 1] = new PointF(
                    center.X + (float)(radius / 2 * Math.Cos(i * 2 * angle + angle)),
                    center.Y - (float)(radius / 2 * Math.Sin(i * 2 * angle + angle))
                );
            }
            graphics.FillPolygon(brush, points); // ��������
        }
        private void tbLogPath_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LogPath = this.tbLogPath.Text;
            Properties.Settings.Default.Save();
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
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
                UpdateLog("cfg�ļ�·��Ϊ��", LogLevel.Error);
                return;
            }
            else if (!this.tbCfgPath.Text.EndsWith(".cfg", StringComparison.OrdinalIgnoreCase) && !this.tbCfgPath.Text.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                UpdateLog("ָ��cfg�ļ�����,������.cfg��.txt", LogLevel.Error);
                return;
            }
            // ����һ���µĴ���ʵ��
            ParamForm paramForm = new ParamForm();
            // ��ʾ�µĴ���
            paramForm.Show();
        }

        private void btnSendParam_Click(object sender, EventArgs e)
        {
            using (StreamReader reader = new StreamReader(this.tbCfgPath.Text))
            {
                string line;
                int NumDone = 0;
                int NumCLI = 0;
                // ���ж�ȡ�ļ�����
                while ((line = reader.ReadLine()) != null)
                {
                    // ��������� "%"������������
                    if (line.StartsWith("%"))
                    {
                        continue;
                    }
                    // ������Ч���У�����%��ͷ��
                    userPort.WriteLine(line);
                    NumCLI++;
                    Thread.Sleep(50);
                    if (userbufferIndex > 0)
                    {
                        byte[] receiveData = new byte[userbufferIndex];
                        Array.Copy(userbuffer, receiveData, userbufferIndex);
                        userbufferIndex = 0;
                        if (BitConverter.ToString(receiveData) == "Done")
                        {
                            NumDone++;
                        }
                        else
                        {
                            UpdateLog(line, LogLevel.Warning);
                        }
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
            IsChangeStart = true;
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
                userPort.Open();
                UpdateLog("�û��˿��Ѵ�", LogLevel.Info);
                dataPort.Open();
                UpdateLog("���ݶ˿��Ѵ�", LogLevel.Info);
                //Thread CheckPortThread = new Thread(IsConnectPort);
                //CheckPortThread.Start();
                IsConnectPort();
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
            for (int i = 0; i < 20; i++)
            {
                userPort.WriteLine("configDataPort " + this.cbDataBaudRate.Text + " 1");
                Thread.Sleep(50);
                if (userbufferIndex > 0)
                {
                    byte[] receiveData = new byte[userbufferIndex];
                    Array.Copy(userbuffer, receiveData, userbufferIndex);
                    userbufferIndex = 0;
                    string reveive = JudgeReceive(Encoding.ASCII.GetString(receiveData));
                    if (reveive.Trim() == "Done")
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
                    UpdateLog(reveive, LogLevel.Info);
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
            sp.Read(userbuffer, userbufferIndex, bytesToRead);
            userbufferIndex = userbufferIndex + bytesToRead;
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
        //�жϷ�����Ϣ
        private string JudgeReceive(string receivedata)
        {
            string[] lines = receivedata.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            // ȥ����һ�к����һ��
            string result = string.Join(Environment.NewLine, lines, 1, lines.Length - 2);
            return result;
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
            userPort.WriteLine("sensorStop \n");
            Thread.Sleep(50);
            if (userbufferIndex > 0)
            {
                byte[] receiveData = new byte[userbufferIndex];
                Array.Copy(userbuffer, receiveData, userbufferIndex);
                userbufferIndex = 0;
                if (BitConverter.ToString(receiveData) == "Done")
                {
                    UpdateLog("�״�̽���ѹر�", LogLevel.Info);
                }
                else
                {
                    UpdateLog(BitConverter.ToString(receiveData), LogLevel.Error);
                }
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
                userPort.WriteLine("sensorStart \n");
                IsChangeStart = false;
            }
            else
            {
                userPort.WriteLine("sensorStart 0 \n");
            }
            Thread.Sleep(50);
            if (userbufferIndex > 0)
            {
                byte[] receiveData = new byte[userbufferIndex];
                Array.Copy(userbuffer, receiveData, userbufferIndex);
                userbufferIndex = 0;
                if (BitConverter.ToString(receiveData) == "Done")
                {
                    UpdateLog("�״�̽���ѿ���", LogLevel.Info);
                }
                else
                {
                    UpdateLog(BitConverter.ToString(receiveData), LogLevel.Error);
                }
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
    }
}