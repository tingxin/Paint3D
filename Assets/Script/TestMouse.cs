using UnityEngine;
using System.Collections;

public class TestMouse : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 pos2 = Camera.main.WorldToScreenPoint(this.transform.position);
        Vector3 rt = new Vector3(this.transform.position.x + this.transform.lossyScale.x / 2, this.transform.position.y + this.transform.lossyScale.y / 2, 0);
        Vector3 pos3 = Camera.main.WorldToScreenPoint(rt);
        if (Input.GetMouseButtonDown(0))
        {
            print("------------" + Input.mousePosition.ToString());
            print(pos3.ToString());
            Vector3 test = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            Vector3 back = Camera.main.ScreenToWorldPoint(pos3);
            print("+++++++++++++" + pos.ToString());
        }
        else
        {
            //print("+++++++++++++" + Input.mousePosition.ToString());
        }




        //this.gameObject.transform.position = pos;
    }
}
