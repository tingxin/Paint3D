using UnityEngine;
using System.Collections;

using NightPen.Utilities;

public class ParentChildExample : MonoBehaviour
{
	void Awake()
	{
		//Force the game objects to be scanned.
		GameObjects.Initialize();
	}
	
	/// <summary>
	/// This example shows how to get a game object using the full path name
	/// of the game object.
	/// </summary>
	IEnumerator Start()
	{
		for(int row=1; row<4; ++row)
		{
			for(int cube=1; cube<5; ++cube)
			{
				GameObjects.Get("BunchOfCubes/Row "+row.ToString()+"/Cube "+cube.ToString()).SetActive(false);
				yield return new WaitForSeconds(.1f);
			}
		}
	}
}
