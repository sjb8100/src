using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// UI工具类
public class UIUtilts
{
    public static void SetUISpriteGray(GameObject root, bool gray)
    {
        if(root==null)
        {
            return;
        }

        UISprite[] sprites = root.GetComponentsInChildren<UISprite>();
        if(sprites==null)
        {
            return;
        }

        for(int i = 0; i < sprites.Length; ++i)
        {
            if(sprites[i]==null)
            {
                continue;
            }

            sprites[i].color = new Color(sprites[i].color.r, sprites[i].color.g, sprites[i].color.b, gray ? 1 : 255);
        }
    }
}
