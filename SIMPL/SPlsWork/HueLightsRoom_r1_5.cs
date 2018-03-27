using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using Crestron;
using Crestron.Logos.SplusLibrary;
using Crestron.Logos.SplusObjects;
using Crestron.SimplSharp;
using HueLights;

namespace UserModule_HUELIGHTSROOM_R1_5
{
    public class UserModuleClass_HUELIGHTSROOM_R1_5 : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        
        Crestron.Logos.SplusObjects.DigitalInput INIT;
        Crestron.Logos.SplusObjects.DigitalInput GETINFO;
        Crestron.Logos.SplusObjects.DigitalInput ROOMON;
        Crestron.Logos.SplusObjects.DigitalInput ROOMOFF;
        Crestron.Logos.SplusObjects.DigitalInput COLORLOOPON;
        Crestron.Logos.SplusObjects.AnalogInput ROOMBRIIN;
        Crestron.Logos.SplusObjects.AnalogInput ROOMHUEIN;
        Crestron.Logos.SplusObjects.AnalogInput ROOMSATIN;
        Crestron.Logos.SplusObjects.StringInput ROOMNAMELISTITEM;
        InOutArray<Crestron.Logos.SplusObjects.AnalogInput> XYVAL;
        InOutArray<Crestron.Logos.SplusObjects.DigitalInput> SCENES;
        Crestron.Logos.SplusObjects.DigitalOutput ROOMONLINE;
        Crestron.Logos.SplusObjects.DigitalOutput ROOMISON;
        Crestron.Logos.SplusObjects.AnalogOutput SCENESNUM;
        Crestron.Logos.SplusObjects.AnalogOutput ROOMBRIOUT;
        Crestron.Logos.SplusObjects.AnalogOutput ROOMHUEOUT;
        Crestron.Logos.SplusObjects.AnalogOutput ROOMSATOUT;
        Crestron.Logos.SplusObjects.StringOutput RMCLASS;
        InOutArray<Crestron.Logos.SplusObjects.StringOutput> ROOMSCENENAME;
        StringParameter ROOMNAME;
        HueLights.HueRoom MYROOM;
        ushort COLORLOOP = 0;
        object GETINFO_OnPush_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                
                __context__.SourceCodeLine = 61;
                MYROOM . GetRoom ( ) ; 
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    object INIT_OnPush_1 ( Object __EventInfo__ )
    
        { 
        Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
        try
        {
            SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
            
            __context__.SourceCodeLine = 66;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( Functions.Length( ROOMNAMELISTITEM ) > 0 ))  ) ) 
                { 
                __context__.SourceCodeLine = 68;
                MYROOM . GroupName  =  ( ROOMNAMELISTITEM  )  .ToString() ; 
                } 
            
            else 
                { 
                __context__.SourceCodeLine = 72;
                MYROOM . GroupName  =  ( ROOMNAME  )  .ToString() ; 
                } 
            
            __context__.SourceCodeLine = 74;
            MYROOM . RoomInit ( ) ; 
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler( __SignalEventArg__ ); }
        return this;
        
    }
    
