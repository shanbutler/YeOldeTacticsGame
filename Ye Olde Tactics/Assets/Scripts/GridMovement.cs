using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GridMovement : MonoBehaviour {

	/*
	 * Holds information for kinematic arrive algorithm.  A lot of this and the aStar algorithm will probably need to be moved to
	 * a different script so that the information is unique per character.  I'm keeping it as is so that someone else can move
	 * everything around once we implement multiple characters.  - Sean
	 */
	private float maxSpeed;		//Holds the maximum speed at which we can travel
	private float radius;		//Holds ths radius of satisfaction; if we are at least this far from the target, we can stop moving
	private float timeToTarget;		//Holds how quickly we wish to get to our target.  The smaller the number, the faster we will travel
	public Rigidbody rigid;			//Holds the rigidbody on our agent
	public Vector3 velocity;		//Holds the velocity vector for our agent; defaulted to the 0 vector
	public Transform trans;

	// Tile class is at the bottom of this code
	private GameObject selectedObject;	//destination cube
	public GameObject gridTile; // turn the renderer off for gameplay; visibility is for testing
	public GameObject startTile; // helps determine starting point of grid generation
	public LayerMask unwalkableMask; // set unwalkable mask on game objects that cannot be walked over
	public GameObject activeChar;		//Holds the character that is moving; used for testing purposes
	private Node start;	//Holds the starting node; used for testing purposes


	private Stack<Node> path = new Stack<Node>();		//Holds the path determined by A*
	private Dictionary<int,Node> grid, explored; // Holds the nodes of the grid and holds the nodes that have already been examined respectively
	private MinHeap frontier; // Open List; holds the nodes that have been expanded but not examined
	public Node[] nodeArray;	//Holds the nodes from the editor; used to populate grid
	private int tempG, tempH, tempF;	//Temporary values used for node comparison
	private int count;
	private Vector3 destination;

	/*
	* Instantiates the grid
	*/
	void Start()
	{
		//Sets information for the kinematic arrive algorithm
		destination = new Vector3(trans.position.x, trans.position.y, trans.position.z);
		maxSpeed = 20.0f;
		radius = .5f;
		timeToTarget = 2.0f;
		velocity = new Vector3 (0, 0, 0);

		grid = new Dictionary<int,Node>();
		for (int i = 0; i < nodeArray.Length; ++i) {
			grid.Add(nodeArray [i].key, nodeArray [i]);
		}
	}

	// Update is called once per frame
	void Update () {
		trans.rotation = Quaternion.Euler (0f, trans.rotation.eulerAngles.y, 0f);
		// get selected tile
		if (Input.GetMouseButtonDown(0))
		{      
			destination = GetClickedObject ().position;
			path = aStar(trans.position, destination);
			print ("Count: " + path.Count);
			foreach (Node thing in path) {
				if (thing.position != trans.position) {
					Debug.DrawLine (trans.position, thing.position, Color.red, 100.0f, false);
				}
			}
			destination = path.Pop ().position;
		}

		//Calculates the destinations for the pack
		destination = new Vector3(destination.x, trans.position.y, destination.z);

		//Calculates the distance vectors from each pack member's position to their destination
		velocity = 5.0f*(destination - trans.position);

		//Cancels movement if agent is within radius of satisfaction
		if (velocity.magnitude < radius) {	
			rigid.velocity = new Vector3 (0f, 0f, 0f);
			if (path.Count > 0) {
				destination = path.Pop ().position;
			} else {
				return;
			}
		}

		//Converts the distance vectors to velocity vectors
		velocity /= timeToTarget;

		//If the agent is going too fast, convert speed to max speed
		if (velocity.magnitude > maxSpeed) {
			Vector3.Normalize (velocity);
			velocity *= maxSpeed;
		}

		//Rotates the agent to face the direction of the velocity
		trans.rotation = Quaternion.Lerp (trans.rotation, 
			Quaternion.LookRotation (velocity), 
			Time.deltaTime * 2.0f);

		//Once the agent faces the velocity, the agent moves towards destination
		if (Vector3.Angle (Vector3.Normalize (trans.forward), Vector3.Normalize (velocity)) < 15.0f) {
			rigid.velocity = velocity;
		}
	}

	/*
	*	Retries the object that has been clicked and returns its node representation in the grid
	*/
	public Node GetClickedObject()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		// get point on plane where clicked
		if (Physics.Raycast(ray, out hit))
		{
			selectedObject = hit.transform.gameObject;
		}
		//Determines the key to access the grid node
		int tempKey = (int)(selectedObject.transform.position.x * 100 + selectedObject.transform.position.z);


		return (Node)grid[tempKey];
	}

	/*
	 * Finds a path from the start Tile to the end Tile
	 */
	public Stack<Node> aStar(Vector3 strt, Vector3 end)
	{
		print ((int)(strt.x * 100 + strt.z));
		Node start = grid [(int)(Mathf.Round(strt.x) * 100 + Mathf.Round(strt.z))];
		start.parent = start;
		frontier = new MinHeap (start);
		explored = new Dictionary<int, Node> ();

		while(!frontier.isEmpty())
		{
			Node curr = frontier.pop ();
			explored.Add(curr.key,curr);
			/*
			 * Resets all of the gCosts and fCosts for the nodes
			 */ 
			if (curr.equals (end)) {
				print ("About to finish: ");
				Stack<Node> returning = generatePath (start, grid[(int)(end.x*100 + end.z)]);
				foreach(KeyValuePair<int,Node> node in grid){
					node.Value.gCost = node.Value.hCost = 0;
				}
				return returning;
			}

			//Generate Successor
			//Do not put successors in frontier is they are in explored, not pathable, or are out of bounds
			Node[] neighbors = curr.generateSuccessors();
			print ("Hello, neighbor: " + neighbors.Length);
			Node temp;

			for (int i = 0; i < neighbors.Length; ++i) {
				temp = neighbors[i];

				if (temp.isPathable && !explored.ContainsKey (temp.key)) {	//Executes if successor is pathable and hasn't been expanded

					//Activates if the neighbor is already in the frontier; sees which path gives the node the lowest gCost
					if (frontier.binaryHeap.Contains (temp)) {

						if (temp.position.x == temp.parent.position.x || temp.position.z == temp.parent.position.z) {
							tempG = 10 + curr.gCost;
							if (tempG < temp.gCost) {
								temp.gCost = tempG;
								temp.fCost ();
								temp.parent = curr;
							}
						} else {
							tempG = 14 + curr.gCost;
							if (tempG < temp.gCost) {
								temp.gCost = tempG;
								temp.fCost ();
								temp.parent = curr;
							}
						}

					} else {		//If the neighbor has not been explored and is eligible, calculate the fcost and put it in frontier

						temp.parent = curr;
						if (temp.position.x == curr.position.x || temp.position.z == curr.position.z) {
							temp.gCost = 10 + curr.gCost;
						} else {
							temp.gCost = 14 + curr.gCost;
						}

						temp.hCost = euclideanDist (temp.position, end);
						temp.fCost ();
						frontier.add (temp);
					}
				}
			}
		}

		return null;
	}

	/*
	 * Calculuates the manhattan distance between two points.  This the heuristic function for A*
	 */
	private int manhattanDist(Node start, Node end)
	{
		return Mathf.Abs ((int)end.position.x - (int)start.position.z) + Mathf.Abs ((int)end.position.x - (int)start.position.z);
	}

	/*
	 * Testing purposes
	 */ 
	private int euclideanDist(Vector3 start, Vector3 end)
	{
		return Mathf.FloorToInt(Mathf.Sqrt(Mathf.Pow((int)end.x - (int)start.x, 2.0f) + Mathf.Pow ((int)end.z - (int)start.z, 2.0f)));
	}

	/*
	*Populates the global path variable to hold the path found by the A* algorithm
	*/
	private Stack<Node> generatePath(Node start, Node end)
	{
		Stack<Node> genPath = new Stack<Node>();
		Node curr = end;
		while (curr != start) {
			genPath.Push (curr);
			curr = curr.parent;
		}
		return genPath;
	}

}
	
