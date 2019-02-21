using UnityEngine;

public class DebugStalker : MonoBehaviour
{
    private GameObject mChild;

    private Transform mTrans;

    // Start is called before the first frame update
    private void Start()
    {
        mTrans = transform;
        mChild = mTrans.GetChild(0).gameObject;
    }

    // Update is called once per frame
    private void Update()
    {
        mChild.SetActive(CEFowFacade.IsWorldPosInView(mTrans.position));
    }
}