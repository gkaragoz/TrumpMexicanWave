public class EffectController
{
	private readonly EffectPlayer _player;
	private bool _isSfxOn;

	public EffectController()
	{
		_player = EffectPlayer.NewPlayer();
		_isSfxOn = true;
	}

	public void PlayEffect( Effect effect, float volume = 1f )
	{
		if ( _isSfxOn )
		{
			_player.PlayEffect( effect, volume );
		}
	}

	public void PlayEffectDistinctly( Effect effect )
	{
		if ( _isSfxOn )
		{
			_player.PlayEffectDistinctly( effect );
		}
	}

	public void StopPlaying()
	{
		_player.Stop();
	}

	public void StopPlayingDistinctEffect( Effect effect )
	{
		_player.StopDistinctEffect( effect );
	}

	private void OnSfxSettingsChanged()
	{
		_isSfxOn = !_isSfxOn;
	}

	public void Destroy()
	{
	}
}
