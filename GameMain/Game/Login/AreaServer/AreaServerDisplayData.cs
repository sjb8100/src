/************************************************************************************
 * Copyright (c) 2018  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Login.AreaServer
 * 创建人：  wenjunhua.zqgame
 * 文件名：  AreaServerDisplayData
 * 版本号：  V1.0.0.0
 * 创建时间：1/9/2018 10:42:28 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class AreaServerDisplayData
{
    #region property
    //ID
    private uint m_uid;
    public uint ID
    {
        get
        {
            return m_uid;
        }
    }

    //母包ID
    private uint m_uMainpackageID = 0;
    public uint MainPackageID
    {
        get
        {
            return m_uMainpackageID;
        }
    }

    //渠道号
    private uint m_uChannelId = 0;
    public uint ChannelId
    {
        get
        {
            return m_uChannelId;
        }
    }

    //区服
    private List<uint> m_lst_areaServers = null;
    public List<uint> AreaServers
    {
        get
        {
            return m_lst_areaServers;
        }
    }
    #endregion

    #region structmethod
    public AreaServerDisplayData (uint id,uint mainPackageId,uint channelId,string displayServer)
    {
        m_uid = id;
        m_uChannelId = channelId;
        m_uMainpackageID = mainPackageId;

        if (!TryParseAreaServerSecions(displayServer, ref m_lst_areaServers))
        {
            Engine.Utility.Log.Error("AreaServerDisplayData->parse data error,db null");
        }
    }
    #endregion

    #region public method
    public static bool TryParseAreaServerSecions(string areaServerDataStr,ref List<uint> result)
    {
        bool success = false;
        if (null != result)
            result.Clear();
        if (!string.IsNullOrEmpty(areaServerDataStr))
        {
            string trimStr = areaServerDataStr.Trim();
            if (trimStr.Length > 2)
            {
                string dataStr = trimStr.Substring(1, trimStr.Length - 2);
                if (!string.IsNullOrEmpty(dataStr))
                {
                    string[] dataArray = dataStr.Split(new char[] { ',' });
                    if (null != dataArray && dataArray.Length > 0)
                    {
                       
                        string tempStr = "";
                        uint tempParse = 0;
                        for (int i = 0, max = dataArray.Length; i < max; i++)
                        {
                            tempStr = dataArray[i];
                            if (string.IsNullOrEmpty(tempStr))
                            {
                                continue;
                            }
                            if (uint.TryParse(tempStr.Trim(), out tempParse))
                            {
                                if (null == result)
                                {
                                    result = new List<uint>();
                                }
                                if (!result.Contains(tempParse))
                                    result.Add(tempParse);
                                success = true;
                            }
                        }
                    }
                }
            }
        }
        return success;
    }
    #endregion

    #region private method
    #endregion
}