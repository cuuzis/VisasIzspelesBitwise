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

        private int playerCount = 3;
        private int burriedCards;

        public Player(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public int LowestCard(int validMoves)
        {
            return validMoves & -validMoves;
        }
        public int RandomCard(int validMoves)
        {
            throw new NotImplementedException();
        }

        public int PlayCard(int[] moveHistory, int moveCount, int validMoves, int[] playerHands)
        {
            //if (moveCount < 12)
            //    return LowestCard(validMoves);

            int[] hist = (int[])moveHistory.Clone();
            // int[] hands = (int[])playerHands.Clone(); // open hands for now
            int hand0 = playerHands[0];
            int hand1 = playerHands[1];
            int hand2 = playerHands[2];
            burriedCards = playerHands[3];

            double score = -20;
            double newScore;
            int playedCard;
            int bestCard = LowestCard(validMoves);
            while (validMoves != 0)
            {
                playedCard = Deck.RemoveLastCard(ref validMoves);
                newScore = SimulateGame(hist, moveCount, playedCard, Deck.EMPTY_CARD, hand0, hand1, hand2, 0, 0, 0, this);
                if (score < newScore)
                {
                    score = newScore;
                    bestCard = playedCard;
                }
            }
            return bestCard;
        }
        /// <summary>
        /// Returns worst case subtree score.
        /// </summary>
        private double SimulateGame(int[] moveHistory, int moveCount, int playedCard, int trickCard,
            int hand0, int hand1, int hand2, int cards0, int cards1, int cards2, Player activePlayer)
        {
            if (moveCount < 12)
                return 0;
            //Random rand = new Random();
            //return rand.Next(-10, 10);
            //for each valid move: find average score
            double result = -10000;
            int validMoves;
            moveHistory[moveCount++] = playedCard;
            if (activePlayer.ID == 0)
                Deck.RemoveCard(ref hand0, playedCard);
            else if (activePlayer.ID == 1)
                Deck.RemoveCard(ref hand1, playedCard);
            else// if (activePlayer.ID == 2)
                Deck.RemoveCard(ref hand2, playedCard);

            Player nextPlayer;
            if (moveCount % playerCount == 0)
            {
                nextPlayer = Deck.GetWinner(moveHistory, moveCount, activePlayer);
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
                nextPlayer = activePlayer.Next;
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
                if (moveCount % playerCount == 0)
                    trickCard = playedCard;
                // result = min/max(PlayGame(P1))
                // Get related games and play them all. Average score.
                if (result == -10000)
                    result = SimulateGame(moveHistory, moveCount, playedCard, trickCard, hand0, hand1, hand2, cards0, cards1, cards2, nextPlayer);
                else if (activePlayer.Role == PlayerRole.Lielais)
                    result = Math.Max(result, result = SimulateGame(moveHistory, moveCount, playedCard, trickCard, hand0, hand1, hand2, cards0, cards1, cards2, nextPlayer));
                else
                    result = Math.Min(result, result = SimulateGame(moveHistory, moveCount, playedCard, trickCard, hand0, hand1, hand2, cards0, cards1, cards2, nextPlayer));
            }

            // End of game
            if (moveCount == 24)//Deck.SIZE - TABLE_SIZE
            {
                nextPlayer = Deck.GetWinner(moveHistory, moveCount, activePlayer);
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
                //gameCount++;
                result = Deck.GetScore(Deck.VALUE[cards0 | burriedCards]);
                //PrintHistory(moveHistory, moveCount);
                //Console.WriteLine(Deck.VALUE[cards0 | burriedCards] + ": " + result); Console.ReadKey();
            }
            return result;
        }
    }
}
