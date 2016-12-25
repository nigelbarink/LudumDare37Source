using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player_Controller : MonoBehaviour {

	public float pwr = 5f;
	public float sens = 1.2f;
	public float Vert_sens = 1.2f;

	//FX
	public ParticleSystem scroll_particle;
	public AudioSource Scrollpick;
	public LineRenderer BulletTrace ;
	public ParticleSystem shotFX;
	public ParticleSystem Bigexplosion1;
	public ParticleSystem lockExplosion;


	int Mode;
	public enum ControlMode { normal, shooting }
	bool paused = false;
	public bool haskey = false;

	Vector3 speed = new Vector3 (0, 0, 0);
	CharacterController cc;
	Objectives mission;

	public void Start () {
		mission = GameObject.Find ("Game_manager").GetComponent<Objectives> ();
		cc = this.GetComponent<CharacterController> ();
	}
	
	public void Update () {
		if (paused == false) {
			Cursor.visible = false;

		}
		else {
			Cursor.visible = true;

		}
		check_movement ();
		check_crosshair ();
		if (Input.GetKey(KeyCode.LeftControl)){
			Cursor.visible = true;

		}

}

	public void check_movement(){
		Vector3 rot =  new Vector3 (-Input.GetAxis("Mouse Y") * sens, Input.GetAxis ("Mouse X")* Vert_sens , 0);
		transform.rotation = Quaternion.Euler (transform.rotation.eulerAngles +rot);
		speed = transform.rotation * new Vector3 (Input.GetAxis("Horizontal")* pwr, 0, Input.GetAxis("Vertical")* pwr );

		cc.SimpleMove (speed );
	}


	public void setpaused( bool state){
		paused = state ;
	}
	public void doDamage(target Target){
		Target.subtractHealth ();
	}
	public void check_crosshair ()
	{
		Ray ray = Camera.main.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0));

		if(Input.GetKeyDown(KeyCode.Q)){
			Mode = Mode == (int)ControlMode.shooting ?  (int)ControlMode.normal : (int)ControlMode.shooting;
		}


		if (Input.GetMouseButtonDown(0) && Mode == (int)ControlMode.shooting ){
			shotFX.Play ();
			RaycastHit shot;
			Debug.Log ("Pang pang...");
			Physics.Raycast (ray, out shot, 50f);
			target t = shot.collider.GetComponent<target> ();
			if (t != null) {
				//TODO: FIX ME  
				Vector3[] positions = { this.transform.position, t.transform.position};
				BulletTrace.SetPositions (positions);

			}

			if (shot.collider.tag == "target" && data.instance.GetActiveMissionIndex() == 8) {
				doDamage (t);
			}if (shot.collider.tag == "lock" && data.instance.GetActiveMissionIndex() == 9) {
				lockExplosion.Play();
				mission.NextMission();

			}
			if (shot.collider.tag == "BIGLAMP" && data.instance.GetActiveMissionIndex() == 12) {
				Bigexplosion1.Play();
				mission.NextMission();

			} else {
				return;
			}
		}

		RaycastHit hit;
		Physics.Raycast (ray, out hit, 1f);
		if (hit.collider == null) {
			mission.Panel.SetActive (false);
		} else if (hit.collider.gameObject.tag == "pickup") {
			//show a tooltip
			if (hit.collider.name == "scroll") {
				mission.Panel.GetComponent<Text> ().text = "<color=green>[E]</color> Pickup Scroll";
				mission.Panel.SetActive (true);

				if (Input.GetKey (KeyCode.E)) {
					scroll_particle.Play();

					paused = true;

					hit.collider.gameObject.SetActive (false);
					mission.getScroll ();
					mission.clock.TimerStart ();
				}
			}else {

				mission.OnUserInteract (hit.collider);
			}
		} else if (hit.collider.tag == "speciallock" && haskey && data.instance.GetActiveMissionIndex() == 6) {
					mission.Panel.GetComponent<Text> ().text = "<color=green>[E]</color>use the key  " + hit.collider.name;
					mission.Panel.SetActive (true);
					if (Input.GetKeyDown (KeyCode.E)) {
				mission.locks.Remove (hit.collider.gameObject);
				mission.broken_locks.Add (hit.collider.gameObject);
				hit.collider.gameObject.SetActive (false);
						mission.setObjectiveDone ();
						haskey = false;
					}
				} 


			
		}
	}
