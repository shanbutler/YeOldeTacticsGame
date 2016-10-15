using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour {

	public bool isPathable;
	public int gCost, hCost, finalCost;
	public Node parent;
	public Vector3 position;
	//public Node connection1, connection2, connection3, connection4, 
	//	connection5, connection6, connection7, connection8;
	public int key;
	public Node[] connections;

	public Node()
	{

	}

	// Use this for initialization
	void Awake () {
		gCost = hCost = finalCost = 0;
		position = GetComponent<Transform>().position;
		key = (int)(position.x * 100 + position.z);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void fCost()
	{
		finalCost = gCost + hCost;
	}

	public int compareToMin(Node otherNode)
	{
		if (this.finalCost < otherNode.finalCost)
			return -1;
		else if (this.finalCost > otherNode.finalCost)
			return 1;
		else
			return 0;
	}

	public Node[] generateSuccessors()
	{
		return connections;
	}
}
