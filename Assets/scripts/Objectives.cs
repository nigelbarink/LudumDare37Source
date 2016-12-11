using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
public class Objectives : MonoBehaviour {
	public GameObject objectivesScreen;
	public GameObject Panel;
	public GameObject Scroll_button;
	public GameObject door;
	public GameObject room;
	public GameObject ghostwalls;

	public List<GameObject> locks;
	public List<GameObject> broken_locks;
	public List<number> lamp;

	public Text missionCounter;
	public Text[] missions;

	public int lamps_repaired;
	public int targets_shot ;
	public int activemission = -1; 

	public Timer clock;

	int lastindex = 0;
	bool hasscroll = false;
	int buttons_pushed = 0;
	Player_Controller player;
	Action  click ;






	public void Start(){
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Controller> ();

		GameObject[] units = GameObject.FindGameObjectsWithTag ("lock");
		GameObject.Find ("ghost walls").SetActive (false);
		foreach (GameObject unit in units) {
			locks.Add (unit);
		}

		locks.Add (GameObject.FindGameObjectWithTag("speciallock"));
		units = GameObject.FindGameObjectsWithTag ("pickup");
		foreach (GameObject unit in units) {
			if (unit.GetComponentInChildren<number> ()) {
				lamp.Add (unit.GetComponentInChildren<number>());
			}
		}
	}
	public int get_activemission(){
		return activemission;
	}

	public void getScroll (){
		hasscroll = true;
		objectivesScreen.SetActive (hasscroll);
		activemission++;

	}

	public void punish (){
		if (broken_locks.Count >= 1) {
			GameObject randomlock = broken_locks [UnityEngine.Random.Range (0, broken_locks.Count  )];
			randomlock.SetActive (true);
			if (lastindex == 0) {
				lastindex = 6; 
			}
			lastindex++;
			missions [lastindex].gameObject.SetActive (true);
			
			broken_locks.Remove (randomlock);
			locks.Add (randomlock);


		} else {
			clock.lesser_time (UnityEngine.Random.Range(10 , 60));
		}
			
	}

	public void  setObjectiveDone(){
		//Debug.Log (missions.Length);
		if (  missions.Length < activemission ){
			Debug.LogError ("the mission text does not exist for teh finshed mission. Mission " + activemission.ToString() + " failed vink off!");
			return;
		}
		missions[activemission].text += "<color=green>V</color>";
		activemission++;
	}
	public void Update (){
		missionCounter.text = "Objectives done: <color=red>" + activemission + "/" + "14</color>";
		if (objectivesScreen.activeSelf == true) {
			Scroll_button.SetActive (!hasscroll);
		} else {
			Scroll_button.SetActive (hasscroll);
		}
		if (activemission == 0) {
			ghostwalls.SetActive (true);
		}
		if (activemission == 10) {
			clock.add_timer ();
		}

		//Debug.Log (room.transform.rotation.eulerAngles );
		if (room.transform.rotation == Quaternion.Euler(new Vector3 (0, 90.00001f, 0)) && activemission == 0) {
			setObjectiveDone ();
			break_lock ();
			ghostwalls.SetActive (false);
		}
		if (lamps_repaired == 3) {
			setObjectiveDone ();
			break_lock();
			lamps_repaired = 0;
		}
		if (targets_shot == 4) {
			setObjectiveDone ();
			break_lock();
			targets_shot = 0;
		}

		if (buttons_pushed == 4 ){
			setObjectiveDone ();
			break_lock ();
			buttons_pushed = 0;
		}

		if (locks.Count == 0 ) {
		// the player wins and can finally leave the room !
			door.SetActive (false);
		}
	}

	public void turnRoom(Vector3 rotate){
		Vector3 rot = room.transform.rotation.eulerAngles;
		rot += rotate;
		room.transform.rotation =  Quaternion.Euler(rot);
		Debug.Log ((room.transform.rotation == Quaternion.Euler(new Vector3 (0,270,0))).ToString());
	} 
	public void break_lock(){
		GameObject selected = locks [UnityEngine.Random.Range (0, locks.Count )];
		if (selected.tag == "speciallock") {
			break_lock ();
			return;
		} 
		selected.SetActive (false);
		locks.Remove (selected);
		broken_locks.Add (selected);
		Debug.Log ("Broken lock added !");
	}

