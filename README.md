# OpenIndustryProject

Free and Open-source PLC driven warehouse/manufacturing simulation made with [Unity](https://unity.com/) and [libplctag.NET](https://github.com/libplctag/libplctag.NET). 

The goal is to provide an open platform for developers to contribute to the creation of virtual industrial equipment/devices and for people to be able to test their ideas or simply educate themselves using a real PLC.

Scroll down to the **Getting Started** section for information on how to work with this project. 

Join our discord group: [Open Industry Project](https://discord.gg/ACRPr6sBpH)

Supported PLCs:

ControlLogix,Micro800/850,PLC-5 ,SLC 500,MicroLogix,Omron NX/NJ Series,Modbus TCP (Help us add more!) 

## Demo

https://user-images.githubusercontent.com/105675984/232076379-6daaa2ef-f203-4381-a7f7-b994346c1ed5.mp4

## Scaleable Objects 

![scale](https://user-images.githubusercontent.com/105675984/228063593-c49b5f93-1ecf-47da-bb42-fd077a8112ce.gif)

## Realistic Physics

![box](https://user-images.githubusercontent.com/105675984/228373219-b74487d8-7b1b-4008-a998-6d3e4f1197f7.gif)

## Getting Started

For getting started with Unity refer to: [Getting Started](https://docs.unity3d.com/560/Documentation/Manual/GettingStarted.html)

Clone the repository or download and extract the ZIP file and select the project folder directly in Unity Hub.

The buttons in the below image are located at the top of the repositories page. 

![image](https://user-images.githubusercontent.com/105675984/230959413-ad75e1fe-8ce8-49f4-bfc1-85247b67e678.png)

The intent is to utilize the abilities of the Unity Editor itself to instantitate equiptment and design your systems. There, objects can be resized, moved around, and adjusted in real-time while the scene is running. It is possible if desired to build your scene into an executable, which can be then used as presentation material. 

Inside the Unity Editor add a PLC object from the 'Assets' folder to the scene and fill in the script fields with the necessary data for your test bench. 

![image](https://user-images.githubusercontent.com/105675984/218582555-4a450d03-8b2e-499c-b1ca-a4e286d686b8.png)

All equipment and devices come with their own script and fields that also need to be configured

![image](https://user-images.githubusercontent.com/105675984/218584052-5b67fdb5-4e44-461f-a5f4-87fe4ebe888d.png)

## Importing Models

This is a good resource for free industrial parts CAD models: [3dfindit](https://www.3dfindit.com/en/)

It is recommened to export the files in their native format (usually STEP), modify them if needed for usage in Unity in any CAD software and then export as FBX for additional work in Blender, or to be imported straight into Unity. 

Alternatively most manufacturers provide the CAD files directly on their own website. 

## Help Wanted

More equipment and devices,
better exception handling,
review code,
documentation,
training videos?


