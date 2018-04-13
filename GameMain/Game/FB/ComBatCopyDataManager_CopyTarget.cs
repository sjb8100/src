using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;


//*******************************************************************************************
//	创建日期：	2017-8-17   11:14
//	文件名称：	ComBatCopyDataManager_CopyTarget,cs
//	创 建 人：	Lin Chuang Yuan
//	版权所有：	ZQB
//	说    明：	副本目标
//*******************************************************************************************

public class CopyTargetInfo
{
    public List<uint> waveIdList; //波数
    public uint waveType;         //波数类型
    public uint stepId;           //阶段
    public uint npcId;            //npcId ,五行阵用
    public uint mapId;            //mapId ,五行阵用
}


partial class ComBatCopyDataManager
{
    /// <summary>
    /// 副本目标
    /// </summary>
    List<CopyTargetInfo> m_listCopyTarget = new List<CopyTargetInfo>();

    public List<CopyTargetInfo> CopyTargetList
    {
        get
        {
            return m_listCopyTarget;
        }
    }

    /// <summary>
    /// 当前副本总共的阶段数
    /// </summary>
    List<uint> m_listCopyStepId = new List<uint>();

    /// <summary>
    /// 现在副本阶段(默认从第1阶段开始)
    /// </summary>
    uint m_nowCopyStepId = 1;

    public uint NowCopyStepId
    {
        get
        {
            return m_nowCopyStepId;
        }
    }

    /// <summary>
    /// 当前副本已经完成的波数
    /// </summary>
    List<uint> m_listCompletedWaveId = new List<uint>();

    public bool m_haveShowCopyGuide = false; //是否已经显示了副本引导

    public bool m_haveEnterWuXinZhen = false; //是否已经进入五行阵

    /// <summary>
    /// 世界聚宝开始倒计时
    /// </summary>
    float m_worldJuBaoCD;
    public float worldJuBaoCD
    {
        get
        {
            return m_worldJuBaoCD;
        }
    }


    /// <summary>
    /// 是否显示左侧副本目标
    /// </summary>
    public MissionAndTeamPanel.BtnStatus GetCopyLeftShowType()
    {
        if (false == m_bIsEnterCopy)
        {
            return MissionAndTeamPanel.BtnStatus.Mission;
        }

        CopyDataBase copyDb = GameTableManager.Instance.GetTableItem<CopyDataBase>(this.m_uEnterCopyID);
        if (copyDb != null)
        {
            if (copyDb.copyLeftShowType == 0)
            {
                return MissionAndTeamPanel.BtnStatus.Mission;
            }
            else if (copyDb.copyLeftShowType == 1)
            {
                return MissionAndTeamPanel.BtnStatus.CopyTarget;
            }
            else if (copyDb.copyLeftShowType == 2)
            {
                return MissionAndTeamPanel.BtnStatus.CopyBattleInfo;
            }
            else if (copyDb.copyLeftShowType == 3)
            {
                return MissionAndTeamPanel.BtnStatus.NvWa;
            }
            else if (copyDb.copyLeftShowType == 4)
            {
                return MissionAndTeamPanel.BtnStatus.Answer;
            }

            return MissionAndTeamPanel.BtnStatus.Mission;
        }
        else
        {
            return MissionAndTeamPanel.BtnStatus.Mission;
        }
    }

    /// <summary>
    /// 进入副本时 初始化波数数据
    /// </summary>
    void InitWaveIdListByCopyId()
    {
        m_listCopyTarget.Clear();

        List<CopyTargetDataBase> copyTargetDbList = GameTableManager.Instance.GetTableList<CopyTargetDataBase>();
        copyTargetDbList = copyTargetDbList.FindAll((data) => { return data.copyId == this.m_uEnterCopyID; });

        for (int i = 0; i < copyTargetDbList.Count; i++)
        {
            CopyTargetInfo info = m_listCopyTarget.Find((data) => { return data.waveType == copyTargetDbList[i].waveType; });

            //有同组的波数
            if (info != null)
            {
                if (false == info.waveIdList.Contains(copyTargetDbList[i].waveId))
                {
                    info.waveIdList.Add(copyTargetDbList[i].waveId);
                }
            }
            //无同组的波数
            else
            {
                CopyTargetInfo copyTargetInfo = new CopyTargetInfo();
                copyTargetInfo.stepId = copyTargetDbList[i].stepId;
                copyTargetInfo.waveType = copyTargetDbList[i].waveType;
                copyTargetInfo.npcId = copyTargetDbList[i].npcId;
                copyTargetInfo.mapId = copyTargetDbList[i].mapId;

                List<uint> waveIdList = new List<uint>();
                waveIdList.Add(copyTargetDbList[i].waveId);
                copyTargetInfo.waveIdList = waveIdList;

                m_listCopyTarget.Add(copyTargetInfo);
            }

            //当前副本总共的阶段数
            if (false == m_listCopyStepId.Exists((d) => { return d == copyTargetDbList[i].stepId; }))
            {
                m_listCopyStepId.Add(copyTargetDbList[i].stepId);
            }
        }
    }

