using UnityEngine;
using System.Collections;

public class MenuControl : MonoBehaviour {

	//public GameObject itemCanvas;
	public int itemCnt = 0;
	public int optionCnt = 0;
	public int helpCnt = 0;
	public int exitCnt = 0;

	public GameObject cycleBtn;
	public GameObject flashBtn;
	public GameObject boatBtn;
	public GameObject gliderBtn;
	public GameObject fireBtn;
	public GameObject lightBtn;
	public GameObject line;

	public GameObject optionMenu;
	public GameObject helpMenu;
	public GameObject exitMenu;
	public bool isAnimated { get; set; }

	public Animation optionAnim;
	public Animation helpAnim;
	public Animation exitAnim;


	// Use this for initialization
	void Start () {
		cycleBtn.SetActive (false);
		flashBtn.SetActive (false);
		boatBtn.SetActive (false);
		gliderBtn.SetActive (false);
		fireBtn.SetActive (false);
		lightBtn.SetActive (false);
		line.SetActive (false);
		optionMenu.SetActive (false);
		helpMenu.SetActive (false);
		exitMenu.SetActive (false);
		isAnimated = false;
	}
	
	IEnumerator OnItem(){
		isAnimated = true;
		line.SetActive (true);
		yield return new WaitForSeconds (0.05F);
		fireBtn.SetActive (true);
		yield return new WaitForSeconds (0.05F);
		gliderBtn.SetActive (true);
		yield return new WaitForSeconds (0.05F);
		boatBtn.SetActive (true);
		yield return new WaitForSeconds (0.05F);
		flashBtn.SetActive (true);
		yield return new WaitForSeconds (0.05F);
		cycleBtn.SetActive (true);
		yield return new WaitForSeconds (0.05F);
		lightBtn.SetActive (true);
		isAnimated = false;
	}
	
	IEnumerator OffItem(){
		isAnimated = true;
		lightBtn.SetActive (false);
		yield return new WaitForSeconds (0.05F);
		cycleBtn.SetActive (false);
		yield return new WaitForSeconds (0.05F);
		flashBtn.SetActive (false);
		yield return new WaitForSeconds (0.05F);
		boatBtn.SetActive (false);
		yield return new WaitForSeconds (0.05F);
		gliderBtn.SetActive (false);
		yield return new WaitForSeconds (0.05F);
		fireBtn.SetActive (false);
		yield return new WaitForSeconds (0.05F);
		line.SetActive (false);
		isAnimated = false;
	}
	
	// Update is called once per frame
	public void Item(){
		if (isAnimated)
			return;

		if (itemCnt == 0) {
			if(optionCnt == 1 || helpCnt == 1 || exitCnt == 1){
				optionMenu.SetActive(false);
				optionCnt = 0;
				helpMenu.SetActive(false);
				helpCnt = 0;
				exitMenu.SetActive(false);
				exitCnt = 0;
			}
			StartCoroutine("OnItem");
			itemCnt = 1;
		} else if (itemCnt == 1) {
			StartCoroutine("OffItem");
			itemCnt = 0;
		}
	}

	public void Option(){
		if (isAnimated)
			return;
		if (optionCnt == 0) {
			if (helpCnt == 1 || exitCnt == 1 || itemCnt == 1) {
				helpMenu.SetActive (false);
				helpCnt = 0;
				exitMenu.SetActive (false);
				exitCnt = 0;
				itemCnt = 0;
				cycleBtn.SetActive (false);
				flashBtn.SetActive (false);
				boatBtn.SetActive (false);
				gliderBtn.SetActive (false);
				fireBtn.SetActive (false);
				lightBtn.SetActive (false);
				line.SetActive (false);
			}
			optionMenu.SetActive(true);
			optionAnim.Play ();
			optionCnt = 1;
		}
		else if(optionCnt == 1){
			optionMenu.SetActive(false);
			optionCnt = 0;
		}
	}

	public void Help(){
		if (isAnimated)
			return;

		if (helpCnt == 0) {
			if (optionCnt == 1 || exitCnt == 1 || itemCnt == 1) {
				optionMenu.SetActive (false);
				optionCnt = 0;
				exitMenu.SetActive (false);
				exitCnt = 0;
				itemCnt = 0;
				cycleBtn.SetActive (false);
				flashBtn.SetActive (false);
				boatBtn.SetActive (false);
				gliderBtn.SetActive (false);
				fireBtn.SetActive (false);
				lightBtn.SetActive (false);
				line.SetActive (false);
			}
			helpMenu.SetActive(true);
			helpAnim.Play ();
			helpCnt = 1;
		}
		else if(helpCnt == 1){
			helpMenu.SetActive(false);
			helpCnt = 0;
		}
	}

	public void Exit(){
		if (isAnimated)
			return;

		if (exitCnt == 0) {
			if (helpCnt == 1 || optionCnt == 1 || itemCnt == 1) {
				helpMenu.SetActive (false);
				helpCnt = 0;
				optionMenu.SetActive (false);
				optionCnt = 0;
				itemCnt = 0;
				cycleBtn.SetActive (false);
				flashBtn.SetActive (false);
				boatBtn.SetActive (false);
				gliderBtn.SetActive (false);
				fireBtn.SetActive (false);
				lightBtn.SetActive (false);
				line.SetActive (false);
			}
			exitMenu.SetActive(true);
			exitAnim.Play ();
			exitCnt = 1;
		}
		else if(exitCnt == 1){
			exitMenu.SetActive(false);
			exitCnt = 0;
		}
	}

	public void ToLevel1(){
		Application.LoadLevel ("StartScene");
	}
	public void ExitExit(){
		exitMenu.SetActive (false);
		exitCnt = 0;
	}
}
