using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerMovement : MonoBehaviour
{
    [Header("Keys")]
    public KeyCode UpKey;
    public KeyCode DownKey;

    public Transform[] MoveTrms;

	private void Update()
	{
		if (Input.GetKeyDown(UpKey))
		{

		}

		if(Input.GetKeyDown(DownKey))
		{

		}
	}

	private void MoveRunner()
	{

	}
}
