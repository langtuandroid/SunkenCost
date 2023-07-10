using System;
using UnityEngine;

namespace UI.Tooltip
{
    public class DynamicTooltipTrigger : TooltipTrigger
    {
        private Func<string> _getTitleFunc;
        private Func<string> _getDescriptionFunc;

        public void Start()
        {
            var listener = GetComponentInParent<IDynamicTooltipTriggerListener>();
            header = listener.GetTitle();
            _getDescriptionFunc = listener.GetDescription;
        }

        protected override string GetContent()
        {
            return _getDescriptionFunc.Invoke();
        }
    }
    
    public interface IDynamicTooltipTriggerListener
    {
        public string GetTitle();
        public string GetDescription();
    }
}