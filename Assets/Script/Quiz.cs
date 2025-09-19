using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR.Interaction.Toolkit.Inputs.Readers;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;


public class Quiz : MonoBehaviour
{
    private Label questionText;
    private Button yesButton;
    private Button noButton;
    private struct QuizQnA
    {
        public string question;
        public bool answer;
    }

    private QuizQnA[] quizData = new QuizQnA[]
    {
        new QuizQnA { question= "Question 1", answer= true },
        new QuizQnA { question= "Question 2", answer= true },
        new QuizQnA { question= "Question 3", answer= true },
        new QuizQnA { question= "Question 4", answer= true },
        new QuizQnA { question= "Question 5", answer= true },
    };
    private int question_counter = 0;
    private int score = 0;

    private void Awake()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        VisualElement root = null;

        if (uiDocument != null)
        {
            root = uiDocument.rootVisualElement;
        }

        UIBinding(root);
        UpdateQuestion();
    }
    private void UIBinding(VisualElement root)
    {
        questionText = root.Q<Label>("question-text");

        yesButton = root.Q<Button>("yes-button");
        noButton = root.Q<Button>("no-button");

        if (yesButton != null)
        {
            yesButton.clicked += onYesClick;
        }
        if (noButton != null)
        {
            noButton.clicked += onNoClick;
        }
    }

    private void UpdateQuestion()
    {
        if (question_counter < quizData.Length)
        {
            questionText.text = quizData[question_counter].question;
        }
        else if (question_counter >= quizData.Length)
        {
            yesButton.SetEnabled(false);
            noButton.SetEnabled(false);
            questionText.text = "Well done!, \n Score: " + score;
        }
    }
    private void onYesClick()
    {
        checkAnswer(true);
        UpdateQuestion();
    }
    private void onNoClick()
    {
        checkAnswer(false);
        UpdateQuestion();
    }

    private void checkAnswer(bool answer)
    {
        bool actualAns = quizData[question_counter].answer;
        question_counter++;
        if (answer == actualAns)
        {
            score++;
        }
        else
        {
            Debug.Log("Noob");
        }
    }
}
