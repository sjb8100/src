using System;
using System.Collections.Generic;
public class KSKILL_BULLET
{
    public uint dwBulletID = 0;

    public KSkill pSkillPointer = null;     // 技能point

    public KCharacter pSkillSrc = null;     // 技能释放者point
    public uint dwSkillSrcID = 0;           // 技能释放者的ID
    public KTarget pTarget = null;
    public uint dwTargetID = 0;
    public int nEndFrame = 0;

    public KSKILL_BULLET pNext = null;

};