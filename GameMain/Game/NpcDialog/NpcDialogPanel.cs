//*************************************************************************
//	创建日期:	2016-10-28 13:57
//	文件名称:	NpcDialogPanel.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	npc对话面板
//*************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
partial class NpcDialogPanel : UIPanelBase
{

    float CONST_TASK_CD = 0;

    float CLOSETIME = 8f;

    UIXmlRichText uiXmlRichText;
    List<string> m_lstDependece = null;
    LangTalkData m_dialogInfo = null;
    int m_nindex = 0;
    private IRenerTextureObj m_RTObj = null;
    float rotateY = 170f;

    uint m_nPlayingAudioId = 0;
    protected override void OnLoading()
    {
        CONST_TASK_CD = (float)GameTableManager.Instance.GetGlobalConfig<int>("TaskCountdown");

        AdjustUI();
        m_btn_btn.transform.localPosition = new Vector3(100000, 10000, 0);
        UIEventListener.Get(m_sprite_DialogBg.gameObject).onClick = OnClickScreen;
        uiXmlRichText = m_widget_RichText.GetComponent<UIXmlRichText>();
        UnityEngine.Object Prefab = UIManager.GetResGameObj("prefab/grid/richtextlabel.unity3d", "Assets/UI/prefab/Grid/RichTextLabel.prefab");
        uiXmlRichText.protoLabel = (GameObject)Prefab;

        UIEventListener.Get(m_widget_dragModel.gameObject).onDrag = OnDragModel;
        UIEventListener.Get(m_widget_dragModel.gameObject).onClick = OnClickScreen;
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (data is LangTalkData)
        {
            this.m_dialogInfo = data as LangTalkData;
            if (this.m_dialogInfo == null)
            {
                return;
            }

            m_btn_btn_jump.gameObject.SetActive(m_dialogInfo.npcType != LangTalkData.NpcType.TalkOnly);

            m_nindex = 0;
            ShowTalkStr(m_nindex);
            ShowBtns();


        }
    }

    protected override void OnHide()
    {
        base.OnHide();
        if (m_RTObj != null)
        {
            m_RTObj.Release();
            m_RTObj = null;
        }
        m_nindex = 0;
        ResetDes();
    }

    public override void OnColliderMaskClicked()
    {
        base.OnColliderMaskClicked();
        DoJump();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }

    private void AdjustUI()
    {
        if (null != m_sprite_DialogBg)
        {
            m_sprite_DialogBg.width = (int)UIRootHelper.Instance.TargetSize.x;
        }
    }

    void ShowBtns()
    {
        if (m_dialogInfo == null)
        {
            return;
        }

        if (m_dialogInfo.buttons.Count > 1)
        {
            m_sprite_select.alpha = 1f;

            m_trans_btnroot.DestroyChildren();


            int btnHeight = m_btn_btn.GetComponent<UISprite>().height;
            UIPanel panel = m_trans_btnroot.parent.GetComponent<UIPanel>();
            Vector4 clip = panel.baseClipRegion;
            clip.w = m_dialogInfo.buttons.Count * (btnHeight + 13);
            panel.baseClipRegion = clip;

            UISprite topSprite = m_sprite_select.transform.Find("Sprite").GetComponent<UISprite>();
            int totalHeight = (int)clip.w + topSprite.height + 35;

            m_sprite_select.height = totalHeight;

            topSprite.transform.localPosition = new UnityEngine.Vector3(0, m_sprite_select.height - 7, 0);

            m_sprite_select.transform.Find("btnview").localPosition = new UnityEngine.Vector3(0, clip.w * 0.5f + 10, 0);
            m_trans_btnroot.localPosition = new UnityEngine.Vector3(0, clip.w * 0.5f - topSprite.height * 0.5f + 5, 0);

            for (int i = 0; i < m_dialogInfo.buttons.Count; i++)
            {
                GameObject go = NGUITools.AddChild(m_trans_btnroot.gameObject, m_btn_btn.gameObject);
                if (go != null)
                {
                    go.GetComponentInChildren<UILabel>().text = m_dialogInfo.buttons[i].strBtnName;
                    go.name = string.Format("btn_{0}", i);
                    UIEventListener.Get(go).onClick = onClick_Btn_Btn;
                    go.transform.localPosition = new UnityEngine.Vector3(0, -i * (btnHeight + 13), 0);
                    InitBtnIcon(go, this.m_dialogInfo.buttons[i].taskId);
                    go.SetActive(true);
                }
            }
            m_trans_btnroot.parent.GetComponent<UIScrollView>().ResetPosition();

        }
        else
        {
            m_sprite_select.alpha = 0f;
        }
    }
    void onClick_Btn_Btn(GameObject caster)
    {
        int index = int.Parse(caster.name.Replace("btn_", ""));
        if (m_dialogInfo == null)
        {
            return;
        }
        HideSelf();
        if (index >= 0 && index < m_dialogInfo.buttons.Count)
        {
            NetService.Instance.Send(new GameCmd.stDialogSelectScriptUserCmd_C()
            {
                step = m_dialogInfo.strStep,
                dwChose = m_dialogInfo.buttons[index].nindex,
            });
        }
    }

