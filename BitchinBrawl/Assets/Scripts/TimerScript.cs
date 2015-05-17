using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour {

	private float timer;
	private Text timerText;
	private bool roundOver = false;
	private string winner = "Mr. T";

	// Use this for initialization
	void Start () {
		timerText = gameObject.GetComponent<Text>();
		timer = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(!roundOver){
			timerText.text = "" + (60 - ((int)Time.time - timer));
			if(timerText.text == "0"){
				//End the round
				roundOver = true;
				PlayerController player1 = GameObject.FindWithTag("Player1").GetComponent<PlayerController>();
				PlayerController player2 = GameObject.FindWithTag("Player2").GetComponent<PlayerController>();
				if(player1.GetHealth() < player2.GetHealth()){
					player1.TakeDamage(100);
					winner = "Michael Jackson";
				}
				else{
					player2.TakeDamage(100);
					winner = "Mr. T";
				}
			}
		}
	}

	public void SetRoundOver(){
		roundOver = true;
		PlayerController player1 = GameObject.FindWithTag("Player1").GetComponent<PlayerController>();
		PlayerController player2 = GameObject.FindWithTag("Player2").GetComponent<PlayerController>();
		if(player1.GetHealth() < player2.GetHealth()){
			winner = "Michael Jackson";
		}
		else{
			winner = "Mr. T";
		}
		StartCoroutine(Victory());
	}

	IEnumerator Victory(){
		timerText.text = "The Winner is...";
		Vector3 scale = timerText.transform.localScale;
		Vector3 pos = timerText.transform.position;
		for(int i=0; i < 100; i++){
			scale.x += .0001f;
			scale.y += .0001f;
			pos.y -= .1f;
			timerText.transform.localScale = scale;
			timerText.transform.position = pos;
			yield return new WaitForSeconds(.01f);
		}
		timerText.text += "\n" + winner;
	}

}
