using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class Timer : MonoBehaviour {
	public Text Show_Time;
	public GameObject gameover ;
	public Objectives manager ;
	public float time = 180f;
	float curr_time;
	bool start =  false;
	bool mission =false;
	float small_time = 30;

	public void TimerStart(){
		curr_time = time; 
		start = true;
	}
	void Update () {
		curr_time -= Time.deltaTime;
		if (start)
			Show_Time.text = "Time Left: " + Mathf.Floor ((curr_time)); 
		if (curr_time <= 0 && start)  {
			Time.timeScale = 0;
			gameover.SetActive (true);
			Cursor.visible = true;
		}
		if (mission) {
			small_time -= Time.deltaTime;
		}

		if (small_time <= 0) {
			mission = false;
			manager.break_lock ();
			manager.setObjectiveDone ();
		}
	}

	public void lesser_time(float time){
		Debug.Log ("you just lost: " +  time + " seconds");
		curr_time -= time;
	}


	public void add_timer(){
		time += 20;
		mission = true;
	}
}
