using System.Collections.Generic;
using UnityEngine;

namespace Code.FogOfWar.Core
{
    public class CEFowExplorer
    {
        /// <summary>
        /// 当前Explorer自己的可视范围,当其Init/Move 时候自身进行Update
        /// </summary>
        public List<Vector2Int> selfViewMapDataList;

        private int mViewRange;
        private float mViewRangeSquare;

        private Vector2Int mNowFowPos;

        public void Initialize(int _viewRange, Vector3 _worldPos)
        {
            mViewRange = _viewRange;
            mViewRangeSquare = mViewRange * mViewRange;
            mNowFowPos = CEFowFacade.GetFowPos(_worldPos);
            selfViewMapDataList = new List<Vector2Int>();

            CEFowFacade.instance.dynamicExplorerList.Add(this);

            RunLogic();
        }

        public void Dispose()
        {
            CEFowFacade.instance.dynamicExplorerList.Remove(this);
        }

        public void Update(Vector3 _nowWorldPos)
        {
            var pos = CEFowFacade.GetFowPos(_nowWorldPos);
            if (pos != mNowFowPos)
            {
                mNowFowPos = pos;
                RunLogic();
            }
        }

        /// <summary>
        /// 更新FogData
        /// 通知Painter重绘RT
        /// </summary>
        private void RunLogic()
        {
            selfViewMapDataList.Clear();
            var minX = mNowFowPos.x - mViewRange;
            var maxX = mNowFowPos.x + mViewRange;
            var minY = mNowFowPos.y - mViewRange;
            var maxY = mNowFowPos.y + mViewRange;
            var isNeedRepaint = false;
            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minY; y <= maxY; y++)
                {
                    //超出边界范围,不进行运算
                    if (x < 1 || y < 1 || x >= CEFowFacade.instance.fowMapWidth - 1 || y >= CEFowFacade.instance.fowMapHeight - 1) continue;

                    //在圆形区域外,不进行运算
                    float disSquare = (x - mNowFowPos.x + 0.5f) * (x - mNowFowPos.x + 0.5f) + (y - mNowFowPos.y + 0.5f) * (y - mNowFowPos.y + 0.5f);
                    if (disSquare > mViewRangeSquare) continue;

                    isNeedRepaint = true;
                    selfViewMapDataList.Add(new Vector2Int(x, y));
                    CEFowFacade.SetMap(CEFowFacade.instance.data.exploreMapData, x, y);
                }
            }

            if (!isNeedRepaint) return;
            CEFowFacade.instance.painter.MarkDynamicViewDataChange();
            CEFowFacade.instance.painter.MarkExploreDataChange();
        }
    }
}