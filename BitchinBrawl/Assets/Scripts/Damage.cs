using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour
{
	private float damage = 5;

	// Use this for initialization
	void Start ()
	{
		Destroy(this.gameObject, .5f);
	}
	
	void OnTriggerEnter2D(Collider2D col){
		if(col.tag != transform.parent.tag){
			col.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
			Destroy(this.gameObject);
		}
	}

	public void SetDamage(float amount){
		damage = amount;
	}
}

