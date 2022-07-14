
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace OpenCV_Test_002
{
    public partial class Form1 : Form
    {
        VideoCapture capture;
        Mat frame;
        Bitmap image;
        private Thread camera;
        int isCameraRunning = 0;

        private void CaptureCamera()
        {
            //System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            //gp.AddEllipse(0, 0, pictureBox1.Width - 1, pictureBox1.Height - 1);
            //Region rg = new Region(gp);
            //pictureBox1.Region = rg;

            Rectangle Rect = new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height);
            GraphicsPath GraphPath = new GraphicsPath();

            GraphPath.AddArc(Rect.X, Rect.Y, 50, 50, 180, 90);
            GraphPath.AddArc(Rect.X + Rect.Width - 50, Rect.Y, 50, 50, 270, 90);
            GraphPath.AddArc(Rect.X + Rect.Width - 50, Rect.Y + Rect.Height - 50, 50, 50, 0, 90);
            GraphPath.AddArc(Rect.X, Rect.Y + Rect.Height - 50, 50, 50, 90, 90);

            pictureBox1.Region = new Region(GraphPath);

            camera = new Thread(new ThreadStart(CaptureCameraCallback));
            camera.Start();
        }

        private void CaptureCameraCallback()
        {
            frame = new Mat();
            capture = new VideoCapture();
            capture.Open(0);
            
            while (isCameraRunning == 1)
            {
                capture.Read(frame);

                if (!frame.Empty())
                {

                    
                    image = BitmapConverter.ToBitmap(frame.Flip(FlipMode.Y));

                    //image.RotateFlip(RotateFlipType.RotateNoneFlipNone);

                    pictureBox1.Image = image;
                    //pictureBox1.Image.RotateFlip(RotateFlipType.Rotate180FlipXY);
                }
                image = null;
            }

        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text.Equals("Start"))
            {
                CaptureCamera();
                button1.Text = "Stop";
                isCameraRunning = 1;
            }
            else
            {
                if (capture.IsOpened())
                {
                    capture.Release();
                }

                button1.Text = "Start";
                isCameraRunning = 0;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (camera.IsAlive)
            {
                camera.Abort();
                //capture.Dispose();
            }

        }
    }
}
