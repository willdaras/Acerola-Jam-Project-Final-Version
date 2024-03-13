using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class CursorLocker : MonoBehaviour
    {
        public void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}