using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Client;

// RenderTextureObject

public interface IRenerTextureObj
{
    // 释放
    void Release();
    // 获取贴图
    Texture GetTexture();

    // 获取ID
    uint GetID();

    /**
    @brief 开启/关闭
    */
    void Enable(bool bEnable);

    /**
    @brief 设置镜头参数
    @param tarOffset 镜头目标与模型原点偏移
    @param rotate 镜头的旋转参数
    @param fDistance 镜头与目标的距离
    */
    void SetCamera(Vector3 tarOffset, Vector3 rotate, float fDistance);

    /// <summary>
    /// 设置展示摄像机
    /// </summary>
    /// <param name="tarOffset"></param>
    /// <param name="rotate"></param>
    void SetDisplayCamera(Vector3 tarOffset, Vector3 rotate);

    /// <summary>
    /// 设置展示摄像机
    /// </summary>
    /// <param name="tarOffsetStr"></param>
    /// <param name="rotateStr"></param>
    void SetDisplayCamera(string tarOffsetStr, string rotateStr, float YAngle);

    //void SetRenderCamera(Vector3 tarOffset, Vector3 pos, Vector3 rotate, float fDistance);

    /**
    @brief Y轴旋转值
    @param 
    */
    float YAngle
    {
        get;
    }

    /**
    @brief 设置模型绕Y轴旋转角度
    @param fAngle 角度值（单位：角度）
    */
    void SetModelRotateY(float fAngle,float fEulerX = 0);

    void SetModelEulerAnglesX(float fEuler); 

    /**
    @brief 设置模型缩放
    @param fScale
    */
    void SetModelScale(float fScale);

    /**
    @brief 设置模型绕Y轴旋转角度
    @param fAngle 角度值（单位：角度）
    */
    void PlayModelAni(string strAni);

    /**
    @brief 修改装备部位
    @param 
    */
    void ChangeSuit(Client.EquipPos pos, int nSuitID);

    /**
     @brief 没有实体的模型挂接特效
     @param 
    */
    void AddLinkEffectWithoutEntity(uint fxResID);
}

class RenderTextureObj : IRenerTextureObj
{
    // 渲染对象
    private Engine.IRenderObj m_RenderObj = null;

    // 角色复制对象
    private Client.IPuppet m_PuppetObj = null;
    private static uint s_uPuppetID = 1000000000; // 从10亿开始

    // 镜头
    private GameObject m_Camera = null;
    // RenderTarget
    private RenderTexture m_RenderText = null;
    // 对象节点
    private GameObject m_Root = null;

    // Y轴旋转角
    private float m_fYAngle = 0.0f;

    // id
    private uint m_uID = 0;

    //挂接特效id
    private uint ufxid = 0;

    public RenderTextureObj(uint uid)
    {
        m_uID = uid;
    }

    /**
    @brief Y轴旋转值
    @param 
    */
    public float YAngle
    {
        get { return m_fYAngle; }
    }

    public uint GetID()
    {
        return m_uID;
    }

    public void Release()
    {
        Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
        if (rs == null)
        {
            Engine.Utility.Log.Error("获取RenderSystem失败！");
            return;
        }


        if (m_PuppetObj != null)
        {
            ClientGlobal.Instance().GetEntitySystem().RemoveEntity(m_PuppetObj);
            m_PuppetObj = null;
            m_RenderObj = null;
        }
        if (m_RenderObj!= null)
        {
            rs.RemoveRenderObj(m_RenderObj);
        }

        if (m_Camera != null)
        {
            GameObject.DestroyImmediate(m_Camera);
        }
        if (m_RenderText != null)
        {
            m_RenderText.Release();
            GameObject.DestroyImmediate(m_RenderText);
            m_RenderText = null;
        }


        if (m_Root != null)
        {
            GameObject.DestroyImmediate(m_Root);
            m_Root = null;
        }

    }
    //-------------------------------------------------------------------------------------------------------
    // 获取贴图
    public Texture GetTexture()
    {
        if (m_RenderText == null)
        {
            return null;
        }

        return m_RenderText;
    }
    //-------------------------------------------------------------------------------------------------------
    /**
    @brief 开启/关闭
    */
    public void Enable(bool bEnable)
    {
        if (m_PuppetObj != null)
        {
            m_PuppetObj.SendMessage(Client.EntityMessage.EntityCommand_SetVisible, bEnable);
        }

        if (m_Root != null)
        {
            m_Root.SetActive(bEnable);
            // 默认播放站立动作
            m_RenderObj.Play(Client.EntityAction.Stand);
        }

    }
    //-------------------------------------------------------------------------------------------------------
    /**
    @brief 设置镜头参数
    @param tarOffset 镜头目标与模型原点偏移
    @param rotate 镜头的旋转参数
    @param fDistance 镜头与目标的距离
    */
    public void SetCamera(Vector3 tarOffset, Vector3 rotate, float fDistance)
    {
        // 计算位置
        Vector3 offset = ClacCameraOffset(rotate, fDistance);
        Vector3 pos = tarOffset + offset;

        if (m_Camera != null && m_RenderObj != null && m_RenderObj.GetNode() != null && m_RenderObj.GetNode().GetTransForm() != null)
        {
            m_Camera.transform.localPosition = pos;
            m_Camera.transform.LookAt(m_RenderObj.GetNode().GetTransForm().position + tarOffset, Vector3.up);
        }
    }

