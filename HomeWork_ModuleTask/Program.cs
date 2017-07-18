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
        public Card(Rank rank, Suit suit, bool isFace)
        {
            Rank = rank;
            Suit = suit;
            IsFace = isFace;
        }

        public Rank Rank;
        public Suit Suit;
        public bool IsFace;

    }

    class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player();
            Player diler = new Player();

            int command = 0;
            do
            {
                Console.WriteLine("1 - Game, 3 - Exit");
                command = int.Parse(Console.ReadLine());
                switch (command)
                {
                    case 1:

                        Card[] deck = GenerateDeck();
                        Shuffle(deck);
                        int index = 0;

                        player.Hand = AddCard(player.Hand, deck[index++]);
                        player.Hand = AddCard(player.Hand, deck[index++]);
                        diler.Hand = AddCard(diler.Hand, deck[index++]);
                        diler.Hand = AddCard(diler.Hand, deck[index++]);

                        DisplayHand(player.Hand);
                        do {
                            Console.WriteLine("More card?");
                            if (Console.ReadLine().ToUpper() == "Y")
                            {
                                player.Hand = AddCard(player.Hand, deck[index++]);
                            }
                            else { break; }
                            DisplayHand(player.Hand);
                        } while (index < deck.Length);


                        Console.WriteLine("1");
                        break;
                    case 2:
                        Console.WriteLine("2");
                        break;
                    case 3:
                        Console.WriteLine("3");
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
                    deck[index] = new Card((Rank)v, (Suit)i, false); 
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
        }

    }
}
