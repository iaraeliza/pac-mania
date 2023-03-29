using UnityEngine;

public class PlayWakaWaka : MonoBehaviour
{
	public AudioClip WakaClip1;
	public AudioClip WakaClip2;

	private AudioSource _audioSource;

	private static bool _switchClip;

	private void OnDestroy()
	{
		_audioSource = FindObjectOfType<AudioSource>();
		if (_audioSource != null)
		{
			_audioSource.PlayOneShot(_switchClip ? WakaClip1 : WakaClip2);
			_switchClip = !_switchClip;
		}
	}
}
