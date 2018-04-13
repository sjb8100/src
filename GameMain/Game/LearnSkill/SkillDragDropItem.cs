using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Utility;
using UnityEngine;
using table;

public enum SkillSettingAction
{
    Add,//添加
    Remove,//卸下
    Replace,//替换
    Swap,//交换
    Move,//移动
}
/*
class SkillDragDropItem:UIDragDropItem
{
    LearnSkillDataManager skillManager = null;
    void Start()
    {
        skillManager = DataManager.Manager<LearnSkillDataManager>();

    }
    protected override void OnClone(GameObject original)
    {
        base.OnClone( original );
        LearnSkillItem copyItem = GetComponent<LearnSkillItem>();
        LearnSkillItem originItem = original.GetComponent<LearnSkillItem>();
        copyItem.InitItem( originItem.ItemDataBase );
    }
    protected override void OnDragStart()
    {
        int level = skillManager.GetPlayerLevel();
        LearnSkillItem copyItem = GetComponent<LearnSkillItem>();
        if(copyItem.ItemDataBase.dwSkillSatus != 0)
        {
            if ( skillManager.ShowState == SkillSettingState.StateOne )
            {
                if ( copyItem.ItemDataBase.dwSkillSatus != skillManager.GetPlayerJob() )
                {
                    Log.Error( "状态不匹配" );
                    return;
                }
            }
            if ( skillManager.ShowState == SkillSettingState.StateTwo )
            {
                if ( copyItem.ItemDataBase.dwSkillSatus != skillManager.GetPlayerJob() + 10 )
                {
                    Log.Error( "状态不匹配" );
                    return;
                }
            }
         
        }
        int index = 0;
        if ( gameObject.name == "select" )
        {
            if ( skillManager.IsSkillSave( copyItem.ItemDataBase.wdID , ref index ) )
            {
                Log.Error( "已经设置过了" );
                return;
            }
        }
        SkillDatabase sdb = GameTableManager.Instance.GetTableItem<SkillDatabase>( copyItem.ItemDataBase.wdID , 1 );
        if ( level < sdb.dwNeedLevel )
        {
            Log.Error( "未解锁，不能拖动" );
        }
        else
        {
            base.OnDragStart();
        }
      
    }
    protected override void OnDragDropEnd()
    {
        base.OnDragDropEnd();
    }

    protected override void OnDragDropRelease(UnityEngine.GameObject surface)
    {

        string name = surface.name;
        bool bHasSkill = false;//要设置的位置已经有技能了
        if ( name == "select(Clone)" )
        {
            bHasSkill = true;
            //替换操作
            name = surface.transform.parent.name;
        }
        int location = skillManager.GetLocation( name );
        LearnSkillItem skillItem = GetComponent<LearnSkillItem>();
        if ( skillItem != null )
        {
            if ( location == -1 )
            {//消除操作
                int index = 0;
                if ( skillManager.IsSkillSave( skillItem.ItemDataBase.wdID , ref index ) )
                {
                    Log.Error( "去除设置" );
                    skillManager.SendSetSkillMessage( 0 , 0 , SkillSettingAction.Remove , (uint)index , skillItem.ItemDataBase.wdID );
                    return;
                }
                base.OnDragDropRelease( surface );
                NGUITools.Destroy( gameObject );

            }
            else
            {

                base.OnDragDropRelease( surface );
        
                int desLocation = 0;
                LearnSkillItem desSkillItem = surface.GetComponent<LearnSkillItem>();
                if ( desSkillItem != null )
                {
                    if ( skillManager.IsSkillSave( desSkillItem.ItemDataBase.wdID , ref desLocation ) )
                    {
                        if ( bHasSkill )
                        {
                            int index = 0;
                            if ( skillManager.IsSkillSave( skillItem.ItemDataBase.wdID , ref index ) )
                            {
                                NGUITools.Destroy( gameObject );
                                //交换
                                skillManager.SendSetSkillMessage( desLocation , desSkillItem.ItemDataBase.wdID , SkillSettingAction.Swap , (uint)index , skillItem.ItemDataBase.wdID );

                                return;
                            }
                          
                        }
                    }

                }
                int srcLoc = 0;
                if ( skillManager.IsSkillSave( skillItem.ItemDataBase.wdID , ref srcLoc ) )
                {
                    NGUITools.Destroy( gameObject );
                    if(bHasSkill)
                    {
                        Log.Error( "此技能已经设置过了 ，不能重复设置" );

                        skillManager.SetAllSettingItem();
                        return;
                    }
                    else
                    {
                        skillManager.SendSetSkillMessage( location , skillItem.ItemDataBase.wdID , SkillSettingAction.Move , (uint)srcLoc , skillItem.ItemDataBase.wdID );
                        return;
                    }
                }

                skillManager.SendSetSkillMessage( location , skillItem.ItemDataBase.wdID ,SkillSettingAction.Add);
            }

        }
    }
}

*/