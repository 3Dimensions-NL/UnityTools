using Sirenix.OdinInspector;
using UnityEngine;
namespace _3Dimensions.Utilities.Helpers
{
    public class GameObjectSteps : MonoBehaviour
    {
        public GameObject[] steps;
        public int activeStep = 0;
        
        [Button]
        private void CollectSteps()
        {
            steps = new GameObject[transform.childCount];

            for (int i = 0; i < steps.Length; i++)
            {
                steps[i] = transform.GetChild(i).gameObject;
            }
            
            #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            #endif
        }

        [HorizontalGroup("Controls"), Button(Name = "|<")]
        private void First()
        {
            activeStep = 0;
            ActivateStep();
        }


        [HorizontalGroup("Controls"), Button(Name = "<")]
        private void Previous()
        {
            activeStep--;
            if (activeStep <= 0)
            {
                activeStep = 0;
            }
            ActivateStep();
        }
        
        [HorizontalGroup("Controls"), Button(Name = ">")]
        private void Next()
        {
            activeStep++;
            if (activeStep >= steps.Length - 1)
            {
                activeStep = steps.Length - 1;
            }
            ActivateStep();
        }
        
        [HorizontalGroup("Controls"), Button(Name = ">|")]
        private void Last()
        {
            activeStep = steps.Length - 1;
            ActivateStep();
        }

        public void ActivateStep(int step)
        {
            if (step < 0) return;
            if (step > steps.Length - 1) return;
            activeStep = step;
            ActivateStep();
        }
        
        private void ActivateStep()
        {
            for (int i = 0; i < steps.Length; i++)
            {
                steps[i].gameObject.SetActive(activeStep == i);
            }
        }
    }
}
