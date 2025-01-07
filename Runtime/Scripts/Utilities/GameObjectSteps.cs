using UnityEngine;

namespace _3Dimensions.Tools.Runtime.Scripts.Utilities
{
    public class GameObjectSteps : MonoBehaviour
    {
        public GameObject[] steps;
        public int activeStep = 0;

        public void CollectSteps()
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

        public void First()
        {
            activeStep = 0;
            ActivateStep();
        }

        public void Previous()
        {
            activeStep--;
            if (activeStep <= 0)
            {
                activeStep = 0;
            }

            ActivateStep();
        }

        public void Next()
        {
            activeStep++;
            if (activeStep >= steps.Length - 1)
            {
                activeStep = steps.Length - 1;
            }

            ActivateStep();
        }

        public void Last()
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
                steps[i].SetActive(activeStep == i);
            }
        }
    }
}