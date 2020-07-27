using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Editor
{
	public class Remover2000 : EditorWindow
	{
		[MenuItem ("Window/Remover2000")]
		public static void  ShowWindow () 
		{
			GetWindow(typeof(Remover2000));
		}

		private void OnGUI () 
		{
			if (GUILayout.Button("DELETE ALL MESH COLLIDER")) RemoveComponenets<MeshCollider>();
			if (GUILayout.Button("DELETE ALL MESH RENDERER")) RemoveComponenets<MeshRenderer>();

			if (GUILayout.Button("SAVE THE MESS")) SaveScene();
		}

		private static void SaveScene()
		{
			var scene = SceneManager.GetActiveScene();
			EditorSceneManager.SaveScene(scene);
		}

		private static void RemoveComponenets<T>() where T : Component
		{
			var currentGameObject = Selection.activeGameObject;

			if (currentGameObject != null)
			{
				RemoveComponent<T>(currentGameObject);
			}
			else
			{
				var scene = SceneManager.GetActiveScene();
				var rootObjectsInScene = new List<GameObject>();
				scene.GetRootGameObjects(rootObjectsInScene);
				foreach (var rootGameObject in rootObjectsInScene)
				{
					RemoveComponent<T>(rootGameObject);
				}
			}
		}

		private static void RemoveComponent<T>(GameObject gameObject)
		{
			var components = gameObject.GetComponentsInChildren(typeof (T), true);

			foreach (var c in components) 
			{
				DestroyImmediate(c);
			}
		}
	}
}
