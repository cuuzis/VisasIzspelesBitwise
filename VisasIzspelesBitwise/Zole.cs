using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;


namespace VisasIzspelesBitwise
{
    class Zole
    {
        // Hardcoded params
        //public const bool WRITE_TO_CONSOLE = true;
        public static String FILENAME = "izspeles.txt";


        static void Main(string[] args)
        {
            


            Deck myDeck = new Deck();//static is 10x slower, why?

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Table table = new Table();
            table.StartGame();


            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
            Console.ReadLine();
        }
    }
}
