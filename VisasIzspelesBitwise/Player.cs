using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisasIzspelesBitwise
{
    public class Player
    {
        public enum PlayerRole { Mazais, Lielais };
        public PlayerRole Role;
        public int ID { get; private set; } // ID 0, 1, 2 = pirmā, otrā, trešā roka
        public string Name { get; private set; }
        public Player Next;
        public Table Table;

        private const int START_FROM = 15;
        //private static bool dbg = true;

        public Player(Table table, int id, string name)
        {
            Table = table;
            ID = id;
            Name = name;
        }

        public int PlayCard(int[] moveHistory, int moveCount, int[] playerTricks, int validMoves, int trickCard, int[] playerHands)
        {
            if (moveCount < START_FROM)
                return Deck.GetLastCard(validMoves);

            int[] hist = (int[])moveHistory.Clone();
            // int[] hands = (int[])playerHands.Clone(); // open hands for now
            int hand0 = playerHands[0];
            int hand1 = playerHands[1];
            int hand2 = playerHands[2];
            int burriedCards = playerHands[3];
            int cards0 = playerTricks[0];
            int cards1 = playerTricks[1];
            int cards2 = playerTricks[2];

            double score;
            if (this.Role == PlayerRole.Lielais)
                score = Deck.MIN_SCORE;
            else
                score = Deck.MAX_SCORE;
            double newScore;
            long gameCount = 0;
            int playedCard;
            int bestCard = Deck.GetLastCard(validMoves);
            while (validMoves != 0)
            {
                playedCard = Deck.RemoveLastCard(ref validMoves);
                newScore = this.SimulateGame(hist, moveCount, playedCard, trickCard, hand0, hand1, hand2, burriedCards, cards0, cards1, cards2, ref gameCount);
                if (((this.Role == PlayerRole.Lielais) && (score < newScore)) || ((this.Role == PlayerRole.Mazais) && (score > newScore)))
                {
                    score = newScore;
                    bestCard = playedCard;
                }
                // output
                if (moveCount == START_FROM) {
                    Console.WriteLine("Unique games " + Deck.SHORTNAME(playedCard) + ": " + gameCount + "\tscore: " + newScore); gameCount = 0;
                }
            }
            return bestCard;
        }

        /// <summary>
        /// Returns worst case subtree score.
        /// </summary>
        private double SimulateGame(int[] moveHistory, int moveCount, int playedCard, int trickCard,
            int hand0, int hand1, int hand2, int burriedCards, int cards0, int cards1, int cards2, ref long gameCount)
        {
            double score;
            if (this.Role == PlayerRole.Lielais)
                score = Deck.MIN_SCORE;
            else
                score = Deck.MAX_SCORE;
            int validMoves;
            if (moveCount % this.Table.playerCount == 0)
                trickCard = playedCard;
            moveHistory[moveCount++] = playedCard;
            if (this.ID == 0)
                Deck.RemoveCard(ref hand0, playedCard);
            else if (this.ID == 1)
                Deck.RemoveCard(ref hand1, playedCard);
            else// if (activePlayer.ID == 2)
                Deck.RemoveCard(ref hand2, playedCard);

            Player nextPlayer;
            if (moveCount % this.Table.playerCount == 0) // trick ends
            {
                nextPlayer = Deck.GetWinner(moveHistory, moveCount, this);
                if (nextPlayer.ID == 0)
                {
                    validMoves = hand0;
                    cards0 |= moveHistory[moveCount - 3] | moveHistory[moveCount - 2] | moveHistory[moveCount - 1];
                }
                else if (nextPlayer.ID == 1)
                {
                    validMoves = hand1;
                    cards1 |= moveHistory[moveCount - 3] | moveHistory[moveCount - 2] | moveHistory[moveCount - 1];
                }
                else
                {//if (nextPlayer.ID == 2)
                    validMoves = hand2;
                    cards2 |= moveHistory[moveCount - 3] | moveHistory[moveCount - 2] | moveHistory[moveCount - 1];
                }
            }
            else
            {
                nextPlayer = this.Next;
                if (nextPlayer.ID == 0)
                    validMoves = Deck.GetValidMoves(hand0, trickCard);
                else if (nextPlayer.ID == 1)
                    validMoves = Deck.GetValidMoves(hand1, trickCard);
                else// if (nextPlayer.ID == 2)
                    validMoves = Deck.GetValidMoves(hand2, trickCard);
            }
            while (validMoves != 0)
            {
                playedCard = Deck.RemoveLastCard(ref validMoves);
                double newScore = nextPlayer.SimulateGame(moveHistory, moveCount, playedCard, trickCard, hand0, hand1, hand2, burriedCards, cards0, cards1, cards2, ref gameCount);
                if (this.Role == PlayerRole.Lielais)
                    score = Math.Max(score, newScore);
                else
                    score = Math.Min(score, newScore);
            }

            // End of game
            if (moveCount == this.Table.playerCount * Table.HAND_SIZE)
            {
                score = Deck.GetScore(Deck.VALUE[cards0 | burriedCards]);
                gameCount++;
                /*if (score == -6 && dbg)
                {
                    Deck.PrintHistory(moveHistory, 24); Console.WriteLine(Deck.VALUE[cards0 | burriedCards] + ": " + score);
                    Console.WriteLine(Deck.VALUE[cards0 | burriedCards] + ": " + Deck.GetScore(Deck.VALUE[cards0 | burriedCards]));
                    Deck.PrintHand(cards0);
                    Console.WriteLine(Deck.VALUE[cards1 | burriedCards] + ": " + Deck.GetScore(Deck.VALUE[cards1 | burriedCards]));
                    Deck.PrintHand(cards1);
                    Console.WriteLine(Deck.VALUE[cards2 | burriedCards] + ": " + Deck.GetScore(Deck.VALUE[cards2 | burriedCards]));
                    Deck.PrintHand(cards2);
                    Console.WriteLine();
                    dbg = false;
                }*/
            }
            return score;
        }
    }
}
