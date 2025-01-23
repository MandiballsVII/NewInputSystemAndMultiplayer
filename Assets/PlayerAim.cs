using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    private Transform aimTransform;
    private Vector3 aimlocalScale = Vector3.one;

    public Vector2 PointerPosition { get; set; }

    [SerializeField] private InputActionReference move, pointerPosition;

    void Awake()
    {
        aimTransform = transform.Find("Aim");
    }
    private void Start()
    {

    }

    void Update()
    {        
        //transform.right = (PointerPosition - (Vector2)transform.position).normalized;
        aimTransform.localScale = aimlocalScale;
        //GetMouseWorldPosition();
        //HandleAiming();
        print(Mouse.current.position.ReadValue());
    }

    // Get mouse position in world with Z = 0f.

    // Para comprobar boton de mouse apretado --> Mouse.current.leftButton.wasPressedThisFrame
    #region
    public static Vector3 GetMouseWorldPosition()
    {
        //if (Mouse.current.leftButton.wasPressedThisFrame)
        //{
        //    print("Hola");
        //    print(Mouse.current.position.ReadValue());
        //}
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
        //Vector3 vec = GetMouseWorldPositionWithZ(Mouse.current.position.ReadValue(), Camera.main);
        //vec.z = 0f;
        //return vec;
    }
    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Mouse.current.position.ReadValue(), Camera.main);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Mouse.current.position.ReadValue(), worldCamera);

    }
    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
    #endregion

    public void HandleAiming(InputAction.CallbackContext context)
    {
        Vector3 mousePosition = GetMouseWorldPosition();

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        if (context.ReadValue<Vector2>().x != 0 || context.ReadValue<Vector2>().y != 0)
        {
            angle = Mathf.Atan2(context.ReadValue<Vector2>().y, context.ReadValue<Vector2>().x) * Mathf.Rad2Deg;
            aimTransform.eulerAngles = new Vector3(0, 0, angle);
        }
        //UnityEngine.Debug.Log(angle);
        if (angle > 90 || angle < -90)
        {
            aimlocalScale.y = -1f;
        }
        else
        {
            aimlocalScale.y = +1f;
        }
    }

}
