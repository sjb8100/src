using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Engine;

namespace Client
{
    public enum SuitPos
    {
        None = 0,
        Cloth,
        Weapon,
        Back,
        Face,
        Wing,
    }

    public class Avater
    {
        public class SuitData
        {
            public SuitInfo info = null;
            public IRenderObj obj = null;
        }

        private IRenderObj m_obj = null;
        private int m_nLayer = 0;
        private byte m_byProfession = 0;
        private byte m_bySex = 0;
        private string m_strAniName = "";
        private float m_fBlendTime = 0f;
        private int m_nLoop;




        private Action<object> m_CallBack = null;
        private object m_Param = null;
        private List<SuitData> m_lstSuitData = new List<SuitData>();

        /// <summary>
        /// 获取渲染对象
        /// </summary>
        public IRenderObj RenderObj
        {
            get
            {
                return m_obj;
            }
        }


        public IRenderObj GetWeapon()
        {
            for (int i = 0; i < m_lstSuitData.Count; i++)
            {
                SuitData data = m_lstSuitData[i];
                if (data.info.pos == SuitPos.Weapon)
                    return data.obj;
            }
            return null;
        }

        /// <summary>
        /// 仅仅创建一个模型
        /// </summary>
        /// <param name="body"></param>
        /// <param name="weapon"></param>
        /// <param name="wing"></param>
        /// <returns></returns>
        public bool CreateAvatar(GameObject parent, List<SuitInfo> lstSuit, int nLayer = 0, Action<object> callback = null, object param = null)
        {
            m_CallBack = callback;
            m_Param = param;

            IRenderSystem rs = RareEngine.Instance().GetRenderSystem();
            if (rs == null)
            {
                return false;
            }

            for (int i = 0; i < lstSuit.Count;++i)
            {
                m_lstSuitData.Add(new SuitData
                {
                    info = lstSuit[i],
                    obj = null,
                });
            }
                

            m_nLayer = nLayer;
            for (int i = 0; i < m_lstSuitData.Count; ++i)
            {
                if(m_lstSuitData[i]==null)
                {
                    continue;
                }

                if (m_lstSuitData[i].info.pos == SuitPos.Cloth)
                {
                    //Engine.Utility.Log.Error("Create Body");
                    //先创建身体
                    rs.CreateRenderObj(ref m_lstSuitData[i].info.modelPath, ref m_obj, OnCreateBodyEvent, m_lstSuitData[i].info.pos, TaskPriority.TaskPriority_Normal, true);
                    if(m_obj != null)
                    {
                        // 挂接父节点
                        Vector3 rot = m_obj.GetNode().GetTransForm().localEulerAngles;
                        m_obj.GetNode().GetTransForm().parent = parent.transform;
                        m_obj.GetNode().GetTransForm().localPosition = Vector3.zero;
                        m_obj.GetNode().GetTransForm().localEulerAngles = rot;
                        m_obj.GetNode().GetTransForm().localScale = Vector3.one;
                    }
                    else
                    {
                        Engine.Utility.Log.Error("obj is null path is " + m_lstSuitData[i].info.modelPath);
                    }
                }
            }

            return true;
        }

        //-------------------------------------------------------------------------------------------------------
        string GetModelPath(uint uSuitID, byte profession, byte sex)
        {
            return "";
        }
        /**
        @brief 对象销毁
        */
        public void Destroy()
        {
            if (m_obj != null)
            {
                IRenderSystem rs = RareEngine.Instance().GetRenderSystem();
                if (rs == null)
                {
                    return;
                }

                if (m_obj.GetNode() == null)
                    return;

                if (m_obj.GetNode().GetTransForm() == null)
                    return;


                if (m_obj.GetNode().GetTransForm().parent != null)
                {
                    m_obj.GetNode().GetTransForm().parent = null;
                }

                rs.RemoveRenderObj(m_obj);
                m_obj = null;

                for (int i = 0; i < m_lstSuitData.Count;++i)
                {
                    if(m_lstSuitData[i].obj!=null)
                    {
                        rs.RemoveRenderObj(m_lstSuitData[i].obj);
                    }
                }

                //Engine.Utility.Log.Error("Destroy Avater");
            }
        }

        public void PlayAni(string strAniName, ActionEvent evt, float fBlendTime = -1f, int nLoop = -1, object param = null)
        {
            m_strAniName = strAniName;
            m_nLoop = nLoop;
            m_fBlendTime = fBlendTime;
            if (m_obj != null)
            {
                m_obj.SetAniCallback(evt, param);
                m_obj.Play(m_strAniName, 0, 1, fBlendTime, nLoop);
            }
        }

        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 对象加载回调
        @param 
        */
        private void OnCreateRenderObj(Engine.IRenderObj obj, object param)
        {
            if (obj == null)
            {
                return;
            }

            //Engine.Utility.Log.Error("OnCreateRenderObj {0}", obj.GetName());

            SuitPos pos = SuitPos.None;
            if(param is SuitPos)
            {
                pos = (SuitPos)param;
            }

            if (m_obj == null)
            {
                Engine.Utility.Log.Error("OnCreateRenderObj is null pos is " + pos);
                return;
            }

            obj.SetLayer(m_nLayer);

            for (int i = 0; i < m_lstSuitData.Count; ++i)
            {
                if (m_lstSuitData[i] == null)
                {
                    continue;
                }

                if(m_lstSuitData[i].info.pos == pos)
                {
                    m_obj.AddLinkObj(obj, ref m_lstSuitData[i].info.locatorName, Vector3.zero, Quaternion.identity);
                }
            }

            m_obj.Play(m_strAniName, 0, 1, m_fBlendTime, m_nLoop);
            // 回调
            if (m_CallBack != null)
            {
                m_CallBack(m_Param);
            }
        }
        //-------------------------------------------------------------------------------------------------------
        private void OnCreateBodyEvent(Engine.IRenderObj obj, object param)
        {
            if (obj == null)
            {
                return;
            }

            m_obj = obj;

            IRenderSystem rs = RareEngine.Instance().GetRenderSystem();
            if (rs == null)
            {
                return;
            }

            //Engine.Utility.Log.Error("OnCreateBodyEvent {0}", m_obj.GetName());

            for (int i = 0; i < m_lstSuitData.Count; ++i)
            {
                if (m_lstSuitData[i] == null)
                {
                    continue;
                }

                if (m_lstSuitData[i].info.pos != SuitPos.Cloth)
                {
                    //Engine.Utility.Log.Error("Create {0}", m_lstSuitData[i].info.pos.ToString());
                    rs.CreateRenderObj(ref m_lstSuitData[i].info.modelPath, ref m_lstSuitData[i].obj, OnCreateRenderObj, m_lstSuitData[i].info.pos, TaskPriority.TaskPriority_Normal, false);
                }
            }

            if (m_lstSuitData.Count == 1 && m_lstSuitData[0].info.pos == SuitPos.Cloth)
            {
                OnCreateRenderObj(obj, SuitPos.None);
            }

            m_obj.SetLayer(m_nLayer);

        }
    }
}
