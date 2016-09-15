namespace HueLights;
        // class declarations
         class HueBridge;
         class HueLight;
         class HueGroup;
         class HueBulb;
         class HueScene;
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
        static STRING_FUNCTION SetLights ( STRING settype , INTEGER setid , STRING value , STRING cmdtype );
        static STRING_FUNCTION SetScene ( STRING groupid , INTEGER sceneid , STRING payload );
        static STRING_FUNCTION SetBri ( STRING settype , STRING lvltype , INTEGER setid , INTEGER value );
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        static INTEGER Authorized;
        static STRING BridgeIp[];
        static STRING BridgeApi[];

        // class properties
    };

     class HueLight 
    {
        // class delegates
        delegate INTEGER_FUNCTION DelegateValueUpdate ( );

        // class events

        // class functions
        FUNCTION Register ();
        FUNCTION getIP ();
        FUNCTION getBulbs ();
        FUNCTION getRooms ();
        FUNCTION getScenes ();
        FUNCTION LightsVal ( INTEGER val , INTEGER lightnum , STRING settype , STRING lvltype );
        FUNCTION LightsOn ( INTEGER i );
        FUNCTION LightsOff ( INTEGER i );
        FUNCTION GroupOn ( INTEGER i );
        FUNCTION GroupOff ( INTEGER i );
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        STRING IPAddress[];
        INTEGER Authorized;
        STRING APIKey[];
        STRING BulbName[][];
        INTEGER BulbIsOn[];
        STRING BulbType[][];
        INTEGER BulbBri[];
        INTEGER BulbHue[];
        INTEGER BulbSat[];
        INTEGER Reachable[];
        INTEGER BulbNum;
        STRING GroupName[][];
        INTEGER GroupVal[];
        INTEGER GroupIsOn[];
        INTEGER GroupNum;

        // class properties
        DelegateProperty DelegateValueUpdate ValueUpdate;
    };

