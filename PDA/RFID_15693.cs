using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Globalization;

namespace PDA
{
    /// <summary>
    /// 该类封装了手持机的高频RFID的15693功能的相关操作
    /// </summary>
    public class RFID_15693
    {
        #region  DeviceAPI
        [DllImport("DeviceAPI.dll", EntryPoint = "RF_ISO15693_init")]
        private static extern bool RF_ISO15693_init();

        [DllImport("DeviceAPI.dll", EntryPoint = "RF_ISO15693_free")]
        private static extern bool RF_ISO15693_free();

        [DllImport("DeviceAPI.dll", EntryPoint = "RF_ModeSwitch")]
        private static extern int RF_ModeSwitch(int iMode);

        [DllImport("DeviceAPI.dll", EntryPoint = "RF_ISO15693_inventory")]
        private static extern int RF_ISO15693_inventory(int iMode, int iAFI, byte[] pszData);

        [DllImport("DeviceAPI.dll", EntryPoint = "RF_ISO15693_read_sm")]
        private static extern int RF_ISO15693_read_sm(int iMode, byte[] pszUID, int iLenUID, int startblock, int blocknum, byte[] pszData);

        [DllImport("DeviceAPI.dll", EntryPoint = "RF_ISO15693_write_sm")]
        private static extern int RF_ISO15693_write_sm(int iMode, byte[] pszUID, int iLenUID, int startblock, int blocknum, byte[] pszData, int iWriteLen);

        [DllImport("DeviceAPI.dll", EntryPoint = "RF_ISO15693_select")]
        public static extern int RF_ISO15693_select(byte[] pszData, int startIndex);

        /// <summary>
        /// 写电子标签AFI值。AFI（Application family identifier）应用族标识
        /// </summary>
        /// <param name="iMode">模式选择范围 0 ~ 7</param>
        /// <param name="pszUID">卡号</param>
        /// <param name="iLenUID">卡号长度</param>
        /// <param name="iAFI">待写入的AFI值。取值范围0~255</param>
        /// <returns></returns>
        [DllImport("DeviceAPI.dll")]
        private static extern int RF_ISO15693_writeAFI(int iMode, byte[] pszUID, int iLenUID, int iAFI);


        /// <summary>
        /// 锁定标签的AFI
        /// </summary>
        /// <param name="iMode">模式选择</param>
        /// <param name="pszUID">卡号</param>
        /// <param name="iLenUID">卡号长度</param>
        /// <returns></returns>
        [DllImport("DeviceAPI.dll")]
        private static extern int RF_ISO15693_lockAFI(int iMode, byte[] pszUID, int iLenUID);



        /// <summary>
        /// 写电子标签的DSFID值 （DSFID： 数据存储格式标识符）
        /// </summary>
        /// <param name="iMode">模式选择取值0 ~ 7</param>
        /// <param name="pszUID">卡号</param>
        /// <param name="iLenUID">卡号长度</param>
        /// <param name="iDSFID">待写入的DSFID值取值 0~255</param>
        /// <returns></returns>
        [DllImport("DeviceAPI.dll")]
        private static extern int RF_ISO15693_writeDSFID(int iMode, byte[] pszUID, int iLenUID, int iDSFID);

        /// <summary>
        /// 锁定电子标签DSFID值
        /// </summary>
        /// <param name="iMode">模式选择</param>
        /// <param name="pszUID">卡号</param>
        /// <param name="iLenUID">卡号长度</param>
        /// <returns></returns>
        [DllImport("DeviceAPI.dll")]
        private static extern int RF_ISO15693_lockDSFID(int iMode, byte[] pszUID, int iLenUID);


        /// <summary>
        /// 获取电子标签信息
        /// </summary>
        /// <param name="iMode">模式选择</param>
        /// <param name="pszUID">卡号</param>
        /// <param name="iLenUID">卡号长度</param>
        /// <param name="pszData">标签信息</param>
        /// <returns></returns>
        [DllImport("DeviceAPI.dll")]
        public static extern int RF_ISO15693_getSystemInformation(int iMode, byte[] pszUID, int iLenUID, byte[] pszData);
        #endregion



