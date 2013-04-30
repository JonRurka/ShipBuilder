using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StandardHallwayScript : MonoBehaviour 
{
    public const string CONNECTION_POINT_TAG = "connectionPoint";

    private bool _dragging;
    public int _index;
    public Vector3 _connectLoc;

    public List<GameObject> connectedTo;

    private ConnectTwoPart conPoint;
    private StandardHallwayScript standardHallway;

	// Use this for initialization
	void Start () 
    {
	    
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

    void OnTriggerStay(Collider other)
    {
        if (_dragging && other.tag == CONNECTION_POINT_TAG)
        {
            Debug.Log(name + " connected to " + other.name);

            char[] delims = { '|' };
            string[] args = other.name.Split(delims);

            if (Input.GetMouseButtonUp(0))
            {
                filterOther(other.gameObject, args[0]);
                //need to figure this out still.

                connectedTo.Add(other.gameObject);
            }

        }
    }

    void filterOther(GameObject other, string obName)
    {
        switch(obName)
        {
            case "commander":
                conPoint = (ConnectTwoPart)other.GetComponent<ConnectTwoPart>();
                break;
            case "standardHallway":
                standardHallway = (StandardHallwayScript)other.GetComponent<StandardHallwayScript>();
                break;
        }
    }
}
