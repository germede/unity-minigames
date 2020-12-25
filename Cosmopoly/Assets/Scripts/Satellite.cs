using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite : MonoBehaviour {

	public float rotationSpeed = 1f;
    void Update () {

		transform.RotateAround (Vector3.zero, Vector3.up, rotationSpeed * Time.deltaTime);
		transform.LookAt (Vector3.zero);

    }

}
