using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ObjectFlash : MonoBehaviour
{
    [Header("Color, duration")]
    [SerializeField]
    Color toColor = Color.white;
    [SerializeField]
    float duration = 0.05f;

    Renderer rend;
    Color defaultColor;

    bool inFlash = false;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        defaultColor = rend.material.color;
    }

    public Coroutine Flash()
    {
        if (inFlash) return null;

        return StartCoroutine(_Flash());
    }

    IEnumerator _Flash()
    {
        inFlash = true;
        rend.material.color = toColor;

        yield return new WaitForSeconds(duration);

        rend.material.color = defaultColor;
        inFlash = false;
    }
}
