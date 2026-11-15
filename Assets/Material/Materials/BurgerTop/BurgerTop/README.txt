UNITY IMPORT
1. Copy this folder into Assets. Drag BurgerTop.obj into the scene.
2. Assign your bread material to its Mesh Renderer (MTL color may require manual assignment depending on render pipeline).
3. Add Mesh Collider to the mesh object. Assign BurgerTop_Collider mesh to its Mesh field; enable Convex; keep Is Trigger OFF. Do not render the collider model.
4. Add Rigidbody if it should fall. For vertical-only stacking, freeze Position X/Z and all Rotation axes. Spawn above existing food without overlap.
5. Set scene tool to Pivot, not Center, to see the base-center origin.

Dimensions: X/Z diameter 2 units; Y height 0.6 units; base Y=0; origin=(0,0,0). Scale uniformly to match food diameter.
Visual: 1150 triangles; collider: 158 triangles. Both are closed convex meshes with outward winding. The bottom is completely flat. Top UV uses planar X/Z mapping, suitable for bread-dot texture.
Geometry was checked for closed edges and nonzero triangle areas. Unity runtime physics was not tested here.
