using UnityEngine;

public abstract class BaseMonoBehaviourScript : MonoBehaviour
{
    protected virtual void Reset()
    {
        LoadComponents();
    }

    protected virtual void LoadComponents(){}
    
}
