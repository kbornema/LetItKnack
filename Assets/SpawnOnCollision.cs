using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnOnCollision : MonoBehaviour
{
    public enum Where { Transform, Hit }

    [SerializeField]
    private GameObject _prefab = default;
    [SerializeField]
    private Where _where = Where.Hit;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(_prefab)
        {
            Vector2 pos = Vector3.zero;

            if (_where == Where.Hit)
            {
                for (int i = 0; i < collision.contactCount; i++)
                    pos += collision.GetContact(i).point;

                pos /= collision.contactCount;
            }

            else if (_where == Where.Transform)
                pos = transform.position;

            Instantiate(_prefab, pos, Quaternion.identity);
        }
    }
}
