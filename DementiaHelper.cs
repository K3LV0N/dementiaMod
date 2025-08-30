using dementiaMod.Content.Players;
using dementiaMod.Content.util;
using System;
namespace dementiaMod
{
    public static class DementiaHelper
    {
        // 70% chance for each item to have price increased
        private const double MAX_SHOP_PRICE_CHANGE_CHANCE = 0.70;

        // 0.5 % chance every second for every item at max dementia
        private const double MAX_INVENTORY_ITEM_REMOVAL_CHANCE = 0.005 / 60;

        // 2% chance every second for a boss summon item to morph
        private const double MAX_BOSS_SUMMON_MORPH_CHANCE = 0.0200 / 60;

        // 2 minutes for each item forgotten at max dementia
        private static readonly TickTimer MAX_INVENTORY_ITEM_FORGET_TIME = 
            new TickTimer(
                hours: 0,
                minutes: 2,
                seconds: 0,
                ticks: 0
                );

        public const double TEST_DEMENTIA_CHANCE = 0.01;
        public const double TEST_SHOP_DEMENTIA_CHANCE = 0.90;
        private readonly static Random random = new();



        public static double GetShopPriceChangeChance(DementiaPlayer player)
        {
            return MAX_SHOP_PRICE_CHANGE_CHANCE * player.DementiaPercent;
        }

        public static double GetBossSummonMorphChance(DementiaPlayer player)
        {
            return MAX_BOSS_SUMMON_MORPH_CHANCE * player.DementiaPercent;
        }

        public static double GetItemRemovalChance(DementiaPlayer player)
        {
            return MAX_INVENTORY_ITEM_REMOVAL_CHANCE * player.DementiaPercent;
        }

        public static TickTimer GetTimeToRememberItem(DementiaPlayer player) {

            // can have up to 50% less or 50% more time to remember item
            long ticksToRemember = (long)((MAX_INVENTORY_ITEM_FORGET_TIME.TotalTicks * player.DementiaPercent) * (1 + (random.NextDouble() - 0.5)));
            return new TickTimer(ticksToRemember);
        }
    }
}
