using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NightPen.Utilities;

/// <summary>
/// This example shows how to get a component on a game object.
/// </summary>
public class DemoChangeUIText : MonoBehaviour
{
	void Awake()
	{
		//Force the game objects to be scanned.
		GameObjects.Initialize();
	}
	
	void Start()
	{
		//Change the text of the uGUI text component attached to the MyText game object.
		GameObjects.GetComponent<Text>("MyText").text = "The Text is Changed!";
		
		//Show the text to the player.
		GameObjects.Get("MyText").SetActive(true);
	}
}
