using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConnectTwoPart : MonoBehaviour 
{
    public const string CONNECTION_POINT_TAG = "connectionPoint";

    public int _index;
    private bool _dragging;
    public GameObject controller;
    private ControlScript control_S;

    string[] nameArgs;

    public List<GameObject> connectedTo;

	// Use this for initialization
	void Start () 
    {
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

    public bool Dragging
    {
        get { return _dragging; }
        set { _dragging = value; }
    }

    void OnTriggerEnter(Collider other)
    {
        if (_dragging && other.tag == CONNECTION_POINT_TAG)
        {
            Debug.Log(name + " is touching " + other.name); 
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
                Debug.Log(name + " connected to " + other.name); 
                connectedTo.Add(other.gameObject);
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
