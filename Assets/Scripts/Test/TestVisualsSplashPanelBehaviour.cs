using UnityEngine;
using UnityEngine.EventSystems;

namespace Ventura.Test
{
    public class TestVisualsSplashPanelBehaviour : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            gameObject.SetActive(false);
        }
    }
}
