using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Common;


//*******************************************************************************************
//	创建日期：	2016-10-31   11:41
//	文件名称：	Title_Protocol,cs
//	创 建 人：	Lin Chuang Yuan
//	版权所有：	Lin Chuang Yuan
//	说    明：	称号系统
//*******************************************************************************************
partial class Protocol
{
    /// <summary>
    /// 称号list
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnAddTitleList(stGetAllTitlePropertyUserCmd_S cmd)
    {
        DataManager.Manager<TitleManager>().OnAddAllTitleList(cmd);
    }

    /// <summary>
    /// 获取新的称号
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnAddTitleList(stAddTitlePropertyUserCmd_S cmd)
    {
        DataManager.Manager<TitleManager>().OnAddNewTitle(cmd);
        
    }

    /// <summary>
    /// 佩戴称号
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnSelectTitle(stSelectTitlePropertyUserCmd_CS cmd)
    {
        DataManager.Manager<TitleManager>().OnSelectTitle(cmd);
    }


    /// <summary>
    /// 激活称号
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnActivateTitle(stActivateTitlePropertyUserCmd_CS cmd)
    {
        DataManager.Manager<TitleManager>().OnActivateTitle(cmd);

    }

    /// <summary>
    /// 该称号剩余使用次数
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnGetTitleUseTimes(stSetCountTitlePropertyUserCmd_S cmd)
    {
         DataManager.Manager<TitleManager>().OnGetTitleUseTimes(cmd);
    }

    /// <summary>
    /// 删除称号
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnDeleteTitle(stDeleteTitlePropertyUserCmd_CS cmd)
    {
        DataManager.Manager<TitleManager>().OnDeleteTitle(cmd);
    }
}

