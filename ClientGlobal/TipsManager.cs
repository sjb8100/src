using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Client
{
    class TipsManager : ITipsManager
    {
        // strTips支持html文本
        public void AddTips(string strTips, Vector3 pos, TipType type)
        {
            // 具体后面再补充实现 // TODO:
            Engine.Utility.Log.Error(strTips);
        }
    }
}
