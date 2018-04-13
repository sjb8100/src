using UnityEngine;
using System.Collections;
using System;
[Flags]
public enum MapScrollItemType
{
    Small = 0,
    NPC,
    Monster,
    Transmit,
}
public class MapNpcScrollItem : MonoBehaviour
{
    Client.NPCInfo npcInfo;
    public Client.NPCInfo Info
    {
        get
        {
            if (npcInfo == null)
            {
                Engine.Utility.Log.Error("npcinfo is null");
            }
            return npcInfo;
        }
        set
        {
            npcInfo = value;
        }
    }
    MapScrollItemType m_type;

    private UISprite m_spMaskIcon = null;
    public void InitInfo(Client.NPCInfo info)
    {
        if (null == m_spMaskIcon)
        {
            m_spMaskIcon = transform.Find("MaskIcon").GetComponent<UISprite>();
        }
        m_type = (MapScrollItemType)info.type;
        npcInfo = info;
        if (info != null)
        {
            Transform labelTrans = transform.Find("Label");
            bool visible = RoleStateBarManager.IsEntityHaveHeadIconMask((uint)info.npcID);
            if (null != m_spMaskIcon)
            {
                if (m_spMaskIcon.gameObject.activeSelf != visible)
                {
                    m_spMaskIcon.gameObject.SetActive(visible);
                }

                if (visible)
                {
                    table.NpcHeadMaskDataBase maskDb = RoleStateBarManager.GetNPCHeadMaskDB((uint)info.npcID);
                    if (null != maskDb)
                    {
                        UIManager.GetAtlasAsyn(maskDb.miniMapMaskIcon, ref m_curIconAsynSeed, () =>
                        {
                            if (null != m_spMaskIcon)
                            {
                                m_spMaskIcon.atlas = null;
                            }
                        }, m_spMaskIcon, false);
                    }
                }
            }
            if (labelTrans != null)
            {
                UILabel label = labelTrans.GetComponent<UILabel>();
                if (label != null)
                {
                    string showText = npcInfo.name;
                    if (npcInfo.pos.x != 0 || npcInfo.pos.y != 0)
                    {
                        string levelStr = string.Empty;

                        table.NpcDataBase ndb = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)npcInfo.npcID);
                        if (ndb != null)
                        {
                            if (ndb.dwType == (int)GameCmd.enumNpcType.NPC_TYPE_NONE)
                            {
                                if (ndb.dwMonsterType != 0)
                                {
                                    levelStr = ndb.dwLevel + "级 ";
                                }
                            }
                        }

                        showText = levelStr + npcInfo.name + "(" + npcInfo.pos.x.ToString() + "," + npcInfo.pos.y.ToString() + ")";
                    }
                    label.text = showText;
                }
            }
        }
    }
    CMResAsynSeedData<CMAtlas> m_curIconAsynSeed = null;
    public void Release(bool depthRelease = true)
    {
        if (m_curIconAsynSeed != null)
        {
            m_curIconAsynSeed.Release(depthRelease);
            m_curIconAsynSeed = null;
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
