using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
	AudioSource[] clips;
	
	void Awake ()
	{
		clips = GetComponents<AudioSource> ();
	}
	
	void Start ()
	{
		StartCoroutine (MusicLoop (0));
	}
	
	IEnumerator MusicLoop (int i)
	{
		// Loop to first if at end.
		if (i >= clips.Length) {
			i = 0;
		}
		
		// Load clip.
		AudioSource clip = clips [i];
		
		// Lower volume and begin Play
		clip.volume = 0;
		clip.Play ();
		
		
		// Fade In
		while (clip.volume < 1) {
			yield return null;
			clip.volume += Time.deltaTime / 4;
		}
		
		// Clip is 32 seconds, wait for 24 at full volume.
		yield return new WaitForSeconds(120);
		
		// Fade Out
		while (clip.volume > 0) {
			yield return null;
			clip.volume -= Time.deltaTime / 4;
		}	
		
		// Stop
		clip.Stop ();
		
		// Load next in the loop
		i += 1;
		StartCoroutine (MusicLoop (i));
	}
	
	// Toggle for Mute/Unmute
	static public void Toggle ()
	{
		AudioListener.pause = !AudioListener.pause;
	}
	
	// SetVolume
	static public void SetVolume (float amount)
	{
		AudioListener.volume = amount; 
	}
	
	// DrawSlider
	void DrawVolumeSlider ()
	{
		//todo -- pass in rect
		AudioListener.volume = GUI.HorizontalSlider (
			new Rect (25, 25, 100, 30),
			AudioListener.volume, 0.0F, 1.0F);
	}
}
