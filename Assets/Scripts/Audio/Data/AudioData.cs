using UnityEngine;
using System.Collections.Generic;

public class AudioData : ScriptableObject
{
	public AudioClip UI_TAP;
	public AudioClip GAME_START;

	private Dictionary<Effect, AudioClip> effectToClipMap;

	void OnEnable()
	{
		effectToClipMap = new Dictionary<Effect, AudioClip>
		{
			{ Effect.NONE, null },
			{ Effect.UI_TAP, UI_TAP },
			{ Effect.GAME_START, GAME_START },
		};
	}

	public AudioClip GetEffectClip(Effect effect)
	{
		return effectToClipMap[effect];
	}
}
