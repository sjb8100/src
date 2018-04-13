//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//partial class CityWarMessagePanel
//{


//    public class CityWarMessageData
//    {
//        public string step;
//        public uint btnIndex;
//        public uint taskId;
//    }


//    CityWarMessageData m_data = null;

//    //奖励的item列表
//    List<uint> m_lstItem = null;

//    //报名的氏族信息
//    List<string> m_lstClan;

//    //氏族列表
//    UIGridCreatorBase m_clanNameGridCreator;

//    //奖励物品
//    UIGridCreatorBase m_rewardItemGridCreator;

//    protected override void OnLoading()
//    {
//        base.OnLoading();

//        InitWidget();
//    }

//    /// <summary>
//    /// 初始化
//    /// </summary>
//    void InitWidget()
//    {
//        if (m_scrollview_signUpClanScrollView != null)
//        {
//            m_clanNameGridCreator = m_scrollview_signUpClanScrollView.gameObject.GetComponent<UIGridCreatorBase>();
//            if (m_clanNameGridCreator == null)
//            {
//                m_clanNameGridCreator = m_scrollview_signUpClanScrollView.gameObject.AddComponent<UIGridCreatorBase>();
//            }
//            UnityEngine.GameObject obj = m_trans_clanName.gameObject;
//            m_clanNameGridCreator.gridContentOffset = new UnityEngine.Vector2(0, 52);
//            m_clanNameGridCreator.arrageMent = UIGridCreatorBase.Arrangement.Vertical;
//            m_clanNameGridCreator.gridWidth = 398;
//            m_clanNameGridCreator.gridHeight = 30;

//            m_clanNameGridCreator.RefreshCheck();
//            m_clanNameGridCreator.Initialize<UICityWarSignUpClanGrid>(obj, OnGridDataUpdate, OnGridUIEvent);

//        }


//        //奖励
//        if (m_scrollview_rewardItemScrollView != null)
//        {
//            m_rewardItemGridCreator = m_scrollview_rewardItemScrollView.gameObject.GetComponent<UIGridCreatorBase>();
//            if (m_rewardItemGridCreator == null)
//            {
//                m_rewardItemGridCreator = m_scrollview_rewardItemScrollView.gameObject.AddComponent<UIGridCreatorBase>();
//            }

//            //UnityEngine.GameObject obj = UIManager.GetResGameObj(GridID.Uiitemshow) as UnityEngine.GameObject;
//            m_rewardItemGridCreator.gridContentOffset = new UnityEngine.Vector2(-120, 0);
//            m_rewardItemGridCreator.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
//            m_rewardItemGridCreator.gridWidth = 90;
//            m_rewardItemGridCreator.gridHeight = 120;

//            m_rewardItemGridCreator.RefreshCheck();
//            m_rewardItemGridCreator.Initialize<UIItemShow>((uint)GridID.Uiitemshow, UIManager.OnObjsCreate, UIManager.OnObjsRelease, OnGridDataUpdate, OnGridUIEvent);
//        }
//    }

//    protected override void OnPrepareShow(object data)
//    {
//        base.OnPrepareShow(data);


//    }
//    protected override void OnShow(object data)
//    {
//        base.OnShow(data);
//        this.m_data = data as CityWarMessageData;
//        if (this.m_data.btnIndex == 1)
//        {
//            //报名
//            SignUp();
//        }

//        if (this.m_data.btnIndex == 3)
//        {
//            //介绍
//            Introduction();
//        }
//    }

//    protected override void OnHide()
//    {
//        base.OnHide();

//        Release();
//    }

//    protected override void OnPanelBaseDestory()
//    {
//        base.OnPanelBaseDestory();
//    }

//    public override void Release(bool depthRelease = true)
//    {
//        base.Release(depthRelease);

//        if (m_rewardItemGridCreator != null)
//        {
//            m_rewardItemGridCreator.Release(depthRelease);
//        }
//    }


//    /// <summary>
//    /// 报名
//    /// </summary>
//    void SignUp()
//    {
//        m_trans_signUpRoot.gameObject.SetActive(true);
//        m_trans_IntroductionRoot.gameObject.SetActive(false);
//        m_trans_bottomRoot.gameObject.SetActive(true);

//        m_label_title_desc.text = "报名条件";

//        m_lstClan = DataManager.Manager<CityWarManager>().CityWarSignUpClanList;
//        m_clanNameGridCreator.CreateGrids(m_lstClan != null ? m_lstClan.Count : 0);
//    }


//    /// <summary>
//    /// 介绍
//    /// </summary>
//    void Introduction()
//    {
//        m_trans_signUpRoot.gameObject.SetActive(false);
//        m_trans_IntroductionRoot.gameObject.SetActive(true);
//        m_trans_bottomRoot.gameObject.SetActive(false);

//        m_label_title_desc.text = "城战介绍";

//        m_lstItem = GameTableManager.Instance.GetGlobalConfigList<uint>("CityWarReward");
//        m_rewardItemGridCreator.CreateGrids(m_lstItem != null ? m_lstItem.Count : 0);
//    }


//    private void OnGridDataUpdate(UIGridBase gridData, int index)
//    {
//        if (gridData is UICityWarSignUpClanGrid)
//        {
//            if (m_lstClan != null && m_lstClan.Count > index)
//            {
//                UICityWarSignUpClanGrid grid = gridData as UICityWarSignUpClanGrid;
//                if (grid == null)
//                {
//                    return;
//                }

//                grid.SetGridData(m_lstClan[index]);
//                grid.SetName(m_lstClan[index]);
//            }
//        }

//        if (gridData is UIItemShow)
//        {
//            if (m_lstItem != null && m_lstItem.Count > index)
//            {
//                UIItemShow grid = gridData as UIItemShow;

//                if (grid == null)
//                {
//                    return;
//                }

//                MissionRewardItemInfo info = new MissionRewardItemInfo { itemBaseId = m_lstItem[index], num = 0 };

//                grid.SetGridData(info);
//            }


//        }

//    }

//    private void OnGridUIEvent(UIEventType eventType, object data, object param)
//    {

//    }

//    void onClick_Btn_bottom_Btn(GameObject caster)
//    {


//        NetService.Instance.Send(new GameCmd.stDialogSelectScriptUserCmd_C()
//        {
//            step = m_data.step,
//            dwChose = m_data.btnIndex,
//        });

//        HideSelf();
//    }

//    void onClick_Close_Btn(GameObject caster)
//    {
//        HideSelf();
//    }

//    public override bool OnMsg(UIMsgID msgid, object param)
//    {
//        //报名的氏族
//        if (msgid == UIMsgID.eCityWarSignUpClan)
//        {
//            SignUp();
//        }

//        return true;
//    }
//}

