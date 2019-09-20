using UnityEngine;

namespace Devdog.QuestSystemPro
{
    public class LookAtMainCamera : MonoBehaviour
    {
        protected void Update()
        {
            if (Camera.main != null)
            {
                transform.LookAt(Camera.main.transform, Vector3.up);
            }
        }
    }
}
