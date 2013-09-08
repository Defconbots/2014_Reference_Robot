using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace TowerDefender
{
    public partial class Form1 : Form
    {
        VideoCaptureDevice videoSource;
        FilterInfoCollection videoDevices;
        private LaserController _controller;
        FrameProcessor processor;

        private bool _inSetColor = false;
        private bool _inSetCenter = false;

        public Form1()
        {
            InitializeComponent();

            _controller = new LaserController();

            processor = new FrameProcessor(_controller);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                comboBox1.Items.Clear();
                if (videoDevices.Count == 0)
                    throw new ApplicationException();

                foreach (FilterInfo device in videoDevices)
                {
                    comboBox1.Items.Add(device.Name);
                }
                comboBox1.SelectedIndex = 1; //make dafault to first cam
            }
            catch (ApplicationException)
            {
                comboBox1.Items.Add("No capture device on your system");
            }

            var ports = SerialPort.GetPortNames();
            foreach (var port in ports)
            {
                comboBox2.Items.Add(port);
            }
            comboBox2.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _controller.Connect(new SerialPort((string)comboBox2.SelectedItem, 57600));

            videoSource = new VideoCaptureDevice(videoDevices[comboBox1.SelectedIndex].MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
            videoSource.Start();
        }
        
        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();

            var processed = processor.ProcessFrame((Bitmap)eventArgs.Frame.Clone());

            try
            {
                pictureBox1.Invoke((Action)(() =>
                {
                    pictureBox1.Width = bitmap.Width;
                    pictureBox1.Height = bitmap.Height;
                    pictureBox1.Image = bitmap;

                    pictureBox2.Width = bitmap.Width;
                    pictureBox2.Height = bitmap.Height;
                    pictureBox2.Image = processed;
                }));
            }
            catch
            {
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoSource != null)
            {
                videoSource.SignalToStop();
            }
        }

        private void btnSetColor_Click(object sender, EventArgs e)
        {
            //_inSetColor = true;
            _inSetCenter = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var em = e as MouseEventArgs;
            if (_inSetColor)
            {
                var color = (pictureBox1.Image as Bitmap).GetPixel(em.X, em.Y);
                processor.SetColor(color);
                _inSetColor = false;
            }
            if (_inSetCenter)
            {
                processor.SetCenter(em.X, em.Y);
                _inSetCenter = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ((videoSource != null) && (videoSource is VideoCaptureDevice))
            {
                try
                {
                    ((VideoCaptureDevice)videoSource).DisplayPropertyPage(this.Handle);
                }
                catch (NotSupportedException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
