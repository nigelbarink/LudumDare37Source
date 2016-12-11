using UnityEngine;
using System.Collections;

public class WinScreen : MonoBehaviour {
	public GameObject target;
	public void OnTriggerEnter( Collider other){
		if(other.gameObject.tag == "Player"){
			Debug.Log ("Ran!");
			other.GetComponent<Player_Controller> ().enabled = false;
			other.gameObject.GetComponent<Rigidbody> ().isKinematic = true;
			Camera.main.enabled = false;
			target.SetActive (true);
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
	}

}