    void InitBtnIcon(GameObject btn, uint btnTaskId)
    {
        Transform iconTransf = btn.transform.Find("Icon");
        if (iconTransf == null)
        {
            return;
        }

        if (btnTaskId == 0)
        {
            iconTransf.gameObject.SetActive(false);
            return;
        }
        else
        {
            iconTransf.gameObject.SetActive(true);
        }

        UISprite iconSp = iconTransf.GetComponent<UISprite>();
        if (iconSp == null)
        {
            iconSp = iconTransf.gameObject.AddComponent<UISprite>();
        }

        List<QuestTraceInfo> traceTask;
        DataManager.Manager<TaskDataManager>().GetAllQuestTraceInfo(out traceTask);
        if (traceTask != null)
        {
            QuestTraceInfo questTraceInfo = traceTask.Find((data) => { return btnTaskId == data.taskId; });
            if (questTraceInfo != null)
            {
                GameCmd.TaskProcess taskProcess = questTraceInfo.GetTaskProcess();
                if (taskProcess == GameCmd.TaskProcess.TaskProcess_None) //可接
                {
                    iconSp.spriteName = "tubiao_renwu_1";
                }
                else if (taskProcess == GameCmd.TaskProcess.TaskProcess_Doing)//执行中
                {
                    iconSp.spriteName = "tubiao_renwu_4";
                }
                else if (taskProcess == GameCmd.TaskProcess.TaskProcess_CanDone)//可以交付
                {
                    iconSp.spriteName = "tubiao_renwu_3";
                }
            }
            else
            {
                iconTransf.gameObject.SetActive(false);
            }

        }
        else
        {
            iconTransf.gameObject.SetActive(false);
        }
    }

    void ShowRoleTexture(bool bPlayer, table.NpcDataBase npcDb = null)
    {
        if (m_RTObj != null)
        {
            m_RTObj.Release();
        }
        if (bPlayer)
        {
            m_RTObj = DataManager.Manager<RenderTextureManager>().CreateRenderTextureObj(MainPlayerHelper.GetMainPlayer(), 700);
        }
        else if (npcDb != null)
        {
            m_RTObj = DataManager.Manager<RenderTextureManager>().CreateRenderTextureObj((int)npcDb.dwViewModelSet, 700);
        }
        if (m_RTObj == null)
        {
            return;
        }
        if (bPlayer)
        {
            Client.IPlayer player = MainPlayerHelper.GetMainPlayer();
            if (player == null)
            {
                return;
            }
            uint job = (uint)player.GetProp((int)Client.PlayerProp.Job);
            int sex = (int)player.GetProp((int)Client.PlayerProp.Sex);
            table.SelectRoleDataBase roleSelectData = GameTableManager.Instance.GetTableItem<table.SelectRoleDataBase>(job, sex);
            float offsety = 1.5f;
            float distance = 1.7f;
            float rotateX = 0f;
            if (roleSelectData != null)
            {
                offsety = roleSelectData.offsetY * 0.01f;
                distance = roleSelectData.distance * 0.01f;
                rotateX = roleSelectData.diaRotateX * 0.01f;
            }

            m_RTObj.SetCamera(new Vector3(0f, offsety, 0), new Vector3(rotateX, 0, 0), distance);
            //m_RTObj.SetCamera(new Vector3(0f, offsety, 0), Vector3.zero, distance);
        }
        else
        {
            float offsety = 1.0f;
            float distance = 1.7f;
            float rotateX = 0f;
            if (npcDb != null)
            {
                offsety = npcDb.diaOffsetY * 0.01f;
                distance = npcDb.diaDistance * 0.01f;
                rotateX = npcDb.diaRotateX * 0.01f;
            }

            m_RTObj.SetCamera(new Vector3(0f, offsety, 0), new Vector3(rotateX, 0, 0), distance);
            //m_RTObj.SetCamera(new Vector3(0f, offsety, 0), Vector3.zero, distance);
        }
        rotateY = 170f;
        //设置人物旋转
        m_RTObj.SetModelRotateY(rotateY);
        m_RTObj.PlayModelAni(Client.EntityAction.Stand);

        //人物
        if (m__NpcTexture != null)
        {
            m__NpcTexture.mainTexture = m_RTObj.GetTexture();
            //m__NpcTexture.MakePixelPerfect();
        }
    }

