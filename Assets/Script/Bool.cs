
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class Bool : MonoBehaviour
{
    [Header("ボールのリセット設定")]
    public Transform startPoint;

    private bool isResetting = false;

    [Header("発射設定")]
    public float launchSpeed = 15f;

    [Header("停止判定")]
    public float stopSpeed = 0.08f;
    public float stopDuration = 1.5f;

    [Header("結果表示 UI")]
    public TMP_Text resultText;

    [Header("履歴表示 UI")]
    public TMP_Text historyText;

    [Header("結果の色")]
    public Color redColor = new Color(0.9f, 0.1f, 0.1f);
    public Color blackColor = new Color(0.15f, 0.15f, 0.15f);
    public Color greenColor = new Color(0.1f, 0.7f, 0.2f);

    private Rigidbody rb;

    private string currentNumber = "";
    private string currentColor = "";

    private float stoppedTime = 0f; 
    private bool resultConfirmed = false;

    private const int MaxHistory = 5;

    private readonly List<string> history = new List<string>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody がありません！");
            enabled = false;
            return;
        }

        if (resultText != null)
        {
            resultText.text = "回転中...";
        }

        if (historyText != null)
        {
            historyText.text = "履歴\n";
        }

        rb.linearDamping = 0f;
        rb.linearVelocity = Vector3.zero;

        rb.AddForce(
            Vector3.forward * launchSpeed,
            ForceMode.VelocityChange
        );

        Debug.Log("ボール発射！");

        ResetBall();
    }

    void Update()
    {
        if (resultConfirmed || rb == null)
            return;

        if (rb.linearVelocity.magnitude <= stopSpeed)
        {
            stoppedTime += Time.deltaTime;

            if (stoppedTime >= stopDuration)
            {
                ConfirmResult();
            }
        }
        else
        {
            stoppedTime = 0f;
        }
    }

    public void SetPocket(string number, string color)
    {
        if (resultConfirmed)
            return;

        currentNumber = number;
        currentColor = color;

        Debug.Log(
            "通過中のマス：" + currentNumber
            + " / 色：" + currentColor
        );
    }

    private void ConfirmResult()
    {
        resultConfirmed = true;

        if (string.IsNullOrEmpty(currentNumber))
        {
            if (resultText != null)
                resultText.text = "判定失敗";

            Debug.LogWarning(
                "ルーレット結果：番号を判定できませんでした"
            );

            return;
        }

        string colorJapanese;
        Color displayColor;

        switch (currentColor)
        {
            case "red":
                colorJapanese = "赤";
                displayColor = redColor;
                break;

            case "black":
                colorJapanese = "黒";
                displayColor = blackColor;
                break;

            case "green":
                colorJapanese = "緑";
                displayColor = greenColor;
                break;

            default:
                colorJapanese = "Unknoun";
                displayColor = Color.white;
                break;
        }

        string result = currentNumber + ":" + colorJapanese;

        // 最新の結果を表示
        if (resultText != null)
        {
            resultText.text =
                $"<color=#{ColorUtility.ToHtmlStringRGB(displayColor)}>{result}</color>";
        }

        // 履歴の先頭に新しい結果を追加
        history.Insert(0, result);

        // 6件目以降は古い履歴を削除
        if (history.Count > MaxHistory)
        {
            history.RemoveAt(history.Count - 1);
        }

        UpdateHistoryUI();

        Debug.Log("ルーレット結果：" + result);
    }

    private void UpdateHistoryUI()
    {
        if (historyText == null)
            return;

        string historyDisplay = "履歴\n";

        for (int i = 0; i < history.Count; i++)
        {
            string number = history[i];

            // 履歴の結果に対応する色を取得
            string colorName = number.Substring(
                number.IndexOf(':') + 1
            );

            Color displayColor;

            switch (colorName)
            {
                case "red":
                    displayColor = redColor;
                    break;

                case "black":
                    displayColor = blackColor;
                    break;

                case "green":
                    displayColor = greenColor;
                    break;

                default:
                    displayColor = Color.black;
                    break;
            }

            switch (colorName)
            {
                case "赤": displayColor = redColor; break;
                case "黒": displayColor = blackColor; break;
                case "緑": displayColor = greenColor; break;
                default: displayColor = Color.black; break;
            }

            string hexColor =
                ColorUtility.ToHtmlStringRGB(displayColor);

            historyDisplay +=
                $"<color=#{hexColor}>{i + 1}. {number}</color>\n";
        }

        historyText.text = historyDisplay;
    }

    public void ResetBall()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody がありません！");
            return;
        }

        if (startPoint == null)
        {
            Debug.LogError("Start Point を設定してください！");
            return;
        }

        // 物理演算を一時停止して初期位置に戻す
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.position = startPoint.position;
        rb.rotation = startPoint.rotation;

        // 停止判定と結果をリセット
        stoppedTime = 0f;
        resultConfirmed = false;
        currentNumber = "";
        currentColor = "";

        // 再び発射
        rb.linearDamping = 0f;
        rb.linearVelocity = Vector3.zero;

        float randomSpeed = Random.Range(1f, 10f); // ランダムスピード(未使用)
        rb.AddForce(
            Vector3.forward * launchSpeed,/*← ランダムにしたい場合は launchSpeed を randomSpeed にする*/
            ForceMode.VelocityChange
        );

        if (resultText != null)
            resultText.text = "回転中...";

        Debug.Log("ボールをリセットして再発射しました！");
    }
}
