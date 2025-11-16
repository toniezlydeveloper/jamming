using System;
using System.Collections.Generic;
using System.Linq;
using Flow.States;
using UnityEngine;

namespace Core
{
    public class IngredientContainer : MonoBehaviour
    {
        [Serializable]
        public class OrganicPresenters
        {
            [SerializeField] private OrganicType type;
            [SerializeField] private GameObject[] models;

            public GameObject[] Models => models;
            public OrganicType Type1 => type;
        }
        
        [Serializable]
        public class NonOrganicPresenters
        {
            [SerializeField] private NonOrganicType type;
            [SerializeField] private GameObject[] models;

            public GameObject[] Models => models;
            public NonOrganicType Type1 => type;
        }
        
        [Serializable]
        public class BasePresenters
        {
            [SerializeField] private BaseType type;
            [SerializeField] private GameObject models;

            public GameObject Models => models;
            public BaseType Type1 => type;
        }
        
        [field:SerializeField] public IngredientType Type { get; set; }
        [SerializeField] private OrganicPresenters[] organicPresenters;
        [SerializeField] private BasePresenters[] basePresenters;
        [SerializeField] private NonOrganicPresenters[] nonorganicPresenters;
        
        public Vector3 Position => transform.position;

        private void Start()
        {
            GameplayState.OnRefreshRequired += TryRefresh;
        }

        private void OnDestroy()
        {
            GameplayState.OnRefreshRequired -= TryRefresh;
        }

        private void TryRefresh(IngredientType type, object datas)
        {
            if (Type != type)
            {
                return;
            }
            
            switch (type)
            {
                case IngredientType.Organic:
                    List<OrganicType> types = datas as List<OrganicType>;
                    
                    foreach (OrganicPresenters organicPresenter in organicPresenters)
                    {
                        foreach (GameObject m in organicPresenter.Models)
                        {
                            m.SetActive(false);
                        }
                    }
                    
                    foreach (OrganicType dT in types.Distinct())
                    {
                        int count = types.Count(x => x == dT);
                        OrganicPresenters c = organicPresenters.First(z => z.Type1 == dT);

                        for (int i = 0; i < count; i++)
                        {
                            c.Models[i].SetActive(true);
                        }
                    }
                    break;
                case IngredientType.Base:
                    List<BaseType> types2 = datas as List<BaseType>;
                    
                    foreach (BasePresenters organicPresenter in basePresenters)
                    {
                        organicPresenter.Models.SetActive(false);
                    }
                    
                    foreach (BaseType dT in types2.Distinct())
                    {
                        BasePresenters c = basePresenters.First(z => z.Type1 == dT);
                        c.Models.SetActive(true);
                    }
                    break;
                case IngredientType.NonOrganic:
                    List<NonOrganicType> types3 = datas as List<NonOrganicType>;
                    
                    foreach (NonOrganicPresenters organicPresenter in nonorganicPresenters)
                    {
                        foreach (GameObject m in organicPresenter.Models)
                        {
                            m.SetActive(false);
                        }
                    }
                    
                    foreach (NonOrganicType dT in types3.Distinct())
                    {
                        int count = types3.Count(x => x == dT);
                        NonOrganicPresenters c = nonorganicPresenters.First(z => z.Type1 == dT);

                        for (int i = 0; i < count; i++)
                        {
                            c.Models[i].SetActive(true);
                        }
                    }
                    break;
            }
        }
    }
}