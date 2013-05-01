using UnityEngine;
using System.Collections;

public class ControlScript : MonoBehaviour 
{
    public Transform myTransform;

    public float speed;
    public float zoomSpeed;
    private float zoomTmp;

    private Ray ray;
    private RaycastHit hit;
    public Transform selectedTrans;
    public GameObject selectedObject;
    private bool selected;

    public bool editorMode;
    public bool draging;
    private string[] objectArgs;

    private string _type;

    private bool _connecting;
    private GameObject _connectingObject;
    float above, below, left, right;
    private float localDistY;
    private float localDistX;
    float offsetX;
    float offsetY;

    private CommanderScript commander;
    private StandardHallwayScript standardHallway;

	// Use this for initialization
	void Start () 
    {
        myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () 
    {
        Move();
        ObjectManagement();

        
	}

    private void Move()
    {
        float multiplier;

        zoomTmp = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;

        if (camera.orthographicSize <= 0.1f && zoomTmp < 0)
        {
            zoomTmp = 0;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            multiplier = 3;
        }
        else
        {
            multiplier = 1;
        }

        camera.orthographicSize += zoomTmp * multiplier;
        
        if (Input.GetKey(KeyCode.W))
        {
            myTransform.Translate(0, speed * Time.deltaTime * multiplier, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            myTransform.Translate(0, -speed * Time.deltaTime * multiplier, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            myTransform.Translate(-speed * Time.deltaTime * multiplier, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            myTransform.Translate(speed * Time.deltaTime * multiplier, 0, 0);
        }
    }

    private void ObjectManagement()
    {
        int objectRotation = 0;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.DrawLine(ray.origin, hit.point);
            
            //select the object
            if (Input.GetMouseButtonDown(0) && !selected)
            {
                selectedTrans = hit.transform;
                selectedObject = hit.transform.gameObject;
                selected = true;
                char[] delims = { '|' };
                objectArgs = selectedObject.name.Split(delims);
                _type = objectArgs[0];
                SendCmd(_type, "connected");
                Debug.Log(selectedObject.name + " selected");
            }

            //rotate an object
            if (Input.GetMouseButtonDown(1))
            {
                selectedTrans = hit.transform;
                selectedTrans.Rotate(0, 0, objectRotation += 90);
            }

            //open the menu of the object is selected and not in editor mode
            if (Input.GetMouseButtonDown(0) && !editorMode && selected)
            {
                //will create with GUI.
                Debug.Log(selectedObject.name + "'s menu.");
            }

            //drage the object if in editor mode.
            else if (Input.GetMouseButton(0) && editorMode && selected)
            {
                Debug.Log("dragging " + selectedObject.name);
                SendCmd(_type, "dragging-on");
                Screen.showCursor = false;
                draging = true;
            }

            //deselects the object.
            if (Input.GetMouseButtonUp(0) && draging)
            {
                SendCmd(_type, "dragging-off");
                selectedTrans = null;
                selectedObject = null;
                Screen.showCursor = true;
                selected = false;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            selectedTrans = null;
            selectedObject = null;
            selected = false;
        }

        if (draging && selectedTrans != null && Input.GetMouseButton(0))
        {
            if (
                _connecting && 
                _connectingObject != null &&
                (ray.origin.x <= right && 
                ray.origin.x >= left) &&
                (ray.origin.y <= above &&
                ray.origin.y >= below)
               )
            {
                localDistY = _connectingObject.transform.position.y - ray.origin.y;
                localDistX = _connectingObject.transform.position.x - ray.origin.x;

                if ((selectedTrans.localEulerAngles.z >= -0.1f && selectedTrans.localEulerAngles.z <= 0.1f) || 
                    (selectedTrans.localEulerAngles.z >= 179.9f && selectedTrans.localEulerAngles.z <= 180.1f))
                {
                    if (localDistY < 0)
                    {
                        selectedTrans.position = new Vector3(ray.origin.x, above - 0.01f, 0);
                    }
                    else if (localDistY > 0)
                    {
                        selectedTrans.position = new Vector3(ray.origin.x, below + 0.01f, 0);
                    }
                }

                else if ((selectedTrans.localEulerAngles.z >= 89.9f && selectedTrans.localEulerAngles.z <= 90.1f) ||
                         (selectedTrans.localEulerAngles.z >= 269.9f && selectedTrans.localEulerAngles.z <= 270.1f))
                {
                    if (localDistX < 0)
                    {
                        selectedTrans.position = new Vector3(right - 0.01f, ray.origin.y, 0);
                    }
                    else if (localDistX > 0)
                    {
                        selectedTrans.position = new Vector3(left + 0.01f, ray.origin.y, 0);
                    }
                }
            }
            else
            {
                selectedTrans.position = new Vector3(ray.origin.x, ray.origin.y, 0);
            }
        }
        else
        {
            draging = false;
        }
    }

    private void SendCmd(string part, string cmd)
    {
        switch (part)
        {
            case "commander":
                commander = (CommanderScript)selectedObject.GetComponent<CommanderScript>();
                commander.SubmitCommand(cmd);
                break;
            case "standardHallway":
                standardHallway = (StandardHallwayScript)selectedObject.GetComponent<StandardHallwayScript>();
                standardHallway.SubmitCommand(cmd);
                break;
        }
    }

    public bool Connecting
    {
        get { return _connecting; }
        set { _connecting = value; }
    }

    public GameObject ConnectingObject
    {


        get { return _connectingObject; }

        set 
        {
            float offsetX = 0;
            float offsetY = 0;
            _connectingObject = value;

            if (_connectingObject.transform.localEulerAngles.z >= -0.1f && _connectingObject.transform.localEulerAngles.z <= 0.1f ||
                _connectingObject.transform.localEulerAngles.z >= 179.9f && _connectingObject.transform.localEulerAngles.z <= 180.1f)
            {
                offsetX = _connectingObject.transform.lossyScale.x / 2;
                offsetY = _connectingObject.transform.lossyScale.y / 2;
            }
            else if (_connectingObject.transform.localEulerAngles.z >= 89.9f && _connectingObject.transform.localEulerAngles.z <= 90.1f ||
                     _connectingObject.transform.localEulerAngles.z >= 269.9f && _connectingObject.transform.localEulerAngles.z <= 270.1f)
            {
                offsetX = _connectingObject.transform.lossyScale.y / 2;
                offsetY = _connectingObject.transform.lossyScale.x / 2;
            }

            above = _connectingObject.transform.position.y + offsetY + selectedTrans.lossyScale.y / 2;
            below = _connectingObject.transform.position.y - (offsetY + selectedTrans.lossyScale.y / 2);
            left = _connectingObject.transform.position.x - (offsetX + selectedTrans.lossyScale.x / 2);
            right = _connectingObject.transform.position.x + offsetX + selectedTrans.lossyScale.x / 2;
        }
    }

    

}
