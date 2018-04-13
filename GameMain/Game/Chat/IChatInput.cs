using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

interface IChatInput
{
    void AppendText(string emoji);

    //1 物品 2 坐骑 3 宠物
    void AddLinkerItem(string itemName, uint thisID, uint quality,int type);

    void ResetPos();
}
