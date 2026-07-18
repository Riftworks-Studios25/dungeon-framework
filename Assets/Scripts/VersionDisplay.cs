using UnityEngine;
using TMPro;

public class VersionDisplay : MonoBehaviour
{
    private TMP_Text versionText;

    void Start()
    {
        versionText = GetComponent<TMP_Text>();
        
        if (versionText != null)
        {
            string baseVersion = "v" + Application.version;

            if (Debug.isDebugBuild || Application.isEditor)
            {
                versionText.text = $"{baseVersion}-dev";
            }
            else
            {
                versionText.text = baseVersion;
            }
        }
    }
}