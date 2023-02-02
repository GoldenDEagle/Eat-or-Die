using UnityEngine;

namespace Assets.Scripts
{
    public class DestroyAfterDelay : MonoBehaviour
    {
        void Start()
        {
            Destroy(gameObject, 2);
        }
    }
}
