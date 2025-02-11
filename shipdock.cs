using System.Reflection.Metadata;
using System.Windows.Forms;
using System.IO.Ports;
using static System.Windows.Forms.DataFormats;

namespace shipdock
{
    public partial class shipdock : Form
    {
        private StreamWriter logWriter;
        private Bitmap traBitmap;
        private Graphics traGraphics;
        private PointF rightTop, leftBottom;
        private float axisX, axisY;
        SerialPort userPort, dataPort;
        public shipdock()
        {
            InitializeComponent();
        }
        // ���ش���
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
                    // �����ļ���
                    Directory.CreateDirectory(this.logPath.Text);
                    UpdateLog("�ļ����Ѵ���: " + this.logPath.Text);
                }
                catch (Exception ex)
                {
                    // �����쳣�����������Ϣ
                    UpdateLog("�����ļ���ʱ��������: " + ex.Message);
                }
            }
            if (string.IsNullOrWhiteSpace(this.DataPath.Text))
            {
                this.DataPath.Text = Path.GetFullPath("../../../data/");
                try
                {
                    // �����ļ���
                    Directory.CreateDirectory(this.DataPath.Text);
                    UpdateLog("�ļ����Ѵ���: " + this.DataPath.Text);
                }
                catch (Exception ex)
                {
                    // �����쳣�����������Ϣ
                    UpdateLog("�����ļ���ʱ��������: " + ex.Message);
                }
            }
            string[] ports = SerialPort.GetPortNames();

            // ���֮ǰ�Ķ˿�
            cbUserPort.Items.Clear();

            // ����п��õĶ˿ڣ���ӵ�ComboBox
            if (ports.Length > 0)
            {
                cbUserPort.Items.AddRange(ports);
                cbDataPort.Items.AddRange(ports);
                UpdateLog("�˿ڿ�������:" + ports.Length.ToString());
            }
            else
            {
                UpdateLog("û�п��õĶ˿ڣ�");
            }
            this.cbUserBaudRate.Text = "115200";
            this.cbDataBaudRate.Text = "921600";
            this.ProgramStart.Text = "ϵͳ����ʱ��Ϊ:  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

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
            logPath.Enabled = IsLog.Checked;
            btnLog.Enabled = IsLog.Checked;

            // ��ѡ������״̬�ı�ؼ�����ʽ����ʾ��Ϣ
            if (IsLog.Checked)
            {
                // ���ÿؼ�ʱ��������ʾ��Ϣ��ؼ���ʽ
                logPath.BackColor = System.Drawing.Color.White;
                IsLog.ForeColor = System.Drawing.Color.Black;
                string LogPathname = this.logPath.Text + "Log" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                logWriter = new StreamWriter(LogPathname, true);  // true ��ʾ׷��д��
                UpdateLog("������־ϵͳ");
            }
            else
            {
                // ���ÿؼ�ʱ��������ʾ��Ϣ��ؼ���ʽ
                IsLog.ForeColor = System.Drawing.Color.Gray;
                logPath.BackColor = System.Drawing.Color.LightGray;
                UpdateLog("�ر���־ϵͳ");
                logWriter.Close();

            }
        }

        //����Log
        private void UpdateLog(string message)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            message = message + $"{"     "}[{timestamp}]{Environment.NewLine}";
            // ����Ƿ��� UI �߳�
            if (Log.InvokeRequired)
            {
                // ����ڷ� UI �̣߳�ʹ�� Invoke �л��� UI �߳�
                Log.Invoke(new Action<string>(UpdateLog), new object[] { message });
            }
            else
            {
                // �� UI �߳��и�����־�� RichTextBox
                Log.AppendText(message);

                // ����־ͬʱд�� txt �ļ�
                if (logWriter != null)
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
        private void logPath_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LogPath = this.logPath.Text;
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
                this.logPath.Text = folderDialog.SelectedPath + @"\";
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
            // ����һ���µĴ���ʵ��
            ParamForm paramForm = new ParamForm();

            // ��ʾ�µĴ���
            paramForm.Show();
        }

        private void btnSendParam_Click(object sender, EventArgs e)
        {

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            //�����û��˿ں����ݶ˿�
            userPort.PortName = this.cbUserPort.Text;
            userPort.BaudRate = int.Parse(this.cbUserBaudRate.Text);
            dataPort.PortName = this.cbDataPort.Text;
            dataPort.BaudRate = int.Parse(this.cbDataBaudRate.Text);
            try
            {
                userPort.Open();
                dataPort.Open();
                UpdateLog("�����Ѵ�");
            }
            catch (Exception ex)
            {
                UpdateLog("����: " + ex.Message);
            }
        }

        private void btnClosePort_Click(object sender, EventArgs e)
        {
            try
            {
                userPort.Close();
                dataPort.Close();
                UpdateLog("�����ѹر�");
            }
            catch (Exception ex)
            {
                UpdateLog("����: " + ex.Message);
            }
        }
    }
}