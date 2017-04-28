namespace HueLights;
        // class declarations
         class HueLight;
         class HueRoom;
         class HueGroup;
         class HueBridge;
         class HueProc;
         class HueBulb;
         class HueScene;
     class HueLight 
    {
        // class delegates

        // class events

        // class functions
        FUNCTION GetBulb ();
        FUNCTION LightsVal ( STRING settype , STRING lvltype , INTEGER val );
        FUNCTION LightsOn ();
        FUNCTION LightsOff ();
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER BulbID;
        STRING BulbName[];
        INTEGER BulbIsOn;
        STRING BulbType[];
        INTEGER BulbBri;
        INTEGER BulbHue;
        INTEGER BulbSat;
        INTEGER Reachable;

        // class properties
    };

     class HueRoom 
    {
        // class delegates

        // class events
        EventHandler RoomBriUpdate ( HueRoom sender, EventArgs e );
        EventHandler RoomHueUpdate ( HueRoom sender, EventArgs e );
        EventHandler RoomSatUpdate ( HueRoom sender, EventArgs e );
        EventHandler RoomOnlineUpdate ( HueRoom sender, EventArgs e );

        // class functions
        FUNCTION GetRoom ();
        FUNCTION GroupOn ();
        FUNCTION GroupOff ();
        FUNCTION RecallRaw ( STRING rawID );
        FUNCTION RecallScene ( INTEGER i );
        FUNCTION TriggerRoomBriUpdate ();
        FUNCTION TriggerRoomHueUpdate ();
        FUNCTION TriggerRoomSatUpdate ();
        FUNCTION TriggerRoomOnlineUpdate ();
        FUNCTION LightsVal ( STRING settype , STRING lvltype , INTEGER val );
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER RoomID;
        INTEGER GroupIsOn;
        STRING GroupName[];
        INTEGER RoomBri;
        INTEGER RoomHue;
        INTEGER RoomSat;
        INTEGER SceneNum;
        INTEGER RoomOnline;
        STRING SceneName[][];
        STRING SceneID[][];

        // class properties
    };

    static class HueBridge 
    {
        // class delegates

        // class events

        // class functions
        static FUNCTION register ();
        static FUNCTION SetupDataStore ();
        static STRING_FUNCTION GetDataStore ();
        static STRING_FUNCTION getIP ();
        static STRING_FUNCTION GetBridgeInfo ( STRING infotype );
        static STRING_FUNCTION SetOnOff ( STRING settype , INTEGER setid , STRING value , STRING cmdtype );
        static STRING_FUNCTION SetScene ( INTEGER setid , STRING payload );
        static STRING_FUNCTION SetLvl ( STRING settype , STRING lvltype , INTEGER setid , INTEGER value , STRING cmdtype );
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        static STRING BridgeIp[];
        static STRING BridgeApi[];

        // class properties
    };

     class HueProc 
    {
        // class delegates
        delegate INTEGER_FUNCTION DelegateValueUpdate ( );

        // class events

        // class functions
        FUNCTION Register ();
        FUNCTION getIP ();
        FUNCTION setIP ( STRING str );
        FUNCTION getBulbs ();
        FUNCTION getRooms ();
        FUNCTION getScenes ();
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        STRING IPSet[];
        STRING IPAddress[];
        INTEGER Authorized;
        STRING APIKey[];
        INTEGER BulbNum;
        INTEGER GroupNum;

        // class properties
        DelegateProperty DelegateValueUpdate ValueUpdate;
    };

