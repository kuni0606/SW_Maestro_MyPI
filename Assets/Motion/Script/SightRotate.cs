using UnityEngine;
using System.Collections;
using Leap;

public class SightRotate : MonoBehaviour {
	/// <summary>
	/// Fist Radius
	/// </summary>
	public float defaultRadius = 50;
	
	/// <summary>
	/// Move Speed
	/// </summary>
	public float moveSpeed = 1.0f;
	
	/// <summary>
	/// MyMenu
	/// </summary>
	public GameObject mymenu;
	public MainMenuControl menuController;
	public GameObject updownBtn;
	public GameObject gliderObj;
	public GameObject bikeObj;
	public GameObject browerUI;
	public OVRManager OVRManagerController = null;
	
	protected OVRPlayerController OVRController = null;
	protected OVRCameraRig CameraController = null;
	protected HandController HandController = null;
	protected Controller LeapController = null;

	float boatSignal = 0;
	bool isInBoat = false, isInGlider=false, isInBike=false;
	
	private float speed = 0.0f;
	CharacterController character;
	private float angle=0.0f, udangle=0.0f;

	// Use this for initialization
	public void setBoat(bool boat){
		isInBoat = boat;
	}
	public float getBoatSignal(){
		return boatSignal;
	}
	public void setBike(bool bike){
		isInBike = bike;
	}
	public void setGlider(bool glider, GameObject gliderObject){
		isInGlider = glider;
		if (glider) {
			udangle = 5f;
			gliderObj = gliderObject;
			gliderObj.transform.Rotate (Vector3.right, udangle);
		}
	}

	public void setActiveUpdownBtn(bool activate){
		if (activate && !isInBoat && !isInGlider && !isInBike) {
			updownBtn.SetActive (true);
		} else {
			updownBtn.SetActive(false);
		}
	}

	void Start () {
		OVRController = gameObject.GetComponent<OVRPlayerController> ();
		
		if (OVRController == null) {
			Debug.LogWarning("OVRPlayerController: No CharacterController attached.");
		}
		
		OVRCameraRig[] CameraControllers;
		CameraControllers = gameObject.GetComponentsInChildren<OVRCameraRig>();
		OVRManagerController = gameObject.GetComponentInChildren<OVRManager> ();
		
		if(CameraControllers.Length == 0)
			Debug.LogWarning("OVRPlayerController: No OVRCameraRig attached.");
		else if (CameraControllers.Length > 1)
			Debug.LogWarning("OVRPlayerController: More then 1 OVRCameraRig attached.");
		else
			CameraController = CameraControllers[0];
		
		LeapController = new Controller ();
		LeapController.EnableGesture (Gesture.GestureType.TYPE_SWIPE);
		LeapController.EnableGesture (Gesture.GestureType.TYPE_CIRCLE);
		character = GetComponent<CharacterController> ();
//		mymenu.SetActive (false);
		menuController = mymenu.GetComponent<MainMenuControl> ();
	}
	
