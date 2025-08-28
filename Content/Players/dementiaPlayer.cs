using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace dementiaMod.Content.Players
{
    public class DementiaPlayer : ModPlayer
    {
        private static int SHOP_COOLDOWN_IN_SECONDS = 30;
        /// <summary>
        /// Helps determine the chance for any dementia effects to occur
        /// </summary>
        private int dementiaTimer;

        private int timeSinceLastShopOpen;

        private int shopPenaltyCounter;

        private short ticks;

        DementiaPlayer()
        {
            dementiaTimer = 0;
            ticks = 0;
            timeSinceLastShopOpen = 9001;
            shopPenaltyCounter = 1;
        }


        public void OpenShop()
        {
            if (timeSinceLastShopOpen < SHOP_COOLDOWN_IN_SECONDS) 
            {
                shopPenaltyCounter++;
            } 
            else 
            {
                shopPenaltyCounter = 1;
            }
                timeSinceLastShopOpen = 0;
        }

        public override void SaveData(TagCompound tag)
        {
            tag["dementiaTimer"] = dementiaTimer;
            tag["ticks"] = ticks;
        }

        public override void LoadData(TagCompound tag)
        {
            dementiaTimer = tag.GetInt("dementiaTimer");
            ticks = tag.GetShort("ticks");
        }

        public override void UpdateDead()
        {
            // reset dementia timer
            dementiaTimer = 0;
            ticks = 0;
        }

        public override void PreUpdate()
        {
            ticks++;
        }

        public override void PostUpdate()
        {
            if (ticks >= 60)
            {
                dementiaTimer++;

                // 30 seconds
                if (timeSinceLastShopOpen < SHOP_COOLDOWN_IN_SECONDS)
                {
                    timeSinceLastShopOpen++;
                }
                ticks = 0;
            }
        }

        public int GetDementiaTimer => dementiaTimer;
        public short GetTicks => ticks;

        public int GetShopPenalty => shopPenaltyCounter;
    }
}