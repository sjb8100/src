using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Client;

namespace EntitySystem
{
    class EntityVisual : BaseCommpent
    {
        public EntityVisual(Entity owner)
        {
            m_Owner = owner;
        }

        public bool Create()
        {
            RegisterMessageDelegate(EntityMessage.EntityCommand_SetPos, SetPos);
            RegisterMessageDelegate(EntityMessage.EntityCommand_SetRotateX, SetRotateX);
            RegisterMessageDelegate(EntityMessage.EntityCommand_SetRotateY, SetRotateY);
            RegisterMessageDelegate(EntityMessage.EntityCommand_SetRotate, SetRotate);
            RegisterMessageDelegate(EntityMessage.EntityCommand_GetRotateY, GetRotateY);
            RegisterMessageDelegate(EntityMessage.EntityCommand_PlayAni, PlayAni);
            RegisterMessageDelegate(EntityMessage.EntityCommand_PauseAni, PauseAni);
            RegisterMessageDelegate(EntityMessage.EntityCommand_GetCurAni, GetCurAni);
            RegisterMessageDelegate(EntityMessage.EntityCommand_ResumeAni, ResumeAni);
            RegisterMessageDelegate( EntityMessage.EntityCommand_ClearAniCallback , ClearAniCallback );
            RegisterMessageDelegate(EntityMessage.EntityCommand_SetAniSpeed, SetAniSpeed);
            RegisterMessageDelegate(EntityMessage.EntityCommand_ChangePart, ChangePart);
            RegisterMessageDelegate(EntityMessage.EntityCommand_EnableShadow, EnableShadow);
            RegisterMessageDelegate(EntityMessage.EntityCommand_AddLinkObj, AddLinkObj);
            RegisterMessageDelegate(EntityMessage.EntityCommand_RemoveLinkObj, RemoveLinkObj);
            RegisterMessageDelegate(EntityMessage.EntityCommand_RemoveAllLinkObj, RemoveAllLinkObj);
            RegisterMessageDelegate(EntityMessage.EntityCommand_AddLinkEffect, AddLinkEffect);
            RegisterMessageDelegate(EntityMessage.EntityCommand_RemoveLinkEffect, RemoveLinkEffect);
            RegisterMessageDelegate(EntityMessage.EntityCommand_RemoveLinkAllEffect, RemoveAllLinkEffect);
            RegisterMessageDelegate(EntityMessage.EntityCommand_SetVisible, SetVisible);
            RegisterMessageDelegate(EntityMessage.EntityCommand_IsVisible, IsVisible);
            RegisterMessageDelegate(EntityMessage.EntityCommand_EnableShowModel, ShowModel);
            RegisterMessageDelegate(EntityMessage.EntityCommand_IsShowModel, IsShowModel);
            RegisterMessageDelegate(EntityMessage.EntityCommand_LookAt, SetLookAt);
            RegisterMessageDelegate(EntityMessage.EntityCommand_RotateY, RotateY);
            RegisterMessageDelegate(EntityMessage.EntityCommand_LookTarget, LookTarget);
            RegisterMessageDelegate(EntityMessage.EntityCommond_SetAlpha, SetAlpha);
            RegisterMessageDelegate(EntityMessage.EntityCommond_SetColor, SetColor);
            RegisterMessageDelegate(EntityMessage.EntityCommand_FlashColor, SetFlashColor);
            RegisterMessageDelegate(EntityMessage.EntityCommand_ChangeMaterial, ChangeMaterial);
            RegisterMessageDelegate(EntityMessage.EntityCommond_SetName, SetName);
            RegisterMessageDelegate(EntityMessage.EntityCommond_ChangeNameColor, ChangeNameColor);
            RegisterMessageDelegate(EntityMessage.EntityCommand_Change, Change);
            RegisterMessageDelegate(EntityMessage.EntityCommand_Restore, Restore);
            RegisterMessageDelegate(EntityMessage.EntityCommand_IsChange, IsChange);
            RegisterMessageDelegate(EntityMessage.EntityCommand_ChangeEquip, ChangeEquip);
            RegisterMessageDelegate(EntityMessage.EntityCommand_AddText, AddText);
            RegisterMessageDelegate(EntityMessage.EntityCommand_RemoveText, RemoveText);
            RegisterMessageDelegate(EntityMessage.EntityCommand_ModifyText, ModifyText);
            RegisterMessageDelegate(EntityMessage.EntityCommand_ModifyColor, ModifyColor);
            RegisterMessageDelegate(EntityMessage.EntityCommand_AddHPBar, AddHPBar);
            RegisterMessageDelegate(EntityMessage.EntityCommand_RemoveHPBar, RemoveHPBar);
            RegisterMessageDelegate(EntityMessage.EntityCommand_ModifyHP, ModifyHP);
            RegisterMessageDelegate(EntityMessage.EntityCommond_Ride, Ride);
            RegisterMessageDelegate(EntityMessage.EntityCommond_UnRide, UnRide);
            RegisterMessageDelegate(EntityMessage.EntityCommond_IsRide, IsRide);
            
            return true;
        }
        public override void Update()
        {

        }
        public void Close()
        {
            Clear();
        }

