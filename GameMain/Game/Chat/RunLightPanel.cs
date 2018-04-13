using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class RunLightInfo
{
    public enum Pos
    {
        Top,
        Bottom,
    }
    public Pos pos;
    public string msg;
    public int showTime;
}


partial class RunLightPanel : UIPanelBase
{
    Transform m_transTopRoot;
    Transform m_transBottomRoot;

    GameObject m_lableObj;

    List<UIXmlRichText> m_updateXmlText = new List<UIXmlRichText>();

    const int moveSpeed = 90;
    UIPanel m_panelTop;
    UIPanel m_panelBottom;
    UIXmlRichText m_bottomXmlText;

    Queue<string> m_bottomQueue = new Queue<string>();
    Queue<RunLightInfo> m_topQueue = new Queue<RunLightInfo>();
    RunLightInfo m_currTopRunLightInfo = null;
    protected override void OnLoading()
    {
        base.OnLoading();
        m_panelTop = m_trans_TOP.GetComponent<UIPanel>();
        m_trans_TOP.Find("bg").GetComponent<UISprite>().width = (int)m_panelTop.width;
        m_trans_TOP.Find("bg").GetComponent<UISprite>().height = (int)m_panelTop.height;

        m_panelBottom = m_trans_Bottom.GetComponent<UIPanel>();

        m_lableObj = m_widget_xmlText.gameObject;

        m_trans_TOP.gameObject.SetActive(false);
        m_trans_Bottom.gameObject.SetActive(false);
    }

    protected override void OnHide()
    {
        base.OnHide();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (data is RunLightInfo)
        {
            RunLightInfo info = (RunLightInfo)data;
            if (info.pos == RunLightInfo.Pos.Bottom)
            {
                m_bottomQueue.Enqueue(info.msg);
                if (m_trans_Bottom.gameObject.activeSelf)
                {
                    return;
                }

                if (m_bottomXmlText == null)
                {
                    GameObject go = GameObject.Instantiate(m_lableObj) as GameObject;
                    go.transform.parent = m_trans_Bottom;
                    go.transform.transform.localScale = Vector3.one;

                    m_bottomXmlText = go.GetComponent<UIXmlRichText>();
                }
                m_trans_Bottom.gameObject.SetActive(true);
                AddBottomText();
            }
            else
            {
                m_topQueue.Enqueue(info);
                if (m_currTopRunLightInfo == null)
                {
                    AddTopTextFromQueue();
                }
            }
        }
    }

    private void AddBottomText()
    {
        if (m_bottomQueue.Count <= 0)
        {
            return;
        }

        string xml = m_bottomQueue.Dequeue();

        UIXmlRichText xmltext = m_bottomXmlText;

        xmltext.fontSize = 18;
        xmltext.host.width = 1200;

        xmltext.UrlClicked += OnClickUrl;
        xmltext.AddXml(xml);
        if (xmltext.overline)
        {
            xmltext.host.width = 1200;
        }
        else
        {
            xmltext.host.width = Mathf.CeilToInt(xmltext.m_layout.x);
        }

        Vector3 pos = new Vector3(-xmltext.host.width * 0.5f, xmltext.host.height > xmltext.fontSize ? xmltext.host.height * 0.5f : 0, 0);
        xmltext.transform.transform.localPosition = pos;

        pos.y += 5;
        pos.x -= 5;
        m_sprite_buttombg.transform.localPosition = pos;
        m_sprite_buttombg.width = xmltext.host.width + 10;
        m_sprite_buttombg.height = xmltext.host.height + 10;

        StartCoroutine(WaitForDestroy());
    }

    IEnumerator WaitForDestroy() 
    {
        yield return new WaitForSeconds(3f);
        if (m_bottomXmlText != null)
        {
            m_bottomXmlText.Clear();            
        }

        if (m_bottomQueue.Count <= 0)
        {
            m_trans_Bottom.gameObject.SetActive(false);
            CheckToHide();
        }
        else
        {
            AddBottomText();
        }
    }

    private void AddNewTopText(RunLightInfo info)
    {
        GameObject go = GameObject.Instantiate(m_lableObj) as GameObject;
        go.transform.parent = m_trans_root;
        go.transform.transform.localScale = Vector3.one;

        if (m_updateXmlText.Count > 0)
        {
            UIXmlRichText text = m_updateXmlText[m_updateXmlText.Count - 1];
            //localPosition.x  为负数 text.m_layout.x  m_trans_root的x点在右边300
            float width = text.mtrans.localPosition.x + text.m_layout.x;
            if (width <= 0)// 小于0 则全部在panel中了。
            {
                go.transform.transform.localPosition = Vector3.zero;
            }
            else
            {
                go.transform.transform.localPosition = new Vector3(text.mtrans.localPosition.x + text.m_layout.x + 15, 0, 0);
            }
        }
        else
        {
            go.transform.transform.localPosition = Vector3.zero;
        }

        UIXmlRichText xmltext = go.GetComponent<UIXmlRichText>();
        xmltext.extObj = info;
        xmltext.UrlClicked += OnClickUrl;
        xmltext.AddXml(info.msg);
        xmltext.host.width = Mathf.CeilToInt(xmltext.m_layout.x);
        m_updateXmlText.Add(xmltext);

        if (m_trans_TOP.gameObject.activeSelf == false)
            m_trans_TOP.gameObject.SetActive(true);
    }

    void AddTopTextFromQueue()
    {
        if (m_topQueue.Count > 0)
        {
            m_currTopRunLightInfo = m_topQueue.Dequeue();
            AddNewTopText(m_currTopRunLightInfo);
        }
        else
        {
            CheckToHide();
        }
    }

    void OnClickUrl(UIWidget sender,string url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            DataManager.Manager<ChatDataManager>().OnUrlClick(url);
        }
    }

    void CheckToHide()
    {
        if (m_trans_TOP.gameObject.activeSelf == false && m_trans_Bottom.gameObject.activeSelf == false)
        {
            HideSelf();
        }
    }


    void Update()
    {
        if (m_updateXmlText.Count <= 0)
        {
            m_trans_TOP.gameObject.SetActive(false);
            CheckToHide();
            return;
        }

        List<UIXmlRichText> todelete = new List<UIXmlRichText>();
        for (int i = 0; i < m_updateXmlText.Count; i++)
        {
            UIXmlRichText text = m_updateXmlText[i];
            Vector3 pos = text.mtrans.localPosition;
            pos.x -= moveSpeed * Time.deltaTime;
            text.mtrans.localPosition = pos;
            if (-pos.x >= m_panelTop.width + text.m_layout.x + 5)
            {
                todelete.Add(text);
            }
        }

        if (todelete.Count > 0)
        {
            for (int i = 0; i < todelete.Count; i++)
            {
                m_updateXmlText.Remove(todelete[i]);
                GameObject.Destroy(todelete[i].gameObject);
                if (todelete[i].extObj != null)
                {
                    RunLightInfo info = (RunLightInfo)todelete[i].extObj;
                    if (--info.showTime > 0)
                    {
                        AddNewTopText(info);
                    }
                    else
                    {
                        m_currTopRunLightInfo = null;
                        AddTopTextFromQueue();
                    }
                }
            }
        }
        todelete.Clear();

    }
}
