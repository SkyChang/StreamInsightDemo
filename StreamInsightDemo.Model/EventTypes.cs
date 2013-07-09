using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamInsightDemo.Model
{
    public class InputEvent
    {
        public string Name { get; set; } //名稱
        public double Tp { get; set; } //溫度
        public double Lsl { get; set; } //最低溫度
        public double Usl { get; set; } //最高溫度
    }


    public class OutputEvent
    {
        public double O { get; set; }//輸出
    }
}
