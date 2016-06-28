using UnityEngine;
using System.Collections;
using NightPen.Utilities;

public class SetActiveExample : MonoBehaviour
{
	void Awake()
	{
		GameObjects.Initialize();
	}
	
	IEnumerator Start()
	{
		//Set all squares inactive at once.
		GameObjects.SetActive("square", false);
		yield return new WaitForSeconds(1.0f);
		
		//Set the spheres inactive 1 at a time.
		GameObjects.SetActive("Sphere1", false);
		GameObjects.SetActive("Sphere2", false);
		yield return new WaitForSeconds(1.0f);
		
		//Set the first sphere active.
		GameObjects.SetActive("Sphere1", true);
	}
}
