using UnityEngine;
using UnityEngine.UI;

public class ToggleIndicator : MonoBehaviour
{
    Color Off = new Color(197, 0, 0);
    Color On = new Color(0, 165, 0);

    // Red/Green light on levers
    public void Start()
    {
        gameObject.GetComponent<Renderer>().material.color = Off;
    }
    public void Evaluate(bool boolean)
    {
        if (boolean)
        {
            gameObject.GetComponent<Renderer>().material.color = On;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = Off;
        }
    }
}
