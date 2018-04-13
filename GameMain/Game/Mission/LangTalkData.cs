//*************************************************************************
//	创建日期:	2017-3-29 20:39
//	文件名称:	LangTalkData.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	langtext 任务对话文本解析
//*************************************************************************
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;


public class LangTalkData
{
    public enum NpcType
    {
        None,
        MissionTalk = 1,//任务有对话
        Transmit = 2,//传送点
        TransferNpc = 3,//传送车夫 
        MissionNoTalk = 4,//任务无对话
        TalkOnly = 5,
        MissionAutoReceive = 6,//任务自动接取
        //CityWarOnly = 7,       //仅城战用
    }

    public uint nTextID;
    public List<Talk> lstTalks = new List<Talk>();
    public uint nNpcId;
    public NpcType npcType = NpcType.None;
    public uint nTaskId;
    public string strStep;
    public List<Button> buttons = new List<Button>();
    //对话音效
    public uint[] talkVoice = null;
    public class Talk
    {
        public bool bUser = false;
        public string strText;
    }

    public class Button
    {
        public uint taskId;
        public string strBtnName;
        public uint nindex;
    }

    public static LangTalkData GetDataByCmd(GameCmd.stTalkDataScriptUserCmd_S cmd)
    {
        table.LangTextDataBase langtextDb = GameTableManager.Instance.GetTableItem<table.LangTextDataBase>(cmd.txt_id);
        LangTalkData data = new LangTalkData();
        data.strStep = cmd.step;
        if (langtextDb != null)
        {
            data.nTextID = langtextDb.dwID;

            uint npcId;
            if (GetNpcIdForDaliyRingTask(out npcId))
            {
                data.nNpcId = npcId;
            }
            else 
            {
                data.nNpcId = langtextDb.npcID;
            }
                    
            data.npcType = (NpcType)langtextDb.npcType;
            data.nTaskId = langtextDb.taskID;

            if (!string.IsNullOrEmpty(langtextDb.talkVoice))
            {
                string[] voices = langtextDb.talkVoice.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                data.talkVoice = new uint[voices.Length];
                for (int i = 0; i < voices.Length; i++)
                {
                    data.talkVoice[i] = uint.Parse(voices[i]);
                }
            }

            for (int i = 0; i < cmd.buttons.Count; ++i)
            {
                table.LangTextDataBase btnDb = GameTableManager.Instance.GetTableItem<table.LangTextDataBase>(cmd.buttons[i]);
                if (btnDb != null)
                {
                    LangTalkData.Button btn = new LangTalkData.Button();
                    btn.taskId = btnDb.taskID;
                    btn.nindex = (uint)i + 1;
                    btn.strBtnName = btnDb.strText;
                    data.buttons.Add(btn);
                }
            }

            IEnumerable<XNode> nodes = XDocument.Parse("<root>" + langtextDb.strText + "</root>").Root.Nodes();
            if (nodes != null)
            {
                foreach (var n in nodes)
                {
                    var e = n as XElement;
                    if (e != null)
                    {
                        switch (e.Name.ToString())
                        {
                            case "talk":
                                LangTalkData.Talk talk = new LangTalkData.Talk();
                                if (e.Attribute("type") != null)
                                {
                                    var type = int.Parse(e.AttributeValue("type"));
                                    talk.bUser = type == 2;
                                }
                                if (e.Attribute("text") != null)
                                {
                                    System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                                    string text = e.AttributeValue("text");
                                    string strColor = "";
                                    bool starttoken = false;
                                    for (int i = 0; i < text.Length; i++)
			                        {
                                        if (starttoken)
                                        {
                                            if (text[i] == '0' || text[i] == '1' || text[i] == '2' || text[i] == '3'||
                                                text[i] == '4' || text[i] == '5'|| text[i] == '6' || text[i] == '7'||
                                                text[i] == '8' || text[i] == '9'|| text[i] == 'a' || text[i] == 'b'||
                                                text[i] == 'c' || text[i] == 'd'|| text[i] == 'e' || text[i] == 'f'||
                                                text[i] == 'A' || text[i] == 'B'|| text[i] == 'C' || text[i] == 'D'||
                                                text[i] == 'E' || text[i] == 'F' || text[i] == '-')
                                            {
                                                strColor += text[i];
                                            }
                                            else if (text[i] == ']')
                                            {
                                                starttoken = false;
                                                strColor += ']';
                                                if (strColor.Length == 8)
                                                {
                                                    strColor = strColor.Replace("[", "<color value=\"#");
                                                    strColor = strColor.Replace("]", "\">");
                                                    stringBuilder.Append(strColor);
                                                }
                                                else if (strColor.Length == 3)
                                                {
                                                    strColor = strColor.Replace("[-]", "</color>");
                                                    stringBuilder.Append(strColor);
                                                }
                                                else
                                                {
                                                    stringBuilder.Append(strColor);
                                                }
                                                strColor = "";
                                            }
                                            else
                                            {
                                                starttoken = false;
                                                strColor += text[i];
                                                stringBuilder.Append(strColor);
                                            }
                                        }
                                        else if (text[i] == '[')
                                        {
                                            strColor = "[";
                                            starttoken = true;
                                        }
                                        else
                                        {
                                            starttoken = false;
                                            stringBuilder.Append(text[i]);
                                        }
			                        }
                                    talk.strText = stringBuilder.ToString();
                                }
                                data.lstTalks.Add(talk);
                                break;
                        }
                        continue;
                    }
                }
            }
        }
        else
        {
            Engine.Utility.Log.Error("Not Found LangTextDataBase id {0}", cmd.txt_id);
        }

        return data;
    }

    public static string GetTextById(uint text_id)
    {
        string text = "";

        table.LangTextDataBase langtextDb = GameTableManager.Instance.GetTableItem<table.LangTextDataBase>(text_id);
        if (langtextDb != null)
        {
            text = langtextDb.strText;
        }
        else
        {
            Engine.Utility.Log.Error("Not Found LangTextDataBase id {0}", text_id);
        }
        return text;
    }

    /// <summary>
    ///  日环任务对话npc
    /// </summary>
    /// <param name="npcId"></param>
    /// <returns></returns>
    static bool GetNpcIdForDaliyRingTask(out uint npcId)
    {
        npcId = 0;
        uint doingTaskId = DataManager.Manager<TaskDataManager>().DoingTaskID;
        table.QuestDataBase qdb = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(doingTaskId);

        if (qdb == null)
        {
            return false;
        }

        if (qdb.dwType == (uint)GameCmd.TaskType.TaskType_Loop)
        {
            if (qdb.dwSubType == (uint)TaskSubType.Talk)
            {
                npcId = qdb.dwEndNpc;
                return true;
            }
        }
        
        return false;
    }
}