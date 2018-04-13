using UnityEngine;
using System.Collections;
using DG.Tweening;
namespace WellFired
{
    /// <summary>
    /// A custom event that alters the color of a gameobject at a given time. 
    /// </summary>
    [USequencerFriendlyName("ShakeCamera")]
    [USequencerEvent("Render/ShakeCamera")]
	//[USequencerEventHideDuration()]
    public class USShakeCamera : USEventBase
    {
        public override USEventType GetEventType()
        {
            return USEventType.US_ShakeCamera;
        }
        public float strength = 3f;
        public int vibrato = 10;
        public float randomness = 90f;

        public override void FireEvent()
        {
            if (!AffectedObject)
                return;

            if (!Application.isPlaying && Application.isEditor)
            {
                Camera camera = AffectedObject.GetComponent<Camera>();
                if (camera != null)
                    camera.DOShakePosition(Duration, strength, vibrato, 40);

            }
            else
            {
                Camera camera = AffectedObject.GetComponent<Camera>();
                if (camera != null)
                    camera.DOShakePosition(Duration, strength, vibrato, 40);
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