        private object RemoveLinkObj(object param)
        {
            if( m_Owner!= null )
            {
                int nID = (int)param;
                m_Owner.RemoveLinkObj(nID, true);
            }

            return null;
        }

        private object AddLinkObj(object param)
        {
            if( m_Owner != null )
            {
                AddLinkObj add = (AddLinkObj)param;
                Quaternion rot = new Quaternion();
                rot.eulerAngles = add.rotate;
                return m_Owner.AddLinkObj(ref add.strRenderObj, ref add.strLinkName, add.vOffset, rot, (Engine.LinkFollowType)add.nFollowType);
            }

            return null;
        }

        private object RemoveAllLinkObj(object param)
        {
            if (m_Owner != null)
            {
                m_Owner.RemoveAllLinkObject();
            }

            return null;
        }
        
        private object AddLinkEffect(object param)
        {
            if (m_Owner != null)
            {
                AddLinkEffect add = (AddLinkEffect)param;
                Quaternion rot = new Quaternion();
                rot.eulerAngles = add.rotate;

                EntityView ev = m_Owner.GetView();
                if (ev != null)
                {
                    return ev.AddLinkEffect(ref add.strEffectName, ref add.strLinkName, add.vOffset, rot,add.scale, (Engine.LinkFollowType)add.nFollowType, add.bIgnoreRide, add.nLevel,add.callback,add.param);
                }
            }

            return null;
        }

        private object RemoveLinkEffect(object param)
        {
            if (m_Owner != null)
            {
                int nID = (int)param;
                m_Owner.RemoveLinkEffect(nID, true);
            }

            return null;
        }

        private object RemoveAllLinkEffect(object param)
        {
            if(m_Owner!=null)
            {
                m_Owner.RemoveAllLinkEffect();
            }
            return null;
        }

        private object SetVisible(object param)
        {
            bool bVisible = (bool)param;
            if(m_Owner!=null)
            {
                m_Owner.SetVisible(bVisible);
            }
            return null;
        }
        private object IsVisible(object param)
        {
            if(m_Owner!=null)
            {
                EntityView ev = m_Owner.GetView();
                if (ev != null)
                {
                    return ev.IsVisible();
                }
            }
            return false;
        }
        // 显示模型
        private object ShowModel(object param)
        {
            if (m_Owner != null)
            {
                bool bEnable = (bool)param;
                EntityView ev = m_Owner.GetView();
                if (ev != null)
                {
                    ev.ShowModel(bEnable);
                }
            }
            return null;
        }
        private object IsShowModel(object param)
        {
            if (m_Owner != null)
            {
                EntityView ev = m_Owner.GetView();
                if (ev != null)
                {
                    return ev.IsShowModel();
                }
            }
            return false;
        }
        private object SetAlpha(object param)
        {
            float alpha = (float)param;
            if ( m_Owner != null )
            {
                m_Owner.SetAlpha( alpha );
            }
            return null;
        }
        private object SetColor(object param)
        {
            Color color = (Color)param;
            if ( m_Owner != null )
            {
                EntityView ev = m_Owner.GetView();
                if (ev != null)
                {
                    ev.SetColor(color);
                }
            }
            return null;
        }

