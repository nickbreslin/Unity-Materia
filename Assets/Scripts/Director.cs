using UnityEngine;
using System.Collections;

public class Director : MonoBehaviour {

	public static Director instance;
	void Awake()
	{
		if(Director.instance == null)
		{
			Director.instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			DestroyImmediate(this);
		}
	}
	
	void Start()
	{
		Application.LoadLevel (1);
	}
}
