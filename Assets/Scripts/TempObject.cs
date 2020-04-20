using UnityEngine;

public class TempObject : MonoBehaviour {
    public float lifetime;

    void Awake() => Destroy(gameObject, lifetime);
}
