using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GageManager : MonoBehaviour
{
	#region Variables

	[Header("Variables")]
	public float decreaseSpeed = 0.1f;
	public float fillSpeed = 0.1f;
	private bool CompleteLoad = false;

	[Header("Input Keys")]
	public KeyCode[] NeedKeyArray;
    public KeyCode NeedToPushKey;

	#endregion

	#region Other

	[Header("Objects")]
	public Image LoadingGage;
	//public Image NeedKeyImage;

	[Header("Sprites")]
	//public Sprite[] KeyImages;

	private Coroutine DecreaseCoroutine;
	#endregion

	private void Awake()
	{
		//LoadingGage = GetComponent<Image>();
		//NeedKeyImage = GetComponent<Image>();
		CompleteLoad = false;
	}

	private void Start()
	{
		LoadingGage.fillAmount = 0;
		DecreaseCoroutine = StartCoroutine(DecreaseGage());
		SettingRandomKey();
	}

	private void Update()
	{
		if (LoadingGage.fillAmount >= 1)
		{
			CompleteLoad = true;
			if(DecreaseCoroutine != null)
			{
				StopCoroutine(DecreaseCoroutine);
				DecreaseCoroutine = null;
			}
		}
		if(Input.GetKeyDown(NeedToPushKey) && CompleteLoad == false)
		{
			IncreaseGage();
		}
	}

	#region Methods

	private void SettingRandomKey()
	{
		int randKey = Random.Range(0, NeedKeyArray.Length);
		NeedToPushKey = NeedKeyArray[randKey];
	}

	private void IncreaseGage()
	{
		if(DecreaseCoroutine != null) StopCoroutine(DecreaseCoroutine); // 이미지 증가 중에는 감소를 중단
		DecreaseCoroutine = StartCoroutine(DecreaseGage());

		float targetFillAmount = LoadingGage.fillAmount + 0.05f;
		targetFillAmount = Mathf.Clamp01(targetFillAmount); // 0과 1 사이로 제한
		StartCoroutine(ChangeFillAmountSmoothly(targetFillAmount));

		SettingRandomKey();
	}

	private IEnumerator DecreaseGage()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.1f); // 0.1초마다 감소
			LoadingGage.fillAmount -= decreaseSpeed * Time.deltaTime;
		}
	}

	private IEnumerator ChangeFillAmountSmoothly(float targetFillAmount)
	{
		while (LoadingGage.fillAmount < targetFillAmount)
		{
			LoadingGage.fillAmount += fillSpeed * Time.deltaTime;
			yield return null;
		}
	}
	#endregion
}
