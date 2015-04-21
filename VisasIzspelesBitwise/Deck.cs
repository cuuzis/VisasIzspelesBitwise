using System;
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
        public const int MAX_CARD = 33554432;
        public const int FULL_DECK = MAX_CARD * 2 - 1;

        public static readonly int[] VALID_MOVES = new int[MAX_CARD + 1];
        public static readonly int[] STRONGER = new int[MAX_CARD + 1];
        public static readonly int[] VALUE = new int[FULL_DECK +1];

        public Deck()
        {
            for (int card = 1; card <= MAX_CARD; card = (card << 1))
                VALID_MOVES[card] = _VALID_MOVES(card);

            for (int card = 1; card <= MAX_CARD; card = (card << 1))
                STRONGER[card] = _STRONGER(card);

            for (int hand = 0; hand <= FULL_DECK; hand++)
                VALUE[hand] = _VALUE(hand);


            if (VALUE[FULL_DECK] != 120)
                throw new Exception("Deck value is not 120!");
        }

        /// <summary>
        /// Returns valid moves for a lead card.
        /// </summary>
        private static int _VALID_MOVES(int card)
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
                case 33554432: return 16773120;//MAX_CARD
                default: return 0;
            }
        }

        /// <summary>
        ///  Returns all cards stronger than card.
        /// </summary>
        private static int _STRONGER(int card)
        {
            switch (card)
            {
                case EMPTY_CARD: return 16777215;//any card
                case 1: return 67104782;//hearts
                case 2: return 67104780;
                case 4: return 67104776;
                case 8: return 67104768;
                case 16: return 67104992;//spades
                case 32: return 67104960;
                case 64: return 67104896;
                case 128: return 67104768;
                case 256: return 67108352;//clubs
                case 512: return 67107840;
                case 1024: return 67106816;
                case 2048: return 67104768;
                case 4096: return 67100672;//trumps
                case 8192: return 67092480;
                case 16384: return 67076096;
                case 32768: return 67043328;
                case 65536: return 66977792;
                case 131072: return 66846720;
                case 262144: return 66584576;
                case 524288: return 66060288;
                case 1048576: return 65011712;
                case 2097152: return 62914560;
                case 4194304: return 58720256;
                case 8388608: return 50331648;
                case 16777216: return 33554432;//MAX_CARD
                case 33554432: return 0;
                default: return 0;
            }
        }
        /// <summary>
        /// Returns point value for any card combination.
        /// </summary>
        private static int _VALUE(int hand)
        {
            int result = 0;
            if ((hand & 2) > 0) result += 4;
            if ((hand & 4) > 0) result += 10;
            if ((hand & 8) > 0) result += 11;
            if ((hand & 32) > 0) result += 4;
            if ((hand & 64) > 0) result += 10;
            if ((hand & 128) > 0) result += 11;
            if ((hand & 512) > 0) result += 4;
            if ((hand & 1024) > 0) result += 10;
            if ((hand & 2048) > 0) result += 11;
            if ((hand & 32768) > 0) result += 4;
            if ((hand & 65536) > 0) result += 10;
            if ((hand & 131072) > 0) result += 11;
            if ((hand & 262144) > 0) result += 2;
            if ((hand & 524288) > 0) result += 2;
            if ((hand & 1048576) > 0) result += 2;
            if ((hand & 2097152) > 0) result += 2;
            if ((hand & 4194304) > 0) result += 3;
            if ((hand & 8388608) > 0) result += 3;
            if ((hand & 16777216) > 0) result += 3;
            if ((hand & 33554432) > 0) result += 3;
            return result;
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
                case 33554432: return "Q♣";//MAX_CARD
                default: return "?";
            }
        }
    }
}
