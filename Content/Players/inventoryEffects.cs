using dementiaMod.Content.util;
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
        Dictionary<Item, TickTimer> forgottenItems;
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
                [DementiaMod.MOD_NAME + "item:"] = ItemIO.Save(kvp.Key),
                [DementiaMod.MOD_NAME + "TickTimer:"] = kvp.Value.CreateTagCompound(kvp.Key.Name)
            }).ToList();
            tag[DementiaMod.MOD_NAME + "forgottenItems:"] = forgottenPairs;

            tag[DementiaMod.MOD_NAME + "itemsToRemember:"] = itemsToRemember.Select(ItemIO.Save).ToList();
            
        }

        public override void LoadData(TagCompound tag)
        {
            itemsToRemember = [];
            forgottenItems = [];

            itemsToRemember = tag.GetList<TagCompound>(DementiaMod.MOD_NAME +"itemsToRemember:").Select(ItemIO.Load).ToList();

            forgottenItems = new Dictionary<Item, TickTimer>();
            if (tag.ContainsKey(DementiaMod.MOD_NAME +"forgottenItems:"))
            {
                foreach (var pairTag in tag.GetList<TagCompound>(DementiaMod.MOD_NAME + "forgottenItems:"))
                {
                    var item = ItemIO.Load(pairTag.GetCompound(DementiaMod.MOD_NAME + "item:"));
                    var timeCompound = pairTag.GetCompound(DementiaMod.MOD_NAME + "TickTimer:");
                    forgottenItems[item] = new TickTimer(timeCompound, item.Name);
                }
            }
        }

        private void AddAllToRemember()
        {
            foreach(var key in forgottenItems.Keys)
            {
                itemsToRemember.Add(key);
            }
            forgottenItems.Clear();
        }
        private void ReturnAllItems()
        {
            AddAllToRemember();
            List<Item> returningItems = itemsToRemember;
            if (Player.difficulty == 0 || Player.difficulty == 3)
            {
                // Use a for loop to safely remove items while iterating
                
                for (int j = returningItems.Count - 1; j >= 0; j--)
                {
                    Item item = returningItems[j];
                    for (int i = STARTING_INDEX; i < ENDING_INDEX; i++)
                    {
                        if (Player.inventory[i].IsAir)
                        {
                            Player.inventory[i] = item.Clone();
                            returningItems.RemoveAt(j);
                            break;
                        }
                    }
                }
            } else
            {
                for (int j = returningItems.Count - 1; j >= 0; j--)
                {
                    Item item = returningItems[j];
                    Item.NewItem(Player.GetSource_Death(), Player.Center, item.type, item.stack, false, item.prefix);
                    returningItems.RemoveAt(j);
                    
                }
            }


            forgottenItems = [];
            itemsToRemember = [];
        }
        private void AddForgottenItemsToInventory()
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
                            break;
                        }
                    }
                }
            }
        }

        private void ForgetItems()
        {
            DementiaPlayer player = Player.GetModPlayer<DementiaPlayer>();
            double itemRemovalProbability = DementiaHelper.GetItemRemovalChance(player);

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
                    TickTimer timeUntilRemember = DementiaHelper.GetTimeToRememberItem(player);
                    forgottenItems.Add(item.Clone(), timeUntilRemember);
                    Player.inventory[i].TurnToAir();
                }
            }

            foreach (var kvp in forgottenItems.ToList())
            {
                if(kvp.Value.IsDone)
                {
                    if (!itemsToRemember.Contains(kvp.Key))
                    {
                        itemsToRemember.Add(kvp.Key);
                    }
                }
                else
                {
                    forgottenItems[kvp.Key]--;
                }
            }
            AddForgottenItemsToInventory();
        }

        public override void PreUpdate()
        {
            // If the player is about to die (health is 0 or less, but not yet marked as dead)

            if (Player.dead && forgottenItems.Count > 0)
            {
                ReturnAllItems();
            }
        }

        public override void PostUpdate()
        {
            // Normal dementia logic
            if (!Player.dead)
            {
                ForgetItems();
            } 
        }
    }
}