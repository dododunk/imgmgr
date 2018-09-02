using System;
using System.Windows.Forms;
using Tigera.LibCommon;
using TigEra.DocScaner.Adapter.SharpTwain;
using TigEra.DocScaner.AdapterFactory;
using TigEra.DocScaner.Definition;

namespace Test.TigEra.DocScaner.Adapter
{
    public partial class TetForm : Form
    {
        public TetForm()
        {
            InitializeComponent();
            this.comboBox1.Items.Add("SharpWebcam");
            this.comboBox1.Items.Add("SharpTwain");
            this.comboBox1.Items.Add("SharpDir");
            this.comboBox1.Items.Add("SharpFile");
            //  this.comboBox1.SelectedIndex = IniConfigSetting.Default.GetConfigParamValue("TEST", "SelectIndex").ToInt();
            this.textBox1.DisableInput();
        }

        private IFileAcquirer acq;
        private SharpAcquirerFactory mgr = new SharpAcquirerFactory();

        private void button1_Click(object sender, EventArgs e)
        {
            // IniConfigSetting.Default.SetConfigParamValue("TEST", "SelectIndex", this.comboBox1.SelectedIndex.ToString());
            this.textBox1.Text = "";

            if (acq != null)
            {
                acq.Dispose();
                acq = null;
            }
            acq = new SharpTwainAcquirer();// mgr.GetAdapter(this.comboBox1.Text);//"WebCam");
            acq.OnAcquired -= acq_OnAcquired;
            acq.OnError -= acq_OnError;
            acq.OnAcquired += acq_OnAcquired;
            acq.OnError += acq_OnError;
            acq.Initialize(null);
            acq.Acquire();
        }

        private void acq_OnError(object sender, global::TigEra.DocScaner.Definition.TEventArg<string> e)
        {
            MessageBox.Show(e.Arg);
        }

        private void acq_OnAcquired(object sender, global::TigEra.DocScaner.Definition.TEventArg<string> e)
        {
            this.textBox1.Text = this.textBox1.Text + Environment.NewLine + e.Arg;
        }
    }
}