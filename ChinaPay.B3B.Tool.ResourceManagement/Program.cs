using System;

namespace ChinaPay.B3B.Tool.ResourceManagement {
    class Program {
        static void Main(string[] args) {
            Processor.Instance.Start();
            while(Console.ReadLine() != "exit") { }
        }
    }
}
