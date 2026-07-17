using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class YesNoUI : MonoBehaviour
{
    private Action<bool> responseCallback;

    [Header("UI References")]
    public GameObject yesNoBox;
    public Button yesButton;
    public Button noButton;
    public TextMeshProUGUI questionText;

    private void Awake()
    {
        if (!yesNoBox || !yesButton || !noButton || !questionText)
        {
            Debug.LogError("YesNoUI references not assigned!");
            enabled = false;
            return;
        }

        // Wire buttons automatically
        yesButton.onClick.AddListener(PressYes);
        noButton.onClick.AddListener(PressNo);

        yesNoBox.SetActive(false); // start hidden
    }

    void Update()
    {
        if (yesNoBox == null)
        {
            yesNoBox = GameObject.Find("YesNoBox");
        }
    }

    public void ShowYesNo(string question, Action<bool> callback)
    {
        responseCallback = callback;
        questionText.text = question;
        yesNoBox.SetActive(true);
        yesNoBox.transform.SetAsLastSibling();
    }

    private void PressYes()
    {
        yesNoBox.SetActive(false);
        responseCallback?.Invoke(true);
        responseCallback = null; // clear after invoking
    }

    private void PressNo()
    {
        yesNoBox.SetActive(false);
        responseCallback?.Invoke(false);
        responseCallback = null; // clear after invoking
    }

}