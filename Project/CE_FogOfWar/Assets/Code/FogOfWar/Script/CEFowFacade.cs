using System.Collections.Generic;
using Code.FogOfWar.Core;
using Code.FogOfWar.Script;
using UnityEngine;

public class CEFowFacade
{
    private static CEFowFacade mInstance;

    public static CEFowFacade instance => mInstance ?? (mInstance = new CEFowFacade());

    public CEFowMapData data;
    public CEFowMapPainter painter;
    public Texture2D rawFowTexture;
    public RenderTexture fowTexture;

    public List<CEFowExplorer> dynamicExplorerList = new List<CEFowExplorer>();
    public List<CEFowStaticExplorer> staticExplorerList = new List<CEFowStaticExplorer>();

    public int fowMapWidth => mProperty.FowMapWidth;
    public int fowMapHeight => mProperty.FowMapHeight;
    public float worldMapWidth => mProperty.WorldMapWidth;
    public float worldMapHeight => mProperty.WorldMapHeight;


    private CEFowProperty mProperty;

    public void InitAsNew(CEFowProperty _fowProperty)
    {
        mProperty = _fowProperty;

        var size = fowMapWidth * fowMapHeight;
        data = new CEFowMapData {exploreMapData = new bool[size], viewMapData = new bool[size]};

        painter = new CEFowMapPainter();
        painter.Initialize(mProperty);

        rawFowTexture = new Texture2D(fowMapWidth, fowMapHeight, TextureFormat.RGB24, false);
        rawFowTexture.wrapMode = TextureWrapMode.Clamp;

        fowTexture = new RenderTexture(fowMapWidth, fowMapHeight, 0);

        DoInitShaderProperty();
    }


    public void Update()
    {
        painter.Update();
    }


    private void DoInitShaderProperty()
    {
        Shader.SetGlobalTexture("_FowTex", fowTexture);
        Shader.SetGlobalFloat("_FowInvisibleAreaAlpha", mProperty.FowInvisibleAreaAlpha);
        Shader.SetGlobalColor("_FowColor", mProperty.FowColor);
        Shader.SetGlobalFloat("_FowWorldWidth", mProperty.WorldMapWidth);
        Shader.SetGlobalFloat("_FowWorldHeight", mProperty.WorldMapHeight);
    }


    //====================
    //== Utils
    //====================

    public static bool IsWorldPosInView(Vector3 _worldPos)
    {
        var fowPos = GetFowPos(_worldPos);
        return GetMap(instance.data.viewMapData, fowPos.x, fowPos.y);
    }

    /// <summary>
    /// 世界坐标->迷雾Block坐标
    /// </summary>
    public static Vector2Int GetFowPos(Vector3 _worldPos)
    {
        var x = Mathf.FloorToInt(_worldPos.x / instance.worldMapWidth * instance.fowMapWidth);
        var y = Mathf.FloorToInt(_worldPos.z / instance.worldMapHeight * instance.fowMapHeight);
        return new Vector2Int(x, y);
    }

    /// <summary>
    /// 设置Fow的地图数据
    /// </summary>
    public static void SetMap(bool[] _map, int _x, int _y)
    {
        if (_map == null || _map.Length == 0) return;
        var index = _x + _y * instance.fowMapWidth;
        if (index < 0 || index > _map.Length) return;
        _map[index] = true;
    }

    public static void SetMap(bool[] _map, int _x, int _y, bool _result)
    {
        if (_map == null || _map.Length == 0) return;
        var index = _x + _y * instance.fowMapWidth;
        if (index < 0 || index > _map.Length) return;
        _map[index] = _result;
    }

    /// <summary>
    /// 取得地图数据
    /// </summary>
    public static bool GetMap(bool[] _map, int _x, int _y)
    {
        if (_map == null || _map.Length == 0) return false;
        var index = _x + _y * instance.fowMapWidth;
        if (index < 0 || index > _map.Length) return false;
        return _map[index];
    }
}