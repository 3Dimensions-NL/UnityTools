# Tools

A collection of utility tools designed to improve the development experience in Unity. This package contains various tools to streamline workflows and enhance productivity during project creation and maintenance. These tools are created with flexibility and customization in mind, enabling developers to adapt to different project requirements easily.

## Features

- **Quality of Life Enhancements**: Simplifies Unity project management by integrating user-friendly tools.
- **Flexible Integration**: Easily incorporate the tools in new or existing Unity projects.
- **Inspector Enhancements**: Tools designed to improve the Unity Editor experience and provide additional functionality for managing projects.
- **Debugging & Profiling**: Features to assist developers in analyzing and optimizing project performance.

## Tools Overview

### 1. **Core Functionality**
- Reusable tools that integrate into development pipelines.
- Unity-specific utilities that enhance workflows and productivity.

### 2. **Utility Functions**
- Provides developers with a collection of commonly used scripts and modules to reduce repetitive tasks.

### 3. **Inspector Customization**
- Extends the basic capabilities of Unity’s default inspector to simplify debugging and working with complex objects.

### 4. **Debugging & Profiling**
- Debugging helpers and profiling tools to track and optimize runtime performance.

## Requirements

The tools package has been built and tested in **Unity Editor 2022.3.55f1** with the **Built-In Render Pipeline**. It has no external dependencies beyond Unity modules/packages.

## Supported Unity Modules

These tools make use of the following Unity-provided modules and packages, which should be active in your Unity project:

- Unity Core Modules:
    - `com.unity.modules.animation`
    - `com.unity.modules.audio`
    - `com.unity.modules.physics`
    - `com.unity.modules.imgui`
    - And others – please verify the packages needed for your setup.
- Other key packages:
    - `com.unity.textmeshpro`
    - `com.unity.mathematics`
    - `com.unity.inputsystem`

## Installation

This Unity package is compatible for installation via the **Unity Package Manager**.

### 1. Install via Git URL
The easiest way to use this tool is by adding the package directly from this repository using Unity's Package Manager:

1. Open your Unity Project.
2. Navigate to `Window -> Package Manager`.
3. In the Package Manager, select the `+` button in the top left corner and choose `Add package from git URL...`.
4. Enter the following URL:
   ```
   https://github.com/yourusername/your-repository.git
   ```
5. Click `Add` to install the package.

### 2. Install via Manual Download
Alternatively, you can download the package as a `.zip` file and install it manually:

1. Download the package repository from the GitHub repository as a `.zip` file.
2. Extract the contents into your Unity project's `Packages` folder.
3. The Unity Editor will automatically detect the package.

## Usage

Once the package is installed, the tools are ready to use. Navigate to the relevant tool scripts or components and integrate them as needed within your Unity project. Each tool is modular and can be used independently or combined.

### Included Sample
This package also includes a **Sample** with multiple demo example scenes to help you get started quickly. Upon importing the sample, you can explore the following demos:

- **Scenario**: Demonstrates tools designed for managing scenarios.
- **Text**: Includes examples showcasing text utilities and workflows.
- **Utilities**: Contains demonstrations for general-purpose tools and utilities.

To import the sample:

1. Open the Unity Package Manager (`Window -> Package Manager`).
2. Select the package from the list.
3. Click the `Import Samples` button and choose the desired sample scenes to include in your project.

These resources can help you understand how to integrate and use the tools effectively within your projects.

## Known Issues

- These tools are developed and tested with the **Universal Render Pipeline**. Compatibility with other render pipelines (Built-in, HDRP) may require adjustments depending on your project setup.

## License

This project is licensed under the [MIT License](LICENSE).

---

### Disclaimer

This package has been tailored for and used in our internal Unity projects. While we aim to create versatile tools, others may require adjustments to match specific project setups.

Feel free to customize this file further to address any specific features or tools related to your project.