        private object SetFlashColor(object param)
        {
            FlashColor color = (FlashColor)param;
            if (color==null)
            {
                color = new FlashColor();
            }
            if ( m_Owner != null )
            {
                EntityView ev = m_Owner.GetView();
                if (ev != null)
                {
                    ev.SetFlashColor(color.color,color.fLift);
                }
            }
            return null;
        }

        //-------------------------------------------------------------------------------------------------------
        private object ChangeMaterial(object param)
        {
            string strShaderName = (string)param;
            if ( m_Owner != null )
            {
                EntityView ev = m_Owner.GetView();
                if (ev != null)
                {
                    ev.ChangeMaterial(strShaderName);
                }
            }
            return null;
        }
        
        private object SetName(object param)
        {
            if(m_Owner==null)
            {
                return null;
            }
            string strName = param as string;
            if (strName != null)
            {
               
                m_Owner.SetName(strName);

                stEntityChangename changeName = new stEntityChangename();
                changeName.uid = m_Owner.GetUID();
                // 发送事件
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ENTITYSYSTEM_CHANGENAME, changeName);
            }
            else
            {
                Engine.Utility.Log.Error( "设置实体{0}名称参数错误:不是字符串" , m_Owner.GetName() );
            }
            return null;
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 变身
        @param 
        */
        private object Change(object param)
        {
            if (m_Owner == null)
            {
                return null;
            }

            ChangeBody changeInfo = (ChangeBody)param;
            EntityView ev = m_Owner.GetView();
            if (ev != null)
            {
                ev.Change(changeInfo);
                return null;
            }

            return null;
        }
        //-------------------------------------------------------------------------------------------------------
        private object Restore(object param)
        {
            if (m_Owner == null)
            {
                return null;
            }

            EntityView ev = m_Owner.GetView();
            if (ev != null)
            {
                ev.Restore((Client.ChangeBody)param);
                return null;
            }

            return null;
        }

        private object IsChange(object param)
        {
            if (m_Owner == null)
            {
                return false;
            }
            EntityView ev = m_Owner.GetView();
            if (ev != null)
            {
                return ev.IsChange();
            }
            return false;
        }

        // 换装
        private object ChangeEquip(object param)
        {
            if (m_Owner == null)
            {
                return null;
            }

            EntityView ev = m_Owner.GetView();
            if (ev != null)
            {
                ev.ChangeEquip((Client.ChangeEquip)param);
                return null;
            }

            return null;
        }


        private object Ride(object param)
        {
            if (m_Owner != null)
            {
                int nRideID = (int)param;
                EntityView ev = m_Owner.GetView();
                if(ev!=null)
                {
                    ev.Ride(nRideID);
                }
            }

            return null;
        }

        private object UnRide(object param)
        {
            if (m_Owner != null)
            {
                EntityView ev = m_Owner.GetView();
                if (ev != null)
                {
                    ev.UnRide();
                }
            }

            return null;
        }

        private object IsRide(object param)
        {
            if (m_Owner != null)
            {
                EntityView ev = m_Owner.GetView();
                if (ev != null)
                {
                    return ev.IsRide();
                }
            }
            return false;
        }

        private object ChangePart(object param)
        {
            if( m_Owner != null )
            {
                ChangePart part = (ChangePart)param;
                m_Owner.ChangePart(ref part.strPartName, ref part.strResName);
            }

