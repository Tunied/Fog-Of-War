using UnityEngine;

namespace Code.FogOfWar.Core
{
    public class CEFowStaticExplorer
    {
        private int mViewRange;
        private float mViewRangeSquare;
        private Vector2Int mNowFowPos;

        public void Initialize(int _viewRange, Vector3 _worldPos)
        {
            mViewRange = _viewRange;
            mViewRangeSquare = mViewRange * mViewRange;
            mNowFowPos = CEFowFacade.GetFowPos(_worldPos);

            CEFowFacade.instance.staticExplorerList.Add(this);
            CEFowFacade.instance.painter.MarkStaticViewDataChange();
        }

        public void Dispose()
        {
            CEFowFacade.instance.staticExplorerList.Remove(this);
            CEFowFacade.instance.painter.MarkStaticViewDataChange();
        }

        public void FillMapData(bool[] _mapData)
        {
            var minX = mNowFowPos.x - mViewRange;
            var maxX = mNowFowPos.x + mViewRange;
            var minY = mNowFowPos.y - mViewRange;
            var maxY = mNowFowPos.y + mViewRange;
            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minY; y <= maxY; y++)
                {
                    //超出边界范围,不进行运算
                    if (x < 1 || y < 1 || x >= CEFowFacade.instance.fowMapWidth - 1 || y >= CEFowFacade.instance.fowMapHeight - 1) continue;

                    //在圆形区域外,不进行运算
                    var disSquare = (x - mNowFowPos.x) * (x - mNowFowPos.x) + (y - mNowFowPos.y) * (y - mNowFowPos.y);
                    if (disSquare > mViewRangeSquare) continue;

                    CEFowFacade.SetMap(_mapData, x, y);
                    CEFowFacade.SetMap(CEFowFacade.instance.data.exploreMapData, x, y);
                }
            }
        }
    }
}