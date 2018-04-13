using System;
using System.Collections.Generic;
using UnityEngine;

partial class ChapterPanel : UIPanelBase
{

    TweenChaptePanel tween = null;
    uint m_chapter = 0;
    protected override void OnLoading()
    {
        base.OnLoading();
        AdjustUI();
        tween = m_trans_chapterPanel.GetComponent<TweenChaptePanel>();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        m_chapter = 0;
        if (data is table.ChapterDabaBase)
        {
            table.ChapterDabaBase cdb = (table.ChapterDabaBase)data;
            m_chapter = cdb.ID;
            m_label_title.text = cdb.strTitle;
            m_label_desc.text = cdb.strDes;
        }
    }

    protected override void OnHide()
    {
        base.OnHide();
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.CHAPTER_EFFECT_END, m_chapter);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        tween.onFinished.Clear();
        tween.AddOnFinished(new EventDelegate(OnPlayForwardFinished));
        tween.PlayForward();
    }
    private void AdjustUI()
    {
        if (null != m_sprite_Bg)
        {
            m_sprite_Bg.width = (int)UIRootHelper.Instance.TargetFillSize.x;
            m_sprite_Bg.height = (int)UIRootHelper.Instance.TargetFillSize.y;
        }
    }

    void DelayShow()
    {
        tween.onFinished.Clear();
        tween.AddOnFinished(new EventDelegate(OnPlayReverse));
        tween.PlayReverse();
    }

    public void OnPlayForwardFinished()
    {
        Invoke("DelayShow",2f);
    }

    public void OnPlayReverse()
    {
        Invoke("Hide", 1f);
    }

    void Hide()
    {
        this.HideSelf();
    }
}
