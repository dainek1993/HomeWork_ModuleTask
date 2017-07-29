using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_ModuleTask
{
    enum GameResult
    {
        PlayerWin,
        DealerWin,
        DeadHeat
    }
    enum Suit
    {
        Hearts,
        Clovers,
        Diamonds,
        Pikes
    }

    enum Rank
    {
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 2,
        Lady = 3,
        King = 4,
        Ace = 11
    }

    struct Player
    {
        public Card[] Hand;
        public int Wins;
        public bool IsDealer;
        public bool IsBlackDjack;
        public bool IsOverLoad;
    }

    struct Card
    {
        public Card(Rank rank, Suit suit)
        {
            Rank = rank;
            Suit = suit;
        }

        public Rank Rank;
        public Suit Suit;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player() {IsDealer = false};
            Player dealer = new Player() {IsDealer = true};
            int command = 0;
            //Console.SetWindowSize(80, 10);

            do
            {
                Console.WriteLine("1 - Game, 2 - View score 3 - Exit");
                command = int.Parse(Console.ReadLine());
                switch (command)
                {
                    case 1:
                        
                        StartGame(ref player, ref dealer);
                        break;

                    case 2:
                        WriteStats(player, dealer);
                        break;

                    case 3:
                        WriteStats(player, dealer);
                        break;

                    default:
                        Console.WriteLine("Unknown command");
                        break;
                }
            }
            while (command != 3);
        }

        static Card[] GenerateDeck()
        {
            Card[] deck = new Card[36];
            int index = 0;
            foreach (var v in Enum.GetValues(typeof(Rank)))
            {
                for (int i = 0; i < Enum.GetNames(typeof(Suit)).Length; i++, index++)
                {
                    deck[index] = new Card((Rank)v, (Suit)i);
                }
            }
            return deck;
        }

        static void Shuffle(Card[] deck)
        {
            Random rnd = new Random();
            int nextIndex = 0;
            Card tmp;

            for (int i = 0; i < deck.Length; i++)
            {
                nextIndex = rnd.Next(deck.Length - 1);
                tmp = deck[i];
                deck[i] = deck[nextIndex];
                deck[nextIndex] = tmp;
            }
        }

        static Card[] AddCard(Card[] arr, Card card)
        {
            if (arr == null)
            {
                arr = new Card[1] { card };
                return arr;
            }
            else
            {
                Card[] tmp = new Card[arr.Length + 1];
                for (int i = 0; i < arr.Length; i++)
                    tmp[i] = arr[i];
                tmp[tmp.Length - 1] = card;
                return tmp;
            }
        }

        static void DislayCard(Card card)
        {

            switch (card.Suit)
            {
                case Suit.Hearts:

                case Suit.Diamonds:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case Suit.Clovers:
                case Suit.Pikes:
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
            }

            Console.Write(GetStringRank(card) + GetStringSuit(card) + '|');
            Console.ResetColor();
        }

        static string GetStringSuit(Card card)
        {
            char c = ' ';
            switch (card.Suit)
            {
                case Suit.Hearts:
                    c = '\u2665';
                    break;
                case Suit.Diamonds:
                    c = '\u2666';
                    break;
                case Suit.Clovers:
                    c = '\u2663';
                    break;
                case Suit.Pikes:
                    c = '\u2660';
                    break;
            }
            return c.ToString();
        }

        static string GetStringRank(Card card)
        {
            String rankString = string.Empty;
            switch (card.Rank)
            {
                case Rank.Ace:
                    rankString += "A";
                    break;
                case Rank.King:
                    rankString += "K";
                    break;
                case Rank.Lady:
                    rankString += "Q";
                    break;
                case Rank.Jack:
                    rankString += "J";
                    break;
                default:
                    rankString += ((int)card.Rank).ToString();
                    break;
            }
            return rankString;
        }



        static void DisplayHand(Card[] hand)
        {
            for (int i = 0; i < hand.Length; i++)
            {
                DislayCard(hand[i]);
            }
            Console.WriteLine("Score : {0}", GetHandScore(hand));
        }

        static int GetHandScore(Card[] hand)
        {
            int score = 0;
            for (int i = 0; i < hand.Length; i++)
            {
                score += (int)hand[i].Rank;
            }
            return score;
        }

        static void ReWriteHand(Player player)
        {
            int position = player.IsDealer ? 50 : 0;
            string role = player.IsDealer ? "Dealer" : "Player";
            Console.SetCursorPosition(position, 1);
            Console.WriteLine(role + ":");
            Console.SetCursorPosition(position, 2);
            DisplayHand(player.Hand);
            Console.SetCursorPosition(position, 3);
            if (player.IsBlackDjack)
                Console.WriteLine(role + " has BlackDjack!");
            else if (player.IsOverLoad)
                Console.WriteLine(role + " Overload");
            Console.SetCursorPosition(0, 3);
        }

        static Card[] InitializeGame()
        {
            Card[] deck = GenerateDeck();
            Shuffle(deck);
            return deck;
        }

        static bool IsBlackJack(Card[] hand)
        {
            return (GetHandScore(hand) == 21 || ((hand[0].Rank == Rank.Ace) && (hand[1].Rank == Rank.Ace)));
        }

        static int CardDealing(ref Player player, Card[] deck, ref int index)
        {
            if (IsBlackJack(player.Hand))
            {
                player.IsBlackDjack = true;
                ReWriteHand(player);
                return 21;
            }
            else
            {

                while (GetHandScore(player.Hand) <= 21)
                {
                    if (player.IsDealer)
                    {
                        if (GetHandScore(player.Hand) < 17)
                        {
                            player.Hand = AddCard(player.Hand, deck[index++]);
                        }
                        else
                        {
                            ReWriteHand(player);
                            break;
                        }
                    }
                    else
                    {
                        Console.SetCursorPosition(0, 4);
                        Console.WriteLine("Need more card? 1 - Yes 2 - No");
                        Console.Write("     ");
                        Console.SetCursorPosition(0, 5);
                        if (int.Parse(Console.ReadLine()) == 1)
                        {
                            player.Hand = AddCard(player.Hand, deck[index++]);
                            ReWriteHand(player);
                        }
                        else
                        {
                            ReWriteHand(player);
                            break;
                        }
                    }
                }

                if (GetHandScore(player.Hand) > 21)
                    player.IsOverLoad = true;

                ReWriteHand(player);
                return GetHandScore(player.Hand);
            }
        }

        static GameResult GetGameResult(Player player, Player dealer)
        {
            if (((player.IsBlackDjack == true) && (dealer.IsBlackDjack == true)) || (GetHandScore(player.Hand) == GetHandScore(dealer.Hand)))
                return GameResult.DeadHeat;

            if ((player.IsBlackDjack && !dealer.IsBlackDjack) || (!player.IsOverLoad && dealer.IsOverLoad))
                return GameResult.PlayerWin;
            else if ((dealer.IsBlackDjack && !player.IsBlackDjack) || (player.IsOverLoad && !dealer.IsOverLoad))
                return GameResult.DealerWin;

            if ((player.IsOverLoad) && (GetHandScore(player.Hand) < GetHandScore(dealer.Hand)))
                return GameResult.PlayerWin;
            else
                return GameResult.DealerWin;
        }

        static Card[] GetStartCards(Card[] deck, ref int index)
        {
            Card[] hand = null;
                hand = AddCard(hand, deck[index++]);
            hand = AddCard(hand, deck[index++]);
            return hand;

        }

        static void StartGame(ref Player player, ref Player dealer)
        {
            Console.Clear();
            player.IsBlackDjack = player.IsOverLoad = false;
            dealer.IsBlackDjack = dealer.IsOverLoad = false;
            int index = 0;
            Card[] deck = InitializeGame();
            player.Hand = GetStartCards(deck, ref index);
            dealer.Hand = GetStartCards(deck, ref index);
            ReWriteHand(player);
            ReWriteHand(dealer);
            Random rnd = new Random();

            ClearLine(0, 0);

            if (rnd.Next(0, 99) > 50)
            {
                Console.WriteLine("Player take cards first");
                int playerScore = CardDealing(ref player, deck, ref index);
                int dealerScore = CardDealing(ref dealer, deck, ref index);
            }
            else
            {
                Console.WriteLine("Dealer take cards first");
                int dealerScore = CardDealing(ref dealer, deck, ref index);
                int playerScore = CardDealing(ref player, deck, ref index);
            }

            ClearLine(0, 0);

            switch (GetGameResult(player, dealer))
            {
                case GameResult.DealerWin:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Dealer Win!");
                    Console.ForegroundColor = ConsoleColor.Black;
                    dealer.Wins++;
                    break;
                case GameResult.PlayerWin:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Player Win!");
                    Console.ForegroundColor = ConsoleColor.Black;
                    player.Wins++;
                    break;
                case GameResult.DeadHeat:
                    Console.WriteLine("Dead heat...");
                    break;
            }

            ClearLine(0, 4);
            ClearLine(0, 5);
        }

        static void ClearLine(int position, int line)
        {
            Console.SetCursorPosition(position, line);
            Console.Write("                                                  ");
            Console.SetCursorPosition(position, line);
        }

        static void WriteStats(Player player, Player dealer)
        {
            Console.Clear();
            Console.WriteLine("Player win {0} times", player.Wins);
            Console.WriteLine("Dealer win {0} times", dealer.Wins);
        }
    }
}
