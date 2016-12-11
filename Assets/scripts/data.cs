using System.Collections;

public  class data  {
	// all data is stored in this class. 
	public static string[] messages = {
		"Why do I always get stuck in these situations...",
		"When did that thing break.",
		"huh ...",
		"I believe i had to push the yellow one?!",
		"<color=green>yellow</color> and <color=yellow>red</color>",
		"Another one<color=red>?</color><color=green>!</color><color=green>?</color><color=red>!</color>",
		"The key ,huh.. I've seen it around here somewhere.",
		"Why do I always get stuck in these situations...",
		"When did that thing break.",
		"huh ...",
		"I believe i had to push the yellow one?!",
		"<color=green>yellow</color> and <color=yellow>red</color>",
		"Another one<color=red>?</color><color=green>!</color><color=green>?</color><color=red>!</color>",
		"The key ,huh.. I've seen it around here somewhere."

	};



	 int lamps_repaired;
	 int targets_shot ;
	 int activemission = -1; 


	bool hasscroll = false;
	int buttons_pushed = 0;

	private static data Instance=null;

	private data()
	{
	}

	public static data instance
	{
		get
		{
			if (Instance==null)
			{
				Instance = new data();
			}
			return Instance;
		}
	}

	// mission index methods 
	public   int GetActiveMissionIndex(){
		return instance.activemission ;
	}
	public void IncreaseMissionIndex(int amount = 1){
		this.activemission += amount;
	}
	// buttons pushed methods 
	public  int GetButtonsPushed (){
		return instance.buttons_pushed ;
	}
	public void IncreaseButtonsPushed(int amount = 1){
		this.buttons_pushed += amount ;
	}
	// targets hit methods 
	public  int GetTargetsDestroyed(){
		return instance.targets_shot;
	}
	public void IncreaseTargetsDestroyed (int amount = 1){
		this.targets_shot += amount;
	}
	// Lamps destroyed methods 
	public  int GetLampsRepaired (){
		return instance.lamps_repaired;
	}

	public void IncreaseLampsRepaired(int amount = 1){
		this.lamps_repaired += amount;
	}

	public  bool  HasScroll(){
		return instance.hasscroll;
	}

	public  void PickedUpScroll (bool done = true ){
		instance.hasscroll = done;
	}  
}
