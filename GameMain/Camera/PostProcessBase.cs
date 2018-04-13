using UnityEngine;

namespace PostRender
{
    class PostRenderBase : MonoBehaviour
    {
        protected Shader m_Shader = null;
        protected Material m_Material = null;
        protected Material GetMaterial()
        {
            if (m_Material == null)
            {
                m_Material = new Material(m_Shader);
                m_Material.hideFlags = HideFlags.HideAndDontSave;
            }
            return m_Material;
        }

        protected void OnEnable()
        {
            if (!SystemInfo.supportsImageEffects)
            {
                enabled = false;
                return;
            }
            if (!m_Shader || !m_Shader.isSupported)
            {
                enabled = false;
                return;
            }
        }
        protected void OnDisable()
        {
            if (m_Material)
            {
                DestroyImmediate(m_Material);
            }
        }
    };
}

