using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public class T_Browser : MonoBehaviour {


	public GameObject FilePrefab;
	public string mypath;
	public MeshRenderer sc;

	List<GameObject> filebuttons;
	List<GameObject> dirbuttons;
	
	DirectoryInfo dir, fi;
	string output = "no file";
	string url="", url2="", url3="", filename="", fileextention="";
	
	WWW www;
	MovieTexture movieTexture;

	/*string debugPath = "C:/Users/종임/Desktop/debug.txt";

	void DebugLine(System.Object obj) {
		FileStream fs = new FileStream (debugPath, FileMode.Append, FileAccess.Write);
		StreamWriter sw = new StreamWriter (fs, System.Text.Encoding.UTF8);
		sw.WriteLine (obj.ToString());
		sw.Close ();
		fs.Close ();
	}*/
	
	void Awake(){
		filebuttons = new List<GameObject>();
		dirbuttons = new List<GameObject>();
	}
	
	// Use this for initialization
	void Start () {
		sc.material.color = Color.clear;

		mypath = Application.dataPath;
		//urltxt.text = mypath;
	}
	
	// Update is called once per frame
	void OnTriggerEnter(Collider other){
		if (other.gameObject.name == "bone3" && Browser.current.gameObject.activeInHierarchy==false) {
			if (movieTexture != null && movieTexture.isPlaying) {
				movieTexture.Stop ();
			} else {
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
			if(cnt%3==0){
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
		infofi.AddRange (fi.GetFiles ("*.avi"));
		infofi.AddRange (fi.GetFiles ("*.mov"));
		infofi.AddRange (fi.GetFiles ("*.mp4"));
		infofi.AddRange (fi.GetFiles ("*.ogv"));
		
		foreach (FileInfo f in infofi) {
			GameObject go = Instantiate (FilePrefab) as GameObject;
			go.transform.SetParent (Browser.current.contents, false);
			
			Button b = go.GetComponent<Button>();
			RectTransform rt = b.GetComponent<RectTransform>();
			
			rt.anchoredPosition = new Vector2(currentPosX, currentPosY);
			currentPosX += 35f + rt.sizeDelta.x;
			if(cnt%3==0){
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
		Browser.current.gameObject.SetActive(false);
		
		url = "file:///";
		string[] urlResult = output.Split('\\');
		foreach (string s in urlResult) {
			url += s;
			url += '/';
		}

		string[] fileResult = r.Split('.');
		filename = fileResult [0];
		fileextention = fileResult [1];


		url2 = output + "\\" + r;
		url3 = output + "\\"+filename+".ogv"; 

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

		if (fileextention != "ogv") {
			try {
				//DebugLine ("1");
				Process myProcess = new Process ();
				myProcess.StartInfo.FileName = Application.dataPath + "/ffmpeg.exe";
				myProcess.StartInfo.Arguments = " -i \"" + url2 + "\" -acodec libvorbis -ac 2 -ab 96k -ar 44100 -b 345k -s 640*360 \"" + url3 + "\"";
				//DebugLine (myProcess.StartInfo.FileName);
				//DebugLine (myProcess.StartInfo.Arguments);
				myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				myProcess.Start ();
				//DebugLine ("1");
				myProcess.WaitForExit ();
				int ExitCode = myProcess.ExitCode;
				//DebugLine (ExitCode);
				url += filename + ".ogv";
				StartCoroutine ("Func");
			} catch (Exception e) {
				//print(e);        
			}
		} else {
			url += filename + ".ogv";
			StartCoroutine ("Func");
		}

	}
	
	IEnumerator Func() {

		www = new WWW (url);

		yield return www;

		movieTexture = www.movie;

		Renderer r = gameObject.GetComponent<Renderer> ();
		r.material.mainTexture = movieTexture;

		AudioSource aud = GetComponent<AudioSource> ();
		aud.clip = movieTexture.audioClip;

		movieTexture.Play ();
	
		aud.Play ();
		StartCoroutine("Func2", movieTexture);

	}

	IEnumerator Func2(MovieTexture m){
		sc.material.color = Color.white;
		while (m.isPlaying)
			yield return null;
		sc.material.color = Color.clear;
	}

}
