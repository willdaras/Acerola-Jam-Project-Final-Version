using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

public class Cat : MonoBehaviour, IDamageable
{
    private Rigidbody _rb;

    [SerializeField] private Transform _target;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _stopDistance = 2;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 dist = new Vector3(_target.position.x, transform.position.y, _target.position.z) - transform.position;

        if (dist.magnitude > _stopDistance)
        {
            _rb.velocity = dist.normalized * _speed;
            transform.rotation = Quaternion.LookRotation(dist);
        }
    }

    public void Damage(int amount)
    {
        Debug.Log("Cat hit");
    }
}
