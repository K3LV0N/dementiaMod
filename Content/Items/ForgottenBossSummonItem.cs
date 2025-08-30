using dementiaMod.Content.Items.Global;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace dementiaMod.Content.Items
{
    public class ForgottenBossSummonItem : ModItem
    {

        HashSet<int> npcIDs;

        public ForgottenBossSummonItem()
        {
            npcIDs = [];
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.SuspiciousLookingEye);
            Item.width = 16;
            Item.height = 29;
            Item.scale = 1f;
            Item.value = Item.buyPrice(platinum: 0, gold: 0, silver: 0, copper: 0);
            Item.consumable = true;
        }

        public override void SaveData(TagCompound tag) {
            tag[DementiaMod.MOD_NAME + "npcIDs"] = npcIDs.ToList();
            tag[DementiaMod.MOD_NAME + "isConsumable"] = Item.consumable;
        }
        public override void LoadData(TagCompound tag)
        {
            npcIDs.Clear();
            npcIDs = [.. tag.GetList<int>(DementiaMod.MOD_NAME + "npcIDs")];
            Item.consumable = tag.GetBool(DementiaMod.MOD_NAME + "isConsumable");
        }
        public override void NetSend(BinaryWriter writer) {
            // Write how many NPC IDs we have
            writer.Write(npcIDs.Count);

            // Write each npcID
            foreach (int id in npcIDs)
            {
                writer.Write(id);
            }
        }
        public override void NetReceive(BinaryReader reader) {
            // Clear existing set just in case
            npcIDs.Clear();

            // Read how many NPC IDs were sent
            int count = reader.ReadInt32();

            // Read them back in
            for (int i = 0; i < count; i++)
            {
                npcIDs.Add(reader.ReadInt32());
            }
        }

        /// <summary>
        /// checks if any of the spawnable npcs are spawned. If they are, you cannot use this item
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public override bool CanUseItem(Player player)
        {
            foreach (int npcID in npcIDs)
            {
                if (NPC.AnyNPCs(npcID)) { return false; }
            }
            return true;
        }


        private static void SummonBoss(Player player, int npcID)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                // If the player using the item is the client
                // (explicitely excluded serverside here)
                SoundEngine.PlaySound(SoundID.Roar, player.position);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // If the player is not in multiplayer, spawn directly
                    NPC.SpawnOnPlayer(player.whoAmI, npcID);
                }
                else
                {
                    // If the player is in multiplayer, request a spawn
                    // This will only work if NPCID.Sets.MPAllowedEnemies[type] is true, which we set in MinionBossBody
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: npcID);
                }
            }
        }

        public override bool? UseItem(Player player)
        {
            foreach(var npcID in npcIDs)
            {
                SummonBoss(player, npcID);
            }

            return true;
        }

        /// <summary>
        /// Clone Clone by Gumi and Rin
        /// </summary>
        /// <param name="bossSummonID"></param>
        public void CloneClone(int itemID)
        {
            npcIDs = IsBossItem.GetBossIDs(itemID);
        }
    }
}
