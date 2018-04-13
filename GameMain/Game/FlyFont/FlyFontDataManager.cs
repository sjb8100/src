using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using table;
using GameCmd;
using Engine;
using Engine.Utility;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine.Profiling;
public enum DamageType
{
    None = 0,
    Normal = 1,
    Critical = 2,   // 暴击
    AddHp,
    AddExp,
    Buff,       // buff伤害
    Resist,     //抵抗
    Doge,       // 闪避

}
class FlyFontDataManager : SingletonMono<FlyFontDataManager>
{
    /// <summary>
    /// 是否显示其他的伤害飘字
    /// </summary>
    public bool m_bShowFlyFont = false;

    Dictionary<int, ObjectPool<FlyFont>> flyPoolDic = new Dictionary<int, ObjectPool<FlyFont>>();
    /// <summary>
    /// 正在使用的飘字
    /// </summary>
    Dictionary<int, List<FlyFont>> m_usingFontDic = new Dictionary<int, List<FlyFont>>();
    /// <summary>
    /// 伤害点临时位置对象池
    /// </summary>
    ObjPool<GameObject> m_posRootPool = new ObjPool<GameObject>();

    // List<FlyFont> m_flyUseList = new List<FlyFont>();
    static string m_strLocatorName = "FlyFont";
    static string m_strFlyroot = "flyroot";
    Transform m_flyRoot = null;
     public int MAX_FONTCOUNT = 8;
    void Awake()
    {
        GameObject flyrootobj = new GameObject(m_strFlyroot);

        m_flyRoot = flyrootobj.transform;
        m_flyRoot.parent = this.transform;
        DOTween.Init(true, true, LogBehaviour.Verbose);
    }

