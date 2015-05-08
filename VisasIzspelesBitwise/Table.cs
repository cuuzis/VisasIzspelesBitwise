using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisasIzspelesBitwise
{
    class Table
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


        private const int HAND_SIZE = 8;
        private const int TABLE_SIZE = 2;
        private long gameCount = 0;
        private const int playerCount = 3;// constant for now
        private Player player1;
        private Player player2;
        private Player player3;
        private Player table;

        public void StartGame()
        {
            player1 = new Player(0, "P1");
            player2 = new Player(1, "P2");
            player3 = new Player(2, "P3");
            table = new Player(3, "Table");
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
            //Random rand = new Random();
            //deck = deck.OrderBy(c => (int)rand.Next()).ToList();
            for (int i = 0; i < Deck.SIZE; i++)
                    playerHands[i / HAND_SIZE] |=  deck.ElementAt(i);
            if ((playerHands[0] ^ playerHands[1] ^ playerHands[2] ^ playerHands[3]) != Deck.FULL_DECK)
                throw new Exception("Incorrect hands");

            // Output
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
            Player firstMover = activePlayer;
            int[] moveHistory = new int[Deck.SIZE];
            int trickCard = Deck.EMPTY_CARD;
            int validMoves;
            int playedCard;
            int[] playerTricks = new int[playerCount];

            for (int moveCount = 0; moveCount < Deck.SIZE - TABLE_SIZE; moveCount++ )
            {
                validMoves = Deck.GetValidMoves(playerHands[activePlayer.ID], trickCard);
                playedCard = activePlayer.PlayCard(moveHistory, 0, validMoves, playerHands); //!playerHands
                moveHistory[moveCount] = playedCard;
                Deck.RemoveCard(ref playerHands[activePlayer.ID], playedCard);
                if (moveCount % playerCount == 0)
                    trickCard = moveHistory[moveCount];
                if (moveCount % playerCount == 2) {
                    activePlayer = Deck.GetWinner(moveHistory, moveCount + 1, activePlayer);
                    playerTricks[activePlayer.ID] |= moveHistory[moveCount] | moveHistory[moveCount - 1] | moveHistory[moveCount - 2];
                }
                else
                    activePlayer = activePlayer.Next;
            }
            // Output
            int burriedCards = playerHands[3];
            activePlayer = firstMover;
            Console.WriteLine("Results:");
            Console.WriteLine("P" + activePlayer.ID);
            Deck.PrintHand(playerTricks[activePlayer.ID]);
            Console.WriteLine(Deck.VALUE[playerTricks[activePlayer.ID] | burriedCards] + ": " + Deck.GetScore(Deck.VALUE[playerTricks[activePlayer.ID] | burriedCards]));
            activePlayer = activePlayer.Next;
            Console.WriteLine("P" + activePlayer.ID);
            Deck.PrintHand(playerTricks[activePlayer.ID]);
            Console.WriteLine(Deck.VALUE[playerTricks[activePlayer.ID] | burriedCards] + ": " + Deck.GetScore(Deck.VALUE[playerTricks[activePlayer.ID] | burriedCards]));
            activePlayer = activePlayer.Next;
            Console.WriteLine("P" + activePlayer.ID);
            Deck.PrintHand(playerTricks[activePlayer.ID]);
            Console.WriteLine(Deck.VALUE[playerTricks[activePlayer.ID] | burriedCards] + ": " + Deck.GetScore(Deck.VALUE[playerTricks[activePlayer.ID] | burriedCards]));
            Console.WriteLine();
        }
    }
}
