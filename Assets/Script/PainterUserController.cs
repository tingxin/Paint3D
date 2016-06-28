using UnityEngine;
using System.Collections;
using DrawingTool;

public class PainterUserController : MonoBehaviour
{

    // Use this for initialization
    public Camera PaintCamera;
    private GameObject pickup;

    private Quaternion zero = new Quaternion(0, 0, 0, 0);
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;


        Ray touchRay = this.PaintCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(touchRay, out hit, 100))
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pencil pen = hit.collider.gameObject.GetComponent<Pencil>();
                if (pen != null)
                {
                    this.pickup = hit.collider.gameObject;
                }
                else
                {
                    if (this.pickup != null)
                    {
                        this.pickup.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z + 0.45f);
                    }
                }

            }
            else if (Input.GetMouseButton(0))
            {
                if (this.pickup != null)
                {
                    this.pickup.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z + 0.45f);

                    this.pickup.transform.rotation = zero;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (this.pickup != null)
                {
                    this.pickup = null;
                }
            }
        }
    }
}
