using Level;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem)), RequireComponent(typeof(InteractableObject))]
public class PortalInteraction : MonoBehaviour
{
    public float initialRadialMulti = 0.5f;
    public float activeRadialMulti = 1f;
    public float initialEmissionRate = 5f;
    public float activeEmissionRate = 15f;
    private InteractableObject interactableObject;
    private ParticleSystem particleSys;

    private void Start()
    {
        particleSys = GetComponent<ParticleSystem>();
        interactableObject = GetComponent<InteractableObject>();
        interactableObject.InteractableChange += OnInteractableChange;
    }

    void OnInteractableChange(bool value)
    {
        ParticleSystem.VelocityOverLifetimeModule velOverLifetime = particleSys.velocityOverLifetime;
        ParticleSystem.EmissionModule emissionModule = particleSys.emission;
        velOverLifetime.radialMultiplier = value ? activeRadialMulti : initialRadialMulti;
        emissionModule.rateOverTime = value ? activeEmissionRate : initialEmissionRate;
    }
}