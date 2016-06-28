using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NightPen.Utilities
{
	public static class GameObjects : System.Object
	{
		static Dictionary<string, List<GameObject>> gameObjectsLocalName;	//Short name of game object
		static Dictionary<string, List<GameObject>> gameObjectsFullName;	//Full path name of game object. object1/object2/object3...
		static Dictionary<string, List<GameObject>> gameObjectsByTag;		//Tag that represents the game object
		static Dictionary<int, List<GameObject>> gameObjectsById;			//Id of the game object
		
		/// <summary>
		/// Initializes the <see cref="NightPen.Utilities.GameObjects"/> class.
		/// </summary>
		static GameObjects()
		{
			Initialize();
		}
		
		/// <summary>
		/// Gets the full path name of the game object.
		/// </summary>
		/// <returns>The full path name.</returns>
		/// <param name="go">Go.</param>
		static string GetFullPathName(GameObject go)
		{
			if ( go.transform.parent == null )
			{
				return go.name;
			}
			return GetFullPathName(go.transform.parent.gameObject) + "/" + go.name;
		}
		
		/// <summary>
		/// Initializes the <see cref="NightPen.GameObjects.GameObjects"/> class by
		/// scraping the scene for game objects and adding them to the indexed lists.
		/// This allows game objects to be retrieved even if inactive. System game
		/// objects and prefabs can also be returned by the functions in this class
		/// so you should only query for objects that are part of your game.
		/// </summary>
		public static void Initialize()
		{
			gameObjectsLocalName = new Dictionary<string, List<GameObject>>();
			gameObjectsFullName = new Dictionary<string, List<GameObject>>();
			gameObjectsByTag = new Dictionary<string, List<GameObject>>();
			gameObjectsById = new Dictionary<int, List<GameObject>>();
			
			GameObject[] gos = Resources.FindObjectsOfTypeAll<GameObject>();
			
			foreach(GameObject go in gos)
			{
				string fullName = GetFullPathName(go);
				if ( gameObjectsFullName.ContainsKey(fullName) == false )
				{
					gameObjectsFullName.Add(fullName, new List<GameObject>());
				}
				gameObjectsFullName[fullName].Add(go);
				
				if ( gameObjectsLocalName.ContainsKey(go.name) == false )
				{
					gameObjectsLocalName.Add(go.name, new List<GameObject>());
				}
				gameObjectsLocalName[go.name].Add(go);
				
				if ( gameObjectsByTag.ContainsKey(go.tag) == false )
				{
					gameObjectsByTag.Add(go.tag, new List<GameObject>());
				}
				
				gameObjectsByTag[go.tag].Add(go);
				
				if ( gameObjectsById.ContainsKey(go.GetInstanceID()) == false )
				{
					gameObjectsById.Add(go.GetInstanceID(), new List<GameObject>());
				}
				gameObjectsById[go.GetInstanceID()].Add(go);
			}
		}
		
		/// <summary>
		/// Gets a list of game objects with the specified name in the
		/// current scene.
		/// </summary>
		/// <returns>A list of game objects with the specified name
		/// in the current scene if any are found. If none exist then
		/// an empty list is returned.
		/// </returns>
		/// <param name="name">The name of the game objects to search for. Slashes
		/// separate path elements so parent/child objects can be found.
		///</param>
		public static List<GameObject> GetByName(string name)
		{
			List<GameObject> gameObjectsList;
			
			if ( gameObjectsLocalName.ContainsKey(name) )
			{
				gameObjectsList = gameObjectsLocalName[name];
			}
			else
			{
				if ( gameObjectsFullName.ContainsKey(name) )
				{
					gameObjectsList = gameObjectsFullName[name];
				}
				else
				{
					gameObjectsList = new List<GameObject>();
				}
			}
			
			return gameObjectsList;
		}
		
		/// <summary>
		/// Gets a list of game objects with the specified tag in the
		/// current scene.
		/// </summary>
		/// <returns>A list of game objects with the specified tag
		/// in the current scene if any are found. If none exist then
		/// an empty list is returned.
		/// </returns>
		/// <param name="tag">The tag of the game objects to search for.</param>
		public static List<GameObject> GetByTag(string tag)
		{
			List<GameObject> gameObjectsList;
			
			if ( gameObjectsByTag.ContainsKey(tag) )
			{
				gameObjectsList = gameObjectsByTag[tag];
			}
			else
			{
				gameObjectsList = new List<GameObject>();
			}
			
			return gameObjectsList;
		}
		
		/// <summary>
		/// Gets a list of game objects with the specified instance id
		/// in the current scene.
		/// </summary>
		/// <returns>A list of game objects with the specified tag
		/// in the current scene if any are found. If none exist then
		/// an empty list is returned.
		/// </returns>
		/// <param name="id">The instance id of the game objects to search for.</param>
		public static List<GameObject> GetByID(int id)
		{
			List<GameObject> gameObjectsList;
			
			if ( gameObjectsById.ContainsKey(id) )
			{
				gameObjectsList = gameObjectsById[id];
			}
			else
			{
				gameObjectsList = new List<GameObject>();
			}
			
			return gameObjectsList;
		}
		
		/// <summary>
		/// Gets the first game object  with the specified name in
		/// the current scene.
		/// </summary>
		/// <returns>The first game object in the scene with the
		/// specified name or null if no game objects with the
		/// specified name exist in the current scene.
		/// </returns>
		/// <param name="name">The name of the game object to search for. Slashes
		/// separate path elements so parent/child objects can be found.
		///</param>
		public static GameObject GetFirstOrDefaultByName(string name)
		{
			GameObject go = null;
			
			if ( gameObjectsLocalName.ContainsKey(name) )
			{
				go = gameObjectsLocalName[name][0];
			}
			else if ( gameObjectsFullName.ContainsKey(name) )
			{
				go = gameObjectsFullName[name][0];
			}
			
			return go;
		}
		
		/// <summary>
		/// Gets the first game object  with the specified tag in
		/// the current scene.
		/// </summary>
		/// <returns>The first game object in the scene with the
		/// specified tag or null if no game objects with the
		/// specified tag exist in the current scene.
		/// </returns>
		/// <param name="name">The tag of the game object to search for.</param>
		public static GameObject GetFirstOrDefaultByTag(string tag)
		{
			GameObject go = null;
			
			if ( gameObjectsByTag.ContainsKey(tag) )
			{
				go = gameObjectsByTag[tag][0];
			}
			
			return go;
		}
		
		/// <summary>
		/// Gets the first game object  with the specified
		/// instance id the current scene.
		/// </summary>
		/// <returns>The first game object in the scene with the
		/// specified instance id or null if no game objects with the
		/// specified instance id exist in the current scene.
		/// </returns>
		/// <param name="name">The tag of the game object to search for.</param>
		public static GameObject GetFirstOrDefaultByInstanceID(int id)
		{
			GameObject go = null;
			
			if ( gameObjectsById.ContainsKey(id) )
			{
				go = gameObjectsById[id][0];
			}
			
			return go;
		}
		
		/// <summary>
		/// Same as GetFirstOrDefault() but with a much shorter name.
		/// </summary>
		/// <param name="name">The name of the game objects to search for. Slashes
		/// separate path elements so parent/child objects can be found.
		///</param>
		public static GameObject Get(string name)
		{
			return GetFirstOrDefaultByName(name);
		}
		
		/// <summary>
		/// Get a component attached to a game object.
		/// </summary>
		/// <returns>The component if it is attached to the game object or null if the component does not exist.</returns>
		/// <param name="name">Name.</param>
		/// <typeparam name="T">The component type to get.</typeparam>
		/// <example>
		///
		/// //This example sets the text of a Text component that is attached
		/// //to a game object called MyGameObject. Note: This works even if
		/// //MyGameObject is inactive!
		/// GameObjects.GetComponent<Text>("MyGameObject").text = "my new text";
		/// </example>
		public static T GetComponent<T>(string name)
		{
			object o = null;
			
			GameObject go = GameObjects.Get(name);
			if ( go != null )
			{
				string s = typeof(T).ToString();
				o = go.GetComponent(s);
			}
			
			return (T)o;
		}
		
		/// <summary>
		/// Sets the active inactive state of all game objects found with the specified name.
		/// </summary>
		/// <param name="name">Name or full path name of the GameObjects activity state to set.</param>
		/// <param name="state">If set to <c>true</c> the game object is active, otherwise if false the game object is set inactive.</param>
		public static void SetActive(string name, bool state)
		{
			List<GameObject> gos = GetByName(name);
			
			gos.ForEach(delegate(GameObject go) { go.SetActive(state); });
		}
	}
}