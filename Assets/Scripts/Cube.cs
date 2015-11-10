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
	Exploded,
	Selected,
	ExplodeAll
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
		
		//if(state == CubeState.Harvesting)
		//{
			//renderer.material.color = Color.cyan;
		//	state = CubeState.Stable;
		//	Planet.instance.Combine (type);
		//}
		
		//  fade in place.
		if(state == CubeState.Depleted)
		{
			destroyDecay();
		}
		
		// fade and move away from origin
		if( state == CubeState.Exploded)
		{
			t += Time.deltaTime;
				explodeDecay();
		}
		
		//clean up from decays.
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
			//GetComponent<Renderer>().material.color = new Color(1,1,0,1f);
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
		GetComponent<Renderer>().material.color -= new Color(.025f,.025f,.025f,0.05f);
	}
	
	public void explodeDecay()
	{
		if(transform.position == Vector3.zero)
		{
			transform.position = new Vector3(0.1f, 0.1f, 0.1f);
		}
		
		transform.position = Vector3.Lerp (transform.position, transform.position * 2, t / 30);
		GetComponent<Renderer>().material.color -= new Color(0,1,1,0.001f);
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

	public void SetHover()
	{
		if(state != CubeState.Stable)
		{
			return;
		}
		state = CubeState.Selected;
		Planet.instance.Combine (type);
		GetComponent<Renderer>().enabled = true;
		GetComponent<Renderer>().material.color = colorNormal * 2;
		// most colorNormals have a 0.5f value, so this makes it brighter.
	}

	public void ClearHover()
	{
		if(state != CubeState.Selected)
		{
			return;
		}
		state = CubeState.Stable;
		Planet.instance.Combine (type);
		GetComponent<Renderer>().material.color = colorNormal;
		GetComponent<Renderer>().enabled = false;
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
