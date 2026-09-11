using UnityEngine;

namespace RubeGoldberg
{
    [DefaultExecutionOrder(-100)]
    public class PhysicsValidator : MonoBehaviour
    {
        public Transform machineRoot;
        public MachineGameManager manager;
        Rigidbody[] bodies;
        Rigidbody[] balls;
        Vector3[] checkpoints;
        public static readonly RigidbodyConstraints SliderConstraints =
            RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ |
            RigidbodyConstraints.FreezeRotation;

        Transform Find(string objectName)
        {
            foreach (var child in machineRoot.GetComponentsInChildren<Transform>(true))
                if (child.name == objectName) return child;
            return null;
        }

        Rigidbody RequiredBody(string objectName, Rigidbody serializedBody = null)
        {
            var child = serializedBody ? serializedBody.transform : Find(objectName);
            if (!child)
            {
                Debug.LogError("[Machine Validator] Missing essential object: " + objectName);
                return null;
            }
            var body = child.GetComponent<Rigidbody>();
            if (!body) body = child.gameObject.AddComponent<Rigidbody>();
            body.mass = Mathf.Max(.01f, body.mass);
            body.useGravity = true;
            body.interpolation = RigidbodyInterpolation.Interpolate;
            body.collisionDetectionMode = body.isKinematic
                ? CollisionDetectionMode.ContinuousSpeculative : CollisionDetectionMode.ContinuousDynamic;
            body.solverIterations = 16;
            body.solverVelocityIterations = 8;
            body.maxAngularVelocity = 20;
            if (!child.GetComponent<Collider>())
            {
                if (objectName.Contains("Ball")) child.gameObject.AddComponent<SphereCollider>();
                else
                {
                    var collider = child.gameObject.AddComponent<BoxCollider>();
                    var visual = child.Find("Visual");
                    if (visual) collider.size = visual.localScale;
                }
            }
            return body;
        }

        void Awake()
        {
            if (!machineRoot) machineRoot = transform.root;
            if (!manager) manager = machineRoot.GetComponentInChildren<MachineGameManager>();
            if (!manager)
            {
                Debug.LogError("[Machine Validator] Missing MachineGameManager");
                enabled = false;
                return;
            }
            manager.playerBall = RequiredBody("PlayerBall", manager.playerBall);
            manager.leverBall = RequiredBody("LeverBall", manager.leverBall);
            manager.finalBall = RequiredBody("FinalBall", manager.finalBall);
            manager.slider = RequiredBody("SliderBlock", manager.slider);
            manager.pendulum = RequiredBody("PendulumArm", manager.pendulum);
            var lever = RequiredBody("LeverArm");
            if (manager.dominoes == null || manager.dominoes.Length != 7) manager.dominoes = new Rigidbody[7];
            for (int i = 0; i < 7; i++) manager.dominoes[i] = RequiredBody("Domino0" + (i + 1), manager.dominoes[i]);
            if (!manager.playerBall || !manager.leverBall || !manager.finalBall ||
                !manager.slider || !manager.pendulum || !lever)
            {
                enabled = false;
                manager.enabled = false;
                return;
            }
            manager.slider.constraints = SliderConstraints;
            foreach (var child in machineRoot.GetComponentsInChildren<Transform>(true))
            {
                if (child.name.Contains("Trigger"))
                {
                    var collider = child.GetComponent<Collider>();
                    if (!collider) collider = child.gameObject.AddComponent<BoxCollider>();
                    collider.isTrigger = true;
                }
                bool isStatic = child.name.Contains("Ramp") || child.name.Contains("Rail") ||
                    child.name.Contains("Wall") || child.name.Contains("Support") ||
                    child.name.Contains("Track") || child.name == "Ground" || child.name == "FinalBallGate";
                var body = child.GetComponent<Rigidbody>();
                if (isStatic && body) { body.isKinematic = true; body.useGravity = false; }
            }
            foreach (string objectName in new[] { "LeverArm", "PendulumArm" })
            {
                var arm = Find(objectName);
                var pivot = Find(objectName == "LeverArm" ? "LeverPivot" : "PendulumPivot");
                if (!pivot) { Debug.LogError("[Machine Validator] Missing pivot for " + objectName); continue; }
                var hinge = arm.GetComponent<HingeJoint>();
                if (!hinge) hinge = arm.gameObject.AddComponent<HingeJoint>();
                hinge.axis = Vector3.forward;
                if (!hinge.connectedBody) hinge.connectedBody = pivot.GetComponent<Rigidbody>();
                if (!hinge.connectedBody || Vector3.Distance(arm.TransformPoint(hinge.anchor), pivot.position) > .05f)
                    Debug.LogError("[Machine Validator] Hinge anchor/pivot mismatch: " + objectName);
            }
            var cameras = machineRoot.GetComponentsInChildren<Camera>();
            if (cameras.Length != 4) Debug.LogError("[Machine Validator] Expected four gameplay cameras");
            foreach (var camera in cameras)
            {
                camera.enabled = camera.name == "Camera01_Start";
                var listener = camera.GetComponent<AudioListener>();
                if (listener) listener.enabled = camera.enabled;
            }
            bodies = machineRoot.GetComponentsInChildren<Rigidbody>();
            balls = new[] { manager.playerBall, manager.leverBall, manager.finalBall };
            checkpoints = new[] { balls[0].position, balls[1].position, balls[2].position };
            // Correct only clear sphere/static penetrations, capped in total per ball.
            // Joint bodies, stage parents and all static geometry remain untouched.
            Physics.SyncTransforms();
            var colliders = machineRoot.GetComponentsInChildren<Collider>();
            foreach (var ball in balls)
            {
                float remaining = .25f;
                var sphere = ball.GetComponent<SphereCollider>();
                foreach (var other in colliders)
                {
                    if (!other.enabled || other.isTrigger || other.attachedRigidbody || remaining <= 0) continue;
                    if (Physics.ComputePenetration(sphere, ball.position, ball.rotation,
                        other, other.transform.position, other.transform.rotation, out var direction, out float distance)
                        && distance > .01f && distance <= remaining)
                    {
                        ball.position += direction * distance;
                        remaining -= distance;
                        Debug.Log("[Machine Validator] Corrected initial penetration: " + ball.name);
                    }
                }
            }
            Debug.Log("[Machine Validator] Configuration checked");
        }

        void FixedUpdate()
        {
            foreach (var body in bodies)
            {
                if (body.isKinematic) continue;
                if (body.linearVelocity.sqrMagnitude > 625) body.linearVelocity = body.linearVelocity.normalized * 25;
                if (body.angularVelocity.sqrMagnitude > 400) body.angularVelocity = body.angularVelocity.normalized * 20;
            }
            for (int i = 0; i < balls.Length; i++)
            {
                if (balls[i].position.y >= -5) continue;
                var checkpoint = checkpoints[i];
                if (i == 1 && manager.CurrentStage >= 4) checkpoint = new Vector3(18.3f, 1.2f, 0);
                if (i == 2 && manager.CurrentStage >= 7) checkpoint = new Vector3(31.2f, 1.7f, 0);
                balls[i].position = checkpoint;
                balls[i].linearVelocity = Vector3.zero;
                balls[i].angularVelocity = Vector3.zero;
                Debug.Log("[Machine Validator] Recovered " + balls[i].name);
            }
        }
    }
}

