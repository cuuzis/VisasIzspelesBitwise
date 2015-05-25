﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisasIzspelesBitwise
{
    public class Table
    {
        // Debug + profiling
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


        public const int HAND_SIZE = 8;
        public const int TABLE_SIZE = 2;
        public int playerCount = 3;// constant for now
        private Player player1;
        private Player player2;
        private Player player3;
        private Player table;

        public void StartGame()
        {
            // Zole.cs assigns players to tables
            player1 = new Player(this, 0, "P1");
            player2 = new Player(this, 1, "P2");
            player3 = new Player(this, 2, "P3");
            table = new Player(this, 3, "Table");
            player1.Next = player2;
            player2.Next = player3;
            player3.Next = player1;
            player1.Role = Player.PlayerRole.Lielais;
            player2.Role = Player.PlayerRole.Mazais;
            player3.Role = Player.PlayerRole.Mazais;

            // Deal cards
            int[] playerHands = { 0, 0, 0, 0 };
            List<int> deck = new List<int>();
            for (int i = 0; i < Deck.SIZE; i++)
                deck.Add(1 << i);
            // My random sort for debug
            deck = deck.OrderBy(c => GetNumTMP()).ToList();
            // Random sort
            Random rand = new Random(); // TODO: Move to Deck, implement Knuth shuffle
            deck = deck.OrderBy(c => (int)rand.Next()).ToList();
            for (int i = 0; i < Deck.SIZE; i++)
                    playerHands[i / HAND_SIZE] |=  deck.ElementAt(i);
            if ((playerHands[0] ^ playerHands[1] ^ playerHands[2] ^ playerHands[3]) != Deck.FULL_DECK)
                throw new Exception("Incorrect hands");

            /*playerHands[0] = Convert.ToInt32("00000010100001000001000110010001", 2);
            playerHands[1] = Convert.ToInt32("00000000001000110110001001001000", 2);
            playerHands[2] = Convert.ToInt32("00000001000110001000110000100010", 2);
            playerHands[3] = Convert.ToInt32("00000000010000000000000000000100", 2);*/

            // Output
            Deck.PrintBinaryInt(playerHands[0]);
            Deck.PrintBinaryInt(playerHands[1]);
            Deck.PrintBinaryInt(playerHands[2]);
            Deck.PrintBinaryInt(playerHands[3]);
            Console.WriteLine("Dealt cards: ");
            Deck.PrintHand(playerHands[0], "P1");
            Deck.PrintHand(playerHands[1], "P2");
            Deck.PrintHand(playerHands[2], "P3");
            Deck.PrintHand(playerHands[3], "table");
            Console.WriteLine();
            PlayGame(playerHands, player1);
        }

        private void PlayGame(int[] playerHands, Player activePlayer)
        {
            //Player firstMover = activePlayer;
            int[] moveHistory = new int[Deck.SIZE];
            int trickCard = Deck.EMPTY_CARD;
            int validMoves;
            int playedCard;
            int[] playerTricks = new int[playerCount];

            for (int moveCount = 0; moveCount < Deck.SIZE - TABLE_SIZE; moveCount++ )
            {
                validMoves = Deck.GetValidMoves(playerHands[activePlayer.ID], trickCard);
                playedCard = activePlayer.PlayCard(moveHistory, moveCount, playerTricks, validMoves, trickCard, playerHands); //!playerHands
                moveHistory[moveCount] = playedCard;
                Deck.RemoveCard(ref playerHands[activePlayer.ID], playedCard);
                if (moveCount % playerCount == 0)
                    trickCard = moveHistory[moveCount];
                if (moveCount % playerCount == 2) {
                    activePlayer = Deck.GetWinner(moveHistory, moveCount + 1, activePlayer);
                    playerTricks[activePlayer.ID] |= moveHistory[moveCount] | moveHistory[moveCount - 1] | moveHistory[moveCount - 2];
                    trickCard = Deck.EMPTY_CARD;
                }
                else
                    activePlayer = activePlayer.Next;
            }
            // Output
            int burriedCards = playerHands[3];
            //activePlayer = firstMover;
            Console.WriteLine("Played game:");
            Deck.PrintHistory(moveHistory, 24);
            while (activePlayer.Role != Player.PlayerRole.Lielais)
                activePlayer = activePlayer.Next;
            int bigscore = Deck.GetScore(Deck.VALUE[playerTricks[activePlayer.ID] | burriedCards]);
            Console.WriteLine("P"+(activePlayer.ID+1)+" score:"); Deck.PrintHand(playerTricks[activePlayer.ID]);
            Console.WriteLine(Deck.VALUE[playerTricks[activePlayer.ID] | burriedCards] + ": " + bigscore);
            activePlayer = activePlayer.Next;
            Console.WriteLine("P" + (activePlayer.ID + 1) + " score:"); Deck.PrintHand(playerTricks[activePlayer.ID]);
            Console.WriteLine(Deck.VALUE[playerTricks[activePlayer.ID] | burriedCards] + ": " + (0-bigscore/2));
            activePlayer = activePlayer.Next;
            Console.WriteLine("P" + (activePlayer.ID + 1) + " score:"); Deck.PrintHand(playerTricks[activePlayer.ID]);
            Console.WriteLine(Deck.VALUE[playerTricks[activePlayer.ID] | burriedCards] + ": " + (0 - bigscore / 2));
            Console.WriteLine();
        }
    }
}
