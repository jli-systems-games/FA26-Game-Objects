# Rube Goldberg Machine

Open `Assets/Scenes/RubeGoldbergMachine.unity` as the active scene and press Play. No input is needed afterward. The ball releases after one second; the current scene reloads five seconds after completion.

## What to watch

1. **Blue ramp:** the white physics character rolls under gravity across a short transition and hits the first domino.
2. **Red dominoes:** seven Rigidbody dominoes tip toward +X. The last domino crosses CameraTrigger01 and contacts the lever latch.
3. **Yellow lever:** the Z-axis HingeJoint releases; its passive spring and angled launch surface move the orange ball toward the catcher. The right end is shortened by 0.2 m to clear the catcher and allow natural hinge rotation. The corrected default-step run completes with failsafes disabled.
4. **Green catcher:** the orange ball travels through the catcher, drops approximately one meter onto the lower ramp, and crosses CameraTrigger02.
5. **Orange slider:** the ball physically strikes the block. Only X translation is free. The block crosses the pendulum release trigger. After physical contact with the pendulum it bursts into matching-color fragments and retires from physics.
6. **Purple pendulum:** gravity swings the compound arm/weight around its top anchor. A filtered trigger sends the final-ball gate spinning upward and out of the way shortly before the weight physically hits the yellow final ball. The narrow pocket avoids obstructing the weight.
7. **Cyan finale:** the final ball crosses CameraTrigger03, rolls downhill, and enters the final trigger. The UI reads MACHINE COMPLETE!, particles play, a light turns on, and the scene restarts after five seconds.

## Assignment evidence

- Seven stages, exactly seven dominoes, four cameras and three one-shot physics camera changes.
- Rigidbody, colliders, triggers, HingeJoint, gravity/forces, camera switching and C# scripting.
- PhysicsContactEvidence logs actual PlayerBall/Domino01, LeverBall/SliderBlock and FinalBall/PendulumArm collisions. It never changes motion.
- Camera and release triggers require explicitly assigned activating bodies, including compound pendulum colliders.
- All stage parents and the machine root use identity transforms. All builder coordinates are world coordinates.
- Compound Rigidbody roots have unit scale. Primitive child visuals and BoxCollider sizes carry dimensions, avoiding shear. The unscaled pendulum's top anchor is local `(0, 1.5, 0)`, exactly at the world pivot.
- LeverBall and FinalBall retain dynamic physics while constraining Z to keep their interactions in the XY plane.
- The validator checks physics setup, cameras, pivots and required references, bounds penetration correction to 0.25m, clamps excessive velocities, and recovers balls only below Y=-5.
- Failsafes measure forward progress after the relevant stage starts. Each stage receives at most one logged assist after approximately seven seconds without progress.

## Verification

The latest scene completed all seven stages with failsafes disabled at fixed timesteps 0.01666667 and 0.025 seconds. Physical contacts, camera switches, finale effects and automatic restart were verified. Scene validation also checks missing scripts, references and joint alignment.

## Rebuilding

`Tools > Rube Goldberg > Build Machine` creates the scene if absent. The automatic builder runs once when a saved project scene is open and the machine scene is absent. An existing machine scene is preserved. To intentionally regenerate, delete only the generated machine scene in Unity, then use the menu command. Materials are reused. Editor test/build helpers never run Play Mode automatically in the normal project.

Runtime scripts use standard Unity physics, UI, particles, coroutines and SceneManager. No new packages or downloaded art assets were added.

## WebGL export

The companion Week3Web.zip contains the final WebGL player. Its index.html is at the archive root, alongside Build and supporting assets. The export includes the lever correction, slider impact particles and spinning gate flight.

