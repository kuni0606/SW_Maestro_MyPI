using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class L_Browser : MonoBehaviour {

	public GameObject browser;
	public Button backbut, homebut, closebut, openbut;
	public GameObject FilePrefab, DirPrefab;
	public Transform contents;
	public string mypath;
	public InputField urltxt;
	public AudioSource source;
	
	List<GameObject> filebuttons;
	List<GameObject> dirbuttons;
	
	DirectoryInfo dir, fi;
	string output = "no file";
	string url="";

	bool recordPlayerActive = false;
	
	GameObject disc;
	GameObject arm;
	
	int mode;
	float armAngle;
	float discAngle;
	float discSpeed;

	float wait;
	
	void Awake(){
		filebuttons = new List<GameObject>();
		dirbuttons = new List<GameObject>();

		disc = gameObject.transform.Find("teller").gameObject;
		arm = gameObject.transform.Find("arm").gameObject;
	}
	
	// Use this for initialization
	void Start () {
		mypath = Application.dataPath;
		urltxt.text = mypath;

		mode = 0;
		armAngle = 0.0f;
		discAngle = 0.0f;
		discSpeed = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			
			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider.gameObject == gameObject) {
					browser.SetActive(true);

					backbut.onClick.RemoveAllListeners ();
					backbut.onClick.AddListener(()=>Back ());
					
					homebut.onClick.RemoveAllListeners ();
					homebut.onClick.AddListener(()=>Home ());

					openbut.onClick.RemoveAllListeners ();
					openbut.onClick.AddListener(()=>open ());
					
					closebut.onClick.RemoveAllListeners ();
					closebut.onClick.AddListener(()=>close ());

					FileList();
				}
			}
		}

		//-- Mode 0: player off
		if(mode == 0)
		{   
			if(recordPlayerActive == true)
				mode = 1;
		}
		//-- Mode 1: activation
		if(mode == 1)
		{
			if(recordPlayerActive == true)
			{
				armAngle += Time.deltaTime * 30.0f;
				if(armAngle >= 30.0f)
				{
					armAngle = 30.0f;
					mode = 2;
				}
				discAngle += Time.deltaTime * discSpeed;
				discSpeed += Time.deltaTime * 80.0f;
			}
			else if(recordPlayerActive == false)
				mode = 3;
		}
		//-- Mode 2: running
		else if(mode == 2)
		{
			if(recordPlayerActive == true)
			{	
				discAngle += Time.deltaTime * discSpeed;

			}
			else if(recordPlayerActive == false)
				mode = 3;
		}
		//-- Mode 3: stopping
		else
		{
			if(recordPlayerActive == false)
			{
				armAngle -= Time.deltaTime * 30.0f;
				if(armAngle <= 0.0f)
					armAngle = 0.0f;
				
				discAngle += Time.deltaTime * discSpeed;
				discSpeed -= Time.deltaTime * 80.0f;
				if(discSpeed <= 0.0f)
					discSpeed = 0.0f;
				
				if((discSpeed == 0.0f) && (armAngle == 0.0f))
					mode = 0;
			}
			else
				mode = 1;
		}
		
		//-- update objects
		arm.transform.localEulerAngles = new Vector3(0.0f, armAngle, 0.0f);
		disc.transform.localEulerAngles = new Vector3(0.0f, discAngle, 0.0f);

		if (recordPlayerActive == true) 
			wait -= Time.deltaTime;


		if (wait < 0f)
			mode = 3;
	}
	
	void FileList(){
		float currentPosY = 0f, currentPosX = 0f;
		int cnt = 1;
		urltxt.text = mypath;
		
		dir = new DirectoryInfo (mypath);
		DirectoryInfo[] info = dir.GetDirectories("*.*");
		
		foreach (DirectoryInfo d in info) {
			GameObject go = Instantiate (DirPrefab) as GameObject;
			go.transform.SetParent (contents, false);
			
			Button b = go.GetComponent<Button>();
			RectTransform rt = b.GetComponent<RectTransform>();
			
			rt.anchoredPosition = new Vector2(currentPosX, currentPosY);
			currentPosX += 35f + rt.sizeDelta.x;
			if(cnt%5==0){
				currentPosX = 0;
				currentPosY -= 40f + rt.sizeDelta.y;
			}
			
			Text t = go.GetComponentInChildren<Text>();
			t.text = d.Name;
			
			var ce = new Button.ButtonClickedEvent();
			ce.AddListener(()=>SelectDir(t.text));
			b.onClick = ce;
			
			dirbuttons.Add (go);
			cnt++;
		}
		
		fi = new DirectoryInfo (mypath);
		FileInfo[] infofi = fi.GetFiles("*.wav");
		
		foreach (FileInfo f in infofi) {
			GameObject go = Instantiate (FilePrefab) as GameObject;
			go.transform.SetParent (contents, false);
			
			Button b = go.GetComponent<Button>();
			RectTransform rt = b.GetComponent<RectTransform>();
			
			rt.anchoredPosition = new Vector2(currentPosX, currentPosY);
			currentPosX += 35f + rt.sizeDelta.x;
			if(cnt%5==0){
				currentPosX = 0;
				currentPosY -= 40f + rt.sizeDelta.y;
			}
			Text t = go.GetComponentInChildren<Text>();
			t.text = f.Name;
			
			var ce = new Button.ButtonClickedEvent();
			ce.AddListener(()=>SelectFile(t.text));
			b.onClick = ce;
			
			filebuttons.Add (go);
			cnt++;
		}
	}
	
	public void SelectFile(string s){
		output = mypath + '\\' + s;
	}
	
	public void SelectDir(string s){
		mypath = mypath + '\\' + s;
		
		foreach (GameObject g in filebuttons) {
			Destroy(g);
		}
		filebuttons.Clear ();
		
		foreach (GameObject g in dirbuttons) {
			Destroy(g);
		}
		dirbuttons.Clear ();
		
		FileList ();
	}
	
	
	public void Back(){
		mypath = dir.Parent.FullName;
		
		foreach (GameObject g in filebuttons) {
			Destroy(g);
		}
		filebuttons.Clear ();
		
		foreach (GameObject g in dirbuttons) {
			Destroy(g);
		}
		dirbuttons.Clear ();
		
		FileList ();
	}
	
	public void Home(){
		mypath = Application.dataPath;
		
		foreach (GameObject g in filebuttons) {
			Destroy(g);
		}
		filebuttons.Clear ();
		
		foreach (GameObject g in dirbuttons) {
			Destroy(g);
		}
		dirbuttons.Clear ();
		
		FileList();
	}
	
	public void close(){
		output = "no file";
		browser.SetActive(false);
	}
	
	public void open(){
		Debug.Log (output);
		browser.SetActive(false);
		
		url = "file:/";
		string[] urlResult = output.Split('\\');
		foreach (string s in urlResult) {
			url += '/';
			url += s;
		}
		
		StartCoroutine ("Func");
	}
	
	IEnumerator Func() {
		
		WWW www = new WWW (url);
		Debug.Log (www.url); 
		
		source = GetComponent<AudioSource>();
		source.clip = www.audioClip;
		wait = source.clip.length;

		while (!source.isActiveAndEnabled) {
			Debug.Log("while source is not ready");
			yield return null;
		}
		
		source.Play ();
		
		recordPlayerActive = true;
	}
}
