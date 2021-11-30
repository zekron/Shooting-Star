using UnityEngine;

public class OverdriveMaterialController : MonoBehaviour
{
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
        PlayerOverdrive.On += PlayerOverdriveOn;
        PlayerOverdrive.Off += PlayerOverdriveOff;
    }

    void OnDisable()
    {
        PlayerOverdrive.On -= PlayerOverdriveOn;
        PlayerOverdrive.Off -= PlayerOverdriveOff;     
    }

    void PlayerOverdriveOn() => renderer.material = overdriveMaterial;

    void PlayerOverdriveOff() => renderer.material = defaultMaterial;
}