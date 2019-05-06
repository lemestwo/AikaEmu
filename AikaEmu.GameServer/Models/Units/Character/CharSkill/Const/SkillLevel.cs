using System;

namespace AikaEmu.GameServer.Models.Units.Character.CharSkill.Const
{
    [Flags]
    public enum SkillLevel
    {
        Level1 = 1<<1,
        Level2 = 1<<2,
        Level3 = 1<<3,
        Level4 = 1<<4,
        Level5 = 1<<5,
        Level6 = 1<<6,
        Level7 = 1<<7,
        Level8 = 1<<8,
        Level9 = 1<<9,
        Level10 = 1<<10,
        Level11 = 1<<11,
        Level12 = 1<<12,
        Level13 = 1<<13,
        Level14 = 1<<14,
        Level15 = 1<<15,
        Level16 = 0,
    }
}