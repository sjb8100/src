using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;
using GameCmd;
using Engine.Utility;
using Engine;
using table;

public partial class CreateRolePanel : UIPanelBase
{

    public enmCharSex m_sel_sex = enmCharSex.MALE;
    public CountryName m_sel_country = CountryName.Fire;
    //用户输入名称
    private string m_str_inputName = "";
    Client.Avater m_Avater = null;
    private bool m_bLockRotate = true;


    float rotateY = 0f;
    private enumProfession[] m_professionList = new[]
	{ 
		enumProfession.Profession_Soldier, 
		enumProfession.Profession_Spy, 
		enumProfession.Profession_Freeman, 
		enumProfession.Profession_Doctor,
	};

    //Camera Animator 
    private int m_icameraHash= Animator.StringToHash("Base Layer.camera");
    private int m_istateChangeParamHash = Animator.StringToHash("Job");
    private int m_iTriggerStartHash = Animator.StringToHash("StartTrigger");


    private List<table.ShowEffect> m_ShowEffect = new List<table.ShowEffect>();

    private List<Engine.IEffect> m_CurShowEffect = new List<Engine.IEffect>();


    List<table.RandomNameDataBase> m_NamePrefix;
    List<table.RandomNameDataBase> m_NameMale;
    List<table.RandomNameDataBase> m_NameFemale;

    enum IS_WEAPON_EFFECT
    {
        SCENE = 0,
        BODY,
    }
    private void InitAnimator()
    {
        if (null != m_cam_animator)
        {
            return;
        }
        m_cam_animator = m_camera_node.GetComponent<Animator>();
        //AnimatorEventHelper helper = m_camera_node.GetComponent<AnimatorEventHelper>();
        //if (null == helper)
        //{
        //    helper = m_camera_node.gameObject.AddComponent<AnimatorEventHelper>();
        //}
        //if (null == helper)
        //{
        //    return ;
        //}
        //helper.RegisterStringCompleteCallback(AnimationComplete);

        //AnimationClip[] clips = m_cam_animator.runtimeAnimatorController.animationClips;
        //if (null != clips)
        //{
        //    AnimationEvent animEvent = null;
        //    AnimationClip clip;
        //    for(int i = 0;i < clips.Length;i++)
        //    {
        //        clip = clips[i];
        //        if (null == clip)
        //        {
        //            continue;
        //        }
        //        animEvent = new AnimationEvent();
        //        animEvent.messageOptions = SendMessageOptions.RequireReceiver;
        //        animEvent.functionName = AnimatorEventHelper.ANIMAION_COMPLETE_CALL_FUC_NAME;
        //        animEvent.stringParameter = "anim";
        //        animEvent.time = clip.length;
        //        clip.AddEvent(animEvent);
        //    }
        //}
    }

    public void ResetCameraAnimator()
    {
        if (null != m_cam_animator)
        {
            m_cam_animator.SetInteger(m_istateChangeParamHash, 0);
            m_cam_animator.SetTrigger(m_iTriggerStartHash);
        }
    }

    /// <summary>
    /// 生成动画名称
    /// </summary>
    /// <param name="profess"></param>
    /// <returns></returns>
    private string BuildAnimFullName(enumProfession profess)
    {
        return string.Format("Base Layer.{0}",profess.ToString());
    }

    /// <summary>
    /// 动画完成
    /// </summary>
    /// <param name="param"></param>
    private void AnimationComplete(string param)
    {
        if (null != m_cam_animator)
        {
            //AnimatorStateInfo state = m_cam_animator.GetCurrentAnimatorStateInfo(0);
            //if (state.fullPathHash != m_icameraHash)
            //{
            //    m_cam_animator.SetInteger(m_istateChangeParamHash, 0);
            //    m_cam_animator.SetTrigger(m_iTriggerStartHash);
            //}
        }
    }
    private Dictionary<GameCmd.enumProfession, UICreateRoleGrid> m_dicGrid = null;
    private GameCmd.enumProfession m_curPro = GameCmd.enumProfession.Profession_None;
    private GameCmd.enumProfession m_prePro = GameCmd.enumProfession.Profession_None;

