using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioExtensions {

	/// <summary>
	/// Plays the supplied clip. Will play an error sound instead if clip is null
	/// </summary>
	/// <param name="audioSource"></param>
	/// <param name="clip"></param>
	public static void PlayOneShotSafe(this AudioSource audioSource, AudioClip clip)
	{
#if DEBUG
		if (clip)
		{
			audioSource.PlayOneShot(clip);
		}
		else
		{
			Debug.LogWarning("Clip array empty");
			audioSource.PlayOneShot(Resources.Load<AudioClip>("Audio/MissingClip"));
		}
#else
		// Minimal footprint for release builds
		audioSource.PlayOneShot(clip);
#endif
	}

	/// <summary>
	/// Plays one of the supplied clips (at random). Will play an error sound instead if no clips are supplied
	/// </summary>
	/// <param name="audioSource"></param>
	/// <param name="clips"></param>
	public static void PlayOneShotSafeRandom(this AudioSource audioSource, params AudioClip[] clips)
	{

		if(clips != null && clips.Length > 0)
		{
			audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
		}
#if DEBUG
		else
		{
			// We don't want to play the missing clip sound in release builds
			Debug.LogWarning("Clip array empty");
			audioSource.PlayOneShot(Resources.Load<AudioClip>("Audio/MissingClip"));
		}
#endif
	}
}
