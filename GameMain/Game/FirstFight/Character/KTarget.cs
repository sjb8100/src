using System;
using System.Collections.Generic;

public enum TARGET_TYPE
{
    ttInvalid = 0,

    ttNoTarget,
    ttCoordination,
    ttNpc,
    ttPlayer,
    ttTotal
};

public class KTargetData
{
    public TARGET_TYPE m_eTargetType = TARGET_TYPE.ttInvalid;
    public uint m_dwID = 0;

    public struct Data
    {
        public float fX;
        public float fY;
        public float fZ;
    };
    public Data m_Coordination;
};

public class KTarget : KTargetData
{

    public TARGET_TYPE GetTargetType()
    {
        return m_eTargetType;
    }

    public int SetTarget(KCharacter pCharacter)
    {
        int nResult = 0;

        if (pCharacter == null)
        {
            goto Exit0;
        }

        m_dwID = pCharacter.m_dwID;
        m_eTargetType = TARGET_TYPE.ttNpc;

        if (pCharacter.m_dwID == 1)
        {
            m_eTargetType = TARGET_TYPE.ttPlayer;
        }

        nResult = 1;
    Exit0:
        return nResult;

    }
    public int SetTarget(TARGET_TYPE eType, uint dwID)
    {
        int nResult = 0;

        switch (eType)
        {
            case TARGET_TYPE.ttNoTarget:
                break;
            case TARGET_TYPE.ttNpc:
                {
                    KNpc pNpc = null;

                    pNpc = FirstFightMgr.Instance().m_NpcSet.GetObj(dwID);
                    if (pNpc == null) { goto Exit0; }

                    break;
                }
            case TARGET_TYPE.ttPlayer:
                {
                    KPlayer pPlayer = null;


                    pPlayer = FirstFightMgr.Instance().m_PlayerSet.GetObj(dwID);
                    if (pPlayer == null) { goto Exit0; }
                    break;
                }

                break;
            default:
                //error
                break;
        };

        m_eTargetType = eType;
        m_dwID = dwID;

        nResult = 1;
    Exit0:
        if (nResult == 0)
        {
            m_eTargetType = TARGET_TYPE.ttNoTarget;
        }
        return nResult;
    }


    public int GetTarget(ref KCharacter ppCharacter)
    {
        int nResult = 0;

        switch (m_eTargetType)
        {
            case TARGET_TYPE.ttNpc:

                KNpc pNpc = FirstFightMgr.Instance().m_NpcSet.GetObj(m_dwID);
                if (pNpc == null) { goto Exit0; }
                ppCharacter = (KCharacter)pNpc;
                break;

            case TARGET_TYPE.ttPlayer:

                KPlayer pPlayer = FirstFightMgr.Instance().m_PlayerSet.GetObj(m_dwID);
                if (pPlayer == null) { goto Exit0; }
                ppCharacter = (KCharacter)pPlayer;
                break;

            default:
                goto Exit0;
        }

        nResult = 1;
    Exit0:
        return nResult;
    }

    public int GetTarget(ref float pfX, ref float pfZ)
    {
        int nResult = 0;

        switch (m_eTargetType)
        {
            case TARGET_TYPE.ttNpc:
                {
                    KNpc pNpc = FirstFightMgr.Instance().m_NpcSet.GetObj(m_dwID);
                    if (pNpc == null) { goto Exit0; }
                    pNpc.GetAbsoluteCoordinate(ref pfX, ref pfZ);

                    break;
                }
            case TARGET_TYPE.ttPlayer:
                {
                    KPlayer pPlayer = FirstFightMgr.Instance().m_PlayerSet.GetObj(m_dwID);
                    if (pPlayer == null) { goto Exit0; }
                    pPlayer.GetAbsoluteCoordinate(ref pfX, ref pfZ);

                }
                break;
            default:
                goto Exit0;
        }

        nResult = 1;
    Exit0:
        return nResult;
    }

}

