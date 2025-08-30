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
            DementiaPlayer dementiaPlayer = Main.LocalPlayer.GetModPlayer<DementiaPlayer>();
            double itemPriceChangeChance = DementiaHelper.GetShopPriceChangeChance(dementiaPlayer);

            dementiaPlayer.OpenShop();
            int penalty = dementiaPlayer.ShopPenalty;

            for (int i = 0; i < items.Length; i++)
            {

                Item item = items[i];
                if (item != null)
                {
                    // remove on probability
                    // items have greater chance to increase in price when you spam shop
                    bool shouldChangeItemCost = random.NextDouble() < itemPriceChangeChance * penalty;
                    if (shouldChangeItemCost)
                    {
                        // ranges from being regular price to 50% more expensive
                        // prices get higher the more frequently you open the shop
                        double priceChangePercent = (random.NextDouble() * 0.5f) * penalty;

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
