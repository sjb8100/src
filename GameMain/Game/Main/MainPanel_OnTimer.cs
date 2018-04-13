
//*************************************************************************
//	创建日期:	2017/9/9 星期六 18:09:04
//	文件名称:	MainPanel_OnTimer
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using Client;
using Engine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public partial class MainPanel
{
    private readonly uint MAIN_TIMER_ID = 1010;
    private readonly uint m_uRefreshEnemyTimerID = 1020;//刷新敌人列表
    private readonly uint m_uSkillLongPressTimerID = 2000;
    private readonly uint m_uUseSkillTimerID = 10058;
    uint m_uRefreshEnemytime = 1000;//1s刷新一次
    StringBuilder m_pingText = new StringBuilder(20);
    void RefreshTime()
    {
        if (m_label_LabelPingandTime != null)
        {
            m_pingText.Remove(0, m_pingText.Length);
            m_pingText.Append(DateTime.Now.ToShortTimeString());
            int pingValue = (int)Protocol.Instance.Ping;
            if (pingValue > 0)
            {
                if (pingValue <= 100)
                {
                    m_pingText.Append(string.Format(" [00ff00]Ping:{0}ms[-]", pingValue));
                }
                else if (pingValue > 100 && pingValue <= 300)
                {
                    m_pingText.Append(string.Format(" [FFFF00]Ping:{0}ms[-]", pingValue));
                }
                else
                {
                    m_pingText.Append(string.Format(" [FF0000]Ping:{0}ms[-]", pingValue));
                }
            }

            m_label_LabelPingandTime.text = m_pingText.ToString();
        }
    }
    public void OnTimer(uint uTimerID)
    {
        if (uTimerID == MAIN_TIMER_ID)
        {
            RefreshTime();
            if (ClientGlobal.Instance().MainPlayer == null)
            {
                return;
            }

            //Vector3 pos = ClientGlobal.Instance().MainPlayer.GetPos();

            //m_label_coordinate.text = string.Format("{0},{1}", (int)(pos.x + 0.5f), (int)(-pos.z + 0.5f));
            //ping值要被动触发 时间60s更新一次 不然 DateTime.Now.ToShortTimeString()频繁触发gc modify by dianyu
        

            if (m_effctSelect != null)
            {
                if (m_effctSelect.GetNode() == null)
                {
                    return;
                }
                if (m_effctSelect.GetNode().GetTransForm() == null)
                {
                    return;
                }
                if (m_effctSelect.GetNode().GetTransForm().gameObject.activeSelf)
                {
                    SetSelectTargetEffectPos();
                }

            }
        } if (uTimerID == m_uUseSkillTimerID)
        {
            UseSkill(nPrePareSkillID);
        }
        else if (uTimerID == m_uSkillLongPressTimerID)
        {
            Log.LogGroup("ZDY", "long attack ......");
            //长按事件
            //Debug.LogError("long attack");
            ReqUseSkill(m_uCommonAttackSkillID);
        }
        else if (uTimerID == m_uRefreshEnemyTimerID)
        {
            Client.IEntity entity = ClientGlobal.Instance().GetControllerSystem().GetActiveCtrl().GetCurTarget();
            if (entity != null)
            {
                NetService.Instance.Send(new GameCmd.stEnmityDataUserCmd_CS() { npcid = entity.GetID() });
            }
        }
    }
}
