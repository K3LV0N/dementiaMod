using System;
namespace dementiaMod
{
    public static class DementiaHelper
    {
        public const double TEST_DEMENTIA_CHANCE = 0.0192;
        public const double TEST_SHOP_DEMENTIA_CHANCE = .9;
        private readonly static Random random = new();


        /// <summary>
        /// Around a 10% chance of triggering 1/59 every 10 minutes
        /// </summary>
        /// <param name="dementiaTimer"></param>
        /// <returns></returns>
        public static double GetShopPriceChangeChance(int dementiaTimer)
        {
            // 70% chance for item costs to change after ~4 hours of being alive in game.
            double chance = Math.Min(.70, .00005 * dementiaTimer);
            return chance;
        }

        /// <summary>
        /// Around a 10% chance of triggering 1/59 every 10 minutes
        /// </summary>
        /// <param name="dementiaTimer"></param>
        /// <returns></returns>
        public static double GetMediumItemRemovalChance(int dementiaTimer)
        {
            return Math.Min(1.0, 0.00000278 * Math.Exp(0.0005 * dementiaTimer));
        }

        /// <summary>
        /// Around a 5% chance of triggering 1/59 every 10 minutes
        /// </summary>
        /// <param name="dementiaTimer"></param>
        /// <returns></returns>
        public static double GetLowItemRemovalChance(int dementiaTimer)
        {
            return Math.Min(1.0, 0.00000139 * Math.Exp(0.0005 * dementiaTimer));
        }

        /// <summary>
        /// Returns dementiaTimer / 60 with some variance
        /// </summary>
        /// <param name="dementiaTimer"></param>
        /// <returns></returns>
        public static int GetTimeToRememberOneSecondForEachMinute(int dementiaTimer) {
            // one second of forgetting the item exists per every minute of dementia
            // also add some random factor to make it more unpredictable
            int secondsToRemember = (dementiaTimer / 120) * (int)(random.NextDouble() * 1.5);
            return secondsToRemember;
        }
    }
}
