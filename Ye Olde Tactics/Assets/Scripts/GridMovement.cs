using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GridMovement : MonoBehaviour {

    // Tile class is at the bottom of this code
    private GameObject selectedObject;	//destination cube
    public GameObject gridTile; // turn the renderer off for gameplay; visibility is for testing
    public GameObject startTile; // helps determine starting point of grid generation
	public GameObject activeChar;
    public GameObject gameManager;

    public LayerMask unwalkableMask; // set unwalkable mask on game objects that cannot be walked over
	private List<Node> path = new List<Node>();
	private Dictionary<int,Node> grid;
	private Dictionary<int,Node> explored; // Closed list; holds the nodes that have already been examined
	private MinHeap frontier; // Open List; holds the nodes that have been expanded but not examined
	public Node[] nodeArray;
	private Node start;
	private int tempG, tempH, tempF;
	private int count;
	private Vector3 dest;
    
	//Creates an array to hold data for the stage.  First array representes the row number, and the second represents the column number.
	//public Node[,] grid = new Node[height, width];  //Use the editor to fill array; it's  a public object after all

	//Come back to fix array declaration later

    void Start()
    {
		explored = new Dictionary<int,Node>();
		grid = new Dictionary<int,Node>();
		for (int i = 0; i < nodeArray.Length; ++i) {
			//print (nodeArray[i].key);
			grid.Add(nodeArray [i].key, nodeArray [i]);
			//print (nodeArray [i].key);
		}
    }

    // Update is called once per frame
    void Update()
    {
        // get selected tile
        if (gameManager.GetComponent<StateMachineScript>().isMove)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Node destination = GetClickedObject();
                path = new List<Node>();
                path = aStar(grid[(int)activeChar.transform.position.x * 100 + (int)activeChar.transform.position.z], destination);
                //print ("Path has been received with a length of " + path.Count);
                count = 0;
                //dest = path [count].position;
                Vector3.MoveTowards(activeChar.transform.position, path[count].position, 1.0f);
            }
            //If the path contains waypoints
            if (path.Count > 0)
            {
                if (count == path.Count - 1 && Vector3.Magnitude(path[count - 1].position - activeChar.transform.position) < 0.25f)
                {
                    activeChar.transform.position = path[count].position;
                    path.Clear();
                    count = 0;
                }
                activeChar.transform.position = Vector3.Lerp(activeChar.transform.position, path[count].position, Time.deltaTime);
                //If character is within radius of satisfaction....
                if (Vector3.Magnitude(path[count].position - activeChar.transform.position) < 0.25f)
                {
                    activeChar.transform.position = path[count].position;
                    if (count < path.Count - 1) count++;
                    activeChar.transform.position = Vector3.Lerp(activeChar.transform.position, path[count].position, Time.deltaTime);
                }
            }

        }
    }
    // get the clicked gameobject
    public Node GetClickedObject()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // get point on plane where clicked
        if (Physics.Raycast(ray, out hit))
        {
            selectedObject = hit.transform.gameObject;
        }
		int tempKey = (int)(selectedObject.transform.position.x * 100 + selectedObject.transform.position.z);


		return (Node)grid[tempKey];
    }

	/*
	 * Finds a path from the start Tile to the end Tile
	 */
	public List<Node> aStar(Node start, Node end)
	{
		start.parent = start;
		frontier = new MinHeap (start);

		if (start.position.x == start.parent.position.x || start.position.z == start.parent.position.z) {
			start.gCost = 10 + start.parent.gCost;
		} else {
			start.gCost = 14 + start.parent.gCost;		//Do we need to reset all of the gCost before reusing A*?
		}
		start.hCost = manhattanDist (start, end);
		start.fCost ();

		explored = new Dictionary<int, Node> ();
		while(!frontier.isEmpty())
		{
			Node curr = frontier.pop ();

			/*
			 * Resets all of the gCosts and fCosts for the nodes
			 */ 
			if (curr.Equals (end)) {
				generatePath (start, end);
				foreach(KeyValuePair<int,Node> node in grid){
					node.Value.gCost = node.Value.hCost = 0;
				}
				break;
			}

			explored.Add(curr.key,curr);

			//Generate Successor
			//Do not put successors in frontier is they are in explored, not pathable, or are out of bounds
			Node[] neighbors = curr.generateSuccessors();
			Node temp;

			for (int i = 0; i < neighbors.Length; ++i) {
				temp = neighbors[i];

				if (temp.isPathable && !explored.ContainsKey (temp.key)) {	//Executes if successor is pathable and hasn't been expanded
					
					//Activates if the neighbor is already in the frontier; sees which path gives the node the lowest gCost
					if (frontier.binaryHeap.Contains (temp)) {
						
						if (temp.position.x == temp.parent.position.x || temp.position.z == temp.parent.position.z) {
							tempG = 10 + curr.parent.gCost;
							if (tempG < temp.parent.gCost + 10) {
								temp.gCost = tempG;
							}
						} else {
							tempG = 14 + curr.parent.gCost;
							if (tempG < temp.parent.gCost + 14) {
								temp.gCost = tempG;
							}
						}

					} else {
						
						frontier.add (temp);
						temp.parent = curr;
						if (temp.position.x == temp.parent.position.x || temp.position.z == temp.parent.position.z) {
							temp.gCost = 10 + temp.parent.gCost;
						} else {
						temp.gCost = 14 + temp.parent.gCost;		//Do we need to reset all of the gCost before reusing A*?
						}

						temp.hCost = manhattanDist (temp, end);
						temp.fCost ();
					}
				}
			}
		}
		explored.Clear ();

		return path;
	}

	/*
	 * Calculuates the manhattan distance between two points.  This the heuristic function for A*
	 */
	private int manhattanDist(Node start, Node end)
	{
		return Mathf.Abs ((int)end.position.x - (int)start.position.z) + Mathf.Abs ((int)end.position.x - (int)start.position.z);
	}

	private void generatePath(Node start, Node end)
	{
		Stack<Node> genPath = new Stack<Node>();
		Node curr = end;
		while (curr != start) {
			genPath.Push (curr);
			curr = curr.parent;
		}
		path.Add (start);
		while(genPath.Count != 0) {
			path.Add (genPath.Pop());
		}
	}

}
	