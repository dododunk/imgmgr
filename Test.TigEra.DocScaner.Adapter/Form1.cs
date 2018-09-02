using Logos.DocScaner.AdapterFactory;
using Logos.DocScaner.Common;
using Logos.DocScaner.Definition;
using System;
using System.Windows.Forms;


namespace Test.Logos.DocScaner.Adapter
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			this.comboBox1.Items.Add("SharpWebcam");
			this.comboBox1.Items.Add("SharpTwain");
			this.comboBox1.Items.Add("SharpDir");
			this.comboBox1.Items.Add("SharpFile");
			this.comboBox1.SelectedIndex = IniConfigSetting.Default.GetConfigParamValue("TEST", "SelectIndex").ToInt();

		}

		IImageAcquirer acq;
		SharpAcquirerFactory mgr = new SharpAcquirerFactory();
		private void button1_Click(object sender, EventArgs e)
		{
			IniConfigSetting.Default.SetConfigParamValue("TEST", "SelectIndex", this.comboBox1.SelectedIndex.ToString());
			this.textBox1.Text = "";

			if (acq != null)
			{
				acq.Dispose();
				acq = null;
			}
			acq = mgr.GetAdapter(this.comboBox1.Text);//"WebCam");
			acq.OnAcquired -= acq_OnAcquired;
			acq.OnError -= acq_OnError;
			acq.OnAcquired += acq_OnAcquired;
			acq.OnError += acq_OnError;
			acq.Initialize(null);
			acq.Acquire();
		}

		void acq_OnError(object sender, global::Logos.DocScaner.Definition.TEventArg<string> e)
		{
			MessageBox.Show(e.Arg);
		}

		void acq_OnAcquired(object sender, global::Logos.DocScaner.Definition.TEventArg<string> e)
		{
			this.textBox1.Text = this.textBox1.Text + Environment.NewLine + e.Arg;
		}
	}
}
