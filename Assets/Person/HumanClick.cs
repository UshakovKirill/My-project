using UnityEngine;
public class HumanClick : MonoBehaviour
{
    public LayerMask humanLayerMask;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, humanLayerMask))
            {
                Human human = hit.transform.GetComponent<Human>();
                if (human != null)
                {
                    UIManager.Instance.UpdateHumanUI(human);
                }
            }
            else
            {
                UIManager.Instance.HideHumanInterface();
            }
        }
    }
}
