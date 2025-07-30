# 🦾 Joint Torque Visualizer

A Windows Forms-based application that visually simulates and analyzes joint torques in robotic arms using an intuitive heatmap-based stick diagram. Built as a module for **RoboAnalyzer**, this tool is useful for understanding torque distribution and **path feasibility** across joints of 2R, 3R, and 6R manipulators.

---

## 🧠 Features

- ✅ **Supports up to 6 joints**
- 🔥 **Color-coded torque heatmap** for easy visualization
- 📈 **Real-time torque plot** as line/bar graph
- 🌀 **Animation** of joint motion from 0 to θ (anticlockwise)
- 🎮 **Play, Pause, Stop controls** for simulation
- 🧩 **User Input Panel**: 
  - Number of joints (max 6)
  - Payload (affects torque calculation)
  - Joint angles (θ) per joint
- 🧵 **Real-time path trace** for each joint in torque-indicated color
- ⚠️ **Path Feasibility Analysis**:
  - Accepts a user-drawn path
  - Evaluates if the path is feasible with respect to joint torque limits and configuration constraints
- 🧰 **Seamless integration with RoboAnalyzer workflow**

---

## 🖥️ How It Works

1. **User Inputs**:
   - Select joint count (2R, 3R, 6R).
   - Input payload weight.
   - Input desired joint angles or draw a desired trajectory.

2. **Visualization Panel**:
   - Displays stick diagram of robotic arm.
   - Each link colored based on torque using a heatmap (e.g. blue = low, red = high).
   - Joint path trace with same color code.

3. **Graph Panel**:
   - Shows torque magnitude at each joint using line or bar graph.
   - Updates dynamically with animation.

4. **Animation**:
   - Visual representation of motion from 0 to input θ.
   - Controls allow real-time simulation and analysis.

5. **Path Feasibility Module**:
   - Users can draw a custom path on the workspace.
   - System simulates joint configuration across the path.
   - Feasibility is flagged based on torque limits, joint range, and possible singularities.

---

## 🧱 Tech Stack

- 💻 C# (.NET Framework)
- 📦 Windows Forms (WinForms)
- 🎨 Custom Graphics (GDI+)
- 🔢 Kinematic Calculations (Inverse Dynamics)
- 🔍 Path Feasibility Engine (Built-in Constraint Evaluator)
- 🔧 Modular Backend for Torque Analysis

---

## 🚀 Getting Started

### Prerequisites

- Visual Studio 2022 or newer
- .NET Framework 4.7.2 or later

### Run Instructions

1. Clone this repository:
   ```bash
   git clone https://github.com/yourusername/joint-torque-visualizer.git
