using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Engine.Utility;
using Engine;
using Client;
using Common;
using Cmd;
using System;
public partial class LoginPanel : UIPanelBase
{
    private LoginDataManager m_lgMgr = null;
    private LoginDataManager Mgr
    {
        get
        {
            return m_lgMgr;
        }
    }
    public enum ShowUIEnum
    {
        None,
        //授权
        Authorize,
        //登陆账号
        LoginAccout,
        //开始游戏
        StartGame,
    }


    private void LoadRenderObj(string strObj, string strMaterial)
    {
        //人物资源预先加载
        IRenderObj obj = null;
        IRenderSystem rs = RareEngine.Instance().GetRenderSystem();

        rs.CreateRenderObj(ref strObj, ref obj, null, Vector3.zero, TaskPriority.TaskPriority_Normal, true);
        if (obj != null)
        {
            obj.ApplyMaterial(strMaterial);
            rs.RemoveRenderObj(obj);
        }



    }

    private void LoadEffect(string strEffect, int nCount = 1)
    {
        Engine.IRenderSystem renderSys = Engine.RareEngine.Instance().GetRenderSystem();
        Engine.IEffect effect = null;
        List<Engine.IEffect> temp = new List<IEffect>();

        for (int i = 0; i < nCount; i++)
        {
            renderSys.CreateEffect(ref strEffect, ref effect, null, Engine.TaskPriority.TaskPriority_Normal, true);
            temp.Add(effect);
        }


        for (int i = 0; i < nCount; i++)
        {
            renderSys.RemoveEffect(temp[i]);
        }
        temp.Clear();

    }
    protected override void OnLoading()
    {
        base.OnLoading();
        m_lgMgr = DataManager.Manager<LoginDataManager>();

        //-------------------------------------------------------
        LoadRenderObj("assetbundles/fashi_nan04_high.obj.u3d", "materials/FaShi_Nan04_D_High.json");
        LoadRenderObj("assetbundles/zhanshi_nan04_high.obj.u3d", "materials/ZhanShi_Nan04_D_High.json");
        LoadRenderObj("assetbundles/anwu_nv04_high.obj.u3d", "materials/AnWu_Nv04_D_High.json");
        LoadRenderObj("assetbundles/huanshi_nv04_high.obj.u3d", "materials/HuanShi_Nv04_D_High.json");

        //特效加载
        LoadEffect("effect/character/zhanshi/zs_zhanshi_tx.fx", 1);
        LoadEffect("effect/character/fashi/s_05.fx", 1);
        LoadEffect("effect/character/anwu/an_zhanshi_tx.fx", 1);
        LoadEffect("effect/character_show/anwuzhaohuanshou.fx", 1);
        LoadEffect("effect/character/huanshi/h_03.fx", 1);


        //table.RandomNameDataBase.PrefixList();

        //DataManager.Manager<UIPanelManager>().LoadPanel(PanelID.CreateRolePanel);
        //DataManager.Manager<UIPanelManager>().LoadPanel(PanelID.ChooseRolePanel);
    }

    protected override void OnShow(object data)
    {
        m_label_version.text = "当前版本号: " + Game.Upgrade.Instance().version;
        if (data != null && data is ShowUIEnum)
        {
            ShowUIEnum uiEnum = (ShowUIEnum)data;
            ShowLoginUI(uiEnum);
        }
        else
        {
            ShowLoginUI(ShowUIEnum.LoginAccout);
        }

    }

