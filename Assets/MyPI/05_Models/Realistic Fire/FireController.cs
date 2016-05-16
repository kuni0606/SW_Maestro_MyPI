using UnityEngine;

public class FireController : MonoBehaviour {

	public Light fireLight;
	public float lightIntensityOffset;
	public float lightIntensityStep;

	private float baseLightIntensity;
	private float lightIntensity;
	private bool increase = true;

	GameObject BaseParticles;
	GameObject FlamesParticles;
	GameObject SparksParticles;
	GameObject FireLight;
	GameObject SmokeParticles;

	// Use this for initialization
	void Start () {
		// return (to avoid errors) if some of the parameters are not set
		if (fireLight == null || lightIntensityStep == null || lightIntensityOffset == null)
			return;
		// baseLightIntensity will be the lowest possible light intensity
		baseLightIntensity =  fireLight.intensity;
		// lightIntensity will be the highest possible light intensity
		lightIntensity =  + baseLightIntensity + lightIntensityOffset;

		BaseParticles = transform.FindChild("BaseParticles").gameObject;
		FlamesParticles = transform.FindChild("FlamesParticles").gameObject;
		SparksParticles = transform.FindChild("SparksParticles").gameObject;
		FireLight = transform.FindChild("FireLight").gameObject;
		SmokeParticles = transform.FindChild("SmokeParticles").gameObject;
	}

	void FixedUpdate () {
		// return (to avoid errors) if some of the parameters are not set
		if (fireLight == null || lightIntensityStep == null || lightIntensityOffset == null)
			return;

		// alternate between baseLightIntensity and lightIntensity using lightIntensityStep as an increment/decrement value
		if (increase)
		{
			if (fireLight.intensity < lightIntensity)
				fireLight.intensity += lightIntensityStep;
			else
				increase = false;
		}
		else
		{
			if (fireLight.intensity > baseLightIntensity)
				fireLight.intensity -= lightIntensityStep;
			else
				increase = true;
		}
	}

	private bool toggleFire = true;
	private bool toggleSmoke = true;
	private bool toggleSparks = true;
	private bool toggleLight = true;
	private bool toggleBase = true;

	/*void OnGUI() {
		toggleFire = GUI.Toggle(new Rect(10, 10, 100, 30), toggleFire, "Fire");
		FlamesParticles.SetActive(toggleFire);

		toggleSmoke = GUI.Toggle(new Rect(10, 50, 100, 30), toggleSmoke, "Smoke");
		SmokeParticles.SetActive(toggleSmoke);

		toggleSparks = GUI.Toggle(new Rect(10, 90, 100, 30), toggleSparks, "Sparks");
		SparksParticles.SetActive(toggleSparks);

		toggleLight = GUI.Toggle(new Rect(10, 130, 100, 30), toggleLight, "Light");
		FireLight.SetActive(toggleLight);

		toggleBase = GUI.Toggle(new Rect(10, 170, 100, 30), toggleBase, "Base");
		BaseParticles.SetActive(toggleBase);
	}*/
}
