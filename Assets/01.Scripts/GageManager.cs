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

			if (Time.time - lastInputTime > 1f) // �÷��̾ 1�� ���� �Է��� ���� �ʾ��� ��
			{
				if (DecreaseCoroutine == null)
				{
					DecreaseCoroutine = StartCoroutine(DecreaseGage()); //������ ���� ����
				}
			}
			else //�Է��� ���� ���
			{
				if (DecreaseCoroutine != null)
				{
					StopCoroutine(DecreaseCoroutine); //������ ���� ����
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
		if(DecreaseCoroutine != null) StopCoroutine(DecreaseCoroutine); // �̹��� ���� �߿��� ���Ҹ� �ߴ�
		DecreaseCoroutine = StartCoroutine(DecreaseGage());
		PushParticle.Play();
		float targetFillAmount = LoadingGage.fillAmount + 0.05f;
		targetFillAmount = Mathf.Clamp01(targetFillAmount); // 0�� 1 ���̷� ����
		IncreaseCoroutine = StartCoroutine(ChangeFillAmountSmoothly(targetFillAmount));

		SettingRandomKey();
	}

	private IEnumerator DecreaseGage()
	{
		if(IncreaseCoroutine != null) StopCoroutine(IncreaseCoroutine);
		while (true)
		{
			//yield return new WaitForSeconds(0.1f); // 0.1�ʸ��� ����
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
