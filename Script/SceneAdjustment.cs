using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneAdjustment : MonoBehaviour
{
    [SerializeField] private Image targetImage; // 색상 전환 대상 이미지
    [SerializeField] private float transitionDuration = 1.5f; // 전환 지속 시간
    [SerializeField] private float delayBetweenTransitions = 1.0f; // 전환 사이의 대기 시간

    private bool isTransitioning = false;
    void Start()
    {
        if (targetImage != null)
        {
            // 초기 색상 설정 (B4B4B4)
            targetImage.color = new Color(180 / 255f, 180 / 255f, 180 / 255f);
            StartCoroutine(ColorFadeCoroutine());
        }
        else
        {
            Debug.LogError("Target Image is not assigned!");
        }
    }

    void Update()
    {

    }

    public void NextLoadingScene()
    {
        LoadingScene.LoadScene("WorldMap");
    }

    public void Restart()
    {
        SceneManager.LoadScene("Main");
    }

    public void Exit()
    {
        Application.Quit();
    }

    private IEnumerator ColorFadeCoroutine()
    {
        while (true)
        {
            // B4B4B4 → 3A3A3A
            yield return StartCoroutine(FadeRGB(180, 180, 180, 58, 58, 58));
            yield return new WaitForSeconds(delayBetweenTransitions);

            // 3A3A3A → B4B4B4
            yield return StartCoroutine(FadeRGB(58, 58, 58, 180, 180, 180));
            yield return new WaitForSeconds(delayBetweenTransitions);
        }
    }

    private IEnumerator FadeRGB(int startR, int startG, int startB, int endR, int endG, int endB)
    {
        isTransitioning = true;

        // 초기 색상과 목표 색상 설정
        Color startColor = new Color(startR / 255f, startG / 255f, startB / 255f);
        Color endColor = new Color(endR / 255f, endG / 255f, endB / 255f);
        float timer = 0f;

        while (timer <= transitionDuration)
        {
            timer += Time.deltaTime;

            // RGB 값을 Lerp로 점진적으로 변화
            float t = timer / transitionDuration;
            float r = Mathf.Lerp(startColor.r, endColor.r, t);
            float g = Mathf.Lerp(startColor.g, endColor.g, t);
            float b = Mathf.Lerp(startColor.b, endColor.b, t);

            // 색상 적용
            targetImage.color = new Color(r, g, b);
            yield return null;
        }

        // 최종 색상 보정
        targetImage.color = endColor;
        isTransitioning = false;
    }
}
