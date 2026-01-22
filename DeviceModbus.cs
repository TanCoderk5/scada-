using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
Function Code 
Read Coils               [01]        : Reads the status of one or more coils (bits).
Read Discrete Inputs     [02]        : Reads the status of one or more discrete inputs (bits).
Read Holding Registers   [03]        : Reads the value of one or more holding registers (words).
Read Input Registers     [04]        : Reads the value of one or more input registers (words).
Write Single Coil        [05]        : Writes a single coil (bit) to either ON or OFF.
Write Single Register    [06]        : Writes a single value to a holding register.
Write Multiple Coils     [15 (0x0F)] : Writes to one or more coils (bits).
Write Multiple Registers [16 (0x10)] : Writes to one or more holding registers.
 */
namespace Modbus_Slave
{
    public class DeviceModbus
    {
        public byte SlaveId { get; set; }

        // ⭐ FIX: Khởi tạo default values để tránh nullable warnings
        public bool[] Coils { get; set; }
        public bool[] DiscreteInputs { get; set; }
        public ushort[] HoldingRegisters { get; set; }
        public ushort[] InputRegisters { get; set; }

        // ⭐ FIX: Constructor mặc định khởi tạo arrays
        public DeviceModbus()
        {
            SlaveId = 1;
            Coils = new bool[100];
            DiscreteInputs = new bool[100];
            HoldingRegisters = new ushort[100];
            InputRegisters = new ushort[100];
        }

        public DeviceModbus(byte slaveId)
        {
            SlaveId = slaveId;
            Coils = new bool[100];
            DiscreteInputs = new bool[100];
            HoldingRegisters = new ushort[100];
            InputRegisters = new ushort[100];
        }

        public DeviceModbus(byte slaveId, int hrCount)
        {
            SlaveId = slaveId;
            Coils = new bool[100];
            DiscreteInputs = new bool[100];
            HoldingRegisters = new ushort[hrCount];
            InputRegisters = new ushort[100];
        }

        public DeviceModbus(byte slaveId, int coilCount, int diCount, int hrCount, int irCount)
        {
            SlaveId = slaveId;
            Coils = new bool[coilCount];
            DiscreteInputs = new bool[diCount];
            HoldingRegisters = new ushort[hrCount];
            InputRegisters = new ushort[irCount];
        }

        /// <summary>
        /// Xử lý PDU (không gồm slaveId, không gồm CRC)
        /// </summary>
        public byte[] HandleRequest(byte[] requestPdu)
        {
            // ⭐ FIX: Kiểm tra null trước
            if (requestPdu == null || requestPdu.Length == 0)
                return BuildException(0x00, 0x01); // illegal function

            byte func = requestPdu[0];

            switch (func)
            {
                case 0x01: return HandleReadCoils(requestPdu);
                case 0x02: return HandleReadDiscreteInputs(requestPdu);
                case 0x03: return HandleReadHoldingRegisters(requestPdu);
                case 0x04: return HandleReadInputRegisters(requestPdu);
                case 0x05: return HandleWriteSingleCoil(requestPdu);
                case 0x06: return HandleWriteSingleRegister(requestPdu);
                case 0x0F: return HandleWriteMultipleCoils(requestPdu);
                case 0x10: return HandleWriteMultipleRegisters(requestPdu);

                default:
                    return BuildException(func, 0x01); // illegal function
            }
        }

        private byte[] HandleReadCoils(byte[] pdu)
        {
            if (pdu.Length < 5)
                return BuildException(0x01, 0x03);

            ushort start = (ushort)((pdu[1] << 8) | pdu[2]);
            ushort qty = (ushort)((pdu[3] << 8) | pdu[4]);

            if (start + qty > Coils.Length)
                return BuildException(0x01, 0x02);

            int byteCount = (qty + 7) / 8;
            byte[] resp = new byte[2 + byteCount];

            resp[0] = 0x01;
            resp[1] = (byte)byteCount;

            for (int i = 0; i < qty; i++)
            {
                if (Coils[start + i])
                    resp[2 + (i / 8)] |= (byte)(1 << (i % 8));
            }

            return resp;
        }

        private byte[] HandleReadDiscreteInputs(byte[] pdu)
        {
            if (pdu.Length < 5)
                return BuildException(0x02, 0x03);

            ushort start = (ushort)((pdu[1] << 8) | pdu[2]);
            ushort qty = (ushort)((pdu[3] << 8) | pdu[4]);

            if (start + qty > DiscreteInputs.Length)
                return BuildException(0x02, 0x02);

            int byteCount = (qty + 7) / 8;
            byte[] resp = new byte[2 + byteCount];

            resp[0] = 0x02;
            resp[1] = (byte)byteCount;

            for (int i = 0; i < qty; i++)
            {
                if (DiscreteInputs[start + i])
                    resp[2 + (i / 8)] |= (byte)(1 << (i % 8));
            }

            return resp;
        }

