using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using NightPen.Utilities;

public class PerformanceTest : MonoBehaviour
{
	public bool testOnOff = true;
	
	public void ToggleTestOnOff()
	{
		testOnOff = (testOnOff) ? false : true;
	}
	
	// Use this for initialization
	void Awake()
	{
		GameObjects.Initialize();
	}
	
	// Update is called once per frame
	void Update()
	{
		//For performance testing only. You should not do this in a real app.
		if ( testOnOff )
		{
			List<GameObject> gos = GameObjects.GetByName("AnObject");
			(GameObjects.GetFirstOrDefaultByName("ObjectsFound").GetComponent<Text>()).text = gos.Count.ToString();
		}
	}
}
