using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//*******************************************************************************************
//	创建日期：	2017-8-16   16:33
//	文件名称：	MissionAndTeamPanel_NvWa,cs
//	创 建 人：	Lin Chuang Yuan
//	版权所有：	ZQB
//	说    明：	女娲
//*******************************************************************************************


partial class MissionAndTeamPanel
{
    NvWaManager NWManager
    {
        get
        {
            return DataManager.Manager<NvWaManager>();
        }
    }

    uint maxGuard = 0;


    /// <summary>
    /// 初始化女娲界面
    /// </summary>
    void InitNvWa()
    {
        InitLeftGuardGridContent();
    }

    /// <summary>
    /// 左侧招募grid
    /// </summary>
    void InitLeftGuardGridContent()
    {
        m_trans_RecruitContent.gameObject.SetActive(true);

        m_label_MebelNum.text = NWManager.NvWaCap.ToString();
        SetNvWaGuardNum();

        for (int i = 0; i < m_grid_GridRoot.transform.childCount; i++)
        {
            Transform transf = m_grid_GridRoot.transform.GetChild(i);
            transf.gameObject.SetActive(true);

            UINvWaGuardGrid grid = transf.gameObject.GetComponent<UINvWaGuardGrid>();
            if (grid == null)
            {
                grid = transf.gameObject.AddComponent<UINvWaGuardGrid>();
            }

            grid.SetGridData(NWManager.GuardDataList[i]);

            grid.RegisterUIEventDelegate(OnGuardGridUIEvent);

            /*
            //名字
            List<string> nameList;
            int index = 0;
            if (NWManager.GuardNameDic.TryGetValue(NWManager.GuardDataList[i].id, out nameList))
            {
                index = (int)NWManager.GuardDataList[i].lv - 1;
                if (index >= 0 && index < nameList.Count)
                {
                    //设置品质beijin
                    grid.SetIconBg(nameList[index]);
                }
            }

            //num
            grid.SetNum(NWManager.GuardDataList[i].num);

            //招募
            List<int> recruitCostList = NWManager.GetRecruitCostList(NWManager.GuardDataList[i].id);
            if (recruitCostList != null && recruitCostList.Count > 0)
            {
                string recruitLbl = string.Format("招募({0})", recruitCostList[0]);
                grid.SetRecruitBtnLbl(recruitLbl);
            }

            //升级
            List<int> lvUpCostList = NWManager.GetLvUpCostList(NWManager.GuardDataList[i].id);
            if (lvUpCostList != null && lvUpCostList.Count > 0)
            {
                string lvUpCostLbl = string.Format("升级({0})", lvUpCostList[0]);
                grid.SetLvUpBtnLbl(lvUpCostLbl);
            }
             */

        }

        SetGuardLvUp();
    }

    /// <summary>
    /// 点击
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnGuardGridUIEvent(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            UINvWaGuardGrid grid = data as UINvWaGuardGrid;
            if (grid != null)
            {
                uint btnIndex = (uint)param;

                if (btnIndex == 1)
                {
                    NWManager.ReqNvWaBuyGuard(grid.m_guardData.id);
                }

                if (btnIndex == 2)
                {
                    NWManager.ReqNvWaLvUpGuard(grid.m_guardData.id);
                }
            }
        }
    }

    /// <summary>
    /// 设置女娲徽章数
    /// </summary>
    void SetNvWaCap()
    {
        m_label_MebelNum.text = NWManager.NvWaCap.ToString();
    }

    /// <summary>
    /// 显示当前守卫数量
    /// </summary>
    void SetNvWaGuardNum()
    {
        uint guardSum = 0;

        List<NvWaManager.GuardData> guardDataList = NWManager.GuardDataList;
        for (int i = 0; i < guardDataList.Count; i++)
        {
            guardSum += guardDataList[i].num;
        }

        m_label_RecruitNum.text = string.Format("{0}/{1}", guardSum, maxGuard);  //剩余数/总共招募的数量
    }

    /// <summary>
    /// 升级后
    /// </summary>
    /// <param name="data"></param>
    void SetGuardLvUp()
    {
        UINvWaGuardGrid[] gridArr = m_grid_GridRoot.GetComponentsInChildren<UINvWaGuardGrid>();
        for (int i = 0; i < gridArr.Length; i++)
        {
            List<string> nameList = NWManager.GuardNameDic[gridArr[i].m_guardData.id];
            int index = (int)gridArr[i].m_guardData.lv - 1;

            List<int> recruitCostList = NWManager.GetRecruitCostList(gridArr[i].m_guardData.id);

            List<int> lvUpCostList = NWManager.GetLvUpCostList(gridArr[i].m_guardData.id);

            //name
            if (index >= 0 && index < nameList.Count)
            {
                gridArr[i].SetIconBg(nameList[index]);
            }

            //num
            gridArr[i].SetNum(NWManager.GuardDataList[i].num);

            //招募
            if (index >= 0 && index < recruitCostList.Count)
            {
                string s = string.Format("招募({0})", recruitCostList[index]);
                gridArr[i].SetRecruitBtnLbl(s);
            }

            //升级
            if (index >= 0)
            {
                if (index >= lvUpCostList.Count)
                {
                    string s = string.Format("满级");
                    gridArr[i].SetLvUpBtnLbl(s);
                    gridArr[i].SetLvUpBtnEnable(false);
                }
                else
                {
                    string s = string.Format("升级({0})", lvUpCostList[index]);
                    gridArr[i].SetLvUpBtnLbl(s);
                }
            }
        }

    }

}

