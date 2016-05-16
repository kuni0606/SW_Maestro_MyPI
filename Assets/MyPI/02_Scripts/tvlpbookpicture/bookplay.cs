using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class bookplay : MonoBehaviour {
	
	public class Page {
		public int line1;
		public int line2;
	}

	public B_Browser book;
	public Text right, left;
	Page[] pagearr;
	int p;
	
	int cnt = 0;

	// Use this for initialization
	void Awake(){
		pagearr = new Page[1000];
		for (int i =0; i < pagearr.Length; i++)
			pagearr [i] = new Page ();
	}

	void Start () {

		cnt = 0;
		make ();
		p = 0;

		if (cnt == 1) {
			for (int i=pagearr[p].line1; i<= pagearr[p].line2; i++)
				right.text += book.bookline [i] + "\n";
		} else {
			for (int i=pagearr[p].line1; i<= pagearr[p].line2; i++)
				right.text += book.bookline [i] + "\n";

			for (int i=pagearr[p+1].line1; i<= pagearr[p+1].line2; i++)
				left.text += book.bookline [i] + "\n";
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit = new RaycastHit();
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			
			if (Physics.Raycast (ray, out hit)) {

				if(hit.collider.gameObject.name == "Text (1)")
				{
					p = p+2;
					Debug.Log (p);
					if(p<cnt)
					{
						right.text = "";
						left.text = "";

						if(p==cnt-1){
							for(int i=pagearr[p].line1; i<= pagearr[p].line2; i++)
								right.text += book.bookline[i] +"\n";
						}
						else{
							for(int i=pagearr[p].line1; i<= pagearr[p].line2; i++)
								right.text += book.bookline[i] +"\n";
							
							for(int i=pagearr[p+1].line1; i<= pagearr[p+1].line2; i++)
								left.text += book.bookline[i] +"\n";
						}
						
					}
					else
						p= p-2;
				}

				if(hit.collider.gameObject.name == "Text")
				{
					p = p-2;
					Debug.Log (p);
					if(p>=0){
						right.text = "";
						left.text = "";

						for(int i=pagearr[p].line1; i<= pagearr[p].line2; i++)
							right.text += book.bookline[i] +"\n";
						
						for(int i=pagearr[p+1].line1; i<= pagearr[p+1].line2; i++)
							left.text += book.bookline[i] +"\n";
					}
					else 
						p=p+2;
				}


			}
		}
		
	}

	void make(){
		for (int i=0; i<=book.l; i+=17) {
			if (i % 17 == 0) {	
				pagearr [cnt].line1 = i;
				if(i+16 < book.l)
					pagearr [cnt].line2 = i+16;
				else
					pagearr[cnt].line2 = book.l;
				cnt++;
			}
		}
		Debug.Log (cnt);
	}
}
