
//*************************************************************************
//	创建日期:	2018/3/13 星期二 10:31:53
//	文件名称:	TraceSeverPos
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	显示服务器点 只在编辑器下生效
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace MapSystem
{
    class ShowTraceSeverPos : SingletonMono<ShowTraceSeverPos>
    {
        List<GameObject> m_usecubeList = new List<GameObject>();
        List<GameObject> m_idelcubeList = new List<GameObject>();
        public void RecieveData(bool add,int x,int y)
        {
            string goName = x + "_" + y;
            GameObject go = null;
            if (add)
            {
                bool bContain = false;
                foreach (var item in m_usecubeList)
                {
                    if (item.name == goName)
                    {
                        bContain = true;
                        break;
                    }
                }
                if(bContain)
                {
                    return;
                }
                if (m_idelcubeList.Count > 0)
                {
                    go = m_idelcubeList[0];
                    m_idelcubeList.RemoveAt(0);
                }
                else
                {
                    go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                }
                go.name =goName;
                go.transform.parent = this.transform;
                go.SetActive(true);
                float height = 3;
                if(MapSystem.m_ClientGlobal.MainPlayer != null)
                {
                    Vector3 vec = MapSystem.m_ClientGlobal.MainPlayer.GetPos();
                    height = vec.y;
                }
               
                go.transform.localPosition = new Vector3(x, height, -y);
                m_usecubeList.Add(go);
            }
            else
            {
                GameObject rmgo = null;
                foreach(var item in m_usecubeList)
                {
                    if(item.name == goName)
                    {
                        rmgo = item;
                        break;
                    }
                }
                if(rmgo != null)
                {
                    m_usecubeList.Remove(rmgo);
                    m_idelcubeList.Add(rmgo);
                    rmgo.SetActive(false);
                }
            }
        }
    }
}