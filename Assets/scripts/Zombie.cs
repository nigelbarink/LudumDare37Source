using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour {
	bool activated = false  ;
	void Update () {
		if(activated){
			// WALK
			Vector3 pos = transform.position;
			pos += Vector3.forward * Time.deltaTime;
			transform.position = pos ;
		}
	}

	void OnColllisionEnter(Collider other){
		if (other.name == "Player") {
		// The player lost!!!
			// show lost screen 
		}
	}
	public void Activate (){
		activated = true;
	}
}
