# 🌫️ Atmospheric Misty Fog Setup Guide

Complete guide for creating beautiful, atmospheric fog rising from the bottom of your screen in Unity.

## ⚠️ Should You Remove the Current Fog System?

**NO!** Keep the `FogParticles.cs` script - it's properly configured. You just need to set it up correctly in the Unity Editor following this guide.

---

## 📋 Prerequisites

- Unity 2D project with URP (Universal Render Pipeline)
- A scene with a camera (you have this already)

---

## 🎨 Step 1: Create the Fog Texture/Sprite

You have three options:

### Option A: Use GIMP (Best Quality - Detailed Guide)

**Download GIMP (free):** https://www.gimp.org/downloads/

#### Step-by-Step GIMP Tutorial:

1. **Create New Image**
   - Open GIMP
   - Go to **File** → **New**
   - Set **Width**: 512, **Height**: 512
   - Click **Advanced Options**
   - Under **Fill with**: Select **Transparency**
   - Click **OK**

2. **Create a White Circle**
   - Select **Ellipse Select Tool** (E key or from toolbox)
   - Hold **Shift + Drag** from center to create a perfect circle
   - Or set **Fixed**: Aspect ratio 1:1 in tool options
   - Position circle in the center (leave some margin at edges)
   
3. **Fill the Circle with White**
   - Make sure Foreground color is **White** (click foreground color → set to white)
   - Go to **Edit** → **Fill with FG Color**
   - Or press **Ctrl+;** (semi-colon)
   - Deselect: **Select** → **None** (or Shift+Ctrl+A)

4. **Apply Strong Gaussian Blur**
   - Go to **Filters** → **Blur** → **Gaussian Blur**
   - Set **Size X**: 100
   - Set **Size Y**: 100
   - Make sure **Chain icon** is linked (same value for both)
   - Click **OK**
   - Wait for processing...

5. **Apply Second Blur for Extra Softness**
   - Go to **Filters** → **Blur** → **Gaussian Blur** again
   - Set **Size X**: 60
   - Set **Size Y**: 60
   - Click **OK**

6. **Fade the Edges to Transparent**
   - Right-click the layer in **Layers panel**
   - Select **Alpha to Selection** (creates selection based on transparency)
   - Go to **Select** → **Shrink**
   - Enter **50** pixels
   - Click **OK**
   - Go to **Select** → **Invert** (or Ctrl+I)
   - Press **Delete** key to remove outer edge
   - Go to **Select** → **Feather**
   - Enter **80** pixels
   - Click **OK**
   - Press **Delete** key again
   - Deselect: **Select** → **None**

7. **Adjust Opacity (Optional)**
   - In **Layers panel**, adjust **Opacity** slider to 80-90%
   - This creates softer fog

8. **Export as PNG**
   - Go to **File** → **Export As** (NOT "Save As")
   - Name it: `fog_particle.png`
   - Make sure file extension is **.png**
   - Click **Export**
   - In PNG export dialog, keep defaults and click **Export** again

9. **Import to Unity**
   - Locate your exported `fog_particle.png` file
   - Drag it into Unity's **Assets** folder (create an `Assets/Sprites/` folder if needed)
   - Done!

#### Quick Alternative Method in GIMP:

If the above seems complex, try this faster method:

1. **File** → **New** → 512x512, Fill with Transparency
2. **Filters** → **Render** → **Gfig** (or use Ellipse tool to draw a white circle)
3. Create white circle in center
4. **Filters** → **Blur** → **Gaussian Blur** → 120 pixels
5. **Filters** → **Light and Shadow** → **Vignette**
   - Set **Soften**: 1.0
   - Set **Gamma**: 1.0
   - Click **OK**
6. **File** → **Export As** → `fog_particle.png`

#### 💡 GIMP Tips & What You Should See:

**Visual Checkpoints:**
- After Step 3 (Fill): You should see a solid white circle on transparent background (checkerboard pattern)
- After Step 4 (First blur): Circle edges should be soft and fuzzy
- After Step 5 (Second blur): Circle should look very soft, like a white cloud
- After Step 6 (Fade edges): Circle should gradually fade to completely transparent at edges
- **Final result**: A soft, cloud-like white blob that's invisible at the edges

**Common GIMP Issues:**

❌ **"I don't see transparency, just white background"**
- Make sure you selected **Transparency** when creating new image
- Check **Windows** → **Dockable Dialogs** → **Layers** to see layer has transparency

❌ **"Export As saves as .xcf instead of .png"**
- Type the full filename with extension: `fog_particle.png`
- Make sure you use **Export As**, not **Save As** (Save As creates GIMP files)

❌ **"My circle has hard edges after blur"**
- Increase blur values to 120-150
- Apply blur twice or three times
- Make sure you deselected (Select → None) before applying blur

❌ **"The Gaussian Blur option is grayed out"**
- Make sure your layer isn't locked
- Right-click layer → Add Alpha Channel if missing

