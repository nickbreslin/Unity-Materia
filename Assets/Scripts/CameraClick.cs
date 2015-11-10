using UnityEngine;
using System.Collections;

public enum ClickMode
{
	Single,
	Automatic
}

public class CameraClick : MonoSingleton<CameraClick>
{
	Cube lastHitCube;

	void Update()
	{
		if(!Game.instance.isReady)
		{
			return;
		}
		
		Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);		
		RaycastHit hit = new RaycastHit();
		UI.instance.ClearHover();

		// Clear Hover effect
		if(lastHitCube)
		{
			lastHitCube.ClearHover();
			lastHitCube = null;
		}

    	if (Physics.Raycast (ray, out hit, 1000)) 
		{
			
			GameObject go = hit.collider.gameObject;
			Cube cube = go.GetComponent<Cube>();
			
			if(cube != null)
			{
				// Set Hover effect and cache object.
				UI.instance.SetHover(cube);
				lastHitCube = cube;
				lastHitCube.SetHover();

				// Single Hit
				if(Player.instance.mode == ClickMode.Single)
				{
					if(Input.GetMouseButtonDown(0))
					{
						lastHitCube.Hit();

					}
				}

				// Multiple Hit
				else if(Player.instance.mode == ClickMode.Automatic)
				{
					if(Input.GetMouseButton (0))
					{
						lastHitCube.Hit();
					}
				}
			}
		}
				

    }
}
