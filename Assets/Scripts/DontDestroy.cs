using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    public string uniqueTagOrName;


    void Awake()
    {
        string id = string.IsNullOrEmpty(uniqueTagOrName) ? gameObject.name : uniqueTagOrName;

        // Check if another instance of me already exists when a scene is loaded, if so destroy me
        DontDestroy[] existingObjects = FindObjectsOfType<DontDestroy>(includeInactive: true);
        foreach (var obj in existingObjects)
        {
            string objId = string.IsNullOrEmpty(obj.uniqueTagOrName) ? obj.gameObject.name : obj.uniqueTagOrName;
            if (objId == id && obj != this)
            {
                Destroy(gameObject);
                return;
            }
        }

        // Make persistent
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (transform != null)
        {
            transform.position = Vector3.zero;
        }
    }
}
