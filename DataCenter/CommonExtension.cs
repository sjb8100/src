using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GameCmd;

namespace GameCmd
{

    public partial class PairNumber
    {
        public static readonly PairNumber Empty = new PairNumber();
    }
    //public partial class stMapDataMapScreenUserCmd_S
    //{
    //    public SceneEntryType Type
    //    {
    //        get
    //        {
    //            if (this.itemlist.Any())
    //                return SceneEntryType.SceneEntry_Object;
     //           else if (this.npclist.Any())
    //                return SceneEntryType.SceneEntry_NPC;
    //            else if (this.userlist.Any())
    //                return SceneEntryType.SceneEntry_Player;
    //            else
    //                throw new NotImplementedException();
    //        }
    //    }
    //}

    /// <summary>
    /// 任务
    /// </summary>
    public partial class SerializeTask
    {
        public TaskColor Color
        {
            get { return (TaskColor)color; }
            set { color = (byte)value; }
        }

        /// <summary>
        /// 获得任务的状态
        /// </summary>
        /// <returns></returns>
        public TaskProcess getTaskProcess()
        {
            if (this.state == 0)
            {
                return TaskProcess.TaskProcess_Done;
            }
            else if (this.state <= this.operate)
            {
                return TaskProcess.TaskProcess_CanDone;
            }
            else if (state > operate)
            {
                return TaskProcess.TaskProcess_Doing;
            }
            else
            {
                return TaskProcess.TaskProcess_None;
            }

        }
    }


}