    /// <summary>
    /// 模型展示Camera
    /// </summary>
    /// <param name="tarOffset"></param>
    /// <param name="rotate"></param>
    public void SetDisplayCamera(Vector3 tarOffset, Vector3 rotate)
    {
        if (m_Camera != null && m_RenderObj != null && m_RenderObj.GetNode() != null && m_RenderObj.GetNode().GetTransForm() != null)
        {
            m_Camera.transform.localRotation = Quaternion.Euler(rotate);
            m_Camera.transform.localPosition = tarOffset;
        }
    }

    /// <summary>
    /// 模型展示Camera
    /// </summary>
    /// <param name="tarOffsetStr"></param>
    /// <param name="rotateStr"></param>
    public void SetDisplayCamera(string tarOffsetStr, string rotateStr, float YAngle)
    {
        m_fYAngle = YAngle;
        Vector3 targetOffset = Vector3.zero;
        string[] tempArray = null;
        if (!string.IsNullOrEmpty(tarOffsetStr))
        {
            tempArray = tarOffsetStr.Split(new char[] { '_' });
            if (null != tempArray && tempArray.Length == 3)
            {
                float tempValue = 0;
                if (float.TryParse(tempArray[0],out tempValue))
                {
                    targetOffset.x = tempValue;
                }
                
                if (float.TryParse(tempArray[1], out tempValue))
                {
                    targetOffset.y = tempValue;
                }

                if (float.TryParse(tempArray[2], out tempValue))
                {
                    targetOffset.z = tempValue;
                }
            }
        }

        Vector3 rotate = Vector3.zero;
        if (!string.IsNullOrEmpty(rotateStr))
        {
            tempArray = rotateStr.Split(new char[] { '_' });
            if (null != tempArray && tempArray.Length == 3)
            {
                float tempValue = 0;
                if (float.TryParse(tempArray[0], out tempValue))
                {
                    rotate.x = tempValue;
                }

                if (float.TryParse(tempArray[1], out tempValue))
                {
                    rotate.y = tempValue;
                }

                if (float.TryParse(tempArray[2], out tempValue))
                {
                    rotate.z = tempValue;
                }
            }
        }
        SetDisplayCamera(targetOffset, rotate);
        SetModelRotateY(m_fYAngle);
    }

    //public void SetRenderCamera(Vector3 tarOffset, Vector3 pos, Vector3 rotate, float fDistance)
    //{
    //    m_Camera.transform.Rotate(rotate);
    //    m_Camera.transform.localPosition = pos;

    //    m_Camera.transform.LookAt(m_RenderObj.GetNode().GetTransForm().position + tarOffset, Vector3.up);
    //}

    //-------------------------------------------------------------------------------------------------------
    /**
    @brief 设置模型绕Y轴旋转角度
    @param fAngle 角度值（单位：角度）
    @param fEulerX X角度值  针对物品没有正方向,模型设置角度时调整为直立，拖拽也是要有正方向X
    */