    /// <summary>
    /// 获取当前阶段波数数据
    /// </summary>
    /// <returns></returns>
    public List<CopyTargetInfo> GetGetWaveIdListByStep()
    {

        return null;
    }



    /// <summary>
    /// 获取当前阶段
    /// </summary>
    /// <returns></returns>
    public uint GetCopyStep()
    {
        for (int i = 0; i < m_listCopyStepId.Count; i++)
        {
            // List<CopyTargetInfo>  targetList = m_listCopyTarget.FindAll((data)=>{return data.stepId ==  m_listCopyStepId[i];});
            List<CopyTargetInfo> targetList = GetCopyTargetListByStepId(m_listCopyStepId[i]);
            for (int j = 0; j < targetList.Count; j++)
            {
                //还有未完成的阶段
                if (false == IsCopyTargetCompleted(targetList[j]))
                {
                    return m_listCopyStepId[i];
                }
            }
        }

        //所有的波数都完成，返回最后一个阶段
        return m_listCopyStepId.Count > 0 ? m_listCopyStepId[m_listCopyStepId.Count - 1] : 1;
    }

    /// <summary>
    /// 由stepId获取副本目标
    /// </summary>
    List<CopyTargetInfo> tempTargetList = new List<CopyTargetInfo>();

    public List<CopyTargetInfo> GetCopyTargetListByStepId(uint stepId)
    {
        //用 Clear ，避免重复new  减少gc 
        tempTargetList.Clear();
        for (int i = 0; i < m_listCopyTarget.Count; i++)
        {
            if (stepId == m_listCopyTarget[i].stepId)
            {
                tempTargetList.Add(m_listCopyTarget[i]);
            }
        }

        return tempTargetList;
    }

    /// <summary>
    /// 获取副本目标
    /// </summary>
    /// <returns></returns>
    public List<CopyTargetInfo> GetCopyTargetList()
    {
        return GetCopyTargetListByStepId(this.m_nowCopyStepId);
    }


    /// <summary>
    /// 获取下一步
    /// </summary>
    /// <param name="stepId"></param>
    /// <returns></returns>
    public bool TryGetNextStep(out uint stepId)
    {
        uint nowStepId = GetCopyStep();

        if (nowStepId > m_nowCopyStepId)
        {
            m_nowCopyStepId = nowStepId;
            stepId = nowStepId;
            return true;
        }
        else
        {
            stepId = m_nowCopyStepId;
            return false;
        }
    }

    /// <summary>
    /// 跟新当前阶段（跟随波数）
    /// </summary>
    void UpdateCopyTargetStepByCopyWave()
    {
        this.m_nowCopyStepId = GetCopyStep();

        //uint nowStepId = GetCopyStep();

        //if (nowStepId > m_nowCopyStepId)
        //{
        //    m_nowCopyStepId = nowStepId;
        //    stepId = nowStepId;
        //    return true;
        //}
        //else
        //{
        //    stepId = m_nowCopyStepId;
        //    return false;
        //}
    }


    public void GetNowStep()
    {

    }

