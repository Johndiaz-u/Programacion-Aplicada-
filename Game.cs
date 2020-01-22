using System;
using System.IO;
using System.Text;

namespace Clase2_RandomGame
{
    public class Game
    {
        #region "Enums"

        public enum eGameState
        {
            Starting,
            Playing,
            Over
        }

        public enum AttemptResult
        {
            Guessed,
            Lower,
            Greater
        }

        #endregion

        #region "Attributes"

        const int MIN = 1, MAX = 1001;
        const string DEFAULTPATH = "scores.txt";
        public int SecretNumber { get; set; }
        public eGameState CurrentState { get; set; }
        public int LastTry { get; set; }

        public int Attemps;

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public TimeSpan TimeSpent
        {
            get { return EndTime.Subtract(StartTime); }
        }

        public string ScorePath { get; set; }

        #endregion

        #region "Behaviours"

        public void GameInit()
        {
            ScorePath = DEFAULTPATH;
            SecretNumber = GenerateNumber(MIN, MAX);
            CurrentState = eGameState.Starting;
            StartTime = new DateTime(DateTime.Now.Ticks);
            LastTry = 0;
            Attemps = 0;
        }

        private int GenerateNumber(int min, int max)
        {
            Random rnd = new Random();
            return rnd.Next(min, max);
        }

        public AttemptResult CheckIfGuessed()
        {
            Attemps++;
            if (LastTry == SecretNumber)
            {
                EndTime = new DateTime(DateTime.Now.Ticks);
                SaveState();
                return AttemptResult.Guessed;
            }
            else if (LastTry > SecretNumber)
            {
                return AttemptResult.Greater;
            }

            return AttemptResult.Lower;
        }

        public void SaveState()
        {
            using (FileStream fstream = File.Open(ScorePath, FileMode.Append))
            {
                var Buff = new UnicodeEncoding().GetBytes($"Tiempo: {TimeSpent.ToString()} - Intentos {Attemps}\n");
                fstream.Write(Buff, 0, Buff.Length);
            }
        }

        #endregion
    }
}