object ROOMON_OnPush_2 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 79;
        if ( Functions.TestForTrue  ( ( COLORLOOP)  ) ) 
            { 
            __context__.SourceCodeLine = 81;
            MYROOM . GroupAction ( "on", "true", "colorloop") ; 
            } 
        
        else 
            { 
            __context__.SourceCodeLine = 85;
            MYROOM . GroupAction ( "on", "true", "none") ; 
            } 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object ROOMOFF_OnPush_3 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 91;
        MYROOM . GroupAction ( "on", "false", "none") ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object COLORLOOPON_OnPush_4 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 96;
        COLORLOOP = (ushort) ( 1 ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object COLORLOOPON_OnRelease_5 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 100;
        COLORLOOP = (ushort) ( 0 ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object SCENES_OnPush_6 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort I = 0;
        
        
        __context__.SourceCodeLine = 106;
        I = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
        __context__.SourceCodeLine = 107;
        MYROOM . RecallScene ( (ushort)( I )) ; 
        __context__.SourceCodeLine = 108;
        ROOMISON  .Value = (ushort) ( 1 ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object ROOMBRIIN_OnChange_7 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 113;
        MYROOM . LightsVal ( "bri", (ushort)( ROOMBRIIN  .UshortValue )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object ROOMHUEIN_OnChange_8 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 118;
        MYROOM . LightsVal ( "hue", (ushort)( ROOMHUEIN  .UshortValue )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object ROOMSATIN_OnChange_9 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 123;
        MYROOM . LightsVal ( "sat", (ushort)( ROOMSATIN  .UshortValue )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object XYVAL_OnChange_10 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 128;
        MYROOM . XYVal ( (ushort)( XYVAL[ 1 ] .UshortValue ), (ushort)( XYVAL[ 2 ] .UshortValue )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

public void MYONOFFHANDLER ( object __sender__ /*HueLights.HueRoom SENDER */, EventArgs ARGS ) 
    { 
    HueRoom  SENDER  = (HueRoom )__sender__;
    try
    {
        SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
        
        __context__.SourceCodeLine = 133;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SENDER == MYROOM))  ) ) 
            { 
            __context__.SourceCodeLine = 135;
            ROOMISON  .Value = (ushort) ( MYROOM.GroupIsOn ) ; 
            } 
        
        
        
    }
    finally { ObjectFinallyHandler(); }
    }
    
public void MYBRIHANDLER ( object __sender__ /*HueLights.HueRoom SENDER */, EventArgs ARGS ) 
    { 
    HueRoom  SENDER  = (HueRoom )__sender__;
    try
    {
        SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
        
        __context__.SourceCodeLine = 141;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SENDER == MYROOM))  ) ) 
            { 
            __context__.SourceCodeLine = 143;
            ROOMBRIOUT  .Value = (ushort) ( MYROOM.RoomBri ) ; 
            } 
        
        
        
    }
    finally { ObjectFinallyHandler(); }
    }
    
public void MYHUEHANDLER ( object __sender__ /*HueLights.HueRoom SENDER */, EventArgs ARGS ) 
    { 
    HueRoom  SENDER  = (HueRoom )__sender__;
    try
    {
        SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
        
        __context__.SourceCodeLine = 149;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SENDER == MYROOM))  ) ) 
            { 
            __context__.SourceCodeLine = 151;
            ROOMHUEOUT  .Value = (ushort) ( MYROOM.RoomHue ) ; 
            } 
        
        
        
    }
    finally { ObjectFinallyHandler(); }
    }
    
public void MYSATHANDLER ( object __sender__ /*HueLights.HueRoom SENDER */, EventArgs ARGS ) 
    { 
    HueRoom  SENDER  = (HueRoom )__sender__;
    try
    {
        SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
        
        __context__.SourceCodeLine = 157;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SENDER == MYROOM))  ) ) 
            { 
            __context__.SourceCodeLine = 159;
            ROOMSATOUT  .Value = (ushort) ( MYROOM.RoomSat ) ; 
            } 
        
        
        
    }
    finally { ObjectFinallyHandler(); }
    }
    
public void MYROOMHANDLER ( object __sender__ /*HueLights.HueRoom SENDER */, EventArgs ARGS ) 
    { 
    HueRoom  SENDER  = (HueRoom )__sender__;
    try
    {
        SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
        
        __context__.SourceCodeLine = 165;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SENDER == MYROOM))  ) ) 
            { 
            __context__.SourceCodeLine = 167;
            ROOMONLINE  .Value = (ushort) ( MYROOM.RoomOnline ) ; 
            __context__.SourceCodeLine = 168;
            ROOMISON  .Value = (ushort) ( MYROOM.GroupIsOn ) ; 
            __context__.SourceCodeLine = 169;
            ROOMBRIOUT  .Value = (ushort) ( MYROOM.RoomBri ) ; 
            __context__.SourceCodeLine = 170;
            ROOMHUEOUT  .Value = (ushort) ( MYROOM.RoomHue ) ; 
            __context__.SourceCodeLine = 171;
            ROOMSATOUT  .Value = (ushort) ( MYROOM.RoomSat ) ; 
            } 
        
        
        
    }
    finally { ObjectFinallyHandler(); }
    }
    
