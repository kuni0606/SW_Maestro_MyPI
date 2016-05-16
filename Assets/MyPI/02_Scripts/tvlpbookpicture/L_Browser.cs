using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public class L_Browser : MonoBehaviour {
	
	public GameObject FilePrefab;
	public string mypath;
	public AudioSource source;

	List<GameObject> filebuttons;
	List<GameObject> dirbuttons;
	
	DirectoryInfo dir, fi;
	string output = "no file";
	string url="", url2="", url3="", filename="", fileextention="";

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
		//urltxt.text = mypath;

		mode = 0;
		armAngle = 0.0f;
		discAngle = 0.0f;
		discSpeed = 0.0f;

		source.Play ();
	}
	
	// Update is called once per frame
	void OnTriggerEnter(Collider other){
		if (other.gameObject.name == "bone3"&& Browser.current.gameObject.activeInHierarchy==false) {
			Browser.current.gameObject.SetActive (true);

			Browser.current.backbut.onClick.RemoveAllListeners ();
			Browser.current.backbut.onClick.AddListener (() => Back ());
					
			Browser.current.homebut.onClick.RemoveAllListeners ();
			Browser.current.homebut.onClick.AddListener (() => Home ());

			Browser.current.openbut.onClick.RemoveAllListeners ();
			Browser.current.openbut.onClick.AddListener (() => open ());
					
			Browser.current.closebut.onClick.RemoveAllListeners ();
			Browser.current.closebut.onClick.AddListener (() => close ());

			FileList ();
		}

	}
	void Update(){
		if (source!=null&&source.isPlaying) {
			if (Input.GetMouseButtonDown (0)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				
				if (Physics.Raycast (ray, out hit)) {
					if (hit.collider.gameObject == gameObject) {
						source.Stop();
						wait = 0f;
					}
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


		if (wait <= 0f) {
			recordPlayerActive = false;
			mode = 3;
		}
	}


	void FileList(){
		float currentPosY = 0f, currentPosX = 0f;
		int cnt = 1;
		Browser.current.urltxt.text = mypath;
		
		dir = new DirectoryInfo (mypath);
		DirectoryInfo[] info = dir.GetDirectories("*.*");
		
		foreach (DirectoryInfo d in info) {
			GameObject go = Instantiate (Browser.current.DirPrefab) as GameObject;
			go.transform.SetParent (Browser.current.contents, false);
			
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
		List<FileInfo> infofi = new List<FileInfo> ();
		infofi.AddRange (fi.GetFiles ("*.mp3"));
		infofi.AddRange (fi.GetFiles ("*.wav"));
		infofi.AddRange (fi.GetFiles ("*.ogg"));
		
		foreach (FileInfo f in infofi) {
			GameObject go = Instantiate (FilePrefab) as GameObject;
			go.transform.SetParent (Browser.current.contents, false);
			
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

	public void SelectFile(string r){
		output = mypath;

		url = "file://";
		string[] urlResult = output.Split('\\');
		foreach (string s in urlResult) {
			url += s;
			url += '/';
		}

		string[] fileResult = r.Split('.');
		filename = fileResult [0];
		fileextention = fileResult [1];

		url2 = output + "\\" + r;
		url3 = output + "\\"+filename+".ogg"; 

		open ();
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
		foreach (GameObject g in filebuttons) {
			Destroy(g);
		}
		filebuttons.Clear ();
		
		foreach (GameObject g in dirbuttons) {
			Destroy(g);
		}
		dirbuttons.Clear ();

		output = "no file";
		Browser.current.gameObject.SetActive(false);
	}
	
	public void open(){

		if (fileextention != "ogg" && fileextention != "wav") {
			try {
				Process myProcess = new Process ();
				myProcess.StartInfo.FileName = Application.dataPath + "/ffmpeg.exe";
				myProcess.StartInfo.Arguments = " -i \"" + url2 + "\" -acodec libvorbis \"" + url3 + "\"";
				myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				myProcess.Start ();
				myProcess.WaitForExit ();
				int ExitCode = myProcess.ExitCode;
				url += filename + ".ogg";
				StartCoroutine ("Func");
			} catch (Exception e) {
				//print(e);        
			}
		} else {
			if(fileextention == "ogg")
			{	
				url += filename + ".ogg";
				StartCoroutine ("Func");
			}
			else
			{	
				url += filename + ".wav";
				StartCoroutine ("Func");
			}
		}

	}
	
	IEnumerator Func() {
		UnityEngine.Debug.Log (url);

		WWW www = new WWW (url); 

		yield return www;

		source.clip = www.audioClip;
		wait = source.clip.length;

		while (!source.isActiveAndEnabled) {
			//Debug.Log("while source is not ready");
			yield return null;
		}

		source.Play ();
		//source.PlayOneShot(source.clip,1f);
		
		recordPlayerActive = true;
	}
	



}
