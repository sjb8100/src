/************************************************************************************
 * Copyright (c) 2018  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Login.AreaServer
 * 创建人：  wenjunhua.zqgame
 * 文件名：  SeverSection
 * 版本号：  V1.0.0.0
 * 创建时间：1/9/2018 10:11:28 AM
 * 描述：服务器区间
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SeverSection
{
    #region property
    //区间最小值
    private uint m_uMin = 0;
    //区间最大值
    private uint m_uMax = 0;
    #endregion

    #region structMethod
    public SeverSection (string sectionStr)
    {
        if (string.IsNullOrEmpty(sectionStr))
        {
            Engine.Utility.Log.Error("SeverSection->Struct Data error,section str empty!");
            return;
        }
        string trimStr = sectionStr.Trim();
        if (trimStr.Length >2)
        {
            string dataStr = trimStr.Substring(1, trimStr.Length - 2);
            if (!string.IsNullOrEmpty(dataStr))
            {
                string []dataArray = dataStr.Split(new char[]{','});
                if (null != dataArray && dataArray.Length == 2)
                {
                    string tempStr = "";
                    uint tempParse = 0;
                    if (!string.IsNullOrEmpty(dataArray[0]))
                    {
                        tempStr = dataArray[0].Trim();
                        if (uint.TryParse(tempStr, out tempParse))
                        {
                            m_uMin = tempParse;
                        }
                    }

                    if (!string.IsNullOrEmpty(dataArray[1]))
                    {
                        tempStr = dataArray[1].Trim();
                        if (uint.TryParse(tempStr, out tempParse))
                        {
                            m_uMax = tempParse;
                        }
                    }

                }else
                {
                    Engine.Utility.Log.Error("SeverSection->Struct Data error,parse Section str:{0} error!",dataStr);
                }
            }
        }
    }
    #endregion

    #region public method 

    /// <summary>
    /// 服务器ID是否在该区间内
    /// </summary>
    /// <param name="serverID"></param>
    /// <returns></returns>
    public bool IsContainID(uint serverID)
    {
        return serverID >= m_uMin && serverID <= m_uMax;
    }

    #endregion

    #region private method
    #endregion

}