	public void  OnUserInteract(Collider other ){
		switch (activemission) {
		case(-1):
			break;
		case(0):
			if (other.name == "knob") {
				Panel.GetComponent<Text> ().text = "<color=green>[E] or [Q]</color>Turn room";
				Panel.SetActive (true);
				if (Input.GetKeyDown (KeyCode.E)) {
					turnRoom (new Vector3(0,10,0));
				}
				if (Input.GetKeyDown (KeyCode.Q)) {
					turnRoom (new Vector3(0,-5,0));
				}
			}
			break;
		case(1):
			if (other.name.Contains("lamp") ) {
				Panel.GetComponent<Text> ().text = "<color=green>[E]</color>destroy Lamp color";
				Panel.SetActive (true);
					if (Input.GetKeyDown (KeyCode.E)) {
						if (other.name.Contains("broken")){
						setObjectiveDone ();
						break_lock ();	
						Destroy(other.gameObject);
							return;
						}
						punish();
					}
				} 
			break;
		case(2):
				if (other.name == "lamp") {
					Panel.GetComponent<Text> ().text = "<color=green>[E]</color>Change Lamp voltage";
					Panel.SetActive (true);
					if (Input.GetKeyUp (KeyCode.E)) {
						other.GetComponent<number> ().change ();
					}
				}
			break;
		case(3):
				if ( other.transform.name.Contains ("button") ){
					Panel.GetComponent<Text> ().text = "<color=green>[E]</color>push Buttons";
					Panel.SetActive (true);
					if (Input.GetKeyDown(KeyCode.E)) {
						other.transform.parent.GetComponent<Machin> ().typein(other);
					}
				}
			break;
		case(4):
				if (other.name.Contains("button") ) {
					Panel.GetComponent<Text> ().text = "<color=green>[E]</color>push Buttons";
					Panel.SetActive (true);
					if (Input.GetKeyDown(KeyCode.E)) {
						other.transform.parent.GetComponent<Machin> ().typein(other);
					}
				}
			break;
		case(5):
				if (other.name.Contains("button")) {
					Panel.GetComponent<Text> ().text = "<color=green>[E]</color>push Buttons";
					Panel.SetActive (true);
					if (Input.GetKeyDown(KeyCode.E)) {
						other.transform.parent.GetComponent<Machin> ().typein(other);
					}
				}
			break;
		case(6):
				if (other.name.Contains("Key") ) {
					Panel.GetComponent<Text> ().text = "<color=green>[E]</color>Pickup " + other.name;
					Panel.SetActive (true);
					if (Input.GetKeyDown(KeyCode.E)) {
						other.gameObject.SetActive (false);
						player.haskey = true;
					}
				}
			break;
		case(7):
			if (other.name.Contains("Box") ) {
				Panel.GetComponent<Text> ().text = "<color=green>[E]</color>Pickup " + other.name;
				Panel.SetActive (true);
				if (Input.GetKeyDown(KeyCode.E)) {
					other.gameObject.SetActive (false);
					setObjectiveDone ();
					break_lock ();
				}
			}
			break;
			/// 8 and 9 get handled by the player as they have to shooot something up!
		case(11):
			if (other.name.Contains("pushme") ) {
				Panel.GetComponent<Text> ().text = "<color=green>[E]</color>Push button " + other.name;
				Panel.SetActive (true);
				if (Input.GetKeyDown(KeyCode.E)) {
					other.gameObject.SetActive (false);
					buttons_pushed++;
				}
			}
			break;
		case(13):
			// activate zombies 
			Zombie[] zombies = GameObject.Find("zombie horde").GetComponentsInChildren<Zombie>();
			foreach (Zombie z in zombies ){
				z.Activate();
			}
				
			break;

		default :
			// the current mission doesn't excist so just don't do anything 
			//actually do et me know for now !
			Debug.Log("The mission doesn't excist in the current context !");
			break;
		}
		}}

