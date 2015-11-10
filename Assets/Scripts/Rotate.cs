using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour
{
	public int rate = 30;

	void Update () {

		transform.Rotate(Vector3.up * Time.deltaTime * rate);

	}
}
