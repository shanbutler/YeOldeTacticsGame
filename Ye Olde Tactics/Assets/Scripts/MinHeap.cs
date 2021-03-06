using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinHeap
{
	public List<Node> binaryHeap;
	private int numberOfItems;
	private Node root;

	public MinHeap(Node thing) {
		binaryHeap = new List<Node> ();
		root = thing;
		binaryHeap.Add (thing);
	}

	public MinHeap() {
		root = null;
	}

	public void add(Node thing)
	{
		if (root == null) {
			root = thing;
		}
		binaryHeap.Add (thing);
		heapify ();
	}

	public Node pop()
	{
		Node temp = binaryHeap[0];
		binaryHeap.RemoveAt (0);
		heapify ();
		//root = binaryHeap [0];
		return temp;
	}

	public Node peek()
	{
		return binaryHeap [0];
	}

	public void heapify()
	{
		binaryHeap.Sort ((x, y) => x.compareToMin (y));
	}

	public bool isEmpty()
	{
		return binaryHeap.Count == 0;
	}
}
