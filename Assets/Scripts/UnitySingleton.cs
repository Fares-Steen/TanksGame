using UnityEngine;

namespace Assets.Scripts
{
    public class UnitySingleton : MonoBehaviour
    {
        public static UnitySingleton Instance { get; private set; }

        public string playerName { get; set; }

        public bool godMod { get; set; } = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}
