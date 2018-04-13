/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Text
 * 创建人：  wenjunhua.zqgame
 * 文件名：  TextManager
 * 版本号：  V1.0.0.0
 * 创建时间：11/17/2016 4:50:59 PM
 * 描述：本地文本控制，敏感词检测，非法字符检测
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class TextManager : BaseModuleData,IManager
{
    #region IManager Method
    public void Initialize()
    {
        InitLocalText();
        //InitSensitive();
    }
    public void ClearData()
    {

    }
    public void Reset(bool depthClearData = false)
    {

    }

    public void Process(float deltaTime)
    {

    }
    #endregion

    #region LocalText
    /// <summary>
    /// 根据文本枚举获取本地文本
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public string GetLocalText(LocalTextType type)
    {
        int id = (int)type;
        return GetLocalText(id);
    }

    /// <summary>
    /// 根据Id获取本地文本
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GetLocalText(int id)
    {
        string text = "";
        if (null == textDic || !textDic.TryGetValue(id,out text))
        {
            Engine.Utility.Log.Error("TextManager->GetLocalText empty id={0}", id);
        }
        return text;
    }

    /// <summary>
    /// 根据枚举串
    /// </summary>
    /// <param name="enumString"></param>
    /// <returns></returns>
    public string GetLocalText(string enumString)
    {
        int id = 0;
        if (null != enumStringDic && enumStringDic.TryGetValue(enumString, out id))
        {
            return GetLocalText(id);
        }
        return "";
    }

    /// <summary>
    /// 获取文本枚举类型获取format text
    /// </summary>
    /// <param name="type"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public string GetLocalFormatText(LocalTextType type, params object[] param)
    {
        return GetLocalFormatText((int)type,param);
    }

    /// <summary>
    /// 获取文本ID获取format text
    /// </summary>
    /// <param name="id"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public string GetLocalFormatText(int id,params object[] param)
    {
        string text = GetLocalText(id);
        try
        {
            text = string.Format(text, param);
        }
        catch (Exception e)
        {
            Engine.Utility.Log.Error("TextManager->GetLocalFormatText error,LocalTextId:{0} ,info:{1}", id, e.ToString());
        }

        return text;
    }
    //<ID,Text>
    private Dictionary<int, string> textDic = null;
    private Dictionary<string, int> enumStringDic = null;
    /// <summary>
    /// 初始化本地文本
    /// </summary>
    private void InitLocalText()
    {
        List<table.LocalTextDataBase> localTxts = GameTableManager.Instance.GetTableList<table.LocalTextDataBase>();
        UILocalText.RegisterOnLocalText(GetLocalText);
        if (null == localTxts || localTxts.Count == 0)
        {
            Engine.Utility.Log.Error("读取本地表格错误");
            return;
        }

        if (null == enumStringDic)
        {
            enumStringDic = new Dictionary<string, int>();
        }
        enumStringDic.Clear();

        if (null == textDic)
        {
            textDic = new Dictionary<int, string>();
        }
        textDic.Clear();

        table.LocalTextDataBase ldb = null;
        for (int i = 0; i < localTxts.Count; i++)
        {
            ldb = localTxts[i];
            if (null == ldb)
                continue;
            if (!string.IsNullOrEmpty(ldb.enumString) && !enumStringDic.ContainsKey(ldb.enumString))
            {
                enumStringDic.Add(ldb.enumString, (int)ldb.id);
            }
            if (!textDic.ContainsKey((int)ldb.id))
            {
                textDic.Add((int)ldb.id, ldb.text);
            }
        }
    }

    /// <summary>
    /// 是不是纯数字
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsPureDigital(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return false;
        }
        if (System.Text.RegularExpressions.Regex.IsMatch(str,"^[0-9]+$"))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 是否匹配NGUI颜色
    /// </summary>
    /// <param name="str"></param>
    /// <param name="matchWords">返回匹配字符</param>
    /// <returns></returns>
    public static bool IsMatchNGUIColor(string str,out List<string> matchWords)
    {
        matchWords = new List<string>();
        if (string.IsNullOrEmpty(str))
        {
            return false;
        }
        string pattern = "\\[([0-9abcdef]|[0-9ABCDEF]){6,8}\\]";
        System.Text.RegularExpressions.Match mc = System.Text.RegularExpressions.Regex.Match(str, pattern);
        if (null != mc && mc.Success)
        {
            do
            {
                matchWords.Add(mc.Value);
                mc = mc.NextMatch();
            } while (null != mc && mc.Success);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 是否匹配NGUI颜色
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsMatchNGUIColor(string str)
    {
        if (!string.IsNullOrEmpty(str))
        {
            string pattern = "\\[([0-9abcdef]|[0-9ABCDEF]){6,8}\\]";
            System.Text.RegularExpressions.Match mc = System.Text.RegularExpressions.Regex.Match(str, pattern);
            if (null != mc && mc.Success)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 清除文本中的NGUI颜色
    /// </summary>
    /// <param name="str"></param>
    public static void ClearStrNGUIColor(ref string str)
    {
        if (!string.IsNullOrEmpty(str))
        {
            string pattern = "\\[([0-9abcdef]|[0-9ABCDEF]){6,8}\\]";
            System.Text.RegularExpressions.Match mc = System.Text.RegularExpressions.Regex.Match(str, pattern);
            if (null != mc && mc.Success)
            {
                do
                {
                    str = str.Replace(mc.Value, "");
                    mc = mc.NextMatch();
                } while (null != mc && mc.Success);
            }
        }
        
    }

    /// <summary>
    /// 取消所有空字符
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string RemoveAllSpace(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return "";
        }

        string result = str.Trim().Replace(" ","");
        return result;
    }
    public const string CONST_TAG_WORDS_FORMAT = "【{0}】";
    //名称最小字数
    public const uint CONST_NAME_MIN_WORDS = 2;
    //名称最大字数
    public const uint CONST_NAME_MAX_WORDS = 6;
    /// <summary>
    /// 鉴定一个输入名称是否合法
    /// </summary>
    /// <param name="title">名字类型</param>
    /// <param name="name">名称字符串</param>
    /// <param name="limitWordsMinNum">最小字数</param>
    /// <param name="limitWordsMaxNum">最大字数</param>
    /// <param name="errorTips">错误是否弹提示</param>
    /// <returns></returns>
    public bool IsLegalNameFormat(string title,string name,uint limitWordsMinNum,uint limitWordsMaxNum,bool errorTips = false)
    {
        List<string> txts = null;
        StringBuilder builder = null;
        string titleName = title + GetLocalText(LocalTextType.Local_TXT_Name);
        if (IsMatchNGUIColor(name,out txts))
        {
            if (errorTips)
            {
                builder = new StringBuilder();
                for (int i = 0; i < txts.Count; i++)
                {
                    builder.Append(string.Format(CONST_TAG_WORDS_FORMAT, txts[i].Substring(1,txts[i].Length -2)));
                    if (i < txts.Count-1)
                    {
                        builder.Append(",");
                    }
                }
                TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Local_TXT_Warning_FM_NGUIColor, titleName, builder.ToString());
            }
            return false;
        }
        //移除所有空格符号
        name = TextManager.RemoveAllSpace(name);
        if (TextManager.IsPureDigital(name))
        {
            if (errorTips)
                TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Local_TXT_Warning_FM_Digital, titleName);
            return false;
        }

        //长度判断
        uint charNum = TextManager.GetCharNumByStrInUnicode(name);
        if (charNum < TextManager.TransformWordNum2CharNum(limitWordsMinNum) ||
            charNum > TextManager.TransformWordNum2CharNum(limitWordsMaxNum))
        {
            if (errorTips)
            {
                string numString = limitWordsMinNum.ToString();
                if (limitWordsMaxNum != limitWordsMinNum)
                {
                    numString = limitWordsMinNum + "-" + limitWordsMaxNum;
                }
                TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Local_TXT_Warning_FM_NameLengthLimit, title, numString);
            }
            return false;
        }
        //非法字符判断
        if (!TextManager.IsLegalText(name))
        {
            if (errorTips)
            {
                TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Local_TXT_Warning_FM_IllegalChar
                     , titleName);
            }
            return false;
        }
        //敏感词判断
        if (DataManager.Manager<TextManager>().IsContainSensitiveWord(name, TextManager.MatchType.Max))
        {
            if (errorTips)
            {
                txts = GetSensitiveWorld(name, MatchType.Max);
                if (null != txts && txts.Count > 0)
                {
                    builder = new StringBuilder();
                    for (int i = 0; i < txts.Count; i++)
                    {
                        builder.Append(string.Format(CONST_TAG_WORDS_FORMAT, txts[i]));
                        if (i < txts.Count-1)
                        {
                            builder.Append(",");
                        }
                    }
                    TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Local_TXT_Warning_FM_Sensitive
                     , titleName,builder.ToString());
                }
            }
            
            return false;
        }
        return true;
    }

    #endregion

    #region LocalText Get
    /// <summary>
    /// 获取武魂属性开启星级描述
    /// </summary>
    /// <param name="attrIndex"></param>
    /// <returns></returns>
    public string GetMuhonAttrLockDes(EquipDefine.AttrIndex attrIndex)
    {
        string txt = "";
        switch(attrIndex)
        {
            case EquipDefine.AttrIndex.Second:
                txt = GetLocalText(LocalTextType.Local_TXT_Soul_TwoStarOpen);
                break;
            case EquipDefine.AttrIndex.Third:
                txt = GetLocalText(LocalTextType.Local_TXT_Soul_ThreeStarOpen);
                break;
            case EquipDefine.AttrIndex.Fourth:
                txt = GetLocalText(LocalTextType.Local_TXT_Soul_FourStarOpen);
                break;
            case EquipDefine.AttrIndex.Fifth:
                txt = GetLocalText(LocalTextType.Local_TXT_Soul_FiveStarOpen);
                break;
        }
        return txt;
    }

    public string GetMuhonStarName(uint starLv)
    {
        string txt = "";
        switch (starLv)
        {
            case 0:
                txt = GetLocalText(LocalTextType.Local_TXT_Soul_ZeroStar);
                break;
            case 1:
                txt = GetLocalText(LocalTextType.Local_TXT_Soul_OneStar);
                break;
            case 2:
                txt = GetLocalText(LocalTextType.Local_TXT_Soul_TwoStar);
                break;
            case 3:
                txt = GetLocalText(LocalTextType.Local_TXT_Soul_ThreeStar);
                break;
            case 4:
                txt = GetLocalText(LocalTextType.Local_TXT_Soul_FourStar);
                break;
            case 5:
                txt = GetLocalText(LocalTextType.Local_TXT_Soul_FiveStar);
                break;
        }
        return txt;
    }
    /// <summary>
    /// 基础属性名称
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public string GetBasePropertyName(EquipDefine.EquipBasePropertyType type)
    {
        string name = "";
        switch(type)
        {
            case EquipDefine.EquipBasePropertyType.PhyAttack:
                name = GetLocalText(LocalTextType.ItemTips_PhyAttack);
                break;
            case EquipDefine.EquipBasePropertyType.PhyDef:
                name = GetLocalText(LocalTextType.ItemTips_PhyDefend);
                break;
            case EquipDefine.EquipBasePropertyType.MagicAttack:
                name = GetLocalText(LocalTextType.ItemTips_MagicAttack);
                break;
            case EquipDefine.EquipBasePropertyType.MagicDef:
                name = GetLocalText(LocalTextType.ItemTips_MagicDefend);
                break;
        }
        return name;
    }

    /// <summary>
    /// 获取职业名称
    /// </summary>
    /// <param name="pf"></param>
    /// <returns></returns>
    public string GetProfessionName(GameCmd.enumProfession pf)
    {
        LocalTextType key = LocalTextType.LocalText_None;
        switch (pf)
        {
            case GameCmd.enumProfession.Profession_Soldier:
                key = LocalTextType.Local_TXT_Profession_Soldier;
                break;
            case GameCmd.enumProfession.Profession_Spy:
                key = LocalTextType.Local_TXT_Profession_Spy;
                break;
            case GameCmd.enumProfession.Profession_Freeman:
                key = LocalTextType.Local_TXT_Profession_Freeman;
                break;
            case GameCmd.enumProfession.Profession_Doctor:
                key = LocalTextType.Local_TXT_Profession_Doctor;
                break;
            case GameCmd.enumProfession.Profession_Gunman:
                key = LocalTextType.Local_TXT_Profession_Gunman;
                break;
            case GameCmd.enumProfession.Profession_Blast:
                key = LocalTextType.Local_TXT_Profession_Blast;
                break;
        }
        if (key == LocalTextType.LocalText_None)
        {
            return "通用";
        }
        return GetLocalText(key);
    }

    /// <summary>
    /// 根据任务类型获取任务标题
    /// </summary>
    /// <param name="tasType"></param>
    /// <returns></returns>
    public string GetTaskTitleByType(GameCmd.TaskType tasType)
    {
        LocalTextType key = LocalTextType.LocalText_None;
        switch (tasType)
        {
            case GameCmd.TaskType.TaskType_Normal:
                key = LocalTextType.Local_TXT_TaskTitle_Main;
                break;
            case GameCmd.TaskType.TaskType_Sub:
                key = LocalTextType.Local_TXT_TaskTitle_Sub;
                break;
            case GameCmd.TaskType.TaskType_Loop:
                key = LocalTextType.Local_TXT_TaskTitle_Daily;
                break;
            case GameCmd.TaskType.TaskType_Clan:
                key = LocalTextType.Local_TXT_Clan;
                break;
            case GameCmd.TaskType.TaskType_Achieve:

                break;
        }
        return GetLocalText(key);
    }

    /// <summary>
    /// 获取宝石类型名称
    /// </summary>
    /// <param name="gType"></param>
    /// <returns></returns>
    public string GetGemNameByType(GameCmd.GemType gType)
    {
        LocalTextType key = LocalTextType.LocalText_None;
        switch (gType)
        {
            case GameCmd.GemType.GemType_MagicAttact:
                key = LocalTextType.Local_TXT_Mattack;
                break;
            case GameCmd.GemType.GemType_MagicDefent:
                key = LocalTextType.Local_TXT_Mdefend;
                break;
            case GameCmd.GemType.GemType_PhysicalAttact:
                key = LocalTextType.Local_TXT_Pattack;
                break;
            case GameCmd.GemType.GemType_PhysicalDefent:
                key = LocalTextType.Local_TXT_Pdefend;
                break;
            case GameCmd.GemType.GemType_UpgradeLife:
                key = LocalTextType.Local_TXT_HpPromote;
                break;
            case GameCmd.GemType.GemType_Exp:
                key = LocalTextType.Local_TXT_ExpAdd;
                break;
        }
        return GetLocalText(key);
    }

    /// <summary>
    /// 获取货币名称
    /// </summary>
    /// <param name="mType"></param>
    /// <returns></returns>
    public string GetCurrencyNameByType(GameCmd.MoneyType mType)
    {
        LocalTextType key = LocalTextType.LocalText_None;
        switch (mType)
        {
            case GameCmd.MoneyType.MoneyType_Coin:
                key = LocalTextType.Local_TXT_YuanBao;
                break;
            case GameCmd.MoneyType.MoneyType_MoneyTicket:
                key = LocalTextType.Local_TXT_Penney;
                break;
            case GameCmd.MoneyType.MoneyType_Gold:
                key = LocalTextType.Local_TXT_Gold;
                break;
            case GameCmd.MoneyType.MoneyType_Reputation:
                key = LocalTextType.Local_TXT_Rep;
                break;
            case GameCmd.MoneyType.MoneyType_Score:
                key = LocalTextType.Local_TXT_JiFen;
                break;
            case GameCmd.MoneyType.MoneyType_AchievePoint:
                key = LocalTextType.Local_TXT_ChengJiuDian;
                break;
            case GameCmd.MoneyType.MoneyType_CampCoin:
                key = LocalTextType.Local_TXT_ZhanXun;
                break;
            case GameCmd.MoneyType.MoneyType_HuntingCoin:
                key = LocalTextType.Local_TXT_LieHun;
                break;
            case GameCmd.MoneyType.MoneyType_FishingMoney:
                key = LocalTextType.Local_TXT_YuBi;
                break;
            case GameCmd.MoneyType.MoneyType_TradeGold:
                key = LocalTextType.Local_TXT_YinLiang;
                break;
        }
        return GetLocalText(key);
    }
    #endregion

    #region SensitiveWords
    /// <summary>
    /// 敏感词对象
    /// </summary>
    public class SensitiveNode
    {
        /// <summary>
        /// 敏感词key
        /// </summary>
        private char key;
        public Char Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
            }
        }

        /// <summary>
        /// 是否结束
        /// </summary>
        private bool isEnd;
        public bool IsEnd
        {
            get
            {
                return isEnd;
            }
            set
            {
                isEnd = value;
            }
        }

        private Dictionary<char, SensitiveNode> childSensitiveNodeDic = new Dictionary<char, SensitiveNode>();
        /// <summary>
        /// 添加敏感词对象
        /// </summary>
        /// <param name="sensitiveNode"></param>
        public void Add(char key, SensitiveNode sensitiveNode)
        {
            if (sensitiveNode == null || childSensitiveNodeDic.ContainsKey(sensitiveNode.Key))
                return;
            childSensitiveNodeDic.Add(sensitiveNode.key, sensitiveNode);
        }

        /// <summary>
        ///是否存在key 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainKey(char key)
        {
            return (childSensitiveNodeDic.ContainsKey(key)) ? true : false;
        }

        /// <summary>
        /// 尝试获取ey 的SensitiveObject
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool TryGetValue(char key, out SensitiveNode node)
        {
            return childSensitiveNodeDic.TryGetValue(key, out node);
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            key = ' ';
            childSensitiveNodeDic.Clear();
            isEnd = false;
        }

    }
    /// <summary>
    /// 敏感词匹配规则
    /// </summary>
    public enum MatchType
    {
        //最小匹配遇到就返回
        Min = 0,
        //最大匹配还需继续查找
        Max = 1,
    }
    private List<string> sensitiveWordList;
    private Dictionary<char, SensitiveNode> sensitiveWordDic;

    /// <summary>
    /// 初始化敏感词
    /// </summary>
    private void InitSensitive()
    {
        if (m_bSensitiveReady)
        {
            return;
        }
        if (null == sensitiveWordList)
        {
            sensitiveWordList = new List<string>();
        }
        sensitiveWordList.Clear();
        if (null == sensitiveWordDic)
        {
            sensitiveWordDic = new Dictionary<char, SensitiveNode>();
        }
        sensitiveWordDic.Clear();
        List<table.SensitiveWordDataBase> sensitiveWords
            = GameTableManager.Instance.GetTableList<table.SensitiveWordDataBase>();
        if (null == sensitiveWords || sensitiveWords.Count == 0)
        {
            Engine.Utility.Log.Error("TextManager ->InitSensitive failed,sensitiveWords null");
        }else
        {
            //读取表格数据
            for (int i = 0; i < sensitiveWords.Count; i++)
            {
                if (!string.IsNullOrEmpty(sensitiveWords[i].sWord))
                {
                    sensitiveWordList.Add(sensitiveWords[i].sWord);
                }
            }
            //添加到敏感词Table
            AddSensitiveWord(sensitiveWordList);    
        }
        m_bSensitiveReady = true;
    }

    private bool m_bSensitiveReady = false;
    public bool SensitiveReady
    {
        get
        {
            return m_bSensitiveReady;
        }
    }



    /// <summary>
    /// 添加敏感词到Hashtable
    /// </summary>
    /// <param name="sensitiveWordList">敏感词列表</param>
    public void AddSensitiveWord(List<string> sensitiveWordList)
    {
        if (sensitiveWordList == null || sensitiveWordList.Count == 0)
            return;
        sensitiveWordDic.Clear();
        //当前节点
        SensitiveNode curSensitiveNode = null;
        //零时节点
        SensitiveNode tempSensitiveNode = null;
        string sensitiveWord = "";
        char charKey ;
        char sensitiveChar;
        for(int i= 0;i < sensitiveWordList.Count;i++)
        {
            sensitiveWord = sensitiveWordList[i];
            if (string.IsNullOrEmpty(sensitiveWord))
                continue;

            for(int j = 0;j < sensitiveWord.Length;j++)
            {
                charKey = sensitiveWord.ElementAt(j);
                if (char.IsWhiteSpace(charKey))
                {
                    continue;
                }
                //如果当前 查找的字符为字母，则对大小写进行合并
                if (TryGetSensitiveChar(charKey, out sensitiveChar))
                {
                    charKey = sensitiveChar;
                }

                if (j == 0)
                {
                    if (!sensitiveWordDic.TryGetValue(charKey,out curSensitiveNode))
                    {
                        curSensitiveNode = new SensitiveNode();
                        curSensitiveNode.Key = charKey;
                        sensitiveWordDic.Add(charKey, curSensitiveNode);
                    }
                }
                else 
                {
                    if (!curSensitiveNode.TryGetValue(charKey, out tempSensitiveNode))
                    {
                        tempSensitiveNode = new SensitiveNode();
                        tempSensitiveNode.Key = charKey;
                        curSensitiveNode.Add(charKey, tempSensitiveNode);
                        curSensitiveNode = tempSensitiveNode;
                    }else
                    {
                        curSensitiveNode = tempSensitiveNode;
                    }
                    
                }
                
                if (j == sensitiveWord.Length - 1)
                    curSensitiveNode.IsEnd = true;
                    
            }
        }
    }

    /// <summary>
    /// 是否含有敏感词
    /// </summary>
    /// <param name="txt"></param>
    /// <param name="matchType"></param>
    /// <returns></returns>
    public  bool IsContainSensitiveWord(string txt,MatchType matchType)
    {
        bool flag = false;
        if (string.IsNullOrEmpty(txt))
            return false;
        for (int i = 0; i < txt.Length;i++ )
        {
            int length = CheckSensitiveWord(txt, i, matchType);
            if (length > 0)
            {
                flag = true;
                break;
            }
        }
        return flag;
    }

    /// <summary>
    /// 获取敏感词
    /// </summary>
    /// <param name="txt"></param>
    /// <param name="matchType"></param>
    /// <returns></returns>
    public List<string> GetSensitiveWorld(string txt, MatchType matchType)
    {
        if (string.IsNullOrEmpty(txt))
            return null;
        List<string> sensitiveWordList = new List<string>();
        for (int i = 0; i < txt.Length;i++ )
        {
            int length = CheckSensitiveWord(txt, i, matchType);
            if (length >0)
            {
                sensitiveWordList.Add(txt.Substring(i, length));
                i = i + length - 1;
            }
        }
        return sensitiveWordList; 
    }

    /// <summary>
    /// 替换敏感词为*(默认)
    /// </summary>
    /// <param name="txt">需要替换的文本</param>
    /// <param name="matchType">匹配类型</param>
    /// <param name="replaceChar">替换字符</param>
    /// <returns></returns>
    public string ReplaceSensitiveWord(string txt, MatchType matchType, string replaceChar = "*")
    {
        string resultText = txt;
        List<string> sensitiveList = GetSensitiveWorld(txt, matchType);
        string replaceString = "";
        string word = "";
        if (sensitiveList != null)
        {
            for (int i = 0;i < sensitiveList.Count;i++)
            {
                word = sensitiveList[i];
                replaceString = GetReplaceChars(replaceChar, word.Length);
                resultText = resultText.Replace(word, replaceString);
            }
        }
        return resultText;
    }

    /// <summary>
    /// 获取代替字符串
    /// </summary>
    /// <param name="replaceChar"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public string GetReplaceChars(string replaceChar,int length)
    {
        string replace = replaceChar;
        for (int i = 1;i < length;i ++)
        {
            replace += replaceChar;
        }
        return replace;
    }

    /// <summary>
    /// 检测是否含有敏感词
    /// </summary>
    /// <param name="txt"></param>
    /// <param name="beginIndex"></param>
    /// <param name="matchType"></param>
    /// <returns></returns>
    public int CheckSensitiveWord(string txt, int beginIndex, MatchType matchType)
    {
        bool flag = false;
        if (string.IsNullOrEmpty(txt))
            return 0;
        if (!SensitiveReady)
        {
            InitSensitive();
        }
        char charKey;
        char sensitiveChar;
        int matchFlag = 0;
        SensitiveNode node = null;
        SensitiveNode temp = null;

        for (int i = beginIndex; i < txt.Length;i++ )
        {
            charKey = txt.ElementAt(i);
            if (char.IsWhiteSpace(charKey))
            {
                matchFlag++;
                continue;
            }
            if (TryGetSensitiveChar(charKey,out sensitiveChar))
            {
                charKey = sensitiveChar;
            }
            if (i == beginIndex)
            {
                if (!sensitiveWordDic.TryGetValue(charKey, out node))
                {
                    break;
                }
            }
            else if (null != node && node.TryGetValue(charKey, out temp))
            {
                node = temp;
            }
            else
            {
                break;
            }

            matchFlag++;
            if (node.IsEnd)
            {
                flag = true;
                if (matchType == MatchType.Min)
                    break;
            }
            
        }

        if (!flag)
            matchFlag = 0;
        return matchFlag;
    }

    /// <summary>
    /// 获取敏感词key
    /// </summary>
    /// <param name="charKey"></param>
    /// <returns></returns>
    public bool TryGetSensitiveChar(char charKey,out char sensitiveChar)
    {
        bool success = false;
        sensitiveChar = ' ';
        if (char.IsLetter(charKey) && char.IsUpper(charKey))
        {
            sensitiveChar = char.ToLower(charKey);
            success = true;
        }
        return success;
    }

    #endregion

    #region IllegalChar

    /// <summary>
    /// 判断是否是非法字符
    /// </summary>
    /// <param name="str">判断是字符</param>
    /// <returns></returns>
    public static Boolean IsLegalText(string str)
    {
        return true;
        char[] charStr = str.ToLower().ToCharArray();
        for (int i = 0; i < charStr.Length; i++)
        {
            int num = Convert.ToInt32(charStr[i]);
            if (!(IsChineseLetter(num) || (num >= 48 && num <= 57) || (num >= 97 && num <= 123) || (num >= 65 && num <= 90) || num == 45))
            {
                return false;
            }
        }
        return true;
    }


    /// <summary>
    /// 判断字符的Unicode值是否是汉字
    /// </summary>
    /// <param name="code">字符的Unicode</param>
    /// <returns></returns>
    public static bool IsChineseLetter(int code)
    {
        int chfrom = Convert.ToInt32("4e00", 16);    //范围（0x4e00～0x9fff）转换成int（chfrom～chend）
        int chend = Convert.ToInt32("9fff", 16);

        if (code >= chfrom && code <= chend)
        {
            return true;     //当code在中文范围内返回true

        }
        else
        {
            return false;    //当code不在中文范围内返回false
        }

        return false;
    }
    #endregion

    #region text bytes calculate
    /// <summary>
    /// 根据文字数量限制获取最终字符
    /// </summary>
    /// <param name="srcStr">字符串</param>
    /// <param name="limitWordsNum">字数限制</param>
    /// <param name="overRemoveEnd">true:如果超过字数限制从尾部开始移除 false:反之</param>
    /// <returns></returns>
    public static string GetTextByWordsCountLimitInUnicode(string str, uint limitWordsNum,bool overRemoveEnd = true)
    {
        if (string.IsNullOrEmpty(str) || limitWordsNum<=0)
        {
            return "";
        }
        uint curSize = GetCharNumByStrInUnicode(str);
        uint maxSize = TransformWordNum2CharNum(limitWordsNum);
        string result = str;
        while (curSize > maxSize)
        {
            if (result.Length > 1)
            {
                result = (overRemoveEnd) ? result.Substring(0, result.Length - 1) : result.Substring(1, result.Length - 1);
            }
            else
            {
                result = "";
                break;
            }
            curSize = GetCharNumByStrInUnicode(result);
        }
        return result;
    }

    /// <summary>
    /// Unicode编码下字符串str对应的英文字符长度
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static uint GetCharNumByStrInUnicode(string str)
    {
        uint charNum = 0;
        if (!string.IsNullOrEmpty(str))
        {
            char[] charstr = str.ToCharArray();
            int codeValue = 0;
            for (int i = 0; i < charstr.Length; i++)
            {
                codeValue = Convert.ToInt32(charstr[i]);
                charNum++;
                if (codeValue > 255)
                {
                    charNum++;
                }
            }
        }
        return charNum;
    }

    /// <summary>
    /// 根据字数获取对应英文字符长度
    /// </summary>
    /// <param name="wordCount"></param>
    /// <returns></returns>
    public static uint TransformWordNum2CharNum(uint wordCount)
    {
        return (wordCount >0) ? wordCount * 2: 0;
    }

    /// <summary>
    /// 变换英文字符长度到字数
    /// </summary>
    /// <param name="charNum"></param>
    /// <param name="ceilToInt">是否为向上取整</param>
    /// <returns></returns>
    public static uint TransforCharNum2WordNum(uint charNum, bool ceilToInt = false)
    {
        return (uint)((charNum > 0) ? ((ceilToInt ? UnityEngine.Mathf.CeilToInt(charNum / 2f)
            : UnityEngine.Mathf.FloorToInt(charNum / 2f))) : 0);
    }
    #endregion

    #region NumFormatText
    /// <summary>
    /// 获取数字格式化字符串
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string GetFormatNumText(uint num)
    {
        string resultStr = num.ToString();
        
        if (num >= 100000)
        {
            /*float divide = num / 10000f;
            string divideStr = divide.ToString();
            if (divideStr.IndexOf('.') >= 0)
            {
                string decimaltStr = divideStr.Substring(divideStr.IndexOf('.') + 1, 1);
                int decimalInt = int.Parse(decimaltStr);
                if (decimalInt > 0)
                {
                    resultStr = string.Format("{0}.{1}万", (uint)divide, decimalInt);
                }else{
                    resultStr = string.Format("{0}万", (uint)divide);
                }
            }else
            {
                resultStr = string.Format("{0}万", (uint)divide);
            }*/

            uint divide = num / 10000;
            resultStr = string.Format("{0}万", divide);
        }
        else if(num == 0)
        {
            resultStr = "0";
        }

        return resultStr;
    }
    #endregion
}