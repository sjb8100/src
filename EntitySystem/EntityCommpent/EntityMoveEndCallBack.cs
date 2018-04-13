using System;
using System.Collections.Generic;
using Client;

namespace EntitySystem
{
    public class EntityMoveEndCallBack : IEntityCallback
    {
        public MoveEndCallback callback = null;

        public delegate void MoveEndCallback(object param = null);

        public void OnMoveEnd(IEntity entity, object param = null)
        {
            if (callback != null)
            {
                callback(param);
            }
        }

        // 落地
        public void OnToGround(IEntity entity, object param = null)
        {

        }

        // 飞行结束
        public void OnFlyEnd(IEntity entity, object param = null)
        {

        }

        public void OnUpdate(IEntity entity, object param = null)
        {

        }

        /// <summary>
        /// 发生碰撞
        /// </summary>
        /// <param name="obj">检测对象</param>
        /// <param name="obj">碰撞信息</param>
        public void OnCollider(IEntity obj, ref EntityColliderInfo entityCollierInfo)
        {

        }
    }
}
