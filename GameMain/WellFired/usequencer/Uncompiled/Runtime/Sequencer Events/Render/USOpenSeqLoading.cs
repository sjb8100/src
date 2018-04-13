using UnityEngine;
using System.Collections;
using DG.Tweening;
namespace WellFired
{
    /// <summary>
    /// A custom event that alters the color of a gameobject at a given time. 
    /// </summary>
    [USequencerFriendlyName("OpenSeqLoading")]
    [USequencerEvent("Render/OpenSeqLoading")]
	[USequencerEventHideDuration()]
    public class USOpenSeqLoading : USEventBase
    {
        public override USEventType GetEventType()
        {
            return USEventType.US_OpenSeqLoading;
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
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.StoryLoadingPanel);
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