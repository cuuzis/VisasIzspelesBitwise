using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Security.Cryptography;

// Utility class
namespace VisasIzspelesBitwise
{
    //public static class Deck
    public class Deck
    {
        public const int SIZE = 26;
        public const int EMPTY_CARD = 0;
        public const int MAX_CARD = 33554432;
        public const int FULL_DECK = MAX_CARD * 2 - 1;

        public const int HAND_SIZE = 8;
        public const int TABLE_SIZE = 2;

        public const double MAX_SCORE = 10000;
        public const double MIN_SCORE = -10000;

        public static readonly int[] VALID_MOVES = new int[MAX_CARD + 1];
        public static readonly int[] STRONGER = new int[MAX_CARD + 1];
        public static readonly int[] VALUE = new int[FULL_DECK + 1];
        public static readonly int[] NEXTHANDSIZE = new int[SIZE];

        private static Random rand = new Random(12);
        private static Random rand2 = new Random(23);
        //private static int md5seed = 0;

        //static Deck()
        public Deck()
        {
            for (int card = 1; card <= MAX_CARD; card = (card << 1))
                VALID_MOVES[card] = _VALID_MOVES(card);

            for (int card = 1; card <= MAX_CARD; card = (card << 1))
                STRONGER[card] = _STRONGER(card);

            for (int hand = 0; hand <= FULL_DECK; hand++)
                VALUE[hand] = _VALUE(hand);

            for (int move = 0; move < SIZE; move++)
                NEXTHANDSIZE[move] = _NEXTHANDSIZE(move);



            if (VALUE[FULL_DECK] != 120)
                throw new Exception("Deck value is not 120!");
        }

            /*MD5 md5Hash = MD5.Create();
            using (md5Hash)
            {
                md5seed++;

                // Convert the input string to a byte array and compute the hash. 
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes("wasd"+md5seed));

                // Create a new Stringbuilder to collect the bytes 
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data  
                // and format each one as a hexadecimal string. 
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string. 
                string hash = sBuilder.ToString();

                int result = 0;
                foreach (char ch in hash)
                {
                    result += (int)ch;
                }
                return result;
            }*/

        public static int[] GetRandomHands()
        {
            int[] playerHands = { 0, 0, 0, 0 };
            List<int> deck = new List<int>();
            for (int i = 0; i < Deck.SIZE; i++)
                deck.Add(1 << i);
            // Random sort
            //Random rand = new Random(); // TODO: Move to Deck, implement Knuth shuffle
            deck = deck.OrderBy(c => (int)rand.Next()).ToList();
            //deck = deck.OrderBy(c => Deck.Next()).ToList();
            for (int i = 0; i < Deck.SIZE; i++)
                playerHands[i / HAND_SIZE] |= deck.ElementAt(i);
            if ((playerHands[0] ^ playerHands[1] ^ playerHands[2] ^ playerHands[3]) != Deck.FULL_DECK)
                throw new Exception("Incorrect hands");
            return playerHands;
        }

