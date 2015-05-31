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

        private const int LAST_ROUND = 22;
        private const int START_FROM = 12;

        public Player(Table table, int id, string name)
        {
            Table = table;
            ID = id;
            Name = name;
        }

        /// <summary>
        /// Returns 2 cards to burry for a given hand. Hand size should be 10.
        /// </summary>
        public int BurryCards(int hand)
        {
            // takes 2 lowest cards
            return Deck.RemoveLowestCard(ref hand) | Deck.RemoveLowestCard(ref hand);
        }
        
        /// <summary>
        /// Returns choice of role
        /// </summary>
        private PlayerRole PickRole(int hand) //int position (is ID for now)
        {
            throw new NotImplementedException();
        }

        public int PlayCard(int[] moveHistory, int moveCount, int[] playerTricks, int validMoves, int trickCard, int[] playerHands) // TODO: no player hands
        {
            //TODO: play only card in last round - also Table.cs
            if (moveCount >= LAST_ROUND - 1)
                return Deck.RemoveLowestCard(ref validMoves);

            if (moveCount < START_FROM - 1)
                return Deck.PickRandomCard(validMoves);
            //if (moveCount == START_FROM)
            //    Deck.PrintHistory(moveHistory, moveCount);

            int[] hist = (int[])moveHistory.Clone();
            int[] hands = new int[playerHands.Length];

            //Console.WriteLine(moveCount);

            // player cards won:
            int cards0 = playerTricks[0];
            int cards1 = playerTricks[1];
            int cards2 = playerTricks[2];
            
            double score;
            if (this.Role == PlayerRole.Lielais)
                score = Deck.MIN_SCORE;
            else
                score = Deck.MAX_SCORE;
            long gameCount = 0;
            int playedCard;
            int bestCard = Deck.GetLowestCard(validMoves);
            while (validMoves != 0)
            {
                playedCard = Deck.RemoveLowestCard(ref validMoves);

                double newScore = 0;
                int simulations = 0;
                int unknownCards = Deck.FULL_DECK ^ Deck.AllHistoryCards(moveHistory, moveCount) ^ playerHands[this.ID];
                int unknownBurried = unknownCards;
                if (this.Role == PlayerRole.Lielais)
                    unknownBurried = playerHands[3]; // burried cards are known

                foreach (int possibleBurried in Deck.Combinations(unknownBurried, Deck.TABLE_SIZE)) // Tests all possible burried cards. TODO: ignore high burried cards
                {
                    hands[3] = possibleBurried;
                    int nextHandSize = Deck.NEXTHANDSIZE[moveCount];
                    foreach (int possibleHand in Deck.Combinations(unknownCards ^ possibleBurried, nextHandSize)) //TODO: learn if player has trumps
                    {
                        hands[this.ID] = playerHands[this.ID];
                        hands[this.Next.ID] = possibleHand;
                        hands[this.Next.Next.ID] = unknownCards ^ possibleBurried ^ possibleHand;

                        double alpha, beta;
                        alpha = Deck.MIN_SCORE; 
                        beta = Deck.MAX_SCORE;

                        //simulates all possible games
                        newScore += this.SimulateGame(hist, moveCount, playedCard, trickCard, hands[0], hands[1], hands[2], hands[3], cards0, cards1, cards2, alpha, beta, ref gameCount);
                        simulations++;
                    }
                }
                newScore = newScore / simulations;
                //if (moveCount == START_FROM)
                //    Console.WriteLine(Deck.SHORTNAME(playedCard) + " score:\t" + newScore + "\tSimulations:\t" + simulations); //Console.ReadKey();
                if (((this.Role == PlayerRole.Lielais) && (score < newScore)) || ((this.Role == PlayerRole.Mazais) && (score > newScore)))
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
            int hand0, int hand1, int hand2, int burriedCards, int cards0, int cards1, int cards2, double alpha, double beta, ref long gameCount)
        {
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
            double score;
            if (nextPlayer.Role == PlayerRole.Lielais)
                score = Deck.MIN_SCORE;
            else
                score = Deck.MAX_SCORE;
            double newScore = score;
            while (validMoves != 0)
            {
                playedCard = Deck.RemoveLowestCard(ref validMoves);
                newScore = nextPlayer.SimulateGame(moveHistory, moveCount, playedCard, trickCard, hand0, hand1, hand2, burriedCards, cards0, cards1, cards2, alpha, beta, ref gameCount);
                if (nextPlayer.Role == PlayerRole.Lielais)
                {
                    score = Math.Max(score, newScore);
                    alpha = Math.Max(alpha, score);
                    if (beta <= alpha)
                        break;
                }
                else
                {
                    score = Math.Min(score, newScore);
                    beta = Math.Min(beta, score);
                    if (beta <= alpha)
                        break;
                }
            }

            // End of game
            if (moveCount == this.Table.playerCount * Deck.HAND_SIZE)
            {
                score = Deck.GetScore(Deck.VALUE[cards0 | burriedCards]);  // round score
                //score = Deck.VALUE[cards0 | burriedCards];                  // round eyes
                gameCount++;
            }
            if (score == Deck.MAX_SCORE || score == Deck.MIN_SCORE)
                throw new Exception("Bad score");
            return score;
        }
    }
}
