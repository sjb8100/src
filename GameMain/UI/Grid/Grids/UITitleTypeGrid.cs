
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UITitleTypeGrid : UIGridBase
{
    #region overridemethod
    private UILabel name;
    private UISprite m_sp_arrow;
    //private UISprite m_sp_bg;
    private uint data;
    public uint SecondTypeId
    {
        get
        {
            return data;
        }
    }

    private Transform container = null;
    private List<Transform> ts = new List<Transform>();

    public UIGrid grid = null;
    private UISprite m_sp_redPoint = null;
    private UIToggle m_tg_hightToggle = null;
    private bool m_bhightLight = false;
    protected override void OnAwake()
    {
        base.OnAwake();
        ts.Clear();
        container = CacheTransform.Find("Content/Container");
        name = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
        grid = CacheTransform.Find("Content/Container/GridContent").GetComponent<UIGrid>();
        m_sp_redPoint = CacheTransform.Find("Content/RedPoint").GetComponent<UISprite>();
        m_sp_arrow = CacheTransform.Find("Content/Arrow").GetComponent<UISprite>();
        m_tg_hightToggle = CacheTransform.Find("Content/Toggle").GetComponent<UIToggle>();
        SetTriggerEffect(false);
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        this.data = (uint)data;
    }

    public override void SetHightLight(bool hightLight)
    {
        m_bhightLight = hightLight;
        base.SetHightLight(hightLight);
        if (null != m_tg_hightToggle && m_tg_hightToggle.value != hightLight)
        {
            m_tg_hightToggle.value = hightLight;
        }

        if (m_sp_arrow != null && m_sp_arrow.enabled)
        {

            TweenRotation tr = m_sp_arrow.GetComponent<TweenRotation>();
            m_sp_arrow.transform.localRotation = hightLight ? Quaternion.Euler(tr.to) : Quaternion.Euler(tr.from);
        }
    }

    public void ToggleHightLight()
    {
        if (m_sp_arrow != null && m_sp_arrow.enabled)
        {
            TweenRotation tr = m_sp_arrow.GetComponent<TweenRotation>();
            m_bhightLight = !m_bhightLight;
            m_sp_arrow.transform.localRotation = m_bhightLight ? Quaternion.Euler(tr.to) : Quaternion.Euler(tr.from);
        }
    }
    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="forward"></param>
    public void PlayTween(AnimationOrTween.Direction direction, bool forward = true)
    {
        UIPlayTween[] tweens = CacheTransform.GetComponents<UIPlayTween>();
        if (null != tweens)
        {
            Vector3 pos = CacheTransform.transform.localPosition;
            foreach (UIPlayTween tween in tweens)
            {
                tween.playDirection = direction;
                if (tween.onFinished.Count <= 0)
                {
                    tween.onFinished.Add(new EventDelegate(() =>
                    {
                        CacheTransform.localPosition = pos;
                    }));
                }
                tween.Play(forward);
            }
        }
    }

    public void EnableArrow(bool enable)
    {
        if (null != m_sp_arrow
            && m_sp_arrow.enabled != enable)
        {
            m_sp_arrow.enabled = enable;
        }
    }

    /// <summary>
    /// 设置名称
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (null != this.name)
        {
            this.name.text = name;
        }
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    public bool Add(Transform trans)
    {
        if (null != grid && null != trans)
        {
            trans.parent = grid.transform;
            trans.localPosition = Vector3.zero;
            trans.localScale = Vector3.one;
            trans.localRotation = Quaternion.identity;
            return true;
        }
        return false;

    }

    public int ChildCount
    {
        get
        {
            if (grid != null)
            {
                return grid.transform.childCount;
            }
            return 0;
        }
    }

    public List<T> GetAllChild<T>()
    {
        List<T> list = new List<T>();
        if (grid != null)
        {
            foreach (Transform item2 in grid.transform)
            {
                T child = item2.GetComponent<T>();
                if (child != null)
                {
                    list.Add(child);
                }
            }
        }
        return list;
    }
    /// <summary>
    /// 红点提示
    /// </summary>
    /// <param name="enable"></param>
    public void EnableRedPoint(bool enable)
    {
        if (null != m_sp_redPoint && m_sp_redPoint.enabled != enable)
        {
            m_sp_redPoint.enabled = enable;
        }
    }

    /// <summary>
    /// 更新格子位置
    /// </summary>
    public void UpdatePostion()
    {
        if (null != grid)
        {
            grid.enabled = true;
            grid.Reposition();
        }
    }

    public void InitContentGrid(float cellWidth, float cellHeight)
    {
        if (null != grid)
        {
            grid.cellWidth = cellWidth;
            grid.cellHeight = cellHeight;
        }
    }
    #endregion
}

