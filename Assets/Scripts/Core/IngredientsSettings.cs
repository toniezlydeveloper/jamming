using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu]
    public class IngredientsSettings : ScriptableObject
    {
        [SerializeField] private List<OrganicData> organicsData;
        [SerializeField] private List<NonOrganicData> nonOrganicsData;
        [SerializeField] private List<BaseData> basesData;
        [SerializeField] private List<PotionData> potionsData;
        [SerializeField] private List<DustData> dustsData;
        [SerializeField] private List<DistilledData> distilledData;
        [SerializeField] private List<CutData> cutsData;
        [SerializeField] private List<ProcessorData> processorsData;
        [SerializeField] private Color highlightColor;
        [SerializeField] private Sprite emptyIcon;
        [SerializeField] private string emptyText;

        public List<DustData> DustsData => dustsData;
        public List<DistilledData> DistilledData => distilledData;
        public List<CutData> CutsData => cutsData;
        public Sprite EmptyIcon => emptyIcon;
        public string EmptyText => emptyText;
        public Color HighlightColor => highlightColor;
        public List<OrganicData> OrganicsData => organicsData;
        public List<NonOrganicData> NonOrganicsData => nonOrganicsData;
        public List<BaseData> BasesData => basesData;
        public List<PotionData> PotionsData => potionsData;
        public List<ProcessorData> ProcessorsData => processorsData;

        [ContextMenu(nameof(Populate))]
        public void Populate()
        {
            foreach (OrganicType value in Enum.GetValues(typeof(OrganicType)))
            {
                if (organicsData.Any(d => d.Type == value))
                {
                    continue;
                }
                
                organicsData.Add(new OrganicData
                {
                    Type = value
                });
            }
            foreach (BaseType value in Enum.GetValues(typeof(BaseType)))
            {
                if (basesData.Any(d => d.Type == value))
                {
                    continue;
                }
                
                basesData.Add(new BaseData
                {
                    Type = value
                });
            }
            
            foreach (PotionType value in Enum.GetValues(typeof(PotionType)))
            {
                if (potionsData.Any(d => d.Type == value))
                {
                    continue;
                }
                
                potionsData.Add(new PotionData
                {
                    Type = value
                });
            }
            
            foreach (NonOrganicType value in Enum.GetValues(typeof(NonOrganicType)))
            {
                if (nonOrganicsData.Any(d => d.Type == value))
                {
                    continue;
                }
                
                nonOrganicsData.Add(new NonOrganicData
                {
                    Type = value
                });
            }
            
            foreach (PotionType value in Enum.GetValues(typeof(PotionType)))
            {
                if (potionsData.Any(d => d.Type == value))
                {
                    continue;
                }
                
                potionsData.Add(new PotionData
                {
                    Type = value
                });
            }
            
            foreach (DistilledType value in Enum.GetValues(typeof(DistilledType)))
            {
                if (distilledData.Any(d => d.Type == value))
                {
                    continue;
                }
                
                distilledData.Add(new DistilledData()
                {
                    Type = value
                });
            }
            
            foreach (CutType value in Enum.GetValues(typeof(CutType)))
            {
                if (cutsData.Any(d => d.Type == value))
                {
                    continue;
                }
                
                cutsData.Add(new CutData()
                {
                    Type = value
                });
            }
            
            foreach (DustType value in Enum.GetValues(typeof(DustType)))
            {
                if (dustsData.Any(d => d.Type == value))
                {
                    continue;
                }
                
                dustsData.Add(new DustData()
                {
                    Type = value
                });
            }
            
            foreach (ProcessorType value in Enum.GetValues(typeof(ProcessorType)))
            {
                if (processorsData.Any(d => d.Type == value))
                {
                    continue;
                }
                
                processorsData.Add(new ProcessorData
                {
                    Type = value
                });
            }
        }
    }
}