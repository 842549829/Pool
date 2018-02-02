using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Tool.AutoPay
{
    class Program
    {
        static void Main(string[] args)
        {
            AutoPayProcessor.Instance.Start();
            while (Console.ReadLine() != "exit") { }
        }
    }
}
