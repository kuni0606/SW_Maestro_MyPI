﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class BookAnimation : MonoBehaviour {
	private List<Page1> activePages = new List<Page1>();
	private int pageNo;
	private const int titleSizeMultiplier = 5; 
	private const int textSizeMultiplier = 3;
	private const int listSizeMultiplier = 2;
	private const int numberSizeMultiplier = 2;
	//set when book is loaded
	private bool twoPages = false;
	private bool isNumbered = false;
	private float pageWidth;
	private float pageHeight;
	private float contentSpace;
	private GUIStyle numberStyle;
	private GUIStyle pageStyle;
	private GUIStyle backgroundStyle;
	private GUIStyle foregroundStyle;
	//set by user
	//public bool useCursor;
	//public Texture2D cursor;
	public AudioClip flipNoise;
	public AudioClip openNoise;
	public AudioClip closeNoise;
	public bool demoCode = false;
	
	//Customize or delete this code if required
	void Update () {			
	}
	
	//Turns page
	public void TurnPage(string direction){	
		if (direction == "Forward"){
			if (pageNo < activePages.Count - 1){
				pageNo += (twoPages) ? 2 : 1;
				if (flipNoise) AudioSource.PlayClipAtPoint(flipNoise, transform.position);
			}
		}
		else if (direction == "Back"){
			if (twoPages && pageNo > 1 || !twoPages && pageNo > 0){
				pageNo -= (twoPages) ? 2 : 1;
				if (flipNoise) AudioSource.PlayClipAtPoint(flipNoise, transform.position);
			}
		}
	}
	
	void OnGUI () {		
		if (isActive()){
			Rect leftBox;
			Rect rightBox;
			if(twoPages){
				Rect backgroundLocation = new Rect (Screen.width/2 - pageWidth, Screen.height/2 - pageHeight/2, pageWidth * 2, pageHeight);
				leftBox = new Rect (backgroundLocation.x, backgroundLocation.y, pageWidth, pageHeight);
				rightBox = new Rect (backgroundLocation.x + backgroundLocation.width/2, backgroundLocation.y, pageWidth, pageHeight);
				GUI.Label(backgroundLocation, "", backgroundStyle);
				if (pageNo - 1 > 0 && pageNo - 1 < activePages.Count)
					PrintPage(leftBox, pageNo - 1);
				if (pageNo >= 0 && pageNo < activePages.Count - 1)
					PrintPage(rightBox, pageNo);
				GUI.Label(backgroundLocation, "", foregroundStyle);
			}
			else{
				Rect pageLocation = new Rect (Screen.width/2 - pageWidth/2, Screen.height/2 - pageHeight/2, pageWidth, pageHeight);
				GUI.Label(pageLocation, "", backgroundStyle);
				PrintPage (pageLocation, pageNo);	
				GUI.Label(pageLocation, "", foregroundStyle);
				leftBox = new Rect(pageLocation.x, pageLocation.y, pageWidth/2, pageHeight);	
				rightBox = new Rect(pageLocation.x + pageWidth/2, pageLocation.y, pageWidth/2, pageHeight);
			}
			Vector2 mousePos = new Vector2(Input.mousePosition.x,Screen.height-Input.mousePosition.y);
			/*if (useCursor){
				GUI.Label(new Rect(mousePos.x, mousePos.y, 30, 30), cursor);
				Cursor.visible = false;
			}*/
			if(Event.current.type == EventType.MouseDown && Event.current.button == 0){
				if (leftBox.Contains(mousePos))
					TurnPage("Back");
				else if (rightBox.Contains(mousePos))
					TurnPage("Forward");
				else
					Close();
			}
		}
	}
	
	void PrintPage (Rect location, int number){	
		if (activePages.Count > number){
			GUI.Label(location, "", pageStyle);
			float x = location.x + (location.width - pageWidth * contentSpace)/2;
			float y = location.y + (location.height - pageHeight * contentSpace)/2;
			Rect contentBox = new Rect(x, y, pageWidth * contentSpace, pageHeight * contentSpace);
			GUI.BeginGroup(contentBox);
			int relativeHeight = 0;
			foreach(PageObject1 pageObject in activePages[number].Objects){
				Rect newLocation = new Rect(0,relativeHeight, contentBox.width,0);
				relativeHeight += (int)pageObject.height;
				if (pageObject.type == PageObject1.Type.Text)
					GUI.Label (newLocation, pageObject.text, pageObject.style);
				else if (pageObject.type == PageObject1.Type.Title)
					GUI.Label (newLocation, pageObject.text, pageObject.style);
				else if (pageObject.type == PageObject1.Type.List)
					GUI.Label (newLocation, " - " + pageObject.text, pageObject.style);
				else if (pageObject.type == PageObject1.Type.Picture)
					GUI.Label(newLocation, pageObject.picture, pageObject.style);
			}				
			GUI.EndGroup();
			if (isNumbered && activePages[number].showNum){
				Rect pageNum = new Rect (location.xMax - location.width/2, location.yMax - location.height/20, 0, 0);
				GUI.Label (pageNum, "Page " + (number + 1), numberStyle);
			}
		}
	}
	//Returns true if book reader is in use
	public bool isActive(){
		return (activePages.Count > 0);
	}
	
	//Opens a given book at a given page number
	public void Open(Book book, int pageNumber){
		if (!isActive()){
			SetupBook(book, pageNumber);
			StartCoroutine("AddPages", book);
		}
	}
	
	//Setup global variables of book.
	void SetupBook(Book book, int pageNumber){
		twoPages = book.twoPages;
		pageNo = pageNumber;
		if (twoPages && pageNo % 2 != 0) pageNo --;
		pageWidth = Screen.width * book.pageWidth;
		pageHeight = pageWidth * book.pageHeight;
		contentSpace = book.contentArea;
		isNumbered = book.isNumbered;
		
		Texture2D page = book.pageTexture;
		Texture2D background = book.backgroundTexture;
		Texture2D foreground = book.foregroundTexture;
		
		numberStyle = new GUIStyle();
		numberStyle.normal.textColor = book.fontColor;
		numberStyle.alignment = TextAnchor.MiddleCenter;
		numberStyle.font = book.font;
		numberStyle.fontSize = Screen.width * numberSizeMultiplier * book.fontSize/1000;
		
		pageStyle = new GUIStyle();
		pageStyle.stretchHeight = true;
		pageStyle.normal.background = page;
		
		backgroundStyle = new GUIStyle();
		int overflow = (int)(pageWidth * book.backgroundArea - pageWidth);
		backgroundStyle.overflow = new RectOffset(overflow,overflow,overflow,overflow);
		backgroundStyle.stretchHeight = true;
		backgroundStyle.normal.background = background;
		
		foregroundStyle = new GUIStyle(backgroundStyle);
		foregroundStyle.normal.background = foreground;
		
		if (openNoise) AudioSource.PlayClipAtPoint(openNoise, transform.position);
	}
	
	//Adds pages based on a books textFile/s
	IEnumerator AddPages(Book book){
		Page1 currentPage;
		activePages.Add(currentPage = new Page1());
		foreach (string textFile in book.pages){ //**
			if (textFile != null){
				StringReader reader = new StringReader(textFile); 
				string txt;
				while ( (txt = reader.ReadLine()) != null ){
					string[] words = txt.Split(' ');							//split line into string commands
					if (words[0] == "/page")
						activePages.Add(currentPage = new Page1());
					else if (words[0] == "/nonum")
						currentPage.showNum = false;
					else{
						PageObject1 newObject = new PageObject1();
						switch (words[0]){					
							case "/title":
								for (int i = 1; i < words.Length; i++)
									newObject.text += words[i] + " ";
								newObject.type = PageObject1.Type.Title;
								newObject.style.normal.textColor = book.fontColor;
								newObject.style.alignment = TextAnchor.UpperCenter;
								newObject.style.wordWrap = true;
								newObject.style.font = book.font;
								newObject.style.fontSize = Screen.width * titleSizeMultiplier * book.fontSize/1000;
								newObject.height = newObject.style.CalcHeight(new GUIContent(newObject.text), pageWidth * contentSpace);
								break;
							case "/list":
								for (int i = 1; i < words.Length; i++)
									newObject.text += words[i] + " ";
								newObject.type = PageObject1.Type.List;
								newObject.style.normal.textColor = book.fontColor;
								newObject.style.font = book.font;
								newObject.style.fontSize = Screen.width * listSizeMultiplier * book.fontSize/1000;
								newObject.height = newObject.style.CalcHeight(new GUIContent(newObject.text), pageWidth * contentSpace);
								break;
							case "/image":
								if (words.Length >= 2 && Resources.Load<Texture2D>(words[1])){
									newObject.picture = Resources.Load<Texture2D>(words[1]);
									newObject.type = PageObject1.Type.Picture;
									newObject.style.alignment = TextAnchor.MiddleCenter;
									int percent = 100;
									for (int i = 2; i < words.Length; i++){
										if (words[i] == "left") 
											newObject.style.alignment = TextAnchor.MiddleLeft;
										else if (words[i] == "right") 
											newObject.style.alignment = TextAnchor.MiddleRight;
										if (int.TryParse(words[i], out percent)){}	
									}
									newObject.style.fixedHeight = pageHeight * contentSpace/2 * percent/100;
									newObject.height = newObject.style.CalcHeight(new GUIContent(newObject.picture), pageWidth * contentSpace);
								}
								break; 	 					
							default:
								newObject.type = PageObject1.Type.Text;
								newObject.style.normal.textColor = book.fontColor; 
								newObject.style.wordWrap = true;	
								newObject.style.font = book.font;
								newObject.style.richText = true;
								newObject.style.fontSize = Screen.width * textSizeMultiplier * book.fontSize/1000;
								GUIStyle textStyle = newObject.style;
								foreach (string word in words){
									float tempageHeight = newObject.style.CalcHeight(new GUIContent(newObject.text + word), pageWidth * contentSpace);	
									if (tempageHeight + currentPage.height > pageHeight * contentSpace){
										currentPage.Objects.Add(newObject);
										currentPage.height += newObject.height;
										activePages.Add(currentPage = new Page1());
										newObject = new PageObject1();
										newObject.type = PageObject1.Type.Text;
										newObject.style = textStyle;
									}
									newObject.text += word + " ";
									newObject.height = tempageHeight;
								}
								break;
						}
						if (newObject.height + currentPage.height > pageHeight * contentSpace)
							activePages.Add(currentPage = new Page1());
						currentPage.Objects.Add(newObject);
						currentPage.height += newObject.height;	
					}
					yield return 0;
				}
			}
		}
		if (twoPages && activePages.Count % 2 != 0)
			activePages.Add(new Page1());
		pageNo = Mathf.Clamp(pageNo, 0, activePages.Count - ((twoPages) ? 0 : 1));
	}
	
	//Closes current book
	public void Close(){
		StopCoroutine("AddPages");
		StartCoroutine("WaitClose");
	}
	
	IEnumerator WaitClose() {
		yield return 0;
		activePages = new List<Page1>();
		if (closeNoise) AudioSource.PlayClipAtPoint(closeNoise, transform.position);
	}
}

[System.Serializable]
public class Page1 {
	public List<PageObject1> Objects = new List<PageObject1>();
	public bool showNum = true;
	public float height;
}

[System.Serializable]
public class PageObject1 {
	public enum Type {Title, Text, List, Picture};
	public float height;
	public Type type;
	public string text;
	public GUIStyle style = new GUIStyle();
	public Texture2D picture;
}