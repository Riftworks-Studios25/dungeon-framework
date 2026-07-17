using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnlockerBehavior : MonoBehaviour
{
    public GameObject triggerableObject;
    public Unlocker unlocker;
    public List<GameObject> unlockerObjects;
    private List<Unlocker> unlockers;
    [SerializeField] public GameObject mainUnlocker;
    public bool unlocked = false;
    public GameObject toggleLight;
    public AudioSource audioSource;
    public void Play()
    {
        audioSource.volume = Random.Range(0.5f, 1.0f);

        audioSource.pitch = Random.Range(0.8f, 1.2f);

        // Play the audio

        audioSource.Play();
    }
    void Awake()
    {
        unlocker = Instantiate(unlocker);
        unlockers = new List<Unlocker>();
    }
    void Start()
    {
        foreach (GameObject unl in unlockerObjects)
        {
            UnlockerBehavior unlocker1 = unl.GetComponent<UnlockerBehavior>();
            if (unlocker1 != null)
            {
                unlockers.Add(unlocker1.unlocker);
            }
        }
    } 
    

    public void Unlock()
    {
        unlocked = unlockers.All(u => u.IsActive());

        TriggerableBehavior triggerable = triggerableObject.GetComponent<TriggerableBehavior>();
        if (triggerable != null)
        {
            if (unlocked && triggerable.IsLocked())
                triggerable.Unlock();
            else if (!unlocked && !triggerable.IsLocked())
                triggerable.Lock();
            Play();
        }
    }
}
