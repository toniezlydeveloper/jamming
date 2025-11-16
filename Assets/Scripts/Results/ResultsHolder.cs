using UnityEngine;

namespace Highscore
{
    public static class ResultsHolder
    {
        private const string HighscoreKey = "HS";
        
        public static bool TrySetHighScore(int score)
        {
            var highscore = PlayerPrefs.GetInt(HighscoreKey, 0);

            if (highscore < score)
            {
                PlayerPrefs.SetInt(HighscoreKey, score);
                return true;
            }

            return false;
        }

        public static bool TryGetHighScore(out int score)
        {
            if (PlayerPrefs.HasKey(HighscoreKey))
            {
                score = PlayerPrefs.GetInt(HighscoreKey);
                return true;
            }

            score = 0;
            return false;
        }
    }
}