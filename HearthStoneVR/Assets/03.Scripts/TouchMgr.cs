using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchMgr : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private Camera cam;
    private int layerBT;

    // Start is called before the first frame update
    void Start()
    {
        layerBT = 1 << LayerMask.NameToLayer("BUTTON");
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
            if(Input.GetMouseButtonUp(0)){
                ray = cam.ScreenPointToRay(Input.mousePosition);
                // Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.green);
                if(Physics.Raycast(ray, out hit, 100.0f, layerBT)){
                    hit.collider.GetComponent<Button>().onClick.Invoke();
                }
            }
    }
}
