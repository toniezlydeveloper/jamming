using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using Core;
using Flow.Setup;
using Internal.Runtime.Dependencies.Core;
using Internal.Runtime.Flow.States;
using Internal.Runtime.Utilities;
using Recipes;
using UI.Panels;
using UnityEngine;

namespace Flow.States
{
    public class GameplayState : AState
    {
        public static event Action<Enum, IngredientType> OnAddRequired; 
        
        private ISelectionPresenter _selectionPresenter = DependencyInjector.Get<ISelectionPresenter>();
        private GameReferences _gameReferences;
        private GameSettings _gameSettings;
        private GameInput _input;

        private IngredientType _selectedIngredientType;
        private IngredientHighlight _hoveredHighlight;
        private Enum _selectedIngredient;

        private static readonly List<IngredientType> OutputTypes = new()
        {
            IngredientType.Dust,
            IngredientType.Potion,
            IngredientType.Distilled,
            IngredientType.Cut
        };
        public GameplayState(GameSettings settings, GameInput input, GameReferences references)
        {
            _gameReferences = references;
            _gameSettings = settings;
            _input = input;
        }

        public override void OnEnter()
        {
            EnableInput();
        }

        public override Type OnUpdate()
        {
            HandleHighlight();
            
            if (!GotMouseClick() || IsMouseOverUI())
            {
                return null;
            }

            if (GotMouseClick())
            {
                if (_hoveredHighlight != null && _hoveredHighlight.TryGetComponent(out RecipeBook _))
                {
                    _hoveredHighlight.ToggleHighlight(false);
                    _hoveredHighlight = null;
                    return typeof(RecipesCheckState);
                }
            }

            if (HasSelectedIngredient())
            {
                if (!HandlePostSelection(out IngredientType t) && HasSelectedOutputIngredient())
                {
                    HandleD(t);
                }
            }
            else
            {
                HandlePreSelection();
                HandlePreProcessing();
                HandleX();
            }
            
            return null;
        }

        private void HandlePreSelection()
        {
            if (!IsMouseOverUI() && TryGetValueFromRaycast(out IngredientContainer container))
            {
                ShowSelection(container);
            }
            else
            {
                HideSelection();
            }
        }

        private void HandleHighlight()
        {
            if (TryGetValueFromRaycast(out IngredientHighlight highlight))
            {
                EnableHighlight(highlight);
            }
            else
            {
                DisableHighlight();
            }
        }

