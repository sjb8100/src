using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using table;
using Client;
partial class ShowModelPanel
{
    IRenerTextureObj m_RTObj = null;
    float rotateY = -8;
    float eulerX = 0;
    protected override void OnLoading()
    {
        base.OnLoading();
        UIEventListener.Get(m__Model.gameObject).onDrag = DragModel;
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (data != null && data is uint)
        {
            InitModelPanel((uint)data);
        }
    }
    void InitModelPanel(uint ModelID)
    {

        //传入的数据全部都是在模型展示表格中的模型id
        ShowModelDataBase data = GameTableManager.Instance.GetTableItem<ShowModelDataBase>(ModelID);
        if(data == null)
        {
            return;
        }
        m_label_Name.text = data.ModelName;
        m_label_Des.text = data.ModelDescription;

        if (m_RTObj != null)
        {
            m_RTObj.Release();
        }

        m_RTObj = DataManager.Manager<RenderTextureManager>().CreateRenderTextureObj((int)ModelID, 800);
        if (m_RTObj == null)
        {
            return;
        }
    
        // 0 1.52  0    7   45   0    5 
        m_RTObj.SetCamera(new Vector3(0, data.quanOffsetY * 0.01f, 0), Vector3.zero, -data.quanDistance * 0.01f);
        //m_RTObj.SetCamera(new Vector3(0, 1.52f, 0), Vector3.zero, -4.89f);

        if (data.type == 1)
        {
            //eulerX = 0;
            m_label_BiaoTi.text = "珍兽详情";
            
        }
        else 
        {
            //eulerX = -90;
            m_label_BiaoTi.text = "神兵详情";
            m_RTObj.AddLinkEffectWithoutEntity(10002);
            //CreateEffect(ModelID);
           
        }
        m_RTObj.SetModelRotateY(rotateY);
        //人物 
        if (m__Model != null)
        {
            m__Model.mainTexture = m_RTObj.GetTexture();
            m__Model.MakePixelPerfect();
        }
    }

    void DragModel(GameObject go, UnityEngine.Vector2 delta)
    {
        if (m_RTObj != null)
        {
            rotateY += -0.5f * delta.x;
            m_RTObj.SetModelRotateY(rotateY);
        }
    }

    protected override void OnHide()
    {
        base.OnHide();
        Release();
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_RTObj != null)
        {
            m_RTObj.Release();
        }

    }
    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }

}
