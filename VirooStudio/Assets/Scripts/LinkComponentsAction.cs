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

        private bool isSelected = false;

        [SerializeField] 
        private ComponentScriptableObject componentSO = default;
        
        
        protected override void LocalExecuteImplementation(string data)
        {
            changeColorIfSelected();
            
            
            
        }

        private void changeColorIfSelected()
        {
            if (!isSelected)
            {
                cubeRenderer.material.color = newColor;
                isSelected = true;
                return;
            }

            isSelected = false;
            cubeRenderer.material.color = color;

        }
    }
}