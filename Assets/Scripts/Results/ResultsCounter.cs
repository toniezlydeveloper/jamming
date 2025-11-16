using System.Collections.Generic;
using System.Linq;
using Core;

namespace Highscore
{
    public static class ResultsCounter
    {
        public static readonly List<PotionType> BrewedPotions = new();

        private static readonly Dictionary<PotionType, int> ValuesByPotion = new()
        {
            { PotionType.Goo, 100},
            { PotionType.Happiness, 700},
            { PotionType.Haste, 500},
            { PotionType.Love, 1000},
            { PotionType.Speed, 400},
        };

        public static bool TryGetHighScore(out int score, out int highscore)
        {
            score = BrewedPotions.Sum(p => ValuesByPotion[p]);

            if (ResultsHolder.TrySetHighScore(score))
            {
                highscore = score;
                return true;
            }

            highscore = 0;
            return false;
        }
    }
}