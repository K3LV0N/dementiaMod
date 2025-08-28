using dementiaMod.Content.Items.Global;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace dementiaMod
{
	public class dementiaMod : Mod
	{
        public override object Call(params object[] args)
        {
            try
            {
                string message = args[0] as string;

                switch (message)
                {
                    case "AddBossSummon":
                        // args: "AddBossSummon", int itemID, int[] npcIDs
                        int itemID = (int)args[1];
                        int[] npcIDs = (int[])args[2];

                        IsBossItem.AddBossItemToDementiaPool(itemID, new HashSet<int>(npcIDs));
                        return true; // success

                    default:
                        Logger.Warn($"Unknown call message: {message}");
                        break;
                }
            }
            catch (Exception e)
            {
                Logger.Error("Error in dementiaMod.Call: " + e);
            }

            return null;
        }
	}
}
