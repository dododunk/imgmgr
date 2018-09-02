using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace TigEra.DocScaner.Adapter.PBTwain
{
    partial class UCPictureBoxClass : PictureBox
    {
        public Image fullSizedImage;

        public UCPictureBoxClass()
        {
            fullSizedImage = null;
        }
    }
}