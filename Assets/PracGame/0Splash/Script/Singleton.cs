using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	public static T Instance;
	public static void Load(GameObject SingletonPrefab)
	{
		if (Instance == null)
		{
			GameObject NewGameObject = Instantiate(SingletonPrefab);
			Instance = NewGameObject.GetComponent<T>();
			DontDestroyOnLoad(NewGameObject);
		}
	}
}
