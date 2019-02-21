using Code.FogOfWar.Core;
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

        int r = Mathf.FloorToInt(viewRange / CEFowFacade.instance.worldMapWidth * CEFowFacade.instance.fowMapWidth);

        mExplorer.Initialize(r, mTrans.position);
    }

    // Update is called once per frame
    private void Update()
    {
        mExplorer?.Update(mTrans.position);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewRange);
    }
}