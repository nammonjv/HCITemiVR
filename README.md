# HCITemiVR
 
## About this project

This project was created with students of the HCI Lab internship program to study programming to remotely control robots and is in development for further Virtual Reality technologies.
Within this project is the code of the Unity program used to transmit data received from KATVR to MQTT to control the Temi robot.

## Requirement

1. Unity (2021.3.4f1 or newer)
2. SteamVR Asset
3. KATVR SDK (Please install all KAT VR .dll files)
4. MQTT Broker

## Basic

To access the data recieved from KAT VR, you can use command

```C#
KATVR.KATDevice_Walk.[DataName]
```

You can access this data from any scene.

For MQTT please go to 

M2MqttUnity > Examples > Scenes > M2MqttUnity_Test

to see the example. If you don't under stand the process please follow link from reference for more tutorial.

and you can see our code that use with KAT VR in script 
```
ValueTest
```


## Reference

[MQTT Library](https://github.com/CE-SDV-Unity/M2MqttUnity)
