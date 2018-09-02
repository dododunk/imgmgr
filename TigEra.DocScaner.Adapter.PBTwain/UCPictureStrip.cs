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
    public partial class UCPictureStrip : System.Windows.Forms.UserControl
    {
        private int pictureOffSet = 0;
        private int[] pictureWidth = new int[100];
        private int maxPicture = 0, currentImage = 0;

        public delegate void SubmitClickedHandler(object sender, EventArgs e);

        [Category("Action")]
        [Description("Fires when the Submit button is clicked.")]
        public event SubmitClickedHandler SubmitClicked;

        public UCPictureStrip()
        {
            InitializeComponent();
            /*    AddPicture("C:\\Users\\Bob\\Pictures\\fathom.jpg");
                  AddPicture("C:\\Users\\Bob\\Pictures\\IANY2040_poweredBySQL.jpg");
                  AddPicture("C:\\Users\\Bob\\Pictures\\girl.jpg");
                  AddPicture("C:\\Users\\Bob\\Pictures\\fathom.jpg");
                  AddPicture("C:\\Users\\Bob\\Pictures\\girl.jpg");
                  AddPicture("C:\\Users\\Bob\\Pictures\\IANY2040_poweredBySQL.jpg");
                  AddPicture("C:\\Users\\Bob\\Pictures\\fathom.jpg");
                  AddPicture("C:\\Users\\Bob\\Pictures\\girl.jpg");
                  AddPicture("C:\\Users\\Bob\\Pictures\\IANY2040_poweredBySQL.jpg");
              */
        }

        //protected override void OnPaint(PaintEventArgs pe)
        //{
        //base.OnPaint(pe);
        //}
        /*
        public Image GetPicture()
        {
            PBPictureBoxClass myPictureBoxClass;

            foreach (Control c in this.panel2.Controls)
            {
                myPictureBoxClass = (PBPictureBoxClass)c;
                return (myPictureBoxClass.fullSizedImage);
           }
        }
    */

        public void AddPicture(string path)
        {
            int width, height;
            Bitmap bm = new Bitmap(path);

            height = panel1.Height - 10;
            width = Convert.ToInt32(((float)bm.Width / (float)bm.Height) * height);
            UCPictureBoxClass pb = new UCPictureBoxClass();
            pb.Name = "Neal";
            pb.Size = new Size(width, height);
            pb.Location = new Point(pictureOffSet, 0);
            pictureOffSet += width;
            pb.BorderStyle = BorderStyle.FixedSingle;
            pb.SizeMode = PictureBoxSizeMode.Normal;
            pb.fullSizedImage = bm;
            pb.Image = (Image)bm.GetThumbnailImage(width, height, null, IntPtr.Zero);
            pb.MouseEnter += new System.EventHandler(this.image_MouseEnter);
            pb.MouseLeave += new System.EventHandler(this.image_MouseLeave);
            pb.Click += new System.EventHandler(this.image_Click);
            pb.BorderStyle = BorderStyle.None;
            this.panel2.Controls.Add(pb);
            pictureWidth[maxPicture] = width;
            maxPicture++;

            if (maxPicture == 1)
                panel2.Size = new Size(width, panel2.Height);
            else
            {
                panel2.Size = new Size(panel2.Width + width, panel2.Height);
            }
        }

        public void AddPicture(Bitmap bm)
        {
            int width, height;

            height = panel1.Height - 10;
            width = Convert.ToInt32(((float)bm.Width / (float)bm.Height) * height);
            UCPictureBoxClass pb = new UCPictureBoxClass();
            pb.Name = "Neal";
            pb.Size = new Size(width, height);
            pb.Location = new Point(pictureOffSet, 0);
            pictureOffSet += width;
            pb.BorderStyle = BorderStyle.FixedSingle;
            pb.SizeMode = PictureBoxSizeMode.Normal;
            pb.fullSizedImage = bm;
            pb.Image = (Image)bm.GetThumbnailImage(width, height, null, IntPtr.Zero);
            pb.MouseEnter += new System.EventHandler(this.image_MouseEnter);
            pb.MouseLeave += new System.EventHandler(this.image_MouseLeave);
            pb.Click += new System.EventHandler(this.image_Click);
            pb.BorderStyle = BorderStyle.None;
            this.panel2.Controls.Add(pb);
            pictureWidth[maxPicture] = width;
            maxPicture++;

            if (maxPicture == 1)
                panel2.Size = new Size(width, panel2.Height);
            else
            {
                panel2.Size = new Size(panel2.Width + width, panel2.Height);
            }
        }

        private void FilmStripPanel_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //			for(int i=0;i< pictureBoxNumber-1;i++)
            //			{
            //				this.FilmStripPanel.Controls[i].Invalidate();
            //				this.FilmStripPanel.Controls[i].Update();
            //			}
        }

        private void FilmStripPanel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            //			for(int i=0;i< pictureBoxNumber-1;i++)
            //			{
            //				//this.FilmStripPanel.Controls[i].Invalidate();
            //				this.FilmStripPanel.Controls[i].Update();
            //			}
        }

        private void cb_left_Click(object sender, EventArgs e)
        {
            int newX = cb_left.Width;

            if (currentImage + 1 < maxPicture)
                currentImage++;
            else
                return;

            for (int i = 0; i < currentImage; i++)
            {
                newX -= pictureWidth[i];
            }

            this.panel2.Location = new Point(newX, 0);
        }

        private void cb_right_Click(object sender, EventArgs e)
        {
            int newX = cb_left.Width;

            if (currentImage > 0)
                currentImage--;
            else
                return;

            for (int i = 0; i < currentImage; i++)
            {
                newX -= pictureWidth[i];
            }

            this.panel2.Location = new Point(newX, 0);
        }

        private void image_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            pb.BorderStyle = BorderStyle.Fixed3D;
        }

        private void image_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            pb.BorderStyle = BorderStyle.None;
        }

        public void image_Click(object sender, EventArgs e)
        {
            // OnSubmitClicked(sender, e);
            SubmitClicked(sender, e);  // Notify Subscribers
        }
    }
}