using UnityEngine;
using System.Collections;
using DG.Tweening;
namespace WellFired
{
    /// <summary>
    /// A custom event that alters the color of a gameobject at a given time. 
    /// </summary>
    [USequencerFriendlyName("LoadScene")]
    [USequencerEvent("Render/LoadScene")]
	[USequencerEventHideDuration()]
    public class USLoadScene : USEventBase
    {
        public uint sceneID = 0;
        public override USEventType GetEventType()
        {
            return USEventType.US_LoadScene;
        }
        
        public override void FireEvent()
        {
            if (!AffectedObject)
                return;

            if (!Application.isPlaying && Application.isEditor)
            {


            }
            else
            {
                table.MapDataBase data = GameTableManager.Instance.GetTableItem<table.MapDataBase>(sceneID);
                if (data == null)
                {
                    return;
                }
                table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(data.dwResPath);
                if (resDB == null)
                {
                    return;
                }

                Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
                if (rs != null)
                {
                    string str = resDB.strPath;
                    Engine.IScene scene = rs.EnterScene(ref str, SequencerManager.Instance());
                    if (scene != null)
                    {
                        ///加载场景
                        SequencerManager.Instance().Pause();
                        
                        scene.StartLoad(Vector3.one);

                        //保存下mapid
                        Client.IMapSystem map = Client.ClientGlobal.Instance().GetMapSystem();
                        SequencerManager.Instance().m_MapID = map.GetMapID();
                    }
                }
            }

        }

        public override void ProcessEvent(float deltaTime)
        {


        }

        public override void StopEvent()
        {
            UndoEvent();
        }

        public override void UndoEvent()
        {
            if (!AffectedObject)
                return;

        }
    }
}