    public void SetModelRotateY(float fAngle, float fEulerX = 0)
    {
        if (m_RenderObj == null)
        {
            return;
        }

        m_fYAngle = fAngle;

        if (m_PuppetObj != null)
        {
            m_PuppetObj.SendMessage(EntityMessage.EntityCommand_SetRotateY, fAngle);
        }
        else
        {
            Quaternion rot = new Quaternion();
            rot.eulerAngles = new Vector3(fEulerX, fAngle, 0);
            if (m_RenderObj != null)
            {
                if (m_RenderObj.GetNode() != null)
                {
                    m_RenderObj.GetNode().SetLocalRotate(rot);
                  
                }
            }
        }
    }
    public void SetModelEulerAnglesX(float fEuler)
    {
        if (m_RenderObj == null)
        {
            return;
        }
        m_RenderObj.GetNode().GetTransForm().eulerAngles = new Vector3(fEuler, 0, 0);
    }
    //-------------------------------------------------------------------------------------------------------
    /**
    @brief 设置模型缩放
    @param fScale
    */
    public void SetModelScale(float fScale)
    {
        if (m_RenderObj == null)
        {
            return;
        }

        m_RenderObj.GetNode().SetScale(new Vector3(fScale, fScale, fScale));
    }
    //-------------------------------------------------------------------------------------------------------
    /**
    @brief 设置模型绕Y轴旋转角度
    @param fAngle 角度值（单位：角度）
    */
    public void PlayModelAni(string strAni)
    {
        if (m_PuppetObj != null)
        {
            PlayAni anim_param = new PlayAni();
            anim_param.strAcionName = strAni;
            anim_param.fSpeed = 1;
            anim_param.nStartFrame = 0;
            anim_param.nLoop = -1;
            anim_param.fBlendTime = 0.2f;
            m_PuppetObj.SendMessage(EntityMessage.EntityCommand_PlayAni, anim_param);
        }
        else
        {
            if (m_RenderObj != null)
            {
                m_RenderObj.Play(strAni);
            }
        }
    }
    //-------------------------------------------------------------------------------------------------------
    /**
    @brief 修改装备部位
    @param 
    */
    public void ChangeSuit(Client.EquipPos pos, int nSuitID)
    {
        if (m_PuppetObj == null)
        {
            return;
        }

        ChangeEquip changeEquip = new ChangeEquip();
        changeEquip.pos = pos;
        changeEquip.nSuitID = nSuitID;
        changeEquip.nLayer = (int)Engine.RenderLayer.ShowModel;
        if (changeEquip.pos == EquipPos.EquipPos_Body)
        {
            changeEquip.evt = OnChangeEquip;
            changeEquip.param = pos;
        }
        changeEquip.nStateType = (int)DataManager.Manager<LearnSkillDataManager>().CurState;

        m_PuppetObj.SendMessage(EntityMessage.EntityCommand_ChangeEquip, changeEquip);
    }
    private void OnChangeEquip(Engine.IRenderObj obj, object param)
    {
        EquipPos pos = (EquipPos)param;
        if (pos == EquipPos.EquipPos_Body)
        {
            m_RenderObj = m_PuppetObj.renderObj;
            if (m_RenderObj != null)
            {
                // 默认播放站立动作
                m_RenderObj.Play(Client.EntityAction.Stand);
                m_RenderObj.SetLayer((int)Engine.RenderLayer.ShowModel); // showModel
                if(ufxid != 0)
                {
                    //换了时装特效就不在新的RenderObj上了  重新挂一下
                  AddLinkEffectWithoutEntity(ufxid);
                }
            }

            if (m_Root != null && m_Camera != null)
            {
                m_Camera.transform.parent = m_Root.transform;
                m_RenderObj.GetNode().GetTransForm().parent = m_Root.transform;

                m_RenderObj.GetNode().SetLocalPosition(Vector3.zero);
                SetModelRotateY(m_fYAngle);
                m_RenderObj.GetNode().SetScale(Vector3.one);
            }
        }
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    public bool Create(string strRenderObj, int nSize, GameObject parentObj)
    {
        Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
        if (rs == null)
        {
            Engine.Utility.Log.Error("获取RenderSystem失败！");
            return false;
        }

        // 对象创建
        if (m_Root == null)
        {
            m_Root = new GameObject("RTObj");
            if (parentObj != null)
            {
                m_Root.transform.parent = parentObj.transform;
            }
        }

        // 
        m_RenderObj = null;
        rs.CreateRenderObj(ref strRenderObj, ref m_RenderObj, CreateRenderObjEvent, null, Engine.TaskPriority.TaskPriority_Normal);
        if (m_RenderObj == null)
        {
            Engine.Utility.Log.Error("创建Renderobj失败{0}！", strRenderObj);
            return false;
        }

        m_RenderObj.SetLayer((int)Engine.RenderLayer.ShowModel);
        // 默认播放站立动作
        m_RenderObj.Play(Client.EntityAction.Stand);

        // CreateCamera
        Camera cam = CreateCamera();

        //  m_RenderText = new RenderTexture(nSize, nSize, 24, RenderTextureFormat.ARGB32);
        m_RenderText = RenderTexture.GetTemporary(nSize, nSize, 24, RenderTextureFormat.ARGB4444);
        if (m_RenderText == null)
        {
            return false;
        }
        cam.targetTexture = m_RenderText;

        // 对象挂在父节点上
        if (m_Root != null)
        {
            m_RenderObj.GetNode().GetTransForm().parent = m_Root.transform;
            m_Camera.transform.parent = m_Root.transform;
        }

        return true;
    }
    //-------------------------------------------------------------------------------------------------------
    public bool Create(Client.IPlayer player, int nSize, GameObject parentObj, Client.SkillSettingState eState = Client.SkillSettingState.StateOne)
    {
        if (player == null)
        {
            return false;
        }

        Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
        if (rs == null)
        {
            Engine.Utility.Log.Error("获取RenderSystem失败！");
            return false;
        }

        // 对象创建
        if (m_Root == null)
        {
            m_Root = new GameObject("RTObj");
            if (parentObj != null)
            {
                m_Root.transform.parent = parentObj.transform;
            }
        }

        // 
        m_RenderObj = null;

        Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            return false;
        }

        List<GameCmd.SuitData> lstSuit = null;
        player.GetSuit(out lstSuit);

        return Create(lstSuit, player.GetProp((int)PlayerProp.Job), player.GetProp((int)PlayerProp.Sex), nSize, parentObj, eState);
    }