    /// <summary>
    /// 一组目标是否完成
    /// </summary>
    /// <param name="copyTargetInfo"></param>
    /// <returns></returns>
    public bool IsCopyTargetCompleted(CopyTargetInfo copyTargetInfo)
    {
        for (int i = 0; i < copyTargetInfo.waveIdList.Count; i++)
        {
            if (false == m_listCompletedWaveId.Contains(copyTargetInfo.waveIdList[i]))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 这一波目标是否完成
    /// </summary>
    /// <param name="waveId"></param>
    /// <returns></returns>
    public bool IsCopyTargetWaveCompleted(uint waveId)
    {
        return m_listCompletedWaveId.Contains(waveId);
    }

    public string GetCopyTargetName(CopyTargetInfo copyTargetInfo)
    {
        uint copyId = DataManager.Manager<ComBatCopyDataManager>().EnterCopyID;

        string name = string.Empty;

        for (int j = 0; j < copyTargetInfo.waveIdList.Count; j++)
        {
            uint waveId = copyTargetInfo.waveIdList[j];
            bool isCompleted = IsCopyTargetWaveCompleted(waveId);
            CopyTargetDataBase db = GameTableManager.Instance.GetTableItem<CopyTargetDataBase>(copyId, (int)waveId);
            if (db != null)
            {
                string tempName = string.Empty;
                if (isCompleted)
                {
                    tempName = ColorManager.GetColorString(ColorType.Green, db.completeWaveName);
                }
                else
                {
                    tempName = db.waveName;
                }

                if (copyTargetInfo.waveIdList.Count > 1 && j < copyTargetInfo.waveIdList.Count - 1)
                {
                    tempName = string.Format("{0}\n", tempName);
                }

                name = name + tempName;

                //五行阵副本
                if (IsWuXinZhenCopy())
                {
                    uint finishCount = GetWuXinZhenWaveFinishCount(db.waveListStr);
                    name = string.Format(name, finishCount);
                }

                //特殊处理演武场
                if (IsYanWuChangCopy(copyId))
                {
                    if (waveId == 37)
                    {
                        name = string.Format(db.waveName, this.LaskSkillWave);
                    }

                    if (waveId == 38)
                    {
                        string exp = ColorManager.GetColorString(ColorType.Yellow, this.m_copyRewardExp.ToString());
                        name = string.Format("{0}\n    {1}", db.waveName, exp);
                    }
                }

                //金银山副本
                if (IsGoldCopy(copyId))
                {
                    if (waveId == 2)
                    {
                        name = string.Format(db.waveName, this.LaskSkillWave);
                    }

                    if (waveId == 3)
                    {
                        string gold = ColorManager.GetColorString(ColorType.Yellow, this.m_gainGoldInCopy.ToString());
                        name = string.Format("{0}\n    {1}", db.waveName, gold);
                    }
                }

                //世界聚宝
                if (IsWorldJuBao(copyId))
                {
                    //排行
                    if (waveId == 1)
                    {
                        string rank = DataManager.Manager<JvBaoBossWorldManager>().MyBossDamRank.ToString();
                        name = string.Format(db.waveName, rank);
                    }

                    //鼓舞加成
                    if (waveId == 2)
                    {
                        string add = DataManager.Manager<JvBaoBossWorldManager>().CurInspirePileValue.ToString();
                        name = string.Format(db.waveName, add);
                    }

                    //累计伤害值
                    if (waveId == 3)
                    {
                        uint myBossDamage = DataManager.Manager<JvBaoBossWorldManager>().MyBossDam;
                        string damage = ColorManager.GetColorString(ColorType.Yellow, myBossDamage.ToString());
                        name = string.Format("{0}\n    {1}", db.waveName, damage);
                    }
                }
            }
        }

        return name;
    }

    /// <summary>
    /// 清副本目标数据
    /// </summary>
    void CleanCopyTargetData()
    {
        m_nowCopyStepId = 1;

        m_copyRewardExp = 0;

        m_listCopyTarget.Clear();
        m_listCopyStepId.Clear();
        m_listCompletedWaveId.Clear();
    }

    List<uint> tempWaveList = new List<uint>();
    uint GetWuXinZhenWaveFinishCount(string listStr)
    {
        tempWaveList.Clear();

        string[] arrStr = listStr.Split('_');
        for (int i = 0; i < arrStr.Length; i++)
        {
            uint waveId;
            if (uint.TryParse(arrStr[i], out waveId))
            {
                tempWaveList.Add(waveId);
            }
            else
            {
                Engine.Utility.Log.Error("--->>> 表格数据出错 ,没取到数据！");
            }
        }

        uint finishCount = 0;
        for (int i = 0; i < tempWaveList.Count; i++)
        {
            if (m_listCompletedWaveId.Contains(tempWaveList[i]))
            {
                finishCount++;
            }
        }

        return finishCount;
    }

}

