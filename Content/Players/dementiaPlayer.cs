using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace dementiaMod.Content.Players
{
    public class DementiaPlayer : ModPlayer
    {
        /// <summary>
        /// Helps determine the chance for any dementia effects to occur
        /// </summary>
        private int dementiaTimer;

        private short ticks;

        DementiaPlayer()
        {
            dementiaTimer = 0;
            ticks = 0;
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
                ticks = 0;
            }
        }

        public int GetDementiaTimer => dementiaTimer;
        public short GetTicks => ticks;
    }
}