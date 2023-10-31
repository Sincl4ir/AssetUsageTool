# AssetUsageTool - WORK IN PROGRESS

Unity asset usage tool



# Asset Usage Tool

## Overview

The Asset Usage Tool is a Unity Editor extension that allows you to check the usage of an asset within your Unity project. It helps you find where a particular asset is used, whether it's referenced in scenes, prefabs, or script files.

## Features

- Check where an asset is used within the project.
- Differentiate between scenes, prefabs, and script files.
- Provides information about the location of asset usage.
- User-friendly interface within the Unity Editor.

## Getting Started

### Prerequisites

- Unity 2021.3.17f or later.

### Installation

1. Clone or download this repository.
2. Copy the `AssetUsageCheckerTool/Editor` folder into your Unity project.

### How to Use

OPTION A:
1. Open the Unity Editor.
2. Go to the "Custom" menu.
3. Select "Asset Usage Checker" to open the tool.
4. In the Asset Usage Tool window, select the asset you want to check.
5. Click the "Check Usage" button to start the search.
6. The tool will display a list of scenes, prefabs, or script files that use the selected asset.

OPTION B:
1. Right click the asset you want to perform a search and select "Check Asset Usage in the Project".

### Know Bugs:

There is an issue loading scenes that are part of read-only packages. 

### To do list:

* Refactor handlers to segregate checks depending on ObjectUsageChecker types from the class itself.
* Testings (Diff unity versions, diferents scenarios)
* Refactor View to read UI settings from scriptable
