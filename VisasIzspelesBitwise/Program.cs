using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;


namespace VisasIzspelesBitwise
{
    class Program
    {
        // Hardcoded params
        //public const bool WRITE_TO_CONSOLE = true;
        public static String FILENAME = "izspeles.txt";


        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();


            Deck myDeck = new Deck();//static is 10x slower, why?


            Table table = new Table();
            table.StartGame();


            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
            Console.ReadLine();
        }
    }
}
