using UnityEngine;
using System.Collections;
using DrawingTool;
using Valve.VR;

public class Painter : MonoBehaviour
{

    public class BrushToolSetting : PropertyAttribute
    {
        public BrushToolSetting(float width, float hardness, float spacing)
        {
            this.Width = width;
            this.Hardness = hardness;
            this.Spacing = spacing;
        }
        public float Width;
        public float Hardness;
        public float Spacing;
    }

    public class EraserToolSetting : PropertyAttribute
    {

        public EraserToolSetting(float width, float hardness)
        {
            this.Width = width;
            this.Hardness = hardness;
        }

        public float Width;
        public float Hardness;
    }


    #region properties
    public Tool CurrentTool = Tool.Brush;
    public Color CurrentColor = Color.black;
    public BrushToolSetting BrushSetting = new BrushToolSetting(10, 5, 5);
    public EraserToolSetting EraserSetting = new EraserToolSetting(100, 50);

    public Camera PaintCamera;
    public Texture2D TextureCanvas;
    public bool NeedAdjust = false;
    public bool UseMouse = false;

   
    #endregion

    #region members
    private Vector3 dragPrePoint;
    private Vector3 dragPoint;

    private BoundRec canvasBound;
    private float textureWidth;
    private float textureHeight;

    private ArrayList currentLline = new ArrayList();

    private GameObject pickupPen = null;

    #endregion


    // Use this for initialization
    void Start()
    {
        this.canvasBound = new BoundRec(this.gameObject);
        this.textureWidth = (float)this.TextureCanvas.width;
        this.textureHeight = (float)this.TextureCanvas.height;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 touchPoint = Vector3.zero;

        if (this.pickupPen != null)
        {
            if (this.UseMouse == false)
            {
                Transform penHead = this.pickupPen.transform.GetChild(0);
                touchPoint = this.TranslatePointFromCameraToTexture(penHead.position);
                this.PenMove(touchPoint);

                //Follow code is low performance
                //Ray touchRay = new Ray (this.pickupPen.transform.position, this.pickupPen.transform.forward*-1);
                //RaycastHit hit;
                //if (Physics.Raycast (touchRay, out hit, 100)) {
                //	if (hit.collider.gameObject == this.gameObject) {
                //		touchPoint = this.TranslatePointFromCameraToTexture(hit.point);
                //		this.PenMove(touchPoint);
                //	}
                //}
            }
            else
            {
                Ray touchRay = this.PaintCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(touchRay, out hit, 100))
                {
                    if (hit.collider.gameObject == this.gameObject)
                    {
                        touchPoint = this.TranslatePointFromCameraToTexture(hit.point);
                        this.PenMove(touchPoint);
                    }
                }
            }
        }
    }


    void OnTriggerEnter(Collider collider)
    {

        Pencil pen = collider.gameObject.GetComponent<Pencil>();
        if (pen != null)
        {
            this.pickupPen = collider.gameObject;
            this.CurrentColor = pen.PenColor;

            Transform penHead = this.pickupPen.transform.GetChild(0);
            Vector3 touchPoint = this.TranslatePointFromCameraToTexture(penHead.position);
           
            this.dragPrePoint = touchPoint;
            this.dragPoint = touchPoint;

            if (pen.IsErase)
            {
                this.CurrentTool = Tool.Eraser;
            }
            else
            {
                this.CurrentTool = Tool.Brush;
            }

        }
    }

    void OnTriggerExit(Collider collider)
    {
        Pencil pen = collider.gameObject.GetComponent<Pencil>();
        if (pen != null)
        {
            this.pickupPen = null;

        }
    }

    void PenBegin(Vector3 touchPoint)
    {
        this.dragPrePoint = touchPoint;
        this.dragPoint = touchPoint;
    }

    void PenMove(Vector3 touchPoint)
    {
        this.dragPoint = touchPoint;

        Vector3 diff = this.dragPoint - this.dragPrePoint;
        if (Mathf.Abs(diff.x) > 1 || Mathf.Abs(diff.y) > 1)
        {
            if (this.CurrentTool == Tool.Brush)
            {
                Debug.LogWarning("use the pen");
                Drwaing.DrawLine(this.dragPoint, this.dragPrePoint, BrushSetting.Width, CurrentColor, BrushSetting.Hardness, this.TextureCanvas);
            }
            else
            {
                Debug.LogWarning("use the erase");
                Drwaing.DrawLine(this.dragPoint, this.dragPrePoint, EraserSetting.Width, Color.white, EraserSetting.Hardness, this.TextureCanvas);
            }
            this.dragPrePoint = this.dragPoint;
        }


    }

    void PenEnd(Vector3 touchPoint)
    {
        this.dragPrePoint = Vector3.zero;
        this.dragPoint = Vector3.zero;

        if (this.NeedAdjust)
        {
            this.AdjustLastLine();
        }
    }

    void AdjustLastLine()
    {
        //TODO: Add Adjust line feature here
        //First: erase last line 
        //second: use the trace to get a new line adjusted
        //third: add the new line
    }

    Vector3 TranslatePointFromCameraToTexture(Vector3 source)
    {
        //Here is temp code for translate
        //If we use HTC Vive controller or other input device
        //we can get the real touch point map to canvas


        Vector3 relativeToObj = source - this.canvasBound.Center;
        Vector3 relativeToTexture = new Vector3(relativeToObj.x * -1, relativeToObj.y * -1);
        Vector3 relativeToCorner = relativeToTexture - this.canvasBound.LTCorner;
        float left = (relativeToCorner.x * this.textureWidth / this.canvasBound.Width);
        float top = (relativeToCorner.y * this.textureHeight / this.canvasBound.Height);
        Vector3 result = new Vector3(left, top * -1, 0.00f);
        return result;
    }

    float Distance(Vector3 diff)
    {
        return Mathf.Sqrt(diff.x * diff.x + diff.y * diff.y);
    }
}

public enum Tool
{
    Brush,
    Eraser,
    None
}

public class BoundRec
{
    public BoundRec(GameObject obj)
    {
        this.Left = obj.transform.position.x - obj.transform.lossyScale.x / 2;
        this.Top = obj.transform.position.y + obj.transform.lossyScale.y / 2;
        this.Right = obj.transform.position.x + obj.transform.lossyScale.x / 2;
        this.Bottom = obj.transform.position.y - obj.transform.lossyScale.y / 2;
        this.Height = obj.transform.lossyScale.y;
        this.Width = obj.transform.lossyScale.x;

        this.Center = new Vector3(obj.transform.position.x, obj.transform.position.y);
        this.LTCorner = new Vector3(this.Left, this.Top, 0);
    }
    public float Left { private set; get; }
    public float Top { private set; get; }
    public float Bottom { private set; get; }
    public float Right { private set; get; }
    public float Height { private set; get; }
    public float Width { private set; get; }
    public Vector3 Center { private set; get; }
    public Vector3 LTCorner { get; private set; }
}

