# Gas Station - Free Convenience Store

## Overview

A free, ready-to-use roadside convenience store from the Gas Station collection: a modular single-storey shop building (walls with window, door and roller-gate openings, corner columns, cornice, double doors, floor / ceiling / roof slabs), its interior furniture (shelves, sales counter, cash register, ICE freezers), three shelf products (soda can, food can, water bottle) and the paving around the foundation with wall lamps. No fuel pumps, canopy, vehicles or vegetation - those are in the Complete pack.

Every model has a ready prefab (`GasStation/Prefabs/P_RSG_*`). `GasStation/Scenes/FreeStore/GasStation_FreeStore_Demo_<Pipeline>` shows the assembled store for each render pipeline. This pack is part of **Gas Station - Complete Environment** and uses the same files, folders and GUIDs, so it merges cleanly with the Complete pack and with the other Gas Station packs.

## Render Pipeline Compatibility

| Pipeline | Supported | Notes |
|---|---|---|
| Built-in | Yes | RSG Shader Graph; install **Shader Graph** from the Package Manager (step 0) |
| URP | Yes | same materials, no conversion |
| HDRP | Yes | same materials, no conversion |

One material set works in all three pipelines: every product material uses the cross-pipeline Shader Graph `RSG_Lit` (shared with the other RetroStyleGames packs, `RetroStyleGames/Base/Shaders`).

## Technical Details

| Item | Value |
|---|---|
| Unique meshes | 25 Unity mesh assets |
| Prefabs | 25 (one per model) |
| Materials | 25 total: 22 product + 1 demo ground + 2 night sky materials |
| Textures | 61 PNG |
| Texture resolution | 4 - 2048 px (power of two) |
| Texture maps | Base Color `_BC` (sRGB), `_MAS` Metallic R / G = B = 1 (no baked AO) / Smoothness A (linear), Normal `_N` (OpenGL), Emissive `_E` |
| Total triangles | 14,414 (sum of unique meshes) |
| Total vertices | 39,882 (Unity vertex count) |
| Triangles per model | 2 - 2,308 |
| LODs | No |
| UV channels | 1 |
| Vertex colors | No |
| Rigging / animation | No |
| Colliders | MeshCollider on every prefab |
| Scale | 1 unit = 1 m, every prefab root at position 0 / rotation 0 / scale 1 |
| Scenes | 3 demo scenes (Built-in, URP, HDRP) |
| Scripts | 2 C# (namespace `GasStationPack.Interaction`): `GasStationCarryable` on the 24 product instances in each demo scene (impact sound on collision) and its `GasStationCarAlarm` dependency; no player input or render-pipeline code. The 25 reusable prefabs are static props, without these interaction components. |
| Audio | 2 WAV impact clips (Kenney, CC0) used by product instances in the demo scenes |
| Unity version | source exported from 6000.0.59f2; this delivery import- and Play-tested in 6000.0.77f1, minimum Unity 6.0 |
| Dependencies | Shader Graph 17.0+ on Built-in (URP / HDRP include it) |

## Models