    private Dictionary<GameCmd.enumProfession, GameObject> m_dicProDes = null;
    private TweenAlpha m_rightDesTA = null;
    private void InitCharacterGrid()
    {
        m_dicGrid = new Dictionary<enumProfession, UICreateRoleGrid>();
        m_dicProDes = new Dictionary<enumProfession,GameObject>();
        if (null == m_trans_UICreateRoleGrid)
            return;
        Transform ts = null;
        UICreateRoleGrid roleGrid = null;
        GameObject cloneObj = null;
        Transform tempTs = null;
        m_rightDesTA = m_widget_RightDesWiget.GetComponent<TweenAlpha>();
        for (int i = 0, max = m_professionList.Length; i<max ; i++)
        {
            ts = m_trans_career_list.Find(m_professionList[i].ToString());
            if (null != ts)
            {
                tempTs = GameObject.Instantiate(m_trans_UICreateRoleGrid);// UIManager.GetObj(GridID.Uicreaterolegrid);
                if (null == tempTs)
                    continue;
                Util.AddChildToTarget(ts, tempTs);
                cloneObj = tempTs.gameObject;
                if (null != cloneObj)
                {
                    roleGrid = cloneObj.GetComponent<UICreateRoleGrid>();
                    if (null == roleGrid)
                    {
                        roleGrid = cloneObj.AddComponent<UICreateRoleGrid>();
                    }
                }

                roleGrid.RegisterUIEventDelegate(OnCreateRoleGridClick);
                roleGrid.SetGridInfo(m_professionList[i]);
                m_dicGrid.Add(m_professionList[i], roleGrid);
            }

            ts = m_widget_RightDesWiget.cachedTransform.Find(m_professionList[i].ToString());
            if (null != ts)
            {
                m_dicProDes.Add(m_professionList[i],ts.gameObject);
            }
        }
    }

    private void ResetCharacterCreate()
    {
        if (null != m_rightDesTA)
        {
            m_rightDesTA.ResetToBeginning();
        }
        if (m_curPro != enumProfession.Profession_None)
        {
            GetGridByPro(m_curPro).SetSelect(false, false);
            GetPropDesContent(m_curPro).SetActive(false);
        }
        m_curPro = enumProfession.Profession_None;
        m_prePro = enumProfession.Profession_None;
    }

    private UICreateRoleGrid GetGridByPro(GameCmd.enumProfession pro)
    {
        if (null != m_dicGrid && m_dicGrid.ContainsKey(pro))
        {
            return m_dicGrid[pro];
        }
        return null;
    }

    private GameObject GetPropDesContent(GameCmd.enumProfession pro)
    {
        if (m_dicProDes.ContainsKey(pro))
        {
            return m_dicProDes[pro];
        }
        return null;
    }


    protected override void OnLoading()
    {
        //m_NamePrefix = table.RandomNameDataBase.PrefixList();
        //m_NameMale = table.RandomNameDataBase.MaleList();
        //m_NameFemale = table.RandomNameDataBase.FemaleList();

        base.OnLoading();
        if (null != m_input_input_name)
        {
            m_input_input_name.onChange.Add(new EventDelegate(() =>
            {
                m_str_inputName = TextManager.GetTextByWordsCountLimitInUnicode(m_input_input_name.value
                    , TextManager.CONST_NAME_MAX_WORDS);
                m_input_input_name.value = m_str_inputName;
            }));
            m_input_input_name.onSubmit.Add(new EventDelegate(() =>
            {
                m_str_inputName = TextManager.GetTextByWordsCountLimitInUnicode(m_input_input_name.value
                     , TextManager.CONST_NAME_MAX_WORDS);
                m_input_input_name.value = m_str_inputName;
            }));
            m_input_input_name.defaultText = string.Empty;
        }

        UIEventListener.Get(m_widget_Container.gameObject).onDrag = OnDragModel;

        //特效表加载
        List<table.ShowEffect> tableList = GameTableManager.Instance.GetTableList<table.ShowEffect>();
        for (int i = 0; i < tableList.Count; i++)
        {
            table.ShowEffect data = tableList[i];
            m_ShowEffect.Add(data);
        }

        InitCharacterGrid();

    }

