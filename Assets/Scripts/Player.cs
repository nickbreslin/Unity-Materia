using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public ClickMode mode = ClickMode.Single;
	public int energy;
	public int maxEnergy;
	public static Player instance;
	public int level = 1;
	public int points = 0;
	public bool hideHints = false;
	
	void Awake()
	{
		if(Player.instance == null)
		{
			Player.instance = this;
		}
		else
		{
			DestroyImmediate(this);
		}
	}
	
	void Start()
	{
		
				StartCoroutine (Regen());
	}
	
	void Update()
	{
				if(Input.GetKeyDown (KeyCode.Tab))
		{
			mode = (mode == ClickMode.Single) ? ClickMode.Automatic : ClickMode.Single;
		}
	}
	
	void OnLevelWasLoaded(int l)
	{
		if(l == 1)
		{
			points = 0;
			level = 1;
		}
		
		energy = maxEnergy;
	}
	
	IEnumerator Regen()
	{
		yield return new WaitForSeconds(1);
		AdjustEnergy (1);
		yield return StartCoroutine (Regen());
	}
	
	public bool AdjustEnergy(int amount)
	{
		energy = Mathf.Clamp (energy + amount, 0, maxEnergy);
		
		if(energy == 0)
		{
			return false;
		}
		
		return true;
	}
}

