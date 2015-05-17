using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private float movementSpeed = 20f;
	private float jumpSpeed = 50f;
	private bool grounded = false;
	private bool isBlocking = false;
	private tk2dSpriteAnimator animator;
	private GameObject opponent;
	public float health = 100f;
	public GameObject damage;
	public GameObject bloodExplosion;
	private bool roundOver = false;

	//controls
	private string punch = "Punch1";
	private string kick = "Kick1";
	private string horizontal = "Horizontal1";
	private string vertical = "Vertical1";

	// Use this for initialization
	void Start () {
		if(gameObject.tag == "Player1"){
			opponent = GameObject.FindWithTag("Player2");

		}
		//if script is on player 2, set controls accordingly
		else if(gameObject.tag == "Player2"){
			opponent = GameObject.FindWithTag("Player1");
			punch = "Punch2";
			kick = "Kick2";
			horizontal = "Horizontal2";
			vertical = "Vertical2";
		}
		animator = gameObject.GetComponent<tk2dSpriteAnimator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!roundOver){
			if(!(animator.IsPlaying("Punch") || animator.IsPlaying("Kick"))){
				if(Input.GetButtonDown(punch)){
					StartCoroutine(Punch());
				}
				else if(Input.GetButtonDown(kick)){
					StartCoroutine(Kick());
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
					Vector2 movement = new Vector2(Input.GetAxis(horizontal) * movementSpeed,
					                               transform.GetComponent<Rigidbody2D>().velocity.y);

					if(movement.x != 0){
						transform.GetComponent<Rigidbody2D>().velocity = movement;
						if(grounded){
							animator.Play("Walk");
						}
						else{
							animator.Play("Jump");
						}
					}

					//if grounded, allow for jumping and ducking
					if(grounded){
						//if vertical input is up, jump
						if(Input.GetAxis(vertical) > 0){
							Jump();
						}
						//if vertical input is down, duck
						else if(Input.GetAxis(vertical) < 0 && Input.GetAxis(horizontal) == 0){
							animator.Play("Crouch");
						}
						//if grounded and not pressing movement, set velocity to 0 and use idle anim
						else if(Input.GetAxis(horizontal) == 0){
							movement.x = 0;
							transform.GetComponent<Rigidbody2D>().velocity = movement;
							animator.Play("Idle");
						}
					}

					//if other player is in front of you, don't move.
					RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, new Vector2(direction.x, 0), 4f);
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
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.tag == "Ground"){
			grounded = true;
		}
	}

	//punch!
	IEnumerator Punch(){
		//play punch animation
		animator.Play("Punch");
		//instantiate prefab at the end of the fist
		Vector3 spawnPos = transform.position;
		spawnPos.x += 3.8f * transform.localScale.x;
		spawnPos.y += 3f;
		GameObject newPunch = (GameObject) Instantiate(damage, spawnPos, Quaternion.identity);
		newPunch.transform.parent = transform;
		if(!grounded)
			newPunch.GetComponent<Damage>().SetDamage(10);
		yield return new WaitForSeconds(.3f);
		animator.Play("Idle");
	}

	//kick!
	IEnumerator Kick(){
		//play kick animation
		animator.Play("Kick");
		//instantiate prefab at the end of the leg
		Vector3 spawnPos = transform.position;
		spawnPos.x += 4f * transform.localScale.x;
		spawnPos.y += -2.5f;
		GameObject newKick = (GameObject) Instantiate(damage, spawnPos, Quaternion.identity);
		newKick.transform.parent = transform;
		if(!grounded)
			newKick.GetComponent<Damage>().SetDamage(8);
		yield return new WaitForSeconds(.3f);
		animator.Play("Idle");
	}

	void Jump(){
		grounded = false;
		//play jumping animation
		animator.Play("Jump");
		transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
	}

	//take the specified amount of damage
	public void TakeDamage(float amount){
		health -= amount;
		if(health <= 0 && !roundOver){
			//DIE!
			StartCoroutine(Die());
		}
	}

	//return the amount of health the player has
	public float GetHealth(){
		if(health < 0){
			return 0;
		}
		return health;
	}

	private IEnumerator Die(){
		roundOver = true;
		opponent.GetComponent<PlayerController>().SetRoundOver();
		GameObject.Find("RoundTimer").GetComponent<TimerScript>().SetRoundOver();
		animator.Pause();
		Vector3 shakeRight = transform.position;
		shakeRight.x += .5f;
		Vector3 shakeLeft = transform.position;
		shakeLeft.x -= .5f;
		gameObject.GetComponent<tk2dSprite>().color = Color.red;
		for(int i = 0; i < 15; i++){
			transform.position = shakeRight;
			yield return new WaitForSeconds(.05f);
			transform.position = shakeLeft;
			yield return new WaitForSeconds(.05f);
		}
		Vector3 spawnPos = transform.position;
		spawnPos.y += 3;
		Instantiate(bloodExplosion, spawnPos, Quaternion.identity);
		Destroy(this.gameObject);
	}

	public void SetRoundOver(){
		roundOver = true;
	}
 }
