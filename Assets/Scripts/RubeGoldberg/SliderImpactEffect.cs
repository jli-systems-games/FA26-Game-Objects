using System.Collections;
using UnityEngine;

namespace RubeGoldberg
{
    public class SliderImpactEffect : MonoBehaviour
    {
        public Rigidbody pendulum;
        public bool HasBurst { get; private set; }

        void OnCollisionEnter(Collision hit)
        {
            if (HasBurst || hit.rigidbody != pendulum) return;
            HasBurst = true;
            StartCoroutine(Burst(hit.contactCount > 0 ? hit.GetContact(0).point : transform.position));
        }

        IEnumerator Burst(Vector3 point)
        {
            // Let the collision solver transfer momentum before retiring the slider.
            yield return new WaitForFixedUpdate();
            var source = GetComponentInChildren<Renderer>();
            var effect = new GameObject("SliderImpactParticles");
            effect.transform.position = point;
            var particles = effect.AddComponent<ParticleSystem>();
            particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            var main = particles.main;
            main.loop = false;
            main.duration = .25f;
            main.startLifetime = new ParticleSystem.MinMaxCurve(.35f, .8f);
            main.startSpeed = new ParticleSystem.MinMaxCurve(2f, 5f);
            main.startSize = new ParticleSystem.MinMaxCurve(.06f, .18f);
            main.gravityModifier = .6f;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            var emission = particles.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new[] { new ParticleSystem.Burst(0, 45) });
            var shape = particles.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = .2f;
            var size = particles.sizeOverLifetime;
            size.enabled = true;
            size.size = new ParticleSystem.MinMaxCurve(1, AnimationCurve.Linear(0, 1, 1, 0));
            // Reuse the slider mesh/material so fragments match the user's color.
            var renderer = particles.GetComponent<ParticleSystemRenderer>();
            renderer.renderMode = ParticleSystemRenderMode.Mesh;
            var mesh = GetComponentInChildren<MeshFilter>();
            if (mesh) renderer.mesh = mesh.sharedMesh;
            if (source) renderer.sharedMaterial = source.sharedMaterial;
            particles.Play();
            Destroy(effect, 2f);
            foreach (var collider in GetComponentsInChildren<Collider>()) collider.enabled = false;
            foreach (var visual in GetComponentsInChildren<Renderer>()) visual.enabled = false;
            var body = GetComponent<Rigidbody>();
            body.linearVelocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
            body.isKinematic = true;
            Debug.Log("[Machine Effect] Slider impact burst; slider retired after physical contact");
        }
    }
}
