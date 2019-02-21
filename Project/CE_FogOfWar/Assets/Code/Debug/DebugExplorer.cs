using Code.FogOfWar.Core;
using UnityEditor;
using UnityEngine;

public class DebugExplorer : MonoBehaviour
{
    public float viewRange;

    private CEFowExplorer mExplorer;

    private Transform mTrans;

    public void Initialize()
    {
        mTrans = transform;
        mExplorer = new CEFowExplorer();

        var r = Mathf.FloorToInt(viewRange / CEFowFacade.instance.worldMapWidth * CEFowFacade.instance.fowMapWidth);

        mExplorer.Initialize(r, mTrans.position);
    }

    // Update is called once per frame
    private void Update()
    {
        mExplorer?.Update(mTrans.position);
    }

    void OnDrawGizmosSelected()
    {
        Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, viewRange);
    }
}