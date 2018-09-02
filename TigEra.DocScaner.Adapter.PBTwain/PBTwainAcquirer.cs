using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tigera.LibCommon;
using TigEra.DocScaner.Common;
using TigEra.DocScaner.Definition;

namespace TigEra.DocScaner.Adapter.PBTwain
{
    public class PBTwainAcquirer : IFileAcquirer
    {
        public bool Initialize(IAcquirerParam initparam = null)
        {
            _param = initparam;
            return true;
        }

        private IAcquirerParam _param;

        public bool Acquire()
        {
            FormImageScaner form = new FormImageScaner(_param);
            form.HideMode();

            form.Setting = this.GetSetting();
            //form.Visible = false;
            // form.Show();
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (var item in form.Images)
                {
                    if (OnAcquired != null)
                    {
                        this.OnAcquired(this, new TEventArg<string>(item));
                    }
                }
            }
            return true;
        }

        public void UnInitialize()
        {
        }

        #region

        public class NestSetting : IPropertiesSetting
        {
            public NestSetting(PBTwainAcquirer acq)
            {
                _acq = acq;
            }

            private PBTwainAcquirer _acq;

            [Browsable(false)]
            public string Name
            {
                get { return "扫描仪设置"; }
            }

            [Category("设置")]
            [Description("保存影像类型")]
            [DisplayName("保存影像类型")]
            public EImgType FType
            {
                get
                {
                    var ret = AppContext.Cur.Cfg.GetConfigParamValue(ConstString.SharpTwainSetting, "SaveImageType");
                    if (string.IsNullOrEmpty(ret))
                    {
                        return EImgType.jpeg;
                    }
                    try
                    {
                        var eret = (EImgType)Enum.Parse(typeof(EImgType), ret);
                        return eret;
                    }
                    catch
                    {
                        return EImgType.jpeg;
                    }
                }
                set
                {
                    AppContext.Cur.Cfg.SetConfigParamValue(ConstString.SharpTwainSetting, "SaveImageType", value.ToString());
                }
            }

            //[Category("设置")]
            //[Description("保存影像类型")]
            //[DisplayName("每次重新选择扫描仪")]
            //public bool ReSelectScanerEachTimer
            //{
            //    get
            //    {
            //        bool ret = IniConfigSetting.Cur.GetConfigParamValue(ConstString.SharpTwainSetting, "ResetSelectScaner").ToBool();
            //        return ret;
            //    }
            //    set
            //    {
            //        IniConfigSetting.Cur.SetConfigParamValue(ConstString.SharpTwainSetting, "ResetSelectScaner", value.ToString());
            //    }
            //}

            //[Category("设置")]
            //[Description("保存影像类型")]
            //[DisplayName("当前选择扫描仪索引")]
            //public int SelectedScanerIndex
            //{
            //    get
            //    {
            //        var ret = IniConfigSetting.Cur.GetConfigParamValue(ConstString.SharpTwainSetting, "SelectedScanerIndex").ToInt();
            //        return ret;
            //    }
            //    set
            //    {
            //        IniConfigSetting.Cur.SetConfigParamValue(ConstString.SharpTwainSetting, "SelectedScanerIndex", value.ToString());
            //    }
            //}

            [Category("设置")]
            [Description("保存影像路径")]
            [DisplayName("保存影像路径")]
            [Browsable(false)]
            public string ImageDir
            {
                get
                {
                    //var ret = AppContext.Cur.Cfg.GetConfigParamValue(ConstString.SharpTwainSetting, "ImageDir");
                    //if (string.IsNullOrEmpty(ret))
                    {
                        var ret = UserProfileSetting.Cur.TempDir;
                        return ret;
                    }
                }
                set
                {
                    UserProfileSetting.Cur.TempDir = value;
                    //   AppContext.Cur.Cfg.SetConfigParamValue(ConstString.cu.SharpTwainSetting, "ImageDir", value.ToString());
                }
            }

            [Category("设置")]
            [DisplayName("保存压缩比例")]
            public long ImgRatio //2014-12-30 Hu 修改，把int类型改为long，不然在处理图像后会发生异常。
            {
                get
                {
                    var ret = (long)AppContext.Cur.Cfg.GetConfigParamValue(ConstString.SharpTwainSetting, "ImgRatio").ToInt();
                    if (ret == 0L)
                        //ret = 100;
                        ret = 30L;
                    return ret;
                }
                set
                {
                    AppContext.Cur.Cfg.SetConfigParamValue(ConstString.SharpTwainSetting, "ImgRatio", value.ToString());
                }
            }
        }

        #endregion
        private NestSetting _setting;

        public event EventHandler<TEventArg<string>> OnAcquired;

        public event EventHandler<TEventArg<string>> OnError;

        public bool Initialized
        {
            get
            {
                return true;
            }
        }

        public string Name
        {
            get
            {
                return "PBTwain";
            }
        }

        public string CnName
        {
            get
            {
                return "新扫描仪";
            }
        }

        public NestSetting GetSetting()
        {
            if (_setting == null)
            {
                _setting = new NestSetting(this);
            }
            return _setting;
        }

        IPropertiesSetting IHasIPropertiesSetting.GetSetting()
        {
            return GetSetting();
        }

        public void Dispose()
        {
            // throw new NotImplementedException();
        }
    }
}