using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using UnityEngine;
using table;



class UIActivityRewardListGrid : UIGridBase
{
    UILabel name;
    UISprite readIcon;
    UISprite readedIcon;
    public RecRetRewardData m_data
    {
        set;
        get;
    }
    protected override void OnAwake()
    {
        base.OnAwake();
        name = CacheTransform.Find("BG/label").GetComponent<UILabel>();
        readIcon = CacheTransform.Find("ICON/Toggle/read").GetComponent<UISprite>();
        readedIcon = CacheTransform.Find("ICON/Toggle/readed").GetComponent<UISprite>();
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
    }
    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if(data != null)
        {
            if (data is RecRetRewardData)
            {
                m_data = (RecRetRewardData)data;
                name.text = m_data.rewardtitle;
                bool readed = m_data.state == 1;
                readIcon.gameObject.SetActive(!readed);
                readedIcon.gameObject.SetActive(readed);

            }
        
        }
    }

}
