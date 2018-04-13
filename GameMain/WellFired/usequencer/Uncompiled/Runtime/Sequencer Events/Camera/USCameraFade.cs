using UnityEngine;
using System.Collections;
using System;

namespace WellFired
{
    /// <summary>
    /// A custom event that will dissolve one camera into another camera.
    /// </summary>
    [USequencerFriendlyName("Set Fade")]
    [USequencerEvent("Camera/Fade")]
    [USequencerEventHideDuration]
    //[ExecuteInEditMode]
    public class USCameraFade : USEventBase
    {
        public override USEventType GetEventType()
        {
            return USEventType.US_CameraFade;
        }

        private Camera cameraToAffect;
        ScreenCameraFade fade = null;

        public bool bOutFade = true;


        public override void FireEvent()
        {
            if (!Application.isPlaying && Application.isEditor)
            {

            }
            else
            {

                if (AffectedObject != null)
                {
                    fade = AffectedObject.AddComponent<ScreenCameraFade>();
                    fade.fTimeFade = this.Duration;
                    fade.bOutFade = bOutFade;
                }
            }

        }

        public override void ProcessEvent(float deltaTime)
        {


        }

        public override void EndEvent()
        {
            UndoEvent();
        }

        public override void StopEvent()
        {
            UndoEvent();
        }

        public override void UndoEvent()
        {
            if (AffectedObject != null)
            {
                if (fade != null)
                {
                    GameObject.DestroyObject(fade);
                    fade = null;
                }
            }

        }



    }
}