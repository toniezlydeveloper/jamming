using System.Collections.Generic;
using System.Linq;
using Internal.Runtime.Dependencies.Core;
using UnityEngine;

namespace UI.Elements
{
    public interface IHintPresenter : IDependency
    {
        void Present(AddHintData data);
        void Present(RemoveHintData data);
    }

    public class AddHintData
    {        public string Text { get; set; }
public Sprite Code { get; set; }
    }

    public class RemoveHintData
    {
        public string Text { get; set; }
    }

    public class HintWindow : MonoBehaviour, IHintPresenter
    {
        [SerializeField] private HintElement elementPrefab;
        [SerializeField] private Transform hintsParent;
        

        private Dictionary<string, HintElement> _elementsByText = new();
        private List<string> _hints = new();
        
        public void Present(AddHintData data)
        {
            if (_hints.Any(g => g == data.Text))
            {
                return;
            }

            var x = Instantiate(elementPrefab, hintsParent);
            _elementsByText.Add(data.Text, x);
            x.Init(data.Text, data.Code);
            _hints.Add(data.Text);
        }

        public void Present(RemoveHintData data)
        {
            if (!_elementsByText.ContainsKey(data.Text))
            {
                return;
            }

            Destroy(_elementsByText[data.Text].gameObject);
            _elementsByText.Remove(data.Text);
            _hints.Remove(data.Text);
        }
    }
}