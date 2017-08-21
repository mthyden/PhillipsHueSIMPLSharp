namespace HueLights;
        // class declarations
         class HueLight;
         class HueRoom;
         class HueGroup;
         class InfoEventArgs;
         class HueBridge;
         class HueProc;
         class HueBulb;
         class HueScene;
     class HueLight 
    {
        // class delegates

        // class events
        EventHandler BulbBriUpdate ( HueLight sender, EventArgs e );
        EventHandler BulbHueUpdate ( HueLight sender, EventArgs e );
        EventHandler BulbSatUpdate ( HueLight sender, EventArgs e );
        EventHandler BulbOnlineUpdate ( HueLight sender, EventArgs e );
        EventHandler BulbOnOffUpdate ( HueLight sender, EventArgs e );

        // class functions
        FUNCTION GetBulb ();
        FUNCTION LightsVal ( STRING settype , STRING lvltype , INTEGER val );
        FUNCTION LightsAction ( STRING actiontype , STRING actioncmd , STRING effect );
        FUNCTION TriggerBulbBriUpdate ();
        FUNCTION TriggerBulbHueUpdate ();
        FUNCTION TriggerBulbSatUpdate ();
        FUNCTION TriggerBulbOnOffUpdate ();
        FUNCTION TriggerBulbOnlineUpdate ();
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
        EventHandler RoomOnOffUpdate ( HueRoom sender, EventArgs e );
        EventHandler RoomOnlineUpdate ( HueRoom sender, EventArgs e );

        // class functions
        FUNCTION GetRoom ();
        FUNCTION GroupAction ( STRING actiontype , STRING actioncmd , STRING effect );
        FUNCTION RecallScene ( INTEGER i );
        FUNCTION TriggerRoomBriUpdate ();
        FUNCTION TriggerRoomHueUpdate ();
        FUNCTION TriggerRoomSatUpdate ();
        FUNCTION TriggerRoomOnOffUpdate ();
        FUNCTION TriggerRoomOnlineUpdate ();
        FUNCTION LightsVal ( STRING settype , STRING lvltype , INTEGER val );
        FUNCTION XYVal ( STRING settype , INTEGER xval , INTEGER yval );
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER RoomID;
        INTEGER GroupIsOn;
        STRING GroupName[];
        INTEGER RoomBri;
        INTEGER RoomHue;
        INTEGER RoomSat;
        INTEGER RoomXVal;
        INTEGER RoomYVal;
        INTEGER SceneNum;
        INTEGER RoomOnline;
        STRING SceneName[][];
        STRING SceneId[][];

        // class properties
    };

     class InfoEventArgs 
    {
        // class delegates

        // class events

        // class functions
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER __class_id__;

        // class properties
        STRING InfoType[];
        STRING JsonData[];
    };

    static class HueBridge 
    {
        // class delegates

        // class events

        // class functions
        static FUNCTION register ();
        static FUNCTION SetupDataStore ();
        static STRING_FUNCTION GetDataStore ();
        static FUNCTION ResetDataStore ();
        static STRING_FUNCTION getIP ();
        static FUNCTION GetBridgeInfo ( STRING infotype );
        static STRING_FUNCTION SetOnOff ( STRING settype , INTEGER setid , STRING value , STRING cmdtype , STRING effect );
        static STRING_FUNCTION SetScene ( INTEGER setid , STRING payload );
        static STRING_FUNCTION SetLvl ( STRING settype , INTEGER setid , STRING cmdtype , STRING cmdval );
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
        EventHandler InitComplete ( HueProc sender, EventArgs e );

        // class functions
        FUNCTION Register ();
        FUNCTION ResetAPI ();
        FUNCTION getIP ();
        FUNCTION setIP ( STRING str );
        FUNCTION getData ();
        FUNCTION OnInitComplete ();
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        STRING IPSet[];
        STRING IPAddress[];
        INTEGER Authorized;
        STRING APIKey[];
        INTEGER BulbNum;
        INTEGER GroupNum;
        INTEGER HueOnline;

        // class properties
        DelegateProperty DelegateValueUpdate ValueUpdate;
    };

