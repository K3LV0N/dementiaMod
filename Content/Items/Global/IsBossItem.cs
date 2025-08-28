using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace dementiaMod.Content.Items.Global
{
    public class IsBossItem : GlobalItem
    {
        private static Dictionary<int, HashSet<int>> bossSummonsItemToNPC = new()
        {
            // pre-hardmode
            { ItemID.SuspiciousLookingEye, [NPCID.EyeofCthulhu] },
            { ItemID.SlimeCrown, [NPCID.KingSlime] },
            { ItemID.WormFood, [NPCID.EaterofWorldsHead] },
            { ItemID.BloodySpine, [NPCID.BrainofCthulhu] },
            { ItemID.DeerThing, [NPCID.Deerclops] },
            { ItemID.Abeemination, [NPCID.QueenBee] },

            // hardmode
            { ItemID.QueenSlimeCrystal, [NPCID.QueenSlimeBoss] },
            { ItemID.MechanicalSkull, [NPCID.SkeletronPrime] },
            { ItemID.MechanicalEye, [NPCID.Retinazer, NPCID.Spazmatism]},
            { ItemID.MechanicalWorm, [NPCID.TheDestroyer] },
            { ItemID.CelestialSigil, [NPCID.MoonLordCore] },
        };

        public override bool InstancePerEntity => true;
        public int npcID;
        public bool IsBossSummon = false; // tag

        public override void SetDefaults(Item item)
        {
            if(bossSummonsItemToNPC.ContainsKey(item.type))
            {
                IsBossSummon = true;
            }
        }


        /// <summary>
        /// Returns the value from bossSummonsItemToNPC associated with the key <paramref name="itemID"/>
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public static HashSet<int> GetBossIDs(int itemID)
        {
            return bossSummonsItemToNPC[itemID];
        }

        /// <summary>
        /// Used to add any bosses and boss summon items to the pool of items that can be
        /// changed by the dementia mod.
        /// </summary>
        /// <param name="itemID">The item type of your boss summon.</param>
        /// <param name="npcID">The npc types of your boss summon</param>
        public static void AddBossItemToDementiaPool(int itemID, HashSet<int> npcID)
        {
            bossSummonsItemToNPC.Add(itemID, npcID);
        }

        /// <summary>
        /// Used to add any bosses and boss summon items to the pool of items that can be
        /// changed by the dementia mod.
        /// </summary>
        /// <param name="itemID">The item type of your boss summon.</param>
        /// <param name="npcID">The npc types of your boss summon</param>
        public static void AddBossItemToDementiaPool(int itemID, params int[] npcIDs)
        {
            bossSummonsItemToNPC[itemID] = [.. npcIDs];
        }
    }
}