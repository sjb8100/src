using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Common;
using Client;
using Engine.Utility;
using GameCmd;

public partial class ChooseRolePanel : UIPanelBase
{
    readonly int MAX_ROLE_NUM = 4;

    uint m_nSelectUID = 0;
    Transform m_model_node;
    GameObject m_scene_obj;

    Client.Avater m_Avater = null;
    private int m_iCurIndex = -1;
    private int m_iPreIndex = -1;
    private List<GameCmd.SelectUserInfo> lstRoleList = null;
    private List<UISelectRoleGrid> m_lstGrids = null;
    private TweenAlpha m_rightDesTA = null;
    private Dictionary<GameCmd.enumProfession, GameObject> m_dicProDes = null;
    float rotateY = 0f;

    private enumProfession[] m_professionList = new[]
	{ 
		enumProfession.Profession_Soldier, 
		enumProfession.Profession_Spy, 
		enumProfession.Profession_Freeman, 
		enumProfession.Profession_Doctor,
	};
    //private Animator m_camera_anim = null;
    private Transform m_camera_node = null;
    protected override void OnLoading()
    {
        base.OnLoading();
        UIEventListener.Get(m_widget_Container.gameObject).onDrag = OnDragModel;
        InitCharacterGrid();
    }

    private void InitCharacterGrid()
    {
        m_lstGrids = new List<UISelectRoleGrid>();
        m_rightDesTA = m_widget_DesWidget.GetComponent<TweenAlpha>();
        m_dicProDes = new Dictionary<enumProfession, GameObject>();
        
        if (null != m_trans_role_list && null != m_trans_UISelectRoleGrid)
        {
            Transform ts = null;
            UISelectRoleGrid roleGrid = null;
            GameObject cloneObj = null;

            lstRoleList = DataManager.Manager<LoginDataManager>().RoleList;
            Transform tempTs = null;
            for (int i = 0, max = m_professionList.Length; i < max; i++)
            {
                ts = m_trans_role_list.Find(m_professionList[i].ToString());
                if (null != ts)
                {
                    tempTs = GameObject.Instantiate(m_trans_UISelectRoleGrid);// UIManager.GetObj(GridID.Uiselectrolegrid);
                    if (null == tempTs)
                        continue;
                    Util.AddChildToTarget(ts, tempTs);
                    cloneObj = tempTs.gameObject;
                    if (null != cloneObj)
                    {
                        roleGrid = cloneObj.GetComponent<UISelectRoleGrid>();
                        if (null == roleGrid)
                        {
                            roleGrid = cloneObj.AddComponent<UISelectRoleGrid>();
                        }
                    }
                    //roleGrid.SetData(i,GetRoleInfoByIndex(i));
                    roleGrid.RegisterUIEventDelegate(OnSelectRoleGridClick);
                    roleGrid.SetGridData(i);
                    m_lstGrids.Add(roleGrid);
                }

                ts = m_widget_DesWidget.cachedTransform.Find(m_professionList[i].ToString());
                if (null != ts)
                {
                    m_dicProDes.Add(m_professionList[i], ts.gameObject);
                }
            }
        }
        
    }

