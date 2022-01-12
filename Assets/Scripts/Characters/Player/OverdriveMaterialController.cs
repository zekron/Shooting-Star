using UnityEngine;

public class OverdriveMaterialController : MonoBehaviour
{
    [SerializeField] private FloatEventChannelSO overdriveOnEvent;
    [SerializeField] private VoidEventChannelSO overdriveOffEvent;
    [SerializeField] private Material overdriveMaterial;

    private Material defaultMaterial;

    private Renderer overdriveRenderer;

    void Awake()
    {
        overdriveRenderer = GetComponent<Renderer>();
        defaultMaterial = overdriveRenderer.material;
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

    void PlayerOverdriveOn(float unused) => overdriveRenderer.material = overdriveMaterial;

    void PlayerOverdriveOff() => overdriveRenderer.material = defaultMaterial;
}