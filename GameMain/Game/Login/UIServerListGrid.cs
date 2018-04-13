//*************************************************************************
//	创建日期:	2016-12-23 14:03
//	文件名称:	UIServerListGrid.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	服务器列表grid
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIServerListGrid :UIGridBase
{
    private UILabel m_lableName = null;
    private UISpriteEx m_spriteStatus = null;
    private UISpriteEx m_spriteRoleIcon = null;
    private GameObject m_goNewTips = null;
    private UILabel m_lableLevel = null;

    public Pmd.ZoneInfo m_zone = null;

    private int m_index = 0;
    public int Index
    {
        get
        {
            return m_index;
        }
    }
    public bool isLastZone = false;

    protected override void OnAwake()
    {
 	    base.OnAwake();
        m_lableName = transform.Find("lableName").GetComponent<UILabel>();
        m_spriteStatus = transform.Find("status").GetComponent<UISpriteEx>();
        m_spriteRoleIcon = transform.Find("roleIcon").GetComponent<UISpriteEx>();
        m_goNewTips = transform.Find("Tip").gameObject;

        if (m_spriteRoleIcon != null)
        {
            m_lableLevel = m_spriteRoleIcon.transform.Find("roleLevel").GetComponent<UILabel>();
        }
    }

    public void SetServerListGridData(Pmd.ZoneInfo zone, int index)
    {
        if (zone == null)
        {
            gameObject.SetActive(false);
            return;
        }
        m_index = index;
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        m_zone = zone;
        if (m_lableName != null)
        {
            m_lableName.text = GetZoneNameByState(zone);
        }

        if (m_spriteStatus != null)
        {
            m_spriteStatus.ChangeSprite(GetZoneOnlineStatus(zone));
        }
        //TODO
        if (m_spriteRoleIcon != null)
        {
            m_spriteRoleIcon.ChangeSprite(0);

            if (m_lableLevel != null)
            {
                m_lableLevel.text = "";
            }
        }


        if (m_goNewTips != null)
        {
            m_goNewTips.SetActive(false);
        }
    }

    /// <summary>
    /// 服务器在线状态
    /// </summary>
    /// <returns></returns>
    public static int GetZoneOnlineStatus(Pmd.ZoneInfo zone)
    {
        uint onlinenum = zone.onlinenum;

        if (zone.state == Pmd.ZoneState.Shutdown)
        {
            return 4;//维护
        }

        if (onlinenum > 1000)
        {
            return 1;//火爆
        }
        else if (onlinenum > 500)
        {
            return 2;//拥挤
        }

        return 3;//畅通
    }

    //通过状态，返回区服文字颜色
    public static string GetZoneNameByState(Pmd.ZoneInfo zone)
    {
        string zonename = zone.zonename;
//         if (zone.state == Pmd.ZoneState.Shutdown)
//         {
//             //灰色
//             zonename = "[777777]" + zonename + "[-]";
//         }
//         else if (zone.state == Pmd.ZoneState.Normal)
//         {
//             //绿色
//             zonename = "[00ff00]" + zonename + "[-]";
//         }
//         else if (zone.state == Pmd.ZoneState.Fullly)
//         {
//             //红色
//             zonename = "[ff0000]" + zonename + "[-]";
//         }
//         else if (zone.state == Pmd.ZoneState.Starting)
//         {
//             zonename = "[68228B]" + zonename + "[-]";
//         }
        return zonename;
    }
}
