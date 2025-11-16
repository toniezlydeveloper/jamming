using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using Core;
using Flow.Setup;
using Highscore;
using Internal.Runtime.Dependencies.Core;
using Internal.Runtime.Flow.States;
using Internal.Runtime.Utilities;
using NUnit.Framework;
using Recipes;
using UI.Elements;
using UI.Panels;
using UnityEngine;

namespace Flow.States
{
    public class GameplayState : AState
    {
        public static event Action<Enum, IngredientType> OnAddRequired; 
        public static event Action<IngredientType, object> OnRefreshRequired; 
        
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

        public override void OnExit()
        {
            StopSound();
        }

        public override Type OnUpdate()
        {
            if (ProcessingWindow.IsProcessing)
            {
                return null;
            }
            
            HandleHighlight();

            if (GotPauseClick())
            {
                return typeof(PauseState);
            }
            
            if (!GotMouseClick() || IsMouseOverUI())
            {
                return null;
            }

            if (GotMouseClick())
            {
                if (TryGetRecipeBook())
                {
                    return typeof(RecipesCheckState);
                }
            }

            if (GotMouseClick())
            {
                if (TryGetResultsBox())
                {
                    return typeof(ResultsCheckState);
                }
            }

            if (GotMouseClick())
            {
                if (TryGetTrash()) return null;
            }

            if (HasSelectedIngredient())
            {
                if (!HandlePostSelection(out IngredientType t) && HasSelectedOutputIngredient())
                {
                    AddIngredientBack(t);
                }
            }
            else
            {
                HandlePreSelection();
                HandlePreProcessing();
                HandleOutputPickUp();
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
            
            _gameReferences.SfxPlayer.Play(SfxType.CorrectClick);
        }

        private bool TryGetRecipeBook()
        {
            if (_hoveredHighlight != null && _hoveredHighlight.TryGetComponent(out RecipeBook _))
            {
                _gameReferences.SfxPlayer.Play(SfxType.CorrectClick);
                _hoveredHighlight.ToggleHighlight(false);
                _hoveredHighlight = null;
                return true;
            }

            return false;
        }

        private bool TryGetResultsBox()
        {
            if (_hoveredHighlight != null && _hoveredHighlight.TryGetComponent(out ResultsBox _))
            {
                _gameReferences.SfxPlayer.Play(SfxType.CorrectClick);
                _hoveredHighlight.ToggleHighlight(false);
                _hoveredHighlight = null;
                return true;
            }

            return false;
        }

        private bool TryGetTrash()
        {
            if (_hoveredHighlight != null && _hoveredHighlight.TryGetComponent(out IngredientsTrash _))
            {
                _gameReferences.SfxPlayer.Play(SfxType.CorrectClick);
                _selectedIngredient = null;
                _selectedIngredientType = IngredientType.None;
                _selectionPresenter.Present(new SelectionData());
                return true;
            }

            return false;
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
                        OnRefreshRequired?.Invoke(IngredientType.Organic, IngredientsContainer.AvailableOrganicTypes);
                        break;
                    case IngredientType.Base:
                        IngredientsContainer.AvailableBaseTypes.Add((BaseType)_selectedIngredient);
                        OnRefreshRequired?.Invoke(IngredientType.Organic, IngredientsContainer.AvailableOrganicTypes);
                        break;
                    case IngredientType.NonOrganic:
                        IngredientsContainer.AvailableNonOrganicTypes.Add((NonOrganicType)_selectedIngredient);
                        OnRefreshRequired?.Invoke(IngredientType.Organic, IngredientsContainer.AvailableOrganicTypes);
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

        private void HandleOutputPickUp()
        {
            if (_hoveredHighlight != null && _hoveredHighlight.TryGetComponent(out IngredientsOutput output))
            {
                _gameReferences.SfxPlayer.Play(SfxType.CorrectClick);
                _selectionPresenter.Present(new PostProcessingData
                {
                    Ingredient = output.Ingredient,
                    IngredientType = output.IngredientType,
                    Text = "Pick up",
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
                    
                        _gameReferences.SfxPlayer.Play(SfxType.CorrectClick);
                        output.Add(null, IngredientType.None);
                    }
                });
            }
        }

        private void AddIngredientBack(IngredientType t)
        {
            OnAddRequired?.Invoke(_selectedIngredient, t);
        }

        private void HandlePreProcessing()
        {
            if (_hoveredHighlight != null && _hoveredHighlight.TryGetComponent(out IngredientProcessor processor))
            {
                if (!processor.HasAnyIngredient)
                {
                    _gameReferences.SfxPlayer.Play(SfxType.WrongClick);
                    return;
                }
                
                _gameReferences.SfxPlayer.Play(SfxType.CorrectClick);
                
                Recipe matchingRecipe = RecipesHolder.Recipes.FirstOrDefault(r =>
                {
                    if (r.ProcessorType != processor.Type)
                    {
                        return false;
                    }

                    if (r.IngredientsMatchingCallback == null && r.RequiredIngredients.All(i => processor.Ingredients.Any(i2 => i2.ToString() == i.ToString())))
                    {
                        return true;
                    }

                    return r.IngredientsMatchingCallback?.Invoke(processor.Ingredients) == true;
                });
            
                if (matchingRecipe == null)
                {
                    matchingRecipe = RecipesHolder.DefaultRecipesByProcessor[processor.Type];
                }
                
                _selectionPresenter.Present(new PreProcessData
                {
                    ProcessingCallback = () => HandlePreProcess(processor),
                    ProcessingFailCallback = () => HandlePreProcessFail(processor),
                    ClickCallback = () => ClickCallback(processor),
                    NormalizedChances = matchingRecipe.NormalizedChances,
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
                _gameReferences.SfxPlayer.Play(SfxType.WrongClick);
            }
        }

        private void HideSelection()
        {
            _selectionPresenter.Present(new PreSelectionData());
        }

        private bool HasSelectedOutputIngredient() => OutputTypes.Contains(_selectedIngredientType);
        
        private bool HasSelectedIngredient() => _selectedIngredientType != IngredientType.None;

        private bool GotMouseClick() => _input.MouseClick.action.triggered;
        private bool GotPauseClick() => _input.PauseKey.action.triggered;

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
            _gameReferences.SfxPlayer.Play(SfxType.CorrectClick);
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
                    OnRefreshRequired?.Invoke(IngredientType.Organic, IngredientsContainer.AvailableOrganicTypes);
                    break;
                case IngredientType.Base:
                    IngredientsContainer.AvailableBaseTypes.Remove((BaseType)_selectedIngredient);
                    OnRefreshRequired?.Invoke(IngredientType.Base, IngredientsContainer.AvailableBaseTypes);
                    break;
                case IngredientType.NonOrganic:
                    IngredientsContainer.AvailableNonOrganicTypes.Remove((NonOrganicType)_selectedIngredient);
                    OnRefreshRequired?.Invoke(IngredientType.NonOrganic, IngredientsContainer.AvailableNonOrganicTypes);
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
        }

        private void DisableHighlight()
        {
            if (_hoveredHighlight == null)
            {
                return;
            }

            _hoveredHighlight.ToggleHighlight(false);
            _hoveredHighlight = null;
        }

        private void HandlePreProcess(IngredientProcessor processor)
        {
            _gameReferences.SoundPlayer.StopAll();
            Recipe matchingRecipe = RecipesHolder.Recipes.FirstOrDefault(r =>
            {
                if (r.ProcessorType != processor.Type)
                {
                    return false;
                }

                if (r.IngredientsMatchingCallback == null && r.RequiredIngredients.All(i => processor.Ingredients.Any(i2 => i2.ToString() == i.ToString())))
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
            
            _gameReferences.SfxPlayer.Play(SfxType.Success);
            _selectionPresenter.Present(new PostProcessingData
            {
                Ingredient = matchingRecipe.Output,
                IngredientType = matchingRecipe.OutputType,
                Text = "Pick up",
                ClaimCallback = () =>
                {
                        _selectionPresenter.Present(new PostProcessingData());
                        
                        _selectedIngredientType = processor.Output.IngredientType;
                        _selectedIngredient = processor.Output.Ingredient;
                    
                        _selectionPresenter.Present(new SelectionData
                        {
                            SelectedIngredient = _selectedIngredient,
                            Type = _selectedIngredientType
                        });
                    
                        _gameReferences.SfxPlayer.Play(SfxType.CorrectClick);
                        processor.Output.Add(null, IngredientType.None);
                },
                Fail = false
            });

            processor.Output.Add(matchingRecipe.Output, matchingRecipe.OutputType);
            processor.Clear();
        }

        private void ClickCallback(IngredientProcessor processor)
        {
            switch (processor.Type)
            {
                case ProcessorType.None:
                    break;
                case ProcessorType.Distiller:
                    _gameReferences.SoundPlayer.Play(SoundType.Distilling);
                    break;
                case ProcessorType.CuttingBoard:
                    _gameReferences.SoundPlayer.Play(SoundType.Cutting);
                    break;
                case ProcessorType.Mortar:
                    _gameReferences.SoundPlayer.Play(SoundType.Mashing);
                    break;
                case ProcessorType.Cauldron:
                    _gameReferences.SoundPlayer.Play(SoundType.Brewing);
                    break;
                case ProcessorType.DistillerOutput:
                    break;
                case ProcessorType.CuttingOutput:
                    break;
                case ProcessorType.MortarOutput:
                    break;
                case ProcessorType.CauldronOutput:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _selectionPresenter.Present(new PreProcessData());
            _gameReferences.SfxPlayer.Play(SfxType.CorrectClick);
        }
        
        private void HandlePreProcessFail(IngredientProcessor processor)
        {
            _gameReferences.SoundPlayer.StopAll();
            switch (processor.Type)
            {
                case ProcessorType.None:
                    break;
                case ProcessorType.Distiller:
                    _gameReferences.SfxPlayer.Play(SfxType.Fail);
                    break;
                case ProcessorType.CuttingBoard:
                    _gameReferences.SfxPlayer.Play(SfxType.Fail);
                    break;
                case ProcessorType.Mortar:
                    _gameReferences.SfxPlayer.Play(SfxType.Fail);
                    break;
                case ProcessorType.Cauldron:
                    _gameReferences.SfxPlayer.Play(SfxType.Explosion);
                    break;
                case ProcessorType.DistillerOutput:
                    break;
                case ProcessorType.CuttingOutput:
                    break;
                case ProcessorType.MortarOutput:
                    break;
                case ProcessorType.CauldronOutput:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _selectionPresenter.Present(new PostProcessingData
            {
                Text = "Accept",
                ClaimCallback = () =>
                {
                    _selectionPresenter.Present(new PostProcessingData());
                    _gameReferences.SfxPlayer.Play(SfxType.CorrectClick);
                },
                Fail = true
            });

            processor.Clear();
        }

        private void HandleRemoval(IngredientProcessor processor, int index)
        {
            _selectionPresenter.Present(new PreProcessData());
            processor.Remove(index, out Enum ingredient, out IngredientType ingredientType);
            _gameReferences.SfxPlayer.Play(SfxType.CorrectClick);
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

        private void StopSound()
        {
            _gameReferences.SoundPlayer.StopAll();
        }
    }
}