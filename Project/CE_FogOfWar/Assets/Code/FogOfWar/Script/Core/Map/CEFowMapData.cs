namespace Code.FogOfWar.Core
{
    public class CEFowMapData
    {
        /// <summary>
        /// 迷雾数据,如果为TRUE则表示当前Block已经被探索.
        ///
        /// Fog数据在Explorer Init/Move 时候在主线程直接给Modify掉,Painter线程直接负责重绘RT.
        /// </summary>
        public bool[] exploreMapData;

        /// <summary>
        /// 视野数据, 如果为TRUE则表示当前Block为可见区域
        ///
        /// ViewData 在Painter线程内,每次Update时候进行重绘,主线程不操作该Data数据
        /// 
        /// </summary>
        public bool[] viewMapData;

    }
}