        /// <summary>
        /// 模块上电，开启串口资源
        /// </summary>
        /// <returns></returns>
        public static bool InitModule()
        {
            bool iRes = RF_ISO15693_init();
            RF_ModeSwitch(2);   //端口切换
            return iRes;
        }

        /// <summary>
        /// 模块断电，释放串口资源
        /// </summary>
        public static bool freeMode()
        {
            return RF_ISO15693_free();
        }



        /// <summary>
        /// 寻卡，模式为：单卡不带AFI
        /// </summary>
        /// <param name="dataUid">标签信息</param>
        /// <returns>true表示获取标签成功，false表示操作失败</returns>
        public static bool ScanSingleTag(ref byte[] dataUid)
        {
            byte[] pszData = new byte[11];
            dataUid = new byte[8];
            int iRes = -1;

            iRes = RF_ISO15693_inventory(1, 0, pszData);


            if (iRes == 0x00)
            {

                for (int i = 0; i < 8; i++)
                {
                    dataUid[i] = pszData[10 - i];
                }
                //int result = RFID_15693.RF_ISO15693_select(dataUid, 8);
                //if (result != 0)
                //{
                //    return false;
                //}
                //else
                //{
                return true;
                //}
            }
            else
            {
                return false;   //寻卡失败
            }
        }

        /// <summary>
        /// 寻卡 模式为：多卡，不带AFI
        /// </summary>
        /// <param name="UIDArray">标签信息</param>
        /// <returns>true表示获取标签成功，false表示操作失败</returns>
        public static bool SearchMultipleTags(ref List<byte[]> UIDArray)
        {
            byte[] data_Multiple_UID = new byte[255];
            byte[] data = new byte[8];
            UIDArray = new List<byte[]>();
            int iRes = RF_ISO15693_inventory(0, 0, data_Multiple_UID);

            if (iRes == 0x00)
            {
                //data_Multiple_UID[0]的值是表示标签信息的全部字节长度，每一张标签的数据长度为10，其中UID是8个字节
                int iDataCount = data_Multiple_UID[0] / 10;

                for (int i = 0; i < iDataCount; i++)
                {
                    data = new byte[8];
                    for (int k = 0; k < 8; k++)
                    {
                        data[k] = data_Multiple_UID[i * 10 + 10 - k];
                    }
                    UIDArray.Add(data);
                }
                return true;
            }
            else
            {
                return false;
            }

        }