    public bool Create(List<GameCmd.SuitData> lstSuit, int nJob, int nSex, int nSize, GameObject parentObj, SkillSettingState eState = SkillSettingState.StateOne)
    {
        Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
        if (rs == null)
        {
            Engine.Utility.Log.Error("获取RenderSystem失败！");
            return false;
        }

        // 对象创建
        if (m_Root == null)
        {
            m_Root = new GameObject("RTObj");
            if (parentObj != null)
            {
                m_Root.transform.parent = parentObj.transform;
            }
        }

        // 
        m_RenderObj = null;

        Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            return false;
        }

        Client.EntityCreateData data = new Client.EntityCreateData();
        data.ID = ++s_uPuppetID;
        data.strName = "";

        //int speed = player.GetProp((int)WorldObjProp.MoveSpeed);
        data.PropList = new EntityAttr[(int)PuppetProp.End - (int)EntityProp.Begin];
        int index = 0;
        data.PropList[index++] = new EntityAttr((int)PuppetProp.Job, nJob);
        data.PropList[index++] = new EntityAttr((int)PuppetProp.Sex, nSex);
        //data.PropList[index++] = new EntityAttr((int)EntityProp.BaseID, 0);
        //data.PropList[index++] = new EntityAttr((int)WorldObjProp.MoveSpeed, speed);
        data.eSkillState = eState;
        data.bViewModel = true;
        // 处理时装外观数据
        EntityViewProp[] propList = new EntityViewProp[(int)Client.EquipPos.EquipPos_Max];
        index = 0;
        propList[index++] = new EntityViewProp((int)Client.EquipPos.EquipPos_Body, 0);
        propList[index++] = new EntityViewProp((int)Client.EquipPos.EquipPos_Weapon, 0);

        if (lstSuit.Count > 0)
        {
            for (int i = 0; i < lstSuit.Count; ++i)
            {
                if (lstSuit[i] == null)
                {
                    continue;
                }

                int pos = GetPropPos((Client.EquipPos)lstSuit[i].suit_type, propList);
                if (pos >= 0)
                {
                    propList[pos] = new EntityViewProp((int)lstSuit[i].suit_type, (int)lstSuit[i].baseid);
                }
                else
                {
                    Client.EquipPos equipPos = (Client.EquipPos)lstSuit[i].suit_type;
                    propList[index++] = new EntityViewProp((int)equipPos, (int)lstSuit[i].baseid);
                }
            }
        }

        data.ViewList = propList;
        data.nLayer = LayerMask.NameToLayer("ShowModel");

        m_PuppetObj = es.CreateEntity(Client.EntityType.EntityType_Puppet, data, true) as Client.IPuppet;
        if (m_PuppetObj == null)
        {
            Engine.Utility.Log.Error("创建Renderobj失败{0}！", "");
            return false;
        }

        m_RenderObj = m_PuppetObj.renderObj;
        if (m_RenderObj == null)
        {
            return false;
        }

        // 默认播放站立动作
        m_RenderObj.Play(Client.EntityAction.Stand);
        m_RenderObj.SetLayer((int)Engine.RenderLayer.ShowModel); // showModel

        // CreateCamera
        Camera cam = CreateCamera();

