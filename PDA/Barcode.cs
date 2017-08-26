using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PDA
{
    /// <summary>
    /// 该类封装了手持机的扫描枪的相关操作，一维、二维条码都适用。
    /// </summary>
    public class Barcode
    {
        #region 扫描条码
        static uint[] ReadDataSize = new uint[5];
        static byte[] barcodeData = new byte[2050];
        static uint iTimeout = 3000;
        static bool reslut = false;
        static int leng = 0;
        static string data = "                                                                                                                                                                                          ";
        #endregion

        #region form DeviceAPI


        [DllImport("DeviceAPI.dll", EntryPoint = "Barcode2D_init")]
        private static extern bool Barcode2D_init();

        [DllImport("DeviceAPI.dll", EntryPoint = "Barcode2D_scan")]
        private static extern int Barcode2D_scan(byte[] pszData, int iBufferlen);

        [DllImport("DeviceAPI.dll", EntryPoint = "Barcode2D_free")]
        private static extern void Barcode2D_free();


        #endregion

        /// <summary>
        /// 模块上电、开启串口。如果是二维条码头，初始化之后要等待2~5秒钟才可进行扫描
        /// </summary>
        /// <returns></returns>
        public static bool InitModule()
        {
            return Barcode2D_init();
        }

        /// <summary>
        /// 模块断电、释放串口资源
        /// </summary>
        public static void FreeModule()
        {
            Barcode2D_free();
        }


        ///// <summary>
        ///// 扫描条码
        ///// </summary>
        ///// <returns>扫描成功则返回条码内容,失败则返回空字符串</returns>
        //public static string Scan()
        //{
        //    int ibarLen = 0;
        //    byte[] pszData = new byte[1024];
        //    string barcode = string.Empty;

        //    try
        //    {
        //        ibarLen = Barcode2D_scan(pszData, pszData.Length);

        //        if (ibarLen > 0)
        //        {
        //            barcode = System.Text.Encoding.GetEncoding("GB2312").GetString(pszData, 0, ibarLen);

        //        }
        //        return barcode;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        try
        //        {
        //            if (ibarLen > 0)
        //            {
        //                barcode = Encoding.ASCII.GetString(pszData, 0, ibarLen);

        //            }
        //            return barcode;
        //        }
        //        catch (System.Exception)
        //        {
        //            try
        //            {
        //                if (ibarLen > 0)
        //                {
        //                    barcode = System.Text.Encoding.GetEncoding("Windows-1252").GetString(pszData, 0, ibarLen);

        //                }
        //                return barcode;
        //            }
        //            catch (System.Exception)
        //            {
        //                return string.Empty;
        //            }

        //        }
        //        //return string.Empty;
        //    }

        //}

        /// <summary>
        /// 获取条码字节信息
        /// </summary>
        /// <param name="nData"></param>
        /// <returns></returns>
        public static int scan_data(ref byte[] nData)
        {
            return Barcode2D_scan(nData, nData.Length);

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string Scan_lys(int n)
        {
            int ibarLen = 0;
            byte[] pszData = new byte[1024];
            string barcode = string.Empty;

            try
            {
                ibarLen = Barcode2D_scan(pszData, pszData.Length);

                if (ibarLen > 0)
                {

                    if (n == 0)
                    {
                        barcode = System.Text.Encoding.UTF8.GetString(pszData, 0, ibarLen);
                    }
                    else if (n == 1)
                    {
                        barcode = System.Text.Encoding.GetEncoding("Windows-1252").GetString(pszData, 0, ibarLen);

                    }
                    else if (n == 2)
                    {
                        barcode = System.Text.Encoding.GetEncoding("GB2312").GetString(pszData, 0, ibarLen);

                    }
                    else if (n == 3)
                    {
                        barcode = System.Text.Encoding.Unicode.GetString(pszData, 0, ibarLen);

                    }
                    else
                    {
                        barcode = System.Text.Encoding.UTF8.GetString(pszData, 0, ibarLen);
                    }

                }


                return barcode;
            }
            catch (System.Exception ex)
            {

                return string.Empty;
            }

        }

        public static string scan()
        {
            ReadDataSize[0] = 0;
            Array.Clear(barcodeData, 0, barcodeData.Length);
            reslut = CommonClass.SoftDecoding_Scan(iTimeout, barcodeData, 2048, ReadDataSize);
            if (reslut)
            {
                leng = (int)(ReadDataSize[0]) - 2;//Convert.ToInt32(ReadDataSize[0])-2;
                if (leng >= 2048)
                {
                    MessageBox.Show("条码长度超长！");
                    return "";
                }
                try
                {
                    int n = 0;
                    if (n == 0)
                    {
                        //从第三位开始取内容（第一位为条码类型，第二位为长度）
                        data = System.Text.Encoding.UTF8.GetString(barcodeData, 2, leng);
                    }
                    else if (n == 1)
                    {
                        data = System.Text.Encoding.GetEncoding("Windows-1252").GetString(barcodeData, 2, leng);
                    }
                    else if (n == 2)
                    {
                        data = System.Text.Encoding.GetEncoding("GB2312").GetString(barcodeData, 2, leng);
                    }
                    else if (n == 3)
                    {
                        data = System.Text.Encoding.Unicode.GetString(barcodeData, 2, leng);
                    }
                    else
                    {
                        data = System.Text.Encoding.UTF8.GetString(barcodeData, 2, leng);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("扫描失败: " + ex.Message);
                    return "";
                }
                return data;
            }
            else
            {
                return "";
            }
        }

    }
}
