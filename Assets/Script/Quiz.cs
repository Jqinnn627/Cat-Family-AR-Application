using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.SceneManagement;

public class Quiz : MonoBehaviour
{   
    private Label questionText;
    private Button yesButton;
    private Button noButton;
    private ProgressBar progressBar;

    private VisualElement quizCompletion;
    private Button restartQuizButton;
    private Label scoreText;
    private VisualElement quizStart;
    private Button menuButton;

    public AudioClip correctSound;
    public AudioClip wrongSound;
    private AudioSource audioSource;
    private VisualElement correctIndicator;
    private VisualElement wrongIndicator;
 
    public class QuizQnA
    {
        public string question;
        public bool answer;
    }
    public class QuizData
    {
        public QuizQnA[] en;
        public QuizQnA[] cn;
        public QuizQnA[] malay;
    }
    public QuizData quizData;
    private QuizQnA[] currentQuiz = null;
    private int question_counter = 0;
    private int score = 0;

    // Functions 
    private void Awake()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        VisualElement root = null;

        if (uiDocument != null)
        {
            root = uiDocument.rootVisualElement;
        }

        audioSource = GetComponent<AudioSource>();
        UIBinding(root);
        
        // Ensure indicators are hidden at start AND set their position to Absolute
        if (correctIndicator != null)
        {
            correctIndicator.style.position = Position.Absolute;
            correctIndicator.style.display = DisplayStyle.None;
        }
        if (wrongIndicator != null)
        {
            wrongIndicator.style.position = Position.Absolute;
            wrongIndicator.style.display = DisplayStyle.None;
        }
    }
    private void Start()
    {
        quizData = new QuizData
        {
            // --- English Questions ---
            en = new QuizQnA[]
            {
                new QuizQnA { question = "Domestic cats are suitable \n in houses.", answer = true },
                new QuizQnA { question = "Leopard is a kind of lion", answer = false },
                new QuizQnA { question = "Jaguar are both carnivore \n and herbivore", answer = false },
                new QuizQnA { question = "Lioness is a type of Felinae", answer = false },
                new QuizQnA { question = "Floppa is a carnivore.", answer = true },
            },

            // --- Chinese Questions ---
            cn = new QuizQnA[]
            {
                new QuizQnA { question = "家猫适合在屋内生活。", answer = true },
                new QuizQnA { question = "豹子是狮子的一种", answer = false },
                new QuizQnA { question = "美洲虎既是食肉动物，\n也是食草动物", answer = false },
                new QuizQnA { question = "母狮是猫科动物的一种", answer = false },
                new QuizQnA { question = "Floppa 是食肉动物", answer = true },
            },

            // --- Malay Questions ---
            malay = new QuizQnA[]
            {
                new QuizQnA { question = "Kucing domestik sesuai \n di dalam rumah.", answer = true },
                new QuizQnA { question = "Harimau Bintang adalah \n sejenis singa", answer = false },
                new QuizQnA { question = "Jaguar adalah \n karnivor dan herbivor", answer = false },
                new QuizQnA { question = "Singa betina adalah \n sejenis Felinae", answer = false },
                new QuizQnA { question = "Floppa ialah karnivor.", answer = true },
            }
        };

        // Get the chosen language preference
        string chosenLanguage = LanguageScript.GetLanguage();

        // Load and display the quiz data based on the chosen language
        LoadQuiz(chosenLanguage);
    }

    public void LoadQuiz(string lang)
    {

        switch (lang)
        {
            case "en":
                currentQuiz = quizData.en;
                yesButton.text = "True";
                noButton.text = "False";
                break;
            case "cn":
                currentQuiz = quizData.cn;
                yesButton.text = "对";
                noButton.text = "错";
                break;
            case "malay":
                currentQuiz = quizData.malay;
                yesButton.text = "Betul";
                noButton.text = "Salah";
                break;
            default:
                // Fallback to English if language is not set
                currentQuiz = quizData.en;
                yesButton.text = "True";
                noButton.text = "False";
                break;
        }

        // Now you can use the 'currentQuiz' array to populate your UI
        if (currentQuiz != null && currentQuiz.Length > 0)
        {
            // Example: display the first question
            questionText.text = currentQuiz[0].question;
        }
        UpdateQuestion();
    }

    private void UIBinding(VisualElement root)
    {
        quizStart = root.Q<VisualElement>("quiz-start");
        questionText = root.Q<Label>("question-text");
        yesButton = root.Q<Button>("yes-button");
        noButton = root.Q<Button>("no-button");
        progressBar = root.Q<ProgressBar>("progress-bar");

        quizCompletion = root.Q<VisualElement>("quiz-completion");
        restartQuizButton = root.Q<Button>("restart-button");
        scoreText = root.Q<Label>("score-text");
        menuButton = root.Q<Button>("menu-button");

        if (yesButton != null)
        {
            yesButton.clicked += () => ProcessAnswer(true, yesButton);
        }
        if (noButton != null)
        {
            noButton.clicked += () => ProcessAnswer(false, noButton);
        }
        if (restartQuizButton != null)
        {
            restartQuizButton.clicked += onRestartClick;
        }
        if (menuButton != null)
        {
            menuButton.clicked += onMenuClick;
        }
        correctIndicator = root.Q<VisualElement>("correct-indicator");
        wrongIndicator = root.Q<VisualElement>("wrong-indicator");
    }

    private void UpdateQuestion()
    {
        if (question_counter < currentQuiz.Length)
        {
            questionText.text = currentQuiz[question_counter].question;
            IncrementProgressBar();
        }
        else if (question_counter >= currentQuiz.Length)
        {
            quizStart.style.display = DisplayStyle.None;
            QuizCompletionUI();
        }
    }


    private void IncrementProgressBar()
    {
        float currentValue = progressBar.value;


        if (currentValue >= progressBar.highValue)
        {
            progressBar.value = progressBar.lowValue;
        }
        else
        {
            progressBar.value++;
        }
    }

    private void QuizCompletionUI()
    {
        quizCompletion.style.display = DisplayStyle.Flex;
        scoreText.text = "Well done!, \n Score: " + score;
        
    }

    private void onRestartClick()
    {
        restartQuiz();
    }
    private void restartQuiz()
    {
        quizCompletion.style.display = DisplayStyle.None;
        quizStart.style.display= DisplayStyle.Flex;
        question_counter = 0;
        score = 0;
        IncrementProgressBar();
        UpdateQuestion();
    }

    public void onMenuClick()
    {
        SceneManager.LoadScene("MainMenu");
    }


    private void ProcessAnswer(bool answer, Button clickedButton)
    {
        bool isCorrect = (answer == currentQuiz[question_counter].answer);

        if (isCorrect)
        {
            score++;
            if (audioSource != null && correctSound != null)
            {
                audioSource.PlayOneShot(correctSound);
            }
            // Start the new animation for the CORRECT indicator
            StartCoroutine(AnimateIndicatorCoroutine(correctIndicator));
        }
        else
        {
            Handheld.Vibrate();
            audioSource.PlayOneShot(wrongSound);
            // Start the new animation for the WRONG indicator
            StartCoroutine(AnimateIndicatorCoroutine(wrongIndicator));
        }

        // Move to the next question after a short delay to allow animation to start
        StartCoroutine(NextQuestionAfterDelay(0.2f));
    }

    private IEnumerator NextQuestionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        question_counter++;
        UpdateQuestion();
    }

    private IEnumerator AnimateIndicatorCoroutine(VisualElement indicator)
    {
        indicator.style.display = DisplayStyle.Flex; // Make it visible

        float duration = 0.8f; // Total animation time
        float elapsedTime = 0f;

        indicator.style.left = new StyleLength(new Length(0, LengthUnit.Percent));
        indicator.style.top = new StyleLength(new Length(0, LengthUnit.Percent));

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration; 
            float alpha;
            if (progress < 0.5f)
            {
                // Fade in
                alpha = Mathf.Lerp(0f, 1f, progress / 0.5f);
            }
            else
            {
                // Fade out
                alpha = Mathf.Lerp(1f, 0f, (progress - 0.5f) / 0.5f);
            }
            indicator.style.opacity = alpha;
            float positionPercent = Mathf.Lerp(0f, 100f, progress);
            indicator.style.left = new StyleLength(new Length(positionPercent, LengthUnit.Percent));
            indicator.style.top = new StyleLength(new Length(positionPercent, LengthUnit.Percent));

            yield return null;
        }
        indicator.style.display = DisplayStyle.None;
    }
}   
