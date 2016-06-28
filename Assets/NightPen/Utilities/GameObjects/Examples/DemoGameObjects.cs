using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NightPen.Utilities;

public class DemoGameObjects : MonoBehaviour
{
	void Awake()
	{
		GameObjects.Initialize();
	}
	
	/// <summary>
	/// Show all game objects named MyWall in the current scene. 
	/// </summary>
	void Start()
	{
		List<GameObject> gos = GameObjects.GetByName("MyWall");
		
		foreach(GameObject go in gos)
		{
			Debug.Log("GameObject: " + go.name + " found! activeSelf == " + go.activeSelf);
		}
	}
}
