using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        Transform child = transform.GetChild(transform.childCount - 1);
        if (child != null)
        {
            animator = child.GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError($"Animator component not found on the child '{child.name}' of GameObject '{gameObject.name}'!");
            }
        }
        else
        {
            Debug.LogError($"Last child not found in GameObject '{gameObject.name}'!");
        }
    }

    public void PlayAnimation(string animationName)
    {
       // Debug.Log(animationName);
        if (animator == null || !animator.HasState(0, Animator.StringToHash(animationName)))
        {
            Debug.LogWarning($"Animation '{animationName}' not found for GameObject '{gameObject.name}'");
            return;
        }

        animator.Play(animationName);
    }

    public void Walk()
    {
        PlayAnimation("walk");
    }
    public void stand()
    {
        PlayAnimation("stand");
    }
    public void getWood()
    {
        PlayAnimation("Get wood");
    }
    public void getStone()
    {
        PlayAnimation("Get stone");
    }
    public void GetR()
    {
        PlayAnimation("Get R");
    }

   
}
