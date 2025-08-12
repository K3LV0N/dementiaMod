using dementiaMod.Content.Players;
using System;
using Terraria;
using Terraria.ModLoader;

namespace dementiaMod.Content.NPCs
{
    public class NPCEffects : GlobalNPC
    {
        private static readonly Random random = new();

        /// <summary>
        /// Messes with shop prices depending on dementia status
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="shopName"></param>
        /// <param name="items"></param>
        private void ChangeShopPrices(NPC npc, string shopName, Item[] items)
        {
            int dementiaTimer = Main.LocalPlayer.GetModPlayer<DementiaPlayer>().GetDementiaTimer;
            double itemPriceChangeChance = DementiaHelper.GetShopPriceChangeChance(dementiaTimer);

            for (int i = 0; i < items.Length; i++)
            {

                Item item = items[i];
                if (item != null)
                {
                    // remove on probability
                    bool shouldChangeItemCost = random.NextDouble() < itemPriceChangeChance;
                    if (shouldChangeItemCost)
                    {
                        // ranges from being 25% cheaper to 75% more expensive :)
                        double priceChangePercent = random.NextDouble() - 0.25;

                        item.shopCustomPrice ??= item.value;
                        item.shopCustomPrice += (int)(item.value * priceChangePercent);
                    }
                }
            }
        }


        public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
        {
            ChangeShopPrices(npc, shopName, items);
        }
    }
}
