# PhillipsHueSIMPLSharp
Crestron SIMPLSharp Phillips Hue Implementation

Current features
- Bridge discovery, Bridge registration, bulb/Room count
- Hue Room on/off/Tog/Color Loop, Brightness, Hue, Saturation adjustment, 20 scenes, XY color adjustment

This implementation of the Phillips Hue will discover a connected bridge on the local subnet, if a Hue bridge is on DHCP I would create a nightly routine to request bridge IP just in case or set it up for DHCP reservation.
50 bulbs are supported in this module since a single bridge can only support 50 bulbs however this module could be easily augmented to increase the bulb count with additional bridges in play.

The Processor module communicates directly to the bridge. Trigger the Hue Init sig after program startup. This will discover the bridge IP if one is not provided in the module serial input join.

After the IP for a bridge is established the Register function must run. To register press the physical button on the top of the bridge and then trigger the register function. A digital feedback will show successful registration. Registration will determine the username (AKA API Key) used to send all commands. When the API key is parsed it is stored in the CrestronDataStore. After reboots or program uploads, the registration sequence will attempt to read from the DataStore, if no key is in the DataStore then the manual button on the Hue Bridge will need to be pushed.

If the Init routine is being run for the first time be sure to press the button on the bridge prior to the Hue Init signal going high or simply trigger it manualy from SIMPLdebugger.

The Hue Init sequence pulls the bulbs, rooms (groups) and scenes from the bridge which then allows the individual Room modules to work on a particular room. The paramemter field of the room module must match the room name that you wish to control. Currently there is a bug where if you have a room defined in the Hue mobile app but it doesn't have any bulbs in it the bridge request routine will fail, this will be fixed in an upcoming ver, be sure to remove all rooms without bulbs until then. If using with a UI I would trigger the get rooms when changing subpages to a room to pull the latest feedback for the room.

The HueBulb module needs some work, I've found it more useful to control the Hue room as a whole rather then individual bulbs which is why it hasn't been a priority for me.

SIMPL windows and sample UI code are included. Works splendidly in my house where I have a blend of Crestron and Phillips Hue lighting
