//*************************************************************************
//	创建日期:	2016/11/9 14:57:34
//	文件名称:	Protocol_ITimer
//   创 建 人:   zhudianyu	
//	版权所有:	中青宝
//	说    明:	协议相关定时器
//*************************************************************************

using Client;
using Common;
using Engine;
using GameCmd;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;
using Engine.Utility;
partial class Protocol:ITimer
{
    private const uint m_uHeartTimerID = 70001;
    private const uint m_uReconnectTimerID = 70002;
    private const uint m_uReadSliderTimerID = 1000;
    const uint m_uActiveTimerID = 70003;//切换到前台后1s检测心跳
    void ITimer.OnTimer(uint uTimerID)
    {
        switch(uTimerID)
        {
            case m_uReadSliderTimerID://技能读条进度条
                {
                    stNotifyUninterruptEventMagicUserCmd_CS endcmd = new stNotifyUninterruptEventMagicUserCmd_CS();
                    endcmd.etype = stNotifyUninterruptEventMagicUserCmd_CS.EventType.EventType_Over;
                    ClientGlobal.Instance().netService.Send( endcmd );
                }
                break;
            case m_uHeartTimerID:
                {
                    OnTimerSendHeart();
                }
                break;
            case m_uReconnectTimerID:
                {
                    OnTimerReconnectGameSever();
                }
                break;
            case 124:
                {
                    OnTimer124();
                }
                break;
            case m_uActiveTimerID:
                {
                    OnTimerSendHeart();
                }
                break;
            default:
                break;
        }
    }
}