    void OnSelectRoleGridClick(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            if (null != data && data is UISelectRoleGrid)
            {
                UISelectRoleGrid cGrid = data as UISelectRoleGrid;
                if (cGrid.Empty)
                {
                    GoToCreateRole();
                }else if (cGrid.Index != m_iCurIndex)
                {
                    OnSelectRole(cGrid.Index);
                }
            }
        }
    }

    private void RefreshCharacterGrid()
    {
        for(int i = 0,max = m_lstGrids.Count;i < max;i++)
        {
            m_lstGrids[i].SetData(i, GetRoleInfoByIndex(i));
        }

        GameObject desOjb = null;
        for (int i = 0, max = m_professionList.Length; i < max; i++)
        {
            desOjb = GetDesObjByPro(m_professionList[i]);
            if (null == desOjb || !desOjb.gameObject.activeSelf)
            {
                continue;
            }
            desOjb.SetActive(false);
        }
    }

    void OnDragModel(GameObject go, UnityEngine.Vector2 delta)
    {
        if (m_Avater != null)
        {
            rotateY = m_Avater.RenderObj.GetNode().GetLocalRotate().eulerAngles.y- 0.5f * delta.x;
            Quaternion rotate = new Quaternion();
            rotate.eulerAngles = new Vector3(0, rotateY, 0);
            m_Avater.RenderObj.GetNode().SetLocalRotate(rotate);
        }
    }

    protected override void OnShow(object data)
    {
        mbAnimEnable = true;
        RefreshCharacterGrid();
        //创建角色列表
        m_nSelectUID = 0;
        LoadRoleModel();

        //Engine.IAudio audio = Engine.RareEngine.Instance().GetAudio();
        //if (audio != null)
        //{
        //    audio.StopMusic();
        //}
        //table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(46001);
        //if (resDB == null)
        //{
        //    Engine.Utility.Log.Error("找不到选择角色的Mp3资源");
        //}

        //if (audio != null && resDB != null)
        //{
        //    audio.PlayMusic(resDB.strPath);
        //}       
        OnSelectRole(0);
    }

    private void ResetSelectRole()
    {
        UISelectRoleGrid roleGrid = GetRoleGridByIndex(m_iCurIndex);
        if (null != roleGrid)
        {
            roleGrid.SetSelect(false, false);
        }
        m_iCurIndex = -1;
        m_iPreIndex = -1;
    }
    /// <summary>
    /// 根据索引获取角色信息
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private GameCmd.SelectUserInfo GetRoleInfoByIndex(int index)
    {
        lstRoleList = DataManager.Manager<LoginDataManager>().RoleList;
        if (index >= 0 && null != lstRoleList && lstRoleList.Count > index)
        {
            return lstRoleList[index];
        }
        return null;
    }

    private UISelectRoleGrid GetRoleGridByIndex(int index)
    {
        if (index >= 0 && null != m_lstGrids && m_lstGrids.Count > index)
        {
            return m_lstGrids[index];
        }
        return null;
    }

    private GameObject GetDesObjByPro(GameCmd.enumProfession pro)
    {
        if (m_dicProDes.ContainsKey(pro))
        {
            return m_dicProDes[pro];
        }
        return null;
    }

    void OnSelectRole(int index)
    {
//         if (m_iCurIndex == index)
//         {
//             return;
//         }
        GameCmd.SelectUserInfo userInfo = GetRoleInfoByIndex(index);

        bool havePre = m_iCurIndex != -1;
        if (havePre)
        {
            if (null != m_rightDesTA)
            {
                m_rightDesTA.ResetToBeginning();
                m_rightDesTA.Play(true);
            }
            GameCmd.SelectUserInfo pre = GetRoleInfoByIndex(m_iCurIndex);
            if (pre != null)
            {
                GetRoleGridByIndex(m_iCurIndex).SetSelect(false);
                GetDesObjByPro(pre.type).SetActive(false);
            }
          
        }
        GetRoleGridByIndex(index).SetSelect(true, havePre);
        GetDesObjByPro(userInfo.type).SetActive(true);
        m_iPreIndex = m_iCurIndex;
        m_iCurIndex = index;
        ChooseRole(userInfo);
    }

    public static string GetSpriteName(GameCmd.enumProfession type)
    {
        switch (type)
        {
            case enumProfession.Profession_Blast:
                break;
            case enumProfession.Profession_Doctor:
                return "touxiang_zhu_wushi";
            case enumProfession.Profession_Freeman:
                return "touxiang_zhu_fashi";
            case enumProfession.Profession_Gunman:
                break;
            case enumProfession.Profession_Max:
                break;
            case enumProfession.Profession_None:
                break;
            case enumProfession.Profession_Soldier:
                return "touxiang_zhu_zhanshi";

            case enumProfession.Profession_Spy:
                return "touxiang_zhu_huanshi";

            default:
                break;
        }
        return "";
    }
    protected override void OnHide()
    {
        ResetSelectRole();

        if (m_scene_obj)
        {
            if (m_Avater!=null)
            {
                m_Avater.Destroy();
                m_Avater = null;
            }
        }
        Engine.IAudio audio = Engine.RareEngine.Instance().GetAudio();
        if (audio != null)
        {
            audio.StopMusic();
        }
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_lstGrids)
        {
            UISelectRoleGrid tempGrid = null;
            uint resID = (uint)GridID.Uiselectrolegrid;
            for (int i = 0; i < m_lstGrids.Count; )
            {
                tempGrid = m_lstGrids[i];
                if (null == tempGrid)
                {
                    i++;
                    continue;
                }
                m_lstGrids.Remove(tempGrid);
                UIManager.ReleaseObjs(resID, tempGrid.CacheTransform);
            }
            m_lstGrids.Clear();
        }
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }

    void onClick_Enter_Btn(GameObject caster)
    {
        SelectUserInfo cur = GetRoleInfoByIndex(m_iCurIndex);

        if (!Application.isEditor)
        {
            uint mapid = cur.mapid & 0xFF;
            if (!KHttpDown.Instance().SceneFileExists(mapid))
            {
                //打开下载界面
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.DownloadPanel);
                return;
            }
        }

        if (null != cur)
        {
            ServerStatusParam status = DataManager.Manager<LoginDataManager>().CurServerState;
            if (status.state == ServerLimit.ServerLimit_Free)
            {
                Log.Info("enter game, role index: " + cur.id);

                if (cur.del ==1)
                {
                    TextManager Tmger = DataManager.Manager<TextManager>();
                    string des = Tmger.GetLocalFormatText(LocalTextType.RecoverRole_Tips_5, cur.name);
                    Action yes = delegate
                    {
                        ReqEnter(cur.id);
                    };
                    TipsManager.Instance.ShowTipWindow(TipWindowType.YesNO, des, yes, null, title: Tmger.GetLocalText(LocalTextType.Local_TXT_Tips), okstr: Tmger.GetLocalText(LocalTextType.Local_TXT_Confirm), cancleStr: Tmger.GetLocalText(LocalTextType.Local_TXT_Cancel));
                }
                else
                {
                    ReqEnter(cur.id);               
                }
            }
            else
            {
                TipsManager.Instance.ShowTips(status.msg);
                Engine.Utility.Log.Error(status.msg);
            }
           
        }
    }

    void onClick_BackBtn_Btn(GameObject caster)
    {
        DataManager.Manager<LoginDataManager>().LogoutData();

        StepManager.Instance.AddLoginScene(StepManager.LOGINSCENE,UnityEngine.SceneManagement.LoadSceneMode.Additive, (obj) =>
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.LoginPanel, data: LoginPanel.ShowUIEnum.StartGame);
        });
        HideSelf();
    }

    void ChooseRole(SelectUserInfo role_info)
    {
        if (role_info == null)
        {
            return;
        }
        if (role_info.id == m_nSelectUID)
        {
            return;
        }

        if(m_Avater!=null)
        {
            m_Avater.Destroy();
            m_Avater = null;
        }
        
        m_nSelectUID = role_info.id;

        //m_model_node.transform.DestroyChildren();
        rotateY = m_model_node.transform.localRotation.y;
        int nLayer = LayerMask.NameToLayer("ShowModel");
        Client.AvatarUtil.CreateAvater(ref m_Avater, role_info, m_model_node, nLayer, null);
        m_Avater.PlayAni(Client.EntityAction.Stand,null);
    }


    private void GoToCreateRole()
    {
       
        StepManager.Instance.AddLoginScene(StepManager.CHOOSEROLESCENE,UnityEngine.SceneManagement.LoadSceneMode.Additive, (obj) =>
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CreateRolePanel);
        });
        HideSelf();
    }

    void onClick_BtnDelete_Btn(GameObject caster)
    {

    }
    private  void ReqEnter(uint selectCharid)
    {
        StepManager.Instance.OnBeginStep(Step.LOAD);

        NetService.Instance.Send(new stSendPhysicalAddressSelectUserCmd()
        {
            mac = Cmd.ConstDefine.DEFAULT_MAC,
        });

        NetService.Instance.Send(new stClientMachineInfoSelectUserCmd()
        {
            adapterinfo64 = "",
            cpuinfo128 = "",
            meminfo64 = "",
        });

        NetService.Instance.Send(new stLoginSelectUserCmd()
        {
            data = new ImageCheckData(),
            charid = selectCharid,
            mapid = 1,
        });
        DataManager.Manager<LoginDataManager>().LastLoginCharID = selectCharid;
    }

    void LoadRoleModel()
    {
        m_scene_obj = StepManager.Instance.GetSceneRoot(StepManager.CHOOSEROLESCENE);
        if (m_scene_obj == null)
        {
            return;
        }
        m_scene_obj.gameObject.SetActive(true);
        m_scene_obj.layer = LayerMask.NameToLayer("ShowModel");
        m_model_node = m_scene_obj.transform.Find("model");
        if (m_model_node == null)
        {
            Log.Error("创建角色场景没有model节点");
        }

        //if (null == m_camera_anim)
        //{
        //    m_camera_anim = m_scene_obj.transform.Find("MainCamera").GetComponent<Animator>();
        //}
        //m_camera_anim.SetInteger(Animator.StringToHash("Job"), 0);
        //m_camera_anim.SetTrigger("StartTrigger");
    }

    void onClick_DeleteRoleBtn_Btn(GameObject caster)
    {
        GameCmd.SelectUserInfo info = GetRoleInfoByIndex(m_iCurIndex);
        if (info == null)
        {
            return;
        }

        string des = "";
        uint limitLv = GameTableManager.Instance.GetGlobalConfig<uint>("DelCharStayMinLevel");
        bool matchLv = info.level >= limitLv;
        bool matchNum = lstRoleList.Count > 1;
        TextManager Tmger =  DataManager.Manager<TextManager>();
        //一个角色不能删除
        if (!matchNum)
        {
            des = Tmger.GetLocalText(LocalTextType.DeleteRole_Tips_4);
            TipsManager.Instance.ShowTipWindow(TipWindowType.Ok, des,null);
            return;
        }
        if (info.del == 1)
        {
            des = Tmger.GetLocalText(LocalTextType.Login_Server_InDelete);
            TipsManager.Instance.ShowTipWindow(TipWindowType.Ok, des, null);
            return;
        }
        //小于30  不可恢复
        if (!matchLv)
        {
            des = Tmger.GetLocalFormatText(LocalTextType.DeleteRole_Tips_1, info.name);
        }
        else
        {
            uint remainHours = GameTableManager.Instance.GetGlobalConfig<uint>("DelCharStayTime");
            des = Tmger.GetLocalFormatText(LocalTextType.DeleteRole_Tips_2, info.name, remainHours, remainHours);
        }
      
        Action yes =delegate
        {
            if (!matchLv)
            {
                m_iCurIndex = -1;
                m_iPreIndex = -1;
            }
            stDeleteSelectUserCmd cmd = new stDeleteSelectUserCmd() { charid = info.id };
            NetService.Instance.Send(cmd);
           
        };
        TipsManager.Instance.ShowTipWindow(TipWindowType.YesNO, des, yes, null, title: Tmger.GetLocalText(LocalTextType.Local_TXT_Tips), okstr: Tmger.GetLocalText(LocalTextType.Local_TXT_Confirm), cancleStr: Tmger.GetLocalText(LocalTextType.Local_TXT_Cancel));
    }

    #region IUIAnimation
    private TweenPosition m_leftTP = null;
    private TweenPosition m_rightTp = null;
    private bool m_bool_playAnim = false;
    //动画In
    public override void AnimIn(EventDelegate.Callback onComplete)
    {
        if (null == m_leftTP)
        {
            m_leftTP = m_trans_LeftInfos.GetComponent<TweenPosition>();
        }
        if (null == m_rightTp)
        {
            m_rightTp = m_trans_RightInfos.GetComponent<TweenPosition>();
        }

        if (null != m_leftTP)
            m_leftTP.ResetToBeginning();

        if (null != m_rightTp)
            m_rightTp.ResetToBeginning();

        EventDelegate.Callback animInAction = () =>
        {
            m_bool_playAnim = false;
        };
        if (null == onComplete)
        {
            onComplete = animInAction;
        }
        else
        {
            onComplete += animInAction;
        }

        m_leftTP.onFinished.Clear();
        EventDelegate.Set(m_leftTP.onFinished, onComplete);
        m_leftTP.PlayForward();
        m_leftTP.enabled = true;

        m_rightTp.PlayForward();
        m_rightTp.enabled = true;
    }
    //动画Out
    public override void AnimOut(EventDelegate.Callback onComplete)
    {
        base.AnimOut(onComplete);
        //if (null == ta)
        //{
        //    ta = m_widget_Content.GetComponent<TweenAlpha>();
        //}
        //tp.onFinished.Clear();
        //EventDelegate d = new EventDelegate(onComplete);
        //d.oneShot = true;
        //EventDelegate.Set(tp.onFinished, d);
        //tp.PlayForward();
    }
    //重置动画
    public void ResetAnim()
    {
        
    }
    #endregion
}
