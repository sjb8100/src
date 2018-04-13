
//*************************************************************************
//	创建日期:	2017/5/12 星期五 13:58:02
//	文件名称:	ChangeLine_Protocol
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using Engine.Utility;
using Engine;
using Client;
using Common;
using UnityEngine;
using GameCmd;
using table;


partial class Protocol
{
    
    [Execute]
    public void OnReceiveChangeLineInfo(stResponLineInfoMapScreenUserCmd_S info)
    {
        DataManager.Manager<MapDataManager>().OnReceiveChangeLineInfo(info);
    }


}
