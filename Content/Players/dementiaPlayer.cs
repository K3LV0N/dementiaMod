using dementiaMod.Content.util;
using Steamworks;
using System;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace dementiaMod.Content.Players
{
    public class DementiaPlayer : ModPlayer
    {

        private static readonly TickTimer DEMENTIA_MAX_TIME = 
            new TickTimer(
                hours: 10,
                minutes: 0,
                seconds: 0,
                ticks: 0
                );

        private static readonly TickTimer SHOP_START = 
            new TickTimer(
                seconds: 30,
                ticks: 0
                );

        /// <summary>
        /// Helps determine the chance for any dementia effects to occur
        /// </summary>
        private TickTimer dementiaTimer;

        private TickTimer shopCooldownTimer;

        private int shopPenaltyCounter;

        public DementiaPlayer()
        {
            dementiaTimer = new TickTimer();
            shopCooldownTimer = new TickTimer(SHOP_START);
            shopPenaltyCounter = 1;
        }


        public void OpenShop()
        {
            if (!shopCooldownTimer.IsDone) 
            {
                shopPenaltyCounter++;
            } 
            else 
            {
                shopPenaltyCounter = 1;
            }

            shopCooldownTimer.Reset();
        }

        public override void SaveData(TagCompound tag)
        {
            // Save the internal ticks as a long
            dementiaTimer.SaveData(tag, "dementia");
            shopCooldownTimer.SaveData(tag, "shopCooldown");
            tag[DementiaMod.MOD_NAME + "shopPenaltyCounter"] = shopPenaltyCounter;
        }

        public override void LoadData(TagCompound tag)
        {
            // Reconstruct TickTimers from saved total ticks
            dementiaTimer = new TickTimer(tag, "dementia");
            shopCooldownTimer = new TickTimer(
                tag: tag,
                identifier:"shopCooldown", 
                fallbackTimer: SHOP_START
                );
            shopPenaltyCounter = tag.GetInt(DementiaMod.MOD_NAME + "shopPenaltyCounter");
        }

        public override void UpdateDead()
        {
            dementiaTimer.Reset();
            shopCooldownTimer.Reset();
        }

        public override void PostUpdate()
        {
            dementiaTimer++;
            shopCooldownTimer--;
        }

        public TickTimer DementiaTimer => dementiaTimer;
        public double DementiaPercent => Math.Min((double)dementiaTimer.TotalTicks / DEMENTIA_MAX_TIME.TotalTicks, 1);
        public int ShopPenalty => shopPenaltyCounter;
    }
}