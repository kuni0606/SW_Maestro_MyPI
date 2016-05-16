using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionToggle : MonoBehaviour {

	public Toggle walkTo;
	public Toggle owlTo;
	public Toggle seaTo;
	public Toggle windTo;
	public Toggle fireTo;
	public Toggle toggleTo;
	public WalkSoundScript walkSound;
	public OwlSoundScript owlSound;
	public GameObject fireSound;

	// Use this for initialization
	void Start () {
		walkTo.onValueChanged.AddListener (delegate {
			WalkCheck ();
		});
		owlTo.onValueChanged.AddListener (delegate {
			OwlCheck ();
		});
		seaTo.onValueChanged.AddListener (delegate {
			SeaCheck ();
		});
		windTo.onValueChanged.AddListener (delegate {
			WindCheck ();
		});
		fireTo.onValueChanged.AddListener (delegate {
			FireCheck ();
		});
		toggleTo.onValueChanged.AddListener (delegate {
			ToggleCheck ();
		});
	}

	public void WalkCheck(){
		if (walkTo.isOn == true) {
			//walkSound play;
			walkSound.enabled = true;
		} else if (walkTo.isOn == false) {
			//walkSound quit;
			walkSound.enabled = false;
		}
	}
	public void OwlCheck(){
		if (owlTo.isOn == true) {
			//owlSound play;
			owlSound.enabled = true;
		} else if (owlTo.isOn == false) {
			//owlSound quit;
			owlSound.enabled = false;
		}
	}
	public void SeaCheck(){
		if (seaTo.isOn == true) {
			//seaSound play;
		} else if (seaTo.isOn == false) {
			//seaSound quit;
		}
	}
	public void WindCheck(){
		if (windTo.isOn == true) {
			//windSound play;
		} else if (windTo.isOn == false) {
			//windSound quit;
		}
	}
	public void FireCheck(){
		if (fireTo.isOn == true) {
			//fireSound play;
			fireSound.SetActive(true);
		} else if (fireTo.isOn == false) {
			//fireSound quit;
			fireSound.SetActive(false);
		}
	}
	public void ToggleCheck(){
		
	}










}
