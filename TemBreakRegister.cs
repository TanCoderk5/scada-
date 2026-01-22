using System;
using System.Collections.Generic;

namespace Modbus_Slave
{
    public class MccbDevice
    {
        public byte SlaveId { get; set; }
        public string Name { get; set; } = "";
        public List<ModbusIOA_Register> Registers { get; set; } = new();
    }

    public class ModbusIOA_Register
    {
        public string Description { get; set; } = "";
        public ushort IOA { get; set; }
        public ushort Address { get; set; }
        public byte FC { get; set; }
        public ushort Length { get; set; }
        public string DataType { get; set; } = "";
        public string Unit { get; set; } = "";
        public float Scale { get; set; }
        public string Style { get; set; } = "";

        public bool[] BoolValues { get; set; } = Array.Empty<bool>();
        public ushort[] UshortValues { get; set; } = Array.Empty<ushort>();
    }
}