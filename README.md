# PhillipsHueSIMPLSharp
Crestron SIMPLSharp Phillips Hue Implementation

This implementation of the Phillips Hue Will discover a connected brdige on the local subnet,  
if Bridge is on DHCP I would create a nightly routine to request bridge IP jsut in case.

50 bulbs are supported in this module since a single bridge can only support 50 bulbs
however this module could be easily augmented to increase the bulb count.

After the IP for a bridge is established the Register function must run. 
To Register press the physical button on the top of the bridge and then trigger the register function. 
A digital feedback will show successful registration. Registration will determine the username (AKA API Key)
used to send all commands. When the API key is parsed it is stored in the CrestronDataStore. 
After reboots or program uploads, the regitration sequence will attempt to read from the DataStore
before requiring the manual button on the HueBridge being pushed.

Then a bulb request should be issued which will parse all the bulbs currently on the HueBridge, 
if Bulbs are changed our, names, etc a new bulb quest query should be issued.

Scenes tied to rooms/groups is on my task list next, The scenes have already been parsed 
(in an archaic fashion unfortunately due to the partcular JSON structure they chose to use 
for that particular get request however it does work and puts it into a C# object list.
Whats left is to reconcile the Scene list by matching the first bulb tied to a particular scene
back to the room/group list which also aggrogates a specific bulb as tied to that room/group.

It would have been sweet if Phillips just put the room/group a scene was assigned to in the 
Scene get request but oh well.

SIMPL windows and sample UI code are included. Works splendidly in my house where I have a 
blend of Crestron and Phillips Hue lighting
