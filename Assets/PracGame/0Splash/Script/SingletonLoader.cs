using UnityEngine;

public class SingletonLoader : MonoBehaviour
{
	public GameObject googlesheetManagerPrefab;
	public GameObject translationManagerPrefab;
	void Awake()
	{
		GoogleSheetManager.Load(googlesheetManagerPrefab);
		TranslationManager.Load(translationManagerPrefab);
	}
}