    void OnDragModel(GameObject go, Vector2 delta)
    {
        if (m_RTObj != null)
        {
            rotateY = m_RTObj.YAngle - 0.5f * delta.x;
            m_RTObj.SetModelRotateY(rotateY);
        }
    }

    bool ShowTalkStr(int nindex)
    {
        bool success = false;

        if (m_dialogInfo != null)
        {
            if (m_dialogInfo.buttons != null)
            {
                if (m_dialogInfo.buttons.Count < 2)
                {
                    StopAllCoroutines();
                    //倒计时
                    tempTime = 0;
                    CLOSETIME = CONST_TASK_CD;
                    m_label_LabelNext.gameObject.SetActive(true);
                    m_label_LabelNext.text = ((int)CLOSETIME).ToString() + "秒后自动跳过";
                    StartCoroutine(WaitToClose());
                }
                else
                {
                    m_label_LabelNext.gameObject.SetActive(false);
                }
            }          
        }
      

        ResetDes();
        if (m_dialogInfo != null)
        {
            if (m_dialogInfo.lstTalks != null)
            {
                if (nindex < m_dialogInfo.lstTalks.Count)
                {
                    LangTalkData.Talk talkInfo = m_dialogInfo.lstTalks[nindex];

                    if (talkInfo.bUser)
                    {
                        string name = "";
                        if(Client.ClientGlobal.Instance().MainPlayer != null)
                        {
                            name =  Client.ClientGlobal.Instance().MainPlayer.GetName();
                        }
                        m_label_nameLabel.text = name;
                        ShowRoleTexture(true);
                    }
                    else
                    {
                        table.NpcDataBase npcdata = GameTableManager.Instance.GetTableItem<table.NpcDataBase>(m_dialogInfo.nNpcId);
                        if (npcdata != null)
                        {
                            ShowRoleTexture(false, npcdata);

                            m_label_nameLabel.text = npcdata.strName;
                        }
                    }

                    bool useRichText = false;//富文本有泄漏bug 暂时不用
                    if (useRichText)
                    {
                        string desc = string.Format("<size value=\"24\"><color value=\"#1c2850\">{0} </color></size>", talkInfo.strText);
                        if (uiXmlRichText != null)
                        {
                            this.uiXmlRichText.fontSize = 24;
                            this.uiXmlRichText.AddXml(RichXmlHelper.RichXmlAdapt(desc));
                        }
                    }
                    else
                    {
                        string desc = string.Format("[1c2850]{0}[-]", talkInfo.strText);
                        m_label_normalText.text = desc;
                    }

                    success = true;
                }
            }
  
            if (m_dialogInfo.talkVoice != null && nindex < m_dialogInfo.talkVoice.Length)
            {
                Engine.IAudio audio = Engine.RareEngine.Instance().GetAudio();
                if (audio != null && m_nPlayingAudioId != 0)
                {
                    audio.StopEffect(m_nPlayingAudioId);
                }
                table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(m_dialogInfo.talkVoice[nindex]);
                if (resDB == null)
                {
                    Engine.Utility.Log.Error("找不到选择角色的Mp3资源");
                }

                if (audio != null && resDB != null)
                {
                    //m_nPlayingAudioId = audio.PlayUIEffect(resDB.strPath);
                    Client.IPlayer mainPlayer = Client.ClientGlobal.Instance().MainPlayer;
                    if (mainPlayer != null)
                    {
                        Transform tf = mainPlayer.GetTransForm();
                        if (tf != null)
                        {
                            m_nPlayingAudioId = audio.PlayEffect(tf.gameObject, resDB.strPath, false, true);
                        }
                    }
                  
                }
            }
        }

        //是否显示下一个
        //bool nextVisible = (null != m_dialogInfo
        //    && null != m_dialogInfo.lstTalks
        //    && (m_dialogInfo.lstTalks.Count > nindex + 1));
        //if (null != m_label_LabelNext && m_label_LabelNext.gameObject.activeSelf != nextVisible)
        //{
        //    m_label_LabelNext.gameObject.SetActive(nextVisible);
        //}

        return success;
    }

