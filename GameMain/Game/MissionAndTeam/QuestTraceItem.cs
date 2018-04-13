using System;
using System.Collections.Generic;
using UnityEngine;
using Cmd;
using GameCmd;
using Client;
class QuestTraceItem : MonoBehaviour
{
    const int TIMER_ID = 2000;


    //public UIXmlRichText m_xmlText = null;
    public UILabel m_lblText = null;

    UILabel m_labelTitle = null;
    UILabel m_labelState = null;

    UIWidget m_widght;

    GameObject m_effectRoot;

    public int m_nHeight = 0;

    public int Height
    {
        get { return m_nHeight; }
    }

    QuestTraceInfo m_taskInfo;

    public uint TaskID
    {
        get
        {
            if (m_taskInfo == null)
            {
                return 0;
            }
            return m_taskInfo.taskId;
        }
    }
    uint m_nPreState = 0;
    uint m_nPreOpreate = 0;
    bool m_bShow = false;

    MainMissionArrow m_MainMissionArrow = null;
    void Awake()
    {
        InitWidgets();
    }

    private void InitWidgets()
    {
        if (m_widght != null)
        {
            return;
        }

        //m_xmlText = transform.Find("RichTextRoot").GetComponent<UIXmlRichText>();
        m_lblText = transform.Find("RichTextRoot").GetComponent<UILabel>();
        m_widght = transform.GetComponent<UIWidget>();
        m_labelTitle = transform.Find("title").GetComponent<UILabel>();
        m_labelState = transform.Find("state").GetComponent<UILabel>();
        m_effectRoot = transform.Find("EffectRoot").gameObject;

        UIEventListener.Get(gameObject).onClick = OnUrlClicked;
    }

    public void UpdateUI(QuestTraceInfo taskInfo)
    {
        //if (m_xmlText == null || m_widght == null || taskInfo == null)
        if (m_lblText == null || m_widght == null || taskInfo == null)
        {
            return;
        }

        if (m_taskInfo != null && (m_taskInfo.taskId != taskInfo.taskId || m_taskInfo.dynamicTranceUpdate))
        {
            m_bShow = false;
            m_taskInfo.dynamicTranceUpdate = false;
        }
        m_taskInfo = taskInfo;
        //如果已经显示 并且状态没发生变化 不刷新UI
        if (m_bShow && m_nPreState == m_taskInfo.state && m_nPreOpreate == m_taskInfo.operate && m_taskInfo.taskType != TaskType.TaskType_Token)
        {
            return;
        }
        m_bShow = true;
        m_nPreState = m_taskInfo.state;
        m_nPreOpreate = m_taskInfo.operate;

        string tempstring = taskInfo.strDesc;
        GameCmd.TaskProcess process = taskInfo.GetTaskProcess();

        m_nHeight = 0;
        if (m_labelTitle != null)
        {
            m_labelTitle.text = taskInfo.StrFormatName;
            m_nHeight = m_labelTitle.height + 8;
        }
        if (m_labelState != null)
        {
            m_labelState.text = taskInfo.StrState;
        }
        //m_xmlText.Clear();
        //m_xmlText.fontSize = 20;

        //m_xmlText.AddXml(RichXmlHelper.RichXmlAdapt(tempstring));

        m_lblText.fontSize = 20;
        m_lblText.text = tempstring;

        //m_widght.height = m_nHeight + m_xmlText.host.height + 8;
        m_widght.height = m_nHeight + m_lblText.height + 8;
        m_nHeight = m_widght.height;

        if (taskInfo.taskType == TaskType.TaskType_Normal)
        {
            if (process == TaskProcess.TaskProcess_None)
            {
                SetEffect(false);
            }
            else if (process == TaskProcess.TaskProcess_CanDone)
            {
                SetEffect(true);
            }
            else
            {
                SetEffect(false);
            }
        }
        else
        {
            SetEffect(false);
        }

        int depth = m_widght.depth + 1;
        //foreach (Transform widget in m_xmlText.transform)
        //{
        //    if (widget.GetComponent<BoxCollider>() != null)
        //    {
        //        widget.GetComponent<BoxCollider>().enabled = false;
        //    }
        //    widget.GetComponent<UIWidget>().depth = depth;
        //}
    }

