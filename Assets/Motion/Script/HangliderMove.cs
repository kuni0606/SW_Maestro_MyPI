using UnityEngine;
using System.Collections;
using Leap;
using System;
using UnityEngine.UI;

public class HangliderMove : MonoBehaviour {
	/// <summary>
	/// Fist Radius
	/// </summary>
	public float defaultRadius = 50;
	
	/// <summary>
	/// Move Speed
	/// </summary>
	public float moveSpeed = 1f;

	public GameObject gliderCam;

	protected OVRPlayerController OVRController = null;
	protected OVRCameraRig CameraController = null;
	protected HandController HandController = null;
	protected Controller LeapController = null;

	private float angle=0.0f, udangle=0.0f;

	// Use this for initialization
	void Start () {
		OVRController = gameObject.GetComponent<OVRPlayerController> ();
		
		if (OVRController == null) {
			Debug.LogWarning("OVRPlayerController: No CharacterController attached.");
		}
		
		OVRCameraRig[] CameraControllers;
		CameraControllers = gameObject.GetComponentsInChildren<OVRCameraRig>();
		
		if(CameraControllers.Length == 0)
			Debug.LogWarning("OVRPlayerController: No OVRCameraRig attached.");
		else if (CameraControllers.Length > 1)
			Debug.LogWarning("OVRPlayerController: More then 1 OVRCameraRig attached.");
		else
			CameraController = CameraControllers[0];
		
		LeapController = new Controller ();
		udangle = 5f;
		transform.Rotate (Vector3.right, udangle);
	}
	
	// Update is called once per frame
	void Update () {
		Frame frame = null;
		GestureList gestures = null;
		HandList hands = null;
		Hand leftHand = null, rightHand = null;
		bool goDown = false, goLeft = false, goRight = false, goUp = false;
		float rotateSpeed = 0.0f;
		frame = LeapController.Frame ();
		gestures = frame.Gestures();
		hands = frame.Hands;
		for (int i=0; i<hands.Count; i++) {
			if (hands[i].IsLeft){
				leftHand = hands[i];
			}else if (hands[i].IsRight){
				rightHand = hands[i];
			}
		}

		if (leftHand != null && rightHand != null) {
			if (leftHand.IsValid && rightHand.IsValid) {
				if (leftHand.SphereRadius < defaultRadius && rightHand.SphereRadius < defaultRadius) {
					Vector LHPP = leftHand.PalmPosition, RHPP = rightHand.PalmPosition;
					
					if (LHPP.z - RHPP.z < -10){
						goLeft = true;
						rotateSpeed = (RHPP.z - LHPP.z - 10) / 100;
					}else if (RHPP.z - LHPP.z < -10){
						goRight = true;
						rotateSpeed = (LHPP.z - RHPP.z - 10) / 100;
					}if (LHPP.z > 40 && RHPP.z > 40){
						goDown = true;
						rotateSpeed = ((LHPP.z + RHPP.z) / 2 / 100);
					}else if (LHPP.z < -40 && RHPP.z < -40){
						goUp  = true;
						rotateSpeed = ((LHPP.z + RHPP.z) / 2 / 100);
					}
				}
			}
		}

		if (goDown && udangle < 40) {
			transform.Rotate(Vector3.right, 0.3f * rotateSpeed);
			udangle += 0.3f * rotateSpeed;
		}
		if (goUp && udangle > 5) {
			transform.Rotate(Vector3.right, 0.3f * rotateSpeed);
			udangle += 0.3f * rotateSpeed;
		}
		if (goLeft && angle>-50) {
//			Quaternion toRotation = transform.rotation * Quaternion.LookRotation (Vector3.back);
//			transform.rotation = Quaternion.Slerp (transform.rotation, 
//			                                      toRotation, Time.fixedDeltaTime * rotateSpeed);
			transform.Rotate (Vector3.forward,-0.3f * rotateSpeed);		//Roll to left
			angle += -0.3f * rotateSpeed;
		} else if (goRight && angle<50) {
//			Quaternion toRotation = transform.rotation * Quaternion.LookRotation (Vector3.forward);
//			transform.rotation = Quaternion.Slerp (transform.rotation, 
//			                                      toRotation, Time.fixedDeltaTime * rotateSpeed);
			transform.Rotate (Vector3.forward,0.3f * rotateSpeed);		//Roll to right
			angle += 0.3f * rotateSpeed;
		} 
		transform.Rotate (Vector3.up, -angle*0.01f,Space.World);			//LEFT,RIGHT Rotation
		transform.Translate (Vector3.forward * moveSpeed);					//GO Forward
		transform.Translate (Vector3.down * 0.05f, Space.World);			//Gravity
		gliderCam.transform.position = transform.position - 25 * transform.forward;
		gliderCam.transform.LookAt (transform.position);
	}
}