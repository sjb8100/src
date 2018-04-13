using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;
using GameCmd;
using table;


partial class ReLivePanel : UIPanelBase
{
    #region property

    ReLiveDataManager RLManager
    {
        get
        {
            return DataManager.Manager<ReLiveDataManager>();
        }
    }

    #endregion


    #region override

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        IPlayer player = ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            if (!player.IsDead())
            {
                HideSelf();
                return;
            }
        }

        if (data != null)
        {
            float delayTime = (float)data;
            m_trans_ReliveContent.gameObject.SetActive(false);
            StartCoroutine(DelayInit(delayTime));
        }
    }

    IEnumerator DelayInit(float delay)
    {
        yield return new WaitForSeconds(delay);
        InitUI();
    }



    protected override void OnHide()
    {

    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        return base.OnMsg(msgid, param);
    }

    #endregion

    #region mono

    void Update()
    {
        //最多30  死亡界面自动关闭
        if (Time.realtimeSinceStartup - DataManager.Manager<ReLiveDataManager>().DeadTime >= 30f)
        {
            if (Client.ClientGlobal.Instance().MainPlayer == null)
            {
                return;
            }

            //玩家已经活了，死亡界面如果还是打开的就要关闭
            if (false == Client.ClientGlobal.Instance().MainPlayer.IsDead() && true == DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.ReLivePanel))
            {
                HideSelf();
            }
        }

    }

    #endregion


    #region method

    /// <summary>
    /// 初始化
    /// </summary>
    void InitUI()
    {
        m_trans_ReliveContent.gameObject.SetActive(true);

        List<ReLiveDataManager.ReLiveData> list = RLManager.ReliveDataList;
        if (list == null)
        {
            Engine.Utility.Log.Error("未收到复活数据！！！");
            return;
        }

        m_grid_Grid.transform.DestroyChildren();

        for (int i = 0; i < list.Count; i++)
        {
            GameObject go = Instantiate(m_btn_UIReliveGrid.gameObject) as GameObject;

            go.transform.parent = m_grid_Grid.transform;
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.SetActive(true);

            UIReliveGrid grid = go.GetComponent<UIReliveGrid>();
            if (grid == null)
            {
                grid = go.AddComponent<UIReliveGrid>();
            }
            grid.SetGridData(list[i]);
            grid.RegisterUIEventDelegate(OnReliveGridEventDlg);
        }

        m_sprite_bg.height = 140 + 80 * list.Count;

        m_grid_Grid.Reposition();
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnReliveGridEventDlg(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            UIReliveGrid grid = data as UIReliveGrid;
            if (grid == null)
            {
                return;
            }

            if (grid.m_info.reliveId == 1)
            {
                IMapSystem mapSys = ClientGlobal.Instance().GetMapSystem();
                if (mapSys == null)
                {
                    return;
                }
                uint uCurMapID = mapSys.GetMapID();

                table.MapDataBase mapDB = GameTableManager.Instance.GetTableItem<table.MapDataBase>(uCurMapID);
                if (mapDB == null)
                {
                    return;
                }

                uint rebackMapID = mapDB.rebackMapID;

                if (!KHttpDown.Instance().SceneFileExists(rebackMapID))
                {
                    // 复活点复活
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.DownloadPanel);
                    HideSelf();
                    return;
                }
            } 


            GameCmd.stOKReliveUserCmd_C cmd = new GameCmd.stOKReliveUserCmd_C();
            cmd.byType = grid.m_info.reliveId;
            cmd.dwUserTempID = ClientGlobal.Instance().MainPlayer.GetID();
            cmd.dwNpcID = 0;
            NetService.Instance.Send(cmd);
        }
    }


    void onClick_UIReliveGrid_Btn(GameObject caster)
    {

    }

    #endregion
}