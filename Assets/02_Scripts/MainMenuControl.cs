using UnityEngine;
using System.Collections;

public class MainMenuControl : MonoBehaviour {

	public int cnt = 0;
	//public GameObject CanvasNb;
	public MenuControl menuControl;
	/*public Animation helpAnim;
	public Animation optionAnim;
	public Animation itemAnim;*/
	public GameObject exitBtn;
	public GameObject optionBtn;
	public GameObject helpBtn;
	public GameObject itemBtn;
	public SightRotate sightRT;
	
	public bool isAnimated { get; set; }

	// Use this for initialization
	void Start () {
		//CanvasNb.SetActive(false);
		exitBtn.SetActive (false);
		optionBtn.SetActive (false);
		helpBtn.SetActive (false);
		itemBtn.SetActive (false);
		isAnimated = false;
	}

	IEnumerator OnMenu(){
		sightRT.setActiveUpdownBtn (false);
		isAnimated = true;
		exitBtn.SetActive (true);
		yield return new WaitForSeconds (0.05F);
		helpBtn.SetActive (true);
		yield return new WaitForSeconds (0.05F);
		optionBtn.SetActive (true);
		yield return new WaitForSeconds (0.05F);
		itemBtn.SetActive (true);
		isAnimated = false;
	}

	IEnumerator OffMenu(){
		isAnimated = true;
		itemBtn.SetActive (false);
		yield return new WaitForSeconds (0.05F);
		optionBtn.SetActive (false);
		yield return new WaitForSeconds (0.05F);
		helpBtn.SetActive (false);
		yield return new WaitForSeconds (0.05F);
		exitBtn.SetActive (false);
		sightRT.setActiveUpdownBtn (true);
		isAnimated = false;
	}
	
	// Update is called once per frame
	public void setActiveMenu(){
		if (menuControl.isAnimated)
			return;
		
		if (isAnimated)
			return;

		if (cnt == 0) {
			StartCoroutine ("OnMenu");
			cnt = 1;
		} else {
			//CanvasNb.SetActive(false);
			StartCoroutine ("OffMenu");
			menuControl.cycleBtn.SetActive (false);
			menuControl.flashBtn.SetActive (false);
			menuControl.boatBtn.SetActive (false);
			menuControl.gliderBtn.SetActive (false);
			menuControl.fireBtn.SetActive (false);
			menuControl.lightBtn.SetActive (false);
			menuControl.line.SetActive (false);

			menuControl.optionMenu.SetActive (false);
			menuControl.helpMenu.SetActive (false);
			menuControl.exitMenu.SetActive (false);
			menuControl.optionCnt = 0;
			menuControl.itemCnt = 0;
			menuControl.helpCnt = 0;
			menuControl.exitCnt = 0;
			cnt = 0;
		}
	}
}
