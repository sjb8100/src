using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// 主要用于模型展示 观察一个对象
public class RenderTextureManager : BaseModuleData,IManager
{
    #region Property
    //renderTexture root
    private GameObject renderTexturePool = null;

    private uint m_uIDSeed = 0;
    //private 
    //名称
    public string CLASS_NAME
    {
        get
        {
            return this.GetType().Name;
        }
    }
    #endregion

    #region IManager Method
    public void ClearData()
    {

    }
    public void Initialize()
    {
        if (null == renderTexturePool)
        {
            renderTexturePool = new GameObject("RTPool");
            GameObject.DontDestroyOnLoad(renderTexturePool);
        }
    }

    public void Reset(bool depthClearData = false)
    {
        
    }

    public void Process(float deltaTime)
    {
    }
    #endregion

    #region method

    // 根据资源ID创建
    public IRenerTextureObj CreateRenderTextureObj(int nResID, int nSize)
    {
        if (renderTexturePool==null)
        {
            return null;
        }

        table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>((uint)nResID);
        if (resDB == null)
        {
            Engine.Utility.Log.Error("创建模型资源路径配置{0}", nResID);
            return null;
        }

        RenderTextureObj obj = new RenderTextureObj(++m_uIDSeed);
        if (!obj.Create(resDB.strPath, nSize, renderTexturePool))
        {
            obj = null;
            return null;
        }

        obj.SetYOffset(obj.GetID() * 10);
        return (IRenerTextureObj)obj;
    }

    // 创建玩家的预览对象
    public IRenerTextureObj CreateRenderTextureObj(Client.IPlayer player, int nSize)
    {
        if (player == null)
        {
            return null;
        }

        if (renderTexturePool == null)
        {
            return null;
        }

        RenderTextureObj obj = new RenderTextureObj(++m_uIDSeed);
        if (!obj.Create(player, nSize, renderTexturePool, (Client.SkillSettingState)player.GetProp((int)Client.PlayerProp.SkillStatus)))
        {
            obj = null;
            return null;
        }

        obj.SetYOffset(obj.GetID() * 10);
        return (IRenerTextureObj)obj;
    }
    // 根据外观数据创建预览对象
    public IRenerTextureObj CreateRenderTextureObj(List<GameCmd.SuitData> lstSuit, int nJob, int nSex, int nSize, Client.SkillSettingState eState = Client.SkillSettingState.StateOne)
    {
        if (renderTexturePool == null)
        {
            return null;
        }

        RenderTextureObj obj = new RenderTextureObj(++m_uIDSeed);
        if (!obj.Create(lstSuit, nJob, nSex, nSize, renderTexturePool, eState))
        {
            obj = null;
            return null;
        }

        obj.SetYOffset(obj.GetID() * 10);
        return (IRenerTextureObj)obj;
    }

    // 根据职业和性别创建
    public IRenerTextureObj CreateRenderTextureObj(int Job, int nSex, int nSize)
    {
        if (renderTexturePool == null)
        {
            return null;
        }

        var table_data = table.SelectRoleDataBase.Where((GameCmd.enumProfession)Job, (GameCmd.enmCharSex)nSex);
        string bodyPath = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(table_data.bodyPathID).strPath;

        string strResName = bodyPath;
        RenderTextureObj obj = new RenderTextureObj(++m_uIDSeed);
        if (!obj.Create(strResName, nSize, renderTexturePool))
        {
            obj = null;
            return null;
        }

        obj.SetYOffset(obj.GetID() * 10);
        return (IRenerTextureObj)obj;
    }
    /// <summary>
    /// 根据模型路径创建
    /// </summary>
    /// <param name="modelPath"></param>
    /// <param name="nSize"></param>
    /// <returns></returns>
    public IRenerTextureObj CreateRenderTextureObj(string modelPath , int nSize)
    {
        if ( renderTexturePool == null )
        {
            return null;
        }
        string strResName = modelPath;
        RenderTextureObj obj = new RenderTextureObj( ++m_uIDSeed );
        if ( !obj.Create( strResName , nSize , renderTexturePool ) )
        {
            obj = null;
            return null;
        }

        obj.SetYOffset(obj.GetID() * 10);
        return (IRenerTextureObj)obj;
    }
    #endregion
}