public void MYONLINEHANDLER ( object __sender__ /*HueLights.HueRoom SENDER */, EventArgs ARGS ) 
    { 
    HueRoom  SENDER  = (HueRoom )__sender__;
    ushort I = 0;
    
    try
    {
        SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
        
        __context__.SourceCodeLine = 178;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SENDER == MYROOM))  ) ) 
            { 
            __context__.SourceCodeLine = 180;
            ROOMONLINE  .Value = (ushort) ( MYROOM.RoomOnline ) ; 
            __context__.SourceCodeLine = 181;
            SCENESNUM  .Value = (ushort) ( MYROOM.SceneNum ) ; 
            __context__.SourceCodeLine = 182;
            RMCLASS  .UpdateValue ( MYROOM . RoomClass  ) ; 
            __context__.SourceCodeLine = 183;
            ushort __FN_FORSTART_VAL__1 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__1 = (ushort)SCENESNUM  .Value; 
            int __FN_FORSTEP_VAL__1 = (int)1; 
            for ( I  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (I  >= __FN_FORSTART_VAL__1) && (I  <= __FN_FOREND_VAL__1) ) : ( (I  <= __FN_FORSTART_VAL__1) && (I  >= __FN_FOREND_VAL__1) ) ; I  += (ushort)__FN_FORSTEP_VAL__1) 
                { 
                __context__.SourceCodeLine = 185;
                ROOMSCENENAME [ I]  .UpdateValue ( MYROOM . SceneName [ I ]  ) ; 
                __context__.SourceCodeLine = 183;
                } 
            
            } 
        
        __context__.SourceCodeLine = 188;
        MYROOM . GetRoom ( ) ; 
        
        
    }
    finally { ObjectFinallyHandler(); }
    }
    
public override object FunctionMain (  object __obj__ ) 
    { 
    try
    {
        SplusExecutionContext __context__ = SplusFunctionMainStartCode();
        
        __context__.SourceCodeLine = 194;
        WaitForInitializationComplete ( ) ; 
        __context__.SourceCodeLine = 195;
        // RegisterEvent( MYROOM , ROOMONOFFUPDATE , MYONOFFHANDLER ) 
        try { g_criticalSection.Enter(); MYROOM .RoomOnOffUpdate  += MYONOFFHANDLER; } finally { g_criticalSection.Leave(); }
        ; 
        __context__.SourceCodeLine = 196;
        // RegisterEvent( MYROOM , ROOMBRIUPDATE , MYBRIHANDLER ) 
        try { g_criticalSection.Enter(); MYROOM .RoomBriUpdate  += MYBRIHANDLER; } finally { g_criticalSection.Leave(); }
        ; 
        __context__.SourceCodeLine = 197;
        // RegisterEvent( MYROOM , ROOMHUEUPDATE , MYHUEHANDLER ) 
        try { g_criticalSection.Enter(); MYROOM .RoomHueUpdate  += MYHUEHANDLER; } finally { g_criticalSection.Leave(); }
        ; 
        __context__.SourceCodeLine = 198;
        // RegisterEvent( MYROOM , ROOMSATUPDATE , MYSATHANDLER ) 
        try { g_criticalSection.Enter(); MYROOM .RoomSatUpdate  += MYSATHANDLER; } finally { g_criticalSection.Leave(); }
        ; 
        __context__.SourceCodeLine = 199;
        // RegisterEvent( MYROOM , ROOMUPDATE , MYROOMHANDLER ) 
        try { g_criticalSection.Enter(); MYROOM .RoomUpdate  += MYROOMHANDLER; } finally { g_criticalSection.Leave(); }
        ; 
        __context__.SourceCodeLine = 200;
        // RegisterEvent( MYROOM , ROOMONLINEUPDATE , MYONLINEHANDLER ) 
        try { g_criticalSection.Enter(); MYROOM .RoomOnlineUpdate  += MYONLINEHANDLER; } finally { g_criticalSection.Leave(); }
        ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler(); }
    return __obj__;
    }
    

