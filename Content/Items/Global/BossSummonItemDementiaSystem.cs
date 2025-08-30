using dementiaMod.Content.Players;
using System;
using Terraria;
using Terraria.ModLoader;

namespace dementiaMod.Content.Items.Global
{
    public class BossSummonItemDementiaSystem : GlobalItem
    {
        readonly static Random random = new();
        public override void UpdateInventory(Item item, Player player)
        {
            IsBossItem gobalItem = item.GetGlobalItem<IsBossItem>();
            if (gobalItem.IsBossSummon)
            {
                DementiaPlayer dementiaPlayer = player.GetModPlayer<DementiaPlayer>();
                double itemMorphProbability = DementiaHelper.GetBossSummonMorphChance(dementiaPlayer);

                // morph boss summon on probability
                bool shouldMorph = random.NextDouble() < itemMorphProbability;

                if (shouldMorph) {
                    int stack = item.stack;
                    bool consumable = item.consumable;
                    int itemType = item.type;
                    item.SetDefaults(ModContent.ItemType<ForgottenBossSummonItem>());
                    (item.ModItem as ForgottenBossSummonItem)?.CloneClone(itemType);
                    item.stack = stack;
                    item.consumable = consumable;
                }
            }  
        }
    }
}
