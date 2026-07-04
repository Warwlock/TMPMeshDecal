# Text Mesh Pro - Mesh Decal

A performant, CPU-side vertex projection decal system for **TextMeshPro** in Unity. Project 3D text onto walls, floors, and uneven meshes without using custom shaders or screen-space decals. Requires collider for the projected surface.

Features
--------
- **Shaderless:** Works with standard TMP shaders. Perfect for mobile, VR, or custom pipelines.
- **Automated:** Automatically projects only when text content, position, rotation or scale changes.
- **Editor Ready:** Updates in real-time inside the Scene view without hitting Play (`[ExecuteAlways]`).


How to Install (Unity Package Manager)
--------
1. Open your Unity Project.
2. Go to `Window > Package Manager`.
3. Click the **`+`** icon in the top left corner.
4. Select **"Add package from git URL..."**
5. Paste your GitHub repository URL:
   `https://github.com/YourUsername/TMPMeshDecal.git`
6. Click **Add**.


How to Use
--------

1. Go to your **Text Mesh Pro** object.
2. Add the component: `TMPDecalProjector`

Limitations & Trade-offs
--------
Because this system uses **CPU-side vertex projection** instead of **GPU shaders**, it comes with a few inherent technical trade-offs:

* **Requires Physics Colliders:** Since the system uses `Physics.Raycast` to find surfaces, any wall or floor you project onto **must have a Collider** (e.g., MeshCollider, BoxCollider). It will not project onto visual meshes without collision data.
* **Vertex Density Limitations:** By default, each character in TextMeshPro is a quad made of only 4 vertices (the corners). If a large character spans across a sharp 90-degree corner, it will not curve smoothly around it; the 4 corners will snap to the walls, causing the middle of the letter to bridge the gap flatly. 
  * *Tip: For best results, keep text size relatively small compared to the curvature of your surfaces.*
* **Not Suitable for Massive Amounts of Text:** While optimized to run only when moved, raycasting and rebuilding mesh data on the CPU is heavier than shader-based decals. Having **hundreds of decals moving simultaneously every single frame** may impact CPU performance.
* **Ignores Normal/Bump Maps:** The vertices snap to the *physical* mesh geometry. Visual surface details created by Normal Maps or Height Maps will not affect how the text deforms.
  * *Tip: You can try to use custom shader with transparent to blend with alpha to mimic normal blending.*

Images
--------
- A stretched text

![](https://raw.githubusercontent.com/Warwlock/TMPMeshDecal/refs/heads/main/Documentation~/Stretched.png)

- Curved Surface example

![](https://raw.githubusercontent.com/Warwlock/TMPMeshDecal/refs/heads/main/Documentation%7E/CurvedSurface.png)

- Works on URP Decal surfaces.

![](https://raw.githubusercontent.com/Warwlock/TMPMeshDecal/refs/heads/main/Documentation~/WorksWithDecal.png)

- Submeshes also works!

![](https://raw.githubusercontent.com/Warwlock/TMPMeshDecal/refs/heads/main/Documentation~/SubMeshSupport.png)