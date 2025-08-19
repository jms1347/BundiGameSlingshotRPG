using UnityEngine;

public class SingletonLoader : MonoBehaviour
{
	public GameObject googlesheetManagerPrefab;
	public GameObject translationManagerPrefab;
	public GameObject soundManagerPrefab;
	public GameObject fadeManagerPrefab;
	public GameObject popupManagerPrefab;
	public GameObject commandInvokerPefab;
	public GameObject InputManagerPefab;
	void Awake()
	{
		GoogleSheetManager.Load(googlesheetManagerPrefab);
		TranslationManager.Load(translationManagerPrefab);
		SoundManager.Load(soundManagerPrefab);
		FadeManager.Load(fadeManagerPrefab);
		PopupManager.Load(popupManagerPrefab);
		CommandInvoker.Load(commandInvokerPefab);
		InputManager.Load(InputManagerPefab);
	}
}
