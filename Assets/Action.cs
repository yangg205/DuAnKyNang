using UnityEngine;

public class Action : MonoBehaviour
{
    public GameObject item;
    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (item == null)
        {
            animator.SetTrigger("dong");
        }
    }
}
