using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	
	public bool isActive;
	public bool isReady;
	public static Game instance;
	void Awake()
	{
		if(Game.instance == null)
		{
			Game.instance = this;
		}
		else
		{
			DestroyImmediate(this);
		}
	}
	
	public void OnLevelWasLoaded()
	{
		isActive = true;
		isReady = false;
	}
	
	public void Exploded()
	{
		isActive = false;
	}
}
