using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class CameraBloomEffect : Engine.ICameraBloom
{

    private List<MonoBehaviour> m_lstEffects = new List<MonoBehaviour>();

    public void OnInitCameraBloom(Camera cam, Engine.JsonObject bloom)
    {
        if(bloom==null)
        {
            return;
        }

        string strBloomName = bloom["name"];
        if (!string.IsNullOrEmpty(strBloomName))
        {
            UnityStandardAssets.ImageEffects.BloomAndFlares bloomEffect = null;
            if (strBloomName == "BloomAndFlares")
            {
                //避免重复添加
                bloomEffect = cam.gameObject.GetComponent<UnityStandardAssets.ImageEffects.BloomAndFlares>();
                if (bloomEffect == null)
                {
                    bloomEffect = cam.gameObject.AddComponent<UnityStandardAssets.ImageEffects.BloomAndFlares>();
                }

                if (bloomEffect != null)
                {
                    bloomEffect.lensFlareShader = Shader.Find("Hidden/LensFlareCreate");
                    bloomEffect.vignetteShader = Shader.Find("Hidden/Vignetting");
                    bloomEffect.separableBlurShader = Shader.Find("Hidden/SeparableBlurPlus");
                    bloomEffect.addBrightStuffOneOneShader = Shader.Find("Hidden/BlendOneOne");
                    bloomEffect.screenBlendShader = Shader.Find("Hidden/Blend");
                    bloomEffect.hollywoodFlaresShader = Shader.Find("Hidden/MultipassHollywoodFlares");
                    bloomEffect.brightPassFilterShader = Shader.Find("Hidden/BrightPassFilterForBloom");

                    bloomEffect.tweakMode = (UnityStandardAssets.ImageEffects.TweakMode34)((int)bloom["TweakMode"]);
                    bloomEffect.screenBlendMode = (UnityStandardAssets.ImageEffects.BloomScreenBlendMode)((int)bloom["BlendMode"]);
                    bloomEffect.hdr = (UnityStandardAssets.ImageEffects.HDRBloomMode)((int)bloom["HDR"]);

                    bloomEffect.bloomIntensity = bloom["Intensity"];
                    bloomEffect.bloomThreshold = bloom["Threshold"];
                    bloomEffect.bloomBlurIterations = bloom["BlurIterations"];
                    bloomEffect.sepBlurSpread = bloom["BlurSpread"];
                    bloomEffect.lensflares = false;
                    bloomEffect.useSrcAlphaAsMask = 0f;

                    m_lstEffects.Add(bloomEffect);
                }
            }
        }
    }

    public void ClearCameraBloom()
    {
        for(int i = 0; i < m_lstEffects.Count; ++i)
        {
            if(m_lstEffects[i]!=null)
            {
                GameObject.DestroyObject(m_lstEffects[i]);
            }
        }
    }
}