        m_RenderText = new RenderTexture(nSize, nSize, 24, RenderTextureFormat.ARGB32);
        if (m_RenderText == null)
        {
            return false;
        }
        cam.targetTexture = m_RenderText;

        // 对象挂在父节点上
        if (m_Root != null)
        {
            m_RenderObj.GetNode().GetTransForm().parent = m_Root.transform;
            m_Camera.transform.parent = m_Root.transform;
        }

        return true;
    }
    //-------------------------------------------------------------------------------------------------------
    private static bool IsExistProp(Client.EquipPos pos, EntityViewProp[] propList)
    {
        if (propList == null)
        {
            return false;
        }

        for (int i = 0; i < propList.Length; ++i)
        {
            if (propList[i] == null)
            {
                continue;
            }

            if (propList[i].nPos == (int)pos)
            {
                return true;
            }
        }

        return false;
    }
    private static int GetPropPos(Client.EquipPos pos, EntityViewProp[] propList)
    {
        if (propList == null)
        {
            return -1;
        }

        for (int i = 0; i < propList.Length; ++i)
        {
            if (propList[i] == null)
            {
                continue;
            }

            if (propList[i].nPos == (int)pos)
            {
                return i;
            }
        }

        return -1;
    }
    //-------------------------------------------------------------------------------------------------------
    public void SetYOffset(float fOffset)
    {
        if (m_Root != null)
        {
            m_Root.transform.localPosition = new Vector3(0, fOffset, 0);
        }
    }
    //-------------------------------------------------------------------------------------------------------
    void CreateRenderObjEvent(Engine.IRenderObj obj, object param)
    {
        m_RenderObj = obj;
        if (m_RenderObj != null)
        {
            m_RenderObj.SetLayer((int)Engine.RenderLayer.ShowModel); // showModel
        }
    }

    //-------------------------------------------------------------------------------------------------------
    private Vector3 ClacCameraOffset(Vector3 rotate, float fDistance)
    {
        Quaternion rot = new Quaternion();
        rot.eulerAngles = new Vector3(rotate.x, rotate.y, rotate.z);
        Matrix4x4 mat = new Matrix4x4();
        mat.SetTRS(Vector3.zero, rot, Vector3.one);
        return -mat.GetColumn(2) * fDistance;
    }
    //-------------------------------------------------------------------------------------------------------
    private Camera CreateCamera()
    {
        if (m_Camera != null)
        {
            GameObject.DestroyImmediate(m_Camera);
            m_Camera = null;
        }
        m_Camera = new GameObject("RTCamera");
        if (m_Camera == null)
        {
            return null;
        }

        Camera cam = m_Camera.AddComponent<Camera>();
        if (cam == null)
        {
            return null;
        }

        cam.backgroundColor = new Color(0.5f, 0.5f, 0.5f, 0); // 背景为黑色
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.cullingMask = (1 << 8); // showModel
        cam.renderingPath = RenderingPath.Forward;
        cam.useOcclusionCulling = false;
        cam.fieldOfView = 45.0f;        // 45度

        //调试暂定  后面要注销
        //cam.farClipPlane = 10;

        return cam;
    }
    /// <summary>
    /// 给没有实体的模型挂接特效
    /// </summary>   effect资源表id
    /// <param name="fxResID"></param>
    public void  AddLinkEffectWithoutEntity(uint fxResID) 
    {
        if (m_RenderObj != null)
        {
            table.FxResDataBase edb = GameTableManager.Instance.GetTableItem<table.FxResDataBase>(fxResID);
            if (edb != null)
            {
                ufxid = fxResID;
                AddLinkEffect node = new AddLinkEffect();
                node.nFollowType = (int)edb.flowType;
                node.rotate = new Vector3(edb.rotate[0], edb.rotate[1], edb.rotate[2]);
                node.vOffset = new Vector3(edb.offset[0], edb.offset[1], edb.offset[2]);
                Quaternion rot = new Quaternion();
                rot.eulerAngles = node.rotate;
                // 使用资源配置表
                table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(edb.resPath);
                if (resDB == null)
                {
                    Engine.Utility.Log.Error("EffectViewFactory:找不到特效资源路径配置{0}", edb.resPath);
                    return ;
                }
                node.strEffectName = resDB.strPath;
                node.strLinkName = edb.attachNode;

                 m_RenderObj.AddLinkEffect(ref node.strEffectName, ref node.strLinkName, node.vOffset, rot, node.scale, (Engine.LinkFollowType)node.nFollowType);


            }
        }
    }
}

