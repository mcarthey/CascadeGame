# PHYSICS.md: The Mechanics of Snow

Welcome to the physics of **Cascade**. This document explains how we turn thousands of independent dots into a cohesive, piling fluid.

## 1. The Coordinate System (The "Where am I?" Problem)

The biggest hurdle we faced was "Screen Space" vs "Physics Space."

- **The Conflict:** In math class, $Y$ usually goes up. In game engines (like Stride), $Y$ goes **down**.
- **The Fix:** We aligned our physics to the engine. $Y = 0$ is the sky, and $Y = 720$ is the floor. Gravity is a positive force $(0, 400)$ pushing particles toward that larger $Y$ value.

------

## 2. SPH: Neighborhood Watch

We use **Smoothed-Particle Hydrodynamics**. Think of each particle having a "circle of influence" (our `InteractionRadius` of $15.0$).

### The Density Pass

Before moving, every particle looks at its neighbors within that radius.

- If a particle is alone, its density is low.
- If it's in a crowd, its density is high.
- **Why?** We need to know how crowded a spot is before we decide how hard to push back.

------

## 3. The Three Forces of Snow

To get the particles to move like snow rather than gas or solid rock, we balance three specific forces in the `CalculateSPHForces` method:

### A. Gravity (The Motor)

A constant downward pull. Without it, your snow would just float in the abyss.

### B. Pressure (The "Personal Space" Force)

This is what caused your "popcorn" effect.

- **The Logic:** If two particles overlap, they push each other away.
- **The Tuning:** We lowered the `pressureMultiplier`. If it's too high, they explode. If it's too low, they pass through each other like ghosts. We found a "sweet spot" where they stay apart but don't "pop".

### C. Viscosity (The "Syrup" Force)

This is the secret ingredient for snow.

- **The Logic:** This force makes neighbors want to move at the same speed.
- **The Result:** It acts like friction in the air. It saps the energy out of the "popcorn" bounces and makes the particles clump together like wet snow.

------

## 4. Boundaries and "The Stick"

When a particle hits the floor ($Y > 715$), we apply **Boundary Constraints**:

1. **Damping:** we multiply the velocity by a small number (like $0.05$). This "kills" the bounce.
2. **Friction:** We slow down their sideways $(X)$ movement. This allows the snow to actually form a pile instead of sliding forever like it's on a frictionless ice rink.

------

## 5. Rendering: The Optical Illusion

Even though the physics sees them as hard points, we render them as soft blobs.

- **The Texture:** A 32x32 radial gradient where the edges fade to $0$ alpha (transparent).
- **The Blending:** We use `NonPremultiplied` blending. This tells the GPU: "Take the white color and use the alpha channel to decide how see-through the edges are".
- **The Sampler:** `LinearClamp` prevents the GPU from drawing "jagged" pixels, giving you that soft, snowy glow.

------

## Glossary for Non-Physics Types

- **Finite:** Not broken. If a particle's position becomes `NaN` (Not a Number), it has "teleported" out of reality, so we reset it.
- **DeltaTime (dt):** The time between frames. We cap this at $0.0166$ ($60$ FPS) so that if the computer lags, the physics doesn't try to calculate a "huge" jump and send particles flying into orbit.
- **Spatial Grid:** A "sorting hat" for particles. It puts them into buckets so they only have to check neighbors in nearby buckets, making the game run fast.