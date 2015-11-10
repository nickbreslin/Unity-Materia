using UnityEngine;
using System.Collections;

public class UI : MonoSingleton<UI>
{
	public Texture barEmpty;
	public Texture barGreen;
	public Texture barBlue;
	public Texture barRed;
	public Font font;
	string hover = "";
	Color hoverColor = Color.white;
	
	void OnGUI()
	{
		GUI.skin.font = font;

		if(Application.loadedLevel == 1)
		{
			UIMenu();
		}
		else if(Application.loadedLevel == 2)
		{
			UIGame();
		}
	}
	
	void UIMenu()
	{
		GUI.color = Color.cyan;
		if (GUI.Button (new Rect (Screen.width-205,5,200,40), "Play"))
		{
			Application.LoadLevel(2);
		}
		GUI.color = Color.white;
		
		GUI.Label (new Rect(10,10, 600,200)	, "Welcome, Collector.\n\nWe are in dire need of materia for our development. Below are the four types of materia you can collect. When you're ready, please begin. Collect as much as you can without causing the materia to destabilize and explode.");
		
		GUILayout.BeginArea (new Rect(220, 165, Screen.width, 3000));
		GUILayout.BeginVertical ();
		//GUILayout.FlexibleSpace ();
		GUILayout.Label ("Common Materia");
		GUILayout.Label ("Always in demand and cheap and easy to collect.");
		GUILayout.Space(40);
		
		GUILayout.Label ("Condensed Materia");
		GUILayout.Label ("The yield from collecting this materia is more\n" +
			"rewarding than common materia, but will require more energy.");
		GUILayout.Space(35);
		
		GUILayout.Label ("Volatile Materia");
		GUILayout.Label ("This materia yields no points. It is highly\n" +
			"unstable and will have violent consequences if collected,\n" +
			"but can replenish your depleted energy.");
		GUILayout.Space(30);
		
		GUILayout.Label ("Core Materia");
		GUILayout.Label ("This materia is the most valuable, but the\n" +
						"most difficult to collect. Collecting Core\n" +
						"Materia will slowly reduce stability, only for\n" +
						"experienced collectors.");
		GUILayout.FlexibleSpace ();
		GUILayout.EndVertical();
		GUILayout.EndArea ();
	}
	
	void UIGame()
	{
		if (GUI.Button (new Rect (Screen.width-105,5,100,20), "Quit"))
		{
			Application.LoadLevel(1);
		}
		if(!Game.instance.isActive)
		{
			string t = "";
			if(Player.instance.points < 200)
			{
				t = "Better luck next time";
			}
			else if(Player.instance.points < 500)
			{
				t = "Not bad";
			}
			else if(Player.instance.points < 900)
			{
				t = "Pretty good";
			}
			else
			{
				t = "Outstanding";
			}
			GUI.Label (new Rect(Screen.width/2 - 200,Screen.height/2 - 100, 400,200), ""+t+", Collector.\n\n" +
				"Unfortunately, the explosion has damaged your " +
				"equipment and you'll need to return for repairs. You reached level " +
				Player.instance.level + " and collected " + Player.instance.points + " points of Materia.");
		
			return;
		}
		
		if(Player.instance.points >= Player.instance.level * 100)
		{
			GUI.color = Color.cyan;
			if (GUI.Button (new Rect (5,Screen.height - 45,200,40), "Next Level"))
			{
				Player.instance.level++;
				Application.LoadLevel(2);
			}
		}
		GUI.color = Color.white;
		if (GUI.Button (new Rect (5,5,150,20), "Toggle Instructions"))
		{
			Player.instance.hideHints = !Player.instance.hideHints;
		}
		
		if(!Player.instance.hideHints)
		{
			GUILayout.BeginArea (new Rect(10,30,500, 120));
			GUILayout.BeginVertical ();
			GUILayout.Label ( "1. Use the Arrow Keys or Mouse Right-Click and Drag to adjust your view.");
			GUILayout.Label ( "2. Use the Mouse Left-Click to collect Materia.");
			GUILayout.Label ( "3. Energy will recharge over time.");
			GUILayout.Label ("4. Collect Materia until you have enough points to go to the next level.");
			GUILayout.EndVertical ();
			GUILayout.EndArea ();
		}
		
		GUILayout.BeginArea (new Rect(10,Screen.height-100,500, 100));
			GetHover();
			GetVolatility();
			GetEnergy();
		GUILayout.EndArea ();
		
		GUILayout.BeginArea (new Rect(Screen.width - 300,Screen.height-100,300, 100));
			GUILayout.Label ("Level: " + Player.instance.level.ToString ());
			GUILayout.Label ("Points To Next Level: " +(Player.instance.level * 100 - Player.instance.points));
			GUILayout.Label ("Total Points: " + Player.instance.points.ToString ());
			GetMode();
		GUILayout.EndArea ();

		//Energy
		//new Rect(10,Screen.height-100,500, 100)
		/*
		GUI.DrawTexture(new Rect (200, 100, 206,28), barEmpty, ScaleMode.ScaleAndCrop);
		Rect manaRect = new Rect(200, 100, 206, 28);
		GUI.BeginGroup (manaRect);
		GUI.DrawTexture(new Rect(0,0,206,28), barGreen, ScaleMode.ScaleAndCrop);  
		GUI.EndGroup ();


		GUI.DrawTexture(new Rect (200, 150, 206,28), barEmpty, ScaleMode.ScaleAndCrop);
		 manaRect = new Rect(200, 150, 206, 28);
		GUI.BeginGroup (manaRect);
		GUI.DrawTexture(new Rect(0,0,206,28), barBlue, ScaleMode.ScaleAndCrop);   
		GUI.EndGroup ();


		GUI.DrawTexture(new Rect (200, 150, 206,28), barEmpty, ScaleMode.ScaleAndCrop);
		manaRect = new Rect(200, 150, 206, 28);
		GUI.BeginGroup (manaRect);
		GUI.DrawTexture(new Rect(0,0,206,28), barRed, ScaleMode.ScaleAndCrop);   
		GUI.EndGroup ();
		*/
	}
	
