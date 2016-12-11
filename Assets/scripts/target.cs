using UnityEngine;
using System.Collections;

public class target : MonoBehaviour {
	public int health = 50;
	public bool willMove = false;
	public float range = 5; 
	Objectives mission;

	public void Start (){
		mission = GameObject.Find ("Game_manager").GetComponent<Objectives> ();
	}
	public void Update (){
		if (health <= 0) {
			this.gameObject.SetActive (false);
			mission.targets_shot++;
		}

		if (willMove) {
			//make it move back and forth
			Vector3 pos = transform.position;
			if (pos.x >= -range || pos.x <= range) {
				pos += new Vector3 (range, 0, 0) * Time.deltaTime;
			} else if (pos.x >= range || pos.x <= -range) {
				pos -= new Vector3 (range, 0, 0) * Time.deltaTime;
			}
		}
	}

	public void subtractHealth (){
		health = Mathf.FloorToInt(health * 0.25f);
	}
}
