using Code.FogOfWar.Script;
using UnityEngine;

namespace Code.FogOfWar.Core
{
    public class CEFowMapPainter
    {
        private bool[] mCacheStaticViewMapData;
        private Color[] mColorBuffer;

        private bool mIsNeedRepaint;
        private bool mIsStaticViewDataChange;

        private Shader mBlurShader;
        private Material mBlurMT;

        private CEFowProperty mProperty;

        public void Initialize(CEFowProperty _property)
        {
            mProperty = _property;

            var mapSize = CEFowFacade.instance.fowMapWidth * CEFowFacade.instance.fowMapHeight;
            mCacheStaticViewMapData = new bool[mapSize];
            mColorBuffer = new Color[mapSize];

            mBlurShader = Shader.Find("Hidden/Eran/FOW/Blur");
            if (mBlurShader != null)
            {
                mBlurMT = new Material(mBlurShader);
            }
        }

        public void MarkExploreDataChange()
        {
            mIsNeedRepaint = true;
        }

        public void MarkStaticViewDataChange()
        {
            mIsStaticViewDataChange = true;
            mIsNeedRepaint = true;
        }

        public void MarkDynamicViewDataChange()
        {
            mIsNeedRepaint = true;
        }

        public void Update()
        {
            if (!mIsNeedRepaint) return;
            mIsNeedRepaint = false;

            RunRepaintLogic();
        }


        private void RunRepaintLogic()
        {
            if (mIsStaticViewDataChange)
            {
                RecalculateStaticViewMapData();
                mIsStaticViewDataChange = false;
            }

            RecalculateColorBuffer();
            RepaintTexture();
        }


        private void RecalculateColorBuffer()
        {
            var tempViewMapData = (bool[]) mCacheStaticViewMapData.Clone();
            CEFowFacade.instance.dynamicExplorerList.ForEach(explorer => { explorer.selfViewMapDataList.ForEach(pos => { CEFowFacade.SetMap(tempViewMapData, pos.x, pos.y); }); });
            CEFowFacade.instance.data.viewMapData = tempViewMapData;

            for (var x = 0; x < CEFowFacade.instance.fowMapWidth; x++)
            {
                for (var y = 0; y < CEFowFacade.instance.fowMapHeight; y++)
                {
                    var color = new Color
                    {
                        r = CEFowFacade.GetMap(CEFowFacade.instance.data.exploreMapData, x, y) ? 1 : 0,
                        g = CEFowFacade.GetMap(tempViewMapData, x, y) ? 1 : 0,
                        b = 0,
                    };

                    mColorBuffer[x + y * CEFowFacade.instance.fowMapWidth] = color;
                }
            }
        }

        private void RecalculateStaticViewMapData()
        {
        }

        private void RepaintTexture()
        {
            CEFowFacade.instance.rawFowTexture.SetPixels(mColorBuffer);
            CEFowFacade.instance.rawFowTexture.Apply();

            Graphics.Blit(CEFowFacade.instance.rawFowTexture, CEFowFacade.instance.fowTexture);
            BlurImage(CEFowFacade.instance.fowTexture, CEFowFacade.instance.fowTexture);
        }

        private void BlurImage(RenderTexture source, RenderTexture destination)
        {
            if (mBlurMT == null)
            {
                Graphics.Blit(source, destination);
                return;
            }

            var widthMod = 1.0f / (1.0f * (1 << mProperty.blurSimpleDownSize));

            mBlurMT.SetVector("_Parameter", new Vector4(mProperty.blurOffset * widthMod, -mProperty.blurOffset * widthMod, 0.0f, 0.0f));
            source.filterMode = FilterMode.Bilinear;

            var rtW = source.width >> mProperty.blurSimpleDownSize;
            var rtH = source.height >> mProperty.blurSimpleDownSize;
            var rt = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);

            rt.filterMode = FilterMode.Bilinear;
            Graphics.Blit(source, rt, mBlurMT, 0);

            for (var i = 0; i < mProperty.blurIterations; i++)
            {
                var iterationOffs = i * 1.0f;
                mBlurMT.SetVector("_Parameter", new Vector4(mProperty.blurOffset * widthMod + iterationOffs, -mProperty.blurOffset * widthMod - iterationOffs, 0.0f, 0.0f));

                // vertical blur
                var rt2 = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);
                rt2.filterMode = FilterMode.Bilinear;
                Graphics.Blit(rt, rt2, mBlurMT, 1);
                RenderTexture.ReleaseTemporary(rt);
                rt = rt2;

                // horizontal blur
                rt2 = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);
                rt2.filterMode = FilterMode.Bilinear;
                Graphics.Blit(rt, rt2, mBlurMT, 2);
                RenderTexture.ReleaseTemporary(rt);
                rt = rt2;
            }

            Graphics.Blit(rt, destination);
            RenderTexture.ReleaseTemporary(rt);
        }
    }
}