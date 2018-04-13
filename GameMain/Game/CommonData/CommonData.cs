using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using GameCmd;
using table;
using Engine;
using Engine.Utility;
using System.Collections;


class CommonData
{

    /// <summary>
    /// 获取本地文字 先写好接口
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    static public string GetLocalString(string str)
    {
        return str;
    }
    static public string GetLocalString(LocalTextType type)
    {
        return DataManager.Manager<TextManager>().GetLocalText(type);
    }
    static public void SafeReleaseDic(IDictionary dic)
    {
        if(dic != null)
        {
            dic.Clear();
            dic = null;
        }
    }
    static public void SafeReleaseList(IList list)
    {
        if (list != null)
        {
            list.Clear();
            list = null;
        }
    }
    static public void SafeClearDic(IDictionary dic)
    {
        if (dic != null)
        {
            dic.Clear();
        }
    }
    static public void SafeClearList(IList list)
    {
        if (list != null)
        {
            list.Clear();
        }
    }
    
}

