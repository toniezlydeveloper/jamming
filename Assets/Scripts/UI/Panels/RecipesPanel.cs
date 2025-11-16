using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Flow.States;
using Internal.Runtime.Dependencies.Core;
using Internal.Runtime.Flow.UI;
using Recipes;
using UI.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels
{
    public interface IRecipesPresenter : IDependency
    {
        void Present(RecipesInfo info);
    }
    
    public class RecipesInfo
    {
        public List<RecipesSaver.RecipeOutputData> Outputs { get; set; }
    }
    
    public class RecipesPanel : AUIPanel, IRecipesPresenter
    {
        [Serializable]
        public class ParentInfo
        {
            [field:SerializeField] public ProcessorType Type { get; set; }
            [field:SerializeField] public Transform Parent { get; set; }
            [field:SerializeField] public GameObject Holder { get; set; }
            [field:SerializeField] public GameObject DefaultText { get; set; }
            [field:SerializeField] public Button SelectButton { get; set; }
        }
        
        [SerializeField] private RecipeElement recipeElementPrefab;
        [SerializeField] private ParentInfo[] infos;
        [SerializeField] private Button backButton;
        [SerializeField] private GameObject defaulshii;
        [SerializeField] private Image defaulshii2;
        [SerializeField] private Color selColor;
        [SerializeField] private GameObject[] anys;

        private List<GameObject> _elements = new();
        private GameObject _selectedShii;
        private Image _sel;
        
        private void Start()
        {
            backButton.onClick.AddListener(RequestTransition<GameplayState>);

            foreach (ParentInfo parentInfo in infos)
            {
                parentInfo.SelectButton.onClick.AddListener(() =>
                {
                    if (_selectedShii != null)
                    {
                        _selectedShii.SetActive(false);
                    }

                    if (_sel != null)
                    {
                        _sel.color = Color.white;
                    }


                    foreach (GameObject any in anys)
                    {
                        any.SetActive(parentInfo.Parent.childCount > 0);
                    }
                    
                    _sel = parentInfo.SelectButton.GetComponent<Image>();
                    _sel.color = selColor;
                    _selectedShii = parentInfo.Holder;
                    _selectedShii.SetActive(true);
                });
            }

            _selectedShii = defaulshii;
            _sel = defaulshii2;
        }

        public void Present(RecipesInfo info)
        {
            foreach (GameObject element in _elements)
            {
                Destroy(element);
            }
            
            _elements.Clear();
            
            foreach (ParentInfo parentInfo in infos)
            {
                parentInfo.DefaultText.SetActive(true);
            }

            foreach (IGrouping<ProcessorType, RecipesSaver.RecipeOutputData> data in info.Outputs.GroupBy(i => i.ProcessorType))
            {
                ParentInfo parentInfo = infos.First(i => i.Type == data.Key);
                parentInfo.DefaultText.SetActive(false);
                
                foreach (RecipesSaver.RecipeOutputData recipe in data)
                {
                    RecipeElement ele = Instantiate(recipeElementPrefab, parentInfo.Parent);
                    ele.Setup(recipe);
                    _elements.Add(ele.gameObject);
                }
            }
        }
    }
}