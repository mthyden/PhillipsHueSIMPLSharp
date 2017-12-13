# PhilipsHueSIMPLSharp
Crestron SIMPLSharp Phillips Hue Implementation

Current features:
- Bridge discovery, Bridge registration, bulb/Room count
- Hue Room on/off/Tog/Color Loop, Brightness, Hue, Saturation adjustment, 20 scenes, XY color adjustment
- Hue bulb adjustment
- SIMPL windows and sample UI code are included. 

How it works:
This implementation of the Phillips Hue will discover a connected bridge on the local subnet, if a Hue bridge is on DHCP I would create a nightly routine to request bridge IP just in case or set it up for DHCP reservation.
50 bulbs are supported in this module since a single bridge can only support 50 bulbs however this module could be easily augmented to increase the bulb count with additional bridges in play.

The Processor module communicates directly to the bridge. Trigger the Hue Init sig after program startup. This will discover the bridge IP if one is not provided in the module serial input join.

After the IP for a bridge is established the Register function must run. To register press the physical button on the top of the bridge and then trigger the register function. A digital feedback will show successful registration. Registration will determine the username (AKA API Key) used to send all commands. When the API key is parsed it is stored in the CrestronDataStore. After reboots or program uploads, the registration sequence will attempt to read from the DataStore, if no key is in the DataStore then the manual button on the Hue Bridge will need to be pushed.

Hue Room and Hue Bulb modules now have the ability to be defined in one of two ways, either via the parameter field in the module requiring the exact room and or bulb name (case sensitive) or from the serial join input which can be fed directly from an ISC symbol that hooks to the output from the main processor module. Those two output serials contain the list of discovered bulbs and discovered rooms. Empty rooms are AOK, this has been resolved in the 2.1 room module.

known issues:
- Using the parameter field to populate the room name for the Room modules will inadvertently pass the literal "[#Room Name]" I need to investigate this further but it may be a bug in SIMPL Windows ver 4.8.15

Best Practices:
If the Init routine is being run for the first time be sure to press the button on the bridge prior to the Hue Init signal going high or simply trigger it manualy from SIMPLdebugger.

The Hue Init sequence pulls the bulbs, rooms (groups) and scenes from the bridge which then allows the individual Room modules to work on a particular room. The paramemter field of the room module must match the room name that you wish to control. Currently there is a bug where  this will be fixed in an upcoming ver, be sure to remove all rooms without bulbs until then. If using with a UI I would trigger the get rooms when changing subpages to a room to pull the latest feedback for the room.




