using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StandardHallwayScript : MonoBehaviour 
{
    public const string CONNECTION_POINT_TAG = "module";

    private bool _dragging;
    public int _index;
    public Vector3 _connectLoc;
    public GameObject controller;
    string[] nameArgs;
    private ControlScript control_S;
    public List<GameObject> connectedTo;

    

	// Use this for initialization
	void Start () 
    {
        ControlScript.parts[1] = this.gameObject;
        //ControlScript.parts.Add(this.gameObject);
        control_S = (ControlScript)controller.GetComponent<ControlScript>(); 
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    public int Index
    {
        get { return _index; }
        set { _index = value; }
    }

    public Vector3 ConnectLoc
    {
        get { return _connectLoc; }
        set { _connectLoc = value; }
    }

    public bool Dragging
    {
        get { return _dragging; }
        set { _dragging = value; }
    }

    public void SubmitCommand(string cmd)
    {
        char[] delims = { '-' };
        string[] args = cmd.Split(delims);

        switch (args[0].ToLower())
        {
            case "dragging":
                if (args[1].Equals("on"))
                {
                    _dragging = true;
                }
                else if (args[1].Equals("off"))
                {
                    _dragging = false;
                }
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (_dragging && other.tag == CONNECTION_POINT_TAG)
        {
            //Debug.Log(name + " is touching " + other.name);
            char[] delims = { '|' };
            nameArgs = other.name.Split(delims);
            control_S.Connecting = true;
            control_S.ConnectingObject = other.gameObject;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (_dragging && other.tag == CONNECTION_POINT_TAG)
        {
            if (Input.GetMouseButtonUp(0))
            {
                //Debug.Log(name + " connected to " + other.name);
                connectedTo.Clear();
                connectedTo.Add(other.gameObject);
                transform.parent = ControlScript.parts[0].transform;
                control_S.Connecting = false;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (_dragging && other.tag == CONNECTION_POINT_TAG)
        {
            control_S.Connecting = false;
        }
    }
}
