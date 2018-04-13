using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using table;
using Engine;
using Client;
using GameCmd;
using System.Text;
using Common;

partial class ShakePhonePanel : UIPanelBase
{
    int timeLimit = 5;
    uint treeEntityID = 0;
    protected override void OnLoading()
    {
        base.OnLoading();
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SHAKEINGPHONEMSG, EventCallBack);
    }
    protected override void OnPanelBaseDestory()
    {
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SHAKEINGPHONEMSG, EventCallBack);
    }
    void EventCallBack(int nEventID, object param)
    {
        if (nEventID == (int)Client.GameEventID.SHAKEINGPHONEMSG)
        {
            ExecuteShakeOver();
        }
    }
    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);

    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        timeLimit = 5;
        if(data != null && data is uint )
        {
            treeEntityID = (uint)data;
        }
    }
    protected override void OnHide()
    {
        base.OnHide();      
    }
    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
        DataManager.Manager<TreasureManager>().ShakingPhone = false;
    }
    void ExecuteShakeOver() 
    {     
        if (timeLimit > 0)
        {         
            NetService.Instance.Send(new stShakeMoneyTreePropertyUserCmd_CS());
            //Engine.Utility.Log.Error("SHAKEINGPHONEMSG");
            timeLimit--;
            CreateEffect();
        }
       
    }
    void onClick_ShakeBtn_Btn(GameObject caster)
    {
        ExecuteShakeOver();
    }

    uint CreateEffect()
    {
        Client.IEntity target = ClientGlobal.Instance().GetEntitySystem().FindNPC(treeEntityID);
        if (target == null)
        {
            Engine.Utility.Log.Error("摇钱树实体查找不到，id为{0}", treeEntityID);
            return 0;
        }
        FxResDataBase edb = GameTableManager.Instance.GetTableItem<FxResDataBase>(10001);
        if (edb != null && target != null)
        {
            AddLinkEffect node = new AddLinkEffect();
            node.nFollowType = (int)edb.flowType;
            node.rotate = new Vector3(edb.rotate[0], edb.rotate[1], edb.rotate[2]);
            node.vOffset = new Vector3(edb.offset[0], edb.offset[1], edb.offset[2]);

            // 使用资源配置表
            ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<ResourceDataBase>(edb.resPath);
            if (resDB == null)
            {
                Engine.Utility.Log.Error("EffectViewFactory:找不到特效资源路径配置{0}", edb.resPath);
                return 0;
            }
            node.strEffectName = resDB.strPath;
            node.strLinkName = edb.attachNode;
            if (node.strEffectName.Length != 0)
            {
                int id = (int)target.SendMessage(EntityMessage.EntityCommand_AddLinkEffect, node);

                return (uint)id;
            }
        }

        return 0;
    }
}   
