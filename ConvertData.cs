using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modbus_Slave
{
    public static class ConvertData
    {

        public static ushort ToUInt16(ushort[] regs)
        {
            return regs[0];
        }
        public static short ToInt16(ushort[] regs)
        {
            return unchecked((short)regs[0]);
        }
        public static uint ToUInt32_BE(ushort[] regs)
        {
            return ((uint)regs[0] << 16) | (uint)regs[1];
        }
        public static int ToInt32_BE(ushort[] regs)
        {
            return (int)(((uint)regs[0] << 16) | regs[1]);
        }
        public static float ToFloat32_BE(ushort[] regs)
        {
            byte[] b = new byte[4];
            b[0] = (byte)(regs[0] >> 8);
            b[1] = (byte)(regs[0] & 0xFF);
            b[2] = (byte)(regs[1] >> 8);
            b[3] = (byte)(regs[1] & 0xFF);

            Array.Reverse(b); // BE -> LE
            return BitConverter.ToSingle(b, 0);
        }
        public static ulong ToUInt64_BE(ushort[] regs)
        {
            return ((ulong)regs[0] << 48) |
                   ((ulong)regs[1] << 32) |
                   ((ulong)regs[2] << 16) |
                   ((ulong)regs[3]);
        }
        public static long ToInt64_BE(ushort[] regs)
        {
            return unchecked(
                ((long)regs[0] << 48) |
                ((long)regs[1] << 32) |
                ((long)regs[2] << 16) |
                ((long)regs[3])
            );
        }
        public static string ToString(ushort[] regs)
        {
            byte[] buf = new byte[regs.Length * 2];

            for (int i = 0; i < regs.Length; i++)
            {
                buf[i * 2] = (byte)(regs[i] >> 8);
                buf[i * 2 + 1] = (byte)(regs[i] & 0xFF);
            }

            return Encoding.ASCII.GetString(buf).TrimEnd('\0', ' ');
        }
        public static float ToFloat32_WORD_SWAP(ushort[] regs)
        {
            byte[] b = new byte[4];

            // WORD SWAP
            b[0] = (byte)(regs[1] >> 8);
            b[1] = (byte)(regs[1] & 0xFF);
            b[2] = (byte)(regs[0] >> 8);
            b[3] = (byte)(regs[0] & 0xFF);

            Array.Reverse(b);
            return BitConverter.ToSingle(b, 0);
        }
        public static string ToValueRegisters(ushort[] regs,int StartAddress, int Length, string DataType)
        {
            string zResult = "";
            ushort[] zReponse = new ushort[Length];
            int k = 0;
           
            for (int i = StartAddress; i < StartAddress+Length; i++)
            {
                zReponse[k] = regs[i];
                k++;
            }
            switch (DataType.ToUpper())
            {
                case "STRING":
                    zResult = ConvertData.ToString(zReponse);
                    break;
                case "FLOAT32_BE":
                    zResult = ConvertData.ToFloat32_BE(zReponse).ToString();
                    break;
                case "FLOAT32_WORD_SWAP":
                    zResult = ConvertData.ToFloat32_WORD_SWAP(zReponse).ToString();
                    break;
                case "U64_BE":
                    zResult = ConvertData.ToUInt64_BE(zReponse).ToString();
                    break;
                case "U32_BE":
                    zResult = ConvertData.ToUInt32_BE(zReponse).ToString();
                    break;
                case "U16":
                    zResult = ConvertData.ToUInt16(zReponse).ToString();
                    break;
                case "S64_BE":
                    zResult = ConvertData.ToInt64_BE(zReponse).ToString();
                    break;
                case "S32_BE":
                    zResult = ConvertData.ToInt32_BE(zReponse).ToString();
                    break;
                case "S16":
                    zResult = ConvertData.ToInt16(zReponse).ToString();
                    break;
            }
            return zResult;
        }

        public static ushort[] FromUInt16(ushort v) => new[] { v };
        public static ushort[] FromInt16(short v) => new[] { unchecked((ushort)v) };
        public static ushort[] FromBool(bool v) => new[] { (ushort)(v ? 1 : 0) };

        public static ushort[] FromUInt32_BE(uint v)
            => new[] { (ushort)(v >> 16), (ushort)(v & 0xFFFF) };

        public static ushort[] FromInt32_BE(int v)
            => FromUInt32_BE(unchecked((uint)v));

        public static ushort[] FromFloat32_BE(float f)
        {
            uint u = BitConverter.SingleToUInt32Bits(f);
            ushort hi = (ushort)(u >> 16);
            ushort lo = (ushort)(u & 0xFFFF);
            // word-order big-endian
            return new[] { hi, lo };
        }

        public static ushort[] FromUInt64_BE(ulong v)
            => new[]
            {
            (ushort)(v >> 48),
            (ushort)(v >> 32),
            (ushort)(v >> 16),
            (ushort)(v)
            };

        public static ushort[] FromInt64_BE(long v)
            => FromUInt64_BE(unchecked((ulong)v));
    }
}
