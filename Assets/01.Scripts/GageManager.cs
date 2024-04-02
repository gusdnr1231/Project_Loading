using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GageManager : MonoBehaviour
{
	#region Variables

	[Header("Variables")]
	public float decreaseSpeed = 0.1f;
	public float fillSpeed = 0.1f;
	private bool CompleteLoad = false;
	private float lastInputTime;

	[Header("Input Keys")]
	public KeyCode[] CanPushKeyArray;
    public KeyCode PushKey;

	#endregion

	#region Other

	[Header("Objects")]
	public Image LoadingGage;
	public TMP_Text KeyText;
	public ParticleSystem PushParticle;
	//public Image NeedKeyImage;

	[Header("Sprites")]
	//public Sprite[] KeyImages;

	private Coroutine IncreaseCoroutine;
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
			KeyText.color = Color.green;
			KeyText.text = "Complete";
			if (DecreaseCoroutine != null)
			{
				StopCoroutine(DecreaseCoroutine);
				DecreaseCoroutine = null;
			}
		}

		if(CompleteLoad == false)	
		{
			if (Input.GetKeyDown(PushKey))
			{
				lastInputTime = Time.time;
				IncreaseGage();
			}

			if (Time.time - lastInputTime > 1f) // 플레이어가 1초 동안 입력을 하지 않았을 때
			{
				if (DecreaseCoroutine == null)
				{
					DecreaseCoroutine = StartCoroutine(DecreaseGage()); //게이지 감소 시작
				}
			}
			else //입력을 했을 경우
			{
				if (DecreaseCoroutine != null)
				{
					StopCoroutine(DecreaseCoroutine); //게이지 감소 중지
					DecreaseCoroutine = null;
				}
			}
		}		
	}

	#region Methods

	private void SettingRandomKey()
	{
		int randKey = Random.Range(0, CanPushKeyArray.Length);
		PushKey = CanPushKeyArray[randKey];
		KeyText.text = PushKey.ToString();
	}

	private void IncreaseGage()
	{
		if(DecreaseCoroutine != null) StopCoroutine(DecreaseCoroutine); // 이미지 증가 중에는 감소를 중단
		DecreaseCoroutine = StartCoroutine(DecreaseGage());
		PushParticle.Play();
		float targetFillAmount = LoadingGage.fillAmount + 0.05f;
		targetFillAmount = Mathf.Clamp01(targetFillAmount); // 0과 1 사이로 제한
		IncreaseCoroutine = StartCoroutine(ChangeFillAmountSmoothly(targetFillAmount));

		SettingRandomKey();
	}

	private IEnumerator DecreaseGage()
	{
		if(IncreaseCoroutine != null) StopCoroutine(IncreaseCoroutine);
		while (true)
		{
			//yield return new WaitForSeconds(0.1f); // 0.1초마다 감소
			yield return null;
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