    public void OnUrlClicked(GameObject go)
    {
        if (m_taskInfo != null)
        {
            uint taskId = m_taskInfo.taskId;
            DataManager.Manager<TeamDataManager>().TeamMemberCheckAndCancelFollow();

            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINBTN_ONTOGGLE, new Client.stMainBtnSet() { pos = 1, isShow = false });
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TASK_DONING,
                new Client.stDoingTask() { taskid = m_taskInfo.taskId });

            //循环引导触发
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTGUIDERECYCLETRIGGER,
                new GuideDefine.RecycleTriggerGuidePassData { TaskID = taskId });
        }
    }

    public void Clear()
    {
        m_taskInfo = null;
        m_bShow = false;
        m_nPreOpreate = 0;
        m_nPreState = 0;
        m_nHeight = 0;
        transform.gameObject.SetActive(false);
        if (m_MainMissionArrow != null)
        {
            m_MainMissionArrow.Clear();
            m_MainMissionArrow = null;
        }
    }

    public void SetArrow(GameObject arrowObj)
    {
        InitWidgets();
        if (m_MainMissionArrow == null)
        {
            Transform arrowParent = arrowObj.transform.parent;
            Vector3 posArrowLocal = arrowParent.InverseTransformPoint(arrowObj.transform.position);
            Vector3 transLocal = arrowParent.InverseTransformPoint(transform.position);
            Vector3 target = Vector3.zero;
            target.x = transLocal.x + m_widght.width + arrowObj.GetComponent<UISprite>().width * 0.5f + 5;
            target.y = transLocal.y - m_widght.height * 0.5f;
            arrowObj.transform.position = arrowParent.TransformPoint(target);

            //TweenPosition tp = arrowObj.GetComponent<TweenPosition>();
            //tp.from = arrowObj.transform.localPosition;
            //Vector3 pos = arrowObj.transform.localPosition;
            //pos.x += 20;
            //tp.to = pos;
            //tp.duration = 0.5f;
            //tp.style = UITweener.Style.PingPong;

            int width = this.gameObject.GetComponent<UIWidget>().width;
            int height = this.gameObject.GetComponent<UIWidget>().height + 16;
            arrowObj.transform.position = new Vector3(this.transform.position.x + 0.5f * width, this.transform.position.y - 0.5f * height+9,this.transform.position.z);
            arrowObj.GetComponent<UIWidget>().width = width+15;
            arrowObj.GetComponent<UIWidget>().height = height;

            m_MainMissionArrow = new MainMissionArrow();
            m_MainMissionArrow.Init(arrowObj);
        }
    }


    public void HideArrow()
    {
        if (m_MainMissionArrow != null)
        {
            m_MainMissionArrow.HideWhenRootHide();
        }
    }

    public void ShowArrow()
    {
        if (m_MainMissionArrow != null)
        {
            m_MainMissionArrow.ShowWhenRootShow();
        }
    }

    /// <summary>
    /// 添加流光特效
    /// </summary>
    /// <param name="b"></param>
    public void SetEffect(bool b)
    {
        if (b)
        {
            m_effectRoot.SetActive(true);
            m_effectRoot.transform.localPosition = new Vector3(130, -26.5f - (m_nHeight - 56) * 0.5f, 0);
            m_effectRoot.transform.localScale = new Vector3(1, m_nHeight / 56.0f, 1);

            //特效
            UIParticleWidget wight = m_effectRoot.GetComponent<UIParticleWidget>();
            if (null == wight)
            {
                wight = m_effectRoot.AddComponent<UIParticleWidget>();
                wight.depth = 100;
            }

            if (wight != null)
            {
                wight.SetDimensions(1, 1);
                wight.ReleaseParticle();
                wight.AddParticle(50026);
            }
        }
        else
        {
            m_effectRoot.SetActive(false);
        }
    }

    public QuestTraceInfo GetTask()
    {
        return this.m_taskInfo;
    }
}
