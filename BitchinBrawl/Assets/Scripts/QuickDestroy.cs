using UnityEngine;
using System.Collections;

public class QuickDestroy : MonoBehaviour {

	public float destroyTime = 2f;

	// Use this for initialization
	void Start () {
		Destroy(this.gameObject, destroyTime);
	}
}
