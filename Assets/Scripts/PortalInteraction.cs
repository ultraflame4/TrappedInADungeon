using Level;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem)), RequireComponent(typeof(InteractableObject))]
public class PortalInteraction : MonoBehaviour
{
    public float activeRadialMulti = 1f;
    public float activeEmissionRate = 15f;
    public InteractableObject interactableObject { get; private set; }
    private float initialRadialMulti;
    private float initialEmissionRate;
    private ParticleSystem particleSys;

    private void Start()
    {
        particleSys = GetComponent<ParticleSystem>();
        interactableObject = GetComponent<InteractableObject>();
        interactableObject.InteractableChange += OnInteractableChange;
        initialRadialMulti = particleSys.velocityOverLifetime.radialMultiplier;
        initialEmissionRate = particleSys.emission.rateOverTimeMultiplier;
    }

    void OnInteractableChange(bool value)
    {
        ParticleSystem.VelocityOverLifetimeModule velOverLifetime = particleSys.velocityOverLifetime;
        ParticleSystem.EmissionModule emissionModule = particleSys.emission;
        velOverLifetime.radialMultiplier = value ? activeRadialMulti : initialRadialMulti;
        emissionModule.rateOverTimeMultiplier = value ? activeEmissionRate : initialEmissionRate;
    }
}