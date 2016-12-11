using UnityEngine;
using System.Collections;

public class target : MonoBehaviour {
	public int health = 50;
	public bool willMove = false;
	public float range = 5; 
	 
	public void Update (){
		if (health <= 0) {
			this.gameObject.SetActive (false);
			data.instance.IncreaseTargetsDestroyed();
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
