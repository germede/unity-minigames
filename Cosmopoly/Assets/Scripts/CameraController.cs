using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	public float moveSensivity = 10.0f;


	public float mouseDragSensivity = 500.0f;
	Vector3 previousMousePosition;

	public float zoomVelocity = 10.0f;
	public float mouseZoomSensivity = 3.0f;
	public float maxZoom = 2.0f;
	public float minZoom = 5.0f;
	public float actualZoom = 5.0f;
	public float currentZoom = 5.0f;



	void FixedUpdate ()
	{
	    // MOVE
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {

		}
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
			transform.RotateAround (Vector3.zero, Vector3.back, moveSensivity);
		}
		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
			transform.RotateAround (Vector3.zero, Vector3.left, moveSensivity);
		}
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
			transform.RotateAround (Vector3.zero, Vector3.forward, moveSensivity);
		}


		// DRAG
		Vector3 currentMousePosition = Input.mousePosition;
		if (Input.GetMouseButtonDown (2)) {
			previousMousePosition = currentMousePosition;
		}
		if (Input.GetMouseButton (2)) {
			Vector3 mouseDisplacement = currentMousePosition - previousMousePosition;

			mouseDisplacement.x /= Screen.width;
			mouseDisplacement.y /= Screen.height;

			Quaternion yaw = Quaternion.AngleAxis (mouseDisplacement.x * mouseDragSensivity, transform.up);
			Quaternion pitch = Quaternion.AngleAxis (mouseDisplacement.y * mouseDragSensivity, -transform.right);

			transform.localRotation = yaw * pitch * transform.localRotation;

			previousMousePosition = currentMousePosition;

		}
		transform.position = transform.forward * -currentZoom;




		// ZOOM
		float mouseWheelInput = Input.GetAxis ("Mouse ScrollWheel");
		if (mouseWheelInput != 0.0f) {
			actualZoom += mouseZoomSensivity * - mouseWheelInput;
			actualZoom = Mathf.Clamp (actualZoom, maxZoom, minZoom);
		}
		if (Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.KeypadPlus)) {
			actualZoom += mouseZoomSensivity*0.1f;
			actualZoom = Mathf.Clamp (actualZoom, maxZoom, minZoom);
		}
		if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus)) {
			actualZoom -= mouseZoomSensivity*0.1f;
			actualZoom = Mathf.Clamp (actualZoom, maxZoom, minZoom);
		}

		currentZoom = Mathf.Lerp (currentZoom, actualZoom, zoomVelocity*Time.deltaTime);

		//GetComponent<Camera> ().fieldOfView = currentZoom*10;
	}


}
