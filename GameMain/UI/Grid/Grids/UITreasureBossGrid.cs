using System;
using table;
using System.Collections.Generic;
using System.Linq;
using GameCmd;
using UnityEngine;
class UITreasureBossGrid : UIGridBase
{
    UITexture m_tex_Icon;
    UILabel m_lab_name;
    UILabel m_lab_lv;
    UILabel m_lab_bossState;
    UIToggle m_tg_hightlight;
    UISprite m_spr_hadKilled;

 
    public uint TableID
    {
        private set;
        get;
    }

    protected override void  OnAwake()
    {
       base.OnAwake();

       m_tex_Icon = CacheTransform.Find("IconContent/Icon").GetComponent<UITexture>();
       m_lab_name = CacheTransform.Find("Name").GetComponent<UILabel>();
       m_lab_lv = CacheTransform.Find("Level").GetComponent<UILabel>();
       m_lab_bossState = CacheTransform.Find("BossState").GetComponent<UILabel>();
       m_tg_hightlight = CacheTransform.Find("Toggle").GetComponent<UIToggle>();
       m_spr_hadKilled = CacheTransform.Find("KilledSign").GetComponent<UISprite>();
    }

    private CMResAsynSeedData<CMTexture> m_iconCASD = null;
    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (null == data)
        {
            return;
        }
        TableID = (uint)data;
        TreasureBossDataBase tabData = GameTableManager.Instance.GetTableItem<TreasureBossDataBase>(TableID);
        if (tabData ==null)
        {
            return;        
        }
        if (null != m_tex_Icon)
        {
            UIManager.GetTextureAsyn(tabData.icon, ref m_iconCASD, () =>
            {
                if (null != m_tex_Icon)
                {
                    m_tex_Icon.mainTexture = null;
                }
            }, m_tex_Icon);
        }
        m_lab_name.text = tabData.bossName;
        
        SetBossState(tabData);
    }
    void SetBossState(TreasureBossDataBase tabData) 
    {
         string msg ="";
         bool match = false;
        if (tabData.bossType == 1)
        {
            m_lab_lv.text = tabData.bossLv.ToString();
            match =MainPlayerHelper.GetPlayerLevel() >= tabData.lvLimit;
            msg = match ? DataManager.Manager<TextManager>().GetLocalText(LocalTextType.HuntingBoss_KeTiaoZhan) :
                                 DataManager.Manager<TextManager>().GetLocalText(LocalTextType.HuntingBoss_WeiJieSuo);

            CopyInfo info = DataManager.Manager<ComBatCopyDataManager>().GetCopyInfoById(tabData.copyID);
            if (info != null)
            {
                m_spr_hadKilled.gameObject.SetActive(info.CopyUseNum >= info.MaxCopyNum);
            }
          
        }
        else 
        {
            if (null != m_spr_hadKilled && m_spr_hadKilled.gameObject.activeSelf)
                m_spr_hadKilled.gameObject.SetActive(false);
            uint worldLv = DataManager.Manager<MailManager>().WorldLevel;
            uint targetLv = (worldLv >= tabData.bossLv) ? worldLv : tabData.bossLv;
            m_lab_lv.text = targetLv.ToString();

            msg = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.HuntingBoss_NotOpen);
            JvBaoBossWorldManager.LocalWorldBossInfo tempInfo = null;
            if (DataManager.Manager<JvBaoBossWorldManager>().TryGetWorldBossStatusInfo(tabData.ID, out tempInfo))
            {
                switch(tempInfo.Status)
                {
                    case JvBaoBossWorldManager.JvBaoBossStatus.JBS_NotOpen:
                        msg = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.HuntingBoss_NotOpen);
                        break;
                    case JvBaoBossWorldManager.JvBaoBossStatus.JBS_CommingSoon:
                        msg = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.HuntingBoss_ShuaXinShiJian, tabData.uiRefreshTime);
                        break;
                    case JvBaoBossWorldManager.JvBaoBossStatus.JBS_Attack:
                        msg = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.HuntingBoss_GongJiZhong);
                        break;
                    case JvBaoBossWorldManager.JvBaoBossStatus.JBS_Finish:
                        msg = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.HuntingBoss_End);
                        break;
                }
            }
        }
        m_lab_bossState.text = msg;
    }
    DateTime SetTime(uint time)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(time.ToString() + "0000000");
        TimeSpan toNow = new TimeSpan(lTime);
        return dtStart.Add(toNow);
    }

    public override void SetHightLight(bool hightLight)
    {
        base.SetHightLight(hightLight);
        if (null != m_tg_hightlight && m_tg_hightlight.value != hightLight)
        {
            m_tg_hightlight.value = hightLight;
        }
    }


    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_iconCASD)
        {
            m_iconCASD.Release(true);
        }
    }

}
