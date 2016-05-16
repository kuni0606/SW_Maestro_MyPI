using UnityEngine;
using System.Collections;

public class SceneFadeinout : MonoBehaviour
{
	public float fadeSpeed = 1.5f;          // Speed that the screen fades to and from black.
	private bool sceneStarting = true;      // Whether or not the scene is still fading in.

	void Awake ()
	{GUITexture gTexture = GetComponent<GUITexture> ();
		// Set the texture so that it is the the size of the screen and covers it.
		 gTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
	}
	
	
	void Update ()
	{
		// If the scene is starting...
		if(sceneStarting)
			// ... call the StartScene function.
			StartScene();
	}
	
	
	void FadeToClear ()
	{GUITexture gTexture = GetComponent<GUITexture> ();
		// Lerp the colour of the texture between itself and transparent.
		gTexture.color = Color.Lerp(gTexture.color, Color.clear, fadeSpeed * Time.deltaTime);
	}
	
	
	void FadeToBlack ()
	{GUITexture gTexture = GetComponent<GUITexture> ();
		// Lerp the colour of the texture between itself and black.
		gTexture.color = Color.Lerp(gTexture.color, Color.black, fadeSpeed * Time.deltaTime);
	}
	
	
	void StartScene ()
	{GUITexture gTexture = GetComponent<GUITexture> ();
		// Fade the texture to clear.
		FadeToClear();
		
		// If the texture is almost clear...
		if(gTexture.color.a <= 0.05f)
		{
			// ... set the colour to clear and disable the GUITexture.
			gTexture.color = Color.clear;
			gTexture.enabled = false;
			
			// The scene is no longer starting.
			sceneStarting = false;
		}
	}
	
	
	public void EndScene ()
	{GUITexture gTexture = GetComponent<GUITexture> ();
		// Make sure the texture is enabled.
		gTexture.enabled = true;
		
		// Start fading towards black.
		FadeToBlack();
		
		// If the screen is almost black...
		if(gTexture.color.a >= 0.95f)
			// ... reload the level.
			Application.LoadLevel(3);
	}
}