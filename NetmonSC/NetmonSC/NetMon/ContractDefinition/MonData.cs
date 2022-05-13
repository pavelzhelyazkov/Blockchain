using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace NetmonSC.NetMon.ContractDefinition
{
    public partial class MonData : MonDataBase { }

    public class MonDataBase 
    {
        [Parameter("string", "hostname", 1)]
        public virtual string Hostname { get; set; }
        [Parameter("bool", "state", 2)]
        public virtual bool State { get; set; }
    }
}
