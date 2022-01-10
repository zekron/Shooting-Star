using UnityEngine;

public class OverdriveMaterialController : MonoBehaviour
{
    [SerializeField] private FloatEventChannelSO overdriveOnEvent;
    [SerializeField] private VoidEventChannelSO overdriveOffEvent;
    [SerializeField] Material overdriveMaterial;

    Material defaultMaterial;

    new Renderer renderer;

    void Awake()
    {
        renderer = GetComponent<Renderer>();
        defaultMaterial = renderer.material;
    }

    void OnEnable()
    {
        overdriveOnEvent.OnEventRaised += PlayerOverdriveOn;
        overdriveOffEvent.OnEventRaised += PlayerOverdriveOff;
    }

    void OnDisable()
    {
        overdriveOnEvent.OnEventRaised -= PlayerOverdriveOn;
        overdriveOffEvent.OnEventRaised -= PlayerOverdriveOff;
    }

    void PlayerOverdriveOn(float unused) => renderer.material = overdriveMaterial;

    void PlayerOverdriveOff() => renderer.material = defaultMaterial;
}