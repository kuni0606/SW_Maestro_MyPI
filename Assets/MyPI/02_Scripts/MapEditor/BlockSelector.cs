using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Mypi.Block;

namespace Mypi {
	namespace MapEditor {
		public class BlockSelector : MonoBehaviour {

			public UIManager uiManager;
			public GameObject buttonPrefab;
			public RectTransform panel;
			public RectTransform contents;
			public RectTransform myTransform;
			public int width;
			public BlockCategory blockCategory;

			public List<Selectable> selectables;


			void Awake () {
				List<string> names = PoolManager.current.GetBlockNames (blockCategory);
			
				Vector2 currentPosition = Vector2.zero;
				int i = 0;
				foreach (string item in names) {
					GameObject go = Instantiate (buttonPrefab) as GameObject;
					go.transform.SetParent(contents, false);
					
					Button b = go.GetComponent<Button>();
					RectTransform rt = b.GetComponent<RectTransform>();

					Image image = go.transform.FindChild("Thumbnail").GetComponent<Image>();
					image.sprite = PoolManager.current.GetThumbnail(item);

					currentPosition.x = (i % width) * (rt.sizeDelta.x + 40f);
					rt.anchoredPosition = new Vector2((40f + rt.sizeDelta.x)*(width - 1) * -0.5f + currentPosition.x, currentPosition.y);
					currentPosition.y = -((i + 1) / width) * (5f + rt.sizeDelta.y);

					Text t = go.GetComponentInChildren<Text>();
					t.text = item;
					
					var ce = new Button.ButtonClickedEvent();
					ce.AddListener(()=>uiManager.SelectBlock(t.text));
					b.onClick = ce;

					i++;
				}
				
				contents.sizeDelta = new Vector2 (0f, -currentPosition.y);
			}
		}
	}
}