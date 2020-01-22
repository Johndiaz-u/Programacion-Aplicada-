using System;
using System.Threading;

namespace Clase2_RandomGame
{
    internal class Program
    {
        public static Game CurrentGame;
        private static Thread inputThread;

        public static void Main(string[] args)
        {
            CurrentGame = new Game();
            CurrentGame.GameInit();
            inputThread = new Thread(Input);
            inputThread.Start();
            while (CurrentGame.CurrentState != Game.eGameState.Over)
            {
                switch (CurrentGame.CurrentState)
                {
                case Game.eGameState.Starting:
                    Console.WriteLine("Digite un valor entre 1 y 1000:");
                    CurrentGame.CurrentState = Game.eGameState.Playing;
                    
                    break;
                case Game.eGameState.Playing:
                    if (CurrentGame.LastTry == 0)
                    {
                        continue;
                    }
                    //Check if secret number is guessed...
                    switch (CurrentGame.CheckIfGuessed())
                    {
                    case Game.AttemptResult.Greater:
                        Console.WriteLine("El número secreto es menor.");
                        break;
                    case Game.AttemptResult.Lower:
                        Console.WriteLine("El número secreto es mayor.");
                        break;
                    default:
                        Console.WriteLine("¡HA ADIVINADO!");
                        CurrentGame.CurrentState = Game.eGameState.Over;
                        inputThread.Abort();
                        break;
                    }
                    
                    // And reset the last try.
                    CurrentGame.LastTry = 0;
                    break;
                default:
                    break;
                }
            }
            Console.WriteLine($"Ha adivinado en {CurrentGame.Attemps} intentos.");
            Console.WriteLine($"Time took to guess: {CurrentGame.TimeSpent}");
            Console.WriteLine("Gracias por jugar.");
            Console.ReadKey();
        }

        static void Input()
        {
            int _currentValue = 0;
            while (CurrentGame.CurrentState != Game.eGameState.Over)
            {
                _currentValue = Convert.ToInt32(Console.ReadLine());
                CurrentGame.LastTry = _currentValue;
            }
        }
    }
}