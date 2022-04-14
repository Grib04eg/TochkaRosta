using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoresText;
    [SerializeField] CanvasGroup tapScreen;
    [SerializeField] GameObject floatSquarePrefab;
    [SerializeField] VioletSquare[] violetSquares;

    int scores;
    float inactiveTimer = 15f;
    Coroutine spawner;
    List<FloatSquare> floatSquares = new List<FloatSquare>();
    public void StartGame()
    {
        Debug.Log("StartGame");
        SetScores(0);
        tapScreen.blocksRaycasts = false;
        tapScreen.DOFade(0f, 1f);
        for (int i = 0; i < violetSquares.Length; i++)
        {
            violetSquares[i].Init(AddScores);
        }
        spawner = StartCoroutine(FloatSquaresSpawner());
    }

    private void Update()
    {
        if (tapScreen.blocksRaycasts)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            inactiveTimer = 15f;
        }
        inactiveTimer -= Time.deltaTime;
        if (inactiveTimer < 0)
            Restart();
    }

    void Restart()
    {
        foreach (var square in floatSquares)
        {
            if (square)
                Destroy(square.gameObject);
        }
        floatSquares = new List<FloatSquare>();
        StopCoroutine(spawner);
        inactiveTimer = 15f;
        tapScreen.DOFade(1f, 0.2f);
        tapScreen.blocksRaycasts = true;
        Debug.Log("Restart");
    }

    IEnumerator FloatSquaresSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 3f));
            var direction = Random.Range(0, 2) == 0 ? Vector3.right : Vector3.left;
            var position = -direction * 15 + Vector3.up * Random.Range(-2, 2);
            var square = Instantiate(floatSquarePrefab, position, Quaternion.identity).GetComponent<FloatSquare>();
            square.Init((FloatSquareType)Random.Range(0, 3), position, direction, AddScores);
            floatSquares.Add(square);
        }
    }

    void AddScores(int amount)
    {
        int scores = this.scores + amount;
        if (scores < 0)
            scores = 0;
        SetScores(scores);
    }

    void SetScores(int value)
    {
        scores = value;
        scoresText.text = $"Scores: {scores}";
    }
}
