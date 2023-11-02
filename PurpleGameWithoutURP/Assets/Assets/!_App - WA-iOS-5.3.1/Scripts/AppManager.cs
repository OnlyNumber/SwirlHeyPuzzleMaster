using UnityEngine;

public class AppManager : MonoBehaviour
{
#if UNITY_EDITOR
    public string Version = "WA-iOS-5.3.1";
#endif
    
    [SerializeField] private AppSettings _appSettings;

    public AppSettings AppSettings => _appSettings;

    public static AppManager Instance => _instance;

    private static AppManager _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
