using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class OptionSlider : MonoBehaviour {
	
	public Slider sliderV;
	public TOD_Sky time;
	
	// Use this for initialization
	void Start () {
		sliderV.onValueChanged.AddListener (delegate {
			ValueChangeCheck ();
		});
	}
	
	// Update is called once per frame
	public void ValueChangeCheck(){
		time.Cycle.Hour = sliderV.value;
	}
	
	void Update(){
		sliderV.value = time.Cycle.Hour;
	}
}
