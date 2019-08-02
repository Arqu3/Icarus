using UnityEngine;
using System.Collections;

/// <summary>
/// Originally the Passpartout music engine
/// </summary>
public class JukeboxMusicManager : MonoBehaviour
{

	public AudioSource MusicSource;
	public AudioSource IntroSource;

	public float MusicVolume;
	public float FadeSpeed;

	public static JukeboxMusicManager instance
	{
		get; private set;
	}

	private bool musicStopping = false;
	private bool playingMusicWithIntro = false;

	private IEnumerator MusicWithIntro;

	// Use this for initialization
	void Awake()
	{
		instance = this;

	}

	public void PlayMusic( AudioClip song, float volume )
	{

		StartCoroutine( _PlayMusic( song, volume ) );
		playingMusicWithIntro = false;

	}



	private IEnumerator _PlayMusic( AudioClip song, float volume )
	{
		Debug.Log( "MusicManager: Trying to play " + song.name + " without intro" );
		if ( musicStopping == true )
			Debug.Log( "MusicManager: Waiting for music to stop" );
		while ( musicStopping == true )
		{
			yield return null;
		}

		if ( song != null )
		{
			while ( MusicSource.volume > 0f )
			{

				MusicSource.volume = Mathf.MoveTowards( MusicSource.volume, 0f, FadeSpeed * Time.deltaTime );//MusicSource.volume - FadeSpeed * Time.deltaTime;
				IntroSource.volume = Mathf.MoveTowards( IntroSource.volume, 0f, FadeSpeed * Time.deltaTime );//MusicSource.volume - FadeSpeed * Time.deltaTime;
				yield return null;

			}
			StartMusic( song, volume );
			Debug.Log( "MusicManager: Started " + song.name + " without intro" );
		}

		else
		{
			Debug.LogWarning( "MusicManager: Tried to play music but there is no song assigned to the starter" );
		}
	}

	void StartMusic( AudioClip song, float volume )
	{
		MusicSource.Stop();
		MusicSource.clip = song;
		MusicSource.volume = volume;
		MusicSource.Play();
	}


	public void PlayMusicWithIntro( AudioClip [] intro, AudioClip song, float volume )
	{

		StopCoroutine( MusicWithIntro );
		MusicWithIntro = _PlayMusicWithIntro( intro, song, volume );
		StartCoroutine( MusicWithIntro );

		playingMusicWithIntro = true;
	}

	private IEnumerator _PlayMusicWithIntro( AudioClip [] intro, AudioClip song, float volume )
	{
		Debug.Log( "MusicManager: Trying to play " + song.name + " with intro" );

		if ( musicStopping == true )
			Debug.Log( "MusicManager: Waiting for music to stop" );
		while ( musicStopping == true )
		{
			yield return null;
		}

		if ( song != null && intro != null )
		{
			while ( MusicSource.volume > 0f )
			{

				MusicSource.volume = Mathf.MoveTowards( MusicSource.volume, 0f, FadeSpeed * Time.deltaTime );//MusicSource.volume - FadeSpeed * Time.deltaTime;
				IntroSource.volume = Mathf.MoveTowards( IntroSource.volume, 0f, FadeSpeed * Time.deltaTime );//MusicSource.volume - FadeSpeed * Time.deltaTime;
				yield return null;


			}


			if ( MusicSource != null )
				MusicSource.Stop();
			if ( IntroSource != null )
				IntroSource.Stop();

			Debug.Log( "MusicManager: Made sure no music is playing" );
			MusicSource.clip = song;
			IntroSource.clip = intro [ UnityEngine.Random.Range( 0, intro.Length ) ];
			MusicSource.volume = volume;
			IntroSource.volume = volume;
			AudioClip SelectedIntro = IntroSource.clip;
			float WaitTime = SelectedIntro.length;
			IntroSource.clip.LoadAudioData();
			MusicSource.clip.LoadAudioData();
			yield return null;
			IntroSource.Play();

			yield return new WaitForSeconds( IntroSource.clip.length );
			//MusicSource.Play();
			//IntroSource.Stop();
			if ( playingMusicWithIntro == true )
			{
				StartMusic( song, volume );
				Debug.Log( "MusicManager: Playing " + song.name + " after intro" );
			}

			if ( playingMusicWithIntro == false )
			{
				Debug.Log( "MusicManager: Prevented music after intro from playing" );
			}


		}

		else
		{
			Debug.LogWarning( "MusicManager: Tried to play music with intro but there was no music assigned" );
		}

	}

	public void StopMusic()
	{
		StartCoroutine( _StopMusic() );
	}

	private IEnumerator _StopMusic()
	{
		musicStopping = true;
		Debug.Log( "MusicManager: Stopping music" );

		while ( MusicSource.volume > 0f )
		{

			MusicSource.volume = Mathf.MoveTowards( MusicSource.volume, 0f, FadeSpeed * Time.deltaTime );//MusicSource.volume - FadeSpeed * Time.deltaTime;
			IntroSource.volume = Mathf.MoveTowards( IntroSource.volume, 0f, FadeSpeed * Time.deltaTime );//MusicSource.volume - FadeSpeed * Time.deltaTime;
			yield return null;

		}
		MusicSource.volume = 0f;
		MusicSource.Stop();
		Debug.Log( "MusicManager: Music stopped" );
		musicStopping = false;

	}
}