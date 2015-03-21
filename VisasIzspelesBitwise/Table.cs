using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisasIzspelesBitwise
{
    class Table
    {
        private int tmp = 0;
        private int GetNumTMP()
        {
            if (tmp == 0)
                tmp = 1;
            else if (tmp == 1)
                tmp = 9;
            else if (tmp == 9)
                tmp = 2;
            else if (tmp == 2)
                tmp = 51;
            else if (tmp == 51)
                tmp = 22;
            else if (tmp == 22)
                tmp = 11;
            else if (tmp == 11)
                tmp = 44;
            else if (tmp == 44)
                tmp = 23;
            else if (tmp == 23)
                tmp = 5;
            else if (tmp == 5)
                tmp = 0;
            return tmp;
        }


        private const int HAND_SIZE = 8;
        private const int TABLE_SIZE = 2;
        private int gameCount = 0;
        private int playerCount = 3;
        private Player player1;
        private Player player2;
        private Player player3;
        private Player table;

        public void StartGame()
        {
            player1 = new Player("P1");
            player2 = new Player("P2");
            player3 = new Player("P3");
            table = new Player("Table");
            player1.Next = player2;
            player2.Next = player3;
            player3.Next = player1;

            int[] playerHands = { 0, 0, 0 };
            List<int> deck = new List<int>();
            for (int i = 0; i < Deck.SIZE; i++)
                deck.Add(1 << i);
            // My random sort for debug
            deck = deck.OrderBy(c => GetNumTMP()).ToList();

            // Random sort
            //Random rand = new Random();
            //deck = deck.OrderBy(c => (int)rand.Next()).ToList();

            // Deal cards
            for (int i = 0; i < Deck.SIZE - TABLE_SIZE; i++)
                playerHands[i / HAND_SIZE] = playerHands[i / HAND_SIZE] | deck.ElementAt(i);
            if ((playerHands[0] & playerHands[1] & playerHands[2]) != 0)
                throw new Exception("Incorrect hands");
            int handP1 = playerHands[0];
            int handP2 = playerHands[1];
            int handP3 = playerHands[2];

            PrintHand(playerHands[0]);
            PrintHand(playerHands[1]);
            PrintHand(playerHands[2]);

            int[] moveHistory = new int[Deck.SIZE];
            int playedCard;
            int cardsLeft = playerHands[0];
            while (cardsLeft != 0) //TODO:Multithreading
            {
                playedCard = RemoveLastCard(ref cardsLeft);
                PlayGame(moveHistory, 0, playedCard, playedCard, Deck.EMPTY_CARD, handP1, handP2, handP3, player1);
            }
        }

        private void PlayGame(int[] moveHistory, int moveCount, int playedCard, int trickCard, int highestCard, int handP1, int handP2, int handP3, Player activePlayer)
        {
            int validMoves;
            moveHistory[moveCount++] = playedCard;
            if (activePlayer == player1)
            {
                RemoveCard(ref handP1, playedCard);
                validMoves = GetValidMoves(handP2, trickCard);
            }
            else if (activePlayer == player2)
            {
                RemoveCard(ref handP2, playedCard);
                validMoves = GetValidMoves(handP3, trickCard);
            }
            else// if (activePlayer == player3)
            {
                RemoveCard(ref handP3, playedCard);
                validMoves = GetValidMoves(handP1, trickCard);
            }



            while (validMoves != 0)
            {
                playedCard = RemoveLastCard(ref validMoves);
                PlayGame(moveHistory, moveCount, playedCard, trickCard, highestCard, handP1, handP2, handP3, nextPlayer);
            }

            // Status
            if (moveCount == Deck.SIZE - TABLE_SIZE)
            {
                gameCount++;
                if (gameCount % 1000000 == 0)
                    Console.WriteLine(gameCount);
                PrintHistory(moveHistory);
            }
        }

        private int GetValidMoves(int hand, int trickCard)
        {
            int validMoves = Intersection(hand, Deck.VALID_MOVES(trickCard));
            if (validMoves == 0)
                validMoves = hand;
            return validMoves;
        }

        private int RemoveLastCard(ref int hand)
        {
            int card = hand & -hand;
            hand = hand ^ card;
            return card;
        }
        private int GetLastCard(int hand)
        {
            return hand & -hand;
        }

        private void RemoveCard(ref int hand, int card)
        {
            hand = hand ^ card;
        }

        private int Intersection(int hand1, int hand2)
        {
            return hand1 & hand2;
        }

        private void PrintHistory(int[] moveHistory)
        {
            if (!Program.WRITE_TO_CONSOLE && !Program.WRITE_TO_FILE)
                return;
            for (int i = 0; i < Deck.SIZE - TABLE_SIZE; i++)
                Console.Write(Deck.SHORTNAME(moveHistory[i]) + " ");
            Console.ReadLine();
        }

        private void PrintHand(int hand)
        {
            if (!Program.WRITE_TO_CONSOLE && !Program.WRITE_TO_FILE)
                return;
            while (hand != 0)
                Console.Write(Deck.SHORTNAME(RemoveLastCard(ref hand)) + " ");
            Console.WriteLine();
        }

        //debug
        public void PrintBinaryInt(int n)
        {
            char[] b = new char[32];
            int pos = 31;

            for (int i = 0; i < 32; i++ )
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
