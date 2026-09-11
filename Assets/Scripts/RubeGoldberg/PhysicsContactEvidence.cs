using UnityEngine;
namespace RubeGoldberg
{
    // Records real contact for the assignment audit; never changes motion.
    public class PhysicsContactEvidence : MonoBehaviour
    {
        public Rigidbody expectedBody;
        public bool HasCollided { get; private set; }
        void OnCollisionEnter(Collision collision)
        {
            if (HasCollided || !expectedBody || collision.rigidbody != expectedBody) return;
            HasCollided = true;
            Debug.Log("[Machine Contact] " + name + " physically hit " + expectedBody.name);
        }
    }
}
