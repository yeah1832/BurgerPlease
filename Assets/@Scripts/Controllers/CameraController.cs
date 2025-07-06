using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] 
    private Transform _target;
    private Vector3 _offset;
    // Start is called once before the first e  xecution of Update after the MonoBehaviour is created
    void Start()
    {
        _offset = transform.position - _target.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = _target.position + _offset;
    }
}
