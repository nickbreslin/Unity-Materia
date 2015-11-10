using UnityEngine;
using System.Collections;

public class Director : MonoSingleton<Director>
{
	void Start()
	{
		DontDestroyOnLoad(this);
		Application.LoadLevel (1);

	}
}
