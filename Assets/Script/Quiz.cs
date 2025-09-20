using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
using UnityEngine.InputSystem.XR.Haptics;


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
        new QuizQnA { question= "Munchkin Cat is a cat who loves \n Munchies cookies", answer= false },
        new QuizQnA { question= "Leopard is a kind of lion", answer= false },
        new QuizQnA { question= "Tigers are both carnivor and herbivore", answer= false },
        new QuizQnA { question= "Ocelot is a type of Felinae", answer= true },
        new QuizQnA { question= "Cheetah is a pantherinae.", answer= false },
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
            Handheld.Vibrate();
        }
    }
}
