using UnityEngine;
using System.Collections;

public class AudioController
{

	private static EffectController _instance;
	public static EffectController Instance
	{
		get
		{
			if( _instance == null )
				_instance = new EffectController();

			return _instance;
		}
	}
}
