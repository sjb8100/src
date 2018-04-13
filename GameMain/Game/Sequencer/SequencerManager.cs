using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleSceneMaterial
{
    enum enumProfession
    {
        Profession_Soldier,
        Profession_Spy,
        Profession_Freeman,
        Profession_Doctor,
    };

    static AppleSceneMaterial s_Instance = null;
    public uint m_nCurMapID = 0;

    public static AppleSceneMaterial Instance()
    {
        if (null == s_Instance)
        {
            s_Instance = new AppleSceneMaterial();
        }

        return s_Instance;
    }
    public AppleSceneMaterial()
    {
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SYSTEM_LOADSCENECOMPELETE, OnSceneLoadCompelete);
    }

    private void ApplyMaterial(Client.IPlayer pPlayer, uint uSuit)
    {
        table.SuitDataBase data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>(uSuit);
        if (data == null)
            return;

        if (data.defaultMaterial == 0)
            return;


        table.ResourceDataBase res = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(data.defaultMaterial);
        if (res == null)
            return;


        if (res.strPath == "")
            return;


        Engine.IRenderObj renderObj = pPlayer.renderObj;
        if (renderObj != null)
        {
            renderObj.ApplyMaterial(res.strPath);
        }
    }


    //自己
    private void OnSceneLoadCompelete(int nEventId, object param)
    {
        Client.stLoadSceneComplete loadScene = (Client.stLoadSceneComplete)param;
        uint nMapID = (uint)loadScene.nMapID;
        m_nCurMapID = nMapID;
        table.MapDataBase mapDB = GameTableManager.Instance.GetTableItem<table.MapDataBase>(nMapID);
        if (mapDB == null)
            return;

        if (mapDB.dwMaterial == 0)
        {
            // 使用默认材质
            Client.IPlayer pPlayer = Client.ClientGlobal.Instance().MainPlayer;
            if (pPlayer != null)
            {
                // 时装id
                uint uSuit = 0;

                List<GameCmd.SuitData> lstSuit = null;
                pPlayer.GetSuit(out lstSuit);

                if (lstSuit.Count > 0)
                {
                    uSuit = lstSuit[0].baseid;
                    ApplyMaterial(pPlayer, uSuit);
                }

            }
        }
        else
        {
            table.ResourceDataBase res = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(mapDB.dwMaterial);
            if (res == null)
                return;

            if (res.strPath == "")
                return;

            Engine.IRenderObj renderObj = Client.ClientGlobal.Instance().MainPlayer.renderObj;
            if (renderObj != null)
            {
                renderObj.ApplyMaterial(res.strPath);
            }
        }

    }


};

public class SequencerManager : Engine.ILoadSceneCallback
{
    //public delegate void SequencerPlayFinished();
    //public SequencerPlayFinished PlaybackFinished = delegate { };
    static SequencerManager s_Instance = null;
    public static SequencerManager Instance()
    {
        if (null == s_Instance)
        {
            s_Instance = new SequencerManager();
        }

        return s_Instance;
    }

    //是否播放中
    private bool m_IsPlay = false;
    //剧情节点
    private GameObject m_SequencerGameObject = null;

    //剧情用的临时对象,剧情结束后要删除
    public List<GameObject> m_SequencerTempObject = new List<GameObject>();
    public List<Engine.IRenderObj> m_SequencerTempRender = new List<Engine.IRenderObj>();
    public List<Engine.IEffect> m_SequencerTempEffect = new List<Engine.IEffect>();
    public List<Client.IEntity> m_SequencerTempEntity = new List<Client.IEntity>();



    private Camera m_SequencerCamera = null;//当前正在使用的剧情相机.
    table.SequencerData m_SequencerData = null;//当前正在使用的剧情数据

    private Engine.IRenderSystem m_RenderSystem = null;


    public uint m_MapID = 0;
    //剧情触发类型

    //跳过剧情按钮是否显示
    private bool m_bSkipSequencerBtn = true;

