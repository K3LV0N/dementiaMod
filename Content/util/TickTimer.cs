using System;
using Terraria.ModLoader.IO;

namespace dementiaMod.Content.util
{
    public class TickTimer
    {
        private static readonly String saveTotalTicks = "TickTimer:totalTicks:";
        private static readonly String saveOriginalTicks = "TickTimer:originalTicks:";
        private long _totalTicks;
        private long _originalTicks;

        // --- Properties ---
        public long Hours => _totalTicks / 216000;          // 60 * 60 * 60
        public long Minutes => (_totalTicks / 3600) % 60;     // 60 * 60
        public long Seconds => (_totalTicks / 60) % 60;
        public long Ticks => _totalTicks % 60;

        // --- Constructors ---
        public TickTimer()
        {
            _totalTicks = 0;
            _originalTicks = _totalTicks;
        }

        public TickTimer(TickTimer originalTimer)
        {
            _totalTicks = originalTimer._totalTicks;
            _originalTicks = originalTimer._originalTicks;
        }

        public TickTimer(long totalTicks)
        {
            _totalTicks = Math.Max(0, totalTicks);
            _originalTicks = _totalTicks;
        }

        public TickTimer(long seconds, long ticks)
        {
            _totalTicks =
                (seconds * 60) +
                ticks;

            if (_totalTicks < 0) _totalTicks = 0; // clamp safety
            _originalTicks = _totalTicks;
        }

        public TickTimer(long minutes, long seconds, long ticks)
        {
            _totalTicks =
                (minutes * 3600) +
                (seconds * 60) +
                ticks;

            if (_totalTicks < 0) _totalTicks = 0; // clamp safety
            _originalTicks = _totalTicks;
        }

        public TickTimer(long hours, long minutes, long seconds, long ticks)
        {
            _totalTicks =
                (hours * 216000) +
                (minutes * 3600) +
                (seconds * 60) +
                ticks;

            if (_totalTicks < 0) _totalTicks = 0; // clamp safety
            _originalTicks = _totalTicks;
        }

        public TickTimer(TagCompound tag, String identifier)
        {
            _totalTicks = tag.GetLong(DementiaMod.MOD_NAME + identifier + saveTotalTicks);
            _originalTicks = tag.GetLong(DementiaMod.MOD_NAME + identifier + saveOriginalTicks);
        }

        public TickTimer(TagCompound tag, String identifier, TickTimer fallbackTimer)
        {
            if (tag.ContainsKey(DementiaMod.MOD_NAME + identifier + saveTotalTicks))
            {
                _totalTicks = tag.GetLong(DementiaMod.MOD_NAME + identifier + saveTotalTicks);
                _originalTicks = tag.GetLong(DementiaMod.MOD_NAME + identifier + saveOriginalTicks);
            }
            else
            {
                _totalTicks = fallbackTimer._totalTicks;
                _originalTicks = fallbackTimer._originalTicks;
            }
        }



        public void Reset()
        {
            _totalTicks = _originalTicks;
        }



        public static TickTimer operator ++(TickTimer t)
        {
            t._totalTicks++;
            return t;
        }

        public static TickTimer operator --(TickTimer t)
        {
            if (t._totalTicks > 0)
                t._totalTicks--;
            return t;
        }



        public TagCompound CreateTagCompound(String identifier)
        {
            TagCompound tag = new()
            {
                [DementiaMod.MOD_NAME + identifier + saveTotalTicks] = _totalTicks,
                [DementiaMod.MOD_NAME +  identifier + saveOriginalTicks] = _originalTicks
            };
            return tag;
        }

        public void SaveData(TagCompound tag, String identifier)
        {
            tag[DementiaMod.MOD_NAME + identifier + saveTotalTicks] = _totalTicks;
            tag[DementiaMod.MOD_NAME + identifier + saveOriginalTicks] = _originalTicks;
        }
 


        public long TotalTicks => _totalTicks;

        public bool IsDone => _totalTicks <= 0;



        public override bool Equals(object obj)
        {
            if (obj is TickTimer other)
                return _totalTicks == other._totalTicks;
            return false;
        }

        public override int GetHashCode()
        {
            return _totalTicks.GetHashCode();
        }
    }
}
