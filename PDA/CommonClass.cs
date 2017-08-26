using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Media;
using System.IO;
using Microsoft.Win32;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Data;

namespace PDA
{
    public class CommonClass
    {
        public static string Path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="directoryPath">路径</param>
        /// <returns></returns>
        public static bool CreaterDirectory(string directoryPath)
        {
            try
            {
                CommonClass.Path = CommonClass.Path.Replace("\\", "/");
                string[] strData = CommonClass.Path.Split('/');
                CommonClass.Path = "";
                for (int k = 0; k < strData.Length - 1; k++)
                {
                    CommonClass.Path += strData[k] + "\\";
                }
                CommonClass.Path += "File";

                if (!Directory.Exists(CommonClass.Path))
                {
                    Directory.CreateDirectory(CommonClass.Path);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 判断RFID卡是否有效
        /// </summary>
        /// <param name="rfid">要校验的RFID</param>
        public static bool ValidateRFID(String rfid)
        {
            DataTable dt =FunPublic.GetDt(string.Format("select * from Tm_RFID where STATUS = 'F' and RFIDLUN = '{0}'",rfid));
            if (dt.Rows.Count>0)
            {
                return true;
            }
            return false;
        }

        public static string[, ,] strCodeType = new string[,,] 
        {                    
          // {{"Bookland EAN","83","0"}}, //一维码
           {{"Code 93","9","0"}},
           {{"Code 11","10","0"}},
           {{"Chinese 2 of 5","408","0"}},
           {{"Codabar","7","0"}},
           {{"Composite CC-A/B","342","0"}},
           {{"Composite CC-C","341","0"}},
           {{"Composite TLC-39","371","0"}},
           {{"Code 128 Emulation","123","0"}},
           {{"Discrete 2 of 5","5","0"}},
           {{"ISSN EAN","617","0"}},
           {{"Korean 3 of 5","581","0"}},
           {{"MSI","11","0"}},
           {{"Matrix 2 of 5","618","0"}},
           {{"MicroPDF417","227","0"}},
          // {{"Trioptic Code 39","13","0"}},//  NOTE Trioptic Code 39 and Code 39 Full ASCII cannot be enabled simultaneously.
           {{"UPC-E1","12","0"}},   //一维码
           {{"USPS 4CB/One Code/Intelligent Mail","592","0"}},
           {{"UPU FICS Postal","611","0"}},
        
          
        };

        /// <summary>
        /// 新建文本 
        /// </summary>
        /// <param name="filepath">文本路径</param>
        /// <param name="contentStr"></param>
        /// <returns></returns>
        public static bool SaveFile(string filepath, List<string> contentStr)
        {
            try
            {
                FileStream filewriter = new FileStream(filepath, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(filewriter, Encoding.Default);
                for (int i = 0; i < contentStr.Count; i++)
                {
                    sw.WriteLine(contentStr[i]);
                }
                sw.Close();
                filewriter.Close();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 读取文本内容
        /// </summary>
        /// <param name="filepath">文本路径</param>
        /// <param name="content">文本内容</param>
        /// <returns></returns>
        public static bool ReadFile(string filepath, ref List<string> fileContent)
        {
            try
            {
                if (File.Exists(filepath) == false)
                {
                    return false;
                }
                // fileContent = new List<string>();
                StreamReader sr = new StreamReader(filepath, Encoding.Default);
                while (!sr.EndOfStream)
                {
                    fileContent.Add(sr.ReadLine());
                }
                sr.Close();
                return true;
            }
            catch (System.Exception)
            {
                //出现异常则返回空值        Abnormal returns the null
                return false;
            }
        }

        #region 2DAPI
        /********************************************  二维软解码声明  *******************************************/
        /**********************************************************************************************************/
        /*                                              						
        功能:    初始化二维软解码。                			
        参数:     无.     																														
        返回值:   返回TRUE表示初始化成功，返回FALSE表示初始化失败。                                                                                     
        */
        [DllImport("DeviceAPI.dll", EntryPoint = "SoftDecoding_Init")]
        public static extern bool SoftDecoding_Init();

        /*                                              						
        功能:     关闭二维软解码时用来释放资源。                			
        参数:     无.     																														
        返回值:   返回TRUE表示释放成功，返回FALSE表示释放失败。                                                                                     
        */
        [DllImport("DeviceAPI.dll", EntryPoint = "SoftDecoding_Deinit")]
        public static extern bool SoftDecoding_Deinit();

        /*                                              						
        功能:   设置二维头为解码模式，在SoftDecoding_Scan之前调用，连续扫描时只要调用一遍就可以了。                			
        参数:     无.     																														
        返回值:   返回TRUE表示设置成功，返回FALSE表示设置失败。                                                                                     
        */
        [DllImport("DeviceAPI.dll", EntryPoint = "SoftDecoding_Select_ScanMode")]
        public static extern bool SoftDecoding_Select_ScanMode();

        /*                                              						
        功能:   扫描条码并解码。                			
        输入:    nTimeout : 扫描超时时间，barcodeData: 条码数据存储区地址，bufSize: 条码存储区大少,根据条码数据量设置大少，默认推荐2048字节.
        输出:   ReadDataSize: 读到的条码数据字节个数																														
        返回值:   返回TRUE表示解码成功，返回FALSE表示解码失败。                                                                                     
        */
        [DllImport("DeviceAPI.dll", EntryPoint = "SoftDecoding_Scan")]
        public static extern bool SoftDecoding_Scan(uint nTimeout, byte[] bacodeData, uint bufSize, uint[] ReadDataSize);

        /*                                              						
           功能:   使能和关闭各类型的条码。                			
           输入:   码的类型和参数，具体用法参考应用程序例子。    																														
           返回值:   返回TRUE表示设置成功，返回FALSE表示设置失败。                                                                                     
           */
        [DllImport("DeviceAPI.dll", EntryPoint = "SoftDecoding_BarcodeType_OnOff")]
        public static extern bool SoftDecoding_BarcodeType_OnOff(byte[] parameter, int parameterSize);

        /*                                              						
        功能:   设置二维头为拍照模式，在SoftDecoding_Snapshot前调用，拍完照后要回到解码模式必须调用SoftDecoding_Select_ScanMode。                			
        参数:     无.     																														
        返回值:   返回TRUE表示设置成功，返回FALSE表示设置失败。                                                                                     
        */
        [DllImport("DeviceAPI.dll", EntryPoint = "SoftDecoding_Select_SnapshotMode")]
        public static extern bool SoftDecoding_Select_SnapshotMode();
        /*                                              						
        功能:   拍照，拍完会在wince根目录生成sdl.jpg文件。                			
        输入:    超时时间。    																														
        返回值:   返回TRUE表示拍照成功，返回FALSE表示拍照失败。                                                                                     
        */
        [DllImport("DeviceAPI.dll", EntryPoint = "SoftDecoding_Snapshot")]
        public static extern bool SoftDecoding_Snapshot(int nTimeout, string fileName);
        #endregion

        #region 端口切换
        #region
        /************************************************************************
        * SerialPortControl_Ex(UINT8 uPortID, UINT8 uValue);
         * 端口控制 通过该函数进行上电、下电等操作                              
         * nPortID 端口号；uValue 参数值 0 低电平；1 高电平                     
        ************************************************************************/
        [DllImport("DeviceAPI.dll", EntryPoint = "SerialPortControl_Ex")]
        public static extern int SerialPortControl_Ex(byte port, byte s);


        /************************************************************************
        * SerialPortSwitch_Ex(int iPort);
        *  端口选择，用于端口间的相互切换；                         
        * iPort 端口号:0 RFID；1 外接串口；2 Barcode；3 GPS；                    
        ************************************************************************/
        [DllImport("DeviceAPI.dll", EntryPoint = "SerialPortSwitch_Ex")]
        public static extern void SerialPortSwitch_Ex(int iPort);
        #endregion
        #endregion

        #region 获取文件的icon

        /// <summary>
        /// 获取文件类型的关联图标
        /// </summary>
        /// <param name="fileName">文件类型的扩展名或文件的绝对路径</param>
        /// <param name="isLargeIcon">是否返回大图标</param>
        /// <returns>获取到的图标</returns>
        public static Icon GetIcon(string fileName, bool isLargeIcon)
        {
            SHFILEINFO shfi = new SHFILEINFO();
            IntPtr hI;

            if (isLargeIcon)
                hI = SHGetFileInfo(fileName, 0, ref shfi, (uint)Marshal.SizeOf(shfi), SHGFI_ICON | SHGFI_USEFILEATTRIBUTES | SHGFI_LARGEICON);
            else
                hI = SHGetFileInfo(fileName, 0, ref shfi, (uint)Marshal.SizeOf(shfi), SHGFI_ICON | SHGFI_USEFILEATTRIBUTES | SHGFI_SMALLICON);

            Icon icon = Icon.FromHandle(shfi.hIcon).Clone() as Icon;

            DestroyIcon(shfi.hIcon); //释放资源
            return icon;
        }




        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        [DllImport("coredll.dll", EntryPoint = "SHGetFileInfo", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        [DllImport("coredll.dll", EntryPoint = "DestroyIcon")]
        public static extern int DestroyIcon(IntPtr hIcon);

        #region API 参数的常量定义

        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0; //大图标 32×32
        public const uint SHGFI_SMALLICON = 0x1; //小图标 16×16
        public const uint SHGFI_USEFILEATTRIBUTES = 0x10;

        #endregion





        /// <summary>
        /// 使用选定的刷子、源位图和ROP3码绘制选定的矩形
        /// 获得屏幕图形并将它写入内存中的一个位图中(截屏)
        /// </summary>
        /// <param name="hdcDest">目的上下文设备的句柄 </param>
        /// <param name="nXDest">目的图形的左上角的x坐标 </param>
        /// <param name="nYDest">目的图形的左上角的y坐标 </param>
        /// <param name="nWidth">目的图形的矩形宽度 </param>
        /// <param name="nHeight">目的图形的矩形高度 </param>
        /// <param name="hdcSrc">源上下文设备的句柄</param>
        /// <param name="nXSrc">源图形的左上角的x坐标</param>
        /// <param name="nYSrc">源图形的左上角的x坐标</param>
        /// <param name="dwRop">光栅操作代码 </param>
        /// <returns></returns>
        [DllImport("CoreDLL.dll")]
        public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        #endregion

        #region 引用coredll
        [DllImport("coredll.dll")]
        public static extern void GwesPowerOffSystem();     //程序控制设备进入待机状态	Stand by mode

        [DllImport("coredll.dll")]
        public static extern void TouchCalibrate();         //弹出触摸笔校准界面


        //ChangeDisplaySettingsEx 旋转屏幕的函数

        #endregion

        /// <summary>
        /// 扫描键的键值
        /// </summary>
        public static int pda_SCAN_KEY = 238;

        /// <summary>
        /// 获取当前程序的路径
        /// </summary>
        /// <returns></returns>
        public static string getCurPath()
        {
            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
        }

        private const string filePath = @"\Windows\Barcodebeep.wav";

        public static bool PlaySound()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    SoundPlayer player = new SoundPlayer(filePath);

                    player.Play();
                }
                return true;

            }
            catch (System.Exception)
            {
                return false;
            }

        }

        private static string filePath1 = @"\Windows\beep.wav";

        public static bool PlaySoundBeep()
        {
            try
            {
                if (File.Exists(filePath1))
                {
                    SoundPlayer player = new SoundPlayer(filePath1);

                    player.Play();
                }
                return true;

            }
            catch (System.Exception)
            {
                return false;
            }
        }

        private static string filePath2 = @"\Windows\\beep1.wav";

        public static bool PlaySoundBeep1()
        {
            try
            {
                if (File.Exists(filePath2))
                {
                    SoundPlayer player = new SoundPlayer(filePath2);

                    player.Play();
                }
                return true;

            }
            catch (System.Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// 调用触摸笔校准程序，直接进入触摸笔校准界面
        /// </summary>
        public static void TouchTest()
        {
            TouchCalibrate();
        }

        /// <summary>
        /// 系统进入待机状态
        /// </summary>
        public static void PowerOffSystem()
        {
            GwesPowerOffSystem();
        }
    }
}
