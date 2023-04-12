using UnityEngine;
using UnityEngine.EventSystems;

namespace Etchings
{
    public class IndicatorController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Indicator _indicator;
        private Plank _plank;

        private LTDescr _delay;

        private void Awake()
        {
            _plank = transform.parent.GetComponent<Plank>();
            _indicator = transform.GetChild(0).GetComponent<Indicator>();
        }
    
        public void OnPointerEnter(PointerEventData eventData)
        {
            _plank.Etching?.UpdateIndicators();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            for (var i = 1; i < PlankMap.current.stickCount; i++)
            {
                var stick = PlankMap.current.GetStick(i);
                stick?.IndicatorController.Hide();
            }
        
        }

        public void Show(Color color)
        {
            _indicator.SetColor(color);
        }

        public void Hide()
        {
            _indicator.Hide();
        }

        /*
    private List<Indicator> _indicators = new List<Indicator>();
    private List<Color> _colors = new List<Color>();

    private void Awake()
    {
        _indicators = transform.GetComponentsInChildren<Indicator>().ToList();

        foreach (var indicator in _indicators)
        {
            indicator.gameObject.SetActive(false);
        }
    }

    public void AddColor(Color color)
    {
        _colors.Add(color);
        UpdateIndicators();
    }

    public void RemoveColor(Color color)
    {
        _colors.Remove(color);
        UpdateIndicators();
    }
    
    // Called whenever a plank adds or removes a color
    private void UpdateIndicators()
    {
        for (var i = 0; i < _indicators.Count; i++)
        {
            var indicator = _indicators[i];
            
            // Deactivate all unused indicators
            if (i >= _colors.Count)
            {
                indicator.gameObject.SetActive(false);
                continue;
            }
            
            indicator.gameObject.SetActive(true);
            indicator.SetColor(_colors[i]);
        }
    }
    
    */
    
    }
}
