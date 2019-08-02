using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public abstract class CameraShakeLayer : MonoBehaviour
{
    [Header("Curves")]
    [SerializeField]
    protected bool useX = true;
    [ConditionalField(nameof(useX))]
    [SerializeField]
    protected AnimationCurve xCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1.0f, 0f));

    [SerializeField]
    protected bool useY = true;
    [ConditionalField(nameof(useY))]
    [SerializeField]
    protected AnimationCurve yCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1.0f, 0f));

    [SerializeField]
    protected bool useZ = true;
    [ConditionalField(nameof(useZ))]
    [SerializeField]
    protected AnimationCurve zCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1.0f, 0f));

    public float Duration
    {
        get
        {
            return Mathf.Max(useX ? xCurve.keys[xCurve.length - 1].time : 0f, useY ? yCurve.keys[yCurve.length - 1].time : 0, useZ ? zCurve.keys[zCurve.length - 1].time : 0);
        }
    }

    protected Vector3 localStartpos;
    protected Quaternion localStartRot;

    protected readonly UnityEvent onStart = new UnityEvent();
    protected readonly GenericUnityEvent<float, float, float> onLoop = new GenericUnityEvent<float, float, float>();
    protected readonly UnityEvent onReset = new UnityEvent();
    protected readonly UnityEvent onStopLoop = new UnityEvent();
    protected readonly UnityEvent onStartLoop = new UnityEvent();

    private bool playing = false;
    private bool playingLoop = false;

    protected virtual void Awake()
    {
        localStartpos = transform.localPosition;
        localStartRot = transform.localRotation;
    }

    public virtual Coroutine Play(float duration)
    {
        return StartCoroutine(_Play(duration));
    }

#if UNITY_EDITOR

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)) Play();
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            if (playingLoop) StopLoop();
            else PlayLoop();
        }
    }

#endif

    public virtual Coroutine Play()
    {
        return Play(Duration);
    }

    protected IEnumerator _Play(float duration)
    {
        if (playing) yield break;

        playing = true;

        float timer = 0.0f;
        onStart.Invoke();

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float percentage = timer / duration;
            onLoop.Invoke(
                useX ? xCurve.Evaluate(xCurve.keys[xCurve.length - 1].time * percentage) : 0f,
                useY ? yCurve.Evaluate(yCurve.keys[yCurve.length - 1].time * percentage) : 0f,
                useZ ? zCurve.Evaluate(zCurve.keys[zCurve.length - 1].time * percentage) : 0f);

            yield return null;
        }

        //float resetTime = 0.1f;
        //float resetTimer = 0.0f;

        //Vector3 resetStartpos = transform.localPosition;
        //Quaternion resetStartrot = transform.localRotation;

        //while (resetTimer < resetTime)
        //{
        //    resetTimer += Time.deltaTime;

        //    transform.localPosition = Vector3.Lerp(resetStartpos, localStartpos, resetTimer / resetTime);
        //    transform.localRotation = Quaternion.Lerp(resetStartrot, localStartRot, resetTimer / resetTime);

        //    yield return new WaitForEndOfFrame();
        //}

        ResetPositionAndRotation();
        playing = false;
    }

    public Coroutine PlayLoop()
    {
        return StartCoroutine(_PlayLoop());
    }

    IEnumerator _PlayLoop()
    {
        if (playingLoop) yield break;

        onStartLoop.Invoke();
        playingLoop = true;

        while (true)
        {
            float duration = Duration;
            Play(duration);
            yield return new WaitForSeconds(duration);
        }
    }

    public void StopLoop()
    {
        StopAllCoroutines();
        ResetPositionAndRotation();
        playing = false;
        playingLoop = false;
        onStopLoop.Invoke();
    }

    protected virtual void ResetPositionAndRotation()
    {
        transform.localPosition = localStartpos;
        transform.localRotation = localStartRot;
        onReset.Invoke();
    }
}
