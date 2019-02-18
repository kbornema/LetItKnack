using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFoo : MonoBehaviour
{
    [SerializeField]
    private float _forceFactor = 1.0f;

    public Vector2 Velocity;
    public ForceMode2D ForceMode = ForceMode2D.Impulse;
    public bool MultTime = true;

    public Camera Cam;

    // Update is called once per frame
    private void Update()
    {
        var x = Input.GetAxis("Mouse X");
        var y = Input.GetAxis("Mouse Y");

        Velocity.x = x;
        Velocity.y = y;

        var pos = Cam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0.0f;
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var body = collision.GetComponent<Rigidbody2D>();

        if(body)
        {
            Vector2 force = new Vector2(0.0f, Mathf.Abs(Velocity.y) * _forceFactor);

            if (MultTime)
                force *= Time.fixedDeltaTime;

            body.AddForce(force, ForceMode);
        }
    }
}
