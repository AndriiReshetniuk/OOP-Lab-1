
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ООП_ЛР1
{
    public class GameAccount
    {
        private long game_index_seed = 1;
        public string UserName {  get; set; }
        public long CurrentRaiting { 
            get
            {
                long totalRaiting = 1;
                foreach(var item in allRound)
                {
                    if (totalRaiting + item.Raiting < 1)
                    {
                        totalRaiting = 1;
                    }
                    else
                    {
                        totalRaiting += item.Raiting;
                    }
                }
                return totalRaiting;
            }
            set
            { 
            }
        }
        public long GamesCount 
        {
            get 
            {
                long totalGames = 0;
                for(int i = 0; i<allRound.Count; i++) 
                {
                    totalGames++;
                }
                return totalGames;
            } 
            set
            {

            }
        }
        public void WinGame(string oponentName, long Raiting)
        {
            if (Raiting < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Raiting), "You can not put a negative bet");
            }
            Round win = new Round(oponentName, Raiting, "Win", game_index_seed);
            game_index_seed++;
            allRound.Add(win);
        }
        public void LoseGame(string oponentName, long Raiting)
        {
            if (Raiting < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Raiting), "You can not put a negative bet");
            }
            Round lose = new Round(oponentName, -Raiting, "Lose", game_index_seed);
            game_index_seed++;
            allRound.Add(lose);
        }
       public string GetStats()
       {
            var stats = new StringBuilder();
            stats.AppendLine("Opponent\t\tresult\tRaiting\t\tGame index");
            foreach(var item in allRound)
            {
                stats.AppendLine($"{item.opponentName}\t\t{item.resultGame}\t{item.Raiting}\t\t{item.gameIndex}");
            }
            return stats.ToString();
        }
        public GameAccount(string UserName)
        {
            this.UserName = UserName;
            CurrentRaiting = 1;
            GamesCount = 0;
        }
        private List<Round>allRound = new List<Round>();
    }
    public class Game
    {
        private static long Game_number_seed = 1;
        public GameAccount BestPlayer { get; set; }
        public string Number { get; set; }
        public void startGame(GameAccount firstPlayer, GameAccount SecondPlayer)
        {
            Console.WriteLine("Hello players! You have to press on \"plus\" for win");        
            string reply = "-";
            long first_player_earned_raiting = 1;
            long second_player_earned_raiting = 1;
            do
            {
                long Raiting = 0;
                bool setRaiting = false;
                Console.WriteLine("Choose a rating rate:");
                do
                {
                    try
                    {
                        Raiting = Convert.ToInt64(Console.ReadLine());
                        if (Raiting < 0)
                        {
                            throw new ArgumentOutOfRangeException(nameof(Raiting), "You can not put a negative bet");
                        }
                        else
                        {
                            setRaiting = true;
                        }
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine("Exeption was caught");
                        Console.WriteLine(e.Message);
                    }
                } while (!setRaiting);

                string symbol = "-";
                do
                {
                    Console.WriteLine(firstPlayer.UserName + ", your move:");
                    symbol = Convert.ToString(Console.ReadLine());
                    if (symbol == "+")
                    {
                        Console.WriteLine($"Congratulations! {firstPlayer.UserName} is a winner");
                        firstPlayer.WinGame(SecondPlayer.UserName, Raiting);
                        SecondPlayer.LoseGame(firstPlayer.UserName, Raiting);
                        first_player_earned_raiting += Raiting;
                        if (second_player_earned_raiting - Raiting < 1)
                        {
                            second_player_earned_raiting = 1;
                        }
                        else
                        {
                            second_player_earned_raiting -= Raiting;
                        }
                        
                    }
                    else
                    {
                        Console.WriteLine(SecondPlayer.UserName + ", your move:");
                        symbol = Convert.ToString(Console.ReadLine());
                        if (symbol == "+")
                        {
                            Console.WriteLine($"Congratulations! {SecondPlayer.UserName} is a winner");
                            SecondPlayer.WinGame(firstPlayer.UserName, Raiting);
                            firstPlayer.LoseGame(SecondPlayer.UserName, Raiting);
                            second_player_earned_raiting += Raiting;
                            if (first_player_earned_raiting - Raiting < 1)
                            {
                                first_player_earned_raiting = 1;
                            }
                            else
                            {
                                first_player_earned_raiting -= Raiting;
                            }
                        }
                    }
                } while (symbol != "+");
                Console.WriteLine("Press \"plus\" if you want to continue:");
                reply = Convert.ToString(Console.ReadLine());
            } while (reply == "+");
            if(first_player_earned_raiting>second_player_earned_raiting)
            {
                BestPlayer = firstPlayer;
            }
            else if(second_player_earned_raiting>first_player_earned_raiting)
            {
                BestPlayer = SecondPlayer;
            }
            else
            {
                return;
            }
            Console.WriteLine("Game number "+Number+":");
            Console.WriteLine("The best player is "+ BestPlayer.UserName);
            if (first_player_earned_raiting > second_player_earned_raiting)
            {
                Console.WriteLine("Raiting in this game: " + first_player_earned_raiting);
            }
            else if (second_player_earned_raiting > first_player_earned_raiting)
            {
                Console.WriteLine("Raiting in this game: " + second_player_earned_raiting);
            }
            Console.WriteLine("Total raiting " + BestPlayer.CurrentRaiting + "\n\n");
        }
        public Game()
        {
            Number = Game_number_seed.ToString();
            Game_number_seed++;
        }
    }

    public class Round
    {
        public string opponentName { get; set; }
        public long Raiting { get; set;}
        public string resultGame { get; set; }
        public string gameIndex;
        public Round(string opponentName, long Raiting, string resultGame, long gameIndex)
        {
            this.opponentName = opponentName;
            this.Raiting = Raiting;
            this.resultGame = resultGame;
            this.gameIndex = gameIndex.ToString();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            GameAccount firstGladiator = new GameAccount("Hercules");
            GameAccount secondGladiator = new GameAccount("The Nemean Lion");
            Game firstGame = new Game();
            firstGame.startGame(firstGladiator, secondGladiator);
           
            Game secondGame = new Game();
            secondGame.startGame(firstGladiator, secondGladiator);
            Console.WriteLine("User name:" + firstGladiator.UserName);
            Console.WriteLine("Curent Raiting: " + firstGladiator.CurrentRaiting);
            Console.WriteLine("Games count: " + firstGladiator.GamesCount);
            Console.WriteLine("History:\n" + firstGladiator.GetStats());

            Console.WriteLine("User name: " + secondGladiator.UserName);
            Console.WriteLine("Curent Raiting: " + secondGladiator.CurrentRaiting);
            Console.WriteLine("Games count: " + secondGladiator.GamesCount);
            Console.WriteLine("History:\n" + secondGladiator.GetStats());
            Console.ReadKey();
        }
    }
}