            return null;
        }
        private object EnableShadow(object param)
        {
            if (m_Owner != null)
            {
                bool bEnable = (bool)param;
                EntityView ev = m_Owner.GetView();
                if (ev != null)
                {
                    ev.EnableShadow(bEnable);
                }
            }

            return null;
        }
        //-------------------------------------------------------------------------------------------------------
        private object PlayAni(object param)
        {
            if( m_Owner != null )
            {
                PlayAni ani = (PlayAni)param;
                m_Owner.PlayAction(ani.strAcionName, ani.nStartFrame, ani.fSpeed, ani.fBlendTime, ani.nLoop, ani.aniCallback, ani.callbackParam);
            }
            return null;
        }
        //-------------------------------------------------------------------------------------------------------
        private object PauseAni(object param)
        {
            if( m_Owner != null )
            {
                m_Owner.PauseAni();
            }

            return null;
        }

        // 获取当前动画名称
        private object GetCurAni(object param)
        {
            if (m_Owner != null)
            {
                return m_Owner.GetView().GetCurAniName();
            }

            return null;
        }

        private object ResumeAni(object param)
        {
            if (m_Owner != null)
            {
                m_Owner.ResumeAni();
            }

            return null;
        }
        
        private object ClearAniCallback(object param)
        {
            if (m_Owner != null)
            {
                m_Owner.ClearAniCallback();
            }

            return null;
        }
        //-------------------------------------------------------------------------------------------------------
        private object SetAniSpeed(object param)
        {
            if (m_Owner != null)
            {
                EntityView ev = m_Owner.GetView();
                if (ev != null)
                {
                    float speed = (float)param;
                    ev.SetAniSpeed(speed);
                }
            }

            return null;
        }
        private object SetRotate(object param)
        {
            if( m_Owner != null )
            {
                Vector3 angle = (Vector3)param;
                m_Owner.SetRotate(angle);
            }

            return null;
        }

        private object GetRotateY(object param)
        {
            if( m_Owner != null )
            {
                return m_Owner.GetView().GetRotate().y;
            }

            return 0.0f;
        }
        //-------------------------------------------------------------------------------------------------------
        private object RotateY(object param)
        {
            if (m_Owner != null)
            {
                float angle = (float)param;
                m_Owner.RotateY(angle);
            }

            return null;
        }
        private object LookTarget(object param)
        {
            if(m_Owner!=null)
            {
                Vector3 target = (Vector3)param;
                target.y = 0;
                Vector3 pos = m_Owner.GetPos();
                pos.y = 0;
                if(target.Equals(pos))
                {
                    return null;
                }
                Vector3 dir = target - pos;
                dir.Normalize();

                // 先设置人物旋转
                Quaternion rotate = new Quaternion();
                rotate.SetLookRotation(dir, Vector3.up);
                m_Owner.SetRotate(rotate.eulerAngles);
            }
            return null;
        }
        private object SetRotateY(object param)
        {
            if (m_Owner != null)
            {
                float angle = (float)param;
                m_Owner.RotateToY(angle);
            }

            return null;
        }

        private object SetRotateX(object param)
        {
            if (m_Owner != null)
            {
                float angle = (float)param;
                m_Owner.RotateToX(angle);
            }

            return null;
        }
        private object SetLookAt(object param)
        {
            if (m_Owner != null)
            {
                LookAt lookup = (LookAt)param;
                m_Owner.LookAt(lookup.vTarget,lookup.vUp);
            }

            return null;
         }

