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

            do
            {
                Console.WriteLine("1 - Game, 2 - View score 3 - Exit");
                command = int.Parse(Console.ReadLine());
                switch (command)
                {
                    case 1:
                        Console.Clear();
                        int index = 0;
                        player.Hand = null;
                        dealer.Hand = null;
                        player.IsBlackDjack = player.IsOverLoad = false;
                        dealer.IsBlackDjack = dealer.IsOverLoad = false;

                        Card[] deck = InitializeGame();
                        Random rnd = new Random();
                        Console.SetCursorPosition(0, 0);
                        Console.Write("                                                  ");
                        Console.SetCursorPosition(0, 0);
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

                        Console.SetCursorPosition(0, 0);
                        Console.Write("                                                  ");
                        Console.SetCursorPosition(0, 0);

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

                        Console.SetCursorPosition(0, 5);
                        break;

                    case 2:
                        Console.Clear();
                        Console.WriteLine("Player win {0} times", player.Wins);
                        Console.WriteLine("Dealer win {0} times", dealer.Wins);
                        break;

                    case 3:
                        Console.Clear();
                        Console.WriteLine("Player win {0} times", player.Wins);
                        Console.WriteLine("Dealer win {0} times", dealer.Wins);
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
            char c;
            String resultString = string.Empty;
            switch (card.Rank)
            {
                case Rank.Ace:
                    resultString += "A";
                    break;
                case Rank.King:
                    resultString += "K";
                    break;
                case Rank.Lady:
                    resultString += "Q";
                    break;
                case Rank.Jack:
                    resultString += "J";
                    break;
                default:
                    resultString += ((int)card.Rank).ToString();
                    break;
            }

            switch (card.Suit)
            {
                case Suit.Hearts:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    c = '\u2665';
                    resultString += c.ToString();
                    break;
                case Suit.Diamonds:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    c = '\u2666';
                    resultString += c.ToString();
                    break;
                case Suit.Clovers:
                    Console.ForegroundColor = ConsoleColor.Black;
                    c = '\u2663';
                    resultString += c.ToString();
                    break;
                case Suit.Pikes:
                    Console.ForegroundColor = ConsoleColor.Black;
                    c = '\u2660';
                    resultString += c.ToString();
                    break;
            }

            Console.Write(resultString + '|');
            Console.ResetColor();
        }

        static void DisplayHand(Card[] hand)
        {
            for (int i = 0; i < hand.Length; i++)
            {
                DislayCard(hand[i]);
            }
            Console.WriteLine("Score : {0}", GetHendScore(hand));
        }

        static int GetHendScore(Card[] hand)
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
            if (player.IsDealer != true)
            {
                Console.SetCursorPosition(0, 1);
                Console.WriteLine("Player:");
                DisplayHand(player.Hand);
                if (player.IsBlackDjack)
                    Console.WriteLine("Player has BlackDjack!");
                else if (player.IsOverLoad)
                    Console.WriteLine("Player Overload");
            }
            if (player.IsDealer)
            {
                Console.SetCursorPosition(50, 1);
                Console.WriteLine("Dealer:");
                Console.SetCursorPosition(50, 2);
                DisplayHand(player.Hand);
                Console.SetCursorPosition(50, 3);
                if (player.IsBlackDjack)
                    Console.WriteLine("Dealer has BlackDjack");
                else if (player.IsOverLoad)
                    Console.WriteLine("Dealer Overload");
            }

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
            return (GetHendScore(hand) == 21 || ((hand[0].Rank == Rank.Ace) && (hand[1].Rank == Rank.Ace)));
        }

        static int CardDealing(ref Player player, Card[] deck, ref int index)
        {
            player.Hand = AddCard(player.Hand, deck[index++]);
            player.Hand = AddCard(player.Hand, deck[index++]);
            ReWriteHand(player);
            if (IsBlackJack(player.Hand))
            {
                player.IsBlackDjack = true;
                ReWriteHand(player);
                return 21;
            }
            else
            {

                while (GetHendScore(player.Hand) <= 21)
                {
                    if (player.IsDealer)
                    {
                        if (GetHendScore(player.Hand) < 17)
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

                if (GetHendScore(player.Hand) > 21)
                    player.IsOverLoad = true;

                ReWriteHand(player);
                return GetHendScore(player.Hand);
            }
        }

        static GameResult GetGameResult(Player player, Player dealer)
        {
            if ((player.IsBlackDjack == true) && (dealer.IsBlackDjack == true))
                return GameResult.DeadHeat;

            if ((player.IsBlackDjack && !dealer.IsBlackDjack) || (!player.IsOverLoad && dealer.IsOverLoad))
                return GameResult.PlayerWin;
            else if ((dealer.IsBlackDjack && !player.IsBlackDjack) || (player.IsOverLoad && !dealer.IsOverLoad))
                return GameResult.DealerWin;

            if(player.IsOverLoad && dealer.IsOverLoad)
            {
                if (GetHendScore(player.Hand) < GetHendScore(dealer.Hand))
                    return GameResult.PlayerWin;
                else if (GetHendScore(player.Hand) > GetHendScore(dealer.Hand))
                    return GameResult.DealerWin;
                else
                    return GameResult.DeadHeat;
            }
            else
            {
                if (GetHendScore(player.Hand) > GetHendScore(dealer.Hand))
                    return GameResult.PlayerWin;
                else if (GetHendScore(player.Hand) < GetHendScore(dealer.Hand))
                    return GameResult.DealerWin;
                else
                    return GameResult.DeadHeat;
            }
        }
    }
}
