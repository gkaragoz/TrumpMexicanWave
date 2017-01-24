using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class EffectPlayer : MonoBehaviour
{
	public AudioData AudioClipData
	{
		get;
		set;
	}

	private AudioSource audioSource;
	private Dictionary<Effect, AudioSource> _distinctAudioSources;

	public static EffectPlayer NewPlayer()
	{
		GameObject effectPlayerGO = new GameObject("EffectPlayer");
		EffectPlayer effectPlayer = effectPlayerGO.AddComponent<EffectPlayer>();

		return effectPlayer;
	}

    public void PlayEffectInLoop(Effect effect, float volume = 1f)
    {
        AudioClip clip = AudioClipData.GetEffectClip(effect);

        if (clip != null)
        {
            Play(clip, volume);
        }

        StartCoroutine(PlayEffectForever(effect, volume));
    }

    private IEnumerator PlayEffectForever(Effect effect, float volume = 1f)
    {
        AudioClip clip = AudioClipData.GetEffectClip(effect);

        if (clip != null)
        {
            Play(clip, volume);
        }

        while (audioSource.isPlaying)
            yield return null;

        PlayEffectInLoop(effect, volume);
    }

    public void PlayEffect(Effect effect, float volume = 1f)
	{
		AudioClip clip = AudioClipData.GetEffectClip(effect);

		if (clip != null)
		{
			Play(clip, volume);
		}
	}

	public void PlayEffectDistinctly(Effect effect)
	{
		AudioClip clip = AudioClipData.GetEffectClip(effect);

		if (clip != null)
		{
			if ( !_distinctAudioSources.ContainsKey( effect ) )
			{
				var newAudioSource = gameObject.AddComponent<AudioSource>();

				_distinctAudioSources[effect] = newAudioSource;

				StartCoroutine( PlayThenDestroy( effect, clip ) );
			}
			else
				_distinctAudioSources[effect].PlayOneShot( clip );
		}

	}

	private IEnumerator PlayThenDestroy(Effect effect, AudioClip clip)
	{
		if(_distinctAudioSources[effect] == null)
			yield break;

		_distinctAudioSources[effect].PlayOneShot( clip );

		while( _distinctAudioSources[effect].isPlaying )
			yield return null;

		Destroy( _distinctAudioSources[effect] );

		_distinctAudioSources.Remove( effect );

		yield return null;
	}

	private void Play(AudioClip clip, float volume = 1.0f)
	{
		audioSource.PlayOneShot(clip, volume);
	}

    private void PlayDistinctly(AudioClip clip)
	{
		throw new NotImplementedException("We don't support playing raw AudioClips disntinctly, yet. Please use an Effect for this, instead.");
	}

	public void Stop()
	{
		if ( audioSource.isPlaying )
			audioSource.Stop();
	}

	public void StopDistinctEffect( Effect effect )
	{
		if ( _distinctAudioSources.ContainsKey( effect ) && _distinctAudioSources[effect] != null )
		{
			//This will cause the PlayThenDestroy coroutine to destroy the AudioSource, FYI
			_distinctAudioSources[effect].Stop();
		}
	}

	public IEnumerable CurrentlyPlayingDistinctEffects
	{
		get
		{
			return _distinctAudioSources.Keys;
		}
	}

	void Awake()
	{
		_distinctAudioSources = new Dictionary<Effect, AudioSource>();

		DontDestroyOnLoad(gameObject);
		audioSource = gameObject.AddComponent<AudioSource>();
		AudioClipData = Resources.Load<AudioData>("AudioData");
	}
}
