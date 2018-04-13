using UnityEngine;

namespace PostRender
{
    class K3DPostRenderRadialBlur : PostRenderBase
    {
        public float m_fCenterX = 0.5f;
        public float m_fCenterY = 0.5f;

        [Range(0.0f, 2.0f)]
        public float m_fSampleDist = 0.14f;

        [Range(1.0f, 10.0f)]
        public float m_fSampleStrength = 1f;


        void Awake()
        {
            m_Shader = Shader.Find("MyShader/RadialBlur");
            //判断是否支持屏幕特效  
            if (!SystemInfo.supportsImageEffects)
            {
                enabled = false;
                return;
            }
        }
        void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
        {
            if (m_fSampleDist != 0 && m_fSampleStrength != 0)
            {
                int rtW = sourceTexture.width / 1;
                int rtH = sourceTexture.height / 1;

                GetMaterial().SetFloat("_SampleDist", m_fSampleDist);
                GetMaterial().SetFloat("_SampleStrength", m_fSampleStrength);

                RenderTexture rtTempA = RenderTexture.GetTemporary(rtW, rtH, 0, RenderTextureFormat.Default);
                rtTempA.filterMode = FilterMode.Bilinear;

                Graphics.Blit(sourceTexture, rtTempA);


                RenderTexture rtTempB = RenderTexture.GetTemporary(rtW, rtH, 0, RenderTextureFormat.Default);
                rtTempB.filterMode = FilterMode.Bilinear;
                //模糊计算
                Graphics.Blit(rtTempA, rtTempB, m_Material, 0);


                m_Material.SetTexture("_BlurTex", rtTempB);
                Graphics.Blit(sourceTexture, destTexture, m_Material, 1);

                RenderTexture.ReleaseTemporary(rtTempA);
                RenderTexture.ReleaseTemporary(rtTempB);


            }
            else
            {
                // 不做处理
                Graphics.Blit(sourceTexture, destTexture);
            }
        }


    }
}