    void OnCreateRoleGridClick(UIEventType eventType,object data,object param)
    {
        if (eventType == UIEventType.Click)
        {
            if (null != data && data is UICreateRoleGrid)
            {
                UICreateRoleGrid cGrid = data as UICreateRoleGrid;
                if (cGrid.Pro != m_curPro)
                {
                    OnSelChanged(cGrid.Pro, m_sel_sex);
                }
            }
        }
    }
    void OnDragModel(GameObject go, UnityEngine.Vector2 delta)
    {
        if (m_Avater != null && m_bLockRotate == false)
        {
            rotateY = m_Avater.RenderObj.GetNode().GetLocalRotate().eulerAngles.y - 0.5f * delta.x;
            Quaternion rotate = new Quaternion();
            rotate.eulerAngles = new Vector3(0, rotateY, 0);
            m_Avater.RenderObj.GetNode().SetLocalRotate(rotate);
        }
    }
    void OnSelect(enumProfession profession)
    {
        OnSelChanged(profession, m_sel_sex);
    }

    protected override void OnShow(object data = null)
    {
        mbAnimEnable = true;
        StepManager.Instance.OnBeginStep(Step.CREATE_ROLE);    
        LoadRoleModel();

        //Engine.IAudio audio = Engine.RareEngine.Instance().GetAudio();
        //if (audio != null)
        //{
        //    audio.StopMusic();
        //}

        //table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(40001);
        //if (resDB == null)
        //{
        //    Engine.Utility.Log.Error("找不到选择角色的Mp3资源");
        //}

        //if (audio != null && resDB != null)
        //{
        //    audio.PlayMusic(resDB.strPath);
        //}
    }

