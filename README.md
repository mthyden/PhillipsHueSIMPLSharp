# PhillipsHueSIMPLSharp
Crestron SIMPLSharp Phillips Hue Implementation

This implementation of the Phillips Hue will discover a connected bridge on the local subnet, if a Hue bridge is on DHCP I would create a nightly routine to request bridge IP just in case or set it up for DHCP reservation.
50 bulbs are supported in this module since a single bridge can only support 50 bulbs however this module could be easily augmented to increase the bulb count with additional bridges in play.

After the IP for a bridge is established the Register function must run. To register press the physical button on the top of the bridge and then trigger the register function. A digital feedback will show successful registration. Registration will determine the username (AKA API Key) used to send all commands. When the API key is parsed it is stored in the CrestronDataStore. After reboots or program uploads, the registration sequence will attempt to read from the DataStore, if no key is in the DataStore then the manual button on the Hue Bridge will need to be pushed.

Then a bulb request should be issued which will parse all the bulbs currently on the Hue Bridge, if Bulbs are changed our, names, etc a new bulb quest query should be issued.

I have a room/group discovery as well which requests all the rooms in the system, then I can trigger room on/off and brightness functions directly on the room as a whole.

Scenes tied to rooms/groups is on my task list next. The scenes have already been parsed (in an archaic fashion unfortunately due to the particular JSON structure they chose to use for that particular get request however it does work and puts it into a C# object list. Whats left is to reconcile the Scene list by matching the first bulb tied to a particular scene back to the room/group list which also aggregates a specific bulb as tied to that room/group. It would have been ideal if Phillips put the room/group info in with the  scene request data. 

Responsiveness and feedback is pretty impressive, I had multiple lights cycling through various diming and color changes simultaneously without disruption or issue.


SIMPL windows and sample UI code are included. Works splendidly in my house where I have a 
blend of Crestron and Phillips Hue lighting