        private void ShowSelection(IngredientContainer container)
        {
            switch (container.Type)
            {
                case IngredientType.None:
                    break;
                case IngredientType.Organic:
                    PresentSelection(container, IngredientsContainer.AvailableOrganicTypes);
                    break;
                case IngredientType.Base:
                    PresentSelection(container, IngredientsContainer.AvailableBaseTypes);
                    break;
                case IngredientType.NonOrganic:
                    PresentSelection(container, IngredientsContainer.AvailableNonOrganicTypes);
                    break;
                case IngredientType.Potion:
                    PresentSelection(container, IngredientsContainer.AvailablePotionTypes);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool HandlePostSelection(out IngredientType iT)
        {
            iT = _selectedIngredientType;
            bool hasAdded = false;
            
            if (_hoveredHighlight == null || !_hoveredHighlight.TryGetComponent(out IngredientProcessor processor))
            {
                _gameReferences.SfxPlayer.Play(SfxType.EmptyClick);
            }
            else if (processor.TryAdd(_selectedIngredient, _selectedIngredientType))
            {
                _gameReferences.SfxPlayer.Play(SfxType.CorrectClick);
                hasAdded = true;
            }
            else
            {
                _gameReferences.SfxPlayer.Play(SfxType.WrongClick);
            }

            if (!hasAdded)
            {
                switch (_selectedIngredientType)
                {
                    case IngredientType.None:
                        break;
                    case IngredientType.Organic:
                        IngredientsContainer.AvailableOrganicTypes.Add((OrganicType)_selectedIngredient);
                        break;
                    case IngredientType.Base:
                        IngredientsContainer.AvailableBaseTypes.Add((BaseType)_selectedIngredient);
                        break;
                    case IngredientType.NonOrganic:
                        IngredientsContainer.AvailableNonOrganicTypes.Add((NonOrganicType)_selectedIngredient);
                        break;
                    case IngredientType.Potion:
                        IngredientsContainer.AvailablePotionTypes.Add((PotionType)_selectedIngredient);
                        break;
                    default:
                        OnAddRequired?.Invoke(_selectedIngredient, _selectedIngredientType);
                        break;
                }
            }
            
            _selectionPresenter.Present(new SelectionData());
            _selectedIngredientType = IngredientType.None;
            return hasAdded;
        }

        private void HandleX()
        {
            if (_hoveredHighlight != null && _hoveredHighlight.TryGetComponent(out IngredientsOutput output))
            {
                _selectionPresenter.Present(new PostProcessingData
                {
                    Ingredient = output.Ingredient,
                    IngredientType = output.IngredientType,
                    Text = "Claim!",
                    ClaimCallback = () =>
                    {
                        _selectionPresenter.Present(new PostProcessingData());
                        
                        _selectedIngredientType = output.IngredientType;
                        _selectedIngredient = output.Ingredient;
                    
                        _selectionPresenter.Present(new SelectionData
                        {
                            SelectedIngredient = _selectedIngredient,
                            Type = _selectedIngredientType
                        });
                    
                        output.Add(null, IngredientType.None);
                    }
                });
            }
        }

        private void HandleD(IngredientType t)
        {
            OnAddRequired?.Invoke(_selectedIngredient, t);
        }

        private void HandlePreProcessing()
        {
            if (_hoveredHighlight != null && _hoveredHighlight.TryGetComponent(out IngredientProcessor processor))
            {
                if (!processor.HasAnyIngredient)
                {
                    return;
                }
                
                _selectionPresenter.Present(new PreProcessData
                {
                    ProcessingCallback = () => HandlePreProcess(processor),
                    Ingredients = processor.Ingredients,
                    CanProcess = processor.CanProcess,
                    IngredientTypes = processor.IngredientTypes,
                    MaxCount = processor.MaxCount,
                    Type = processor.Type,
                    RemoveIngredientCallback = index => HandleRemoval(processor, index)
                });
            }
            else
            {
                _selectionPresenter.Present(new PreProcessData());
            }
        }

        private void HideSelection()
        {
            _selectionPresenter.Present(new PreSelectionData());
        }

        private bool HasSelectedOutputIngredient() => OutputTypes.Contains(_selectedIngredientType);
        
        private bool HasSelectedIngredient() => _selectedIngredientType != IngredientType.None;

        private bool GotMouseClick() => _input.MouseClick.action.triggered;

        private bool IsMouseOverUI() => _gameReferences.EventSystem.IsPointerOverGameObject();

        private bool TryGetValueFromRaycast<TValue>(out TValue value) where TValue : class
        {
            Vector2 mousePosition = _input.MousePosition.action.ReadValue<Vector2>();
            Ray ray = _gameReferences.MainCamera.ScreenPointToRay(mousePosition);
            value = null;
            
            if (!ray.Raycast(out RaycastHit hit))
            {
                return false;
            }

            return hit.transform.TryGetComponent(out value);
        }

        private void PresentSelection<TEnum>(IngredientContainer container, List<TEnum> values) where TEnum : Enum
        {
            Dictionary<Enum, int> countsByValue = new();
            
            foreach (TEnum distinctValue in Enum.GetValues(typeof(TEnum)))
            {
                int count = values.Count(v => v.Equals(distinctValue));
                countsByValue.Add(distinctValue, count);
            }

            _selectionPresenter.Present(new PreSelectionData
            {
                CountsByType = countsByValue,
                Options = values.Cast<Enum>().Distinct().ToArray(),
                SelectionCallback = Select,
                Type = container.Type
            });
        }

        private void Select(Enum value, IngredientType type)
        {
            _selectionPresenter.Present(new PreSelectionData());
            _selectionPresenter.Present(new SelectionData
            {
                SelectedIngredient = value,
                Type = type
            });
            _selectedIngredientType = type;
            _selectedIngredient = value;
            
            switch (_selectedIngredientType)
            {
                case IngredientType.None:
                    break;
                case IngredientType.Organic:
                    IngredientsContainer.AvailableOrganicTypes.Remove((OrganicType)_selectedIngredient);
                    break;
                case IngredientType.Base:
                    IngredientsContainer.AvailableBaseTypes.Remove((BaseType)_selectedIngredient);
                    break;
                case IngredientType.NonOrganic:
                    IngredientsContainer.AvailableNonOrganicTypes.Remove((NonOrganicType)_selectedIngredient);
                    break;
                case IngredientType.Potion:
                    IngredientsContainer.AvailablePotionTypes.Remove((PotionType)_selectedIngredient);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void EnableHighlight(IngredientHighlight highlight)
        {
            if (_hoveredHighlight != null)
            {
                _hoveredHighlight.ToggleHighlight(false);
            }

            _hoveredHighlight = highlight;
            _hoveredHighlight.ToggleHighlight(true);
            _gameReferences.SfxPlayer.Play(SfxType.HoverStart);
        }

        private void DisableHighlight()
        {
            if (_hoveredHighlight == null)
            {
                return;
            }

            _gameReferences.SfxPlayer.Play(SfxType.HoverEnd);
            _hoveredHighlight.ToggleHighlight(false);
            _hoveredHighlight = null;
        }

        private void HandlePreProcess(IngredientProcessor processor)
        {
            _selectionPresenter.Present(new PreProcessData());

            Recipe matchingRecipe = RecipesHolder.Recipes.FirstOrDefault(r =>
            {
                if (r.ProcessorType != processor.Type)
                {
                    return false;
                }

                if (r.RequiredIngredients.All(i => processor.Ingredients.Any(i2 => i2.ToString() == i.ToString())))
                {
                    return true;
                }

                return r.IngredientsMatchingCallback?.Invoke(processor.Ingredients) == true;
            });
            
            if (matchingRecipe == null)
            {
                matchingRecipe = RecipesHolder.DefaultRecipesByProcessor[processor.Type];
            }
            
            RecipesSaver.Save(processor.Ingredients, processor.IngredientTypes, matchingRecipe.Output, matchingRecipe.OutputType, processor.Type);
            
            _selectionPresenter.Present(new PostProcessingData
            {
                Ingredient = matchingRecipe.Output,
                IngredientType = matchingRecipe.OutputType,
                Text = "Okay!",
                ClaimCallback = () =>
                {
                    _selectionPresenter.Present(new PostProcessingData());
                }
            });

            processor.Output.Add(matchingRecipe.Output, matchingRecipe.OutputType);
            processor.Clear();
        }

        private void HandleRemoval(IngredientProcessor processor, int index)
        {
            _selectionPresenter.Present(new PreProcessData());
            processor.Remove(index, out Enum ingredient, out IngredientType ingredientType);
            _selectionPresenter.Present(new SelectionData
            {
                SelectedIngredient = ingredient,
                Type = ingredientType
            });
            _selectedIngredientType = ingredientType;
            _selectedIngredient = ingredient;
        }

        private void EnableInput()
        {
            _input.All.Enable();
        }
    }
}