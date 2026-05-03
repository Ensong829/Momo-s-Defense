# Sprite Asset Guide

Use these rules for Momo walk strips and any similar character walk assets in this project.

## Two Asset Types

This project currently uses two different kinds of walk-strip assets for Momo:

- Runtime 6-frame strips:
  - `momo_walk_down.png`
  - `momo_walk_left.png`
  - `momo_walk_right.png`
  - `momo_walk_up.png`
- Raw side-walk source strips:
  - `walkleft.png`
  - `walkright.png`

The runtime strips are the Unity-facing assets.

The raw side-walk source strips are larger source sheets that may need normalization before use.

## Required Format

- Each walk strip must contain exactly `6` frames.
- Each frame must be exactly `100 x 100` pixels.
- Final strip size must be exactly `600 x 100` pixels.
- The asset width must be divisible by `6`.
- The asset must use a transparent background.
- Do not bake a checkerboard or white matte into the PNG.

## Naming

Put character walk strips in [Assets/Resources/Momo](C:/Users/masil/Desktop/PROJECTS/momo's defense/Assets/Resources/Momo).

Use these filenames:

- `momo_walk_down.png`
- `momo_walk_left.png`
- `momo_walk_right.png`
- `momo_walk_up.png`

Raw side-walk source files may also exist in the same folder as:

- `walkleft.png`
- `walkright.png`

## Layout Rules

- Keep one direction per file.
- Keep all `6` frames in a single horizontal strip.
- Each frame must sit inside its own `100 x 100` area.
- Keep the character visually centered in each frame.
- Keep the feet aligned to a shared baseline across the strip.
- Leave enough transparent padding so swords, capes, and hair do not touch frame edges.
- Do not let any frame content spill into the neighboring `100 x 100` area.

For raw 10-frame side strips:

- Final strip size must be exactly `1000 x 100` pixels.
- The asset width must be divisible by `10`.
- Each frame must still be exactly `100 x 100`.
- Keep the character visually centered in each frame just like the 6-frame runtime strips.
- Keep the feet aligned to the same baseline across all `10` frames.

## Transparency Rules

- Export as `.png`.
- Background must be fully transparent.
- Before using the file in Unity, verify corner pixels are transparent.
- If you see a white square, gray checkerboard, or matte edge in Unity, the image is not truly transparent and must be fixed before use.

Important for `walkleft.png` and `walkright.png`:

- The saved source image may contain a baked light checkerboard or matte even when it looks transparent at a glance.
- Treat that checkerboard as source background and strip it out during normalization.
- Do not rely on alpha alone to detect transparency in those raw source files.

## Unity Import

For each walk strip:

1. Place the PNG in [Assets/Resources/Momo](C:/Users/masil/Desktop/PROJECTS/momo's defense/Assets/Resources/Momo).
2. Select the PNG in Unity.
3. Set `Texture Type` to `Sprite (2D and UI)`.
4. Set `Sprite Mode` to `Multiple`.
5. Set `Mesh Type` to `Tight`.
6. Set `Compression` to `None`.
7. Enable `Alpha Is Transparency`.
8. Apply the import settings.

## Unity Slicing

Slice each strip in Unity instead of cutting frames in external code.

1. Open `Sprite Editor`.
2. Choose `Slice`.
3. Use `Grid By Cell Size`.
4. Set cell size to `100 x 100`.
5. Slice the strip into `6` frames.
6. Check that each frame is centered and grounded correctly.
7. Apply.

## Validation Checklist

Before considering a sprite strip finished, confirm:

- Image size is `600 x 100`.
- Background is transparent.
- There are `6` frames.
- Every frame fits inside a `100 x 100` cell.
- No sword, cape, or body part is cut off.
- No frame crosses into the next cell.
- The character does not drift sideways between frames unless the motion intentionally requires it.
- The walk reads cleanly in Unity after slicing.

For 10-frame side strips, also confirm:

- Image size is `1000 x 100`.
- There are exactly `10` frames.
- Each pose is centered within its own `100 x 100` cell.
- The character footprint is visually consistent with `momo_walk_up.png` and `momo_walk_down.png`.
- Corner pixels are fully transparent after removing the baked source matte.

## What Worked

The following workflow worked reliably for fixing `walkleft.png` and `walkright.png`:

1. Use the newly saved `walkleft.png` and `walkright.png` files as the source, not the older `momo_walk_left.png` or `momo_walk_right.png` runtime strips.
2. Treat those files as raw 10-frame source sheets, not finished runtime strips.
3. Detect frame regions by finding non-background columns rather than trusting the existing canvas or alpha channel.
4. Ignore tiny 1-pixel noise columns when finding frame runs.
5. Crop each detected pose to its real non-background bounds.
6. Remove the baked light checkerboard background while cropping so the final export becomes truly transparent.
7. Re-pack the cropped poses into a brand-new horizontal strip using a strict `100 x 100` grid.
8. Use one shared scale for every frame in the strip.
9. Use one shared baseline for every frame in the strip.
10. Center each pose horizontally inside its own `100 x 100` cell.
11. Validate that the final strip width is exactly:
    - `600` for 6-frame strips
    - `1000` for 10-frame strips
12. Validate that the strip height is exactly `100`.
13. Validate that frame bounds stay comfortably inside each cell so swords and capes do not hit the edges.
14. Validate that the corner pixels have alpha `0`.

## Side-Walk Normalization Recipe

Use this exact approach when another agent needs to repair `walkleft.png` or `walkright.png`:

1. Open the raw source strip.
2. Identify the background by color, not just alpha.
3. Detect the `10` real pose runs across the strip.
4. Drop tiny noise runs that are only artifacts.
5. Compute each pose bounding box.
6. Find the tallest pose in the strip.
7. Choose a shared scale so the tallest pose fits comfortably inside `100 x 100` with transparent padding.
8. Create a new transparent canvas sized `1000 x 100`.
9. For each frame:
   - Crop the pose.
   - Remove the baked background.
   - Scale the pose using the shared strip scale.
   - Center it horizontally in its `100 x 100` cell.
   - Align the feet to the shared baseline.
10. Export as PNG.
11. Re-check the final result visually for:
   - even spacing
   - no cutoffs
   - transparent background
   - consistent scale

## Things That Failed

Avoid these mistakes. They produced bad results during this task:

- Reusing the broken runtime side strips as the source of truth.
- Shifting already cut-off frames instead of rebuilding from the newer saved source files.
- Letting each frame use its own scale.
- Letting frames drift left or right inside the `100 x 100` cell.
- Treating the baked checkerboard in `walkleft.png` and `walkright.png` as true transparency.
- Trusting a strip only because its total size was divisible by the frame count.

If the poses are not evenly centered inside the cells, the strip is still wrong even if the file size is mathematically correct.

## Current Project Expectation

The current Momo runtime expects four sliced walk strips:

- `momo_walk_down`
- `momo_walk_left`
- `momo_walk_right`
- `momo_walk_up`

The game loads the Unity-sliced sprites from those imported assets, so manual slicing in Unity is the correct workflow.
