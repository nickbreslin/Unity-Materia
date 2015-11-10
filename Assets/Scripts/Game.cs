using UnityEngine;
using System.Collections;

public class Game : MonoSingleton<Game>
{
	public bool isActive;
	public bool isReady;
	
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
