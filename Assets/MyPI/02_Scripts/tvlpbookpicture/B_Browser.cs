using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
//using UnityEditor;

public class B_Browser : MonoBehaviour {
	//BookAnimation b;
	//Book bookobj;
	bookplay b;
	public GameObject bookin;
	public string mypath;
	public GameObject FilePrefab;
	
	List<GameObject> filebuttons;
	List<GameObject> dirbuttons;
	
	DirectoryInfo dir, fi;
	string output = "no file";
	string url="";
	
	void Awake(){
		filebuttons = new List<GameObject>();
		dirbuttons = new List<GameObject>();

		bookline = new string[5000];
		//b = GetComponent<BookAnimation> ();
	}
	
	// Use this for initialization
	void Start () {
		bookin = page.currentpage.gameObject;

		mypath = Application.dataPath;
		//urltxt.text = mypath;
	}
	
	// Update is called once per frame
	void OnTriggerEnter(Collider other){
		if (other.gameObject.name == "bone3"&& Browser.current.gameObject.activeInHierarchy==false) {

			//bookobj = hit.transform.GetComponent<Book>();
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
		FileInfo[] infofi = fi.GetFiles("*.txt");
		
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

	public string[] bookline;
	public int l;
	
	public void open(){
		Debug.Log (output);
		Browser.current.gameObject.SetActive(false);
		string buf = System.IO.File.ReadAllText (output);

		bookin.SetActive (true);

		l=-1;
		string line=" ";
			if (buf != null) {
			StringReader reader = new StringReader (buf); 
			string txt;
			while ((txt = reader.ReadLine()) != null) {	
					
				if (txt.Length>19)
				{
					string[] words = txt.Split(' ');
		
					for(int i=0; i<words.Length; i++)
					{
						if(line.Length>19)
						{
							l++;
							bookline[l] = line;
							Debug.Log (line);
							line = "";

						}
						line += " " + words[i];
					
					}
					if(line != null)
					{
						l++;
						bookline[l] = line;
						Debug.Log (line);
						line = " ";
					}
				}

				else
				{
					l++;
					bookline[l] = txt;
					Debug.Log (txt);
				}
			}

		}
	}
}

