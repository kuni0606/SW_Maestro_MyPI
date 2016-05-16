using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class Variables {
	public static int i;
	public static int j;
}

public class customize : MonoBehaviour {
	
	int i;
	public GameObject tool;
	public Image btn1, btn2, btn3, btn4, btn5, btn6;
	public Sprite[] newImage;
	public Animation cameraAnim;
	public GameObject CharacCanvas;
	public AudioClip btn_Click;
	AudioSource audio;
	public AnimationClip[] goback;

	[System.Serializable]
	public class CharacterSet {
		public GameObject character;
		public GameObject[] cloths;
		public Animation[] ani;
	}

	public CharacterSet[] characters;

	// Use this for initialization
	void Start () {
		cameraAnim.AddClip (goback[0], "go");
		cameraAnim.AddClip (goback[1], "back");
		audio = GetComponent<AudioSource> ();
		Variables.i = 0;
		Variables.j = 0;
		
		btn1.sprite = newImage[0];
		btn2.sprite = newImage[1];
		btn3.sprite = newImage[2];
		btn4.sprite = newImage[3];
		btn5.sprite = newImage[4];
		btn6.sprite = newImage[5];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void CharacterOneclick(){
		characters [1].character.SetActive (false);
		characters [2].character.SetActive (false);
		characters [0].character.SetActive (true);
		
		Variables.i = 0;
		//버튼 이미지 바꾸기
		btn1.sprite = newImage[0];
		btn2.sprite = newImage[1];
		btn3.sprite = newImage[2];
		btn4.sprite = newImage[3];
		btn5.sprite = newImage[4];
		btn6.sprite = newImage[5];
	}
	
	public void CharacterTwoclick(){
		characters [0].character.SetActive (false);
		characters [2].character.SetActive (false);
		characters [1].character.SetActive (true);
		
		Variables.i = 1;
		
		//버튼이미지 바꾸기
		btn1.sprite = newImage[6];
		btn2.sprite = newImage[7];
		btn3.sprite = newImage[8];
		btn4.sprite = newImage[9];
		btn5.sprite = newImage[10];
		btn6.sprite = newImage[11];
	}
	
	public void CharacterThreeclick(){
		characters [0].character.SetActive (false);
		characters [1].character.SetActive (false);
		characters [2].character.SetActive (true);
		
		Variables.i = 2;
		//버튼이미지 바꾸기
		btn1.sprite = newImage[12];
		btn2.sprite = newImage[13];
		btn3.sprite = newImage[14];
		btn4.sprite = newImage[15];
		btn5.sprite = newImage[16];
		btn6.sprite = newImage[17];
	}
	
	
	public void Oneclick(){
		Variables.j = 0;
		characters [Variables.i].cloths [1].SetActive (false);
		characters [Variables.i].cloths [2].SetActive (false);
		characters [Variables.i].cloths [3].SetActive (false);
		characters [Variables.i].cloths [4].SetActive (false);
		characters [Variables.i].cloths [5].SetActive (false);
		characters [Variables.i].cloths [0].SetActive (true);
		
		characters [Variables.i].ani [0].Play ();
	}
	
	public void Twoclick(){
		Variables.j = 1;
		characters [Variables.i].cloths [0].SetActive (false);
		characters [Variables.i].cloths [2].SetActive (false);
		characters [Variables.i].cloths [3].SetActive (false);
		characters [Variables.i].cloths [4].SetActive (false);
		characters [Variables.i].cloths [5].SetActive (false);
		characters [Variables.i].cloths [1].SetActive (true);
		
		characters[Variables.i].ani[1].Play ();
	}
	
	public void Threeclick(){
		Variables.j = 2;
		characters [Variables.i].cloths [0].SetActive (false);
		characters [Variables.i].cloths [1].SetActive (false);
		characters [Variables.i].cloths [3].SetActive (false);
		characters [Variables.i].cloths [4].SetActive (false);
		characters [Variables.i].cloths [5].SetActive (false);
		characters [Variables.i].cloths [2].SetActive (true);
		
		characters[Variables.i].ani[2].Play ();
	}
	
	public void Fourclick(){
		Variables.j = 3;
		characters [Variables.i].cloths [0].SetActive (false);
		characters [Variables.i].cloths [1].SetActive (false);
		characters [Variables.i].cloths [2].SetActive (false);
		characters [Variables.i].cloths [4].SetActive (false);
		characters [Variables.i].cloths [5].SetActive (false);
		characters [Variables.i].cloths [3].SetActive (true);
		
		characters[Variables.i].ani[3].Play ();
	}
	
	public void Fiveclick(){
		Variables.j = 4;
		characters [Variables.i].cloths [0].SetActive (false);
		characters [Variables.i].cloths [1].SetActive (false);
		characters [Variables.i].cloths [2].SetActive (false);
		characters [Variables.i].cloths [3].SetActive (false);
		characters [Variables.i].cloths [5].SetActive (false);
		characters [Variables.i].cloths [4].SetActive (true);
		
		characters[Variables.i].ani[4].Play ();
	}
	
	public void Sixclick(){
		Variables.j = 5;
		characters [Variables.i].cloths [0].SetActive (false);
		characters [Variables.i].cloths [1].SetActive (false);
		characters [Variables.i].cloths [2].SetActive (false);
		characters [Variables.i].cloths [3].SetActive (false);
		characters [Variables.i].cloths [4].SetActive (false);
		characters [Variables.i].cloths [5].SetActive (true);
		
		characters[Variables.i].ani[5].Play ();
	}

	public void restart(){
		Application.LoadLevel (1);
	}

	public void MapChoice(){
		audio.PlayOneShot (btn_Click, 0.7f);
		cameraAnim.Play ("go");
		CharacCanvas.SetActive (false);
	}

	public void Back(){
		audio.PlayOneShot (btn_Click, 0.7f);
		cameraAnim.Play ("back");
		CharacCanvas.SetActive (true);
	}

	public void Map1(){
		audio.PlayOneShot (btn_Click, 0.7f);
		Debug.Log ("1");
	}
	public void Map2(){
		audio.PlayOneShot (btn_Click, 0.7f);
		Debug.Log ("2");
	}
	public void Map3(){
		audio.PlayOneShot (btn_Click, 0.7f);
		Debug.Log ("3");
	}
	public void MapDefault(){
		audio.PlayOneShot (btn_Click, 0.7f);
		Debug.Log ("Default");
	}
}
