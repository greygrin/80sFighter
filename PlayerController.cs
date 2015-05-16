using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private float movementSpeed = 20f;
	private float jumpSpeed = 50f;
	private bool grounded = false;
	private bool isBlocking = false;
	private GameObject opponent;
	private float health = 100f;

	// Use this for initialization
	void Start () {
		if(gameObject.tag == "Player1"){
			opponent = GameObject.FindWithTag("Player2");
		}
		else if(gameObject.tag == "Player2"){
			opponent = GameObject.FindWithTag("Player1");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Punch")){
			Punch();
		}
		else if(Input.GetButtonDown("Kick")){
			Kick();
		}
		//else if input is Special???
		else{
			//Always face opponent
			Vector3 direction = transform.localScale;
			if(opponent.transform.position.x < transform.position.x){
				direction.x = -1f;
			}
			else{
				direction.x = 1f;
			}
			transform.localScale = direction;
			//apply horizontal movement to transform
			Vector2 movement = new Vector2(Input.GetAxis("Horizontal") * movementSpeed,
			                               transform.GetComponent<Rigidbody2D>().velocity.y);
			if(movement.x != 0){
				transform.GetComponent<Rigidbody2D>().velocity = movement;
				if((movement.x > 0 && transform.localScale.x == 1)
				   || (movement.x < 0 && transform.localScale.x == -1)){
					//TODO: play forward movement animation
				}
				else{
					//TODO: play backpedal animation
				}
			}
			//if grounded and not pressing movement, set velocity to 0
			else if(grounded){
				movement.x = 0;
				transform.GetComponent<Rigidbody2D>().velocity = movement;
			}

			//if grounded, allow for jumping and ducking
			if(grounded){
				//if vertical input is up, jump
				if(Input.GetAxis("Vertical") > 0){
					grounded = false;
					Debug.Log(gameObject.tag + " is jumping!");
					transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
					//TODO: play jumping animation
				}
				//if vertical input is down, duck
				else if(Input.GetAxis("Vertical") < 0){
					//TODO: play ducking animation
				}
			}

			//if other player is in front of you, don't move.
			RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, new Vector2(direction.x, 0), 2f);
			foreach(RaycastHit2D hit in hits){
				if(hit.transform.tag == opponent.tag && 
				   (transform.GetComponent<Rigidbody2D>().velocity.x * direction.x > 0)){
					movement.x = 0;
					movement.y = transform.GetComponent<Rigidbody2D>().velocity.y;
					transform.GetComponent<Rigidbody2D>().velocity = movement;
				}
			}
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.tag == "Ground"){
			grounded = true;
		}
	}

	//punch!
	void Punch(){
		//TODO: play punch animation
		//TODO: instantiate prefab at the end of the fist
		//TODO: destroy that prefab after a number of seconds: do this in the prefab's start 
	}

	//kick!
	void Kick(){
		//TODO: play kick animation
		//TODO: instantiate prefab at the end of the kick
		//TODO: destroy that prefab after a number of seconds: do this in the prefab's start 
	}

	//take the specified amount of damage
	public void TakeDamage(float amount){
		health -= amount;
		if(health <= 0){
			//TODO: DIE!
		}
	}

	//return the amount of health the player has
	public float GetHealth(){
		if(health < 0){
			return 0;
		}
		return health;
	}
 }