**Keyboard Shortcuts:**
- **E**: Ellipse Select Tool
- **Shift+Ctrl+A**: Deselect All
- **Ctrl+Z**: Undo
- **Tab**: Hide/Show all panels (see your work better)

#### ⚡ FASTEST GIMP Method (30 Seconds):

For absolute beginners who want something quick that works:

1. **File** → **New** → 512x512, **Fill with**: Transparency → OK
2. **Filters** → **Render** → **Clouds** → **Solid Noise**
   - Set **X Size**: 1.5
   - Set **Y Size**: 1.5
   - Set **Detail**: 1
   - Click **OK**
3. **Colors** → **Invert** (makes it white on transparent)
4. **Filters** → **Blur** → **Gaussian Blur** → 80 pixels → OK
5. **Colors** → **Brightness-Contrast**
   - Increase **Brightness**: +30
   - Decrease **Contrast**: -20
   - Click **OK**
6. **File** → **Export As** → `fog_particle.png`

This creates a softer, more organic fog texture in under 1 minute!

---

### Option A2: Use Krita (Also Free, More Modern UI)

**Download Krita:** https://krita.org/en/download/

1. **File** → **New** → 512x512px, Transparent background
2. Select **Ellipse Tool** (circle with square icon)
3. Draw a white circle in the center
4. **Filter** → **Blur** → **Gaussian Blur** → Radius: 100
5. **Filter** → **Adjust** → **Levels** → Move right slider left slightly (fades edges)
6. **File** → **Export** → Save as PNG
7. Import to Unity

### Option B: Use Online Tools (Quick & Easy)

1. Visit: https://www.remove.bg/upload or similar
2. Or use: https://www.photopea.com (free Photoshop alternative)
3. Create a soft, feathered white circle with gradual transparency
4. Download as PNG
5. Import into Unity

### Option C: Use Unity's Default (Fastest, But Less Realistic)

1. Use Unity's built-in **Default-Particle** sprite
2. Located at: `Assets/Sprites/Default-Particle`
3. Skip to Step 2

---

## 🖼️ Step 2: Configure the Fog Sprite in Unity

1. Select your fog sprite in the Project window
2. In the **Inspector**:
   - **Texture Type**: Sprite (2D and UI)
   - **Sprite Mode**: Single
   - **Mesh Type**: Full Rect
   - **Pixels Per Unit**: 100
   - **Filter Mode**: Bilinear
   - **Max Size**: 512 or 1024
3. Click **Apply**

---

## 🎭 Step 3: Create the Fog Material

1. **Right-click** in Project window → **Create** → **Material**
2. Name it: `FogMaterial`
3. In Inspector, configure:
   
   **For URP (Universal Render Pipeline):**
   - **Shader**: Select `Universal Render Pipeline/Particles/Unlit`
   - **Surface Type**: Transparent
   - **Blending Mode**: Alpha
   - **Base Map**: Drag your fog sprite here
   - **Base Color**: White (or light gray for darker fog)
   - **Alpha**: Adjust to control transparency (0.2 - 0.5 works well)

   **Alternative Shader Options:**
   - `Particles/Standard Unlit` (legacy)
   - `Sprites/Default` (simple, but less control)

4. **Important Settings:**
   - Enable **Soft Particles** if available (creates smoother edges)
   - Disable **Cast Shadows**
   - Disable **Receive Shadows**

---

## 🎮 Step 4: Create the Fog Particle System

1. In your scene (e.g., Level1), **right-click Hierarchy** → **Effects** → **Particle System**
2. Rename it: `Fog`
3. **Add the FogParticles script:**
   - Select the Fog GameObject
   - Click **Add Component**
   - Search for `FogParticles`
   - Add it

---

## ⚙️ Step 5: Configure the Particle System Renderer

1. Select the `Fog` GameObject
2. Scroll down to **Particle System Renderer** component:
   - **Render Mode**: Billboard
   - **Material**: Drag your `FogMaterial` here
   - **Sorting Layer**: Default
   - **Order in Layer**: 1000 (or higher than any UI)

---

## 🎛️ Step 6: Adjust Fog Settings (Optional)

The script automatically configures everything, but you can tweak these values in the Inspector:

### Fog Settings:
- **Max Particles**: 100 (increase for denser fog, decrease for performance)
- **Fog Rise Speed**: 1.5 (how fast fog rises)
- **Fog Size**: 5 (particle size - larger = more blurry)
- **Fog Color**: Light gray with low alpha (0.7, 0.7, 0.7, 0.2)

### Area Settings:
- **Spawn Width**: 25 (should cover camera view width + extra)
- **Bottom Offset**: 5 (how far below camera to spawn)

---

## 🎨 Step 7: Advanced Visual Enhancements (Optional)

### For Extra Blur Effect:

1. Select your `FogMaterial`
2. Duplicate it (Ctrl+D) → Name: `FogMaterial_ExtraBlur`
3. In the duplicated material:
   - Increase **Base Color** alpha slightly
   - Or add a **Secondary Texture** with even softer edges

