using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Settings/App Settings", fileName = "AppSettings")]
public class AppSettings : ScriptableObject
{
    [Header("Ads")]
    public string IronSourceAppKey;
    
    [Header("URLs:")]
    public string PrivacyPolicyURL;
}