    protected override void OnHide()
    {
        ResetCameraAnimator();
        ResetCharacterCreate();
        if(m_scene_obj)
        {
            //m_scene_obj.gameObject.SetActive(false);
           // Destroy(m_scene_obj);
            if (m_Avater != null)
            {
                m_Avater.Destroy();
                m_Avater = null;
            }

            Resources.UnloadUnusedAssets();
        }
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_dicGrid)
        {
            var iter = m_dicGrid.GetEnumerator();
            uint id = (uint)GridID.Uicreaterolegrid;
            while(iter.MoveNext())
            {
                if (null == iter.Current.Value)
                    continue;
                UIManager.ReleaseObjs(id, iter.Current.Value.CacheTransform);
            }
            m_dicGrid.Clear();
            m_dicGrid = null;
        }
    }

    void onClick_Random_name_Btn(GameObject caster)
    {
        
        var name = string.Empty;
        //if (m_sel_sex == enmCharSex.MALE)
        //{

        //    name = string.Format("{0}{1}", m_NamePrefix.Random().namePrefix, m_NameMale.Random().maleName);
        //}
        //else
        //{
        //    var nameFemale = table.RandomNameDataBase.FemaleList();
        //    name = string.Format("{0}{1}", m_NamePrefix.Random().namePrefix, m_NameFemale.Random().femaleName);
        //}
        name = DataManager.Manager<LoginDataManager>().GetRandomName(m_sel_sex);
        if (m_input_input_name != null)
        {
            m_input_input_name.value = name;
        }
    }

    void onClick_Create_Btn(GameObject caster)
    {
        if (!DataManager.Manager<TextManager>().IsLegalNameFormat(
            DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_TXT_Person)
            ,m_str_inputName
            ,TextManager.CONST_NAME_MIN_WORDS
            ,TextManager.CONST_NAME_MAX_WORDS
            ,true))
        {
            return;
        }
        Log.Info("检查角色名的合法性");
        NetService.Instance.Send(new GameCmd.stCheckNameSelectUserCmd() { name = TextManager.RemoveAllSpace(m_str_inputName) });
    }

    public void OnRetCheckName(string char_name, uint err_code)
    {
        if (err_code == 0)
        {
            //服务器要求随机
            m_sel_country = (CountryName)UnityEngine.Random.Range((int)CountryName.Sky, (int)CountryName.Fire + 1);
            
            ushort wdFace = (ushort)(m_sel_sex == enmCharSex.MALE ? UnityEngine.Random.Range(0, 100) : UnityEngine.Random.Range(101, 200));
            DataManager.Instance.Sender.CreateSelectUser(char_name, (byte)m_curPro, (byte)m_sel_country,wdFace,"");
        }
        else
        {
            //0 没有错误 1 名字重复 2 名字包含不合法的内容
            string tips = "";
            TextManager tmgr = DataManager.Manager<TextManager>();
            if (err_code == 1)
            {
                tips = tmgr.GetLocalText(LocalTextType.Local_TXT_Warning_NameExist);
            }else
            {
                tips = tmgr.GetLocalFormatText(LocalTextType.Local_TXT_Warning_FM_IllegalChar,
                    tmgr.GetLocalText(LocalTextType.Local_TXT_Name));
            }
            TipsManager.Instance.ShowTips(tips);
            //m_input_input_name.value = string.Empty;
            //m_input_input_name.isSelected = true;
        }
    }

    void OnSelChanged(enumProfession prof, enmCharSex sex, bool force=false)
    {
        if (m_curPro == prof && m_sel_sex == sex && force == false)
        {
            //没有改变
            return;
        }
        m_bLockRotate = true;
        bool havePre = m_curPro != enumProfession.Profession_None;
        GetGridByPro(prof).SetSelect(true,havePre);
        GetPropDesContent(prof).SetActive(true);
        if (havePre)
        {
            if (null != m_rightDesTA)
            {
                m_rightDesTA.ResetToBeginning();
                m_rightDesTA.Play(true);
            }
            GetGridByPro(m_curPro).SetSelect(false);
            GetPropDesContent(m_curPro).SetActive(false);
        }
        else
        {
            if (m_rightDesTA != null )
            {
                m_rightDesTA.GetComponent<UIWidget>().alpha = m_rightDesTA.to;
            }
        }
        m_prePro = m_curPro;
        m_curPro = prof;
        if(m_curPro == enumProfession.Profession_Soldier || m_curPro == enumProfession.Profession_Freeman)
        {
            sex = enmCharSex.MALE;
        }
        else
        {
            sex = enmCharSex.FEMALE;
        }
        m_sel_sex = sex;
        //for (int i = 0; i < m_lstCreateRoleBtn.Count; i++)
        //{
        //    m_lstCreateRoleBtn[i].ToggleSelectMask(m_lstCreateRoleBtn[i].m_enumProfession == m_sel_profession);
        //}

        if (m_char_obj !=  null)
        {
            GameObject.Destroy(m_char_obj);
        }

        //if(m_sel_sex == enmCharSex.MALE)
        //{
        //    m_sprite_male_bg.spriteName = "nan_liang";
        //    m_sprite_female_bg.spriteName = "nv_an";
        //}
        //else
        //{
        //    m_sprite_male_bg.spriteName = "nan_an";
        //    m_sprite_female_bg.spriteName = "nv_liang";
        //}

        
        //随机一个名字
        onClick_Random_name_Btn(null);

        //if(m_sel_sex == enmCharSex.MALE)
        //    StartCoroutine(LoadModel("ZS_Male_Show"));
        //else
        //    StartCoroutine(LoadModel("ZS_Female_Show"));

        var t = Table.Query<table.SelectRoleDataBase>();
        table.SelectRoleDataBase item = null;
        for (int i = 0; i < t.Count; i++)
        {
            if (t[i].professionID == (uint)m_curPro && t[i].Sex == m_sel_sex)
            {
                item = t[i];
                break;
            }
        }
       // var item = table.SelectRoleDataBase.Where(m_sel_profession, m_sel_sex);

        if (item == null || (item != null && string.IsNullOrEmpty(item.bodyPathRoleShowcase)))
        {
            Log.Error("角色展示模型未配置,请检查选人界面表!");
            return;
        }


        if (m_Avater != null)
        {
            RenderObjHelper.EndDissolveEffect(m_Avater.RenderObj);
        }
        if (m_Avater != null)
        {
            m_Avater.Destroy();
            m_Avater = null;
        }
        
        if (m_model_node == null)
        {
            Engine.Utility.Log.Error("m_model_node 为 null !!!");
            return;
        }

        m_model_node.transform.DestroyChildren();
        rotateY = m_model_node.localRotation.y;

        //var table_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)item.createRoleResID);

       
        //创建角色，模型自带武器
        var ritem = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>((uint)item.createRoleResID);
        if (ritem == null)
        {
            Engine.Utility.Log.Error("ritem 为 null !!!");
            return;
        }

        Client.AvatarUtil.CreateAvater(ref m_Avater, ritem.strPath, m_model_node, m_model_node.gameObject.layer, OnCreateAvater, (int)prof);

        Engine.IRenderSystem renderSys = Engine.RareEngine.Instance().GetRenderSystem();
        if (renderSys == null)
        {
            return;
        }         

        for (int i = m_CurShowEffect.Count - 1; i >= 0; i--)
        {
            Engine.IEffect effect = m_CurShowEffect[i];
            renderSys.RemoveEffect(effect);
        }
        m_CurShowEffect.Clear();
    }
    void OnCreateAvater(object param)
    {
        if (null == m_Avater ||null == m_Avater.RenderObj)
        {
            Engine.Utility.Log.Error("CreateRolePanel->OnCreateAvater failed,m_Avater or m_Avater.RnederObj null!");
            return;
        }
        m_Avater.RenderObj.ClearFrameEvent();
        int nProfession = (int)param;
        PlayCreateAudio(nProfession);
        switch (nProfession)
        {
            case (int)enumProfession.Profession_Soldier:
                {
                    for (int i = 0; i < m_ShowEffect.Count; i++)
                    {
                        table.ShowEffect data = m_ShowEffect[i];
                        if (data.Profession == (uint)enumProfession.Profession_Soldier)
                        {
                            switch (data.IsWeaponEffect)
                            {
                                case (uint)IS_WEAPON_EFFECT.SCENE:
                                    m_Avater.RenderObj.AddFrameEvent(Client.EntityAction.ShowLog, "addEffect_scene", data.StartTime, uint.Parse(data.EffectID));
                                    break;
                                case (uint)IS_WEAPON_EFFECT.BODY:
                                    m_Avater.RenderObj.AddFrameEvent(Client.EntityAction.ShowLog, "addEffect_body", data.StartTime, uint.Parse(data.EffectID));
                                    break;
                            }

                        }
                    }
                    AnimationState ani = m_Avater.RenderObj.GetAnimationState(Client.EntityAction.ShowLog);
                    m_Avater.RenderObj.AddFrameEvent(Client.EntityAction.ShowLog, "standAni", ani.length*0.9f);



                    //wbw 测试



                }
                break;
            case (int)enumProfession.Profession_Spy:
                {
                    for (int i = 0; i < m_ShowEffect.Count; i++)
                    {
                        table.ShowEffect data = m_ShowEffect[i];
                        if (data.Profession == (uint)enumProfession.Profession_Spy)
                        {
                            switch (data.IsWeaponEffect)
                            {
                                case (uint)IS_WEAPON_EFFECT.SCENE:
                                    m_Avater.RenderObj.AddFrameEvent(Client.EntityAction.ShowLog, "addEffect_scene", data.StartTime, uint.Parse(data.EffectID));
                                    break;
                                case (uint)IS_WEAPON_EFFECT.BODY:
                                    m_Avater.RenderObj.AddFrameEvent(Client.EntityAction.ShowLog, "addEffect_body", data.StartTime, uint.Parse(data.EffectID));
                                    break;
                            }
                        }
                    }

                    AnimationState ani = m_Avater.RenderObj.GetAnimationState(Client.EntityAction.ShowLog);
                    m_Avater.RenderObj.AddFrameEvent(Client.EntityAction.ShowLog, "standAni", ani.length * 0.9f);


                }
                break;
            case (int)enumProfession.Profession_Freeman:
                {
                    for (int i = 0; i < m_ShowEffect.Count; i++)
                    {
                        table.ShowEffect data = m_ShowEffect[i];
                        if (data.Profession == (uint)enumProfession.Profession_Freeman)
                        {
                            switch (data.IsWeaponEffect)
                            {
                                case (uint)IS_WEAPON_EFFECT.SCENE:
                                    m_Avater.RenderObj.AddFrameEvent(Client.EntityAction.ShowLog, "addEffect_scene", data.StartTime, uint.Parse(data.EffectID));
                                    break;
                                case (uint)IS_WEAPON_EFFECT.BODY:
                                    m_Avater.RenderObj.AddFrameEvent(Client.EntityAction.ShowLog, "addEffect_body", data.StartTime, uint.Parse(data.EffectID));
                                    break;
                            }
                        }
                    }

                    AnimationState ani = m_Avater.RenderObj.GetAnimationState(Client.EntityAction.ShowLog);
                    m_Avater.RenderObj.AddFrameEvent(Client.EntityAction.ShowLog, "standAni", ani.length * 0.9f);

                    //溶解
                    m_Avater.RenderObj.AddFrameEvent(Client.EntityAction.ShowLog, "Dissolve", 0);
                }
                break;
            case (int)enumProfession.Profession_Doctor:
                {
                    for (int i = 0; i < m_ShowEffect.Count; i++)
                    {
                        table.ShowEffect data = m_ShowEffect[i];
                        if (data.Profession == (uint)enumProfession.Profession_Doctor)
                        {

                            switch (data.IsWeaponEffect)
                            {
                                case (uint)IS_WEAPON_EFFECT.SCENE:
                                    m_Avater.RenderObj.AddFrameEvent(Client.EntityAction.ShowLog, "addEffect_scene", data.StartTime, uint.Parse(data.EffectID));
                                    break;
                                case (uint)IS_WEAPON_EFFECT.BODY:
                                    m_Avater.RenderObj.AddFrameEvent(Client.EntityAction.ShowLog, "addEffect_body", data.StartTime, uint.Parse(data.EffectID));
                                    break;
                            }
                        }
                    }
                    AnimationState ani = m_Avater.RenderObj.GetAnimationState(Client.EntityAction.ShowLog);
                    m_Avater.RenderObj.AddFrameEvent(Client.EntityAction.ShowLog, "standAni", ani.length * 0.9f);

                    //溶解
                    m_Avater.RenderObj.AddFrameEvent(Client.EntityAction.ShowLog, "Dissolve", 0);


                }
                break;
        }
       
        if (m_cam_animator != null)
        {
            int nJob = (int)param;
            m_cam_animator.SetTrigger(m_iTriggerStartHash);
            m_cam_animator.SetInteger(m_istateChangeParamHash , nJob);
        }
        m_Avater.PlayAni(Client.EntityAction.ShowLog, OnActionEvent,0.3f, -1);

    }
    uint m_uAudioID = 0;
    void PlayCreateAudio(int profession)
    {
        Engine.IAudio audio = Engine.RareEngine.Instance().GetAudio();
        if (audio == null)
        {
            return;
        }
        audio.StopEffect(m_uAudioID);
        float originVol = 1;
        SelectRoleDataBase sdb = SelectRoleDataBase.Where((enumProfession)profession,(enmCharSex)1);
        if(sdb != null)
        {
            uint audioID = sdb.createAudioID;
            table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(audioID);
      
            if (audio != null && resDB != null)
            {
                if (m_audioSource != null)
                {
                    m_audioSource.volume = 0.2f;
                }
                if(m_Avater != null)
                {
                    if(m_Avater.RenderObj != null)
                    {
                        if(m_Avater.RenderObj.GetNode() != null)
                        {
                            if(m_Avater.RenderObj.GetNode().GetTransForm() != null)
                            {
                                m_uAudioID = audio.PlayEffect(m_Avater.RenderObj.GetNode().GetTransForm().gameObject, resDB.strPath, endCallback: () => {
                                    if(m_audioSource != null)
                                    {
                                        m_audioSource.volume = originVol;
                                    }
                                });
                            }
                        }
                    }
                }
               
            }
        }

    }
    void OnActionEvent(ref string strEventName, ref string strAnimationName, float time, object param)
    {
        if (strEventName == "addEffect_scene" && strAnimationName == Client.EntityAction.ShowLog)
        {
            uint nEffectID = (uint)param;
            table.FxResDataBase edb = GameTableManager.Instance.GetTableItem<table.FxResDataBase>(nEffectID);
            if (edb != null)
            {
                table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(edb.resPath);
                if (resDB != null)
                {
                    string strPath = resDB.strPath;
                    string strAttachNode = edb.attachNode;
                    List<float> rotate = edb.rotate;
                    //m_Avater.RenderObj.AddLinkEffect(ref strPath, ref strAttachNode,
                    //    new Vector3(edb.offset[0], edb.offset[1], edb.offset[2]),
                    //    Quaternion.identity,
                    //    Vector3.one);
                    Engine.IRenderSystem renderSys = Engine.RareEngine.Instance().GetRenderSystem();
                    Engine.IEffect effect = null;

                    Vector3 vecRotate = new Vector3(rotate[0], rotate[1], rotate[2]);
                    UnityEngine.Quaternion q = new Quaternion();
                    q.eulerAngles = vecRotate;
                    renderSys.CreateEffect(ref strPath, ref effect, null, Engine.TaskPriority.TaskPriority_Normal, true);
                    effect.GetNode().SetLocalRotate(q);

                    Vector3 pos = new Vector3(edb.offset[0], edb.offset[1], edb.offset[2]);
                    effect.GetNode().SetLocalPosition(pos);

                    m_CurShowEffect.Add(effect);
                }

            }

        }
        else if (strEventName == "addEffect_body" && strAnimationName == Client.EntityAction.ShowLog)
        {
            AssetBundle m_AssetBundle;

            

            uint nEffectID = (uint)param;
            table.FxResDataBase edb = GameTableManager.Instance.GetTableItem<table.FxResDataBase>(nEffectID);
            if (edb != null)
            {
                table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(edb.resPath);
                if (resDB != null)
                {
                    string strPath = resDB.strPath;
                    string strAttachNode = edb.attachNode;
                    List<float> rotate = edb.rotate;

                    Vector3 vecRotate = new Vector3(rotate[0], rotate[1], rotate[2]);
                    UnityEngine.Quaternion q = new Quaternion();
                    q.eulerAngles = vecRotate;

                    m_Avater.RenderObj.AddLinkEffect(ref strPath, ref strAttachNode,
                        new Vector3(edb.offset[0], edb.offset[1], edb.offset[2]),
                        q,
                        Vector3.one);

                }

            }
        }


        if (strEventName == "standAni" && strAnimationName == Client.EntityAction.ShowLog)
        {
            m_bLockRotate = false;
            m_Avater.PlayAni(Client.EntityAction.Show, null, 0.3f, -1);
        }

        if (strEventName == "Dissolve" && strAnimationName == Client.EntityAction.ShowLog)
        {
            RenderObjHelper.BeginDissolveEffect(m_Avater.RenderObj);
        }

        //if(strEventName=="end" && strAnimationName == Client.EntityAction.ShowLog)
        //{
        //    m_bLockRotate = false;
        //    if (null != m_cam_animator)
        //    {
        //        m_cam_animator.SetInteger(m_istateChangeParamHash, 0);
        //    }
            //m_Avater.PlayAni(Client.EntityAction.Show, null, 0.3f, -1);
        //}
    }

    void onClick_Return_Btn(GameObject caster)
    {
        Engine.IRenderSystem renderSys = Engine.RareEngine.Instance().GetRenderSystem();
        for (int i = m_CurShowEffect.Count - 1; i >= 0; i--)
        {
            Engine.IEffect effect = m_CurShowEffect[i];
            renderSys.RemoveEffect(effect);
        }
        m_CurShowEffect.Clear();

        ResetCameraAnimator();

       

        //返回选择角色界面
        if (DataManager.Manager<LoginDataManager>().RoleList.Count > 0)
            StepManager.Instance.AddLoginScene(StepManager.CHOOSEROLESCENE, UnityEngine.SceneManagement.LoadSceneMode.Additive, (obj) =>
            {
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ChooseRolePanel);
            });
        else
        {
            //StepManager.Instance.AddLoginScene(StepManager.LOGINSCENE, (obj) =>
            //{
            //    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.LoginPanel);
            //});
            DataManager.Manager<LoginDataManager>().Logout(LoginPanel.ShowUIEnum.StartGame);
        }

        HideSelf();
    }

    int index = 1;

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
        m_camera_node = m_scene_obj.transform.Find("MainCamera");
        InitAnimator();
        if (m_model_node == null)
        {
            Log.Error("创建角色场景没有model节点");
        }
        if(m_camera_node != null)
        {
            m_audioSource = m_camera_node.GetComponent<AudioSource>();
        }
        OnSelChanged(enumProfession.Profession_Soldier, enmCharSex.MALE, true);
    }

    Transform m_model_node;
    Transform m_camera_node = null;
    Animator m_cam_animator = null;
    GameObject m_scene_obj;
    GameObject m_char_obj;

    AudioSource m_audioSource = null;
    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eLoginCheckName)
        {
            GameCmd.stCheckNameSelectUserCmd cmd = (GameCmd.stCheckNameSelectUserCmd)param;
            OnRetCheckName(cmd.name, cmd.err_code);
        }

        return true;
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


