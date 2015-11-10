using UnityEngine;
using System.Collections;

public class FontManager : MonoSingleton<FontManager> {

	public Font font1;
	public Font font2;
	public Font font3;

	/*
	public void OnGUI()
	{
		GUI.color = Color.green;
		GUI.skin.font = font1;
		GUI.Label(new Rect(10, 10, 300, 100), "The quick brown fox jumped over the lazy dog.");
		GUI.skin.font = font2;
		GUI.Label(new Rect(10, 50, 300, 100), "The quick brown fox jumped over the lazy dog.");
		GUI.skin.font = font3;
		GUI.Label(new Rect(10, 90, 300, 100), "The quick brown fox jumped over the lazy dog.");
	}
	*/
}