	// Update is called once per frame
	void Update () {
		Frame frame = null;
		GestureList gestures = null;
		HandList hands = null;
		Hand leftHand = null, rightHand = null;
		bool goForward = false, goLeft = false, goRight = false;
		bool gl_Up = false, gl_Down = false, gl_Left = false, gl_Right = false;
		int c_swipe = 0, c_circle = 0;
		float rotateSpeed = 0.0f;
		frame = LeapController.Frame ();
		gestures = frame.Gestures ();
		hands = frame.Hands;
		for (int i=0; i<hands.Count; i++) {
			if (hands [i].IsLeft) {
				leftHand = hands [i];
			} else if (hands [i].IsRight) {
				rightHand = hands [i];
			}
		}
//		if (leftHand != null && rightHand != null) {
//			if (leftHand.IsValid && rightHand.IsValid) {
//				if (leftHand.SphereRadius < defaultRadius && rightHand.SphereRadius < defaultRadius) {
//					goForward = true;
//					
//					if (leftHand.PalmPosition.y - rightHand.PalmPosition.y < -10){
//						goLeft = true;
//						rotateSpeed = (rightHand.PalmPosition.y - leftHand.PalmPosition.y - 10) / 100;
//					}
//					if (rightHand.PalmPosition.y - leftHand.PalmPosition.y < -10){
//						goRight = true;
//						rotateSpeed = (leftHand.PalmPosition.y - rightHand.PalmPosition.y - 10) / 100;
//					}
//				}
//			}
//		}z
//		if (leftHand != null && leftHand.IsValid && !goForward) {
//			if (leftHand.SphereRadius > defaultRadius + 30 && leftHand.PalmNormal.y < -0.5 && leftHand.PalmNormal.y > -0.9){
//				mymenu.SetActive(true);
//			}else{
//				mymenu.SetActive(false);
//			}
//		}
//		
		if (Input.GetKeyDown (KeyCode.N)) {
			menuController.setActiveMenu ();
		}
		if (Input.GetKeyDown(KeyCode.X)){
			OVRManagerController.ReCenter();
		}
		for (int i=0; i<gestures.Count; i++) {
			Gesture gesture = gestures [i];
			SwipeGesture swipe = new SwipeGesture (gesture);
			CircleGesture circle = new CircleGesture (gesture);
			if (swipe.IsValid) {
				c_swipe++;
			} else if (circle.IsValid) {
				c_circle++;
			}
			
			if (c_swipe >= 2) {
				if (swipe.Hands.Rightmost.IsValid) {
					if (swipe.Direction.Normalized.z > 0.6 && browerUI.activeInHierarchy==false) {
						menuController.setActiveMenu ();
					}
				}
				break;
			} else if (c_circle > 2) {
				break;
			}
		}
		if (rightHand != null && leftHand != null && leftHand.IsValid && rightHand.IsValid) {
			if (!isInGlider && !isInBoat && !isInBike && rightHand.SphereRadius < defaultRadius) {
				if (rightHand.PalmPosition.x > -50)
					goLeft = true;
				else if (rightHand.PalmPosition.x < -80)
					goRight = true;
			} else if ((isInBike || isInBoat) && leftHand.SphereRadius < defaultRadius && rightHand.SphereRadius < defaultRadius) {
				float lhpy = leftHand.PalmPosition.z;
				float rhpy = rightHand.PalmPosition.z;
				float diff = lhpy - rhpy;
				if (diff > 20) {
					boatSignal = diff - 20;
				} else if (diff < -20) {
					boatSignal = diff + 20;
				} else {
					boatSignal = 0;
				}
				boatSignal *= -1;
			} else if (isInGlider && leftHand.SphereRadius < defaultRadius && rightHand.SphereRadius < defaultRadius) {
				Vector LHPP = leftHand.PalmPosition, RHPP = rightHand.PalmPosition;
				
				if (LHPP.z - RHPP.z < -10) {
					gl_Left = true;
					rotateSpeed = (RHPP.z - LHPP.z - 10) / 100;
				} else if (RHPP.z - LHPP.z < -10) {
					gl_Right = true;
					rotateSpeed = (LHPP.z - RHPP.z - 10) / 100;
				}
			}
		}


		if (isInGlider) {
			if (gl_Left && angle > -50) {
				//			Quaternion toRotation = transform.rotation * Quaternion.LookRotation (Vector3.back);
				//			transform.rotation = Quaternion.Slerp (transform.rotation, 
				//			                                      toRotation, Time.fixedDeltaTime * rotateSpeed);
				gliderObj.transform.Rotate (Vector3.forward, -0.3f * rotateSpeed);		//Roll to left
				angle += -0.3f * rotateSpeed;
			} else if (gl_Right && angle < 50) {
				//			Quaternion toRotation = transform.rotation * Quaternion.LookRotation (Vector3.forward);
				//			transform.rotation = Quaternion.Slerp (transform.rotation, 
				//			                                      toRotation, Time.fixedDeltaTime * rotateSpeed);
				gliderObj.transform.Rotate (Vector3.forward, 0.3f * rotateSpeed);		//Roll to right
				angle += 0.3f * rotateSpeed;
			} 
			gliderObj.transform.Rotate (Vector3.up, -angle * 0.01f, Space.World);			//LEFT,RIGHT Rotation
			gliderObj.transform.Translate (Vector3.forward * moveSpeed);					//GO Forward
			gliderObj.transform.Translate (Vector3.down * 0.05f, Space.World);			//Gravity
		} else if (!isInBoat&&!isInBike) {
			if (goLeft) {
				Quaternion toRotation = character.transform.rotation * Quaternion.LookRotation (Vector3.left);
				character.transform.rotation = Quaternion.Slerp (character.transform.rotation, 
				                                                toRotation, Time.fixedDeltaTime * 0.75f);
			}
			if (goRight) {
				Quaternion toRotation = character.transform.rotation * Quaternion.LookRotation (Vector3.right);
				character.transform.rotation = Quaternion.Slerp (character.transform.rotation, 
				                                                 toRotation, Time.fixedDeltaTime * 0.75f);
			}
		}
	}
}
