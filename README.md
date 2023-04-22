# OpenIndustryProject

Free and Open-source PLC driven warehouse/manufacturing simulation made with [Unity](https://unity.com/), [OPC UA .NET](https://github.com/OPCFoundation/UA-.NETStandard), and [libplctag](https://github.com/libplctag/libplctag). 

The goal is to provide an open platform for developers to contribute to the creation of virtual industrial equipment/devices and for people to be able to test their ideas or simply educate themselves using a real PLC.

Scroll down to the **Getting Started** section for information on how to work with this project. 

Join our discord group: [Open Industry Project](https://discord.gg/ACRPr6sBpH)

Supported Communication Protocols:

- OPC UA 
- Ethernet/IP via libplctag
- Modbus TCP via libplctag

## Demo

https://user-images.githubusercontent.com/105675984/232076379-6daaa2ef-f203-4381-a7f7-b994346c1ed5.mp4

## Scaleable Objects 

![scale](https://user-images.githubusercontent.com/105675984/228063593-c49b5f93-1ecf-47da-bb42-fd077a8112ce.gif)

## Realistic Physics

![box](https://user-images.githubusercontent.com/105675984/228373219-b74487d8-7b1b-4008-a998-6d3e4f1197f7.gif)

## Getting Started

Installation Instructions:

1. Download Github Desktop and install on local machine. [Download link](https://desktop.github.com/)

	a. You don't need an account to download the project file using Github Desktop. In the installation welcome screen, click 'Skip this step' instead of logging in or creating an account.

	![image](https://user-images.githubusercontent.com/131540786/233792831-d851df0b-e585-4a0f-b43f-aecbf0facac3.png)

	b. Alternatively, if you don't want the desktop application, download and extract the project folder to your computer. 

2. Clone the project in Github Desktop using the link from this page.

	a. In Github Desktop, select 'Clone a repository from the Internet...'. 

	b. Select the tab 'URL' and then copy and paste the URL from the top of this page. Choose a destination folder.

	![image](https://user-images.githubusercontent.com/131540786/233792833-7f62ce6f-0b0b-4251-9d4b-49ba2493a802.png)

	![image](https://user-images.githubusercontent.com/131540786/233792834-c83f14fd-d1c3-45a2-8377-71d74dc88e10.png)

	c. Click 'Clone'.

3. Install Git for your operating system. [Git Downloads](https://git-scm.com/downloads)

	a. Without installing Git, you'll receive an error from Unity when opening the project.

	b. I kept all settings default during the installation of Git.

4. Download Unity Hub installer for your operating system from [here](https://unity.com/download).

	a. Install Unity Hub. 

	b. Sign in or create an account with Unity. 

	c. Install Unity Editor using the prompts from Unity Hub (requires 5 GB disk space). Use personal edition license.

	![image](https://user-images.githubusercontent.com/131540786/233792836-65536a95-f8ff-477f-8782-a7aec40f96cf.png)

	c. Downloads should start for Unity 2021.3.22f1. This may take awhile to fully download and install. Microsoft .NET Framework 4.8 is also installed during this process.

5. Once Unity Editor is installed, navigate to the 'Projects' tab and 'Open'. Browse to Github project folder from Step #1. Select entire folder and 'Open'.

	![image](https://user-images.githubusercontent.com/131540786/233792840-f17c4c5a-adb9-4b56-a709-38b9710b558e.png)

	![image](https://user-images.githubusercontent.com/131540786/233792841-9310017d-d817-422c-b059-433154a44d92.png)

6. Once imported, click on the project to open in Unity.

	a. If you get the error 'An error has occurred while resolving packages: Project has invalid dependecies: No 'git' executable was found' you'll need to install Git and restart your computer. This should resolve the error.

	![image](https://user-images.githubusercontent.com/131540786/233792845-d27d7b22-0b05-4940-8a7e-254d3c3806b9.png)


For getting started with Unity refer to: [Getting Started](https://docs.unity3d.com/560/Documentation/Manual/GettingStarted.html)

	For a practical project to learn the basics of Unity refer to the [Roll-a-ball Tutorial](https://learn.unity.com/project/roll-a-ball).

The intent is to utilize the abilities of the Unity Editor itself to instantiate equipment and design your systems. There, objects can be resized, moved around, and adjusted in real-time while the scene is running. It is possible if desired to build your scene into an executable, which can be then used as presentation material. 

Inside the Unity Editor add a PLC object from the 'Assets' folder to the scene and fill in the script fields with the necessary data for your test bench. 

![image](https://user-images.githubusercontent.com/105675984/218582555-4a450d03-8b2e-499c-b1ca-a4e286d686b8.png)

Equipment and Devices are located in inside the assets folder, and can be dragged into the scene:

![image](https://user-images.githubusercontent.com/105675984/232112274-dd880d11-418c-4ab2-97de-e01b064a951b.png)

All equipment and devices come with their own script and fields that also need to be configured

![image](https://user-images.githubusercontent.com/105675984/218584052-5b67fdb5-4e44-461f-a5f4-87fe4ebe888d.png)

## Importing Models

Although this project has a few models, you maybe interested in adding more. 

This is a good resource for free industrial parts CAD models: [3dfindit](https://www.3dfindit.com/en/)

It is recommened to export the files in their native format (usually STEP), modify them if needed for usage in Unity in any CAD software and then export as FBX for additional work in Blender, or to be imported straight into Unity. 

Alternatively most manufacturers provide the CAD files directly on their own website. 

## Help Wanted

- More equipment and devices
- Better exception handling
- Review code
- Documentation
- Training videos?


