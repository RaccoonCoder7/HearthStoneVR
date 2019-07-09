using UnityEngine;

public class FollowMouseDemo : MonoBehaviour
{
    public ArrowRenderer arrowRenderer;

    private Ray ray;
    private RaycastHit hit;
    private Camera cam;
    
    private void Start()
    {
        cam = Camera.main;
    }
    
    void Update()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.green);
        if (Physics.Raycast(ray, out hit, 100.0f)){
            arrowRenderer.SetPositions(transform.position, hit.point);
        }
    }
}
