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
        public const String FILENAME = "izspeles.txt";


        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            for (int i = 1; i <= 24; i++)
                Console.Write("{0}\t", i);
            Console.WriteLine();

            Deck myDeck = new Deck();//static is 10x slower, why?

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Table table = new Table();
            table.StartGame();


            //Console.ReadLine();
            stopwatch.Stop();

            //Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed.TotalSeconds);
            //Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed.TotalMilliseconds);
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
            //Console.ReadLine();
        }
    }
}
