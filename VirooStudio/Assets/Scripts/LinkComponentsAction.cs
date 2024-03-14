using UnityEngine;
using Viroo.Interactions;

namespace VirooLab.Actions
{
    public class LinkComponent : BroadcastObjectAction
    {
        [SerializeField]
        private Renderer cubeRenderer = default;

        [SerializeField]
        private Color color = default;
        
        [SerializeField]
        private Color newColor = default;
        
        [SerializeField] 
        private ComponentController component;
        
        
        
        
        protected override void LocalExecuteImplementation(string data)
        {
            ChangeColorIfSelected();
            component.MarkAsSelected();
        }

        private void ChangeColorIfSelected()
        {
            if (!component.isSelected)
            {
                cubeRenderer.material.color = newColor;
                Debug.Log("Selected!");
                return;
            }
            
            cubeRenderer.material.color = color;
            Debug.Log("Deselected!");

        }
    }
}