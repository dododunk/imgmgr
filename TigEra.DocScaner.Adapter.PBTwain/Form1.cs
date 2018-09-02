using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TigEra.DocScaner.Adapter.PBTwain
{
    public partial class Form1 : Form
    {
        private imageScan imageWindow;

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Hello");
            imageWindow = new imageScan(0, 0, 0, 0, 6, 65, 10, 15);

            imageWindow.Show();
        }

        private void imageWindow_SubmitClicked(object sender, EventArgs e)
        {
            pictureBox1.Image = imageWindow.myImage;
            if (pictureBox1.Image != null)
                pictureBox1.Size = new Size(imageWindow.myImage.Width, imageWindow.myImage.Height);

            imageWindow.Close();
        }
    }
}