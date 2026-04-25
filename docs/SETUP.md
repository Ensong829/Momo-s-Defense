# Setup Notes

## Unity

Installed editor detected:

- Unity `6000.4.4f1`

The Unity project has been created in:

- `C:\Users\masil\Desktop\PROJECTS\momo's defense`

## Android Target

Android is the first target platform.

Current machine status:

- Unity editor is installed.
- Android Build Support is installed.
- Android SDK & NDK Tools are installed.
- OpenJDK is installed.

Needed Unity Hub modules:

- Android Build Support
- Android SDK & NDK Tools
- OpenJDK

The Unity project has been switched to the Android build target through Unity batch mode.

## Version Control

Git was not available from the current PowerShell environment during setup.

Target GitHub repository:

- `https://github.com/Ensong829/Momo-s-Defense`

Recommended next step:

- Install Git for Windows or make sure Git is available on `PATH`.
- Initialize the repository.
- Connect the local repository to `https://github.com/Ensong829/Momo-s-Defense`.
- Use Git LFS before adding large binary assets.

Planned commands once Git is available:

```powershell
git init
git branch -M main
git remote add origin https://github.com/Ensong829/Momo-s-Defense.git
git add .gitignore README.md docs Assets/_MomosDefense Packages ProjectSettings
git commit -m "Set up Unity project foundation"
git push -u origin main
```

## First Unity Open

Open the project through Unity Hub:

1. Open Unity Hub.
2. Choose Add project.
3. Select `C:\Users\masil\Desktop\PROJECTS\momo's defense`.
4. Open with Unity `6000.4.4f1`.

When Unity opens, it will generate imported asset metadata and may take a few minutes on first launch.

## Opening the Prototype Scene

After the project opens in Unity:

1. In the Project window, go to `Assets/_MomosDefense/Scenes`.
2. Double-click `Prototype_MomoDefense`.
3. Press Play.

The rough prototype currently uses primitive placeholder shapes. That is expected.

## Installing Android Modules

Unity's Android requirements should be installed through Unity Hub.

Steps:

1. Open Unity Hub.
2. Go to `Installs`.
3. Find Unity `6000.4.4f1`.
4. Click the gear or three-dot menu for that editor.
5. Choose `Add modules`.
6. Select `Android Build Support`.
7. Expand it and also select:
   - `Android SDK & NDK Tools`
   - `OpenJDK`
8. Continue and accept the prompts.

Unity recommends installing Android SDK, NDK, and OpenJDK through Unity Hub so the editor receives the matching versions.

Status: complete.
