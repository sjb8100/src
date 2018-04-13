using UnityEngine;
using System.Collections;

namespace WellFired
{
    /// <summary>
    /// A custom event that alters the color of a gameobject at a given time. 
    /// </summary>
    [USequencerFriendlyName("Open Dialog")]
    [USequencerEvent("Render/Open Dialog")]
    [USequencerEventHideDuration()]
    public class USOpenDialog : USEventBase
    {
        public override USEventType GetEventType()
        {
            return USEventType.US_OpenDialog;
        }
        public int dialogID;
        public string backgroundImage = "";

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

            }
            else
            {
                if (dialogID == 0)
                {
                    StoryPanel.StoryData data = new StoryPanel.StoryData();
                    data.Des = "";
                    data.BgTexPath = backgroundImage;
                    data.ShowSkip = SequencerManager.Instance().IsShowSkipSequencerBtn();
                    data.SkipDlg = SequencerManager.Instance().OnSkipSequencer;
                    data.ColliderClickDlg = SequencerManager.Instance().OnClickSequencer;

                    DataManager.Manager<UIPanelManager>().ShowStory(data);
                }
                else
                {
                    string strText = SequencerManager.Instance().GetDialogText((uint)dialogID);
                    StoryPanel.StoryData data= new StoryPanel.StoryData();
                    data.Des = strText;
                    data.BgTexPath = backgroundImage;
                    data.ShowSkip = SequencerManager.Instance().IsShowSkipSequencerBtn();
                    data.SkipDlg = SequencerManager.Instance().OnSkipSequencer;
                    data.ColliderClickDlg = SequencerManager.Instance().OnClickSequencer;

                    DataManager.Manager<UIPanelManager>().ShowStory(data);
                }

            }
        }
        private void dlg()
        {

        }
        public override void EndEvent()
        {
            StoryPanel.StoryData data = new StoryPanel.StoryData();
            data.Des = "";
            data.ShowSkip = SequencerManager.Instance().IsShowSkipSequencerBtn();
            data.SkipDlg = SequencerManager.Instance().OnSkipSequencer;
            data.ColliderClickDlg = SequencerManager.Instance().OnClickSequencer;

            DataManager.Manager<UIPanelManager>().ShowStory(data);

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