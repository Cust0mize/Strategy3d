using UnityEngine;

public class ParticleAnimationService {
    public void DisableParticles(ParticleSystem[] particleSystems) {
        foreach (var item in particleSystems) {
            item.gameObject.SetActive(false);
        }
    }

    public void EnableParticles(ParticleSystem[] particleSystems, bool isLoop = false) {
        foreach (var particle in particleSystems) {
            particle.loop = isLoop;
            particle.gameObject.SetActive(false);
            particle.gameObject.SetActive(true);
        }
    }

    public void ParticlesStopLoop(ParticleSystem[] particleSystems) {
        foreach (var particle in particleSystems) {
            particle.loop = false;
        }
    }
}