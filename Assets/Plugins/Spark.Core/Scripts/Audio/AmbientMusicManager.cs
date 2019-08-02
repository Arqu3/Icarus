using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Originally the Sapling music engine
/// </summary>
public class AmbientMusicManager : MonoBehaviour
{

	public AudioSource musicSource;
	public AudioClip [] songs;

	public AudioMixer masterMix;

	public AudioMixerSnapshot mainSnapshot, musicSnapshot;

	private int currentMusic = 0;
	private int newMusic = 0;

	[SerializeField]
	[Range( 0, 1 )]
	public float musicVolume;

	[SerializeField]
	private float minTime, maxTime;


	// Use this for initialization
	void Start()
	{
		StartCoroutine( WaitForMusic() );
		musicSource.volume = musicVolume;
		newMusic = 0;
	}

	// Update is called once per frame
	void Update()
	{

	}

	void PlayMusic()
	{
		if ( songs != null && musicSource != null )
		{

			newMusic = Random.Range( 0, songs.Length );

			if ( newMusic != currentMusic || songs.Length < 2 )
			{
				musicSource.clip = songs [ newMusic ];
				currentMusic = newMusic;
				musicSource.Play();

				if ( musicSource.volume != 0 )
				{
					musicSnapshot.TransitionTo( 5 );
				}


				StartCoroutine( WaitForMusic() );
			}

			else
				PlayMusic();

		}
	}


	public void MuteMusic()
	{
		Debug.Log( "Click mute music button." );
		if ( musicSource.volume == 0 )
		{
			musicSource.volume = musicVolume;
			Debug.Log( "Music unmute, it should be " + musicVolume + " and is " + musicSource.volume + "." );
		}
		else if ( musicSource.volume != 0 )
		{

			musicSource.volume = 0;
			Debug.Log( "Music mute, it should be " + 0 + " and is " + musicSource.volume + "." );
		}
	}




	IEnumerator WaitForMusic()
	{

		if ( musicSource.clip != null )
		{
			yield return new WaitForSecondsRealtime( musicSource.clip.length );
			mainSnapshot.TransitionTo( 5 );
			yield return new WaitForSecondsRealtime( Random.Range( minTime, maxTime ) );

		}

		else
		{
			yield return new WaitForSecondsRealtime( Random.Range( minTime, maxTime ) );
		}

		PlayMusic();

		yield return null;
	}
}