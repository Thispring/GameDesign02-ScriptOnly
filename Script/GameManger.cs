using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManger : MonoBehaviour
{
    public float gameTimer = 300f;
    private float timeRemaining;
    public TextMeshProUGUI timerText;
    // 체크포인트 관리
    public List<Vector3> checkpointPositions = new List<Vector3>();
    public float point;
    public PlayerState playerState;
    public Image image;
    public float startAlpha = 0;
    public float targetAlpha = 1f; // 목표 투명도 (0: 완전 투명, 1: 완전 불투명)
    public float fadeDuration = 2f; // 투명도를 변경하는 데 걸리는 시간
    public Image keyW;
    public Image keyA;
    public Image keyS;
    public Image keyD;
    public Image arrow;
    private Color defaultColor = Color.white; // 기본 흰색
    private Color pressedColor = new Color(1f, 0f, 0f); // 빨간색 (FF0000)
    public RadioObject radioObject;
    private bool arrowColorOut;

    void Start()
    {
        arrowColorOut = false;
        gameTimer = 300;
        SetResolution();
        point = 0;
        timeRemaining = gameTimer;
        Application.targetFrameRate = 120;
    }
    void Update()
    {
        timeRemaining = Mathf.Clamp(timeRemaining - Time.deltaTime, 0, gameTimer);
        UpdateTimerUI();
        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            GameOver();
        }
        // 백버튼을 눌렀을 경우 게임 종료
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main");
        }

        if (playerState.playerDeath == false)
        {
            startAlpha = 0;
            image.color = new Color(image.color.r, image.color.g, image.color.b, startAlpha);
        }
        else
        {
            // 테스트 
            StartCoroutine(FadeToTransparency(targetAlpha, fadeDuration));
        }

        if (point == 7)
        {
            SceneManager.LoadScene("ClearEnding");
        }

        // W 키
        if (Input.GetKey(KeyCode.W))
            keyW.color = pressedColor;
        else
            keyW.color = defaultColor;

        // A 키
        if (Input.GetKey(KeyCode.A))
            keyA.color = pressedColor;
        else
            keyA.color = defaultColor;

        // S 키
        if (Input.GetKey(KeyCode.S))
            keyS.color = pressedColor;
        else
            keyS.color = defaultColor;

        // D 키
        if (Input.GetKey(KeyCode.D))
            keyD.color = pressedColor;
        else
            keyD.color = defaultColor;

        if (point >= 1)
        {
            keyW.color = new Color(image.color.r, image.color.g, image.color.b, 0);
            keyS.color = new Color(image.color.r, image.color.g, image.color.b, 0);
            keyA.color = new Color(image.color.r, image.color.g, image.color.b, 0);
            keyD.color = new Color(image.color.r, image.color.g, image.color.b, 0);
            arrowColorOut = true;
        }
        else
        {
            StartCoroutine(FadeToColorLoop(Color.white, Color.red, 2f));
        }
    }
    // 투명도를 점진적으로 변경하는 코루틴
    private IEnumerator FadeToTransparency(float targetAlpha, float duration)
    {
        float startAlpha = image.color.a; // 현재 알파 값 (기존 투명도)
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha); // 알파 값만 변경
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종 투명도 설정 (알파가 정확히 목표값에 도달하도록 보장)
        image.color = new Color(image.color.r, image.color.g, image.color.b, targetAlpha);
    }

    private IEnumerator FadeToColorLoop(Color color1, Color color2, float duration)
    {
        Color startColor = color1;
        Color targetColor = color2;

        while (!arrowColorOut) // arrowColorOut가 false일 때만 실행
        {
            float elapsedTime = 0f;

            // 한 방향으로 색상 변화
            while (elapsedTime < duration)
            {
                if (arrowColorOut) // 중간에 arrowColorOut이 true로 바뀌면 루프 종료
                {
                    break;
                }

                Color newColor = Color.Lerp(startColor, targetColor, elapsedTime / duration);
                arrow.color = newColor;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 색상 전환 방향 반전
            (startColor, targetColor) = (targetColor, startColor);
        }

        // arrowColorOut이 true일 경우 즉시 투명 처리
        arrow.color = new Color(arrow.color.r, arrow.color.g, arrow.color.b, 0);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void UpdateTimerUI()
    {
        if (timerText != null && point <= 1 || point == 2 && radioObject.destroyRadio)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("Escape this place\n" + "{0:00}:{1:00}", minutes, seconds);
        }
        else if (timerText != null && point == 2 && !radioObject.destroyRadio)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("Destroy the radio\n" + "{0:00}:{1:00}", minutes, seconds);
        }
        else if (timerText != null && point >= 5)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("Hide & Escape this place\n" + "{0:00}:{1:00}", minutes, seconds);
        }
    }

    private void GameOver()
    {
        SceneManager.LoadScene("TimeEnding");
        Debug.Log("Game Over!");
    }

    /* 해상도 설정하는 함수 */
    public void SetResolution()
    {
        int setWidth = 1920; // 사용자 설정 너비
        int setHeight = 1080; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
    }
}
