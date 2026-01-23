using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GunExplosion : MonoBehaviour
{
    public Image image;
    public Sprite[] frames;
    public float flashDuration = 0.03f;

    void Awake()
    {
        image.enabled = false;
    }

    public void Play()
    {
        StopAllCoroutines();
        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        image.enabled = true;

        for (int i = 0; i < frames.Length; i++)
        {
            image.sprite = frames[i];
            yield return new WaitForSeconds(flashDuration);
        }

        image.enabled = false;
    }
}