### For Color Tinted Fog:

Change the **Fog Color** in the FogParticles script:
- Blue-gray fog: `(0.6, 0.7, 0.8, 0.2)`
- Purple fog: `(0.7, 0.6, 0.8, 0.2)`
- Green fog (toxic): `(0.6, 0.8, 0.6, 0.2)`

### For Layered Fog:

1. Duplicate the entire `Fog` GameObject
2. Rename to `Fog_Layer2`
3. In FogParticles script:
   - Reduce **Fog Size** to 3
   - Change **Fog Rise Speed** to 1.0
   - Adjust **Bottom Offset** to 4

This creates depth with multiple fog layers!

---

## 🔍 Step 8: Testing

1. **Play your scene**
2. You should see:
   - Soft fog rising from the bottom of the screen
   - Fog growing and fading as it rises
   - Fog always in the foreground (over everything)
   - Fog following the camera as you move

---

## 🐛 Troubleshooting

### Problem: Fog particles are too sharp/not blurry

**Solution:**
- Use a softer sprite with more feathering
- Increase **Fog Size** to 6-8
- Enable **Soft Particles** in the material
- Make sure your sprite has a gradual alpha fade

### Problem: Fog not visible

**Solution:**
- Check **Order in Layer** is very high (1000+)
- Verify fog color alpha isn't 0
- Make sure **Culling Mask** on Camera includes particle layer
- Check Z-position is positive (closer to camera)

### Problem: Fog too dense/opaque

**Solution:**
- Reduce **Max Particles** (try 50-75)
- Lower fog color alpha (0.15 instead of 0.2)
- Reduce **Emission** rate in particle system

### Problem: Fog doesn't cover full screen

**Solution:**
- Increase **Spawn Width** (try 30-40)
- Adjust **Bottom Offset** (try 6-8)

### Problem: Performance issues/lag

**Solution:**
- Reduce **Max Particles** (try 50 instead of 100)
- Use smaller texture size (256x256 instead of 512x512)
- Reduce **Emission Rate**

---

## 📊 Recommended Settings for Different Effects

### Subtle Atmospheric Fog
```
Max Particles: 75
Fog Rise Speed: 1.2
Fog Size: 4
Fog Color: (0.8, 0.8, 0.8, 0.15)
```

### Dense Mysterious Fog
```
Max Particles: 150
Fog Rise Speed: 1.0
Fog Size: 6
Fog Color: (0.6, 0.6, 0.6, 0.25)
```

### Fast Rising Steam
```
Max Particles: 100
Fog Rise Speed: 2.5
Fog Size: 3
Fog Color: (0.9, 0.9, 0.9, 0.3)
```

### Toxic/Neon Fog
```
Max Particles: 100
Fog Rise Speed: 1.5
Fog Size: 5
Fog Color: (0.2, 1.0, 0.3, 0.2) // Neon green
```

---

## 🎯 Layer Order Reference

For correct rendering order (bottom to top):
1. **Background** (Sorting Order: -100)
2. **Level Tiles/Sprites** (Sorting Order: 0)
3. **Player & Enemies** (Sorting Order: 10)
4. **Neon Stars** (Sorting Order: 50)
5. **UI Elements** (Sorting Order: 100-500)
6. **Fog** (Sorting Order: 1000) ← Absolute foreground

---

## ✅ Final Checklist

- [ ] Fog sprite created with soft, feathered edges
- [ ] Sprite imported and configured in Unity
- [ ] Material created with correct shader (URP/Particles/Unlit)
- [ ] Material assigned to Particle System Renderer
- [ ] FogParticles script attached to Particle System GameObject
- [ ] Sorting Order set to 1000+
- [ ] Tested in Play mode
- [ ] Fog rises from bottom of screen
- [ ] Fog stays in foreground
- [ ] Fog follows camera movement

---

## 📚 Additional Resources

**Creating Soft Sprites:**
- Photopea: https://www.photopea.com
- Krita (free): https://krita.org
- GIMP (free): https://www.gimp.org

**Free Fog/Smoke Textures:**
- OpenGameArt: https://opengameart.org (search "smoke" or "fog")
- itch.io: https://itch.io/game-assets/free (filter by "particles")
- Unity Asset Store: Search "particle pack" (many free options)

**Unity Particle System Docs:**
- https://docs.unity3d.com/Manual/PartSysReference.html

---

## 🎨 Pro Tip: Combine with Other Effects

For maximum atmosphere:
1. ✅ Fog (foreground layer) - YOU HAVE THIS
2. ✅ Neon Stars (mid layer) - YOU HAVE THIS
3. ⭐ Add subtle camera shake on player landing
4. ⭐ Add screen color grading (slightly desaturated + cyan tint)
5. ⭐ Add vignette post-processing effect

This creates a complete cyberpunk/atmospheric aesthetic! 🌃

---

Made for LeidenRooftopRun 🏃‍♂️
