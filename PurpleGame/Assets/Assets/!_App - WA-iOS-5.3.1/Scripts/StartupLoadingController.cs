

using System;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartupLoadingController : MonoBehaviour
{
#if UNITY_EDITOR
    public string Version = "WA-iOS-5.3.1";
#endif

    [SerializeField] private bool _rotateToLanscapeMode = false;
    
    private static StartupLoadingController _instance;

    // ! - DO NOT CHANGE - !
    private async void Awake () 
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);

#if UNITY_EDITOR
        await LoadEditorApp();
#endif
        
        //PluginNavigationController.NavigationFinished += async () => 
        Startup();
    }

    // ! - CAN BE CHANGED - !
    private void Startup()
    {
        SetupAppOrientation();

        this.gameObject.SetActive(false);
        
        // ! - ANY ADDITIONAL LOGIC ADD HERE...
        
        // TODO 1. Enable background music here
    }

    private void SetupAppOrientation()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
        if (!_rotateToLanscapeMode)
        {
            SetupPortraitOrientation();
        }
        else
        {
            SetupLandscapeOrientation();
        }
    }

    private void SetupPortraitOrientation()
    {
#if UNITY_ANDROID
        Screen.orientation = ScreenOrientation.Portrait;
        return;
#endif
        
        Screen.autorotateToPortrait = true;
        
        Screen.autorotateToLandscapeLeft
            = Screen.autorotateToLandscapeRight
                = Screen.autorotateToPortraitUpsideDown = false;
    }
    
    private void SetupLandscapeOrientation()
    {
#if UNITY_ANDROID
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        return;
#endif
        
        Screen.autorotateToLandscapeLeft = true;
        
        Screen.autorotateToPortrait
            = Screen.autorotateToPortraitUpsideDown
                = Screen.autorotateToLandscapeRight = false;
    }


