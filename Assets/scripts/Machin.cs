using UnityEngine;
using System.Collections;

public class Machin : MonoBehaviour {
	public int cursorPlace = 0; 
	public string[] Button_names = { "yellow" }; 
	Objectives mission ;

	public void Start (){
		mission = GameObject.Find ("Game_manager").GetComponent<Objectives>();
	} 


	public void Update (){
		if (cursorPlace == Button_names.Length){
			mission.setObjectiveDone ();
			mission.break_lock ();
			cursorPlace = 0;
			this.enabled = false;
		}

	}
	public void typein (Collider button){
		//Debug.Log ("button name is: " + button.name);
		if (button.name.Contains ("correct") && button.name.Contains(Button_names[cursorPlace])) {
			//Debug.Log ("Hey it worked !");
			cursorPlace++;
		}else if (button.name.Contains("wrong")){
			cursorPlace = 0;
			mission.punish ();
		}
	}
}
