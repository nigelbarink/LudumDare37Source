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
	public Text message;

	public List<GameObject> locks;
	public List<GameObject> broken_locks;
	public List<number> lamp;

	public Text missionCounter;
	public Text[] missions;

	public Animation Open_crate;

	public Timer clock;

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

	public void getScroll (){
		data.instance.PickedUpScroll ();
		objectivesScreen.SetActive (data.instance.HasScroll());
		data.instance.IncreaseMissionIndex ();
		Debug.Log (data.instance.GetActiveMissionIndex().ToString());
	}

	public void punish (){
		if (broken_locks.Count >= 1) {
			GameObject randomlock = broken_locks [UnityEngine.Random.Range (0, broken_locks.Count  )];
			randomlock.SetActive (true);
			
			broken_locks.Remove (randomlock);
			locks.Add (randomlock);

		} else {
			clock.lesser_time (UnityEngine.Random.Range(10 , 60));
		}
			
	}

	public void  setObjectiveDone(){
		//Debug.Log (missions.Length);
		if (  missions.Length < data.instance.GetActiveMissionIndex() ){
			Debug.LogError ("the mission text does not exist for the finshed mission. Mission " + data.instance.GetActiveMissionIndex().ToString() + " failed vink off!");
			return;
		}
		clock.IncreaseTime (10);
		missions[data.instance.GetActiveMissionIndex()].text += "<color=green>V</color>";
		data.instance.IncreaseMissionIndex();
	}
	public void Update (){
		
		if (data.instance == null) {
			Debug.LogError ("the instance is null");
			return;
		}
		// check and update some info about the game status 
		missionCounter.text = "Objectives done: <color=red>" +  data.instance.GetActiveMissionIndex().ToString() + "/" + "14</color>";
		if (objectivesScreen.activeSelf == true) {
			Scroll_button.SetActive (data.instance.HasScroll() == false );
		} else {
			Scroll_button.SetActive (data.instance.HasScroll());
		}
		if (data.instance.GetActiveMissionIndex() == 0) {
			ghostwalls.SetActive (true);
		}
		if (data.instance.GetActiveMissionIndex() == 10) {
			clock.add_timer ();
		}

		//Debug.Log (room.transform.rotation.eulerAngles );
		if (room.transform.rotation == Quaternion.Euler(new Vector3 (0, 90.00001f, 0)) && data.instance.GetActiveMissionIndex() == 0) {
			NextMission ();
			ghostwalls.SetActive (false);
		}
		if (data.instance.GetLampsRepaired() == 3) {
			NextMission ();
			data.instance.IncreaseLampsRepaired(-3);// this will set it to null! Since we do 3 + -3 which would result in 0 
		}
		if (data.instance.GetTargetsDestroyed() == 4) {
			NextMission ();
			data.instance.IncreaseTargetsDestroyed(-4); // this will set it to null!
		}

		if (data.instance.GetButtonsPushed() == 4 ){
			NextMission ();
			data.instance.IncreaseButtonsPushed(-4); // this will set it back to null 
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
		//Debug.Log ("Broken lock added !");
	}

	public void  OnUserInteract(Collider other ){
		switch (data.instance.GetActiveMissionIndex()) {
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
				Panel.GetComponent<Text> ().text = "<color=green>[E]</color>Open " + other.name;
				Panel.SetActive (true);
				if (Input.GetKeyDown(KeyCode.E)) {
					Open_crate.Play ();
					other.gameObject.SetActive (false);
					NextMission ();
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
					data.instance.IncreaseButtonsPushed();
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
		}

public void NextMission (){
		setObjectiveDone ();
		break_lock();
		message.text =  data.messages[data.instance.GetActiveMissionIndex()] != null  ? data.messages[data.instance.GetActiveMissionIndex()] : "Quickly there's only so much time!";
}
}