using System;

namespace ChinaPay.B3B.Tool.Tradement {
    class Program {
        static void Main(string[] args) {
            RoyaltyProcessor.Instance.Start();
            RefundProcessor.Instance.Start();
            while(Console.ReadLine() != "exit") { }
        }
    }
}