    IEnumerator DelayShowNoticePanel()
    {
        yield return new WaitForSeconds(0.5f);
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.LoginNoticePanel);
    }
    protected override void OnHide()
    {
        base.OnHide();
    }

    #region

    void onClick_BtnServerList_Btn(GameObject caster)
    {
        if (GlobalConfig.Instance().Test)
        {
            TipsManager.Instance.ShowTips("本次只开放内部服");
            return;
        }
        if (!Mgr.CurAreaServerEnable)
        {
            TipsManager.Instance.ShowTips("当前无可用区服");
            return;
        }
        this.HideSelf();
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ChooseServerPanel);
    }

    /// <summary>
    ///  非sdk 登录账号
    /// </summary>
    /// <param name="caster"></param>
    void onClick_BtnLoginAcont_Btn(GameObject caster)
    {
        //UserData.Account = m_input_accunt.value;
        DataManager.Manager<LoginDataManager>().Acount = m_input_accunt.value;
        
        //Action act = new Action(() =>
        //{
        //    m_trans_loginAccount.gameObject.SetActive(false);
        //    m_trans_loginServer.gameObject.SetActive(true);
        //    ShowGongGaoContent();
        //    m_btn_btnback.gameObject.SetActive(true);
        //    InitZone();

        //});
        //DataManager.Manager<LoginDataManager>().StateMachine.ChangeState((int)LoginSteps.LGS_Platform, act);

        DataManager.Manager<LoginDataManager>().DoLoginStep(LoginSteps.LGS_FetchASFilterData);

    }

    private void DoLoginGameServer()
    {

    }

    void onClick_BtnStartGame_Btn(GameObject caster)
    {
        /*
        //         Pmd.ZoneInfo zone = DataManager.Manager<LoginDataManager>().GetLoginZoneInfo();
        //         if (zone != null && zone.state == Pmd.ZoneState.Shutdown)
        //         {
        //             TipsManager.Instance.ShowTips(LocalTextType.Login_ZoneIsClosed);
        //             TipsManager.Instance.ShowTips("本区还没开放");
        //             return;
        //         }
        */
        // LoginStepTwo
        DataManager.Manager<LoginDataManager>().StartGame();
    }

    void onClick_BtnNotice_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.LoginNoticePanel);
    }
    void onClick_Btnback_Btn(GameObject caster)
    {
        ShowLoginUI(ShowUIEnum.LoginAccout);
    }

    private void ShowLoginUI(ShowUIEnum type)
    {
        //登陆平台
        //账号登陆
        bool visible = ((type != ShowUIEnum.StartGame)
            && !DataManager.Manager<LoginDataManager>().IsSDKLogin);
        if (null != m_trans_loginAccount && m_trans_loginAccount.gameObject.activeSelf != visible)
        {
            m_trans_loginAccount.gameObject.SetActive(visible);
        }
        if (visible)
        {
            m_input_accunt.value = DataManager.Manager<LoginDataManager>().Acount;
        }

        //登陆游戏服
        visible = !visible;
        if (null != m_trans_loginServer && m_trans_loginServer.gameObject.activeSelf != visible)
        {
            m_trans_loginServer.gameObject.SetActive(visible);
        }
        if (visible)
        {
            visible = (type == ShowUIEnum.StartGame);
            if (null != m_sprite_ZoneInfoContent && m_sprite_ZoneInfoContent.gameObject.activeSelf != visible)
            {
                m_sprite_ZoneInfoContent.gameObject.SetActive(visible);
            }
            if (visible)
            {
                InitZone();
            }

            visible = (type != ShowUIEnum.None);
            if (null != m_btn_btnStartGame && m_btn_btnStartGame.gameObject.activeSelf != visible)
            {
                m_btn_btnStartGame.gameObject.SetActive(visible);
            }
        }
        visible = (type == ShowUIEnum.StartGame) && !DataManager.Manager<LoginDataManager>().IsSDKLogin;
        //返回按钮
        if (null != m_btn_btnback && m_btn_btnback.gameObject.activeSelf != visible)
        {
            m_btn_btnback.gameObject.SetActive(visible);
        }

        visible = (type != ShowUIEnum.Authorize);
        if (null != m_btn_btnNotice && m_btn_btnNotice.gameObject.activeSelf != visible)
        {
            m_btn_btnNotice.gameObject.SetActive(visible);
        }

        visible = (type != ShowUIEnum.Authorize);
        if (null != m_btn_btnAccount && m_btn_btnAccount.gameObject.activeSelf != visible)
        {
            m_btn_btnAccount.gameObject.SetActive(visible);
        }

        StartCoroutine(RfreshLogin());
    }
    IEnumerator RfreshLogin()
    {
        yield return new WaitForSeconds(1f);
        if (Panel != null)
        {
            Panel.RebuildAllDrawCalls();
        }

    }
    #endregion


    void ShowGongGaoContent()
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.LoginNoticePanel);
    }

    void InitZone()
    {
        m_label_LabelServer.text = "";
        DataManager.Manager<LoginDataManager>().IpStr = null;

        Pmd.ZoneInfo curZoneInfo = null;
        if (GlobalConfig.Instance().Test)
        {
            curZoneInfo = Mgr.GetZoneInfo(GlobalConfig.Instance().TestServerId);
        }else
        {
            curZoneInfo = Mgr.GetZoneInfo();
        }

        int zoneState = 4;
        string zoneName = "";
        if (null != curZoneInfo)
        {
            //有可用区服
            zoneState = UIServerListGrid.GetZoneOnlineStatus(curZoneInfo);
            zoneName = curZoneInfo.zonename;
            DataManager.Manager<LoginDataManager>().StartCheckServerStateTimer();
        }
        else
        {
            //无可用区服
            zoneState = 4;
            zoneName = "无可用区服";
        }

        if (m_spriteEx_status != null)
        {
            m_spriteEx_status.ChangeSprite(zoneState);
        }
        if (null != m_label_LabelServer)
        {
            m_label_LabelServer.text = zoneName;
        }
    }

    void onClick_BtnAccount_Btn(GameObject obj)
    {
        DataManager.Manager<LoginDataManager>().DoSDKLoginout();
    }


    public void RestartApplication()
    {
        if (!Application.isEditor)
        {
            //AndroidJavaClass jc = new AndroidJavaClass("com.zqgame.hxworld.MainActivity");
            //jc.CallStatic("Restart");
        }
    }
    void onClick_BtnFeedback_Btn(GameObject obj)
    {
    }
    
    
    public void DelectDir(string srcPath)
    {
        try
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(srcPath);
            System.IO.FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
            foreach (System.IO.FileSystemInfo i in fileinfo)
            {
                if (i is System.IO.DirectoryInfo)
                {
                    System.IO.DirectoryInfo subdir = new System.IO.DirectoryInfo(i.FullName);
                    subdir.Delete(true);          //删除子目录和文件
                }
                else
                {
                    System.IO.File.Delete(i.FullName);      //删除指定文件
                }
            }
        }
        catch (Exception e)
        {
            throw;
        }
    }

    void onClick_BtnRepare_Btn(GameObject obj)
    {
        /// 这是修复,不是准备
        Action agree = delegate
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                string strPath = Application.persistentDataPath + "/assets/game.version";
                System.IO.FileInfo fi = new System.IO.FileInfo(strPath);
                if (fi.Exists)
                {
                    fi.Delete();
                }
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                DelectDir(Application.persistentDataPath);
            }

            Application.Quit();

            RestartApplication();
        };

        Action refuse = delegate
        {

        };

        string des;
        table.LocalTextDataBase data = GameTableManager.Instance.GetTableItem<table.LocalTextDataBase>(460001);
        if (data != null)
        {
            des = data.text;
        }
        else
        {
            des = string.Format("当登录界面显示异常，无法登录游戏时，可以尝试进行修复。该操作会清除本地补丁并重新下载，请在良好的网络环境下进行，并注意手动重启客户端");
        }

        TipsManager.Instance.ShowTipWindow(Client.TipWindowType.CancelOk, des, agree, refuse, null, "修复", "进行修复", "取消", 1);


    }

    void onClick_Random_name_Btn()
    {
        //var namePrefix = table.RandomNameDataBase.PrefixList();
        //var name = string.Empty;
        //{
        //    var nameFemale = table.RandomNameDataBase.FemaleList();
        //    name = string.Format("{0}{1}", namePrefix.Random().namePrefix, nameFemale.Random().femaleName);
        //}

        Log.Trace("rand name: " + name);
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eLoginAuthorizeState)
        {
            if (null != param && param is bool)
            {
                bool visible = (bool)param;
                if (null != m_btn_btnStartGame && m_btn_btnStartGame.gameObject.activeSelf != visible)
                {
                    m_btn_btnStartGame.gameObject.SetActive(visible);
                }
            }
        }
        //         if (msgid == UIMsgID.eZoneData)
        //         {
        //             Pmd.ZoneInfoListLoginUserPmd_S zoneData = (Pmd.ZoneInfoListLoginUserPmd_S)param;
        //             onGetZoneList(zoneData.zonelist);
        //           
        //             OnShow(null);
        //         }
        return true;
    }
}
