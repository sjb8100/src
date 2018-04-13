using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;
using Client;
using GameCmd;
partial class FirstRechargePanel 
{
    
    class RewardData
    {
        public uint id;
        public uint num;
    }
    List<RewardData> m_lst_Reward = null;
    List<FirstRechargeRewardDataBase> m_lst_Recharge = null;

    
    protected override void OnLoading()
    {
        base.OnLoading();
        m_lst_Recharge = GameTableManager.Instance.GetTableList<FirstRechargeRewardDataBase>();
    }

    protected override void OnShow(object data) 
    {
        base.OnShow(data);
//        RegisterGlobalEvent(true);
        m_ctor_itemRoot.RefreshCheck();
        m_ctor_itemRoot.Initialize<UIItemRewardGrid>(m_trans_UIItemRewardGrid.gameObject, OnUpdateRewardGrid, null);
        if (m_lst_Recharge != null)
        {
            if (m_lst_Recharge.Count > 0)
            {
                if (m_lst_Reward == null)
                {
                    m_lst_Reward = new List<RewardData>();
                }
                else 
                {
                    m_lst_Reward.Clear();
                }
                string str = m_lst_Recharge[0].rewards;
                string[] infos = str.Split(';');
                for (int i = 0; i < infos.Length;i++ )
                {
                    uint itemID = 0;
                    uint num = 0;
                    string[] ItemParams = infos[i].Split('_');
                    if (ItemParams.Length == 2)
                    {
                        if (uint.TryParse(ItemParams[0], out itemID) && uint.TryParse(ItemParams[1], out num))
                        {
                            RewardData msg = new RewardData()
                            {
                               id = itemID,
                               num = num,
                            };
                            m_lst_Reward.Add(msg);
                        }
                        else 
                        {
                            Engine.Utility.Log.Error("首充奖励表格第{0}个参数{1}无法解析出ID和数量--(请检查分隔符)！", i,ItemParams);
                        }
                    }
                    else 
                    {
                        Engine.Utility.Log.Error("首充奖励表格第{0}个参数长度不符合规则！", i);
                    }
                }

                m_ctor_itemRoot.CreateGrids(m_lst_Reward.Count);

                //ResetPosition();
            }
           
        }

        RefreshBtnStatus();
    }
    /// <summary>
    /// 已经领奖励  hadGotReward
    /// </summary>
    /// <param name="hadGotReward"></param>
    void RefreshBtnStatus()
    {
        //完成首充了
        bool afterFirstRechage = DataManager.Manager<Mall_HuangLingManager>().AlreadyFirstRecharge.Count > 0;
        bool hadGotReward = DataManager.Manager<ActivityManager>().HadGotFirstRechargeReward;
        m_btn_btn_recharge.gameObject.SetActive(!afterFirstRechage );
        m_btn_btn_getReward.gameObject.SetActive(afterFirstRechage );
        if (afterFirstRechage)
        {
            UIParticleWidget p = m_btn_btn_getReward.GetComponent<UIParticleWidget>();
            if (p == null)
            {
                p = m_btn_btn_getReward.gameObject.AddComponent<UIParticleWidget>();
                p.depth = 20;
            }
          
            if (hadGotReward)
            {
                if (p != null)
                {
                    p.ReleaseParticle();
                }
            }
            else
            {
                //播放特效
                if (p != null)
                {
                    p.SetDimensions(180, 54);
                    p.ReleaseParticle();
                    p.AddRoundParticle();
                }             
            }
        }
    }
    private void OnUpdateRewardGrid(UIGridBase grid, int index)
    {
        if (grid is UIItemRewardGrid)
        {
            UIItemRewardGrid data = grid as UIItemRewardGrid;
            if (index < m_lst_Reward.Count)
            {
                RewardData param = m_lst_Reward[index];
                if (param != null)
                {
                    data.SetGridData(param.id, param.num, false, false);
                    if(index < 3)
                    {
                        data.AddEffect(true, 52018);
                    }
                }
            }

        }
    }

//     private void RegisterGlobalEvent(bool register)
//     {
//         if (register)
//         {
//             Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.GOTFIRSTRECHARGEREWARD, GlobalEventHandler);
//         }
//         else 
//         {
//             Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.GOTFIRSTRECHARGEREWARD, GlobalEventHandler);
//         }
//     }
//     public void GlobalEventHandler(int eventid, object data)
//     {
//         switch (eventid)
//         {
//             case (int)Client.GameEventID.GOTFIRSTRECHARGEREWARD:
//                 {
//                     RefreshBtnStatus();
//                 }
//                 break;
//         }
//     }



    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_ctor_itemRoot != null)
        {
            m_ctor_itemRoot.Release(depthRelease);
        }
        if (m_lst_Reward != null)
        {
            m_lst_Reward.Clear();
        }
    }
    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();

    }


    protected override void OnHide()
    {
        base.OnHide();
//        RegisterGlobalEvent(false); 
        Release();
    }
    void onClick_Btn_close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_Btn_recharge_Btn(GameObject caster)
    {
        if (m_lst_Recharge != null)
          {
              if (m_lst_Recharge.Count > 0)
              {
                  uint jumpID = m_lst_Recharge[0].jumpID;
                  ItemManager.DoJump(jumpID);
              }
          }
    }
    void onClick_Btn_getReward_Btn(GameObject caster)
    {
          if (m_lst_Recharge != null)
          {
            if (m_lst_Recharge.Count > 0)
            {
               NetService.Instance.Send(new stFirstRechargePropertyUserCmd_CS() { id = m_lst_Recharge [0].ID});
            }
          }
        
    }


    void ResetPosition() 
    {
       List<UIItemRewardGrid> grids = m_ctor_itemRoot.GetGrids<UIItemRewardGrid>();
       if (grids != null )
        {
            for (int i = 0; i < grids.Count; i++)
            {
                if (i < 3)
                {
                    grids[i].transform.localPosition = new Vector3(100* i ,0, 0 );
                }
                else 
                {
                    grids[i].transform.localPosition = new Vector3(-50 +100* (i-3), -100, 0);
                }
            }
        }
    }
  
}
