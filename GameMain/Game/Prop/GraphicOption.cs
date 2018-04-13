using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// 游戏图形设置
public class GraphicOption
{
    private GameObject m_UIRoot = null;

    public bool ui
    {
        get { return m_bUI; }
        set
        {
            m_bUI = value;

            if (m_UIRoot == null)
            {
                m_UIRoot = GameObject.Find("ui_root");

            }
            if (m_UIRoot != null)
            {
                m_UIRoot.SetActive(m_bUI);
            }
        }
    }
    public bool model
    {
        get { return m_bModel; }
        set
        {
            m_bModel = value;
            Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
            if (es != null)
            {
                es.ShowEntity(m_bModel);
            }
        }
    }
    public bool headName
    {
        get{return m_bName;}
        set
        {
            m_bName = value;
            if (m_bName)
            {

                RoleStateBarManager.ShowHeadStatus();
            }
            else
            {
                RoleStateBarManager.HideHeadStatus();
            }
        }
    }

    public bool terrain
    {
        get { return m_bTerrain; }
        set
        {
            m_bTerrain = value;
            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs != null)
            {
                Engine.IScene scene = rs.GetActiveScene();
                if (scene != null)
                {
                    scene.ShowTerrain(m_bTerrain);
                }
            }
        }
    }

    public bool terrainObj
    {
        get { return m_bTerrainObj; }
        set
        {
            m_bTerrainObj = value;
            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs != null)
            {
                Engine.IScene scene = rs.GetActiveScene();
                if (scene != null)
                {
                    scene.ShowTerrainObj(m_bTerrainObj);
                }
            }
        }
    }

    public bool grass
    {
        get { return m_bGrass; }
        set
        {
            m_bGrass = value;
            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs != null)
            {
                Engine.IScene scene = rs.GetActiveScene();
                if (scene != null)
                {
                    scene.ShowGrass(m_bGrass);
                }
            }
        }
    }

    public bool shadow
    {
        get { return m_bShadow; }
        set
        {
            m_bShadow = value;
            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs != null)
            {
                if (!m_bShadow)
                {
                    rs.SetShadowLevel(Engine.ShadowLevel.None);
                }
                else
                {
                    rs.SetShadowLevel(Engine.ShadowLevel.Height);
                }
            }
        }
    }

    private bool m_bUI = true;
    private bool m_bModel = true;
    private bool m_bName = true;
    private bool m_bTerrain = true;
    private bool m_bTerrainObj = true;
    private bool m_bGrass = true;
    private bool m_bShadow = true;
}