    public void InitFlyFont()
    {
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SKILLSYSTEM_SHOWDAMAGE, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SKILLSYSTEM_SHOWBUFFDAMAGE, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_LEAVEMAP, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.APPLICATION_LOWMEMORY, OnEvent);
       // PreLoadFont();
    }
    Dictionary<DamageType, List<FlyFont>> m_dicFt = null;
    const int m_nNormalFontPreLoad = 20;
    const int m_nOtherFontPreLoad = 10;
    void PreLoadFont()
    {
        m_dicFt = new Dictionary<DamageType, List<FlyFont>>();
        Profiler.BeginSample("PreLoadFightFont");
        Array types = Enum.GetValues(typeof(DamageType));
        for (int i = 0; i < types.Length; i++)
        {
            DamageType dt = (DamageType)types.GetValue(i);
            if (dt == DamageType.Normal || dt == DamageType.AddHp)
            {
                for (int n = 0; n < m_nNormalFontPreLoad; n++)
                {
                    FlyFont ff = GetFlyFont(dt);
                    if (ff == null)
                    {
                        return;
                    }
                    ff.InitDamageType(dt);
                    AddFontPool(dt, ff);

                }
            }
            else if (dt == DamageType.None)
            {
                continue;
            }
            else
            {
                for (int n = 0; n < m_nOtherFontPreLoad; n++)
                {
                    FlyFont ff = GetFlyFont(dt);
                    if (ff == null)
                    {
                        return;
                    }
                    ff.InitDamageType(dt);
                    AddFontPool(dt, ff);

                }
            }
        }
        var iter = m_dicFt.GetEnumerator();
        while (iter.MoveNext())
        {
            var dic = iter.Current;
            List<FlyFont> fl = dic.Value;
            for (int i = 0; i < fl.Count; i++)
            {
                FlyFont ff = fl[i];
                ReturnFlyFont(dic.Key, ff);
            }
        }
        m_dicFt.Clear();
        m_dicFt = null;
        Profiler.EndSample();
    }
    void AddFontPool(DamageType dt, FlyFont ft)
    {
        List<FlyFont> fl = null;
        if (m_dicFt.ContainsKey(dt))
        {
            fl = m_dicFt[dt];
        }
        else
        {
            fl = new List<FlyFont>();
            m_dicFt.Add(dt, fl);
        }
        fl.Add(ft);
    }
    void OnEvent(int eventID, object param)
    {
        if (eventID == (int)Client.GameEventID.SKILLSYSTEM_SHOWDAMAGE)
        {
            stShowDemage st = (stShowDemage)param;
            bool IsMyNpc = false;//是否是其他人的宠物或者召唤物
            if ((GameCmd.SceneEntryType)st.attacktype != GameCmd.SceneEntryType.SceneEntry_Player)
            {
                IEntity attacker = EntitySystem.EntityHelper.GetEntity((GameCmd.SceneEntryType)st.attacktype, st.attackID);
                if (attacker != null)
                {
                    INPC npc = attacker as INPC;
                    if (npc != null)
                    {
                        if (npc.IsMainPlayerSlave())
                        {
                            IsMyNpc = true;
                        }
                    }
                }
            }

            uint skillid = st.skillid;
            List<GameCmd.stMultiAttackDownMagicUserCmd_S.stDefender> defenerList = st.defenerList;
            if (IsMyNpc || st.attackID == MainPlayerHelper.GetPlayerID())
            {
                //如果攻击者是自己或者自己的宠物 飘血

                for (int i = 0; i < defenerList.Count; i++)
                {
                    var info = defenerList[i];
                    EntityType type = EntitySystem.EntityHelper.GetEntityEtype(info.byDefencerType);
                    if (info.dwDamage == 0 && info.byDamType == (uint)GameCmd.AttackType.ATTACK_TYPE_FO)
                    {
                        continue;
                    }
                    ShowDamage(info.dwDamage, info.byDamType, info.dwDefencerID, type, info.dwDefencerHP, skillid, st.attackID);
                }
            }
            else
            {
                for (int i = 0; i < defenerList.Count; i++)
                {
                    var info = defenerList[i];
                    //如果受击者是自己或者自己的宠物 飘血
                    if (info.dwDamage == 0 && info.byDamType == (uint)GameCmd.AttackType.ATTACK_TYPE_FO)
                    {
                        continue;
                    }
                    if (info.dwDefencerID != MainPlayerHelper.GetPlayerID())
                    {
                        IEntity en = EntitySystem.EntityHelper.GetEntity(info.byDefencerType, info.dwDefencerID);
                        if (en != null)
                        {
                            INPC npc = en as INPC;
                            if (npc != null)
                            {
                                if (npc.IsTrap())
                                {
                                    return;
                                }
                                if (npc.IsMainPlayerSlave())
                                {//防御者是自己的宠物
                                    EntityType type = EntitySystem.EntityHelper.GetEntityEtype(info.byDefencerType);
                                    ShowDamage(info.dwDamage, info.byDamType, info.dwDefencerID, type, info.dwDefencerHP, skillid, st.attackID);
                                }
                            }
                            else
                            {
                                //怪打人 血量同步 先不同步
                                //IPlayer otherPlayer = en as IPlayer;
                                //if(otherPlayer != null)
                                //{
                                //    SetEntityCurHP(en, info.dwDefencerHP);
                                //}
                            }

                        }
                    }
                    else
                    {//防御者是自己
                        EntityType type = EntitySystem.EntityHelper.GetEntityEtype(info.byDefencerType);
                        ShowDamage(info.dwDamage, info.byDamType, info.dwDefencerID, type, info.dwDefencerHP, skillid, st.attackID);
                    }
                }
            }
        }
        else if (eventID == (int)Client.GameEventID.SKILLSYSTEM_SHOWBUFFDAMAGE)
        {
            stBuffDamage st = (stBuffDamage)param;

            ShowBuffDamage(st);

        }
        else if (eventID == (int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE)
        {
            stPropUpdate prop = (stPropUpdate)param;
            if (MainPlayerHelper.GetPlayerUID() != prop.uid)
            {
                return;
            }
            if (prop.nPropIndex == (int)PlayerProp.Exp)
            {

                int exp = prop.newValue - prop.oldValue;
                if (exp < 0)
                {
                    return;
                }

                ShowExp(exp);
            }
        }
        else if (eventID == (int)Client.GameEventID.ENTITYSYSTEM_LEAVEMAP)
        {
            OnLowMemoryWarning();
        }
        else if(eventID == (int)GameEventID.APPLICATION_LOWMEMORY)
        {
            OnLowMemoryWarning();
        }
    }
    void OnLowMemoryWarning()
    {
        ReleaseFontParent();
        ReleaseIdleFont();
    }
    void ReleaseIdleFont()
    {
        var iter = flyPoolDic.GetEnumerator();
        while(iter.MoveNext())
        {
            var pool = iter.Current.Value;
            pool.ReleasePool();
        }
    }
    void ReleaseFontParent()
    {
        int num = transform.childCount;
        List<Transform> transList = new List<Transform>();
        for (int i = 0; i < num; i++)
        {
            Transform t = transform.GetChild(i);
            if (t.name != m_strFlyroot)
            {
                if(t.childCount == 0)
                {
                    transList.Add(t);
                }
            }
        }
        for(int i = 0;i<transList.Count;i++)
        {
            Transform t = transList[i];
            DestroyImmediate(t.gameObject);
        }
        transList.Clear();
        m_posRootPool.Clear();
    }
    #region flyfont
    void ShowExp(int exp)
    {
        DamageType dt = DamageType.AddExp;
        FlyFont font = GetFlyFont(dt);
        if(font == null)
        {
            return;
        }
        font.InitDamageType(dt);
        AddFlyFont(MainPlayerHelper.GetMainPlayer(), EntityType.EntityType_Player, dt, font, exp);
    }
    FlyFont GetFlyFont(DamageType type)
    {
        int key = (int)type;
        if(!CanGetFont(key))
        {
            Log.Error("超出最大数量，不在实例化飘字");
            return null;
        }
        FlyFont ft = null;
        if (flyPoolDic.ContainsKey(key))
        {
            ObjectPool<FlyFont> pool = flyPoolDic[key];
            ft = pool.GetObject();
        }
        else
        {
            ObjectPool<FlyFont> pool = new ObjectPool<FlyFont>();
            flyPoolDic.Add(key, pool);
            ft = pool.GetObject();
        }
        AddToUsingDic(key, ft);
        return ft;
    }
    bool CanGetFont(int key)
    {
        if(m_usingFontDic.ContainsKey(key))
        {
            List<FlyFont> list = m_usingFontDic[key];
            if (list.Count > MAX_FONTCOUNT)
            {
                return false;
            }
        }
        return true;
    }
    void AddToUsingDic(int key,FlyFont ft)
    {
        if (!m_usingFontDic.ContainsKey(key))
        {
            List<FlyFont> fontList = new List<FlyFont>();
            fontList.Add(ft);
            m_usingFontDic.Add(key, fontList);
        }
        else
        {
            List<FlyFont> fontList = m_usingFontDic[key];
            fontList.Add(ft);
        }
    }
    void RemoveFormUsingDic(int key, FlyFont font)
    {
        if(m_usingFontDic.ContainsKey(key))
        {
            List<FlyFont> list = m_usingFontDic[key];
            if(list.Contains(font))
            {
                list.Remove(font);
            }
        }
    }
    public void ReturnFlyFont(DamageType type, FlyFont font)
    {
        DOTweenAnimation[] tweenArray = font.GetTweens();
        foreach(var ani in tweenArray)
        {
            if(ani.onComplete != null)
            {
                ani.onComplete.RemoveAllListeners();
            }
          
            if(ani.onStart != null)
            {
                ani.onStart.RemoveAllListeners();
            }
            if(ani.onUpdate != null)
            {
                ani.onUpdate.RemoveAllListeners();
            }
            if(ani.onPlay != null)
            {
                ani.onPlay.RemoveAllListeners();
            }
            
        }
  
        int key = (int)type;
        RemoveFormUsingDic(key, font);
        font.ReleaseTextMesh();
        ObjectPool<FlyFont> pool = flyPoolDic[key];
        if (pool.GetPoolCount() > MAX_FONTCOUNT)
        {
            Log.Error("超出最大数量，销毁飘字");
            font.ReleaseObject();
            return;
        }
        pool.ReturnObject(font);
        Transform fontTrans = font.GetFontTransform();
        if (fontTrans != null)
        {
            Transform posroot = fontTrans.parent;
            if (posroot != null)
            {
                if(posroot.name != m_strFlyroot)
                {
                    m_posRootPool.Free(posroot.gameObject);
                }
            }
            if (m_flyRoot == null)
            {
                GameObject flyrootobj = new GameObject(m_strFlyroot);

                m_flyRoot = flyrootobj.transform;
                m_flyRoot.parent = this.transform;
            }
            fontTrans.parent = m_flyRoot;
        }

    }
    public void ShowBuffDamage(stBuffDamage damage)
    {
        Client.IEntity entity = GetEntity(damage.uid);
        if (entity == null)
        {
            Engine.Utility.Log.Error("找不到实体roleid:{0}", damage.uid);
            return;
        }

        SetEntityCurHP(entity, damage.curHp);

        if (damage.uid != MainPlayerHelper.GetPlayerUID())
        {
            return;
        }

        EntityType entype = damage.entityType == (int)EntityType.EntityType_Player ? EntityType.EntityType_Player : EntityType.EntityType_NPC;

        DamageType dt = DamageType.Normal;
        if (damage.changeValue > 0)
        {
            dt = DamageType.AddHp;
        }
        if (testType != DamageType.None)
        {
            dt = testType;
        }
        if (entity != null)
        {
            if (damage.damagetype == (int)GameCmd.HPChangeType.HPChangeType_Buff || damage.damagetype == (int)GameCmd.HPChangeType.HPChangeType_Imme)
            {
                FlyFont font = GetFlyFont(dt);
                if (font == null)
                {
                    return;
                }
                font.InitDamageType(dt);
                AddFlyFont(entity, entype, dt, font, damage.changeValue);
            }
        }
    }
    void SetEntityCurHP(IEntity entity, uint curHp)
    {//除去伤害包之外的类型

        stPropUpdate prop = new stPropUpdate();
        prop.uid = entity.GetUID();
        prop.nPropIndex = (int)CreatureProp.Hp;
        prop.oldValue = entity.GetProp((int)CreatureProp.Hp);
        prop.newValue = (int)curHp;
        float maxhp = (float)entity.GetProp((int)CreatureProp.MaxHp);
        if (curHp < 0)
        {
            curHp = (uint)Mathf.CeilToInt(maxhp * 0.1f);
        }
        entity.SetProp((int)CreatureProp.Hp, (int)curHp);
        if (curHp == 0)
        {
          //  Engine.Utility.Log.LogGroup("ZDY", "cmd.curhp----------------" + curHp);
        }

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_HPUPDATE, prop);
      
    }
    public void ShowDamage(uint damageValue, uint damType, uint roleid, EntityType type, uint curHp, uint skillid = 1, uint attackerid = 0)
    {//伤害包

       // Log.LogGroup("ZDY", "show damage damagevalue is {0}", damageValue);
        Client.IEntity entity = GetEntity(roleid, type);
        if (entity == null)
        {
            Engine.Utility.Log.Error("找不到实体roleid:{0},type :{1}", roleid, type);
            return;
        }
        SetEntityCurHP(entity, curHp);
        table.SkillDatabase skillData = GameTableManager.Instance.GetTableItem<table.SkillDatabase>(skillid);
        uint damageTimes = 1;
        uint hurtinterval = 0;
        if (skillData != null)
        {
            damageTimes = skillData.damagetimes;
            hurtinterval = skillData.hurtinterval;
        }


        DamageType dt = GetDamageType(damType);
        if (testType != DamageType.None)
        {
            dt = testType;
        }
        if (entity != null)
        {
            //if (damageTimes == 1 || dt == DamageType.Doge)
            {

                FlyFont font = GetFlyFont(dt);
                if (font == null)
                {
                    return;
                }
                font.InitDamageType(dt);
                AddFlyFont(entity, type, dt, font, damageValue);
            }
            //else
            //{
            //    uint damage = damageValue / damageTimes;
            //    List<int> damageList = new List<int>();
            //    int totalHurt = 0;
            //    uint averageValue = damage - damage / 4;
            //    for (int i = 0; i < damageTimes - 1; i++)
            //    {
            //        int hurt = UnityEngine.Random.Range((int)averageValue, (int)damage);
            //        damageList.Add(hurt);
            //        totalHurt += hurt;
            //    }

            //    int lastHurt = (int)damageValue - totalHurt;
            //    damageList.Add(lastHurt);
            //    StartCoroutine(AddDamage(roleid, damType, type, damageList, hurtinterval));
            //}
        }

    }
    StringBuilder damagetext = new StringBuilder(32);
    string strtype = "";
    void SetFontText(FlyFont font, int damage, DamageType damType, bool isSelf)
    {

        damage = Math.Abs(damage);

        damagetext.Remove(0, damagetext.Length);
        if (isSelf)
        {
            if (damType == DamageType.Critical)
            {
                damagetext.Append("1005");
                damagetext.Append("0510");
            }
            else if (damType == DamageType.AddExp)
            {
                damagetext.Append("1004");
                damagetext.Append("0410");
            }
            else if (damType == DamageType.Buff || damType == DamageType.Resist)
            {
                damagetext.Append("1002");
            }
            else if (damType == DamageType.Doge)
            {
                damagetext.Append("1003");
            }
            else if (damType == DamageType.AddHp)
            {
                damagetext.Append("0310");
            }
            else
            {
                damagetext.Append("0510");
            }
        }
        else
        {
            if (damType == DamageType.Critical)
            {
                damagetext.Append("1001");
                damagetext.Append("0210");
            }
            else if (damType == DamageType.AddExp)
            {
                damagetext.Append("1004");
                damagetext.Append("0410");
            }
            else if (damType == DamageType.Buff || damType == DamageType.Resist)
            {
                damagetext.Append("1002");
            }
            else if (damType == DamageType.Doge)
            {
                damagetext.Append("1003");
            }
            else if (damType == DamageType.AddHp)
            {
                damagetext.Append("0310");
            }
            else
            {
                damagetext.Append("0110");
            }
        }
        bool bReplaceAdd = false;
        if (damage != 0)
        {
            string str = damage.ToString();
            //经验buff加成显示 先暂时去掉 （要区分打怪经验 ）
            //if(damType == DamageType.AddExp)
            //{
            //    if (MainPlayerHelper.GetMainPlayer() != null)
            //    {
            //        int expbuff = MainPlayerHelper.GetMainPlayer().GetProp((int)PlayerProp.ExpAddBuff);
            //        expbuff = expbuff / 100;
            //        if (expbuff != 0)
            //        {
            //            bReplaceAdd = true;
            //            str = string.Format("{0}(+{1}%)", str, expbuff);
            //        }
            //    }
            //}
           
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                damagetext.Append("0");
                switch ((int)damType)
                {
                    case 0:
                        strtype = "0";
                        break;
                    case 1:
                        strtype = "1";
                        break;
                    case 2:
                        strtype = "2";
                        break;
                    case 3:
                        strtype = "3";
                        break;
                    case 4:
                        strtype = "4";
                        break;
                    case 5:
                        strtype = "5";
                        break;
                    case 6:
                        strtype = "6";
                        break;
                    case 7:
                        strtype = "7";
                        break;

                }
                if (!isSelf)
                {
                    damagetext.Append(strtype);
                }
                else
                {
                    if (damType == DamageType.AddHp || damType == DamageType.AddExp)
                    {
                        damagetext.Append(strtype);
                    }
                    else
                    {
                        damagetext.Append("5");
                    }
                }
                if (bReplaceAdd)
                {
                    if (c.ToString() == "+")
                    {
                        damagetext.Append("10");
                    }
                    else
                    {
                        damagetext.Append("0");
                        damagetext.Append(c);
                    }
                }
                else
                {
                    damagetext.Append("0");
                    damagetext.Append(c);
                }
                
              
            }
        }

        font.SetText(damagetext.ToString());

    }
    DamageType GetDamageType(uint damType)
    {
        DamageType dt = DamageType.Normal;
        if (damType == (uint)GameCmd.AttackType.ATTACK_TYPE_FO)
        {
            dt = DamageType.Normal;
        }
        else if (damType == (uint)GameCmd.AttackType.ATTACK_TYPE_LU)
        {
            dt = DamageType.Critical;
        }
        else if (damType == (uint)GameCmd.AttackType.ATTACK_TYPE_DK)
        {
            dt = DamageType.Resist;
        }
        else if (damType == (uint)GameCmd.AttackType.ATTACK_TYPE_HD)
        {
            dt = DamageType.Doge;
        }
        return dt;
    }
    public DamageType testType = DamageType.None;
    Vector3 GetMainCamDir()
    {
        if (Camera.main != null)
        {
            Matrix4x4 mat = new Matrix4x4();
            mat.SetTRS(Vector3.zero, Camera.main.transform.rotation, Vector3.one);
            return mat.GetColumn(2); // 镜头方向
        }

        return Vector3.forward;
    }
    Quaternion GetLookRotation()
    {
        Vector3 camDir = GetMainCamDir();
        camDir.Normalize();
        Vector3 r = Vector3.Cross(Vector3.up, camDir);
        r.Normalize();

        Vector3 up = Vector3.Cross(camDir, r);
        return Quaternion.LookRotation(camDir, up);
    }

    void AddFlyFont(IEntity entity, EntityType type, DamageType damType, FlyFont font, float damage = 0)
    {

        Transform posRoot = null;
        DamageType dt = damType;
        if (testType != DamageType.None)
        {
            dt = testType;
        }
        bool bSelf = false;
        if (type == EntityType.EntityType_Player)
        {
            if (ClientGlobal.Instance().MainPlayer.GetUID() == entity.GetUID())
            {
                bSelf = true;
            }
        }
        if (damage == 0 && damType == DamageType.Normal)
        {
          //  Log.Error("damage == 0 && damType == DamageType.Normal  parent is flyroot");
            ReturnFlyFont(dt, font);
            return;
        }
        SetFontText(font, (int)damage, dt, bSelf);
        font.SetActive(true);

        GameObject rootGo = m_posRootPool.Alloc();
        posRoot = rootGo.transform;
        posRoot.parent = this.transform;
        Vector3 enpos = entity.GetPos();
        Vector3 pos = Vector3.zero;
        entity.GetLocatorPos(m_strLocatorName, Vector3.zero, Quaternion.identity, ref pos, true);
        float r = 0;// entity.GetRadius() * 2;
        posRoot.position = new Vector3(enpos.x, pos.y + r, enpos.z);

        posRoot.transform.rotation = GetLookRotation();
        Transform fontTrans = font.GetFontTransform();
        posRoot.name = entity.GetName();
        if (fontTrans != null && posRoot != null)
        {

            fontTrans.parent = posRoot;
            fontTrans.localPosition = Vector3.zero;
            fontTrans.localRotation = Quaternion.identity;
            MeshText mt = font.GetMt();
            if (mt != null)
            {
                DOTweenAnimation[] tweenArray = font.GetTweens();
                DOTweenPath pathTween = font.GetPathTween();

                Tween maxAni = null;
                float during = 0;
                for (int i = 0; i < tweenArray.Length; i++)
                {
                    DOTweenAnimation tweenAni = tweenArray[i];
                    
                    float totalTime = tweenAni.duration + tweenAni.delay;
                    if (totalTime > during)
                    {
                        maxAni = tweenAni.tween;
                        during = totalTime;
                    }
                    if (pathTween != null)
                    {
                        if (pathTween.duration + pathTween.delay > during)
                        {
                            maxAni = pathTween.tween;
                            during = pathTween.duration + pathTween.delay;
                        }
                    }
                    if (tweenAni.animationType == DOTweenAnimationType.Color)
                    {
                        int a = 10;
                    }
                    tweenAni.tween.Restart();
                    tweenAni.tween.Play();
                   // tweenAni.DOPlay();
                    //if (tweenAni.animationType == DOTweenAnimationType.Color)
                    //{
                    //    tweenAni.tween.Restart();
                    //    tweenAni.tween.Kill();
                    //    tweenAni.CreateTween();
                    //    //tweenAni.DOPlay();
                    //}
                    //else
                    //{
                    //    tweenAni.tween.Kill();
                    //    tweenAni.CreateTween();
                    //   // tweenAni.tween.Restart();
                    //}
                }

                if (pathTween != null)
                {
                    pathTween.DORestart();
                    pathTween.DOPlay();
                   
                }
             
                
                maxAni.OnComplete(() =>
                {
                    ReturnFlyFont(dt, font);
                   
                });


            }

        }
        else
        {
            ReturnFlyFont(dt, font);
        }

    }

    //IEnumerator AddDamage(uint roleid, uint damtype, EntityType type, List<int> damageList, uint hurtinterval)
    //{
    //    Client.IEntity entity = GetEntity(roleid, type);
    //    float interval = (float)(hurtinterval * 0.001f); // 毫秒转成秒
    //    for (int j = 0; j < damageList.Count; j++)
    //    {
    //        int hurt = damageList[j];
    //        if (entity != null)
    //        {
    //            if (hurt > 0)
    //            {
    //                DamageType dt = GetDamageType(damtype);
    //                if (testType != DamageType.None)
    //                {
    //                    dt = testType;
    //                }
    //                FlyFont font = GetFlyFont(dt);
    //                if (font == null)
    //                {
    //                   yield return;
    //                }
    //                font.InitDamageType(dt);
    //                AddFlyFont(entity, type, GetDamageType(damtype), font, hurt);
    //            }
    //        }
    //        yield return new WaitForSeconds(interval);
    //    }

    //}
    #endregion
    IEntity GetEntity(long uid)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es != null)
        {
            return es.FindEntity(uid);
        }
        return null;
    }
    IEntity GetEntity(uint id, EntityType type)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            return null;
        }

        IEntity entity = null;
        switch (type)
        {
            case EntityType.EntityType_Item:
                entity = es.FindItem(id);
                break;
            case EntityType.EntityType_Monster:
                entity = es.FindMonster(id);
                break;
            case EntityType.EntityType_NPC:
                {
                    entity = es.FindNPC(id);
                    if (entity == null)
                    {
                        entity = es.FindRobot(id);
                    }
                }
                break;
            case EntityType.EntityType_Player:
                entity = es.FindPlayer(id);
                break;
            case EntityType.EntityType_Puppet:
                entity = es.FindPuppet(id);
                break;
            default:
                break;
        }
        return entity;
    }
}

