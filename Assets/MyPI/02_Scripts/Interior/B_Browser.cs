using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using UnityEditor;

public class B_Browser : MonoBehaviour {

	BookAnimation b;
	Book bookobj;
	public GameObject browser;
	public Button backbut, homebut, closebut, openbut;
	public GameObject FilePrefab, DirPrefab;
	public Transform contents;
	public string mypath;
	public InputField urltxt;
	
	List<GameObject> filebuttons;
	List<GameObject> dirbuttons;
	
	DirectoryInfo dir, fi;
	string output = "no file";
	string url="";
	
	void Awake(){
		filebuttons = new List<GameObject>();
		dirbuttons = new List<GameObject>();

		b = GetComponent<BookAnimation> ();
	}
	
	// Use this for initialization
	void Start () {
		mypath = Application.dataPath;
		urltxt.text = mypath;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			
			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider.gameObject == gameObject) {

					bookobj = hit.transform.GetComponent<Book>();
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
		FileInfo[] infofi = fi.GetFiles("*.txt");
		
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
		
		FileList ();;
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
		string buf = System.IO.File.ReadAllText (output);
		//ConvertStringToTextAsset (buf);

		bookobj.pages.Add (buf);
		b.Open(bookobj, 0);
	}

	/*TextAsset ConvertStringToTextAsset(string text) {
		string temporaryTextFileName = "TemporaryTextFile";
		File.WriteAllText(Application.dataPath +  "/Resources/" + temporaryTextFileName + ".txt", text);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		TextAsset textAsset = Resources.Load(temporaryTextFileName) as TextAsset;
		return textAsset;
	}*/
	
	
	
	
}