    void ResetDes()
    {
        uiXmlRichText.Clear();
    }

    private bool DoNextTalk()
    {
        bool success = false;
        m_nindex++;
        if (m_dialogInfo != null && m_nindex < m_dialogInfo.lstTalks.Count)
        {
            success = ShowTalkStr(m_nindex);
        }
        //else
        //{
        //    JumpEnd();
        //}
        return success;
    }
    void OnClickScreen(GameObject go)
    {
        if (!DoNextTalk()
            && null != m_dialogInfo
            && null != m_dialogInfo.buttons && m_dialogInfo.buttons.Count <= 1)
        {
            JumpEnd();
        }
    }

    void onClick_Btn_jump_Btn(GameObject caster)
    {
        DoJump();
    }

    /// <summary>
    /// 执行跳过
    /// </summary>
    private void DoJump()
    {
        if (m_dialogInfo != null)
        {
            if (m_dialogInfo.talkVoice != null && m_dialogInfo.lstTalks.Count == m_dialogInfo.talkVoice.Length)
            {
                if (m_nindex == m_dialogInfo.talkVoice.Length - 1)
                {
                    JumpEnd();
                    return;
                }
                Engine.IAudio audio = Engine.RareEngine.Instance().GetAudio();
                if (audio != null && m_nPlayingAudioId != 0)
                {
                    audio.StopEffect(m_nPlayingAudioId);
                }
                table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(m_dialogInfo.talkVoice[m_dialogInfo.lstTalks.Count - 1]);
                if (resDB == null)
                {
                    Engine.Utility.Log.Error("找不到选择角色的Mp3资源");
                }

                if (audio != null && resDB != null)
                {
                    //m_nPlayingAudioId = audio.PlayUIEffect(resDB.strPath);
                    Client.IPlayer mainPlayer = Client.ClientGlobal.Instance().MainPlayer;
                    Transform tf = mainPlayer.GetTransForm();
                    if (tf != null)
                    {
                        m_nPlayingAudioId = audio.PlayEffect(tf.gameObject, resDB.strPath, false, true);
                    }
                }
            }
        }

        JumpEnd();
    }

    private void JumpEnd()
    {
        StopAllCoroutines();
        if (m_dialogInfo != null)
        {
            HideSelf();
            //d多选项不做任何处理
            if (m_dialogInfo.buttons != null)
            {
                if (m_dialogInfo.buttons.Count >= 2)
                {
                    return;
                }
                else
                {
                    if (m_dialogInfo.npcType == LangTalkData.NpcType.TalkOnly)
                    {
                        NetService.Instance.Send(new GameCmd.stDialogSelectScriptUserCmd_C()
                        {
                            step = m_dialogInfo.strStep,
                            dwChose = 1,
                        });
                    }
                    else
                    {
                        if (this.m_dialogInfo.nTaskId == 0)
                        {
                            return;
                        }

                        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.MissionMessagePanel, data: m_dialogInfo);
                    }
                }
            }
        }
    }

    float tempTime = 0;
    IEnumerator WaitToClose()
    {
        if (m_dialogInfo != null)
        {
            while (m_nindex < m_dialogInfo.lstTalks.Count)
            {
                for (CLOSETIME = CONST_TASK_CD; CLOSETIME > 0; CLOSETIME -= Time.deltaTime)
                {
                    tempTime += Time.deltaTime;
                    if (tempTime >= 0.9)
                    {
                        int leftTime = (int)CLOSETIME;
                        m_label_LabelNext.text = leftTime.ToString() + "秒后自动跳过";

                        tempTime = 0;
                    }

                    yield return 0;
                }

                //yield return new WaitForSeconds(CLOSETIME);

                //测试   暂时注销
                if (!DoNextTalk())
                {
                    JumpEnd();
                }

            }
        }
    }
}
