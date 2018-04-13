
//*************************************************************************
//	创建日期:	2017/11/11 星期六 13:57:56
//	文件名称:	FBConfirmItemInfo
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using table;
using Client;
using Engine;
class FBConfirmItemInfo:UIGridBase
{
    ComBatCopyDataManager CopyDataManager
    {
        get
        {
            return DataManager.Manager<ComBatCopyDataManager>();
        }
    }
    private CMResAsynSeedData<CMTexture> m_iconCASD = null;
    public void Refresh(TeamMemberInfo info )
    {
        Transform nameTrans = this.transform.Find("name");
        if (nameTrans == null)
        {
            return;
        }
        Transform iconTrans = this.transform.Find("icon");
        if (iconTrans == null)
        {
            return;
        }
        Transform statusConf = this.transform.Find("status_Confirm");
        if (statusConf == null)
        {
            return;
        }
        Transform statusRefuse = this.transform.Find("status_Refuse");
        if (statusRefuse == null)
        {
            return;
        }
        Transform Label = this.transform.Find("Label");
        if (Label == null)
        {
            return;
        }
        bool bShow = false;
        if (info != null)
        {
            bShow = true;
        }
        nameTrans.gameObject.SetActive(bShow);
        iconTrans.gameObject.SetActive(bShow);
        statusConf.gameObject.SetActive(bShow);
        statusRefuse.gameObject.SetActive(bShow);
        if (info == null)
        {
            Label.gameObject.SetActive(true);
            return;
        }
        UILabel label = nameTrans.GetComponent<UILabel>();
        if (label != null)
        {
            label.text = info.name;
        }
        UITexture iconSpr = iconTrans.GetComponent<UITexture>();
        if (iconSpr != null)
        {
            List<SelectRoleDataBase> list = GameTableManager.Instance.GetTableList<SelectRoleDataBase>();
            SelectRoleDataBase db = list.Find((x) =>
            {
                return x.sexID == 1 && x.professionID == info.job;
            });
            UIManager.GetTextureAsyn(db.strprofessionIcon, ref m_iconCASD, () =>
            {
                if (null != iconSpr)
                {
                    iconSpr.mainTexture = null;
                }
            }, iconSpr);
        }

        bool bConfirm = false;
        if (CopyDataManager.m_dicTeammateStatus.ContainsKey(info.id))
        {
            bConfirm = CopyDataManager.m_dicTeammateStatus[info.id];
        }
        Label.gameObject.SetActive(false);
        statusConf.gameObject.SetActive(false);
        statusRefuse.gameObject.SetActive(!bConfirm);
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_iconCASD)
        {
            m_iconCASD.Release(true);
            m_iconCASD = null;
        }
    }
}
