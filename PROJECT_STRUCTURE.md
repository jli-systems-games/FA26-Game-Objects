# Human Museum — project structure

Main scene: `Assets/Scenes/MidtermMuseum.unity`

## Opening

Story: the humans are extinct; the museum was built afterwards from what they left behind.
The alien visitor starts inside its spaceship, presses E to open the hatch, crosses the bridge and enters the museum lobby.

## Runtime scripts (in the game) — `Assets/MuseumScripts/`

| Script | On | What it does |
|---|---|---|
| `MuseumVisitorController.cs` | MuseumVisitor | WASD move, mouse look, Shift run, V first/third person, F fullscreen, click to lock mouse, Esc to release, respawn after falling, drives walk animations |
| `MuseumExhibit.cs` | each exhibit | Holds the label text (title, meta, body, "why it is here", credit) and optional modes (iPhone stages, turntable records) |
| `MuseumExhibitInteractor.cs` | MuseumVisitor | Finds the nearest exhibit in front of you, shows "[E] INSPECT", opens/closes the scrollable panel, pauses movement while reading, plays UI sounds |
| `MuseumSlidingDoor.cs` | 08_ShipAirlock | Spaceship hatch at the start: press E nearby, it slides open and stays open |
| `MuseumAutoDoor.cs` | 09_EntranceAutoDoor | Museum entrance: glass doors slide apart automatically when the visitor is near, close when they leave |
| `MuseumMenu.cs` | Canvas/MuseumMenu | Start screen (START) and pause menu on TAB/P (RESUME, RESTART, EXIT); pauses time and locks movement while open |
| `MuseumHelmetMarker.cs` | (tag only) | Empty marker component used by a build tool |

## Editor build tools (NOT in the game) — `Assets/Editor/MuseumBuildTools/`

One-off helpers run from **Tools → Museum**. Each one placed things in the scene and saved it; the results now live in the scene.
They are numbered in the order they were used. You do not need to run them again — edit the scene directly.

| # | Tool | Built |
|---|---|---|
| 01 | PolygonFloor | Irregular hexagon floor with collider |
| 02 | Architecture | Glass walls, metal frames, light strips, entry bridge, lobby desk, plinths |
| 03 | PlayerSetup | Starter Assets visitor + controls HUD |
| 04 | Exhibit | iPhone exhibit + shared exhibit panel UI |
| 05 | PlaceModels | Downloaded exhibits on plinths; astronaut/alien visitor body |
| 06 | Shape | Crystal roof, entrance canopy, guiding fins |
| 07 | RecordPlayer | Glass fins, lobby record player with music |
| 08 | SketchfabModels | Turntable, synthesizer, Boeing 757 cockpit |
| 09 | SkyShip | Alien nebula skybox, parked spaceship |
| 10 | FourthRecord | 4th record + 4th panel button |
| 11 | PanelLayout | Larger, scrollable, screen-scaled exhibit panel |
| 12 | ShipAirlock | Airlock cabin at the bridge end; the visitor starts inside the ship and opens the hatch |
| 13 | Menu | Start screen + pause menu UI, buttons wired to MuseumMenu |
| 14 | ShipInterior | Moves/scales the imported spaceship so the airlock sits inside its hull; welcome text + hatch prompt as sharp screen UI |
| 15 | ShipHull | Scales the spaceship ~3x so its hull hides the cabin; cabin re-skinned as dark ship interior |
| 16 | StoryAndShipFix | Fixed ship size/position (explicit numbers), Aphex Twin as record 1, extinction story text |
| 17 | ShipReset | Reverts a moved part inside the spaceship model, measures it properly and wraps the hull around the cabin |
| 18 | ShipLayFlat | Lays the nose-up spaceship flat (tries 90-degree turns, keeps the flattest), then wraps the hull around the cabin |
| 19 | AutoDoor | Automatic sliding glass doors at the museum entrance |
| 20 | Font | Pixelify Sans (headings) + Space Mono (body) TMP font assets applied to all scene text |

## Assets

| Folder | Contents |
|---|---|
| `Assets/MuseumModels/` | Downloaded .glb models (imported with glTFast) |
| `Assets/MuseumAudio/` | Music for the turntable |
| `Assets/MuseumFonts/` | Pixelify Sans + Space Mono (OFL) and their TMP font assets |
| `Assets/MuseumSky/` | Nebula skybox image + material |
| `Assets/MuseumGenerated/` | Materials, meshes, animation controllers made by the tools |
| `Assets/MuseumGenerated/SceneBackups/` | Automatic scene backups taken before each tool ran (not in the build) |

Sources and licences of every model/sound: `MuseumCredits.md`.

## Rendering chain (keep intact)

Main Camera → `PixelRenderTexture` → `Canvas/PixelRender` (RawImage) → DisplayCamera → screen.
`Canvas/ColorNoise` adds the coloured noise on top of the pixelated image.