public override void LogosSplusInitialize()
{
    _SplusNVRAM = new SplusNVRAM( this );
    
    INIT = new Crestron.Logos.SplusObjects.DigitalInput( INIT__DigitalInput__, this );
    m_DigitalInputList.Add( INIT__DigitalInput__, INIT );
    
    GETINFO = new Crestron.Logos.SplusObjects.DigitalInput( GETINFO__DigitalInput__, this );
    m_DigitalInputList.Add( GETINFO__DigitalInput__, GETINFO );
    
    ROOMON = new Crestron.Logos.SplusObjects.DigitalInput( ROOMON__DigitalInput__, this );
    m_DigitalInputList.Add( ROOMON__DigitalInput__, ROOMON );
    
    ROOMOFF = new Crestron.Logos.SplusObjects.DigitalInput( ROOMOFF__DigitalInput__, this );
    m_DigitalInputList.Add( ROOMOFF__DigitalInput__, ROOMOFF );
    
    COLORLOOPON = new Crestron.Logos.SplusObjects.DigitalInput( COLORLOOPON__DigitalInput__, this );
    m_DigitalInputList.Add( COLORLOOPON__DigitalInput__, COLORLOOPON );
    
    SCENES = new InOutArray<DigitalInput>( 20, this );
    for( uint i = 0; i < 20; i++ )
    {
        SCENES[i+1] = new Crestron.Logos.SplusObjects.DigitalInput( SCENES__DigitalInput__ + i, SCENES__DigitalInput__, this );
        m_DigitalInputList.Add( SCENES__DigitalInput__ + i, SCENES[i+1] );
    }
    
    ROOMONLINE = new Crestron.Logos.SplusObjects.DigitalOutput( ROOMONLINE__DigitalOutput__, this );
    m_DigitalOutputList.Add( ROOMONLINE__DigitalOutput__, ROOMONLINE );
    
    ROOMISON = new Crestron.Logos.SplusObjects.DigitalOutput( ROOMISON__DigitalOutput__, this );
    m_DigitalOutputList.Add( ROOMISON__DigitalOutput__, ROOMISON );
    
    ROOMBRIIN = new Crestron.Logos.SplusObjects.AnalogInput( ROOMBRIIN__AnalogSerialInput__, this );
    m_AnalogInputList.Add( ROOMBRIIN__AnalogSerialInput__, ROOMBRIIN );
    
    ROOMHUEIN = new Crestron.Logos.SplusObjects.AnalogInput( ROOMHUEIN__AnalogSerialInput__, this );
    m_AnalogInputList.Add( ROOMHUEIN__AnalogSerialInput__, ROOMHUEIN );
    
    ROOMSATIN = new Crestron.Logos.SplusObjects.AnalogInput( ROOMSATIN__AnalogSerialInput__, this );
    m_AnalogInputList.Add( ROOMSATIN__AnalogSerialInput__, ROOMSATIN );
    
    XYVAL = new InOutArray<AnalogInput>( 2, this );
    for( uint i = 0; i < 2; i++ )
    {
        XYVAL[i+1] = new Crestron.Logos.SplusObjects.AnalogInput( XYVAL__AnalogSerialInput__ + i, XYVAL__AnalogSerialInput__, this );
        m_AnalogInputList.Add( XYVAL__AnalogSerialInput__ + i, XYVAL[i+1] );
    }
    
    SCENESNUM = new Crestron.Logos.SplusObjects.AnalogOutput( SCENESNUM__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( SCENESNUM__AnalogSerialOutput__, SCENESNUM );
    
    ROOMBRIOUT = new Crestron.Logos.SplusObjects.AnalogOutput( ROOMBRIOUT__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( ROOMBRIOUT__AnalogSerialOutput__, ROOMBRIOUT );
    
    ROOMHUEOUT = new Crestron.Logos.SplusObjects.AnalogOutput( ROOMHUEOUT__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( ROOMHUEOUT__AnalogSerialOutput__, ROOMHUEOUT );
    
    ROOMSATOUT = new Crestron.Logos.SplusObjects.AnalogOutput( ROOMSATOUT__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( ROOMSATOUT__AnalogSerialOutput__, ROOMSATOUT );
    
    ROOMNAMELISTITEM = new Crestron.Logos.SplusObjects.StringInput( ROOMNAMELISTITEM__AnalogSerialInput__, 20, this );
    m_StringInputList.Add( ROOMNAMELISTITEM__AnalogSerialInput__, ROOMNAMELISTITEM );
    
    RMCLASS = new Crestron.Logos.SplusObjects.StringOutput( RMCLASS__AnalogSerialOutput__, this );
    m_StringOutputList.Add( RMCLASS__AnalogSerialOutput__, RMCLASS );
    
    ROOMSCENENAME = new InOutArray<StringOutput>( 20, this );
    for( uint i = 0; i < 20; i++ )
    {
        ROOMSCENENAME[i+1] = new Crestron.Logos.SplusObjects.StringOutput( ROOMSCENENAME__AnalogSerialOutput__ + i, this );
        m_StringOutputList.Add( ROOMSCENENAME__AnalogSerialOutput__ + i, ROOMSCENENAME[i+1] );
    }
    
    ROOMNAME = new StringParameter( ROOMNAME__Parameter__, this );
    m_ParameterList.Add( ROOMNAME__Parameter__, ROOMNAME );
    
    
    GETINFO.OnDigitalPush.Add( new InputChangeHandlerWrapper( GETINFO_OnPush_0, false ) );
    INIT.OnDigitalPush.Add( new InputChangeHandlerWrapper( INIT_OnPush_1, false ) );
    ROOMON.OnDigitalPush.Add( new InputChangeHandlerWrapper( ROOMON_OnPush_2, false ) );
    ROOMOFF.OnDigitalPush.Add( new InputChangeHandlerWrapper( ROOMOFF_OnPush_3, false ) );
    COLORLOOPON.OnDigitalPush.Add( new InputChangeHandlerWrapper( COLORLOOPON_OnPush_4, false ) );
    COLORLOOPON.OnDigitalRelease.Add( new InputChangeHandlerWrapper( COLORLOOPON_OnRelease_5, false ) );
    for( uint i = 0; i < 20; i++ )
        SCENES[i+1].OnDigitalPush.Add( new InputChangeHandlerWrapper( SCENES_OnPush_6, false ) );
        
    ROOMBRIIN.OnAnalogChange.Add( new InputChangeHandlerWrapper( ROOMBRIIN_OnChange_7, true ) );
    ROOMHUEIN.OnAnalogChange.Add( new InputChangeHandlerWrapper( ROOMHUEIN_OnChange_8, true ) );
    ROOMSATIN.OnAnalogChange.Add( new InputChangeHandlerWrapper( ROOMSATIN_OnChange_9, true ) );
    for( uint i = 0; i < 2; i++ )
        XYVAL[i+1].OnAnalogChange.Add( new InputChangeHandlerWrapper( XYVAL_OnChange_10, true ) );
        
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    MYROOM  = new HueLights.HueRoom();
    
    
}

public UserModuleClass_HUELIGHTSROOM_R1_5 ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}




const uint INIT__DigitalInput__ = 0;
const uint GETINFO__DigitalInput__ = 1;
const uint ROOMON__DigitalInput__ = 2;
const uint ROOMOFF__DigitalInput__ = 3;
const uint COLORLOOPON__DigitalInput__ = 4;
const uint ROOMBRIIN__AnalogSerialInput__ = 0;
const uint ROOMHUEIN__AnalogSerialInput__ = 1;
const uint ROOMSATIN__AnalogSerialInput__ = 2;
const uint ROOMNAMELISTITEM__AnalogSerialInput__ = 3;
const uint XYVAL__AnalogSerialInput__ = 4;
const uint SCENES__DigitalInput__ = 5;
const uint ROOMONLINE__DigitalOutput__ = 0;
const uint ROOMISON__DigitalOutput__ = 1;
const uint SCENESNUM__AnalogSerialOutput__ = 0;
const uint ROOMBRIOUT__AnalogSerialOutput__ = 1;
const uint ROOMHUEOUT__AnalogSerialOutput__ = 2;
const uint ROOMSATOUT__AnalogSerialOutput__ = 3;
const uint RMCLASS__AnalogSerialOutput__ = 4;
const uint ROOMSCENENAME__AnalogSerialOutput__ = 5;
const uint ROOMNAME__Parameter__ = 10;

[SplusStructAttribute(-1, true, false)]
public class SplusNVRAM : SplusStructureBase
{

    public SplusNVRAM( SplusObject __caller__ ) : base( __caller__ ) {}
    
    
}

SplusNVRAM _SplusNVRAM = null;

public class __CEvent__ : CEvent
{
    public __CEvent__() {}
    public void Close() { base.Close(); }
    public int Reset() { return base.Reset() ? 1 : 0; }
    public int Set() { return base.Set() ? 1 : 0; }
    public int Wait( int timeOutInMs ) { return base.Wait( timeOutInMs ) ? 1 : 0; }
}
public class __CMutex__ : CMutex
{
    public __CMutex__() {}
    public void Close() { base.Close(); }
    public void ReleaseMutex() { base.ReleaseMutex(); }
    public int WaitForMutex() { return base.WaitForMutex() ? 1 : 0; }
}
 public int IsNull( object obj ){ return (obj == null) ? 1 : 0; }
}


}