#if UNITY_EDITOR
    private async Task LoadEditorApp()
    {
        var loadingCanvas = Resources.Load("LoadingCanvas" + " - " + Version) as GameObject;
        if (loadingCanvas != null)
        {
            Debug.LogError("Resources - Loading Canvas prefab must be Removed from project after placed in Scene");
        }
        
        var appManager = Resources.Load("AppManager" + " - " + Version) as GameObject;
        if (appManager != null)
        {
            Debug.LogError("Resources - App Manager prefab must be Removed from project after placed in Scene");
        }
        
        var transforms = this.gameObject.GetComponentsInChildren<Transform>(true);;
        CheckGraphicsSetup(transforms);

        try
        {
            var loadingView = transforms.First(tr => tr.gameObject.name == "LoadingView").gameObject;
            var notificationsView = transforms.First(tr => tr.gameObject.name == "NotificationsView").gameObject;
            var privacyPolicyView =  transforms.First(tr => tr.gameObject.name == "PrivacyPolicyView").gameObject;
        
            var allowNotificationsButton =  transforms.First(tr => tr.gameObject.name == "AllowNotifications").GetComponent<Button>();
            var laterNotificationsButton =   transforms.First(tr => tr.gameObject.name == "LaterNotifications").GetComponent<Button>();
            var readPrivacyButton =  transforms.First(tr => tr.gameObject.name == "ReadPrivacy").GetComponent<Button>();
            var agreePrivacyButton =   transforms.First(tr => tr.gameObject.name == "AgreePrivacy").GetComponent<Button>();
        
            var loadingText = transforms.First(tr => tr.gameObject.name == "LoadingText").GetComponent<TextMeshProUGUI>();
            var loadingMask = transforms.First(tr => tr.gameObject.name == "LoadingMask").GetComponent<RectMask2D>();

            await ShowPrivacyPolicy(privacyPolicyView, readPrivacyButton, agreePrivacyButton);
            await ShowLoadingView(loadingView, loadingText, loadingMask);
            await ShowNotificationsView(notificationsView, allowNotificationsButton, laterNotificationsButton);
        }
        catch (Exception e)
        {
            Debug.LogError("Loading Canvas - invalid structure, please update with the last App plugin version for this project Type.");
            throw new UnityException();
        }
        
    }

    
    private void CheckGraphicsSetup(Transform[] transforms)
    {
        

        // ! - Checking Loading View
        try
        {
            var logoImage = transforms.First(tr => tr.gameObject.name == "Logo").GetComponent<Image>();
            if (logoImage.sprite == null)
            {
                Debug.LogError("Loading Canvas - Loading View - has no setup graphics.");
            }
            
            var loadingMask = transforms.First(tr => tr.gameObject.name == "LoadingMask").GetComponent<RectMask2D>();
        }
        catch (Exception e)
        {
            Debug.LogError("Loading Canvas - Loading View - invalid structure!");
        }

        // ! - Notifications View
        try
        {
            var allowNotificationImage = transforms.First(tr => tr.gameObject.name == "AllowNotifications").GetComponent<Image>();
            if (allowNotificationImage.sprite == null)
            {
                Debug.LogError("Loading Canvas - Notification View - has no setup graphics.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Loading Canvas - Notification View - invalid structure!"); 
        }
        
        // ! - Privacy Policy View
        try
        {
            var readPrivacyImage = transforms.First(tr => tr.gameObject.name == "ReadPrivacy").GetComponent<Image>();
            if (readPrivacyImage.sprite == null)
            {
                Debug.LogError("Loading Canvas - Privacy Policy View - has no setup graphics.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Loading Canvas - Privacy Policy View - invalid structure!"); 
        }

        // ! - Nav Background
        try
        {
            var navBackground =  transforms.First(tr => tr.gameObject.name == "NavBackground").GetComponent<Image>();
            if (navBackground.sprite != null)
            {
                Debug.LogError("Loading Canvas - Nav Background - must be Black!");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Loading Canvas - Nav Background - invalid structure!"); 
        }
    }

    private async Task ShowPrivacyPolicy(GameObject privacyPolicyView, Button readPrivacyButton, Button agreePrivacyButton)
    {
        privacyPolicyView.SetActive(true);
        var privacyShowedCompletionSource = new TaskCompletionSource<bool>();
        
        readPrivacyButton.onClick.AddListener(() => Application.OpenURL(AppManager.Instance.AppSettings.PrivacyPolicyURL));
        agreePrivacyButton.onClick.AddListener(() => privacyShowedCompletionSource.TrySetResult(true));

        await privacyShowedCompletionSource.Task;
        privacyPolicyView.SetActive(false);
    }
    
    private async Task ShowLoadingView(GameObject loadingView, TextMeshProUGUI loadingText, RectMask2D loadingMask)
    {
        loadingView.SetActive(true);

        loadingText.text = "Loading in Progress";
        
        var startPadding = loadingMask.padding.z;
        var loadingPercent = 0;
        while (loadingPercent < 35)
        {
            loadingMask.padding = new Vector4(
                loadingMask.padding.x,
                loadingMask.padding.y,
                startPadding * (1 - loadingPercent++ / 100.0f),
                loadingMask.padding.w);
            await Task.Delay(150);
        }


        loadingView.SetActive(false);
    }
    
    private async Task ShowNotificationsView(GameObject notificationsView, Button allowNotificationsButton, Button laterNotificationsButton)
    {
        notificationsView.SetActive(true);
        
        var notificationsShowedCompletionSource = new TaskCompletionSource<bool>();
        
        allowNotificationsButton.onClick.AddListener(() => notificationsShowedCompletionSource.TrySetResult(true));
        laterNotificationsButton.onClick.AddListener(() => notificationsShowedCompletionSource.TrySetResult(true));

        await notificationsShowedCompletionSource.Task;
        notificationsView.SetActive(false);
    }

    
#endif
}


