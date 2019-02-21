using Code.FogOfWar.Core;
using UnityEditor;
using UnityEngine;

public class DebugStaticExplorer : MonoBehaviour
{
    public float viewRange;

    private CEFowStaticExplorer mExplorer;

    private Transform mFrogTrans;

    private void Start()
    {
        mFrogTrans = GameObject.Find("Frog").transform;

        Invoke(nameof(OnTime), 1);
    }

    private void OnTime()
    {
        Debug.Log("Called");
        var r = Mathf.FloorToInt(viewRange / CEFowFacade.instance.worldMapWidth * CEFowFacade.instance.fowMapWidth);
        mExplorer = new CEFowStaticExplorer();
        mExplorer.Initialize(r, transform.position);
    }

    private void Update()
    {
        if (!(Vector3.Distance(mFrogTrans.position, transform.position) < 2f)) return;

        Destroy(gameObject);
        mExplorer.Dispose();
    }

    void OnDrawGizmosSelected()
    {
        Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, viewRange);
    }
}