| Group | Mesh | Triangles | Vertices | Size (m) |
|---|---|---|---|---|
| Building | SM_RSG_Wall_Straight | 12 | 24 | 3.00 x 3.18 x 0.55 |
| Building | SM_RSG_Wall_Window | 192 | 384 | 3.00 x 3.18 x 0.67 |
| Building | SM_RSG_Wall_Door | 188 | 362 | 3.00 x 3.18 x 0.63 |
| Building | SM_RSG_Wall_Gates | 176 | 384 | 3.00 x 3.18 x 0.93 |
| Building | SM_RSG_Wall_Column | 12 | 24 | 1.02 x 3.18 x 1.02 |
| Building | SM_RSG_Wall_TopStraight | 36 | 72 | 3.00 x 0.67 x 0.76 |
| Building | SM_RSG_Wall_TopCube | 36 | 72 | 1.17 x 0.52 x 1.19 |
| Building | SM_RSG_Door_Double | 488 | 1,040 | 1.15 x 2.34 x 0.29 |
| Building | SM_RSG_Slab_Floor | 2 | 4 | 10.78 x 0.00 x 9.56 |
| Building | SM_RSG_Slab_Ceiling | 2 | 4 | 10.78 x 0.00 x 9.56 |
| Building | SM_RSG_Slab_Roof | 2 | 4 | 10.78 x 0.00 x 9.56 |
| Store furniture | SM_RSG_Shelf | 2,044 | 4,688 | 1.20 x 2.10 x 0.80 |
| Store furniture | SM_RSG_ShopCounter | 1,216 | 3,648 | 2.27 x 1.07 x 0.73 |
| Store furniture | SM_RSG_CashRegister | 2,308 | 6,924 | 0.44 x 0.34 x 0.45 |
| Store furniture | SM_RSG_IceFreezer | 2,132 | 6,396 | 1.29 x 1.85 x 0.92 |
| Products | SM_RSG_SodaCan | 1,220 | 3,660 | 0.07 x 0.14 x 0.07 |
| Products | SM_RSG_FoodCan | 1,004 | 3,012 | 0.08 x 0.11 x 0.08 |
| Products | SM_RSG_WaterBottle | 1,912 | 5,736 | 0.06 x 0.21 x 0.06 |
| Outside | SM_RSG_Pavement_Straight_A | 16 | 48 | 1.00 x 0.07 x 1.00 |
| Outside | SM_RSG_Pavement_Straight_B | 16 | 48 | 1.00 x 0.07 x 1.00 |
| Outside | SM_RSG_Pavement_Corner_A | 138 | 414 | 1.00 x 0.07 x 1.00 |
| Outside | SM_RSG_Pavement_Corner_B | 138 | 414 | 1.00 x 0.07 x 1.00 |
| Outside | SM_RSG_Pavement_Infill_A | 12 | 36 | 1.00 x 0.07 x 1.00 |
| Outside | SM_RSG_Pavement_Infill_B | 12 | 36 | 1.00 x 0.07 x 1.00 |
| Outside | SM_RSG_Lamp | 1,100 | 2,448 | 1.02 x 4.48 x 0.43 |
| **Total** | | **14,414** | **39,882** | **25 meshes** |

## Textures

The 20 texture sets contain Base Color, MAS and Normal maps. Lamp_Glow and Neon_RedCreek are additional emissive materials; this delivery has no separate emissive texture maps. Resolutions: 48 x 2048x2048, 3 x 1024x1024, 9 x 512x512, 1 x 4x4.

## Features

- 25 game-ready models, 14,414 triangles in total, every model with its own PBR texture set (4 - 2048 px)
- Modular building on a 3 m wall grid: straight, window, door and gate walls, corner column and cornice pieces snap together
- Assembled store demo scene for Built-in, URP and HDRP: the shop as it stands in the Complete, night lighting, interior lamps
- One material set for all three render pipelines (RetroStyleGames `RSG_Lit` Shader Graph)
- Colliders on every prefab, prefab roots at 0 / 0 / 1, 1 unit = 1 m

## Included Content

```
Assets/RetroStyleGames/
  Base/Shaders/RSG_Lit.shadergraph      shared RetroStyleGames cross-pipeline shader
  GasStation/
    GasStation_FreeStore_Documentation.md
    Models/      SM_RSG_CashRegister, SM_RSG_Door_Double, SM_RSG_FoodCan, SM_RSG_IceFreezer, SM_RSG_Lamp, SM_RSG_Pavement_Corner_A, SM_RSG_Pavement_Corner_B, SM_RSG_Pavement_Infill_A, SM_RSG_Pavement_Infill_B, SM_RSG_Pavement_Straight_A, SM_RSG_Pavement_Straight_B, SM_RSG_Shelf, SM_RSG_ShopCounter, SM_RSG_Slab_Ceiling, SM_RSG_Slab_Floor, SM_RSG_Slab_Roof, SM_RSG_SodaCan, SM_RSG_Wall_Column, SM_RSG_Wall_Door, SM_RSG_Wall_Gates, SM_RSG_Wall_Straight, SM_RSG_Wall_TopCube, SM_RSG_Wall_TopStraight, SM_RSG_Wall_Window, SM_RSG_WaterBottle
    Materials/   M_RSG_CashRegister, M_RSG_FoodCan, M_RSG_IceFreezer, M_RSG_Lamp_Body, M_RSG_Lamp_Glow, M_RSG_Neon_RedCreek, M_RSG_Pavement_Corner, M_RSG_Pavement_Infill, M_RSG_Pavement_Straight, M_RSG_Shelf, M_RSG_ShopCounter, M_RSG_Slab_ConcreteTiles, M_RSG_Slab_Ruberoid, M_RSG_SodaCan, M_RSG_Wall_Column, M_RSG_Wall_Door, M_RSG_Wall_Gates, M_RSG_Wall_Straight, M_RSG_Wall_TopCube, M_RSG_Wall_TopStraight, M_RSG_Wall_Window, M_RSG_WaterBottle
    Textures/<Group>/  61 PNG
    Prefabs/     P_RSG_CashRegister, P_RSG_Door_Double, P_RSG_FoodCan, P_RSG_IceFreezer, P_RSG_Lamp, P_RSG_Pavement_Corner_A, P_RSG_Pavement_Corner_B, P_RSG_Pavement_Infill_A, P_RSG_Pavement_Infill_B, P_RSG_Pavement_Straight_A, P_RSG_Pavement_Straight_B, P_RSG_Shelf, P_RSG_ShopCounter, P_RSG_Slab_Ceiling, P_RSG_Slab_Floor, P_RSG_Slab_Roof, P_RSG_SodaCan, P_RSG_Wall_Column, P_RSG_Wall_Door, P_RSG_Wall_Gates, P_RSG_Wall_Straight, P_RSG_Wall_TopCube, P_RSG_Wall_TopStraight, P_RSG_Wall_Window, P_RSG_WaterBottle
    Interaction/ Scripts/GasStationCarryable.cs + GasStationCarAlarm.cs, Audio/S_RSG_Impact_Metal_A/B.wav + Kenney_impact_LICENSE.txt
    Scenes/FreeStore/GasStation_FreeStore_Demo_{BiRP,URP,HDRP}.unity + M_RSG_FreeStore_Ground.mat + VP_RSG_FreeStore_Night_HDRP.asset
    Settings/    night sky (HDRI + sky materials), URP / HDRP volume profiles of the demo scenes
    Terrain/Reflections/T_RSG_Reflection_Shop.exr   baked reflection of the shop interior
```

