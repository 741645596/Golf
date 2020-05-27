using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using IGG.Core;
using IGG.Core.Manger.Coroutine;
using System.Collections;

namespace UnityStandardAssets.ImageEffects
{
    [ExecuteInEditMode]
    [AddComponentMenu("Image Effects/Blur/Blur")]
    public class Blur : MonoBehaviour
    {
        private RawImage m_rawImage;
        public RenderTexture rt;
        
        /// Blur iterations - larger number means more blur.
        //public int iterations = 3;
        
        /// Blur spread for each iteration. Lower values
        /// give better looking blur, but require more iterations to
        /// get large blurs. Value is usually between 0.5 and 1.0.
        //public float blurSpread = 0.6f;
        
        // 采样参数，
        public uint SampleUnitOffset = 1;
        // 1 相当与1<< (1+1)采样为一个点，
        // 2.相当于1<< (2+1)采样为一个点，
        // 3.相当于1<< (3+1)采样为一个点，
        //依次类推
        
        
        // --------------------------------------------------------
        // The blur iteration shader.
        // Basically it just takes 4 texture samples and averages them.
        // By applying it repeatedly and spreading out sample locations
        // we get a Gaussian blur approximation.
        
        public Shader blurShader = null;
        
        static Material m_Material = null;
        protected Material material {
            get {
                if (m_Material == null)
                {
                    m_Material = new Material(blurShader);
                    m_Material.hideFlags = HideFlags.DontSave;
                }
                return m_Material;
            }
        }
        
        protected void OnDisable()
        {
            if (m_Material) {
                DestroyImmediate(m_Material);
            }
        }
        
        public virtual void OnDestroy()
        {
            gameObject.GetComponent<Camera>().targetTexture = null;
        }
        
        // --------------------------------------------------------
        
        protected void Start()
        {
            // Disable if we don't support image effects
            if (!SystemInfo.supportsImageEffects) {
                enabled = false;
                return;
            }
            // Disable if the shader can't run on the users graphics card
            if (!blurShader || !material.shader.isSupported) {
                enabled = false;
                return;
            }
        }
        
        
        // Performs one blur iteration.
        /*public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
        {
            float off = 0.5f + iteration * blurSpread;
            Graphics.BlitMultiTap(source, dest, material,
                new Vector2(-off, -off),
                new Vector2(-off,  off),
                new Vector2(off,  off),
                new Vector2(off, -off)
            );
        }*/
        
        // Downsamples the texture to a quarter resolution.
        private void DownSamplexX(RenderTexture source, RenderTexture dest, uint offset)
        {
            if (offset == 0) {
                return;
            }
            List<Vector2> l = new List<Vector2>();
            for (int i = (int)offset * -1; i <= (int)offset; i++) {
                if (i == 0) {
                    continue;
                }
                for (int j = (int)offset * -1; j <= (int)offset; j++) {
                    if (j == 0) {
                        continue;
                    }
                    l.Add(new Vector2(i, j));
                }
            }
            
            //float off = offset * 1.0f;
            Graphics.BlitMultiTap(source, dest, material, l.ToArray());
            /*Graphics.BlitMultiTap(source, dest, material,
                new Vector2(-off, -off),
                new Vector2(-off,  off),
                new Vector2(off,  off),
                new Vector2(off, -off)
            );*/
        }
        
        // Called by the camera to apply the image effect
        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (rt != null) {
                m_rawImage.texture = rt;
                //gameObject.GetComponent<Camera>().targetTexture = null;
                this.enabled = false;
            }
            return;
            int sampleParam = 1 << ((int)SampleUnitOffset + 1);
            if (m_rawImage != null) {
                int rtW = source.width / sampleParam;
                int rtH = source.height / sampleParam;
                
                RenderTexture buffer = RenderTexture.GetTemporary(rtW, rtH, 0);
                
                // Copy source to the 4x4 smaller texture.
                DownSamplexX(source, buffer, SampleUnitOffset);
                
                // Blur the small texture
                /*for (int i = 0; i < iterations; i++) {
                    RenderTexture buffer2 = RenderTexture.GetTemporary(rtW, rtH, 0);
                    FourTapCone(buffer, buffer2, i);
                    RenderTexture.ReleaseTemporary(buffer);
                    buffer = buffer2;
                }*/
                m_rawImage.texture = buffer;
                this.enabled = false;
            }
        }
        
        /// <summary>
        /// 关联Rawimage
        /// </summary>
        /// <param name="img"></param>
        public void SetRawImage(RawImage img)
        {
            /*if (img != null) {
                m_rawImage = img;
                this.enabled = true;
            }*/
            if (img != null) {
                m_rawImage = img;
                this.enabled = true;
                if (rt != null) {
                    gameObject.GetComponent<Camera>().targetTexture = rt;
                }
            }
            StartCoroutine(EmptytargetTexture());
        }
        /// <summary>
        /// 关闭模糊效果
        /// </summary>
        public void HideBlur()
        {
            gameObject.GetComponent<Camera>().targetTexture = null;
            /*if (m_rawImage != null) {
                RenderTexture.ReleaseTemporary((RenderTexture)this.m_rawImage.texture);
            }*/
            this.enabled = false;
        }
        
        private IEnumerator EmptytargetTexture()
        {
            yield return Yielders.GetWaitForSeconds(0.5f);
            gameObject.GetComponent<Camera>().targetTexture = null;
        }
    }
}
