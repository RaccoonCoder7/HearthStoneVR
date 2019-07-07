using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTouchMgr : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private Camera cam;
    private int layerFieldCard;
    private int layerHandCard;

    enum TouchState { Idle, CardStay, CardDrag, ModelStay, ModelDrag, Disable};
    TouchState state = TouchState.Idle;

    void Start()
    {
        layerHandCard = 1 << LayerMask.NameToLayer("HANDCARD");
        layerFieldCard = 1 << LayerMask.NameToLayer("FIELDCARD");
        cam = Camera.main;
    }

    void Update()
    {
        if(state == TouchState.Disable) return;

        if(state == TouchState.CardDrag){
            // TODO: buttonUp 에서 타겟팅한 오브젝트를 확인 후 할 동작을 정하고 실행.
            return;
        }

        if(Input.GetMouseButtonUp(0)){
            ray = cam.ScreenPointToRay(Input.mousePosition);
            // Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.green);
            if(Physics.Raycast(ray, out hit, 100.0f)){
                int layer = hit.transform.gameObject.layer;
                if(layer == layerFieldCard){
                    // TODO: 카드 보여주기
                } else if (layer == layerHandCard){
                    // TODO: 
                }
                // else if (){
                    // TODO: 
                // }
            }
            // if(Physics.Raycast(ray, out hit, 100.0f, layerBT)){
            //     hit.collider.GetComponent<Button>().onClick.Invoke();
            // }
        }
    }
}
