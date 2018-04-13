using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Engine.Utility;
public class UIItemUseCommonGrid : UIGridBase,ITimer
{
    #region Property
    private UILabel name;
    private UILabel num;
    private UILabel des;
    private UITexture icon;
    private Transform notEnoughMask;
    //id
    private uint data;
    public uint Data
    {
        get
        {
            return data;
        }
    }
    //长按定时器
    private const int LONGPRESS_TIMER_ID = 2000;
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        name = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
        num = CacheTransform.Find("Content/Num").GetComponent<UILabel>();
        des = CacheTransform.Find("Content/Des").GetComponent<UILabel>();
        icon = CacheTransform.Find("Content/Icon").GetComponent<UITexture>();
        notEnoughMask = CacheTransform.Find("Content/NotEnoughGetMask");
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (null == data)
            return;
        this.data = (uint)data;
    }
    public override void OnPress(bool isDown)
    {
        base.OnPress(isDown);
        if (isDown)
        {
            TimerAxis.Instance().SetTimer(LONGPRESS_TIMER_ID, 500, this);
        }
        else
        {
            TimerAxis.Instance().KillTimer(LONGPRESS_TIMER_ID, this);
        }
    }
    #endregion

    #region ITimer
    public void OnTimer(uint uTimerID)
    {
        if (uTimerID == LONGPRESS_TIMER_ID)
        {
            InvokeUIDlg(UIEventType.LongPressing, this, data);
        }
    }
    #endregion

    #region Set
    
    public void SetNotEnoughMask(bool notEnoughMask)
    {
        if (null != this.notEnoughMask && this.notEnoughMask.gameObject.activeSelf != notEnoughMask)
        {
            this.notEnoughMask.gameObject.SetActive(notEnoughMask);
        }
    }
    CMResAsynSeedData<CMTexture> m_playerAvataCASD = null;
    public void SetIcon(string iconName)
    {
        UIManager.GetTextureAsyn(iconName, ref m_playerAvataCASD, () =>
        {
            if (null != icon)
            {
                icon.mainTexture = null;
            }
        }, icon);

    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_playerAvataCASD != null)
        {
            m_playerAvataCASD.Release(depthRelease);
            m_playerAvataCASD = null;
        }
    }

    protected override void OnUIBaseDestroy()
    {
        base.OnUIBaseDestroy();
        Release();
    }

    public void SetName(string name)
    {
        if (null != this.name)
        {
            this.name.text = name;
        }
    }

    public void SetNum(uint num)
    {
        if(null != this.num)
        {
            this.num.text = (num == 0) ? ColorManager.GetColorString(ColorType.Red, "0") : ("" + num);
        }
    }

    public void SetDes(string des)
    {
        if (null != this.des)
        {
            this.des.text = des;
        }
    }

    #endregion
}