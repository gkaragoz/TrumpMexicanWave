using UnityEngine;
using System.Collections.Generic;

public class AudioData : ScriptableObject
{
	public AudioClip UI_TAP;
	public AudioClip GAME_START;
	public AudioClip TRUMP_MEXICANS_1;
	public AudioClip TRUMP_AFRICANAMERICANS_1;
	public AudioClip TRUMP_WOMEN_1;
	public AudioClip TRUMP_LIBERALS_1;
	public AudioClip TRUMP_MUSLIMS_1;
	public AudioClip APPLAUSE_1;
	public AudioClip APPLAUSE_2;
	public AudioClip APPLAUSE_3;
	public AudioClip CHEERING;
	public AudioClip FAIL_TRUMPET;
	public AudioClip FANFARE_JAZZ;
	public AudioClip FANFARE_2;
	public AudioClip FANFARE_3;
	public AudioClip HAPPY_OPENING;
	public AudioClip SHORT_FAIL;

	private Dictionary<Effect, AudioClip> effectToClipMap;

	void OnEnable()
	{
		effectToClipMap = new Dictionary<Effect, AudioClip>
		{
			{ Effect.NONE, null },
			{ Effect.UI_TAP, UI_TAP },
			{ Effect.GAME_START, GAME_START },
			{ Effect.TRUMP_MEXICANS_1, TRUMP_MUSLIMS_1 },
			{ Effect.TRUMP_AFRICANAMERICANS_1, TRUMP_AFRICANAMERICANS_1},
			{ Effect.TRUMP_WOMEN_1, TRUMP_WOMEN_1},
			{ Effect.TRUMP_LIBERALS_1, TRUMP_LIBERALS_1},
			{ Effect.TRUMP_MUSLIMS_1, TRUMP_MUSLIMS_1},
			{ Effect.APPLAUSE_1, APPLAUSE_1},
			{ Effect.APPLAUSE_2, APPLAUSE_2},
			{ Effect.APPLAUSE_3, APPLAUSE_3},
			{ Effect.CHEERING, CHEERING},
			{ Effect.FAIL_TRUMPET, FAIL_TRUMPET},
			{ Effect.FANFARE_JAZZ, FANFARE_JAZZ},
			{ Effect.FANFARE_2, FANFARE_2},
			{ Effect.FANFARE_3, FANFARE_3},
			{ Effect.HAPPY_OPENING, HAPPY_OPENING},
			{ Effect.SHORT_FAIL, SHORT_FAIL}
		};
	}

	public AudioClip GetEffectClip(Effect effect)
	{
		return effectToClipMap[effect];
	}
}
