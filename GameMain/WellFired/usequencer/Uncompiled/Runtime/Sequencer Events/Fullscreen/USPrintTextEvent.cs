using UnityEngine;
using System.Collections;

namespace WellFired
{
    /// <summary>
    /// A custom event that let's you print text to screen at a given time, over a given duration. 
    /// 
    /// You can get a typewritter like effect with the printRatePerCharacter field.
    /// </summary>
    [USequencerFriendlyName("Print Text")]
    [USequencerEvent("Fullscreen/Print Text")]
    public class USPrintTextEvent : USEventBase
    {
        public override USEventType GetEventType()
        {
            return USEventType.US_PrintTextEvent;
        }

        /// <summary>
        /// The UILayer to display this text on.
        /// </summary>
        public UILayer uiLayer = UILayer.Front;

        /// <summary>
        /// The text we will print.
        /// </summary>
        public string textToPrint = "";

        /// <summary>
        /// The position at which to print the text.
        /// </summary>
        public Rect position = new Rect(0, 0, Screen.width, Screen.height);

        /// <summary>
        /// The print rate per character, alter this for a typewritter effect.
        /// </summary>
        public float printRatePerCharacter = 0.0f;

        private string priorText = "";
        private string currentText = "";
        private bool display = false;


        public int fontSize = 20;

        public int offsetX = 0;
        public int offsetY = 0;

        public override void FireEvent()
        {
            priorText = currentText;
            currentText = textToPrint;

            if (Duration > 0.0f)
                currentText = "";

            display = true;
        }

        public override void ProcessEvent(float deltaTime)
        {
            if (printRatePerCharacter <= 0.0f)
                currentText = textToPrint;
            else
            {
                int numCharacters = (int)(deltaTime / printRatePerCharacter);

                if (numCharacters < textToPrint.Length)
                    currentText = textToPrint.Substring(0, numCharacters);
                else
                    currentText = textToPrint;
            }

            display = true;
        }

        public override void StopEvent()
        {
            UndoEvent();
        }

        public override void UndoEvent()
        {
            currentText = priorText;
            display = false;
        }

        public override void EndEvent()
        {
            display = false;
        }

        void OnGUI()
        {
            if (!Sequence.IsPlaying)
                return;

            if (!display)
                return;

            int previousDepth = GUI.depth;

            position.x = Screen.width - offsetX;
            position.y = offsetY;

            GUIStyle bb = new GUIStyle();
            bb.normal.textColor = new Color(1, 1, 1);
            bb.fontSize = fontSize;
            bb.wordWrap = true;

            GUI.depth = (int)uiLayer;
            GUI.Label(position, currentText, bb);

            GUI.depth = previousDepth;
        }
    }
}

