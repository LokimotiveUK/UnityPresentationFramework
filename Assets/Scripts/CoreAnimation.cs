﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
public class CoreAnimation : MonoBehaviour {

    public enum EnterAnimation {
        None = 0,
        Appear = 1,
        FadeIn = 2,
        TransformIn = 3,
        Swing = 4,
        Custom = 5
    }

    public enum ExitAnimation
    {
        None = 0,
        Disappear = 1,
        FadeOut = 2,
        TransformOut = 3,
        Swing = 4,
        Custom = 5
    }

    public EnterAnimation enterAnimation = EnterAnimation.None;
    public ExitAnimation exitAnimation = ExitAnimation.None;


    [Range(0f, 10f)] [SerializeField] private float m_EnterDuration = 2;
    [Range(0f, 10f)] [SerializeField] private float m_ExitDuration = 2;

    [SerializeField] private Vector3 m_PositionInFrom;
    [SerializeField] private Vector3 m_RotationInFrom;
    [SerializeField] private Vector3 m_ScaleInFrom;

    [SerializeField] private Vector3 m_PositionOutTo;
    [SerializeField] private Vector3 m_RotationOutTo;
    [SerializeField] private Vector3 m_ScaleOutTo;

    [SerializeField] private bool m_AutoRotate = true;
	[Range(-50f, 50f)] [SerializeField] private float m_RotateSpeedX = 0f;
	[Range(-50f, 50f)] [SerializeField] private float m_RotateSpeedY = 10.0f;
	[Range(-50f, 50f)] [SerializeField] private float m_RotateSpeedZ = 0f;

	[SerializeField] private bool m_AutoVibrate = true;
	[Range(0f, 10f)] [SerializeField] private float m_FrequencyX = 1.0f;
	[Range(0f, 10f)] [SerializeField] private float m_MagnitudeX = 0.2f;
	[Range(-Mathf.PI, Mathf.PI)] [SerializeField] private float m_PhaseX = 1.5f;

	[Range(0f, 10f)] [SerializeField] private float m_FrequencyY = 1.0f;
	[Range(0f, 10f)] [SerializeField] private float m_MagnitudeY = 0.08f;
	[Range(-Mathf.PI, Mathf.PI)] [SerializeField] private float m_PhaseY = 0f;

	[Range(0f, 10f)] [SerializeField] private float m_FrequencyZ = 0f;
	[Range(0f, 10f)] [SerializeField] private float m_MagnitudeZ = 0f;
	[Range(-Mathf.PI, Mathf.PI)] [SerializeField] private float m_PhaseZ = 0f;


    private MeshRenderer m_MeshRenderer;
	private Vector3 m_OriginalPosition;
    private Quaternion m_OriginalRotation;
    private Vector3 m_OriginalScale;

    private bool m_Entered = false;
    private bool m_StartExiting = false;

	void Awake(){
        m_MeshRenderer = transform.GetComponent<MeshRenderer>();
		m_OriginalPosition = transform.localPosition;
        m_OriginalRotation = transform.localRotation;
        m_OriginalScale = transform.localScale;
        //if (enterAnimation == EnterAnimation.Appear) m_MeshRenderer.enabled = false;
	}

	// Use this for initialization
	void Start () {
        // Setting up size after all child loaded
        if (enterAnimation == EnterAnimation.TransformIn)
        {
            transform.localPosition = m_PositionInFrom;
            transform.localRotation = Quaternion.Euler(m_RotationInFrom);
            transform.localScale = m_ScaleInFrom;
            StartCoroutine(TransformInCoroutine());
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (m_Entered)
        {
            if (m_AutoRotate)
            {
                transform.Rotate(
                    m_RotateSpeedX * Time.deltaTime,
                    m_RotateSpeedY * Time.deltaTime,
                    m_RotateSpeedZ * Time.deltaTime
                );
            }

            if (m_AutoVibrate)
            {

                Vector3 newPos = new Vector3(
                                     m_OriginalPosition.x + OffsetByTime(m_FrequencyX, m_MagnitudeX, m_PhaseX),
                                     m_OriginalPosition.y + OffsetByTime(m_FrequencyY, m_MagnitudeY, m_PhaseY),
                                     m_OriginalPosition.z + OffsetByTime(m_FrequencyZ, m_MagnitudeZ, m_PhaseZ)
                                 );

                transform.position = newPos;
            }
        }
		
	}

	private float OffsetByTime(float frequency, float magnitude, float phase){
		return magnitude * Mathf.Sin (frequency * Time.time * Mathf.PI + phase);
    }

    private IEnumerator TransformInCoroutine()
    {
        for (float t = 0.0f; t < m_EnterDuration; t += Time.deltaTime)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, m_OriginalPosition, t / m_EnterDuration);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, m_OriginalRotation, t / m_EnterDuration);
            transform.localScale = Vector3.Lerp(transform.localScale, m_OriginalScale, t / m_EnterDuration);
            yield return null;
        }
        transform.localPosition = m_OriginalPosition;
        transform.localRotation = m_OriginalRotation;
        transform.localScale = m_OriginalScale;
        m_Entered = true;
    }
}