        private object AddText(object param)
        {
            if (m_Owner != null)
            {
                AddText addText = (AddText)param;
                EntityView ev = m_Owner.GetView();
                if (ev != null)
                {
                    Engine.FontID fontID = Engine.FontID.Fzy4jw; 
                    string strFont = addText.strFontName.ToLower();
                    if(strFont.Contains("fzy4jw"))
                    {
                        fontID = Engine.FontID.Fzy4jw;
                    }
                    else if(strFont.Contains("fzy4fw"))
                    {
                        fontID = Engine.FontID.Fzy4fw;
                    }
                    return ev.AddRenderText(addText.strText, fontID, addText.nFontSize, addText.fCharSize, addText.color, addText.strLocatorName, addText.offset);
                }
            }

            return 0;
        }

        private object RemoveText(object param)
        {
            if (m_Owner != null)
            {
                EntityView ev = m_Owner.GetView();
                if (ev != null)
                {
                    int id = (int)param;
                    ev.RemoveRenderText(id);
                }
            }

            return null;
        }

        private object ChangeNameColor(object param)
        {
            if (m_Owner != null)
            {
                EntityView ev = m_Owner.GetView();
                if (ev != null)
                {
                    Color c = (Color)param;
                    ev.ChangeNameColor(c);
                }
            }

            return null;
        }

        private object ModifyText(object param)
        {
            if (m_Owner != null)
            {
                EntityView ev = m_Owner.GetView();
                if (ev != null)
                {
                    ModifyText modify = (ModifyText)param;
                    ev.ModifyText(modify.nLinkID,modify.strText);
                }
            }

            return null;
        }

        private object ModifyColor(object param)
        {
            if (m_Owner != null)
            {
                EntityView ev = m_Owner.GetView();
                if (ev != null)
                {
                    ModifyColor modify = (ModifyColor)param;
                    ev.ModifyColor(modify.nLinkID, modify.color);
                }
            }

            return null;
        }

        private object AddHPBar(object param)
        {
            if (m_Owner != null)
            {
                EntityView ev = m_Owner.GetView();
                if (ev != null)
                {
                    AddHPBar addhpbar = (AddHPBar)param;
                    ev.AddHPBar(addhpbar.fProgress, addhpbar.strLocatorName, addhpbar.offset);
                }
            }

            return null;
        }

        private object RemoveHPBar(object param)
        {
            if (m_Owner != null)
            {
                EntityView ev = m_Owner.GetView();
                if (ev != null)
                {
                    ev.RemoveHPBar();
                }
            }

            return null;
        }

        private object ModifyHP(object param)
        {
            if (m_Owner != null)
            {
                EntityView ev = m_Owner.GetView();
                if (ev != null)
                {
                    ModifyHP modify = (ModifyHP)param;
                    ev.ModifyHPProgress(modify.fProgress);
                }
            }

            return null;
        }

        private object SetPos(object param)
        {
            if (m_Owner != null)
            {
                Vector3 pos = (Vector3)param;
                Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
                if (rs == null)
                {
                    return null;
                }
                // 取下地表高度
                // 再计算移动速度
                Engine.IScene curScene = rs.GetActiveScene();
                if (curScene == null)
                {
                    return null;
                }
                Engine.TerrainInfo info;
                if (curScene.GetTerrainInfo(ref pos, out info))
                {
                    //if (pos.y < info.pos.y)
                    {
                        pos.y = info.pos.y;// +0.08f; // 往上提一个距离
                    }
                }

                //if (pos.y <= 10f)
                //{
                //    if (!EntitySystem.m_ClientGlobal.IsMainPlayer(m_Owner))
                //    {
                //        Vector3 mainPos = EntitySystem.m_ClientGlobal.MainPlayer.GetPos();
                //        pos.y = mainPos.y;
                //    }
                //}


                //if (EntitySystem.m_ClientGlobal.IsMainPlayer(m_Owner))
                //{
                //    Engine.Utility.Log.LogGroup("XXF", "Entity.SetPos ({0},{1})", pos.x,-pos.z);
                //}

                m_Owner.SetPos(ref pos);
            }
            else
            {
                Engine.Utility.Log.LogGroup("XXF", "Entity.SetPos m_Owner is null");
            }

            return null;
        }
    }
}
