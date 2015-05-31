using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisasIzspelesBitwise
{
    public class Table
    {
        //public const int HAND_SIZE = 8;//to deck
        //public const int TABLE_SIZE = 2;//to deck
        public int playerCount = 3;// constant for now
        private Player player1;
        private Player player2;
        private Player player3;
        private Player table;

        private static Random randT = new Random(234);

        public void StartGame()
        {
            // TODO: Zole.cs assigns players to tables
            player1 = new Player(this, 0, "P1");
            player2 = new Player(this, 1, "P2");
            player3 = new Player(this, 2, "P3");
            table = new Player(this, 3, "Table");
            player1.Next = player2;
            player2.Next = player3;
            player3.Next = player1;

            for (int test = 0; test < 100; test++) // 100 test runs
            {
                int[] playerHands = Deck.GetRandomHands();                   

                // Output
                /*Console.WriteLine("Dealt cards: ");
                Deck.PrintHand(playerHands[0], "P1");
                Deck.PrintHand(playerHands[1], "P2");
                Deck.PrintHand(playerHands[2], "P3");
                Deck.PrintHand(playerHands[3], "table");
                Console.WriteLine();*/

                // Assigns random player roles. TODO: role selection
                Player lielais = player1;
                for (int i = 0; i < randT.Next(0, playerCount - 1); i++)
                {
                    lielais.Role = Player.PlayerRole.Mazais;
                    lielais = lielais.Next;
                }
                lielais.Role = Player.PlayerRole.Lielais;

                // Card burrying
                if (lielais.Role == Player.PlayerRole.Lielais)
                {
                    playerHands[lielais.ID] = playerHands[lielais.ID] | playerHands[table.ID];
                    playerHands[table.ID] = lielais.BurryCards(playerHands[lielais.ID]);
                    playerHands[lielais.ID] = playerHands[lielais.ID] ^ playerHands[table.ID];
                }

                //Console.Write("Lielais: {0}, burried: ", lielais.Name); Deck.PrintHand(playerHands[table.ID]);
                PlayGame(playerHands, player1);
            }
        }

        private void PlayGame(int[] playerHands, Player activePlayer)
        {
            Stopwatch stopwatch = new Stopwatch(); 

            int[] moveHistory = new int[Deck.SIZE];
            int trickCard = Deck.EMPTY_CARD;
            int validMoves;
            int playedCard;
            int[] playerTricks = new int[playerCount];

            for (int moveCount = 0; moveCount < Deck.SIZE - Deck.TABLE_SIZE; moveCount++ )
            {
                validMoves = Deck.GetValidMoves(playerHands[activePlayer.ID], trickCard);

                stopwatch.Restart();
                playedCard = activePlayer.PlayCard(moveHistory, moveCount, playerTricks, validMoves, trickCard, playerHands); //TODO: remove playerHands
                stopwatch.Stop();
                Console.Write("{0}\t", stopwatch.Elapsed.TotalSeconds);

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
            Console.WriteLine();

            /*int burriedCards = playerHands[3];
            Console.WriteLine("Played game:");
            Deck.PrintHistory(moveHistory, 24);
            while (activePlayer.Role != Player.PlayerRole.Lielais)
                activePlayer = activePlayer.Next;
            int bigscore = Deck.GetScore(Deck.VALUE[playerTricks[activePlayer.ID] | burriedCards]);
            Console.WriteLine("P{0} score:", (activePlayer.ID + 1)); Deck.PrintHand(playerTricks[activePlayer.ID]);
            Console.WriteLine(Deck.VALUE[playerTricks[activePlayer.ID] | burriedCards] + ": " + bigscore);
            activePlayer = activePlayer.Next;
            Console.WriteLine("P{0} score:", (activePlayer.ID + 1)); Deck.PrintHand(playerTricks[activePlayer.ID]);
            Console.WriteLine(Deck.VALUE[playerTricks[activePlayer.ID] | burriedCards] + ": " + (0 - bigscore / 2));
            activePlayer = activePlayer.Next;
            Console.WriteLine("P{0} score:", (activePlayer.ID + 1)); Deck.PrintHand(playerTricks[activePlayer.ID]);
            Console.WriteLine(Deck.VALUE[playerTricks[activePlayer.ID] | burriedCards] + ": " + (0 - bigscore / 2));
            Console.WriteLine();*/
        }
    }
}
