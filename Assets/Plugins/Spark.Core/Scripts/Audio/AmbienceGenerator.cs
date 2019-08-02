using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceGenerator : MonoBehaviour
{

	[SerializeField]
	private bool PlayOnAwake = false;
	public bool loop1Active, loop2Active, loop3Active;

	[Space( 10 )]

	[SerializeField]
	private AudioSource loop1Source;
	[SerializeField]
	private AudioSource loop2Source;
	[SerializeField]
	private AudioSource loop3Source;


	[Space( 10 )]

	[SerializeField]
	[Tooltip( "Constant ambience loops." )]
	private AudioClip loop1;
	[SerializeField]
	[Tooltip( "Constant ambience loops." )]
	private AudioClip loop2;
	[SerializeField]
	[Tooltip( "Constant ambience loops." )]
	private AudioClip loop3;

	[SerializeField]
	[Range( 0f, 1f )]
	private float loop1Volume = 1f;
	[SerializeField]
	[Range( 0f, 1f )]
	private float loop2Volume = 1f;
	[SerializeField]
	[Range( 0f, 1f )]
	private float loop3Volume = 1f;
	[SerializeField]
	[Tooltip( "Determines how quickly loops crossfade in distance moved per second." )]
	private float loopFadeSpeed = 0.1f;


	[Space( 20 )]


	[SerializeField]
	private AudioSource clips1Source;
	[SerializeField]
	private AudioSource clips2Source;
	[SerializeField]
	private AudioSource clips3Source;

	[Space( 10 )]

	[SerializeField]
	[Tooltip( "Clips to be played with each loop" )]
	private List<AudioClip> loop1Clips;
	[SerializeField]
	[Tooltip( "Clips to be played with each loop" )]
	private List<AudioClip> loop2Clips;
	[SerializeField]
	[Tooltip( "Clips to be played with each loop" )]
	private List<AudioClip> loop3Clips;

	[Space( 20 )]

	[SerializeField]
	[Tooltip( "Determines the intensity at which clips are played." )]
	[Range( 0f, 100f )]
	private int intensity = 1;

	[SerializeField]
	[Tooltip( "Determines the size of the simulated environment" )]
	[Range( 0f, 100f )]
	private int size = 1;

	[SerializeField]
	[Range( 0f, 1f )]
	private float clipMaxVolume = 1f;


	public List<AudioClip> currentClips;

	private float clipMinVolume;
	private float minTime, maxTime;
	private float loopCutOffVolume = 0.05f;

	private bool clips1Running = false, clips2Running = false, clips3Running = false;



	// Use this for initialization
	void Awake()
	{

	}

	private void Start()
	{
		loop1Source.clip = loop1;
		loop2Source.clip = loop2;
		loop3Source.clip = loop3;

		loop1Source.loop = true;
		loop2Source.loop = true;
		loop3Source.loop = true;

		loop1Source.volume = 0;
		loop2Source.volume = 0;
		loop3Source.volume = 0;

		loop1Source.Play();
		loop2Source.Play();
		loop3Source.Play();

		loop1Source.spatialBlend = 0f;
		loop2Source.spatialBlend = 0f;
		loop3Source.spatialBlend = 0f;
		clips1Source.spatialBlend = 0f;
		clips2Source.spatialBlend = 0f;
		clips3Source.spatialBlend = 0f;

		clipMinVolume = clipMaxVolume - ( size * ( clipMaxVolume / 100f ) );
		minTime = 100f - intensity;
		maxTime = minTime * 3f;
		
		if ( PlayOnAwake == true )
		{
			PlayAmbience();
		}
	}

	public void PlayAmbience()
	{
		StartCoroutine( LoopFadeStuff() );
		UpdateClips();
		StartPlayingClips();
	}

	public void StopAmbience()
	{
		StartCoroutine( LoopFadeStuff() );
		StopPlayingClips();
	}

	IEnumerator LoopFadeStuff()
	{
		bool loop1Correct = false;
		bool loop2Correct = false;
		bool loop3Correct = false;

		while ( loop1Correct == false || loop2Correct == false || loop3Correct == false )
		{
			//Fade loop 1
			if ( loop1Active == false && loop1Source.volume < loopCutOffVolume )
				loop1Source.volume = 0;
			else if ( loop1Active == false && loop1Source.volume >= loopCutOffVolume )
				loop1Source.volume = loop1Source.volume - loopFadeSpeed * Time.deltaTime;
			else if ( loop1Active == true && loop1Source.volume < loop1Volume )
				loop1Source.volume = loop1Source.volume + loopFadeSpeed * Time.deltaTime;

			//Fade loop 2
			if ( loop2Active == false && loop2Source.volume < loopCutOffVolume )
				loop2Source.volume = 0;
			else if ( loop2Active == false && loop2Source.volume >= loopCutOffVolume )
				loop2Source.volume = loop2Source.volume - loopFadeSpeed * Time.deltaTime;
			else if ( loop2Active == true && loop2Source.volume < loop2Volume )
				loop2Source.volume = loop2Source.volume + loopFadeSpeed * Time.deltaTime;

			//Fade loop 3
			if ( loop3Active == false && loop3Source.volume < loopCutOffVolume )
				loop3Source.volume = 0;
			else if ( loop3Active == false && loop3Source.volume >= loopCutOffVolume )
				loop3Source.volume = loop3Source.volume - loopFadeSpeed * Time.deltaTime;
			else if ( loop3Active == true && loop3Source.volume < loop3Volume )
				loop3Source.volume = loop3Source.volume + loopFadeSpeed * Time.deltaTime;


			//Check if loops are correct
			if ( loop1Active == true && loop1Source.volume >= loop1Volume )
				loop1Correct = true;
			else if ( loop1Active == false && loop1Source.volume <= 0 )
				loop1Correct = true;
			else
				loop1Correct = false;

			if ( loop2Active == true && loop2Source.volume >= loop2Volume )
				loop2Correct = true;
			else if ( loop2Active == false && loop2Source.volume <= 0 )
				loop2Correct = true;
			else
				loop2Correct = false;


			if ( loop3Active == true && loop3Source.volume >= loop3Volume )
				loop3Correct = true;
			else if ( loop3Active == false && loop3Source.volume <= 0 )
				loop3Correct = true;
			else
				loop3Correct = false;

			yield return null;
		}

		yield return null;
	}

	public void StartPlayingClips()
	{
		if ( clips1Running == false )
			StartCoroutine( Play1stClips() );
		if ( clips2Running == false )
			StartCoroutine( Play2ndClips() );
		if ( clips3Running == false )
			StartCoroutine( Play3rdClips() );
	}

	public void StopPlayingClips()
	{
		StopCoroutine( Play1stClips() );
		clips1Running = false;
		StopCoroutine( Play2ndClips() );
		clips2Running = false;
		StopCoroutine( Play3rdClips() );
		clips3Running = false;
	}

	IEnumerator Play1stClips()
	{
		clips1Running = true;

		while ( clips1Source != null )
		{
			float time = Random.Range( minTime, maxTime );
			yield return new WaitForSeconds( time );
			UpdateClips();
			clips1Source.volume = Random.Range( clipMinVolume, clipMaxVolume );
			clips1Source.pitch = Random.Range( 0.9f, 1.1f );
			clips1Source.panStereo = Random.Range( -1f, 1f );
			if ( currentClips.Count != 0 )
			{
				clips1Source.clip = currentClips [ Random.Range( 0, currentClips.Count ) ];
				clips1Source.Play();
			}

			if ( clips1Source.clip != null )
				yield return new WaitForSeconds( clips1Source.clip.length );
			else
				yield return new WaitForSeconds( 3f );
		}

		clips1Running = false;
		yield return null;
	}

	IEnumerator Play2ndClips()
	{
		clips2Running = true;

		yield return new WaitForSeconds( ( Random.Range( minTime, maxTime ) ) * 1.5f );

		while ( clips2Source != null )
		{
			float time = Random.Range( minTime, maxTime );
			yield return new WaitForSeconds( time );
			UpdateClips();
			clips2Source.volume = Random.Range( clipMinVolume, clipMaxVolume );
			clips2Source.pitch = Random.Range( 0.9f, 1.1f );
			clips2Source.panStereo = Random.Range( -1f, 1f );
			if ( currentClips.Count != 0 )
			{
				clips2Source.clip = currentClips [ Random.Range( 0, currentClips.Count ) ];
				clips2Source.Play();
			}


			if ( clips2Source.clip != null )
				yield return new WaitForSeconds( clips2Source.clip.length );
			else
				yield return new WaitForSeconds( 3f );
		}

		clips2Running = false;
		yield return null;
	}

	IEnumerator Play3rdClips()
	{
		clips3Running = true;


		yield return new WaitForSeconds( ( Random.Range( minTime, maxTime ) ) * 2.5f );

		while ( clips3Source != null )
		{
			float time = Random.Range( minTime, maxTime );
			yield return new WaitForSeconds( time );
			UpdateClips();
			clips3Source.volume = Random.Range( clipMinVolume, clipMaxVolume );
			clips3Source.pitch = Random.Range( 0.9f, 1.1f );
			clips3Source.panStereo = Random.Range( -1f, 1f );
			if ( currentClips.Count != 0 )
			{
				clips3Source.clip = currentClips [ Random.Range( 0, currentClips.Count ) ];
				clips3Source.Play();
			}

			if ( clips3Source.clip != null )
				yield return new WaitForSeconds( clips3Source.clip.length );
			else
				yield return new WaitForSeconds( 3f );
		}

		clips3Running = false;
		yield return null;
	}

	public void UpdateClips()
	{
		currentClips.Clear();

		if ( loop1Active == true && loop1Clips.Count != 0 )
		{
			currentClips.AddRange( loop1Clips );
		}

		if ( loop2Active == true && loop2Clips.Count != 0 )
		{
			currentClips.AddRange( loop2Clips );
		}

		if ( loop3Active == true && loop3Clips.Count != 0 )
		{
			currentClips.AddRange( loop3Clips );
		}


	}

}