using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class P_Browser : MonoBehaviour {

	public GameObject FilePrefab;
	public string mypath;
	
	List<GameObject> filebuttons;
	List<GameObject> dirbuttons;
	
	DirectoryInfo dir, fi;
	string output = "no file";
	string url="";
	
	void Awake(){
		filebuttons = new List<GameObject>();
		dirbuttons = new List<GameObject>();
	}
	
	// Use this for initialization
	void Start () {
		mypath = Application.dataPath;
		//urltxt.text = mypath;
	}
	
	// Update is called once per frame
	void OnTriggerEnter(Collider other){
		if (other.gameObject.name == "bone3") {
			Browser.current.gameObject.SetActive(true);
			
			Browser.current.backbut.onClick.RemoveAllListeners ();
			Browser.current.backbut.onClick.AddListener(()=>Back ());
			
			Browser.current.homebut.onClick.RemoveAllListeners ();
			Browser.current.homebut.onClick.AddListener(()=>Home ());

			Browser.current.openbut.onClick.RemoveAllListeners ();
			Browser.current.openbut.onClick.AddListener(()=>open ());

			Browser.current.closebut.onClick.RemoveAllListeners ();
			Browser.current.closebut.onClick.AddListener(()=>close ());
			
			FileList();
			//DirList();
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
		infofi.AddRange (fi.GetFiles ("*.jpg"));
		infofi.AddRange (fi.GetFiles ("*.png"));
		
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
	
	public void SelectFile(string s){
		output = mypath + '\\' + s;
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
		//DirList();
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
		//DirList();
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
		//DirList();
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
		//Debug.Log (output);
		Browser.current.gameObject.SetActive(false);
		
		url = "file://";
		string[] urlResult = output.Split('\\');
		foreach (string s in urlResult) {
			url += '/';
			url += s;
		}

		StartCoroutine ("Func");
	}
	
	IEnumerator Func() {
		Renderer renderer = GetComponent<Renderer> ();
		renderer.material.mainTexture = new Texture2D(128, 128, TextureFormat.DXT5, false);

		// Start a download of the given URL
		WWW www = new WWW(url);
			
		// wait until the download is done
		yield return www;
			
		// assign the downloaded image to the main texture of the object
		www.LoadImageIntoTexture(renderer.material.mainTexture as Texture2D);

		/*WWW www = new WWW (url);
		yield return www;
		Renderer renderer = GetComponent<Renderer> ();
		renderer.material.mainTexture = www.texture;*/
	}

		
}


