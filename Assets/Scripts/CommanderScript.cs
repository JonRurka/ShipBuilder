using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommanderScript : MonoBehaviour 
{
    public List<GameObject> _parts;
    public int _partNum;

    public GameObject connectionPoint;
    public ConnectTwoPart connectionPoint_S;

    private bool _dragging;
    public int _index;

    


	// Use this for initialization
	void Start () 
    {
        connectionPoint = GameObject.Find("commander|connectionPoint|" + _index);
        connectionPoint_S = (ConnectTwoPart)connectionPoint.GetComponent<ConnectTwoPart>();
        connectionPoint_S.Parent = this.gameObject;
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
                    connectionPoint_S.Dragging = true;
                }
                else if (args[1].Equals("off"))
                {
                    _dragging = false;
                    connectionPoint_S.Dragging = false;
                }
                break;
        }
    }

    public void AddPart(GameObject part, int partNum, string type)
    {
        _parts.Add(part);

    }




}
