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
    public partial class PBPictureBox : System.Windows.Forms.UserControl
    {
        public PBPictureBox()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        private void imageBox_Click(object sender, EventArgs e)
        {
        }

        private void pictureStrip1_Load(object sender, EventArgs e)
        {
        }

        private void pictureStrip1_Clicked(object sender, EventArgs e)
        {
            UCPictureBoxClass pb = (UCPictureBoxClass)sender;

            imageBox.Image = pb.fullSizedImage;
        }
    }
}