using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraControler : MonoBehaviour
{
    //Variables
    [SerializeField] private float mouseSensitivity;

    //References
    private Transform parent;

    void Start()
    {
        parent = transform.parent;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Rotate();
    }

    //Functions
    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        parent.Rotate(Vector3.up, mouseX);
    }
}
