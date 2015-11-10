using UnityEngine;
using System.Collections;

public enum CubeType {
	Common,
	Core,
	Condensed,
	Volatile
}

public enum CubeState {
	Stable,
	Harvesting,
	Depleted,
	Exploded
}
public class Cube : MonoBehaviour {
	
	public CubeType type;
	public CubeState state;

	public int costPerHit;
	public int stabilityPerHit;
	public int hits;
	public int maxHits;
	public Color colorNormal;
	
	float t = 0;
	
	void Awake () {
		hits = maxHits;
		colorNormal = GetComponent<Renderer>().material.color;
	}
	
	void Update () {
		
		if(state == CubeState.Stable)
		{
			GetComponent<Renderer>().material.color = colorNormal;
			return;
		}
		
		if(state == CubeState.Harvesting)
		{
			GetComponent<Renderer>().material.color = Color.cyan;
			state = CubeState.Stable;
			Planet.instance.Combine (type);
		}
		
		if(state == CubeState.Depleted)
		{
			destroyDecay();
		}
		
		if( state == CubeState.Exploded)
		{
			t += Time.deltaTime;
				explodeDecay();
		}
		
		if(GetComponent<Renderer>().material.color.a <= 0)
		{
			Planet.instance.Combine (type);	
			DestroyImmediate(gameObject);

		}
	}
	
	public void Hit() {
		if(Player.instance.AdjustEnergy(-costPerHit))
		{
			Planet.instance.AdjustVolatility(-stabilityPerHit);
			AdjustHits(-1);
		}
	}
	
	public void AdjustHits(int amount)
	{
		hits = Mathf.Clamp (hits + amount, 0, maxHits);
		
		if(hits == 0)
		{
			state = CubeState.Depleted;
			Depleted();
		}
		else{
			state = CubeState.Harvesting;
			Planet.instance.Combine (type);
			GetComponent<Renderer>().enabled = true;
		}
	}
	
	
	
	public void destroyDecay()
	{
		GetComponent<Renderer>().material.color -= new Color(1,0,1,0.05f);
	}
	
	public void explodeDecay()
	{
		transform.position = Vector3.Lerp (transform.position, transform.position * 2, t / 10);
		GetComponent<Renderer>().material.color -= new Color(0,1,1,0.02f);
	}
	
	public void Depleted()
	{
		Planet.instance.Combine (type);
		DestroyImmediate(GetComponent<Collider>());
		GetComponent<Renderer>().enabled = true;
		
		if(type == CubeType.Common)
		{
			Player.instance.points += maxHits;	
		}
		if(type == CubeType.Condensed)
		{
			Player.instance.points += maxHits * 2;	
		}
		if(type == CubeType.Core)
		{
			Player.instance.points += maxHits * 10;	
		}
		if(type == CubeType.Volatile)
		{
			Player.instance.AdjustEnergy(25);
		}
	}
	
	public void Explode()
	{
		t = 0;
		state = CubeState.Exploded;
		//isActive = false;
		//isExplode = true;
		Planet.instance.Combine (type);
		DestroyImmediate(GetComponent<Collider>());
		GetComponent<Renderer>().enabled = true;
	}
	
	
}
