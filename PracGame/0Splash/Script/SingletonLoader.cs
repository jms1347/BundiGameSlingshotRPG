using UnityEngine;

public class SingletonLoader : MonoBehaviour
{
	public GameObject googlesheetManagerPrefab;
	public GameObject translationManagerPrefab;
	public GameObject soundManagerPrefab;
	public GameObject fadeManagerPrefab;
	public GameObject popupManagerPrefab;
	void Awake()
	{
		GoogleSheetManager.Load(googlesheetManagerPrefab);
		TranslationManager.Load(translationManagerPrefab);
		SoundManager.Load(soundManagerPrefab);
		FadeManager.Load(fadeManagerPrefab);
		PopupManager.Load(popupManagerPrefab);
	}
}
