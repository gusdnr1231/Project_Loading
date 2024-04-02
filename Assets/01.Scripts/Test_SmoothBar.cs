using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_SmoothBar : MonoBehaviour
{
	public Image progressBar;
	public float targetProgress;
	public float fillSpeed = 0.5f;

	private void Start()
	{
		StartCoroutine(ChangeProgressBarSmoothly());
	}

	IEnumerator ChangeProgressBarSmoothly()
	{
		while (progressBar.fillAmount < targetProgress)
		{
			progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, targetProgress, fillSpeed * Time.deltaTime);
			yield return null;
		}
	}
}
