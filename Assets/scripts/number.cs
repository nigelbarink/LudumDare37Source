using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class number : MonoBehaviour {
	public float max_volt;
	public Text  Display ;
	public int voltage = 0;

	public void change(){
		voltage++;
		voltage =(int) Mathf.Clamp (voltage, 0, max_volt);
	}
	void Update () {
		if (voltage == max_volt) {
			data.instance.IncreaseLampsRepaired();
			Display.text = "Lamp Repaired ";
			GetComponentInChildren<Light> ().intensity = 4f;
			this.enabled = false;
			return;
		}
		Display.text = "Lamp's voltage: " + voltage+ " / " + max_volt;
	}
}
