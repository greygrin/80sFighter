using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HPScript : MonoBehaviour {

	public GameObject player;
	private PlayerController playerController;
	private Text HPtext;
	private Transform visualHP;
	private Transform visualDMG;
	private float currentHP = 100;
	private float timer = 5;

	// Use this for initialization
	void Start () {
		HPtext = transform.Find("HPText").GetComponent<Text>();
		visualHP = transform.Find("BGColor").Find("VisualHP");
		visualDMG = transform.Find("BGColor").Find("VisualDMG");
		playerController = player.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		if(playerController.GetHealth() != currentHP){
			StartCoroutine(DamageDisplay());
		}
	}

	//displays for a moment the difference between current and previous hp.
	IEnumerator DamageDisplay(){
		currentHP = playerController.GetHealth();
		HPtext.text = "HP: " + currentHP;
		Vector3 pos = visualHP.localPosition;
		float HPdiff = 100 - currentHP;
		pos.x = -HPdiff;
		visualHP.localPosition = pos;
		timer = 5;
		while(timer > 0){
			timer--;
			yield return new WaitForSeconds(.1f);
		}
		visualDMG.position = visualHP.position;
	}

}
