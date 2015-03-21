﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisasIzspelesBitwise
{
    class Deck
    {
        public const int SIZE = 26;
        public const int EMPTY_CARD = 0;
                                                 //1,     2,      4,     8,    16,    32,     64,   128,   256,   512,   1024,    11,    12,    13,    14,    15,     16,    17,    18,    19,    20,    21,    22,    23,    24,    25
        //public static readonly string[] SHORTNAME = { "9♥", "K♥", "10♥", "A♥", "9♠", "K♠", "10♠", "A♠", "9♣", "K♣", "10♣", "A♣", "7♦", "8♦", "9♦", "K♦", "10♦", "A♦", "J♦", "J♥", "J♠", "J♣", "Q♦", "Q♥", "Q♠", "Q♣" };
        //public static int[] deck


        public static int VALID_MOVES(int card)
        {
            switch (card)
            {
                case EMPTY_CARD: return 16777215;//any card
                case 1: return 15;//hearts
                case 2: return 15;
                case 4: return 15;
                case 8: return 15;
                case 16: return 240;//spades
                case 32: return 240;
                case 64: return 240;
                case 128: return 240;
                case 256: return 3840;//clubs
                case 512: return 3840;
                case 1024: return 3840;
                case 2048: return 3840;
                case 4096: return 16773120;//trumps
                case 8192: return 16773120;
                case 16384: return 16773120;
                case 32768: return 16773120;
                case 65536: return 16773120;
                case 131072: return 16773120;
                case 262144: return 16773120;
                case 524288: return 16773120;
                case 1048576: return 16773120;
                case 2097152: return 16773120;
                case 4194304: return 16773120;
                case 8388608: return 16773120;
                case 16777216: return 16773120;
                case 33554432: return 16773120;
                default: return 0;
            }
        }

        public static int STRONGER(int card)
        {
            switch (card)
            {
                case EMPTY_CARD: return 16777215;//any card
                case 1: return 15;//hearts
                case 2: return 15;
                case 4: return 15;
                case 8: return 15;
                case 16: return 240;//spades
                case 32: return 240;
                case 64: return 240;
                case 128: return 240;
                case 256: return 3840;//clubs
                case 512: return 3840;
                case 1024: return 3840;
                case 2048: return 3840;
                case 4096: return 16773120;//trumps
                case 8192: return 16773120;
                case 16384: return 16773120;
                case 32768: return 16773120;
                case 65536: return 16773120;
                case 131072: return 16773120;
                case 262144: return 16773120;
                case 524288: return 16773120;
                case 1048576: return 16773120;
                case 2097152: return 16773120;
                case 4194304: return 16773120;
                case 8388608: return 16773120;
                case 16777216: return 16773120;
                case 33554432: return 16773120;
                default: return 0;
            }
        }
        public static string SHORTNAME(int card)
        {
            switch (card)
            {
                case 1: return "9♥";//hearts
                case 2: return "K♥";
                case 4: return "10♥";
                case 8: return "A♥";
                case 16: return "9♠";//spades
                case 32: return "K♠";
                case 64: return "10♠";
                case 128: return "A♠";
                case 256: return "9♣";//clubs
                case 512: return "K♣";
                case 1024: return "10♣";
                case 2048: return "A♣";
                case 4096: return "7♦";//trumps
                case 8192: return "8♦";
                case 16384: return "9♦";
                case 32768: return "K♦";
                case 65536: return "10♦";
                case 131072: return "A♦";
                case 262144: return "J♦";
                case 524288: return "J♥";
                case 1048576: return "J♠";
                case 2097152: return "J♣";
                case 4194304: return "Q♦";
                case 8388608: return "Q♥";
                case 16777216: return "Q♠";
                case 33554432: return "Q♣";
                default: return "?";
            }
        }
    }
}