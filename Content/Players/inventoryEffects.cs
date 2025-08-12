using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace dementiaMod.Content.Players
{
    public class InventoryEffects : ModPlayer
    {
        Dictionary<Item, int> forgottenItems;
        List<Item> itemsToRemember;
        readonly Random random;

        private static int STARTING_INDEX = 10;
        private static int ENDING_INDEX = 49;

        InventoryEffects()
        {
            forgottenItems = [];
            itemsToRemember = [];
            random = new Random();
        }

        public override void SaveData(TagCompound tag)
        {

            var forgottenPairs = forgottenItems.Select(kvp => new TagCompound
            {
                ["item"] = ItemIO.Save(kvp.Key),
                ["time"] = kvp.Value
            }).ToList();
            tag["forgottenItems"] = forgottenPairs;

            tag["itemsToRemember"] = itemsToRemember.Select(ItemIO.Save).ToList();
            
        }

        public override void LoadData(TagCompound tag)
        {
            itemsToRemember = [];
            forgottenItems = [];


            itemsToRemember = tag.GetList<TagCompound>("itemsToRemember").Select(ItemIO.Load).ToList();

            forgottenItems = new Dictionary<Item, int>();
            if (tag.ContainsKey("forgottenItems"))
            {
                foreach (var pairTag in tag.GetList<TagCompound>("forgottenItems"))
                {
                    var item = ItemIO.Load(pairTag.GetCompound("item"));
                    var time = pairTag.GetInt("time");
                    forgottenItems[item] = time;
                }
            }
        }

        private bool AddForgottenItemsToInventory()
        {
            if (!Main.playerInventory)
            {
                // Use a for loop to safely remove items while iterating
                for (int j = itemsToRemember.Count - 1; j >= 0; j--)
                {
                    Item item = itemsToRemember[j];
                    for (int i = STARTING_INDEX; i < ENDING_INDEX; i++)
                    {
                        if (Player.inventory[i].IsAir)
                        {
                            Player.inventory[i] = item.Clone();
                            forgottenItems.Remove(item);
                            itemsToRemember.RemoveAt(j); // Remove after restoring
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void ForgetItems()
        {
            int dementiaTimer = Player.GetModPlayer<DementiaPlayer>().GetDementiaTimer;
            short ticks = Player.GetModPlayer<DementiaPlayer>().GetTicks;

            int secondsToRemember = DementiaHelper.GetTimeToRememberOneSecondForEachMinute(dementiaTimer);
            double itemRemovalProbability = DementiaHelper.GetMediumItemRemovalChance(dementiaTimer);

            // traverse indexes 10-49 to prevent removing hotbar items
            for (int i = STARTING_INDEX; i < ENDING_INDEX; i++)
            {
                Item item = Player.inventory[i];

                // remove on probability
                bool shouldRemove = random.NextDouble() < itemRemovalProbability;

                // only remove if it is an actual item
                shouldRemove = shouldRemove && !item.IsAir;

                // hide removals for when the player is looking
                shouldRemove = shouldRemove && !Main.playerInventory;

                if (shouldRemove)
                {
                    

                    forgottenItems.Add(item.Clone(), secondsToRemember);
                    Player.inventory[i].TurnToAir();
                }
            }

            foreach (var kvp in forgottenItems.ToList())
            {
                if (kvp.Value <= 0)
                {
                    if (!itemsToRemember.Contains(kvp.Key))
                        itemsToRemember.Add(kvp.Key);
                }
                else
                {
                    if (ticks % 60 == 0)
                    {
                        forgottenItems[kvp.Key] = kvp.Value - 1;
                    }    
                }
            }
            AddForgottenItemsToInventory();
        }

        
        public override void PostUpdate()
        {
            // Causes the player to forgor items in their inventory.
            ForgetItems();


        }
    }

}