        private byte[] HandleReadHoldingRegisters(byte[] pdu)
        {
            if (pdu.Length < 5)
                return BuildException(0x03, 0x03);

            ushort start = (ushort)((pdu[1] << 8) | pdu[2]);
            ushort qty = (ushort)((pdu[3] << 8) | pdu[4]);

            if (start + qty > HoldingRegisters.Length)
                return BuildException(0x03, 0x02);

            byte byteCount = (byte)(qty * 2);
            byte[] resp = new byte[2 + byteCount];

            resp[0] = 0x03;
            resp[1] = byteCount;

            int idx = 2;
            for (int i = 0; i < qty; i++)
            {
                ushort val = HoldingRegisters[start + i];
                resp[idx++] = (byte)(val >> 8);
                resp[idx++] = (byte)(val & 0xFF);
            }

            return resp;
        }

        private byte[] HandleReadInputRegisters(byte[] pdu)
        {
            if (pdu.Length < 5)
                return BuildException(0x04, 0x03);

            ushort start = (ushort)((pdu[1] << 8) | pdu[2]);
            ushort qty = (ushort)((pdu[3] << 8) | pdu[4]);

            if (start + qty > InputRegisters.Length)
                return BuildException(0x04, 0x02);

            byte byteCount = (byte)(qty * 2);
            byte[] resp = new byte[2 + byteCount];

            resp[0] = 0x04;
            resp[1] = byteCount;

            int idx = 2;
            for (int i = 0; i < qty; i++)
            {
                ushort val = InputRegisters[start + i];
                resp[idx++] = (byte)(val >> 8);
                resp[idx++] = (byte)(val & 0xFF);
            }

            return resp;
        }

        private byte[] HandleWriteSingleCoil(byte[] pdu)
        {
            if (pdu.Length < 5)
                return BuildException(0x05, 0x03);

            ushort addr = (ushort)((pdu[1] << 8) | pdu[2]);
            ushort value = (ushort)((pdu[3] << 8) | pdu[4]);

            if (addr >= Coils.Length)
                return BuildException(0x05, 0x02);

            Coils[addr] = value == 0xFF00;

            // Echo request
            byte[] resp = new byte[5];
            Array.Copy(pdu, resp, 5);
            return resp;
        }

        private byte[] HandleWriteSingleRegister(byte[] pdu)
        {
            // PDU format: [0]=0x06, [1-2]=Addr, [3-4]=Value
            if (pdu.Length < 5)
                return BuildException(0x06, 0x03);

            ushort addr = (ushort)((pdu[1] << 8) | pdu[2]);
            ushort value = (ushort)((pdu[3] << 8) | pdu[4]);

            if (addr >= HoldingRegisters.Length)
                return BuildException(0x06, 0x02);

            HoldingRegisters[addr] = value;

            // Theo chuẩn Modbus, response = echo lại request
            byte[] resp = new byte[5];
            Array.Copy(pdu, 0, resp, 0, 5);
            return resp;
        }

        private byte[] HandleWriteMultipleCoils(byte[] pdu)
        {
            if (pdu.Length < 6)
                return BuildException(0x0F, 0x03);

            ushort start = (ushort)((pdu[1] << 8) | pdu[2]);
            ushort qty = (ushort)((pdu[3] << 8) | pdu[4]);
            byte byteCount = pdu[5];

            if (start + qty > Coils.Length)
                return BuildException(0x0F, 0x02);

            int idx = 6;
            for (int i = 0; i < qty; i++)
            {
                int b = i / 8;
                int bit = i % 8;

                bool val = (pdu[idx + b] & (1 << bit)) != 0;
                Coils[start + i] = val;
            }

            // Response = start + qty
            return new byte[]
            {
                0x0F,
                pdu[1], pdu[2],
                pdu[3], pdu[4]
            };
        }

        private byte[] HandleWriteMultipleRegisters(byte[] pdu)
        {
            if (pdu.Length < 6)
                return BuildException(0x10, 0x03);

            ushort start = (ushort)((pdu[1] << 8) | pdu[2]);
            ushort qty = (ushort)((pdu[3] << 8) | pdu[4]);
            byte byteCount = pdu[5];

            if (start + qty > HoldingRegisters.Length)
                return BuildException(0x10, 0x02);

            int idx = 6;
            for (int i = 0; i < qty; i++)
            {
                ushort val = (ushort)((pdu[idx] << 8) | pdu[idx + 1]);
                HoldingRegisters[start + i] = val;
                idx += 2;
            }

            // Response = start + qty
            return new byte[] { 0x10, pdu[1], pdu[2], pdu[3], pdu[4] };
        }

        private byte[] BuildException(byte func, byte exceptionCode)
        {
            // Exception PDU: [0]=func|0x80, [1]=exceptionCode
            return new byte[] { (byte)(func | 0x80), exceptionCode };
        }
    }
}