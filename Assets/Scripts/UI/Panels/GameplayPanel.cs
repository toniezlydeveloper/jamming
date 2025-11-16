using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Flow.States;
using Internal.Runtime.Dependencies.Core;
using Internal.Runtime.Flow.UI;
using TMPro;
using UI.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels
{
    public interface ISelectionPresenter : IDependency
    {
        void Present(PreSelectionData data);
        void Present(SelectionData data);
        void Present(PreProcessData data);
        void Present(PostProcessingData data);
    }

    public class PreSelectionData
    {
        public Dictionary<Enum, int> CountsByType { get; set; }
        public Action<Enum, IngredientType> SelectionCallback { get; set; }
        public IngredientType Type { get; set; }
        public Enum[] Options { get; set; }
    }

    public class SelectionData
    {
        public Enum SelectedIngredient { get; set; }
        public IngredientType Type { get; set; }
    }

    public class PreProcessData
    {
        public List<IngredientType> IngredientTypes { get; set; } = new();
        public Action<int> RemoveIngredientCallback { get; set; }
        public Action ProcessingCallback { get; set; }
        public List<Enum> Ingredients { get; set; } = new();
        public int MaxCount { get; set; }
        public ProcessorType Type { get; set; }
        public bool CanProcess { get; set; }
    }

    public class PostProcessingData
    {
        public Enum Ingredient { get; set; }
        public IngredientType IngredientType { get; set; }
        public Action ClaimCallback { get; set; }
        public string Text { get; set; }
    }
    
    public class GameplayPanel : AUIPanel, ISelectionPresenter
    {
        [Header("Settings")]
        [SerializeField] private IngredientsSettings settings;
        [SerializeField] private Button quitButton;
        
        [Header("Selection")]
        [SerializeField] private IngredientElement[] buttonsData;
        [SerializeField] private GameObject buttonsHolder;
        
        [Header("Post Selection")]
        [SerializeField] private GameObject nameHolder;
        [SerializeField] private TextMeshProUGUI nameContainer;
        [SerializeField] private GameObject iconHolder;
        [SerializeField] private Image iconContainer;
        
        [Header("Processing")]
        [SerializeField] private IngredientElement[] ingredientsData;
        [SerializeField] private GameObject processingWindowHolder;
        [SerializeField] private TextMeshProUGUI textContainer;
        [SerializeField] private Button processingButton;
        
        [Header("Post Process")]
        [SerializeField] private TextMeshProUGUI postProcessNameContainer;
        [SerializeField] private TextMeshProUGUI postProcessClaimNameContainer;
        [SerializeField] private Image postProcessIconContainer;
        [SerializeField] private GameObject postProcessHolder;
        [SerializeField] private Button postProcessClaimButton;

        private void Start()
        {
            quitButton.onClick.AddListener(RequestTransition<QuitState>);
        }

        public void Present(PreSelectionData data)
        {
            switch (data.Type)
            {
                case IngredientType.None:
                    buttonsHolder.SetActive(false);
                    break;
                case IngredientType.Organic:
                    for (int i = 0; i < data.Options.Length; i++)
                    {
                        Enum value = data.Options[i];
                        IIngredientData ingredientData = GetIngredientData(value, IngredientType.Organic);
                        buttonsData[i].IconContainer.sprite = ingredientData.Icon;
                        buttonsData[i].TextContainer.text = ingredientData.Name;
                        buttonsData[i].CountContainer.text = data.CountsByType[value].ToString();
                        buttonsData[i].Button.interactable = data.CountsByType[value] > 0;
                        buttonsData[i].Holder.SetActive(true);
                        buttonsData[i].Button.onClick.RemoveAllListeners();
                        buttonsData[i].Button.onClick.AddListener(() => data.SelectionCallback.Invoke(value, data.Type));
                    }
                    
                    for (int i = data.Options.Length; i < buttonsData.Length; i++)
                    {
                        buttonsData[i].Holder.SetActive(false);
                    }
                    
                    buttonsHolder.SetActive(true);
                    break;
                case IngredientType.Base:
                    for (int i = 0; i < data.Options.Length; i++)
                    {
                        Enum value = data.Options[i];
                        IIngredientData ingredientData = GetIngredientData(value, IngredientType.Base);
                        buttonsData[i].IconContainer.sprite = ingredientData.Icon;
                        buttonsData[i].TextContainer.text = ingredientData.Name;
                        buttonsData[i].CountContainer.text = data.CountsByType[value].ToString();
                        buttonsData[i].Button.interactable = data.CountsByType[value] > 0;
                        buttonsData[i].Holder.SetActive(true);
                        buttonsData[i].Button.onClick.RemoveAllListeners();
                        buttonsData[i].Button.onClick.AddListener(() => data.SelectionCallback.Invoke(value, data.Type));
                    }
                    
                    for (int i = data.Options.Length; i < buttonsData.Length; i++)
                    {
                        buttonsData[i].Holder.SetActive(false);
                    }
                    
                    buttonsHolder.SetActive(true);
                    break;
                case IngredientType.NonOrganic:
                    for (int i = 0; i < data.Options.Length; i++)
                    {
                        Enum value = data.Options[i];
                        IIngredientData ingredientData = GetIngredientData(value, IngredientType.NonOrganic);
                        buttonsData[i].IconContainer.sprite = ingredientData.Icon;
                        buttonsData[i].TextContainer.text = ingredientData.Name;
                        buttonsData[i].CountContainer.text = data.CountsByType[value].ToString();
                        buttonsData[i].Button.interactable = data.CountsByType[value] > 0;
                        buttonsData[i].Holder.SetActive(true);
                        buttonsData[i].Button.onClick.RemoveAllListeners();
                        buttonsData[i].Button.onClick.AddListener(() => data.SelectionCallback.Invoke(value, data.Type));
                    }
                    
                    for (int i = data.Options.Length; i < buttonsData.Length; i++)
                    {
                        buttonsData[i].Holder.SetActive(false);
                    }
                    
                    buttonsHolder.SetActive(true);
                    break;
                case IngredientType.Potion:
                    for (int i = 0; i < data.Options.Length; i++)
                    {
                        Enum value = data.Options[i];
                        IIngredientData ingredientData = GetIngredientData(value, IngredientType.Potion);
                        buttonsData[i].IconContainer.sprite = ingredientData.Icon;
                        buttonsData[i].TextContainer.text = ingredientData.Name;
                        buttonsData[i].CountContainer.text = data.CountsByType[value].ToString();
                        buttonsData[i].Button.interactable = data.CountsByType[value] > 0;
                        buttonsData[i].Holder.SetActive(true);
                        buttonsData[i].Button.onClick.RemoveAllListeners();
                        buttonsData[i].Button.onClick.AddListener(() => data.SelectionCallback.Invoke(value, data.Type));
                    }
                    
                    for (int i = data.Options.Length; i < buttonsData.Length; i++)
                    {
                        buttonsData[i].Holder.SetActive(false);
                    }
                    
                    buttonsHolder.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Present(SelectionData data)
        {
            switch (data.Type)
            {
                case IngredientType.None:
                    iconHolder.SetActive(false);
                    nameHolder.SetActive(false);
                    break;
                default:
                    IIngredientData ingredientData = GetIngredientData(data.SelectedIngredient, data.Type);
                    iconContainer.sprite = ingredientData.Icon;
                    nameContainer.text = ingredientData.Name;
                    iconHolder.SetActive(true);
                    nameHolder.SetActive(true);
                    break;
            }
        }

        public void Present(PreProcessData data)
        {
            processingWindowHolder.SetActive(data.Type != ProcessorType.None);
            string text = settings.ProcessorsData.FirstOrDefault(d => d.Type == data.Type)?.Text;
            textContainer.text = text;

            for (int i = 0; i < data.Ingredients.Count; i++)
            {
                int localIndex = i;
                IIngredientData ingredientData = GetIngredientData(data.Ingredients[i], data.IngredientTypes[i]);
                ingredientsData[i].IconContainer.sprite = ingredientData.Icon;
                ingredientsData[i].TextContainer.text = ingredientData.Name;
                ingredientsData[i].Button.interactable = true;
                ingredientsData[i].Button.onClick.RemoveAllListeners();
                ingredientsData[i].Button.onClick.AddListener(() => data.RemoveIngredientCallback.Invoke(localIndex));
                ingredientsData[i].Holder.SetActive(true);
            }
            
            for (int i = data.Ingredients.Count; i < ingredientsData.Length && i < data.MaxCount; i++)
            {
                ingredientsData[i].IconContainer.sprite = settings.EmptyIcon;
                ingredientsData[i].TextContainer.text = settings.EmptyText;
                ingredientsData[i].Button.interactable = false;
                ingredientsData[i].Holder.SetActive(true);
            }
            
            for (int i = data.MaxCount; i < ingredientsData.Length; i++)
            {
                ingredientsData[i].Holder.SetActive(false);
            }
            
            processingButton.onClick.RemoveAllListeners();
            processingButton.onClick.AddListener(() => data.ProcessingCallback.Invoke());

            processingButton.interactable = data.CanProcess;
        }

        public void Present(PostProcessingData data)
        {
            postProcessHolder.SetActive(data.IngredientType != IngredientType.None);

            if (data.IngredientType == IngredientType.None)
            {
                return;
            }
            
            IIngredientData ingredientData = GetIngredientData(data.Ingredient, data.IngredientType);
            postProcessClaimButton.onClick.RemoveAllListeners();
            postProcessClaimButton.onClick.AddListener(() => data.ClaimCallback.Invoke());
            postProcessIconContainer.sprite = ingredientData.Icon;
            postProcessNameContainer.text = ingredientData.Name;
            postProcessClaimNameContainer.text = data.Text;
        }

        private IIngredientData GetIngredientData(Enum ingredient, IngredientType ingredientType) => ingredientType switch
        {
            IngredientType.None => null,
            IngredientType.Organic => settings.OrganicsData.First(d => d.Type.Equals(ingredient)),
            IngredientType.Base => settings.BasesData.First(d => d.Type.Equals(ingredient)),
            IngredientType.NonOrganic => settings.NonOrganicsData.First(d => d.Type.Equals(ingredient)),
            IngredientType.Potion => settings.PotionsData.First(d => d.Type.Equals(ingredient)),
            IngredientType.Dust => settings.DustsData.First(d => d.Type.Equals(ingredient)),
            IngredientType.Cut => settings.CutsData.First(d => d.Type.Equals(ingredient)),
            IngredientType.Distilled => settings.DistilledData.First(d => d.Type.Equals(ingredient)),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}