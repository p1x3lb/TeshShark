using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utils.Project.Scripts.Modules.Utils.UI
{
    public class ActionPanel : MonoBehaviour, IPointerClickHandler
    {
        public event Action Clicked; 

        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke();
        }
    }
}