        /// <summary>
        /// 将字符转换成byte类型。字符长度长于8的，取低8位，字符不满8位的，高位补零
        /// </summary>
        /// <param name="dataToWrite"></param>
        /// <param name="data"></param>
        /// <returns>true表示操作成功，false表示操作失败</returns>
        static bool getByteData(string dataToWrite, ref byte[] data)
        {
            try
            {
                data = new byte[4];
                string[] dataStr = new string[] { "00", "00", "00", "00" };
                int L = dataToWrite.Length;
                if (L > 8)
                {
                    dataToWrite = dataToWrite.Substring(L - 8, 8);
                }
                if (L < 8)
                {
                    for (int i = L; i < 8; i++)
                    {
                        dataToWrite = "0" + dataToWrite;
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    dataStr[i] = dataToWrite.Substring(2 * i, 2);
                    data[i] = Convert.ToByte(dataStr[i], 16);

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// 将16进制的字符吸入指定的block，字符长度长于8的，取低8位，字符不满8位的，高位补零
        /// </summary>
        /// <param name="dataText">16进制的字符</param>
        /// <param name="nBlock">block序号</param>
        /// <returns></returns>
        public static bool WriteBlockByString(string dataText, int nBlock)
        {
            byte[] data = new byte[4];
            bool res = getByteData(dataText, ref data);
            if (res)
            {
                return WriteBlock(data, nBlock);
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 将数据写入block中
        /// </summary>
        /// <param name="bWrite">数据长度为4个字节,如果长度小于四，则在前面补0x00，大于4则取前面的4个字节</param>
        /// <param name="bolck">指定的block序号</param>
        /// <returns></returns>
        public static bool WriteBlock(byte[] bWrite, int bolck)
        {
            if (bWrite.Length == 0)
            {
                return false;   //数据不能为空
            }
            byte[] pszData = new byte[4];
            if (bWrite.Length >= 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    pszData[i] = bWrite[i];
                }
            }
            else
            {
                //字节长度小于4 前面补零

                int Len = 4 - bWrite.Length;

                for (int i = 0; i < bWrite.Length; i++)
                {
                    pszData[i + Len] = bWrite[i];
                }
            }


            int iRes = -1;
            if (GetTagType() == RFID_15693TagType.ICODE2)
            {
                iRes = RF_ISO15693_write_sm(0, null, 0, bolck, 1, pszData, pszData.Length);//API的数据写入操作}
            }
            else if (GetTagType() == RFID_15693TagType.TI2048)
            {
                iRes = RF_ISO15693_write_sm(4, null, 0, bolck, 1, pszData, pszData.Length);
            }
            else if (GetTagType() == RFID_15693TagType.STLRIS64K)
            {
                iRes = RF_ISO15693_write_sm(8, null, 0, bolck, 1, pszData, pszData.Length);
            }

            if (iRes == 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 写入AFI
        /// </summary>
        /// <param name="iAFI"></param>
        /// <returns></returns>
        public static bool WriteAFI(byte iAFI)
        {
            byte[] data = new byte[1];
            int AFI = iAFI;
            int res = -1;

            if (GetTagType() == RFID_15693TagType.ICODE2)
            {
                res = RF_ISO15693_writeAFI(0, data, 0, AFI);
            }
            else if (GetTagType() == RFID_15693TagType.TI2048)
            {
                res = RF_ISO15693_writeAFI(4, data, 0, AFI);
            }
            else if (GetTagType() == RFID_15693TagType.STLRIS64K)
            {
                res = RF_ISO15693_writeAFI(0, data, 0, AFI);
            }
            if (res == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 锁定AFI
        /// </summary>
        /// <param name="iAFI"></param>
        /// <returns></returns>
        public static bool LockAFI()
        {
            byte[] data = new byte[1];
            int res = -1;
            //if (GetTagType() == RFID_15693TagType.STLRIS64K)
            //{
            //      res = RF_ISO15693_lockAFI(8, data, 0);
            //}
            //else
            //{
            res = RF_ISO15693_lockAFI(0, data, 0);
            //}
            if (res == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 写入DSFID
        /// </summary>
        /// <param name="iDSFID"></param>
        /// <returns></returns>
        public static bool WriteDSFID(byte iDSFID)
        {
            byte[] data = new byte[1];
            int DSFID = iDSFID;

            int res = -1;
            if (GetTagType() == RFID_15693TagType.ICODE2)
            {
                res = RF_ISO15693_writeDSFID(0, data, 0, DSFID);
            }
            else if (GetTagType() == RFID_15693TagType.TI2048)
            {
                res = RF_ISO15693_writeDSFID(4, data, 0, DSFID);
            }
            else if (GetTagType() == RFID_15693TagType.STLRIS64K)
            {
                res = RF_ISO15693_writeDSFID(0, data, 0, DSFID);
            }
            if (res == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 锁定DSFID
        /// </summary>
        /// <param name="iDSFID"></param>
        /// <returns></returns>
        public static bool LockDSFID()
        {
            byte[] data = new byte[1];
            int res = -1;
            if (GetTagType() == RFID_15693TagType.STLRIS64K)
            {
                res = RF_ISO15693_lockDSFID(0, data, 0);
            }
            else
            {
                res = RF_ISO15693_lockDSFID(0, data, 0);
            }

            if (res == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 读取指定block的数据
        /// </summary>
        /// <param name="iblock">block序号</param>
        /// <param name="data">保存读取到的数据</param>
        /// <returns></returns>
        public static bool ReadBlockData(int iblock, ref byte[] data)
        {
            byte[] pszData = new byte[255];
            int iRes = -1;
            if (GetTagType() == RFID_15693TagType.STLRIS64K)
            {
                iRes = RF_ISO15693_read_sm(8, null, 0, iblock, 1, pszData);
            }
            else
            {
                iRes = RF_ISO15693_read_sm(0, null, 0, iblock, 1, pszData);
            }
            if (iRes == 0x00)
            {
                data = new byte[pszData[0]];
                for (int i = 0; i < pszData[0]; i++)
                {
                    data[i] = pszData[i + 1];
                }
                return true;
            }
            else
            {
                data = new byte[4];
                return false;
            }

        }

        /// <summary>
        /// 标签类型
        /// </summary>
        public enum RFID_15693TagType
        {
            ICODE2 = 0,
            TI2048 = 4,
            STLRIS64K = 8,
            NUll_
        }
        public static RFID_15693TagType GetTagType()
        {
            byte[] pszData = new byte[11];
            int iRes = RF_ISO15693_inventory(1, 0, pszData);
            if (pszData[9] == 4)
                return RFID_15693TagType.ICODE2;
            else if (pszData[9] == 7)
                return RFID_15693TagType.TI2048;
            else if (pszData[9] == 2)
                return RFID_15693TagType.STLRIS64K;
            else
                return RFID_15693TagType.NUll_;
        }


        public static bool GetTagInfo(ref Tag_15693 tag)
        {

            byte[] pszData = new byte[11];
            int iRes = RF_ISO15693_inventory(1, 0, pszData);

            byte[] dataUid = new byte[8];

            if (iRes == 0x00)
            {

                for (int i = 0; i < 8; i++)
                {
                    dataUid[i] = pszData[10 - i];
                }

                tag.tag_ID = BitConverter.ToString(dataUid, 0, dataUid.Length).Replace("-", "");

                if (pszData[9] == 4)
                    tag.tag_type = RFID_15693TagType.ICODE2;
                else if (pszData[9] == 7)
                    tag.tag_type = RFID_15693TagType.TI2048;
                else if (pszData[9] == 2)
                    tag.tag_type = RFID_15693TagType.STLRIS64K;
                else
                    tag.tag_type = RFID_15693TagType.NUll_;


                return true;

                //int result = RFID_15693.RF_ISO15693_select(dataUid, 8);
                //if (result != 0)
                //{
                //    return false;
                //}
                //else
                //{
                //    return true;
                //}
            }
            else
            {
                tag.tag_ID = "";
                tag.tag_type = RFID_15693TagType.NUll_;
                return false;   //寻卡失败
            }


        }

        /// <summary>
        /// 15693标签，含标签ID、标签类型
        /// </summary>
        public struct Tag_15693
        {
            public string tag_ID;
            public RFID_15693TagType tag_type;
        }


        public static string RFID_ID()
        {
            try
            {
                string RFID_ID = "";

                byte[] pszData = new byte[255];
                int iRes = -1;
                if (GetTagType() == RFID_15693TagType.STLRIS64K)
                {
                    iRes = RF_ISO15693_read_sm(8, null, 0, 0, 4, pszData);
                }
                else
                {
                    iRes = RF_ISO15693_read_sm(0, null, 0, 0, 4, pszData);
                }
                if (iRes == 0x00)
                {
                    byte[] data = new byte[pszData[0]];
                    for (int i = 0; i < pszData[0]; i++)
                    {
                        data[i] = pszData[i + 1];
                    }
                    string[] Block = BitConverter.ToString(data, 0, data.Length).Split('-');
                    RFID_ID = long.Parse(Block[10] + Block[9] + Block[8] + Block[7] + Block[6] + Block[5] + Block[4] + Block[3], NumberStyles.AllowHexSpecifier).ToString();
                }

                return RFID_ID;
            }
            catch (Exception)
            {
                return "";
            }


        }

    }
}
