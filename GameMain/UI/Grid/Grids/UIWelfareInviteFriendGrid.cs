
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using UnityEngine;
using table;
class UIWelfareInviteFriendGrid : UIGridBase
{

    private UILabel NameLabel;
    private UISprite HeadIcon;
    private UILabel ProfessionLabel;
    private UILabel LevelLabel;
    private UILabel RechargeNum;
    private uint palyerID;
    public uint PlayID
    {
        get
        {
            return palyerID;
        }

    }
    protected override void OnAwake()
    {
        base.OnAwake();
        NameLabel = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
        HeadIcon = CacheTransform.Find("Content/HeadIcon").GetComponent<UISprite>();
        ProfessionLabel = CacheTransform.Find("Content/Profession").GetComponent<UILabel>();
        LevelLabel = CacheTransform.Find("Content/Level").GetComponent<UILabel>();
        RechargeNum = CacheTransform.Find("Content/RechargeNum").GetComponent<UILabel>();
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (data == null)
        {
            return;
        }
        BaseUserInfo info = (BaseUserInfo)data;
        palyerID = info.uid;
        NameLabel.text = info.name;
        LevelLabel.text = info.lv.ToString();
        SelectRoleDataBase tab = table.SelectRoleDataBase.Where((GameCmd.enumProfession)info.profession, (GameCmd.enmCharSex)1);
        if (tab != null)
        {
            ProfessionLabel.text = tab.professionName;
            SetIcon(tab.mainUIHeadIcon);
        }
        RechargeNum.text = info.recharge.ToString();

    }
    private void SetIcon(string iconName)
    {
        UIManager.GetAtlasAsyn(iconName, ref m_playerAvataCASD, () =>
        {
            if (null != HeadIcon)
            {
                HeadIcon.atlas = null;
            }
        }, HeadIcon);

    }
    CMResAsynSeedData<CMAtlas> m_playerAvataCASD = null;
}
