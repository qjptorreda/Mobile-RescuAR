using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace RescuAR.App.ViewModels.Prepare;

public class AssessmentQuestion
{
    public int Number { get; set; }
    public string Category { get; set; } = string.Empty;
    public string QuestionText { get; set; } = string.Empty;
    public List<string> Options { get; set; } = new();
    public string SelectedOption { get; set; } = string.Empty;
}

public partial class AssessmentViewModel : ObservableObject
{
    private readonly List<AssessmentQuestion> _questions = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ProgressValue))]
    [NotifyPropertyChangedFor(nameof(PercentText))]
    [NotifyPropertyChangedFor(nameof(QuestionProgressText))]
    [NotifyPropertyChangedFor(nameof(CanGoPrevious))]
    [NotifyPropertyChangedFor(nameof(IsLastQuestion))]
    [NotifyPropertyChangedFor(nameof(IsNotLastQuestion))]
    public partial int CurrentIndex { get; set; } = 0;

    [ObservableProperty]
    public partial AssessmentQuestion CurrentQuestion { get; set; } = null!;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotFinished))]
    public partial bool IsFinished { get; set; } = false;

    public bool IsNotFinished => !IsFinished;

    // Option Properties for UI binding
    [ObservableProperty]
    public partial string Option1Text { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Option2Text { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Option3Text { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsOption1Selected { get; set; }

    [ObservableProperty]
    public partial bool IsOption2Selected { get; set; }

    [ObservableProperty]
    public partial bool IsOption3Selected { get; set; }

    [ObservableProperty]
    public partial bool IsOption3Visible { get; set; }

    // Next/Submit state
    [ObservableProperty]
    public partial bool CanGoNext { get; set; }

    [ObservableProperty]
    public partial bool CanSubmit { get; set; }

    public double ProgressValue => (CurrentIndex + 1) / 15.0;

    public string PercentText
    {
        get
        {
            double pct = (CurrentIndex + 1) * 100.0 / 15.0;
            return pct >= 100.0 ? "100%" : $"{pct:F2}%";
        }
    }

    public string QuestionProgressText => $"Question {CurrentIndex + 1} of 15";

    public bool CanGoPrevious => CurrentIndex > 0;

    public bool IsLastQuestion => CurrentIndex == 14;
    public bool IsNotLastQuestion => CurrentIndex < 14;

    public AssessmentViewModel()
    {
        InitializeQuestions();
        LoadCurrentQuestion();
    }

    private void InitializeQuestions()
    {
        _questions.Add(new AssessmentQuestion
        {
            Number = 1,
            Category = "Emergency Supplies",
            QuestionText = "Do you currently have at least a 3-day supply of drinking water for your household?",
            Options = new List<string> { "Yes", "No" }
        });
        _questions.Add(new AssessmentQuestion
        {
            Number = 2,
            Category = "Emergency Supplies",
            QuestionText = "Do you have non-perishable food supplies that can last for at least 3 days?",
            Options = new List<string> { "Yes", "No" }
        });
        _questions.Add(new AssessmentQuestion
        {
            Number = 3,
            Category = "Emergency Supplies",
            QuestionText = "Do you have a stocked first-aid kit available at home?",
            Options = new List<string> { "Yes", "No" }
        });
        _questions.Add(new AssessmentQuestion
        {
            Number = 4,
            Category = "Emergency Supplies",
            QuestionText = "Do you have a working flashlight with spare batteries?",
            Options = new List<string> { "Yes", "No" }
        });
        _questions.Add(new AssessmentQuestion
        {
            Number = 5,
            Category = "Emergency Supplies",
            QuestionText = "Do you have a charged power bank or backup power source available?",
            Options = new List<string> { "Yes", "No" }
        });
        _questions.Add(new AssessmentQuestion
        {
            Number = 6,
            Category = "Evacuation Readiness",
            QuestionText = "Do you know the nearest designated evacuation center in your area?",
            Options = new List<string> { "Yes", "No" }
        });
        _questions.Add(new AssessmentQuestion
        {
            Number = 7,
            Category = "Evacuation Readiness",
            QuestionText = "Have you reviewed an evacuation route to the nearest evacuation center within the last six months?",
            Options = new List<string> { "Yes", "No" }
        });
        _questions.Add(new AssessmentQuestion
        {
            Number = 8,
            Category = "Evacuation Readiness",
            QuestionText = "Can your household evacuate within 15 minutes if instructed by authorities?",
            Options = new List<string> { "Yes", "Not Sure", "No" }
        });
        _questions.Add(new AssessmentQuestion
        {
            Number = 9,
            Category = "Evacuation Readiness",
            QuestionText = "Do you know the recommended actions during floods, earthquakes, and typhoons?",
            Options = new List<string> { "Yes", "Partially", "No" }
        });
        _questions.Add(new AssessmentQuestion
        {
            Number = 10,
            Category = "Emergency Communication",
            QuestionText = "Have you configured emergency contacts that can be reached during an emergency?",
            Options = new List<string> { "Yes", "No" }
        });
        _questions.Add(new AssessmentQuestion
        {
            Number = 11,
            Category = "Emergency Communication",
            QuestionText = "Do you know how to contact local emergency hotlines if needed?",
            Options = new List<string> { "Yes", "No" }
        });
        _questions.Add(new AssessmentQuestion
        {
            Number = 12,
            Category = "Emergency Communication",
            QuestionText = "Do members of your household know how to communicate if mobile internet becomes unavailable?",
            Options = new List<string> { "Yes", "Not Sure", "No" }
        });
        _questions.Add(new AssessmentQuestion
        {
            Number = 13,
            Category = "Household Preparedness",
            QuestionText = "Does your household have a designated meeting point in case family members become separated?",
            Options = new List<string> { "Yes", "No" }
        });
        _questions.Add(new AssessmentQuestion
        {
            Number = 14,
            Category = "Household Preparedness",
            QuestionText = "Have household members discussed what to do during an emergency evacuation?",
            Options = new List<string> { "Yes", "No" }
        });
        _questions.Add(new AssessmentQuestion
        {
            Number = 15,
            Category = "Household Preparedness",
            QuestionText = "Does your household have a plan for pets, infants, or elderly members during an evacuation?",
            Options = new List<string> { "Yes", "Not Applicable", "No" }
        });
    }

    private void LoadCurrentQuestion()
    {
        if (CurrentIndex < 0 || CurrentIndex >= _questions.Count) return;

        CurrentQuestion = _questions[CurrentIndex];

        // Setup options
        Option1Text = CurrentQuestion.Options.Count > 0 ? CurrentQuestion.Options[0] : string.Empty;
        Option2Text = CurrentQuestion.Options.Count > 1 ? CurrentQuestion.Options[1] : string.Empty;
        Option3Text = CurrentQuestion.Options.Count > 2 ? CurrentQuestion.Options[2] : string.Empty;

        IsOption3Visible = CurrentQuestion.Options.Count > 2;

        // Load selection status
        IsOption1Selected = CurrentQuestion.SelectedOption == Option1Text;
        IsOption2Selected = CurrentQuestion.SelectedOption == Option2Text;
        IsOption3Selected = CurrentQuestion.SelectedOption == Option3Text;

        UpdateNavigationStates();
    }

    private void UpdateNavigationStates()
    {
        bool hasSelection = !string.IsNullOrEmpty(CurrentQuestion.SelectedOption);
        CanGoNext = CurrentIndex < 14 && hasSelection;
        CanSubmit = CurrentIndex == 14 && hasSelection;
    }

    [RelayCommand]
    private void SelectOption(string optionNumber)
    {
        if (CurrentQuestion == null) return;

        string selectedValue = optionNumber switch
        {
            "1" => Option1Text,
            "2" => Option2Text,
            "3" => Option3Text,
            _ => string.Empty
        };

        CurrentQuestion.SelectedOption = selectedValue;

        IsOption1Selected = selectedValue == Option1Text;
        IsOption2Selected = selectedValue == Option2Text;
        IsOption3Selected = selectedValue == Option3Text;

        UpdateNavigationStates();
    }

    [RelayCommand]
    private void Next()
    {
        if (CurrentIndex < 14)
        {
            CurrentIndex++;
            LoadCurrentQuestion();
        }
    }

    [RelayCommand]
    private void Previous()
    {
        if (CurrentIndex > 0)
        {
            CurrentIndex--;
            LoadCurrentQuestion();
        }
    }

    [RelayCommand]
    private void Submit()
    {
        IsFinished = true;
    }

    [RelayCommand]
    private async Task GoToPreparationAssessmentAsync()
    {
        if (Shell.Current != null)
        {
            // Navigate back to the PASS main page
            await Shell.Current.GoToAsync("..");
        }
    }

    [RelayCommand]
    private async Task BackAsync()
    {
        if (Shell.Current != null)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
