using UnityEngine;
using System.Collections;
using Valve.VR;

public class TakePenController : MonoBehaviour
{

    private SteamVR_TrackedObject trackedObject;

    private EVRButtonId triggerButton = EVRButtonId.k_EButton_SteamVR_Trigger;

    private SteamVR_Controller.Device controller
    {
        get
        {
            return SteamVR_Controller.Input((int)trackedObject.index);
        }
    }

    private GameObject pickup = null;
    // Use this for initialization
    void Start()
    {
        this.trackedObject = this.GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.controller != null)
        {
            if (this.controller.GetPressDown(this.triggerButton) && this.pickup != null)
            {
                this.pickup.transform.parent = this.transform;
                this.pickup.GetComponent<Rigidbody>().isKinematic = true;
            }

            if (this.controller.GetPressUp(this.triggerButton) && this.pickup != null)
            {
                this.pickup.transform.parent = null;
				this.pickup.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        else
        {
            Debug.LogError("Controller should not be null");
        }
    }

    void OnTriggerEnter(Collider collider)
    {
		if ( this.pickup== null &&collider.gameObject.tag == "pen")
        {
            this.pickup = collider.gameObject;
        }

    }

    void OnTriggerExit(Collider collider)
    {
        if (this.pickup != null && collider.gameObject.tag == "pen"&& this.pickup == collider.gameObject)
        {
            this.pickup.GetComponent<Rigidbody>().isKinematic = false;
            this.pickup = null;
        }
    }
}
