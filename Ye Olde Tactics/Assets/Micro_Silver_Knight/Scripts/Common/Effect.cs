using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour 
{


	public GameObject fireEffect;


	void Start()
	{
		fireEffect.SetActive(false);
	
	}



	void Skill()
	{
		fireEffect.SetActive(true);


	}

	


	void Destroy()
	{
		fireEffect.SetActive(false);
	
	}

}