    enum TriggerType
    {
        task = 1,//任务触发
        chapterEnd = 3,
    };

    private List<table.SequencerData> m_TaskTriggerType = new List<table.SequencerData>();
    private List<table.SequencerData> m_ChapterType = new List<table.SequencerData>();
    private List<table.ChapterDabaBase> m_lstChapterTaskTrigger = new List<table.ChapterDabaBase>();


    public void Init()
    {

    }
    public void UnInit()
    {

    }

    public bool IsPlaying()
    {
        return m_IsPlay;
    }

    public bool IsShowSkipSequencerBtn()
    {
        return m_bSkipSequencerBtn;
    }

    public SequencerManager()
    {
        m_RenderSystem = Engine.RareEngine.Instance().GetRenderSystem();

        //任务触发
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_DONE, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_ACCEPT, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_CANSUBMIT, OnEvent);

        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CHAPTER_EFFECT_END, OnChapterEnd);



        //table.SequencerData
        List<table.SequencerData> tableList = GameTableManager.Instance.GetTableList<table.SequencerData>();
        for (int i = 0; i < tableList.Count; i++)
        {
            table.SequencerData data = tableList[i];

            switch (data.TriggerType)
            {
                case (int)TriggerType.task:
                    m_TaskTriggerType.Add(data);
                    break;
                case (int)TriggerType.chapterEnd:
                    m_ChapterType.Add(data);
                    break;
            }
        }

        List<table.ChapterDabaBase> lstchapter = GameTableManager.Instance.GetTableList<table.ChapterDabaBase>();
        table.ChapterDabaBase database = null;

        for (int i = 0, imax = lstchapter.Count; i < imax; i++)
        {
            database = lstchapter[i];

            switch ((TriggerType)database.TriggerType)
            {
                case TriggerType.task:
                    m_lstChapterTaskTrigger.Add(database);
                    break;
                default:
                    break;
            }
        }
    }

    private void OnChapterEnd(int nEventId, object param)
    {
        if (nEventId == (int)Client.GameEventID.CHAPTER_EFFECT_END)
        {
            uint nChapter = (uint)param;
            table.SequencerData database = null;
            for (int i = 0; i < m_ChapterType.Count; i++)
            {
                database = m_ChapterType[i];
                if (database.Paramer1 == 1) // 章节结束触发
                {
                    if (database.Paramer2 == nChapter) //章节id
                    {
                        m_SequencerData = database;
                        SequencerManager.Instance().PlaySequencer(m_SequencerData.SequencerFile);
                        return;
                    }
                }
            }

        }

    }
    private void OnEvent(int nEventId, object param)
    {
        if (nEventId == (int)Client.GameEventID.TASK_DONE)
        {
            //完成任务
            Client.stTaskDone td = (Client.stTaskDone)param;
            table.ChapterDabaBase database = null;
            bool flag = false;

            for (int i = 0, imax = m_lstChapterTaskTrigger.Count; i < imax; i++)
            {
                database = m_lstChapterTaskTrigger[i];

                if (database.Paramer2 == td.taskid)
                {
                    if (database.Paramer1 == 2)
                    {
                        PlayChapterEffect(database);
                        return;
                    }
                }
            }

            for (int i = 0; i < m_TaskTriggerType.Count; i++)
            {
                table.SequencerData data = m_TaskTriggerType[i];

                if (data.Paramer1 == 2)
                {
                    if (data.Paramer2 == td.taskid)
                    {
                        m_SequencerData = data;
                        SequencerManager.Instance().PlaySequencer(m_SequencerData.SequencerFile);
                        return;
                    }

                }

            }

        }
        else if ((int)Client.GameEventID.TASK_ACCEPT == nEventId)
        {
            //接受任务
            uint taskid = (uint)param;
            table.ChapterDabaBase database = null;
            for (int i = 0, imax = m_lstChapterTaskTrigger.Count; i < imax; i++)
            {
                database = m_lstChapterTaskTrigger[i];

                if (database.Paramer2 == taskid)
                {
                    if (database.Paramer1 == 1)
                    {
                        PlayChapterEffect(database);
                        return;
                    }
                }
            }
            for (int i = 0; i < m_TaskTriggerType.Count; i++)
            {
                table.SequencerData data = m_TaskTriggerType[i];

                // 这个剧情是接受任务触发
                if (data.Paramer1 == 1)
                {
                    if (data.Paramer2 == taskid)
                    {
                        m_SequencerData = data;
                        SequencerManager.Instance().PlaySequencer(m_SequencerData.SequencerFile);
                        return;
                    }

                }

            }
        }
        else if ((int)Client.GameEventID.TASK_CANSUBMIT == nEventId)
        {
            //提交任务
            Client.stTaskCanSubmit cs = (Client.stTaskCanSubmit)param;
            table.ChapterDabaBase database = null;
            for (int i = 0, imax = m_lstChapterTaskTrigger.Count; i < imax; i++)
            {
                database = m_lstChapterTaskTrigger[i];

                if (database.Paramer2 == cs.taskid)
                {
                    if (database.Paramer1 == 3)
                    {
                        PlayChapterEffect(database);
                        return;
                    }
                }
            }
            for (int i = 0; i < m_TaskTriggerType.Count; i++)
            {
                table.SequencerData data = m_TaskTriggerType[i];
                if (data.Paramer1 == 3)
                {
                    if (data.Paramer2 == cs.taskid)
                    {
                        m_SequencerData = data;
                        SequencerManager.Instance().PlaySequencer(m_SequencerData.SequencerFile);
                        return;
                    }
                }
            }

        }



    }


    public string GetDialogText(uint nID)
    {
        table.SequencerDialog dlg = GameTableManager.Instance.GetTableItem<table.SequencerDialog>(nID);
        if (dlg != null)
        {
            return dlg.Text;
        }

        return "error";
    }

    public GameObject GetGameObject(string strName)
    {
        GameObject result = null;
        for (int i = 0; i < m_SequencerTempRender.Count; i++)
        {
            Engine.IRenderObj renderObj = m_SequencerTempRender[i];
            if (renderObj != null)
            {
                if (renderObj.GetNode().GetTransForm().gameObject.name == strName)
                {
                    result = renderObj.GetNode().GetTransForm().gameObject;
                    return result;
                }

            }
        }

        for (int i = 0; i < m_SequencerTempEntity.Count; i++)
        {
            Client.IEntity entity = m_SequencerTempEntity[i];
            if (entity != null)
            {
                Engine.IRenderObj renderObj = entity.renderObj;
                if (renderObj != null)
                {
                    if (renderObj.GetNode().GetTransForm().gameObject.name == strName)
                    {
                        result = renderObj.GetNode().GetTransForm().gameObject;
                        return result;
                    }
                }

            }

        }


        return null;

    }

    private void ClearSequencerTempObject()
    {
        for (int i = 0; i < m_SequencerTempObject.Count; i++)
        {
            GameObject temp = m_SequencerTempObject[i];
            if (temp != null)
            {
                GameObject.DestroyObject(temp);
            }
        }
        m_SequencerTempObject.Clear();


        for (int i = 0; i < m_SequencerTempRender.Count; i++)
        {
            Engine.IRenderObj temp = m_SequencerTempRender[i];
            if (temp != null)
            {
                m_RenderSystem.RemoveRenderObj(temp);
            }
        }
        m_SequencerTempRender.Clear();


        for (int i = 0; i < m_SequencerTempEffect.Count; i++)
        {
            Engine.IEffect temp = m_SequencerTempEffect[i];
            if (temp != null)
            {
                m_RenderSystem.RemoveEffect(temp);
            }
        }
        m_SequencerTempEffect.Clear();


        Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
        if (es == null) { return; }
        for (int i = 0; i < m_SequencerTempEntity.Count; i++)
        {
            Client.IEntity entity = m_SequencerTempEntity[i];
            if (entity != null)
            {
                es.RemoveEntity(entity);
            }
        }
        m_SequencerTempEntity.Clear();

    }

    //暂停剧情
    public void Pause()
    {
        if (m_SequencerGameObject != null)
        {
            WellFired.USSequencer sequencer = m_SequencerGameObject.GetComponent<WellFired.USSequencer>();
            sequencer.Pause();
        }
    }

    public void Play()
    {
        if (m_SequencerGameObject != null)
        {
            WellFired.USSequencer sequencer = m_SequencerGameObject.GetComponent<WellFired.USSequencer>();
            sequencer.Play();
        }
    }

    //停止剧情
    public void StopSequencer()
    {
        if (m_SequencerGameObject != null)
        {
            WellFired.USSequencer sequencer = m_SequencerGameObject.GetComponent<WellFired.USSequencer>();
            sequencer.Stop();

            PlaybackFinished(sequencer);
        }

    }

    // ID播放
    public void PlaySequencer(int nID)
    {
        for (int i = 0; i < m_TaskTriggerType.Count; i++)
        {
            table.SequencerData data = m_TaskTriggerType[i];
            if (data.ID == nID)
            {
                PlaySequencer(data.SequencerFile);
                return;
            }
        }
        for (int i = 0; i < m_ChapterType.Count; i++)
        {
            table.SequencerData data = m_ChapterType[i];
            if (data.ID == nID)
            {
                PlaySequencer(data.SequencerFile);
                return;
            }
        }


    }
    public void PlaySequencer(string strSequencer)
    {
        if (m_IsPlay)
            return;

        if (strSequencer == "")
            return;

        //隐藏npc
        //Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
        //if (es != null)
        //    es.ShowEntity(false);
        try
        {
            m_SequencerGameObject = WellFired.USSequencerLoad.LoadSequencerFromXml(strSequencer);
        }
        catch
        {
            PlaybackFinished(null);
            goto Exit0;
        }

        if (m_SequencerGameObject != null)
        {
            WellFired.USSequencer sequencer = m_SequencerGameObject.GetComponent<WellFired.USSequencer>();

            if (sequencer != null)
            {
                sequencer.PlaybackFinished += this.PlaybackFinished;
                sequencer.BeforeUpdate += this.BeforeUpdate;

                m_IsPlay = true;
                if (m_IsPlay == true)
                {
                    RoleStateBarManager.HideHeadStatus();//隐藏npc血条

                    //隐藏提示
                    TipsManager.Instance.EnableTips(false);

                    //关闭声音
                    //IClientGlobal的MuteGameSound方法
                    Client.ClientGlobal.Instance().MuteGameSound(true);


                    ///显示黑边
                    StoryPanel.StoryData data = new StoryPanel.StoryData();
                    data.Des = "";
                    data.ShowSkip = SequencerManager.Instance().IsShowSkipSequencerBtn();
                    data.SkipDlg = SequencerManager.Instance().OnSkipSequencer;
                    data.ColliderClickDlg = SequencerManager.Instance().OnClickSequencer;

                    ////隐藏npc
                    Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
                    if (es != null)
                        es.ShowEntity(false);


                }

                sequencer.Play();
            }
        }



    Exit0:
        return;

    }



    public void BeforeUpdate(WellFired.USSequencer sequencer, float newRunningTime)
    {
        //int i = 0;
        //if (m_IsPlay)
        //{
        //    Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
        //    if (rs != null)
        //    {
        //        Engine.IScene scene = rs.GetActiveScene();
        //        if (scene != null)
        //        {
        //            if (m_SequencerCamera)
        //                scene.CullVisibleSet(m_SequencerCamera);
        //        }
        //    }
        //}

    }


    //正常播放完成
    public void PlaybackFinished(WellFired.USSequencer sequencer)
    {
        m_IsPlay = false;

        if (m_SequencerCamera != null)
        {
            GameObject obj = GameObject.Find("MainCamera");
            Camera camera = obj.GetComponent<Camera>();
            if (camera != m_SequencerCamera)
                camera.enabled = true;

        }
        m_SequencerCamera = null;
        m_SequencerData = null;


        // 取消回调
        if (sequencer != null)
        {
            sequencer.PlaybackFinished -= this.PlaybackFinished;
            sequencer.BeforeUpdate -= this.BeforeUpdate;
        }

        try
        {
            //显示npc
            Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
            if (es != null)
                es.ShowEntity(true);

            //删除剧情gameobject
            if (m_SequencerGameObject != null)
            {
                GameObject.Destroy(m_SequencerGameObject);
                m_SequencerGameObject = null;
            }
            Debug.Log("清理剧情对象");

            //清理剧情对象
            ClearSequencerTempObject();

            //打开声音
            Client.ClientGlobal.Instance().MuteGameSound(false);
        }
        catch
        {
            goto Exit0;
        }
    Exit0:
        //隐藏提示
        TipsManager.Instance.EnableTips(true);

        //DataManager.Manager<UIPanelManager>().ShowStoryCachePanel();
        Debug.Log("剧情结束显示UI");
        RoleStateBarManager.ShowHeadStatus();
        //剧情结束隐藏黑边
        DataManager.Manager<UIPanelManager>().HideStory();
        Debug.Log("剧情结束隐藏黑边");
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.STORY_PLAY_OVER, null);

        //却换到原场景
        Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
        if (m_MapID != 0)
        {
            // 剧情播放完毕加载原场景
            table.MapDataBase data = GameTableManager.Instance.GetTableItem<table.MapDataBase>(m_MapID);
            if (data == null)
            {
                return;
            }
            table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(data.dwResPath);
            if (resDB == null)
            {
                return;
            }

            if (rs != null)
            {
                string str = resDB.strPath;
                Engine.IScene scene = rs.EnterScene(ref str, SequencerManager.Instance());
                if (scene != null)
                {
                    ///加载场景
                    SequencerManager.Instance().Pause();

                    scene.StartLoad(Vector3.one);
                }
            }
            m_MapID = 0;
        }
    }


    public void ChangeKeyframeCamera(Camera camera)
    {
        if (camera != null)
        {
            if (camera.name != "MainCamera")
            {
                m_SequencerCamera = camera;
            }
        }

    }

    //-----------------------------------------------------------------------------------------------
    public void PlayChapterEffect(table.ChapterDabaBase data)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ChapterPanel, data: data);
    }

    public void OnComplete()
    {
        if (m_MapID != 0)
        {
            //场景加载完毕
            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.StoryLoadingPanel);
            //继续播放
            Play();
        }

    }
    public void OnProgress(float fProgress,string debuginfo)
    {

    }



    private void JumpToSequencer()
    {
        if (m_IsPlay)
        {
            if (m_SequencerData != null)
            {
                if (m_SequencerData.IsJumpTo == 1)//允许跳过
                {
                    StopSequencer();
                }
            }
        }

    }


    /// 剧情跳过
    public void OnSkipSequencer()
    {
        JumpToSequencer();
    }
    //剧情中点击屏幕 
    public void OnClickSequencer()
    {

        //if (m_IsPlay)
        //{
        //    if (m_SequencerData != null)
        //    {
        //        if (m_SequencerData.IsJumpTo == 1)//允许跳过
        //        {
        //            m_bSkipSequencerBtn = true;
        //            StoryPanel.StoryData data = new StoryPanel.StoryData();
        //            data.Des = "";
        //            data.ShowSkip = SequencerManager.Instance().IsShowSkipSequencerBtn();
        //            data.SkipDlg = SequencerManager.Instance().OnSkipSequencer;
        //            data.ColliderClickDlg = SequencerManager.Instance().OnClickSequencer;

        //            DataManager.Manager<UIPanelManager>().ShowStory(data);

        //        }
        //    }
        //}

    }


};








