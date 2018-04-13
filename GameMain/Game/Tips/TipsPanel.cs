using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class TipsPanel : UIPanelBase
{
    protected class Entry
    {
        public float time;			// Timestamp of when this entry was added
        public float stay = 0f;		// How long the text will appear to stay stationary on the screen
        public float offset = 0f;	// How far the object has moved based on time
        public UIXmlRichText label;		// Label on the game object
        public UISprite widget;
        public float height;
        public float movementStart { get { return time + stay; } }
    }
    static int Comparison(Entry a, Entry b)
    {
        if (a.movementStart < b.movementStart) return -1;
        if (a.movementStart > b.movementStart) return 1;
        return 0;
    }

    public AnimationCurve offsetCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(0.5f, 30f), new Keyframe(2f, 150f) });

    /// <summary>
    /// Curve used to fade out entries with time.
    /// </summary>

    public AnimationCurve alphaCurve = new AnimationCurve(new Keyframe[] { new Keyframe(1f, 1f), new Keyframe(1f, 0.5f), new Keyframe(2f, 0f) });

    /// <summary>
    /// Curve used to scale the entries.
    /// </summary>

    public AnimationCurve scaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(0.1f, 0.3f), new Keyframe(0.25f, 1f) });

    List<Entry> mList = new List<Entry>();
    List<Entry> mUnused = new List<Entry>();

    int counter = 0;

    Dictionary<bool, List<GameObject>> fightPowerObjs = new Dictionary<bool, List<GameObject>>();
    List<GameObject> list = new List<GameObject>();
    List<GameObject> list2 = new List<GameObject>();
    int countNum = 0;
    /// <summary>
    /// Whether some HUD text is visible.
    /// </summary>

    public bool isVisible { get { return mList.Count != 0; } }

    GameObject m_prefab;
    public delegate void OnAddText(string msg);
    OnAddText m_addtextCallback;
    Entry Create()
    {
        if (mUnused.Count > 0)
        {
            Entry ent = mUnused[mUnused.Count - 1];
            mUnused.RemoveAt(mUnused.Count - 1);
            ent.time = Time.realtimeSinceStartup;
            ent.widget.depth = 10;//NGUITools.CalculateNextDepth(gameObject);
            NGUITools.SetActive(ent.label.gameObject, true);
            ent.offset = 0f;
            mList.Add(ent);
            return ent;
        }

        Entry ne = new Entry();
        ne.time = Time.realtimeSinceStartup;
        GameObject go = NGUITools.AddChild(m_trans_root.gameObject, m_prefab);
        ne.label = go.GetComponent<UIXmlRichText>();
        ne.label.name = counter.ToString();
        ne.widget = go.GetComponent<UISprite>();
        ne.widget.cachedTransform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        mList.Add(ne);
        ++counter;
        return ne;
    }

    void Delete(Entry ent)
    {
        ent.label.Clear();
        mList.Remove(ent);
        mUnused.Add(ent);
        NGUITools.SetActive(ent.label.gameObject, false);
    }

    protected override void OnLoading()
    {
        base.OnLoading();
        m_prefab = m_sprite_tip.gameObject;
        m_prefab.SetActive(false);
        m_trans_miscomplete.gameObject.SetActive(false);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SYSTEM_LOADSCENECOMPELETE, OnEventCallback);
    }
    public bool IsLoadingSuccess = false;
    void OnEventCallback(int nEventID, object param)
    {
        if (nEventID == (int)Client.GameEventID.SYSTEM_LOADSCENECOMPELETE)
        {
            IsLoadingSuccess = true;
        }

    }
    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SYSTEM_LOADSCENECOMPELETE, OnEventCallback);
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
    }

    public void ShowMissonDoneTips()
    {
        StartCoroutine(DelayShowMissionTips());
    }

    IEnumerator DelayShowMissionTips()
    {
        m_trans_fightPowerRoot.gameObject.SetActive(false);
        m_trans_miscomplete.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        m_trans_miscomplete.gameObject.SetActive(false);
        m_trans_fightPowerRoot.gameObject.SetActive(false);
    }

    public void ShowFightPowerChangeTips(bool value, int newPower, int prePower)
    {
        DelayShowFightPowerChangeTips(value, newPower, prePower);
    }
    void DelayShowFightPowerChangeTips(bool value, int newPower, int prePower)
    {
        m_trans_miscomplete.gameObject.SetActive(false);
        m_trans_fightPowerRoot.gameObject.SetActive(true);
       
        UILabel label;
        m_trans_Up.gameObject.SetActive(value);
        m_trans_Down.gameObject.SetActive(!value);
        if (value)
        {
            label = m_label_powerUpLabel;
        }
        else
        {
            label = m_label_powerDownLabel;
        }
        SlideAnimation diamondSlide = m_label_powerChangeLabel.GetComponent<SlideAnimation>();
        if (null == diamondSlide)
            diamondSlide = m_label_powerChangeLabel.gameObject.AddComponent<SlideAnimation>();
        int gap = (int)Mathf.Abs(newPower - prePower);
        label.text = gap.ToString();
        float speed = gap / 0.5f;
        if (null != diamondSlide)
            diamondSlide.DoSlideAnim(prePower, newPower, true, speed, null);
    }
    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eFightPowerChange)
        {
            m_trans_fightPowerRoot.gameObject.SetActive(false);
        }

        return base.OnMsg(msgid, param);
    }

    public void ShowFxTips(uint effectResId)
    {
        m_trans_fxRoot.gameObject.SetActive(true);

        table.ResourceDataBase rdb = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(effectResId);
        if (rdb == null)
        {
            return;
        }

        Engine.IRenderSystem es = Engine.RareEngine.Instance().GetRenderSystem();
        if (es == null)
        {
            return;
        }

        string path = rdb.strPath;
        Engine.IEffect effect = null;

        if (es.CreateEffect(ref path, ref effect))
        {
            Transform transf = effect.GetNodeTransForm();

            if (transf == null)
            {
                return;
            }

            transf.parent = m_trans_fxRootContent;
            transf.localPosition = Vector3.zero;

            m_trans_fxRootContent.SetChildLayer(LayerMask.NameToLayer("UI"));

        }

    }

    public void AddTips(string xml)
    {
        StartCoroutine(DelayAddTips(xml));
    }

    IEnumerator DelayAddTips(string xml)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        float time = Time.realtimeSinceStartup;
        Entry ne = Create();
        ne.stay = 0.25f;
        int maxWidth = 1200;
        ne.widget.cachedTransform.localPosition = Vector3.zero;
        ne.widget.width = maxWidth;
        ne.widget.name = mList.Count.ToString();
        ne.widget.pivot = UIWidget.Pivot.TopLeft;
        ne.label.fontSize = 24;
        ne.label.AddXml(xml);


        ne.widget.pivot = UIWidget.Pivot.Center;

        if (ne.label.overline)
        {
            ne.widget.width = maxWidth;
        }
        else
        {
            ne.widget.width = Mathf.CeilToInt(ne.label.m_layout.x) + 20;
            if (ne.widget.width <= 400)
            {
                ne.widget.width = 400;
            }
        }
        ne.widget.height = ne.label.fontSize + 10;
        foreach (Transform child in ne.widget.transform)
        {
            Vector3 pos = child.localPosition;
            if (ne.label.overline)
            {
                pos.x -= maxWidth * 0.5f;
            }
            else
            {
                pos.x -= ne.label.m_layout.x * 0.5f;
            }
            pos.y += ne.label.fontSize * 0.5f;
            child.localPosition = pos;
        }

        ne.widget.cachedTransform.localPosition = Vector3.zero;
        ne.widget.gameObject.SetActive(true);
        mList.Sort(Comparison);
    }
    void OnDisable()
    {
        for (int i = mList.Count; i > 0; )
        {
            Entry ent = mList[--i];
            if (ent.label != null) { ent.label.Clear(); ent.label.enabled = false; }
            else mList.RemoveAt(i);
        }
    }

   /* void Update()
    {
#if UNITY_EDITOR
		if (!Application.isPlaying) return;
#endif
        if (mList.Count <= 0)
        {
            return;
        }
        float time = RealTime.time;

        Keyframe[] offsets = offsetCurve.keys;
        // Keyframe[] alphas = alphaCurve.keys;
        Keyframe[] scales = scaleCurve.keys;

        float offsetEnd = offsets[offsets.Length - 1].time;
        // float alphaEnd = alphas[alphas.Length - 1].time;
        float scalesEnd = scales[scales.Length - 1].time;
        float totalEnd = Mathf.Max(scalesEnd, Mathf.Max(offsetEnd, 0));

        // Adjust alpha and delete old entries
        for (int i = mList.Count; i > 0; )
        {
            Entry ent = mList[--i];
            float currentTime = time - ent.movementStart;
            ent.offset = offsetCurve.Evaluate(currentTime);
            //             float alpha = alphaCurve.Evaluate(currentTime);
            //             if (alpha == 0.2f)
            //             {
            //                 alpha = 0.19f;
            //             }
            //             ent.widget.alpha = alpha;

            // Make the label scale in
            float s = scaleCurve.Evaluate(time - ent.time);
            if (s < 0.001f) s = 0.001f;
            ent.widget.cachedTransform.localScale = new Vector3(s, s, s);

            // Delete the entry when needed
            if (currentTime > totalEnd) Delete(ent);
            else ent.label.enabled = true;
        }

        float offset = 0f;

        // Move the entries
        for (int i = mList.Count; i > 0; )
        {
            Entry ent = mList[--i];

            offset = Mathf.Max(offset, ent.offset);
            ent.widget.cachedTransform.localPosition = new Vector3(ent.widget.cachedTransform.localPosition.x, offset, 0f);
            offset += Mathf.Round(ent.widget.cachedTransform.localScale.y * ent.widget.height);
        }
    }*/
}
