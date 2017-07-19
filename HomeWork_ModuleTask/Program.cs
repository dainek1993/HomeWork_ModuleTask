using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_ModuleTask
{
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
        public int Count;
        public int Wins;
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
            Player player = new Player();
            Player dealer = new Player();

            int command = 0;
            do
            {
                Console.WriteLine("1 - Game, 2 - View score 3 - Exit");
                command = int.Parse(Console.ReadLine());
                switch (command)
                {
                    case 1:

                        Card[] deck = GenerateDeck();
                        Shuffle(deck);
                        int index = 0;
                        player.Hand = AddCard(player.Hand, deck[index++]);
                        player.Hand = AddCard(player.Hand, deck[index++]);
                        dealer.Hand = AddCard(dealer.Hand, deck[index++]);
                        RewriteWindow(player, dealer);

                        if ((player.Hand[0].Rank == Rank.Ace && player.Hand[1].Rank == Rank.Ace) || GetHendScore(player.Hand) == 21)
                            Console.WriteLine("Black Jack");
                        else
                        {
                            do
                            {
                                Console.WriteLine("More card? 1 - yes, 2 - no");
                                if (int.Parse(Console.ReadLine()) == 1)
                                {
                                    player.Hand = AddCard(player.Hand, deck[index++]);
                                    Console.Clear();
                                    RewriteWindow(player, dealer);
                                    if (GetHendScore(player.Hand) == 21)
                                    {
                                        Console.WriteLine("BlackJack");
                                        break;
                                    }
                                    else if(GetHendScore(player.Hand) > 21)
                                    {
                                        Console.WriteLine("Overload");
                                        break;
                                    }
                                }
                                else { break; }                             
                            } while (index < deck.Length);
                        }

                        Console.WriteLine("Dealer:");
                        dealer.Hand = AddCard(dealer.Hand, deck[index++]);
                        RewriteWindow(player, dealer);

                        if ((player.Hand[0].Rank == Rank.Ace && player.Hand[1].Rank == Rank.Ace) || GetHendScore(dealer.Hand) == 21)
                            Console.WriteLine("Dealer BlackJack");
                        else
                        {
                            if (GetHendScore(dealer.Hand) < 21)
                            {
                                Random rnd = new Random();
                                while (GetHendScore(dealer.Hand) < 21)
                                {
                                    if (GetHendScore(player.Hand) > 21 && rnd.Next(0, 100) > 50)
                                        break;

                                    dealer.Hand = AddCard(dealer.Hand, deck[index++]);
                                    RewriteWindow(player, dealer);
                                    if (GetHendScore(dealer.Hand) == 21)
                                    {
                                        Console.WriteLine("Dealer BlackJack");
                                        break;
                                    }
                                }
                            }
                        }

                        if (GetHendScore(dealer.Hand) == GetHendScore(player.Hand))
                        {
                            Console.WriteLine("Dead hit");
                        }
                        else if (GetHendScore(dealer.Hand) > GetHendScore(player.Hand))
                        {
                            Console.WriteLine("Player WIN!!!!");
                            player.Wins++;
                        }
                        else
                        {
                            Console.WriteLine("Dealer win");
                            dealer.Wins++;
                        }

                        index = 0;
                        Shuffle(deck);
                        dealer.Hand = null;
                        player.Hand = null;
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

        static void RewriteWindow(Player player, Player dealer)
        {
            Console.Clear();
            Console.WriteLine("Player:");
            DisplayHand(player.Hand);
            if (dealer.Hand != null)
            {
                Console.SetCursorPosition((player.Hand.Length * 3) + 50, 0);
                Console.WriteLine("Dealer:");
                Console.SetCursorPosition((player.Hand.Length * 3) + 50, 1);
                DisplayHand(dealer.Hand);
            }

            Console.SetCursorPosition(0, 2);
        }
    }
}
