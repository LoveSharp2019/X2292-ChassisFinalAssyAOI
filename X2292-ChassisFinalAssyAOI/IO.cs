using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace X2292_ChassisFinalAssyAOI
{

   

    public class OutputBitInfo
    {
        public int Index { get; set; }
        public int CardNo { get; set; }
        public bool Enable { get; set; } = false;
        public int BitNum { get; set; }
        public int BitPort { get; set; }
        public IOState Status { get; set; } = IOState.OFF;
        public string Item { set; get; }
    }
    public class InputBitInfo
    {
        public short Index { get; set; }
        public short CardNo { get; set; }
        public short BitNum {get; set;}
        public short BitPort {get;set;}
        public IOState Status { get; set; } = IOState.OFF;
        public string Item{set; get;}
    }

    public enum IOState
    {
        ON, OFF
    }
}
