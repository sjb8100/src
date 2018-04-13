using UnityEngine;
using System.Collections;

namespace WellFired
{
    /// <summary>
    /// A custom event that alters the color of a gameobject at a given time. 
    /// </summary>
    [USequencerFriendlyName("Set Active")]
    [USequencerEvent("Render/Set Objects Active")]
    [USequencerEventHideDuration()]
    public class USSetActive : USEventBase
    {
        public override USEventType GetEventType()
        {
            return USEventType.US_SetActiveEvent;
        }
        public bool active;
        private bool bIsFireEvent = false;
        public override void FireEvent()
        {
            if (!AffectedObject)
                return;

            if (bIsFireEvent == true)
                return;

            bIsFireEvent = true;

            if (!Application.isPlaying && Application.isEditor)
            {
                //previousColor = AffectedObject.GetComponent<Renderer>().sharedMaterial.color;
                //AffectedObject.GetComponent<Renderer>().sharedMaterial.color = newColor;

                AffectedObject.SetActive(active);

            }
            else
            {
                //previousColor = AffectedObject.GetComponent<Renderer>().material.color;
                //AffectedObject.GetComponent<Renderer>().material.color = newColor;
                AffectedObject.SetActive(active);
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

            //if(!Application.isPlaying && Application.isEditor)
            //    AffectedObject.GetComponent<Renderer>().sharedMaterial.color = previousColor;
            //else
            //    AffectedObject.GetComponent<Renderer>().material.color = previousColor;
        }
    }
}