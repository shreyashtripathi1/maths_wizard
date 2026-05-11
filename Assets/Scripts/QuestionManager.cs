using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class QuestionManager : MonoBehaviour
{
    [Serializable]
    public class MathQuestion
    {
        public string question;
        public string[] options;
        public string correct;
        public int points;
    }

    public TextMeshProUGUI questionText;
    public TextMeshProUGUI[] optionTexts;
    public TextMeshProUGUI questionStatusA;
    public TextMeshProUGUI questionStatusB;
    public TextMeshProUGUI pointsTextA;
    public TextMeshProUGUI pointsTextB;

    private List<MathQuestion> mathQuestions = new List<MathQuestion>();
    private MathQuestion currentQuestion;
    private System.Random random = new System.Random();

    private bool hasPlayer1Answered = false;
    private bool hasPlayer2Answered = false;
    private bool isChoosingSpell = false;
    private int currentPoints = 0;
    private int player1Score = 0;
    private int player2Score = 0;

    private bool player1Correct = false;
    private bool player2Correct = false;
    private int firstCorrectPlayer = 0;

    public bool player1light = false;
    public bool player1heavy = false;
    public bool player2light = false;
    public bool player2heavy = false;

    void Start()
    {
        InitializeQuestions();

        // Get local button and add listener
        GameObject localBtn = GameObject.FindGameObjectWithTag("localButton");
        if (localBtn != null)
        {
            localBtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnLocalButtonClicked);
        }

        // Get back button and add listener
        GameObject backBtn = GameObject.FindGameObjectWithTag("backButton");
        if (backBtn != null)
        {
            backBtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnBackButtonClicked);
        }

        // Initially hide this GameObject
        gameObject.SetActive(false);
    }

    void OnLocalButtonClicked()
    {
        // Show this GameObject when local button is clicked
        gameObject.SetActive(true);
        DisplayRandomQuestion();
    }

    void OnBackButtonClicked()
    {
        // Hide this GameObject when back button is clicked
        gameObject.SetActive(false);
        questionText.text = "";
        optionTexts[0].text = "";
        optionTexts[1].text = "";
        optionTexts[2].text = "";
        optionTexts[3].text = "";
    }

    void InitializeQuestions()
    {
        mathQuestions = new List<MathQuestion>
        {
            new MathQuestion { question = "What is 2 + 2?", options = new[] { "4", "5", "3", "6" }, correct = "4", points = 1 },
            new MathQuestion { question = "What is 5 - 3?", options = new[] { "2", "3", "1", "4" }, correct = "2", points = 1 },
            new MathQuestion { question = "What is 3 x 3?", options = new[] { "9", "6", "3", "12" }, correct = "9", points = 1 },
            new MathQuestion { question = "What is 8 / 2?", options = new[] { "4", "2", "6", "3" }, correct = "4", points = 1 },
            new MathQuestion { question = "What is 10 % 3?", options = new[] { "1", "0", "3", "2" }, correct = "1", points = 1 },
            new MathQuestion { question = "What is 7 + 6?", options = new[] { "13", "14", "15", "12" }, correct = "13", points = 1 },
            new MathQuestion { question = "What is 6 - 1?", options = new[] { "5", "4", "3", "2" }, correct = "5", points = 1 },
            new MathQuestion { question = "What is 9 x 1?", options = new[] { "9", "8", "7", "6" }, correct = "9", points = 1 },
            new MathQuestion { question = "What is 12 / 4?", options = new[] { "3", "2", "4", "5" }, correct = "3", points = 1 },
            new MathQuestion { question = "What is 3 + 4?", options = new[] { "7", "6", "8", "5" }, correct = "7", points = 1 },

            new MathQuestion { question = "What is 14 - 6?", options = new[] { "8", "9", "7", "6" }, correct = "8", points = 2 },
            new MathQuestion { question = "What is 5 x 5?", options = new[] { "25", "20", "15", "30" }, correct = "25", points = 2 },
            new MathQuestion { question = "What is 16 / 4?", options = new[] { "4", "5", "3", "6" }, correct = "4", points = 2 },
            new MathQuestion { question = "What is 18 % 5?", options = new[] { "3", "2", "1", "4" }, correct = "3", points = 2 },
            new MathQuestion { question = "What is 11 + 11?", options = new[] { "22", "21", "23", "24" }, correct = "22", points = 2 },
            new MathQuestion { question = "What is 20 - 9?", options = new[] { "11", "12", "10", "9" }, correct = "11", points = 2 },
            new MathQuestion { question = "What is 6 x 3?", options = new[] { "18", "12", "15", "16" }, correct = "18", points = 2 },
            new MathQuestion { question = "What is 15 / 3?", options = new[] { "5", "4", "6", "3" }, correct = "5", points = 2 },
            new MathQuestion { question = "What is 17 % 4?", options = new[] { "1", "0", "2", "3" }, correct = "1", points = 2 },
            new MathQuestion { question = "What is 9 + 10?", options = new[] { "19", "20", "21", "18" }, correct = "19", points = 2 },
        };
    }

    void DisplayRandomQuestion()
    {
        int index = random.Next(mathQuestions.Count);
        currentQuestion = mathQuestions[index];

        questionText.text = currentQuestion.question;
        currentPoints = currentQuestion.points;

        for (int i = 0; i < optionTexts.Length; i++)
        {
            optionTexts[i].text = $"{(char)('A' + i)}. {currentQuestion.options[i]}";
        }

        hasPlayer1Answered = false;
        hasPlayer2Answered = false;
        player1Correct = false;
        player2Correct = false;
        firstCorrectPlayer = 0;
        isChoosingSpell = false;

        questionStatusA.text = "";
        questionStatusB.text = "";
    }

    void Update()
    {
        if (isChoosingSpell)
        {
            if (firstCorrectPlayer == 1)
            {
                if (Input.GetKeyDown(KeyCode.W)) { player1light = true; StartCoroutine(WaitAndNextQuestion()); }
                if (Input.GetKeyDown(KeyCode.E)) { player1heavy = true; StartCoroutine(WaitAndNextQuestion()); }
            }
            else if (firstCorrectPlayer == 2)
            {
                if (Input.GetKeyDown(KeyCode.I)) { player2light = true; StartCoroutine(WaitAndNextQuestion()); }
                if (Input.GetKeyDown(KeyCode.O)) { player2heavy = true; StartCoroutine(WaitAndNextQuestion()); }
            }
            return;
        }

        if (!hasPlayer1Answered)
        {
            if (Input.GetKeyDown(KeyCode.A)) HandleAnswer(1, 0);
            if (Input.GetKeyDown(KeyCode.S)) HandleAnswer(1, 1);
            if (Input.GetKeyDown(KeyCode.D)) HandleAnswer(1, 2);
            if (Input.GetKeyDown(KeyCode.F)) HandleAnswer(1, 3);
        }

        if (!hasPlayer2Answered)
        {
            if (Input.GetKeyDown(KeyCode.H)) HandleAnswer(2, 0);
            if (Input.GetKeyDown(KeyCode.J)) HandleAnswer(2, 1);
            if (Input.GetKeyDown(KeyCode.K)) HandleAnswer(2, 2);
            if (Input.GetKeyDown(KeyCode.L)) HandleAnswer(2, 3);
        }
    }

    void HandleAnswer(int player, int optionIndex)
    {
        bool isCorrect = currentQuestion.options[optionIndex] == currentQuestion.correct;

        if (player == 1)
        {
            hasPlayer1Answered = true;
            if (isCorrect)
            {
                player1Correct = true;
                questionStatusA.text = "Correct Answer!";
                if (!hasPlayer2Answered)
                {
                    firstCorrectPlayer = 1;
                    ShowSpellOptions();
                }
                else
                {
                    player1Score += currentPoints;
                    UpdatePointsUI();
                    StartCoroutine(WaitAndNextQuestion());
                }
            }
            else
            {
                questionStatusA.text = "Wrong Answer!";
            }
        }
        else if (player == 2)
        {
            hasPlayer2Answered = true;
            if (isCorrect)
            {
                player2Correct = true;
                questionStatusB.text = "Correct Answer!";
                if (!hasPlayer1Answered)
                {
                    firstCorrectPlayer = 2;
                    ShowSpellOptions();
                }
                else
                {
                    player2Score += currentPoints;
                    UpdatePointsUI();
                    StartCoroutine(WaitAndNextQuestion());
                }
            }
            else
            {
                questionStatusB.text = "Wrong Answer!";
            }
        }

        if (hasPlayer1Answered && hasPlayer2Answered && firstCorrectPlayer == 0)
        {
            StartCoroutine(WaitAndNextQuestion());
        }
    }

    void ShowSpellOptions()
    {
        isChoosingSpell = true;
        questionText.text = "Choose Spell:";
        optionTexts[0].text = "A. Light Spell";
        optionTexts[1].text = "B. Heavy Spell";
        optionTexts[2].text = "";
        optionTexts[3].text = "";
    }

    void UpdatePointsUI()
    {
        pointsTextA.text = $"Player 1 Points: {player1Score}";
        pointsTextB.text = $"Player 2 Points: {player2Score}";
    }

    IEnumerator WaitAndNextQuestion()
    {
        yield return new WaitForSeconds(2f);
        DisplayRandomQuestion();
    }
}