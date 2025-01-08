using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ChatbotUIController : MonoBehaviour
{
    public Button chatButton;
    public TMP_InputField userInputField;
    public TMP_Text chatbotResponseText;
    public Button submitButton;
    private bool isChatUIVisible = false;

    // Predefined question-answer pairs
    private Dictionary<string, string> qaDictionary = new Dictionary<string, string>()
    {
        { "what is the goal of the game", "The goal of the game is to find the key to escape from the house." },
        { "what is the passcode on the number lock", "The passcode is 7159." },
        { "what should i do with the key", "You should open the left drawer to get items from it." },
        { "what should i do with the knife", "You should cut the cardboard box." },
        { "where is the cardboard box", "It is behind the sofa." },
        { "where is the final key", "It is in the locker, which is on the wall near the fireplace." },
        { "what is the solution for the chess puzzle", "It is B1C2." },
        { "what should i do with the weights puzzle", "Match the weights to get an item." },
        { "what is the passcode for the final locker", "The passcode is 8426." },
        { "how should i open the drawer", "You need a key to open the drawer." },
        { "where should i find the key for the drawer", "You should find the key in the locker." },
        { "what is the passcode of the locker", "You should find the passcode; the clues are hidden in the rooms." }
    };

    // Hints and clues for each question
    private Dictionary<string, List<string>> hintsDictionary = new Dictionary<string, List<string>>()
    {
        { "what is the goal of the game", new List<string> { "Look around carefully, everything you need is inside the house.", "The key to success is hidden in the smallest details." } },
        { "what is the passcode on the number lock", new List<string> { "Have you checked all the notes scattered around?", "Look for numbers that seem out of place." } },
        { "what should i do with the key", new List<string> { "Think about which locked objects you haven't opened yet.", "Keys are meant to unlock, don't they?" } },
        { "what should i do with the knife", new List<string> { "Sharp objects can cut through things; find something that can be cut.", "You can't force a lock with it, but you can cut other things." } },
        { "where is the cardboard box", new List<string> { "Check places where items might be stored.", "Boxes usually hide in corners or behind large objects." } },
        { "where is the final key", new List<string> { "The locker is well hidden, but it's in plain sight.", "Try checking near the fireplace." } },
        { "what is the solution for the chess puzzle", new List<string> { "Focus on the movements of the knight.", "Think of the chessboard as a path for specific pieces." } },
        { "what should i do with the weights puzzle", new List<string> { "Balance is key to solving this.", "Try to find the correct combination of weights." } },
        { "what is the passcode for the final locker", new List<string> { "You might have seen this number before.", "Check the last clue you found." } },
        { "how should i open the drawer", new List<string> { "You need something to unlock it.", "Have you found a key?" } },
        { "where should i find the key for the drawer", new List<string> { "Keys are usually hidden in locked places.", "Check the locker for a key." } },
        { "what is the passcode of the locker", new List<string> { "The code is hidden somewhere in the room.", "Look for numbers that might be a code." } }
    };

    // Random general hints
    private List<string> generalHints = new List<string>()
    {
        "Pay attention to your surroundings, every detail counts.",
        "Sometimes the most obvious places hold the greatest secrets.",
        "Have you checked all the drawers and cabinets?",
        "Try interacting with objects you haven't touched yet."
    };

    // Welcome greetings
    private List<string> welcomeGreetings = new List<string>()
    {
        "Hello! How can I assist you in your game?",
        "Hi there! Need any help with the puzzles?",
        "Greetings! How can I guide you today?",
        "Welcome! I'm here to help with any game-related questions."
    };

    void Start()
    {
        // Initialize components
        userInputField = GameObject.Find("UserInputField").GetComponent<TMP_InputField>();
        chatbotResponseText = GameObject.Find("ChatbotResponseText").GetComponent<TMP_Text>();

        // Initially hide the input field, chatbot response, and submit button
        SetChatUIVisibility(false);

        // Add a listener to the chat button to toggle input fields when clicked
        chatButton.onClick.AddListener(ToggleChatUI);

        // Add a listener to the submit button to handle user input when clicked
        submitButton.onClick.AddListener(OnSubmit);

        // Add a listener for the Enter key (TMP_InputField's submit action)
        userInputField.onSubmit.AddListener(delegate { OnSubmit(); });
    }

    void ToggleChatUI()
    {
        // Toggle the visibility state
        isChatUIVisible = !isChatUIVisible;
        SetChatUIVisibility(isChatUIVisible);
    }

    void SetChatUIVisibility(bool isVisible)
    {
        userInputField.gameObject.SetActive(isVisible);
        chatbotResponseText.gameObject.SetActive(isVisible);
        submitButton.gameObject.SetActive(isVisible);

        if (isVisible)
        {
            userInputField.Select();
            userInputField.ActivateInputField();
        }
    }

    void OnSubmit()
    {
        string userInput = userInputField.text.ToLower().Trim();

        if (!string.IsNullOrEmpty(userInput))
        {
            // Handle chatbot response logic
            string response = GetChatbotResponse(userInput);
            chatbotResponseText.text = response;
            userInputField.text = "";

            // If the game has ended, ask for feedback
            if (userInput.Contains("game end") || userInput.Contains("game over"))// Trigger for game end
            {
                AskForFeedback();
            }
        }
    }

    string GetChatbotResponse(string userInput)
    {
        // Handle greetings
        if (userInput.Contains("hi") || userInput.Contains("hello"))
        {
            return GetRandomWelcomeGreeting();
        }

        // Handle requests for a general hint
        if (userInput.Contains("provide me a hint"))
        {
            return GetRandomGeneralHint();
        }

        // Check if the input matches any predefined questions
        if (qaDictionary.ContainsKey(userInput))
        {
            return qaDictionary[userInput];
        }

        // Handle requests for hints or clues
        if (userInput.Contains("hint") || userInput.Contains("clue"))
        {
            return GetRandomHint(userInput);
        }

        // Handle edge cases and error responses
        if (userInput.Contains("knife") && userInput.Contains("lock"))
        {
            return "I'm not sure about that action. The knife is intended to cut the cardboard box.";
        }
        else if (userInput.Contains("help"))
        {
            return "Hints are not available. Please refer to the game’s instructions for help.";
        }
        else if (userInput.Contains("key") && userInput.Contains("find"))
        {
            return "The key is needed to escape from the house, so finding it is essential.";
        }
        else if (userInput == "asdfghjkl")
        {
            return "I didn't understand that. Could you please rephrase your question?";
        }
        else if (userInput.Contains("joke"))
        {
            return "I'm here to help with game-related questions. How can I assist you with the game?";
        }
        else if (userInput.Contains("time"))
        {
            return "I'm not able to provide the current time. How can I help with the game?";
        }

        // Default response for unhandled inputs
        return "I didn't understand that. Could you please rephrase your question?";
    }

    string GetRandomHint(string userInput)
    {
        // Try to find a related question in the hints dictionary
        foreach (var question in hintsDictionary.Keys)
        {
            if (userInput.Contains(question))
            {
                List<string> hints = hintsDictionary[question];
                return hints[Random.Range(0, hints.Count)]; // Return a random hint from the list
            }
        }

        // Default hint response if no specific hint found
        return "I don't have a specific hint for that, but keep searching and you'll find what you need!";
    }

    string GetRandomGeneralHint()
    {
        // Return a random general hint from the list
        return generalHints[Random.Range(0, generalHints.Count)];
    }

    string GetRandomWelcomeGreeting()
    {
        // Return a random welcome greeting from the list
        return welcomeGreetings[Random.Range(0, welcomeGreetings.Count)];
    }

    void AskForFeedback()
    {
        chatbotResponseText.text = "Thank you for playing the game! How was your experience? Please share your feedback.";
        userInputField.onSubmit.RemoveAllListeners();
        userInputField.onSubmit.AddListener(delegate { AnalyzeSentiment(userInputField.text); });
    }

    void AnalyzeSentiment(string feedback)
    {
        string sentiment = GetSentiment(feedback);
        chatbotResponseText.text = $"Your feedback was {sentiment}. Thank you!";
        userInputField.text = "";
    }

    string GetSentiment(string feedback)
    {
        feedback = feedback.ToLower();

        if (feedback.Contains("great") || feedback.Contains("awesome") || feedback.Contains("loved"))
        {
            return "positive";
        }
        else if (feedback.Contains("didn't like") || feedback.Contains("hate") || feedback.Contains("terrible"))
        {
            return "negative";
        }
        else
        {
            return "neutral";
        }
    }
}