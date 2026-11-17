using UnityEngine;

public class Helper : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private void Update()
    {
        transform.position = Vector2.Lerp(transform.position, _target.position+ new Vector3(0.6f,0.6f,transform.position.z), Time.deltaTime);
    }
}
