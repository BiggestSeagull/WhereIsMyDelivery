using UnityEngine;

public class ShopCursorControl : MonoBehaviour
{
    private void OnEnable()
    {
        UnlockCursor();
    }
    private void OnDisable()
    {
        LockCursor();
    }
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
