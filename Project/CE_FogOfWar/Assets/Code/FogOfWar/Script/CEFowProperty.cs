using UnityEngine;

namespace Code.FogOfWar.Script
{
    public class CEFowProperty
    {
        public Color FowColor = Color.black;
        public float FowInvisibleAreaAlpha = 0.2f;

        public int blurSimpleDownSize = 1;
        public int blurIterations = 1;
        public float blurOffset = 0;

        public int FowMapWidth;
        public int FowMapHeight;
        public float WorldMapWidth;
        public float WorldMapHeight;
    }
}