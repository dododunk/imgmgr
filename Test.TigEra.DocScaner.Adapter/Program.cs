using System;
using System.Windows.Forms;
using Tigera.LibCommon;

namespace Test.TigEra.DocScaner.Adapter
{
    public class test
	{


		public DateTime Gx
		{
			get
			{
				return DateTime.Now;
			}

		}
	}
	static class Program
	{
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main()
		{

            var ret = FileHelper.SetFileVersion("asdfasf.txt",11);

			test v = new test();
			//SerializeHelper.SerializeToXML<test>(v, "D:\\test.xml");
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new TetForm());

		}
	}
}
