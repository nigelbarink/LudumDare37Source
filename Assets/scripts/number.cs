using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class number : MonoBehaviour {
	public float max_volt;
	public Text  Display ;
	public int voltage = 0;
	Objectives missions;
	public void Start(){
		missions = GameObject.Find ("Game_manager").GetComponent<Objectives> ();
	}
	public void change(){
		voltage++;
		voltage =(int) Mathf.Clamp (voltage, 0, max_volt);
	}
	void Update () {
		if (voltage == max_volt) {
			missions.lamps_repaired += 1;
			Display.text = "Lamp Repaired ";
			GetComponentInChildren<Light> ().intensity = 4f;
			this.enabled = false;
			return;
		}
		Display.text = "Lamp's voltage: " + voltage+ " / " + max_volt;
	}
}
