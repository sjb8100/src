using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;

namespace EntitySystem
{
    /// <summary>
    /// 实体工厂 管理实体的生命周期
    /// </summary>
    class EntityFactory
    {
        static EntityFactory s_Inst = null;
        public static EntityFactory Instance()
        {
            if (null == s_Inst)
            {
                s_Inst = new EntityFactory();
            }

            return s_Inst;
        }

        public IEntity CreateEntity(EntityType etype, EntityCreateData data)
        {
            if (!CheckEntityType(etype))
            {
                return null;
            }

            // 设置属性
            IEntity en = CreateEntityObj(etype, data);
            if (en != null)
            {
                en.UpdateProp(data); // 设置属性
            }

            return en;
        }

        public void RemoveEntity(IEntity en)
        {
            if(en!=null)
            {
                //先发送事件在删除。因为触发事件中有可能会获取该实体
                Client.stRemoveEntity removeEntiy = new Client.stRemoveEntity();
                removeEntiy.uid = en.GetUID();
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_REMOVEENTITY, removeEntiy);

                EntityManager.Instance().RemoveEntity(en);
            }
        }

        public void RemoveEntity(long uid)
        {
            IEntity en = EntityManager.Instance().GetEntity(uid);
            RemoveEntity(en);
        }

        public IEntity GetEntity(long uid)
        {
            return EntityManager.Instance().GetEntity(uid);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 检查实体类型的合法性
        /// </summary>
        /// <param name="etype"></param>
        /// <returns></returns>
        private bool CheckEntityType(EntityType etype)
        {
            if( etype < EntityType.EntityType_Null || etype >= EntityType.EntityType_Max )
            {
                return false;
            }

            return true;
        }

        //-------------------------------------------------------------------------------------------------------
        public long MakeUID(EntityType etype,uint id)
        {
            long UID = 0;

            UID |= (((long)etype) << 32);
            UID |= (long)id;
            return UID;
        }

        private IEntity CreateEntityObj(EntityType etype, EntityCreateData data)
        {
            IEntity en = null;
            switch(etype)
            {
                case EntityType.EntityType_Player:
                    {
                        Player player = new Player();
                        if(player!=null)
                        {
                            player.SetUID(MakeUID(etype, data.ID));
                            EntityManager.Instance().AddEntity(player);
                            if (data.bMainPlayer)
                            {
                                EntitySystem.m_ClientGlobal.MainPlayer = player;
                            }

                            player.Create(data, ColliderCheckType.ColliderCheckType_CloseTerrain | ColliderCheckType.ColliderCheckType_Wall);
                        }
                        en = player;
                        break;
                    }
                case EntityType.EntityType_Monster:
                    {
                        Monster monster = new Monster();
                        if (monster != null)
                        {
                            monster.SetUID(MakeUID(etype, data.ID));
                            EntityManager.Instance().AddEntity(monster);
                            monster.Create(data, ColliderCheckType.ColliderCheckType_CloseTerrain | ColliderCheckType.ColliderCheckType_Wall);
                        }
                        
                        en = monster;
                        break;
                    }
                case EntityType.EntityType_NPC:
                    {
                        NPC npc = new NPC();
                        if (npc != null)
                        {
                            npc.SetUID(MakeUID(etype, data.ID));
                            EntityManager.Instance().AddEntity(npc);
                            npc.Create(data, ColliderCheckType.ColliderCheckType_CloseTerrain | ColliderCheckType.ColliderCheckType_Wall);
                        }
                        
                        en = npc;
                        break;
                    }
                case EntityType.EntityTYpe_Robot:
                    {
                        Robot robot = new Robot();
                        if (robot != null)
                        {
                            robot.SetUID(MakeUID(etype, data.ID));
                            EntityManager.Instance().AddEntity(robot);
                            robot.Create(data, ColliderCheckType.ColliderCheckType_CloseTerrain | ColliderCheckType.ColliderCheckType_Wall);
                        }

                        en = robot;
                        break;
                    }
                case EntityType.EntityType_Item: // 物品
                    {
                        Item item = new Item();
                        if (item != null)
                        {
                            item.SetUID(MakeUID(etype, data.ID));
                            EntityManager.Instance().AddEntity(item);
                            item.Create(data, ColliderCheckType.ColliderCheckType_null);
                        }
                        
                        en = item;
                        break;
                    }
                case EntityType.EntityType_Pet: // 宠物
                    {
                        Pet pet = new Pet();
                        if(pet != null)
                        {
                            pet.SetUID(MakeUID(etype, data.ID));
                            EntityManager.Instance().AddEntity(pet);
                            pet.Create( data , ColliderCheckType.ColliderCheckType_CloseTerrain | ColliderCheckType.ColliderCheckType_Wall );
                        }
                        
                        en = pet;
                        break;
                    }
                /////////////////////////////////////////////////////////////////////////////////////////////////////////
                // 家园对象
                case EntityType.EntityType_Plant: // 植物
                    {
                        Plant plant = new Plant();
                        if (plant != null)
                        {
                            plant.SetUID(MakeUID(etype, data.ID));
                            EntityManager.Instance().AddEntity(plant);
                            plant.Create(data, ColliderCheckType.ColliderCheckType_CloseTerrain | ColliderCheckType.ColliderCheckType_Wall);
                        }
                        
                        en = plant;
                        break;
                    }
                case EntityType.EntityType_Animal: // 动物
                    {
                        Animal animal = new Animal();
                        if (animal != null)
                        {
                            animal.SetUID(MakeUID(etype, data.ID));
                            EntityManager.Instance().AddEntity(animal);
                            animal.Create(data, ColliderCheckType.ColliderCheckType_CloseTerrain | ColliderCheckType.ColliderCheckType_Wall);
                        }
                        
                        en = animal;
                        break;
                    }
                case EntityType.EntityType_Tree: // 许愿树
                    {
                        Tree tree = new Tree();
                        if (tree != null)
                        {
                            tree.SetUID(MakeUID(etype, data.ID));
                            EntityManager.Instance().AddEntity(tree);
                            tree.Create(data, ColliderCheckType.ColliderCheckType_CloseTerrain | ColliderCheckType.ColliderCheckType_Wall);
                        }
                        
                        en = tree;
                        break;
                    }
                case EntityType.EntityType_Minerals: // 矿产
                    {
                        Minerals mine = new Minerals();
                        if (mine != null)
                        {
                            mine.SetUID(MakeUID(etype, data.ID));
                            EntityManager.Instance().AddEntity(mine);
                            mine.Create(data, ColliderCheckType.ColliderCheckType_CloseTerrain | ColliderCheckType.ColliderCheckType_Wall);
                        }
                        
                        en = mine;
                        break;
                    }
                case EntityType.EntityType_Soil: // 土地
                    {
                        Soil soil = new Soil();
                        if (soil != null)
                        {
                            soil.SetUID(MakeUID(etype, data.ID));
                            EntityManager.Instance().AddEntity(soil);
                            soil.Create(data, ColliderCheckType.ColliderCheckType_CloseTerrain | ColliderCheckType.ColliderCheckType_Wall);
                        }
                        
                        en = soil;
                        break;
                    }
                case EntityType.EntityType_Puppet: // 木偶
                    {
                        Puppet puppet = new Puppet();
                        if (puppet != null)
                        {
                            puppet.SetUID(MakeUID(etype, data.ID));
                            EntityManager.Instance().AddEntity(puppet);
                            puppet.Create(data, ColliderCheckType.ColliderCheckType_CloseTerrain | ColliderCheckType.ColliderCheckType_Wall);
                        }

                        en = puppet;
                        break;
                    }
                case EntityType.EntityType_Box:
                    {
                        //UnityEngine.Sprite sp;


                        Box box = new Box();
                        if (box != null)
                        {
                            box.SetUID(MakeUID(etype, data.ID));
                            EntityManager.Instance().AddEntity(box);
                            box.Create(data, ColliderCheckType.ColliderCheckType_CloseTerrain | ColliderCheckType.ColliderCheckType_Wall);
                        }

                        en = box;
                        break;
                    }
            }

            //if(en!=null)
            //{
            //    Client.stCreateEntity createEntity = new Client.stCreateEntity();
            //    createEntity.uid = en.GetUID();
            //    Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_CREATEENTITY, createEntity);
            //}

            return en;
        }
    }
}
