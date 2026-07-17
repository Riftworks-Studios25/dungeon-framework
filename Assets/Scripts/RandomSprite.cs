using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomSprite : MonoBehaviour
{
    private SpriteRenderer sr;
    private Image img;
    [SerializeField] public List<Sprite> sprites;
    void Start()
    {
        // Set a random sprite for the object from a list of serialized sprites
        sr = GetComponent<SpriteRenderer>();
        img = GetComponent<Image>();

        Sprite newSprite = sprites[Random.Range(0, sprites.Count)];
        sr.sprite = newSprite;
        img.sprite = newSprite;
    }
}
