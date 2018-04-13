using System;
using System.Collections.Generic;
using UnityEngine;

public class UIRideGrid : UIGridBase
{
    GameObject lockGo;
    GameObject ownerGo;
    GameObject addGo;
    GameObject selectGo;
    GameObject fightGo;
    GameObject m_goSuffering;//传承--被吃掉
    GameObject m_goAdiration;//继承--吃掉别人获得经验


    RideData m_rideData = null;
    public RideData RideData { get { return m_rideData; } }

    public int TransExpSelect { set; get; }
    bool m_blastItem = false;
    public bool LastGrid { get { return m_blastItem; } }
    protected override void OnAwake()
    {
        base.OnAwake();
        Transform trans = transform;
        lockGo = trans.Find("lock_icon").gameObject;
        ownerGo = trans.Find("owner").gameObject;
        addGo = trans.Find("add_icon").gameObject;
        selectGo = trans.Find("select").gameObject;
        fightGo = trans.Find("owner/fightingLabel").gameObject;
        m_goSuffering = trans.Find("status_Suffering").gameObject;
        m_goAdiration = trans.Find("status_Adiration").gameObject;
    }

    public void SetSelect(bool active)
    {
        if (selectGo != null)
        {
            selectGo.SetActive(active);
        }
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        if (data is RideData)
        {
            m_rideData = (RideData)data;
            if (m_rideData.id != 0)
            {
                InitWithRideData(m_rideData);
            }else if (m_rideData.id == 0 && m_rideData.baseid == 0)
            {
                InitWithEmptyData(m_rideData.index == DataManager.Manager<RideManager>().MaxRideNum);
            }
        }

    }

    public void InitWithRideData(RideData data)
    {
        TransExpSelect = 0;
        m_blastItem = false;
        if (lockGo != null)
            lockGo.SetActive(false);
        if (addGo != null)
            addGo.SetActive(false);
        if (ownerGo != null)
            ownerGo.SetActive(true);

        if (m_goSuffering != null && m_goSuffering.activeSelf)
        {
            m_goSuffering.SetActive(false);
        }
        if (m_goAdiration != null && m_goAdiration.activeSelf)
        {
            m_goAdiration.SetActive(false);
        }

        if (m_rideData != null && ownerGo != null)
        {
            UITexture icon = ownerGo.transform.Find("icon").GetComponent<UITexture>();
            if (icon != null)
            {
                UIManager.GetTextureAsyn(data.icon
                    , ref iuiIconAtlas
                    , () =>
                    {
                        if (null != icon)
                        {
                            icon.mainTexture = null;
                        }
                    }, icon, true);
            }
            
            UISprite border = ownerGo.transform.Find("icon/biankuang").GetComponent<UISprite>();
            if (border != null)
            {
                UIManager.GetAtlasAsyn(data.QualityBorderIcon
                    , ref iuiBorderAtlas, () =>
                    {
                        if (null != border)
                        {
                            border.atlas = null;
                        }
                    }, border, true);
            }

            UILabel lableName = ownerGo.transform.Find("name").GetComponent<UILabel>();
            if (lableName != null)
            {
                lableName.text = m_rideData.name;
            }

            UILabel lableLevel = ownerGo.transform.Find("level").GetComponent<UILabel>();
            if (lableLevel != null)
            {
                //if (m_rideData.level == 0)
                //{
                //    lableLevel.text = "0级";
                //}
                //else
                //{
                //    lableLevel.text = "等级：" + m_rideData.level.ToString();
                //}
               // lableLevel.color = Color.white;
                lableLevel.text = DataManager.Manager<RideManager>().GetRideQualityStr(data.quality);
            }

            SetFightState(DataManager.Manager<RideManager>().Auto_Ride == m_rideData.id);

        }
    }

    public void InitWithEmptyData(bool blastItem)
    {
        TransExpSelect = 0;
        m_blastItem = blastItem;
        if (m_goSuffering != null && m_goSuffering.activeSelf)
        {
            m_goSuffering.SetActive(false);
        }
        if (m_goAdiration != null && m_goAdiration.activeSelf)
        {
            m_goAdiration.SetActive(false);
        }

        if (blastItem)
        {
            if (lockGo != null)
                lockGo.SetActive(true);
            if (addGo != null)
                addGo.SetActive(false);
            if (ownerGo != null)
                ownerGo.SetActive(false);
        }
        else
        {
            if (lockGo != null)
                lockGo.SetActive(false);
            if (addGo != null)
                addGo.SetActive(true);
            if (ownerGo != null)
                ownerGo.SetActive(false);
        }

        SetFightState(false);
    }


    public void SetSufferingState(bool active)
    {
        if (m_goSuffering != null)
        {
            m_goSuffering.SetActive(active);
        }
    }

    public void SetAdirationState(bool active)
    {
        if (m_goAdiration != null)
        {
            m_goAdiration.SetActive(active);
        }
    }

    public void SetFightState(bool active)
    {
        if (fightGo != null)
        {
            fightGo.SetActive(active);
        }
    }

    private CMResAsynSeedData<CMTexture> iuiIconAtlas = null;

    private CMResAsynSeedData<CMAtlas> iuiBorderAtlas = null;

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != iuiIconAtlas)
        {
            iuiIconAtlas.Release(true);
            iuiIconAtlas = null;
        }

        if (null != iuiBorderAtlas)
        {
            iuiBorderAtlas.Release(true);
            iuiBorderAtlas = null;
        }
    }
}