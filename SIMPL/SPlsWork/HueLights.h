namespace HueLights;
        // class declarations
         class HueItem;
         class HueBulb;
         class HueGroup;
         class HueScene;
         class HueSensor;
         class HueMotion;
         class HueLight;
         class HueRoom;
         class InfoEventArgs;
         class PayloadType;
         class Payload;
         class HueBridge;
         class HueProc;
         class HttpConnect;
         class HttpsConnect;
     class HueItem 
    {
        // class delegates

        // class events

        // class functions
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        STRING Id[];
        STRING Name[];

        // class properties
    };

     class HueMotion 
    {
        // class delegates

        // class events
        EventHandler PresenceUpdate ( HueMotion sender, EventArgs e );

        // class functions
        FUNCTION MotionInit ();
        FUNCTION GetMotion ();
        FUNCTION TriggerPresenceUpdate ();
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER MotionId;
        STRING MotionName[];
        INTEGER MotionOnline;
        INTEGER MotionDaylight;
        INTEGER MotionPresence;
        INTEGER MotionTemp;

        // class properties
    };

     class HueLight 
    {
        // class delegates

        // class events
        EventHandler BulbBriUpdate ( HueLight sender, EventArgs e );
        EventHandler BulbHueUpdate ( HueLight sender, EventArgs e );
        EventHandler BulbSatUpdate ( HueLight sender, EventArgs e );
        EventHandler BulbOnlineUpdate ( HueLight sender, EventArgs e );
        EventHandler BulbUpdate ( HueLight sender, EventArgs e );
        EventHandler BulbOnOffUpdate ( HueLight sender, EventArgs e );

        // class functions
        FUNCTION BulbInit ();
        FUNCTION GetBulb ();
        FUNCTION LightsAction ( STRING lvltype , STRING val , STRING effect );
        FUNCTION LightsVal ( STRING lvltype , INTEGER val );
        FUNCTION TriggerBulbBriUpdate ();
        FUNCTION TriggerBulbHueUpdate ();
        FUNCTION TriggerBulbSatUpdate ();
        FUNCTION TriggerBulbOnOffUpdate ();
        FUNCTION TriggerBulbUpdate ();
        FUNCTION TriggerBulbOnlineUpdate ();
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER BulbId;
        STRING BulbName[];
        INTEGER BulbIsOn;
        STRING BulbType[];
        INTEGER BulbBri;
        INTEGER BulbHue;
        INTEGER BulbSat;
        INTEGER BulbOnline;
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
        EventHandler RoomUpdate ( HueRoom sender, EventArgs e );
        EventHandler RoomOnlineUpdate ( HueRoom sender, EventArgs e );

        // class functions
        FUNCTION RoomInit ();
        FUNCTION GetRoom ();
        FUNCTION GroupAction ( STRING lvltype , STRING val , STRING effect );
        FUNCTION LightsVal ( STRING lvltype , INTEGER val );
        FUNCTION RecallScene ( INTEGER i );
        FUNCTION XYVal ( INTEGER xval , INTEGER yval );
        FUNCTION TriggerRoomBriUpdate ();
        FUNCTION TriggerRoomHueUpdate ();
        FUNCTION TriggerRoomSatUpdate ();
        FUNCTION TriggerRoomOnOffUpdate ();
        FUNCTION TriggerRoomUpdate ();
        FUNCTION TriggerRoomOnlineUpdate ();
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER RoomId;
        INTEGER GroupIsOn;
        STRING GroupName[];
        STRING RoomClass[];
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

    static class PayloadType // enum
    {
        static SIGNED_LONG_INTEGER RoomOnOff;
        static SIGNED_LONG_INTEGER BulbOnOff;
        static SIGNED_LONG_INTEGER Lvl;
        static SIGNED_LONG_INTEGER XY;
        static SIGNED_LONG_INTEGER Scene;
    };

     class Payload 
    {
        // class delegates

        // class events

        // class functions
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER __class_id__;

        // class properties
        STRING OnOff[];
        INTEGER Lvl;
        STRING LvlType[];
        STRING Effect[];
        INTEGER Xval;
        INTEGER Yval;
        STRING Scene[];
        STRING SetType[];
        STRING CmdType[];
    };

    static class HueBridge 
    {
        // class delegates

        // class events

        // class functions
        static FUNCTION Register ();
        static FUNCTION SetupDataStore ();
        static STRING_FUNCTION GetDataStore ();
        static FUNCTION ResetDataStore ();
        static STRING_FUNCTION GetIp ();
        static FUNCTION GetBridgeInfo ( STRING infotype );
        static STRING_FUNCTION SetCmd ( PayloadType payloadtype , Payload payload , INTEGER setid );
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

        // class events
        EventHandler InitComplete ( HueProc sender, EventArgs e );

        // class functions
        FUNCTION Register ();
        FUNCTION ResetAPI ();
        FUNCTION getIP ();
        FUNCTION setIP ( STRING str );
        FUNCTION getData ();
        FUNCTION OnInitComplete ();
        FUNCTION ProcBulbs ( STRING jsondata );
        FUNCTION ProcRooms ( STRING jsondata );
        FUNCTION ProcScenes ( STRING jsondata );
        FUNCTION ProcSensors ( STRING jsondata );
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        STRING IPSet[];
        STRING IPAddress[];
        INTEGER Authorized;
        STRING APIKey[];
        INTEGER BulbNum;
        INTEGER GroupNum;
        INTEGER SensorNum;
        INTEGER HueOnline;
        STRING GrpName[][];
        STRING BlbName[][];

        // class properties
    };

     class HttpConnect 
    {
        // class delegates

        // class events

        // class functions
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER __class_id__;

        // class properties
        HttpConnect Instance;
    };

     class HttpsConnect 
    {
        // class delegates

        // class events

        // class functions
        STRING_FUNCTION Request ( STRING url , STRING cmd );
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER __class_id__;

        // class properties
        HttpsConnect Instance;
    };