	public void SetHover(Cube cube)
	{
		hoverColor = cube.colorNormal;
		int percent = Mathf.RoundToInt(((float)cube.hits / (float)cube.maxHits) * 100);
		hover = cube.type.ToString () + " Materia" +
				" ("+percent+"%)";
	
	}

		public void GetHover()
	{
		GUI.color = hoverColor;
		GUILayout.Label ("Target: "+hover);
		GUI.color = Color.white;
	}
	public void ClearHover() {
		hover = "";

	}
	
	public void GetMode()
	{
		GUILayout.BeginHorizontal();
		GUI.color = Color.white;
		GUILayout.Label ( "Mode: [" );
		if(Player.instance.mode == ClickMode.Single)
		{
			GUI.color = Color.green;
		}
		else
		{
			GUI.color = Color.white;
		}
		GUILayout.Label ( ClickMode.Single.ToString());
		GUI.color = Color.white;
		GUILayout.Label ( "/" );
		if(Player.instance.mode == ClickMode.Automatic)
		{
			GUI.color = Color.green;
		}
		else
		{
			GUI.color = Color.white;
		}
		GUILayout.Label ( ClickMode.Automatic.ToString());
		GUI.color = Color.white;
		GUILayout.Label ( "] (Tab)" );
		GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

	}
	
	public void GetVolatility()
	{
		GUI.color = Color.white;
		var v = Mathf.FloorToInt(Planet.instance.volatility/10);
		Color color;
		if(v < 3)
		{
			color = Color.red;
		}
		else if( v < 6)
		{
			color = Color.yellow;
		}
		else{
			color = Color.green;
		}
		/*
		 * [==============     ] (70%)
		 */
		
		GUILayout.BeginHorizontal();
		GUILayout.Label ("Volatility: ");
		GUILayout.Label ("[");
		GUI.color = color;
		string s = "";
		for(int i=0;i<v;i++)
		{
			s += "=";
		}
		for(int i=0;i<10-v;i++)
		{
			s += "_";
		}
		GUILayout.Label (s);
		
		GUI.color = Color.white;
		GUILayout.Label ("] (");
		GUI.color = color;
		GUILayout.Label (Planet.instance.volatility+"%");
		GUI.color = Color.white;
		GUILayout.Label (")");
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}
	
	public void GetEnergy()
	{
		GUI.color = Color.white;
		var v = Mathf.FloorToInt((float)Player.instance.energy/(float)Player.instance.maxEnergy*10);
		Color color;
		if(v < 3)
		{
			color = Color.red;
		}
		else if( v < 6)
		{
			color = Color.yellow;
		}
		else{
			color = Color.green;
		}
	
		
		GUILayout.BeginHorizontal();
		GUILayout.Label ("Energy: ");
		GUILayout.Label ("[");
		GUI.color = color;
		string s = "";
		for(int i=0;i<v;i++)
		{
			s += "=";
		}
		for(int i=0;i<10-v;i++)
		{
			s += "_";
		}
		GUILayout.Label (s);
		
		GUI.color = Color.white;
		GUILayout.Label ("] (");
		GUI.color = color;
		GUILayout.Label (Mathf.FloorToInt((float)Player.instance.energy/(float)Player.instance.maxEnergy*100)+"%");
		GUI.color = Color.white;
		GUILayout.Label (")");
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}
	
	
	static public void DrawLabel (Rect rect, string value, Color color, int padding = 1)
	{
		GUIStyle style = GUI.skin.label;
		style.alignment = TextAnchor.MiddleCenter;
		
//		Rect r = new Rect (rect.x + padding, rect.y + padding, rect.width, rect.height);
		
		//GUI.color = color - new Color(0.5f,0.5f,0.5f, 0f);
		//GUI.Label (r, value, style);
		GUI.color = color;
		GUI.Label (rect, value, style);
	}
}

