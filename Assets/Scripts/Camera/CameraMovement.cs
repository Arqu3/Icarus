using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Movement, rotation")]
    [SerializeField]
    float speed = 10f;
    [SerializeField]
    float boostMulti = 1.5f;
    [SerializeField]
    float rotationSpeed = 15f;
    [SerializeField]
    Vector2 rotationXClamp = new Vector2(30f, 80f);

    bool active = false;

    Vector2 storedRotation;

    private void Awake()
    {
        storedRotation = new Vector2(transform.localEulerAngles.x, transform.localEulerAngles.y);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            active = !active;
            Cursor.lockState = active ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !active;
        }

        if (!active) return;

        UpdateMovement();

        UpdateRotation();
    }

    void UpdateMovement()
    {
        float updown = Input.GetKey(KeyCode.X) ? -1f : Input.GetKey(KeyCode.Space) ? 1f : 0f;

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), updown, Input.GetAxisRaw("Vertical"));
        input = Vector3.ClampMagnitude(input, 1f);

        Vector3 current = transform.position;
        Vector3 target = current + Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f) * input;
        transform.position = Vector3.MoveTowards(current, target, speed * (Input.GetKey(KeyCode.LeftShift) ? boostMulti : 1f) * Time.deltaTime);
    }

    void UpdateRotation()
    {
        Vector2 input = new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        input = Vector2.ClampMagnitude(input, 1f);

        if (input.x > 0f && storedRotation.x < rotationXClamp.y) storedRotation.x += input.x * rotationSpeed;
        if (input.x < 0f && storedRotation.x > rotationXClamp.x) storedRotation.x += input.x * rotationSpeed;
        storedRotation.y += input.y * rotationSpeed;

        transform.rotation = Quaternion.Euler(storedRotation);
    }
}
