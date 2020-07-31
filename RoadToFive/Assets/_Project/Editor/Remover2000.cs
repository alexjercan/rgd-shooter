using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Editor
{
	public class Remover2000 : EditorWindow
	{
		[MenuItem("Window/Remover2000")]
		public static void ShowWindow()
		{
			GetWindow(typeof(Remover2000));
		}

		private void OnGUI()
		{
			if (GUILayout.Button("DELETE ALL MESH COLLIDER")) RemoveComponenets<MeshCollider>();
			if (GUILayout.Button("DELETE ALL MESH RENDERER")) RemoveComponenets<MeshRenderer>();
			if (GUILayout.Button("ADD ALL MESH COLLIDER")) AddComponents<MeshCollider>();



			if (GUILayout.Button("SAVE THE MESS")) SaveScene();
		}

		private static void SaveScene()
		{
			var scene = SceneManager.GetActiveScene();
			EditorSceneManager.SaveScene(scene);
		}

		private static void AddComponents<T>() where T : Component
		{
			var scene = SceneManager.GetActiveScene();
			var rootObjectsInScene = new List<GameObject>();
			scene.GetRootGameObjects(rootObjectsInScene);
			foreach (var rootGameObject in rootObjectsInScene)
			{
				AddComponent<T>(rootGameObject);
			}
		}

		private static void AddComponent<T>(GameObject gameObject) where T : Component
		{
			var gameObjects = gameObject.GetComponentsInChildren<Transform>();

			foreach (var c in gameObjects)
			{
				if (c.GetComponent<T>() == null) 
					c.gameObject.AddComponent<T>();
			}
		}
		

		private static void RemoveComponenets<T>() where T : Component
		{
			var scene = SceneManager.GetActiveScene();
			var rootObjectsInScene = new List<GameObject>();
			scene.GetRootGameObjects(rootObjectsInScene);
			foreach (var rootGameObject in rootObjectsInScene)
			{
				RemoveComponent<T>(rootGameObject);
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
