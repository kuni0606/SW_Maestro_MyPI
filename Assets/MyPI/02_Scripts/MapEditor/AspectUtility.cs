using UnityEngine;

public class AspectUtility : MonoBehaviour {
	
	private const float ASPECT_1280_720 = 1280f / 720f;
	
	public static Camera uiCamera;
	public static Camera camera;

	public Camera _uiCamera;
	public Camera _camera;

	
	void Awake () {
		uiCamera = _uiCamera;
		camera = _camera;
//		if (!cam) {
//			cam = Camera.main;
//		}
//		if (!cam) {
//			Debug.LogError ("No camera available");
//			return;
//		}
//		wantedAspectRatio = _wantedAspectRatio;
		SetCamera();
	}
	
	public static void SetCamera () {
		float currentAspectRatio = (float)Screen.width / Screen.height;
		// If the current aspect ratio is already approximately equal to the desired aspect ratio,
		// use a full-screen Rect (in case it was set to something else previously)
		if ((int)(currentAspectRatio * 100) == (int)(ASPECT_1280_720 * 100)) {
			uiCamera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
			return;
		}

		if (currentAspectRatio > ASPECT_1280_720) {
			float inset = 1f - ASPECT_1280_720/currentAspectRatio;
			uiCamera.rect = new Rect(inset * 0.5f, 0.0f, 1f - inset, 1f);
		} else {
			float inset = 1f - currentAspectRatio/ASPECT_1280_720;
			uiCamera.rect = new Rect(0f, inset * 0.5f, 1f, 1f - inset);
		}
	}
	
	public static int screenHeight {
		get {
			return (int)(Screen.height * uiCamera.rect.height);
		}
	}
	
	public static int screenWidth {
		get {
			return (int)(Screen.width * uiCamera.rect.width);
		}
	}
	
	public static int xOffset {
		get {
			return (int)(Screen.width * uiCamera.rect.x);
		}
	}
	
	public static int yOffset {
		get {
			return (int)(Screen.height * uiCamera.rect.y);
		}
	}
	
	public static Rect screenRect {
		get {
			return new Rect(uiCamera.rect.x * Screen.width, uiCamera.rect.y * Screen.height, uiCamera.rect.width * Screen.width, uiCamera.rect.height * Screen.height);
		}
	}
	
	public static Vector3 mousePosition {
		get {
			Vector3 mousePos = Input.mousePosition;
			mousePos.y -= (int)(uiCamera.rect.y * Screen.height);
			mousePos.x -= (int)(uiCamera.rect.x * Screen.width);
			return mousePos;
		}
	}
	
	public static Vector2 guiMousePosition {
		get {
			Vector2 mousePos = Event.current.mousePosition;
			mousePos.y = Mathf.Clamp(mousePos.y, uiCamera.rect.y * Screen.height, uiCamera.rect.y * Screen.height + uiCamera.rect.height * Screen.height);
			mousePos.x = Mathf.Clamp(mousePos.x, uiCamera.rect.x * Screen.width, uiCamera.rect.x * Screen.width + uiCamera.rect.width * Screen.width);
			return mousePos;
		}
	}
}