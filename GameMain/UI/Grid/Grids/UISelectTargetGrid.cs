using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class UISelectTargetGrid : UIGridBase
{
    UILabel m_lblName;
    UILabel m_lblLv;
    UILabel m_lblDistance;
    UITexture m_spIcon;
    UISlider m_hp;
    GameObject m_goLock;
    GameObject m_goLockBg;
    GameObject m_goSelect;

    public uint m_playerId;

    private CMResAsynSeedData<CMTexture> iuiIconAtlas = null;

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != iuiIconAtlas)
        {
            iuiIconAtlas.Release(true);
            iuiIconAtlas = null;
        }
    }

    protected override void OnUIBaseDestroy()
    {
        base.OnUIBaseDestroy();
        Release();
    }

    protected override void OnAwake()
    {
        base.OnAwake();

        m_lblName = this.transform.Find("name").GetComponent<UILabel>();
        m_lblLv = this.transform.Find("level").GetComponent<UILabel>();
        m_lblDistance = this.transform.Find("distance").GetComponent<UILabel>();
        m_spIcon = this.transform.Find("icon").GetComponent<UITexture>();
        m_hp = this.transform.Find("hp").GetComponent<UISlider>();
        m_goLock = this.transform.Find("lockBg/lock").gameObject;
        m_goLockBg = this.transform.Find("lockBg").gameObject;
        m_goSelect = this.transform.Find("select").gameObject;

        UIEventListener.Get(m_goLockBg).onClick = OnClickLock;
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        this.m_playerId = (uint)data;

        IEntitySystem entitySystem = ClientGlobal.Instance().GetEntitySystem();
        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;

        if (entitySystem == null)
        {
            return;
        }

        if (mainPlayer == null)
        {
            return;
        }

        IPlayer player = entitySystem.FindPlayer(this.m_playerId);

        if (player == null)
        {
            //锁定玩家太远  取不到九屏数据  不显示距离
            m_lblDistance.gameObject.SetActive(false);
            return;
        }

        //name 
        SetName(player.GetName());

        //icon 
        int job = player.GetProp((int)PlayerProp.Job);
        SetIcon((uint)job);

        //lv
        int lv = player.GetProp((int)CreatureProp.Level);
        SetLv(lv);

        //hp
        float hp = (float)player.GetProp((int)CreatureProp.Hp);
        int maxHp = player.GetProp((int)CreatureProp.MaxHp);
        if (maxHp > 0)
        {
            SetHp(hp / maxHp);
        }

        //distance
        float distace = Vector3.Distance(mainPlayer.GetPos(), player.GetPos());
        SetDistance(distace);

    }

    public void SetName(string name)
    {
        if (m_lblName != null)
        {
            m_lblName.text = name;
        }
    }

    public void SetIcon(uint job)
    {
        if (m_spIcon != null)
        {
            table.SelectRoleDataBase sdb = table.SelectRoleDataBase.Where((GameCmd.enumProfession)job, (GameCmd.enmCharSex)1);
            if (sdb != null)
            {
                // DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_spIcon, sdb.strprofessionIcon, false);

                UIManager.GetTextureAsyn(sdb.strprofessionIcon, ref iuiIconAtlas, () =>
                {
                    if (m_spIcon != null)
                    {
                        m_spIcon.mainTexture = null;
                    }
                }, m_spIcon,false);
            }
        }
    }

    public void SetLv(int lv)
    {
        if (m_lblLv != null)
        {
            m_lblLv.text = lv.ToString();
        }
    }

    public void SetDistance(float distance)
    {
        if (m_lblDistance != null)
        {
            m_lblDistance.gameObject.SetActive(true);
            m_lblDistance.text = string.Format("{0:F1}m", distance);
        }
    }

    public void SetHp(float hpValue)
    {
        if (m_hp != null)
        {
            m_hp.value = hpValue;
        }
    }

    public void SetSelect(bool b)
    {
        if (m_goSelect != null && m_goSelect.activeSelf != b)
        {
            m_goSelect.SetActive(b);
        }
    }


    public void SetLock(bool b)
    {
        if (m_goLock != null && m_goLock.activeSelf != b)
        {
            m_goLock.SetActive(b);
        }
    }

    void OnClickLock(GameObject go)
    {
        int btnIndex = 2;
        object param = btnIndex;
        InvokeUIDlg(UIEventType.Click, this, param);
    }

}