        /// <summary>
        /// Returns valid moves for a lead card.
        /// </summary>
        private static int _VALID_MOVES(int card)
        {
            switch (card)
            {
                case EMPTY_CARD: return FULL_DECK;//any card
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
                case EMPTY_CARD: return FULL_DECK;//any card
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
                case 16777216: return 33554432;
                case 33554432: return 0;//MAX_CARD
                default: return 0;
            }
        }
        /// <summary>
        /// Returns card points value for any card combination.
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
        /// <summary>
        /// Returns number of cards left in next player's hand.
        /// </summary>
        private static int _NEXTHANDSIZE(int moveCount)
        {
            return HAND_SIZE - ((moveCount+1) / 3); //3 == player count
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


        // Public utility methods
        public static Player GetWinner(int[] moveHistory, int moveCount, Player activePlayer)
        {
            if (IsStronger(moveHistory[moveCount - 2], moveHistory[moveCount - 3]))
            {
                if (IsStronger(moveHistory[moveCount - 1], moveHistory[moveCount - 2]))
                    return activePlayer;
                else
                    return activePlayer.Next.Next;
            }
            else if (IsStronger(moveHistory[moveCount - 1], moveHistory[moveCount - 3]))
                return activePlayer;
            else
                return activePlayer.Next;
        }

        public static int GetValidMoves(int hand, int trickCard)
        {
            int validMoves = Intersection(hand, Deck.VALID_MOVES[trickCard]);
            if (validMoves == 0)
                validMoves = hand;
            return validMoves;
        }

        public static int AllHistoryCards(int[] moveHistory, int moveCount)
        {
            int cards = 0;
            for (int i = 0; i < moveCount; i++)
                cards |= moveHistory[i];
            return cards;
        }

        /// <summary>
        /// Returns true if card1 > card2.
        /// </summary>
        public static bool IsStronger(int card1, int card2)
        {
            return ((card1 & Deck.STRONGER[card2]) != 0);
        }
        public static int RemoveLowestCard(ref int hand)
        {
            int card = hand & -hand;
            hand = hand ^ card;
            return card;
        }
        public static int GetLowestCard(int hand)
        {
            return hand & -hand;
        }

        public static void RemoveCard(ref int hand, int card)
        {
            hand = hand ^ card;
        }

        public static int Intersection(int hand1, int hand2)
        {
            return hand1 & hand2;
        }

        public static int CountCards(int hand)
        {
            int count = 0;
            while (hand != 0)
            {
                hand &= hand - 1;
                count++;
            }
            return count;
        }

        public static int PickRandomCard(int hand)
        {
            int card = 0;
            int count = CountCards(hand);
            for (int i = 0; i < rand2.Next(1, count); i++)
            //for (int i = 0; i < (Next() % count)+1; i++)
                card = RemoveLowestCard(ref hand);
            return card;
        }

        public static void PrintHistory(int[] moveHistory, int moveCount)
        {
            for (int i = 0; i < moveCount; i++)
                Console.Write(Deck.SHORTNAME(moveHistory[i]) + " ");
            Console.WriteLine();
        }

        public static void PrintHand(int hand, string message = "")
        {
            if (message != "")
                Console.Write(message + ": ");
            while (hand != 0)
                Console.Write(Deck.SHORTNAME(RemoveLowestCard(ref hand)) + " ");
            Console.WriteLine();
        }

        public static int GetScore(int points)  // Lielā rezultāts - 3 spēlētāji, bez pulēm // Needs speedy lookup table SCORE[120]
        {
            if (points == 0) return -8;
            else if (points <= 30) return -6;
            else if (points <= 60) return -4;
            else if (points < 90) return 2;
            else if (points < 120) return 4;
            else if (points == 120) return 6; //single trick with 0 points?
            return 0;
        }

        /// <summary>
        /// Returns all possible subsets of size "size" from given cards.
        /// </summary>
        /// <param name="cards">Set of cards</param>
        /// <param name="size">Size of subsets</param>
        /// <returns>All possible subsets of size "size" from given cards</returns>
        public static IEnumerable<int> Combinations(int cards, int size, int a = 0, int elems = 0)
        {
            if (elems == size)
            {
                yield return a;
            }
            while (cards != 0)
            {
                int b = Deck.RemoveLowestCard(ref cards);
                foreach (int c in Combinations(cards, size, a | b, elems + 1))
                    yield return c;
            }
        }

        //debug
        public static void PrintBinaryInt(int n)
        {
            char[] b = new char[32];
            int pos = 31;

            for (int i = 0; i < 32; i++)
            {
                if ((n & (1 << i)) != 0)
                {
                    b[pos] = '1';
                }
                else
                {
                    b[pos] = '0';
                }
                pos--;
            }
            Console.WriteLine(b);
        }
    }
}
