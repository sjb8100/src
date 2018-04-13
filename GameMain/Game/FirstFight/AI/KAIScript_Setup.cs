using System;
using System.Collections.Generic;

public class KAIScript_Setup
{
    public static void Setup(int nType, KAILogic pAI)
    {
        const int PRISONER_INIT = 100;
        const int PRISONER_IDLE = 20;
        const int PRISONER_FIGHT = 30;

        if (pAI == null)
        {
            return;
        }

        KAIAction pAIAction = null;
        KAIState pAIState = null;


        pAIState = pAI.NewState(PRISONER_INIT);
        pAIState.HandleEvent((int)KAI_EVENT.aevOnPrimaryTimer, 101);


        pAIAction = pAI.NewAction(101, (int)KAI_ACTION_KEY.eakRecordOriginPosition);
        pAIAction.SetBranch(28);


        pAIState = pAI.NewState(PRISONER_IDLE);
        pAIState.HandleEvent((int)KAI_EVENT.aevOnPrimaryTimer, 21);


        pAIAction = pAI.NewAction(21, (int)KAI_ACTION_KEY.eakSearchEnemy);
        pAIAction.SetParam(50);
        pAIAction.SetBranch(23, 22);


        pAIAction = pAI.NewAction(22, (int)KAI_ACTION_KEY.eakKeepOriginDirection);
        pAIAction.SetBranch(28);

        pAIAction = pAI.NewAction(23, (int)KAI_ACTION_KEY.eakAddTargetToThreatList);
        pAIAction.SetBranch(24);

        pAIAction = pAI.NewAction(24, (int)KAI_ACTION_KEY.eakRecordReturnPosition);
        pAIAction.SetBranch(31);


        pAIAction = pAI.NewAction(28, (int)KAI_ACTION_KEY.eakSetPrimaryTimer);
        pAIAction.SetParam(16);
        pAIAction.SetBranch(29);


        pAIAction = pAI.NewAction(29, (int)KAI_ACTION_KEY.eakSetState);
        pAIAction.SetParam(PRISONER_IDLE);


        pAIAction = pAI.NewAction(31, (int)KAI_ACTION_KEY.eakIsInFight);
        pAIAction.SetBranch(219, 41);
        
        
        // 选技能
        pAIAction = pAI.NewAction(219, (int)KAI_ACTION_KEY.eakNpcStandardSkillSelector);
        pAIAction.SetBranch(1120, 998);

        
        pAIAction = pAI.NewAction(1120, (int)KAI_ACTION_KEY.eakIsTargetExist);
        pAIAction.SetBranch(225, 28);

        pAIAction = pAI.NewAction(225, (int)KAI_ACTION_KEY.eakNpcKeepSkillCastRange);
        pAIAction.SetBranch(230, 38);


        pAIAction = pAI.NewAction(230, (int)KAI_ACTION_KEY.eakStand);
        pAIAction.SetBranch(231);


        pAIAction = pAI.NewAction(231, (int)KAI_ACTION_KEY.eakNpcCastSelectSkill);
        pAIAction.SetBranch(755, 38);

        // 动画播放时间就算了
        //pAIAction = pAI.NewAction(532..................................................)
        pAIAction = pAI.NewAction(755, (int)KAI_ACTION_KEY.eakSetPrimaryTimer);
        pAIAction.SetParam(8);
        pAIAction.SetBranch(39);


        pAIAction = pAI.NewAction(38, (int)KAI_ACTION_KEY.eakSetPrimaryTimer);
        pAIAction.SetParam(2);
        pAIAction.SetBranch(39);


        pAIAction = pAI.NewAction(39, (int)KAI_ACTION_KEY.eakSetState);
        pAIAction.SetParam(PRISONER_FIGHT);


        pAI.SetInitState(PRISONER_INIT);
    }
}

