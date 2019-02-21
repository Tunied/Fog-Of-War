using Code.FogOfWar.Script;
using UnityEngine;
using UnityEngine.UI;

public class FowDebugRoot : MonoBehaviour
{
    public RawImage img;
    public DebugExplorer actor;

    public float worldWidth = 50f;
    public float worldHeight = 50f;
    public int FowTextureSize = 512;

    [Range(0, 5)]
    public int blurSimpleDownSize = 1;

    [Range(0, 10)]
    public int blurIterations = 1;

    [Min(0)]
    public float blurOffset;

    private CEFowProperty mProperty;

    // Start is called before the first frame update
    private void Start()
    {
        mProperty = new CEFowProperty
        {
            FowMapWidth = FowTextureSize,
            FowMapHeight = FowTextureSize,
            WorldMapWidth = worldWidth,
            WorldMapHeight = worldHeight,
            blurIterations = blurIterations,
            blurOffset = blurOffset,
            blurSimpleDownSize = blurSimpleDownSize
        };

        CEFowFacade.instance.InitAsNew(mProperty);

        actor.Initialize();
        img.texture = CEFowFacade.instance.fowTexture;
    }

    private void Update()
    {
        mProperty.blurIterations = blurIterations;
        mProperty.blurOffset = blurOffset;
        mProperty.blurSimpleDownSize = blurSimpleDownSize;

        CEFowFacade.instance.Update();
    }
}