## Demo Scene

Open `GasStation/Scenes/FreeStore/GasStation_FreeStore_Demo_<Pipeline>` (BiRP = Built-in): the store exactly as it stands in the Complete's night scene - 10 shelves, the counter with the register, two ICE freezers at the front, 24 products on three shelves, 58 paving tiles around the foundation and 4 lamps - lit by the Complete's shop lights (interior ceiling lights, facade spots, lamp spots), with a plain dark ground plane. The enabled camera shows the store front in Play. For inspecting the lit interior, use the editor Scene view; this Free pack does not include a first-person player controller. The HDRP scene uses its own copy of the night volume profile (`Scenes/FreeStore/VP_RSG_FreeStore_Night_HDRP`, fixed exposure EV -6) because the store has fewer lights than the Complete's forecourt; the Built-in / URP scenes use a lighter fog (exp2 density 0.07 instead of the Complete's forest fog 0.269) so the store reads from the street.

## Setup Instructions

0. **Built-in only:** install *Shader Graph* (Window > Package Manager > Unity Registry > Shader Graph, 17.0 or newer). URP and HDRP already include it.
1. Keep *Project Settings > Player > Color Space* on **Linear** (the default of the Unity 6 templates).
2. Import the package.
3. Open the demo scene of your pipeline, or drag prefabs from `GasStation/Prefabs` into your own scenes.

## Notes

- Walls are 3 m long and 3.18 m high; place them on a 3 m grid, corner columns at the grid points, cornice pieces on top.
- The floor, ceiling and roof slabs are single planes sized for the demo store (10.8 x 9.6 m).
- GPU instancing is off on the RSG materials: on Built-in, Shader Graph's instanced path draws a whole instanced batch at one object's position. Keep it off when you duplicate materials for Built-in projects (URP / HDRP use the SRP Batcher).
- The demo scenes reference the product prefabs directly, like the Complete's demo scenes. The 24 product instances add a kinematic Rigidbody, BoxCollider and GasStationCarryable; their inherited MeshCollider is disabled. Reusable product prefabs remain static and have no interaction script or Rigidbody.
- BRP shadow bias is at least 0.05; URP lights use their own Depth/Normal bias of 1/1.
- FBX copies of the models with their textures are delivered separately (`GasStation_FreeStore_FBX.zip`).

## AI-Generated Content

Disclosed per Asset Store guideline 1.6:

- The 3D models and their textures were created with an AI-assisted workflow (AI image generation and AI image-to-3D generation) and then edited, textured and prepared for real-time use by RetroStyleGames artists.

## Part of Gas Station - Complete Environment

All models of this pack are also included in **Gas Station - Complete Environment** (fuel pumps and canopy, vehicles, forest, survival and trash props, a lit night scene and a playable first-person demo for Built-in, URP and HDRP).

## Support

RetroStyleGames - https://retrostylegames.com

## License

Standard Unity Asset Store EULA. Third-party content: two Kenney Impact Sounds clips (author Kenney, CC0 1.0 Universal, https://kenney.nl/assets/impact-sounds); the licence notice ships with the clips.
