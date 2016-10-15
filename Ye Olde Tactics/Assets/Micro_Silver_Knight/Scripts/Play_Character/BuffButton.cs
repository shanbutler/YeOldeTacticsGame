using UnityEngine;
using System.Collections;

public class BuffButton : MonoBehaviour {

	public Texture2D normalTex;
	public Texture2D hoverTex;
	public GameObject frog;
	protected Animator avatar;

	void Start () 
	{
		avatar = frog.GetComponent<Animator>();
		avatar.SetBool("Buff", false);
	}

	
	private void OnMouseEnter ()
	{
		
		GetComponent<GUITexture>().texture = hoverTex;
	}
	
	private void OnMouseExit ()
	{
		
		GetComponent<GUITexture>().texture = normalTex;
	}
	
	
	private void OnMouseDown()
	{
		avatar.SetBool("Buff", true);
		
	}

	private void OnMouseUp()
	{
		avatar.SetBool("Buff", false);
	}

}
