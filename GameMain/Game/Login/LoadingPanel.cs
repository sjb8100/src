using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Engine.Utility;
using Client;
using Common;
public partial class LoadingPanel : UIPanelBase
{
    UILabel m_tipsLabel;
    UISlider m_sliderPro;
    //List<string> m_lstTexture = new List<string>();
    List<uint> m_lstTexture = new List<uint>();
    List<string> m_lstTips = new List<string>();

    bool update = false;
    IMapSystem mapSystem;

    private CMResAsynSeedData<CMTexture> iuiTex = null;
    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();
//         panelBaseData.m_em_showMode = UIDefine.UIPanelShowMode.NoNeedBack;
//         panelBaseData.m_em_colliderMode = UIDefine.UIPanelColliderMode.Normal;
    }
    protected override void OnLoading()
    {
        AdjustUI();
        m_sliderPro = GetChildComponent<UISlider>("bottom/Sprite/progress");
        m_tipsLabel = GetChildComponent<UILabel>("bottom/tips");
    }
    private CMTexture tex = null; 
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        m_sliderPro.value = 0.0f;
        mapSystem = ClientGlobal.Instance().GetMapSystem();
        if (mapSystem != null)
        {
            Progress = 0f;
            update = true;
        }

        if (m_lstTips.Count <= 0)
        {
            List<table.LoadingTipsDatabase> lstTips = GameTableManager.Instance.GetTableList<table.LoadingTipsDatabase>();
            for (int i = 0; i < lstTips.Count; i++)
            {
                if (!string.IsNullOrEmpty(lstTips[i].strText))
                {
                    m_lstTips.Add(lstTips[i].strText);
                }
            }

            m_lstTexture.Add((uint)TextureID.Loadingbg);
            m_lstTexture.Add((uint)TextureID.Loadingbg2);
        }

        int tipRandom = UnityEngine.Random.Range(0, m_lstTips.Count);
        int texRandom = UnityEngine.Random.Range(0, m_lstTexture.Count);

        m_tipsLabel.text = m_lstTips[tipRandom];
        if (null != m__texture)
        {
            if (null != tex)
            {
                tex.ReleaseReference();
                tex = null;
            }
            tex = UIManager.GetTexture(m_lstTexture[texRandom]);
            if (tex != null)
            {
                m__texture.mainTexture = tex.GetTexture();
                m__texture.Update();
                m__texture.SetDirty();
                m__texture.MarkAsChanged();
            }
        }
        
    }

    private void AdjustUI()
    {
        Vector2 offset = UIRootHelper.Instance.OffsetSize;
        if (null != m_sprite_LeftBorder)
        {
            m_sprite_LeftBorder.width = Mathf.Max(1, Mathf.CeilToInt(offset.x * 0.5f));
            m_sprite_LeftBorder.height = (int)UIRootHelper.Instance.TargetSize.y;
        }

        if (null != m_sprite_RigtBorder)
        {
            m_sprite_RigtBorder.width = Mathf.Max(1, Mathf.CeilToInt(offset.x * 0.5f));
            m_sprite_RigtBorder.height = (int)UIRootHelper.Instance.TargetSize.y;
        }

        if (null != m_sprite_TopBorder)
        {
            m_sprite_TopBorder.width = (int)UIRootHelper.Instance.TargetSize.x;
            m_sprite_TopBorder.height = Mathf.Max(1, Mathf.CeilToInt(offset.y * 0.5f));
        }

        if (null != m_sprite_BottomBorder)
        {
            m_sprite_BottomBorder.width = (int)UIRootHelper.Instance.TargetSize.x;
            m_sprite_BottomBorder.height = Mathf.Max(1, Mathf.CeilToInt(offset.y * 0.5f));
        }
    }

    public float Progress
    {
        set
        {
            if (value > 1.0f)
            {
                m_sliderPro.value = 1.0f;
                update = false;
            }
            else if (value < 0.0f)
                m_sliderPro.value = 0.0f;
            m_sliderPro.value = value;
        }

        get { return m_sliderPro.value; }
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m__texture)
        {
            m__texture.mainTexture = null;
        }

        if (tex != null)
        {
            tex.ReleaseReference();
            tex = null;
        }
    }

    protected override void OnHide()
    {
        base.OnHide();
        Release();
    }

    private const float UI_LOADING_PERCENTAGE = 0.3f;

    void Update()
    {
        if (update != false && null != mapSystem)
        {
            Progress = (1 - UI_LOADING_PERCENTAGE) * mapSystem.Process + UI_LOADING_PERCENTAGE * DataManager.Manager<UIManager>().Progress;
        }
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eLoadingTips)
        {
            //Tips = (string)param;
        }
        else if (msgid == UIMsgID.eLoadingProcess)
        {
            Progress = (float)param;

        }
        return true;
    }


}
