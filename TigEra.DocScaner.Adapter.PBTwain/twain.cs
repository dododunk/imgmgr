using System;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace TigEra.DocScaner.Adapter.PBTwain
{
    public enum TwainRc
    {
        TWRC_SUCCESS = 0,
        TWRC_FAILURE = 1, /* Application may get TW_STATUS for info on failure */
        TWRC_CHECKSTATUS = 2, /* "tried hard"; get status                  */
        TWRC_CANCEL = 3,
        TWRC_DSEVENT = 4,
        TWRC_NOTDSEVENT = 5,
        TWRC_XFERDONE = 6,
        TWRC_ENDOFLIST = 7, /* After MSG_GETNEXT if nothing left         */
        TWRC_INFONOTSUPPORTED = 8,
        TWRC_DATANOTAVAILABLE = 9
    }

    public enum TwainCommand
    {
        Not = -1,
        Null = 0,
        TransferReady = 1,
        CloseRequest = 2,
        CloseOk = 3,
        DeviceEvent = 4
    }

    internal class twain
    {
        private const short CountryUSA = 1;
        private const short LanguageUSA = 13;
        private const short LanguageCHINESE = 37;

        public twain()
        {
            appid = new twainDefs.TwIdentity();
            appid.Id = IntPtr.Zero;
            appid.Version.MajorNum = 1;
            appid.Version.MinorNum = 1;
            appid.Version.Language = LanguageCHINESE;
            appid.Version.Country = CountryUSA;
            appid.Version.Info = "Version 2.0";
            appid.ProtocolMajor = twainDefs.TwProtocol.Major;
            appid.ProtocolMinor = twainDefs.TwProtocol.Minor;
            appid.SupportedGroups = (int)(twainDefs.TwDG.Image | twainDefs.TwDG.Control);
            appid.Manufacturer = "PBSoftware";
            appid.ProductFamily = "InfinityPOS";
            appid.ProductName = "PBSoftware TWAIN";

            srcds = new twainDefs.TwIdentity();
            //srcds.Id = IntPtr.Zero;

            evtmsg.EventPtr = Marshal.AllocHGlobal(Marshal.SizeOf(winmsg));
        }

        ~twain()
        {
            Marshal.FreeHGlobal(evtmsg.EventPtr);
        }

        public void Init(IntPtr hwndp)
        {
            Finish();
            twainDefs.TwRC rc = DSMparent(appid, IntPtr.Zero, twainDefs.TwDG.Control, twainDefs.TwDAT.Parent, twainDefs.TwMSG.OpenDSM, ref hwndp);
            if (rc == twainDefs.TwRC.Success)
            {
                rc = DSMident(appid, IntPtr.Zero, twainDefs.TwDG.Control, twainDefs.TwDAT.Identity, twainDefs.TwMSG.GetDefault, srcds);
                if (rc == twainDefs.TwRC.Success)
                    hwnd = hwndp;
                else
                    rc = DSMparent(appid, IntPtr.Zero, twainDefs.TwDG.Control, twainDefs.TwDAT.Parent, twainDefs.TwMSG.CloseDSM, ref hwndp);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        // Function: SetImageInfo --
        // Author: TWAIN Working Group
        // Input:
        // Output: none
        // Comments:
        //
        public twainDefs.TwRC SetImageLayout(short topWhole, short topFrac, short leftWhole, short leftFrac, short rightWhole, short rightFrac, short bottomWhole, short bottomFrac)
        {
            twainDefs.TwRC rc = 0;
            twainDefs.TwImageLayout twImageLayout = new twainDefs.TwImageLayout();

            // Check ImageInfo information
            //rc = CallDSMEntry(appID, dsID, DG_IMAGE, DAT_IMAGELAYOUT, MSG_GET, (TW_MEMREF) twImageLayout);

            rc = DSMImageLayout(appid, srcds, twainDefs.TwDG.Image, twainDefs.TwDAT.ImageLayout, twainDefs.TwMSG.Get, twImageLayout);

            switch (rc)
            {
                case twainDefs.TwRC.Success:
                    twImageLayout.Frame.Left.Whole = leftWhole;
                    twImageLayout.Frame.Left.Frac = (ushort)(leftFrac / 100.0 * 65536);
                    twImageLayout.Frame.Top.Whole = topWhole;
                    twImageLayout.Frame.Top.Frac = (ushort)(topFrac / 100.0 * 65536);
                    twImageLayout.Frame.Right.Whole = rightWhole;
                    twImageLayout.Frame.Right.Frac = (ushort)(rightFrac / 100.0 * 65536);
                    twImageLayout.Frame.Bottom.Whole = bottomWhole;
                    twImageLayout.Frame.Bottom.Frac = (ushort)(bottomFrac / 100.0 * 65536);

                    //rc = CallDSMEntry(&appID, &dsID, DG_IMAGE, DAT_IMAGELAYOUT, MSG_SET, (TW_MEMREF) & twImageLayout);
                    rc = DSMImageLayout(appid, srcds, twainDefs.TwDG.Image, twainDefs.TwDAT.ImageLayout, twainDefs.TwMSG.Set, twImageLayout);
                    twainDefs.TwStatus dsmstat1 = new twainDefs.TwStatus();
                    rc = DSMstatus(appid, srcds, twainDefs.TwDG.Control, twainDefs.TwDAT.Status, twainDefs.TwMSG.Get, dsmstat1);
                    break;

                case twainDefs.TwRC.Failure:
                    twainDefs.TwStatus dsmstat = new twainDefs.TwStatus();
                    rc = DSMstatus(appid, srcds, twainDefs.TwDG.Control, twainDefs.TwDAT.Status, twainDefs.TwMSG.Get, dsmstat);
                    break;

                default: break;
            }

            return (rc);
        }

        public void Select()
        {
            twainDefs.TwRC rc = 0;
            CloseSrc();

            if (appid.Id == IntPtr.Zero)
            {
                Init(hwnd);
                if (appid.Id == IntPtr.Zero)
                    return;
            }

            rc = DSMident(appid, IntPtr.Zero, twainDefs.TwDG.Control, twainDefs.TwDAT.Identity, twainDefs.TwMSG.UserSelect, srcds);
        }

        public bool Setup()
        {
            twainDefs.TwRC rc = 0;
            CloseSrc();

            if (appid.Id == IntPtr.Zero)
            {
                Init(hwnd);

                if (appid.Id == IntPtr.Zero)
                    return false;
            }

            rc = DSMident(appid, IntPtr.Zero, twainDefs.TwDG.Control, twainDefs.TwDAT.Identity, twainDefs.TwMSG.OpenDS, srcds);

            if (rc != twainDefs.TwRC.Success)
                return false;

            twainDefs.TwCapability cap = new twainDefs.TwCapability(twainDefs.TwCap.CAP_ENABLEDSUIONLY);
            rc = DScap(appid, srcds, twainDefs.TwDG.Control, twainDefs.TwDAT.Capability, twainDefs.TwMSG.Get, cap);

            if (rc != twainDefs.TwRC.Success)
            {
                CloseSrc();
                return false;
            }

            twainDefs.TwUserInterface guif = new twainDefs.TwUserInterface();
            guif.ShowUI = 1;
            guif.ModalUI = 0;
            guif.ParentHand = hwnd;
            rc = DSuserif(appid, srcds, twainDefs.TwDG.Control, twainDefs.TwDAT.UserInterface, twainDefs.TwMSG.EnableDSUIOnly, guif);

            if (rc != twainDefs.TwRC.Success)
            {
                CloseSrc();
                return false;
            }

            return true;
        }

        //
        // Function Acquire(int);
        //
        // aNumberOfPages : -1 duplex and multipage scanning support
        //                : 1 or greater scan that many images in.
        //
        public bool Acquire(short aNumberOfPages)
        {
            twainDefs.TwRC rc = 0;
            CloseSrc();
            if (appid.Id == IntPtr.Zero)
            {
                Init(hwnd);
                if (appid.Id == IntPtr.Zero)
                    return (false);
            }

            rc = DSMident(appid, IntPtr.Zero, twainDefs.TwDG.Control, twainDefs.TwDAT.Identity, twainDefs.TwMSG.OpenDS, srcds);

            if (rc != twainDefs.TwRC.Success)
                return (false);

            rc = SetImageLayout(1, 0, 1, 0, 6, 65, 7, 15);
            twainDefs.TwCapability cap = new twainDefs.TwCapability(twainDefs.TwCap.XferCount, aNumberOfPages);
            rc = DScap(appid, srcds, twainDefs.TwDG.Control, twainDefs.TwDAT.Capability, twainDefs.TwMSG.Set, cap);

            if (rc != twainDefs.TwRC.Success)
            {
                CloseSrc();
                return (false);
            }

            twainDefs.TwUserInterface guif = new twainDefs.TwUserInterface();
            guif.ShowUI = 1;
            guif.ModalUI = 1;
            guif.ParentHand = hwnd;
            rc = DSuserif(appid, srcds, twainDefs.TwDG.Control, twainDefs.TwDAT.UserInterface, twainDefs.TwMSG.EnableDS, guif);

            if (rc != twainDefs.TwRC.Success)
            {
                CloseSrc();
                return (false);
            }

            return (true);
        }

        public ArrayList TransferPictures()
        {
            ArrayList pics = new ArrayList();
            if (srcds.Id == IntPtr.Zero)
                return pics;

            twainDefs.TwRC rc = 0;
            IntPtr hbitmap = IntPtr.Zero;
            twainDefs.TwPendingXfers pxfr = new twainDefs.TwPendingXfers();

            do
            {
                pxfr.Count = 0;
                hbitmap = IntPtr.Zero;

                twainDefs.TwImageInfo iinf = new twainDefs.TwImageInfo();
                rc = DSiinf(appid, srcds, twainDefs.TwDG.Image, twainDefs.TwDAT.ImageInfo, twainDefs.TwMSG.Get, iinf);
                if (rc != twainDefs.TwRC.Success)
                {
                    CloseSrc();
                    return pics;
                }

                rc = DSixfer(appid, srcds, twainDefs.TwDG.Image, twainDefs.TwDAT.ImageNativeXfer, twainDefs.TwMSG.Get, ref hbitmap);
                if (rc != twainDefs.TwRC.XferDone)
                {
                    CloseSrc();
                    return pics;
                }

                rc = DSpxfer(appid, srcds, twainDefs.TwDG.Control, twainDefs.TwDAT.PendingXfers, twainDefs.TwMSG.EndXfer, pxfr);
                if (rc != twainDefs.TwRC.Success)
                {
                    CloseSrc();
                    return pics;
                }

                pics.Add(hbitmap);
            }
            while (pxfr.Count != 0);

            rc = DSpxfer(appid, srcds, twainDefs.TwDG.Control, twainDefs.TwDAT.PendingXfers, twainDefs.TwMSG.Reset, pxfr);
            return pics;
        }

        public TwainCommand PassMessage(ref Message m)
        {
            if (srcds.Id == IntPtr.Zero)
                return TwainCommand.Not;

            int pos = GetMessagePos();

            winmsg.hwnd = m.HWnd;
            winmsg.message = m.Msg;
            winmsg.wParam = m.WParam;
            winmsg.lParam = m.LParam;
            winmsg.time = GetMessageTime();
            winmsg.x = (short)pos;
            winmsg.y = (short)(pos >> 16);

            Marshal.StructureToPtr(winmsg, evtmsg.EventPtr, false);
            evtmsg.Message = 0;
            twainDefs.TwRC rc = DSevent(appid, srcds, twainDefs.TwDG.Control, twainDefs.TwDAT.Event, twainDefs.TwMSG.ProcessEvent, ref evtmsg);

            if (rc == twainDefs.TwRC.NotDSEvent)
                return TwainCommand.Not;
            if (evtmsg.Message == (short)twainDefs.TwMSG.XFerReady)
                return TwainCommand.TransferReady;
            if (evtmsg.Message == (short)twainDefs.TwMSG.CloseDSReq)
                return TwainCommand.CloseRequest;
            if (evtmsg.Message == (short)twainDefs.TwMSG.CloseDSOK)
                return TwainCommand.CloseOk;
            if (evtmsg.Message == (short)twainDefs.TwMSG.DeviceEvent)
                return TwainCommand.DeviceEvent;

            return TwainCommand.Null;
        }

        public void CloseSrc()
        {
            twainDefs.TwRC rc = 0;
            try
            {
                if (srcds.Id != IntPtr.Zero)
                {
                    twainDefs.TwUserInterface guif = new twainDefs.TwUserInterface();
                    rc = DSuserif(appid, srcds, twainDefs.TwDG.Control, twainDefs.TwDAT.UserInterface, twainDefs.TwMSG.DisableDS, guif);
                    rc = DSMident(appid, IntPtr.Zero, twainDefs.TwDG.Control, twainDefs.TwDAT.Identity, twainDefs.TwMSG.CloseDS, srcds);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void Finish()
        {
            twainDefs.TwRC rc = 0;
            CloseSrc();

            if (appid.Id != IntPtr.Zero)
                rc = DSMparent(appid, IntPtr.Zero, twainDefs.TwDG.Control, twainDefs.TwDAT.Parent, twainDefs.TwMSG.CloseDSM, ref hwnd);
            appid.Id = IntPtr.Zero;
        }

        private IntPtr hwnd;
        private twainDefs.TwIdentity appid;
        private twainDefs.TwIdentity srcds;
        private twainDefs.TwEvent evtmsg;
        private WINMSG winmsg;

        // ------ DSM entry point DAT_ variants:
        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern twainDefs.TwRC DSMparent([In, Out] twainDefs.TwIdentity origin, IntPtr zeroptr, twainDefs.TwDG dg, twainDefs.TwDAT dat, twainDefs.TwMSG msg, ref IntPtr refptr);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern twainDefs.TwRC DSMident([In, Out] twainDefs.TwIdentity origin, IntPtr zeroptr, twainDefs.TwDG dg, twainDefs.TwDAT dat, twainDefs.TwMSG msg, [In, Out] twainDefs.TwIdentity idds);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern twainDefs.TwRC DSMImageLayout([In, Out] twainDefs.TwIdentity origin, [In, Out] twainDefs.TwIdentity dest, twainDefs.TwDG dg, twainDefs.TwDAT dat, twainDefs.TwMSG msg, [In, Out] twainDefs.TwImageLayout idds);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern twainDefs.TwRC DSMstatus([In, Out] twainDefs.TwIdentity origin, [In, Out] twainDefs.TwIdentity dest, twainDefs.TwDG dg, twainDefs.TwDAT dat, twainDefs.TwMSG msg, [In, Out] twainDefs.TwStatus dsmstat);

        // ------ DSM entry point DAT_ variants to DS:
        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern twainDefs.TwRC DSuserif([In, Out] twainDefs.TwIdentity origin, [In, Out] twainDefs.TwIdentity dest, twainDefs.TwDG dg, twainDefs.TwDAT dat, twainDefs.TwMSG msg, twainDefs.TwUserInterface guif);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern twainDefs.TwRC DSevent([In, Out] twainDefs.TwIdentity origin, [In, Out] twainDefs.TwIdentity dest, twainDefs.TwDG dg, twainDefs.TwDAT dat, twainDefs.TwMSG msg, ref twainDefs.TwEvent evt);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern twainDefs.TwRC DSstatus([In, Out] twainDefs.TwIdentity origin, [In] twainDefs.TwIdentity dest, twainDefs.TwDG dg, twainDefs.TwDAT dat, twainDefs.TwMSG msg, [In, Out] twainDefs.TwStatus dsmstat);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern twainDefs.TwRC DScap([In, Out] twainDefs.TwIdentity origin, [In] twainDefs.TwIdentity dest, twainDefs.TwDG dg, twainDefs.TwDAT dat, twainDefs.TwMSG msg, [In, Out] twainDefs.TwCapability capa);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern twainDefs.TwRC DSiinf([In, Out] twainDefs.TwIdentity origin, [In] twainDefs.TwIdentity dest, twainDefs.TwDG dg, twainDefs.TwDAT dat, twainDefs.TwMSG msg, [In, Out] twainDefs.TwImageInfo imginf);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern twainDefs.TwRC DSixfer([In, Out] twainDefs.TwIdentity origin, [In] twainDefs.TwIdentity dest, twainDefs.TwDG dg, twainDefs.TwDAT dat, twainDefs.TwMSG msg, ref IntPtr hbitmap);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern twainDefs.TwRC DSpxfer([In, Out] twainDefs.TwIdentity origin, [In] twainDefs.TwIdentity dest, twainDefs.TwDG dg, twainDefs.TwDAT dat, twainDefs.TwMSG msg, [In, Out] twainDefs.TwPendingXfers pxfr);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern IntPtr GlobalAlloc(int flags, int size);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern IntPtr GlobalLock(IntPtr handle);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern bool GlobalUnlock(IntPtr handle);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern IntPtr GlobalFree(IntPtr handle);

        [DllImport("user32.dll", ExactSpelling = true)]
        private static extern int GetMessagePos();

        [DllImport("user32.dll", ExactSpelling = true)]
        private static extern int GetMessageTime();

        [DllImport("gdi32.dll", ExactSpelling = true)]
        private static extern int GetDeviceCaps(IntPtr hDC, int nIndex);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr CreateDC(string szdriver, string szdevice, string szoutput, IntPtr devmode);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        private static extern bool DeleteDC(IntPtr hdc);

        public static int ScreenBitDepth
        {
            get
            {
                IntPtr screenDC = CreateDC("DISPLAY", null, null, IntPtr.Zero);
                int bitDepth = GetDeviceCaps(screenDC, 12);
                bitDepth *= GetDeviceCaps(screenDC, 14);
                DeleteDC(screenDC);
                return bitDepth;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        internal struct WINMSG
        {
            public IntPtr hwnd;
            public int message;
            public IntPtr wParam;
            public IntPtr lParam;
            public int time;
            public int x;
            public int y;
        }
    }
}