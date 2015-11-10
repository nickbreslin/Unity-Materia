using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Planet : MonoSingleton<Planet>
{
	public GameObject prefabCore;
	public GameObject prefabDefault;
	public GameObject prefabCondensed;
	public GameObject prefabVolatile;
	
	public List<Cube> cubes = new List<Cube>();
	public List<Node> nodes = new List<Node>();
	
	public GameObject combinedVolatile;
	public GameObject combinedCommon;
	public GameObject combinedCondensed;
	public GameObject combinedCore;
	
	public int volatility = 100;
	
	public float delay = 0;
	
	public int size = 0;
	
	public void AdjustVolatility(int amount)
	{
		volatility = Mathf.Clamp (volatility + amount, 0, 100);
		
		if(volatility == 0)
		{
			Explode ();
			Game.instance.Exploded();
		}
	}
	

	void Start ()
	{			
		if(size == 0)
		{
			size = Mathf.RoundToInt(100 * (1 + (float)Player.instance.level * (float)Player.instance.level * 0.1f));
		}
		
		int c = 10;
		int v = Mathf.Clamp(12 - Player.instance.level * 2, 2, 10);
		StartCoroutine (Build(size, c, v));
	}
	/*
	IEnumerator Build(List<Vector3> vectors)
	{
		foreach(Vector3 v in vectors)
		{
			GameObject go = Instantiate(prefab, v, Quaternion.identity) as GameObject;
			go.transform.parent = transform;
			cubes.Add(go.GetComponent<Cube>());
			yield return new WaitForSeconds(0.2f);
		}
	}
	*/
	IEnumerator Build(int remaining, int chanceC, int chanceV)
	{
		Node node = findClosestNode();
		GameObject prefab = prefabDefault;
		
		if(node.position == Vector3.zero)
		{
			prefab = prefabCore;
		}
		else
		{
			int chance = Random.Range(0,chanceC);
			
			if(chance == 0)
			{
				prefab = prefabCondensed;
			}
			else
			{
				chance = Random.Range(0,chanceV);
				
				if(chance == 0)
				{
					prefab = prefabVolatile;
				}
				
			}
		}
		GameObject go = Instantiate(prefab, node.position, Quaternion.identity/*transform.rotation*/) as GameObject;
		go.transform.parent = transform;
		node.cube = go.GetComponent<Cube>();
		Combine (node.cube.type);

		if(remaining > 0)
		{
			yield return new WaitForSeconds(delay);
			yield return StartCoroutine(Build (remaining-1, chanceC, chanceV));
		}
		else
		{
			Combine (CubeType.Common);
			Combine (CubeType.Condensed);
			Combine (CubeType.Core);
			Combine (CubeType.Volatile);
			Game.instance.isReady = true;
		}
	}
	
	Node findClosestNode()
	{
		if(nodes.Count == 0)
		{
			initializeNodes (10);
		}
		
		foreach(Node node in nodes)
		{
			if(node.cube == null)
				return node;
		}
		
		return null;
	}
			
	void initializeNodes(int limit)
	{
		for(int x = -limit; x < limit; x++)
		{
			for(int y = -limit; y < limit; y++)
			{
				for(int z = -limit; z < limit; z++)
				{
					Node node = new Node();
					node.position = new Vector3(x,y,z);
					node.distance = Vector3.Distance(transform.position, node.position);
					nodes.Add(node);
				}
			}
		}
		
		nodes.Sort(delegate(Node p1, Node p2) { return p1.distance.CompareTo(p2.distance); });
	}

	

    public void Combine(CubeType type) {
		GameObject[] gos = GameObject.FindGameObjectsWithTag("Cube");
		List <MeshFilter> meshFilters = new List<MeshFilter>();

		for(int x=0; x < gos.Length;x++)
		{
			Cube cube = gos[x].GetComponent<Cube>();
			if(cube.state == CubeState.Stable && cube.type == type)
			{
				meshFilters.Add(gos[x].GetComponent<MeshFilter>());
			}
		}

		CombineInstance[] combine = new CombineInstance[meshFilters.Count];
        int i = 0;
        while (i < meshFilters.Count) {
			if(meshFilters[i] != null)
			{
            	combine[i].mesh = meshFilters[i].sharedMesh;
            	combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            	meshFilters[i].gameObject.GetComponent<Renderer>().enabled = false;
			}
            i++;
            
        }

		if(type == CubeType.Common)
		{
        	combinedCommon.GetComponent<MeshFilter>().mesh = new Mesh();
        	combinedCommon.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
			combinedCommon.transform.rotation = Quaternion.identity;
        	//combinedCommon.gameObject.SetActive(true);
		}

		if(type == CubeType.Condensed)
		{
        	combinedCondensed.GetComponent<MeshFilter>().mesh = new Mesh();
        	combinedCondensed.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
			combinedCondensed.transform.rotation = Quaternion.identity;
        	//combinedCondensed.gameObject.SetActive(true);
		}
		if(type == CubeType.Volatile)
		{
        	combinedVolatile.GetComponent<MeshFilter>().mesh = new Mesh();
        	combinedVolatile.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
			combinedVolatile.transform.rotation = Quaternion.identity;
        	//combinedVolatile.gameObject.SetActive(true);
		}
		if(type == CubeType.Core)
		{
        	combinedCore.GetComponent<MeshFilter>().mesh = new Mesh();
        	combinedCore.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
			combinedCore.transform.rotation = Quaternion.identity;
        	//combinedCore.gameObject.SetActive(true);
		}

    }
	
	static public void Explode()
	{
		foreach(Node node in Planet.instance.nodes)
		{
			if(node.cube)
			{
				node.cube.Explode();
			}
		}
	 }
	
	public IEnumerator LocalExplode(float t)
	{
		foreach(Node node in nodes)
		{
			if(node.cube)
			{
				//node.cube.
				if(node.cube.transform.position == Vector3.zero)
				{
					node.cube.transform.position = new Vector3(0.1f, 0.1f, 0.1f);
				}
				
				node.cube.transform.position = Vector3.Lerp (node.cube.transform.position, node.cube.transform.position * 2, t / 30);
				DestroyImmediate(node.cube.GetComponent<Collider>());
			}
		}
		
		combinedCommon.GetComponent<Renderer>().material.color -= new Color(0,1,1,0.001f);
		combinedVolatile.GetComponent<Renderer>().material.color -= new Color(0,1,1,0.001f);
		combinedCore.GetComponent<Renderer>().material.color -= new Color(0,1,1,0.001f);
		combinedCondensed.GetComponent<Renderer>().material.color -= new Color(0,1,1,0.001f);
		
		Combine (CubeType.Common);
		Combine (CubeType.Condensed);
		Combine (CubeType.Core);
		Combine (CubeType.Volatile);
	 
		t += Time.deltaTime;
		
		yield return 0;
		
		StartCoroutine (LocalExplode(t));
	}
}

public class Node
{
	public Vector3 position;
	public float distance;
	public Cube cube;
}
