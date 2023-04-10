using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugTool : MonoBehaviour
{
    public Transform temp;
    public Transform pointParent;
    private List<Transform> pointList = new List<Transform>();   
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.AddListener<Vector2,Color>(MyEventType.addDebugPoints , SetDebugPointPosition);

        EventCenter.AddListener<bool>(MyEventType.triggetDebugTool , triggetDebugTool);

        pointParent.gameObject.SetActive(false);

        GetComponent<CanvasScaler>().referenceResolution=new Vector2(Screen.width, Screen.height);
    }

    public void triggetDebugTool(bool value)
    {
         pointParent.gameObject.SetActive(value);
    }

    public void ClearPoints()
    {
        pointList.Clear();  

    }
    public void SetDebugPointPosition(Vector2 pos,Color _c)
    {
       var newPoint= Instantiate(temp, pointParent);
        newPoint.localPosition = pos;
        newPoint.GetComponent<Image>().color = _c;  
        newPoint.gameObject.SetActive(true);

        pointList.Add(newPoint);
    }

    private void OnDestroy()
    {
        ClearPoints();
    }
}
