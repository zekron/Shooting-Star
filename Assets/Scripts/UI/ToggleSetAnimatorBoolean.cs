using UnityEngine;

namespace HeathenEngineering.UX.Samples
{
    public class ToggleSetAnimatorBoolean : MonoBehaviour
    {
        [SerializeField] private string booleanName;
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void SetBoolean(bool value)
        {
            animator.SetBool(booleanName, value);
        }
    }
}
