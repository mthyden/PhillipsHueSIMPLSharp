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

namespace UserModule_HUELIGHTSROOM_R1_1
{
    public class UserModuleClass_HUELIGHTSROOM_R1_1 : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        
        Crestron.Logos.SplusObjects.DigitalInput HUEONLINE;
        Crestron.Logos.SplusObjects.DigitalInput GETINFO;
        Crestron.Logos.SplusObjects.DigitalInput ROOMON;
        Crestron.Logos.SplusObjects.DigitalInput ROOMOFF;
        Crestron.Logos.SplusObjects.AnalogInput ROOMBRIIN;
        Crestron.Logos.SplusObjects.AnalogInput ROOMHUEIN;
        Crestron.Logos.SplusObjects.AnalogInput ROOMSATIN;
        InOutArray<Crestron.Logos.SplusObjects.DigitalInput> SCENES;
        Crestron.Logos.SplusObjects.DigitalOutput ROOMONLINE;
        Crestron.Logos.SplusObjects.DigitalOutput ROOMISON;
        Crestron.Logos.SplusObjects.AnalogOutput SCENESNUM;
        Crestron.Logos.SplusObjects.AnalogOutput ROOMBRIOUT;
        Crestron.Logos.SplusObjects.AnalogOutput ROOMHUEOUT;
        Crestron.Logos.SplusObjects.AnalogOutput ROOMSATOUT;
        InOutArray<Crestron.Logos.SplusObjects.StringOutput> ROOMSCENENAME;
        StringParameter ROOMNAME;
        HueLights.HueRoom MYROOM;
        object GETINFO_OnPush_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                ushort I = 0;
                
                
                __context__.SourceCodeLine = 56;
                if ( Functions.TestForTrue  ( ( HUEONLINE  .Value)  ) ) 
                    { 
                    __context__.SourceCodeLine = 58;
                    MYROOM . GroupName  =  ( ROOMNAME  )  .ToString() ; 
                    __context__.SourceCodeLine = 59;
                    MYROOM . GetRoom ( ) ; 
                    __context__.SourceCodeLine = 60;
                    ROOMISON  .Value = (ushort) ( MYROOM.GroupIsOn ) ; 
                    __context__.SourceCodeLine = 61;
                    ROOMBRIOUT  .Value = (ushort) ( MYROOM.RoomBri ) ; 
                    __context__.SourceCodeLine = 62;
                    ROOMHUEOUT  .Value = (ushort) ( MYROOM.RoomHue ) ; 
                    __context__.SourceCodeLine = 63;
                    ROOMSATOUT  .Value = (ushort) ( MYROOM.RoomSat ) ; 
                    __context__.SourceCodeLine = 64;
                    SCENESNUM  .Value = (ushort) ( MYROOM.SceneNum ) ; 
                    __context__.SourceCodeLine = 66;
                    ushort __FN_FORSTART_VAL__1 = (ushort) ( 1 ) ;
                    ushort __FN_FOREND_VAL__1 = (ushort)SCENESNUM  .Value; 
                    int __FN_FORSTEP_VAL__1 = (int)1; 
                    for ( I  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (I  >= __FN_FORSTART_VAL__1) && (I  <= __FN_FOREND_VAL__1) ) : ( (I  <= __FN_FORSTART_VAL__1) && (I  >= __FN_FOREND_VAL__1) ) ; I  += (ushort)__FN_FORSTEP_VAL__1) 
                        { 
                        __context__.SourceCodeLine = 68;
                        ROOMSCENENAME [ I]  .UpdateValue ( MYROOM . SceneName [ I ]  ) ; 
                        __context__.SourceCodeLine = 66;
                        } 
                    
                    } 
                
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    object ROOMON_OnPush_1 ( Object __EventInfo__ )
    
        { 
        Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
        try
        {
            SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
            
            __context__.SourceCodeLine = 75;
            MYROOM . GroupOn ( ) ; 
            __context__.SourceCodeLine = 76;
            ROOMISON  .Value = (ushort) ( MYROOM.GroupIsOn ) ; 
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler( __SignalEventArg__ ); }
        return this;
        
    }
    
object ROOMOFF_OnPush_2 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 81;
        MYROOM . GroupOff ( ) ; 
        __context__.SourceCodeLine = 82;
        ROOMISON  .Value = (ushort) ( MYROOM.GroupIsOn ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object SCENES_OnPush_3 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort I = 0;
        
        
        __context__.SourceCodeLine = 88;
        I = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
        __context__.SourceCodeLine = 89;
        MYROOM . RecallScene ( (ushort)( I )) ; 
        __context__.SourceCodeLine = 90;
        ROOMISON  .Value = (ushort) ( 1 ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object ROOMBRIIN_OnChange_4 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 95;
        MYROOM . LightsVal ( "groups", "bri", (ushort)( ROOMBRIIN  .UshortValue )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object ROOMHUEIN_OnChange_5 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 100;
        MYROOM . LightsVal ( "groups", "hue", (ushort)( ROOMHUEIN  .UshortValue )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object ROOMSATIN_OnChange_6 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 105;
        MYROOM . LightsVal ( "groups", "sat", (ushort)( ROOMSATIN  .UshortValue )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

public void MYBRIHANDLER ( object __sender__ /*HueLights.HueRoom SENDER */, EventArgs ARGS ) 
    { 
    HueRoom  SENDER  = (HueRoom )__sender__;
    try
    {
        SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
        
        __context__.SourceCodeLine = 110;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SENDER == MYROOM))  ) ) 
            { 
            __context__.SourceCodeLine = 112;
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
        
        __context__.SourceCodeLine = 118;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SENDER == MYROOM))  ) ) 
            { 
            __context__.SourceCodeLine = 120;
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
        
        __context__.SourceCodeLine = 126;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SENDER == MYROOM))  ) ) 
            { 
            __context__.SourceCodeLine = 128;
            ROOMSATOUT  .Value = (ushort) ( MYROOM.RoomSat ) ; 
            } 
        
        
        
    }
    finally { ObjectFinallyHandler(); }
    }
    
public void MYONLINEHANDLER ( object __sender__ /*HueLights.HueRoom SENDER */, EventArgs ARGS ) 
    { 
    HueRoom  SENDER  = (HueRoom )__sender__;
    try
    {
        SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
        
        __context__.SourceCodeLine = 134;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SENDER == MYROOM))  ) ) 
            { 
            __context__.SourceCodeLine = 136;
            ROOMONLINE  .Value = (ushort) ( MYROOM.RoomOnline ) ; 
            } 
        
        
        
    }
    finally { ObjectFinallyHandler(); }
    }
    
public override object FunctionMain (  object __obj__ ) 
    { 
    try
    {
        SplusExecutionContext __context__ = SplusFunctionMainStartCode();
        
        __context__.SourceCodeLine = 143;
        WaitForInitializationComplete ( ) ; 
        __context__.SourceCodeLine = 144;
        // RegisterEvent( MYROOM , ROOMBRIUPDATE , MYBRIHANDLER ) 
        try { g_criticalSection.Enter(); MYROOM .RoomBriUpdate  += MYBRIHANDLER; } finally { g_criticalSection.Leave(); }
        ; 
        __context__.SourceCodeLine = 145;
        // RegisterEvent( MYROOM , ROOMHUEUPDATE , MYHUEHANDLER ) 
        try { g_criticalSection.Enter(); MYROOM .RoomHueUpdate  += MYHUEHANDLER; } finally { g_criticalSection.Leave(); }
        ; 
        __context__.SourceCodeLine = 146;
        // RegisterEvent( MYROOM , ROOMSATUPDATE , MYSATHANDLER ) 
        try { g_criticalSection.Enter(); MYROOM .RoomSatUpdate  += MYSATHANDLER; } finally { g_criticalSection.Leave(); }
        ; 
        __context__.SourceCodeLine = 147;
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
    
    HUEONLINE = new Crestron.Logos.SplusObjects.DigitalInput( HUEONLINE__DigitalInput__, this );
    m_DigitalInputList.Add( HUEONLINE__DigitalInput__, HUEONLINE );
    
    GETINFO = new Crestron.Logos.SplusObjects.DigitalInput( GETINFO__DigitalInput__, this );
    m_DigitalInputList.Add( GETINFO__DigitalInput__, GETINFO );
    
    ROOMON = new Crestron.Logos.SplusObjects.DigitalInput( ROOMON__DigitalInput__, this );
    m_DigitalInputList.Add( ROOMON__DigitalInput__, ROOMON );
    
    ROOMOFF = new Crestron.Logos.SplusObjects.DigitalInput( ROOMOFF__DigitalInput__, this );
    m_DigitalInputList.Add( ROOMOFF__DigitalInput__, ROOMOFF );
    
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
    
    SCENESNUM = new Crestron.Logos.SplusObjects.AnalogOutput( SCENESNUM__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( SCENESNUM__AnalogSerialOutput__, SCENESNUM );
    
    ROOMBRIOUT = new Crestron.Logos.SplusObjects.AnalogOutput( ROOMBRIOUT__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( ROOMBRIOUT__AnalogSerialOutput__, ROOMBRIOUT );
    
    ROOMHUEOUT = new Crestron.Logos.SplusObjects.AnalogOutput( ROOMHUEOUT__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( ROOMHUEOUT__AnalogSerialOutput__, ROOMHUEOUT );
    
    ROOMSATOUT = new Crestron.Logos.SplusObjects.AnalogOutput( ROOMSATOUT__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( ROOMSATOUT__AnalogSerialOutput__, ROOMSATOUT );
    
    ROOMSCENENAME = new InOutArray<StringOutput>( 20, this );
    for( uint i = 0; i < 20; i++ )
    {
        ROOMSCENENAME[i+1] = new Crestron.Logos.SplusObjects.StringOutput( ROOMSCENENAME__AnalogSerialOutput__ + i, this );
        m_StringOutputList.Add( ROOMSCENENAME__AnalogSerialOutput__ + i, ROOMSCENENAME[i+1] );
    }
    
    ROOMNAME = new StringParameter( ROOMNAME__Parameter__, this );
    m_ParameterList.Add( ROOMNAME__Parameter__, ROOMNAME );
    
    
    GETINFO.OnDigitalPush.Add( new InputChangeHandlerWrapper( GETINFO_OnPush_0, false ) );
    ROOMON.OnDigitalPush.Add( new InputChangeHandlerWrapper( ROOMON_OnPush_1, false ) );
    ROOMOFF.OnDigitalPush.Add( new InputChangeHandlerWrapper( ROOMOFF_OnPush_2, false ) );
    for( uint i = 0; i < 20; i++ )
        SCENES[i+1].OnDigitalPush.Add( new InputChangeHandlerWrapper( SCENES_OnPush_3, false ) );
        
    ROOMBRIIN.OnAnalogChange.Add( new InputChangeHandlerWrapper( ROOMBRIIN_OnChange_4, true ) );
    ROOMHUEIN.OnAnalogChange.Add( new InputChangeHandlerWrapper( ROOMHUEIN_OnChange_5, true ) );
    ROOMSATIN.OnAnalogChange.Add( new InputChangeHandlerWrapper( ROOMSATIN_OnChange_6, true ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    MYROOM  = new HueLights.HueRoom();
    
    
}

public UserModuleClass_HUELIGHTSROOM_R1_1 ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}




const uint HUEONLINE__DigitalInput__ = 0;
const uint GETINFO__DigitalInput__ = 1;
const uint ROOMON__DigitalInput__ = 2;
const uint ROOMOFF__DigitalInput__ = 3;
const uint ROOMBRIIN__AnalogSerialInput__ = 0;
const uint ROOMHUEIN__AnalogSerialInput__ = 1;
const uint ROOMSATIN__AnalogSerialInput__ = 2;
const uint SCENES__DigitalInput__ = 4;
const uint ROOMONLINE__DigitalOutput__ = 0;
const uint ROOMISON__DigitalOutput__ = 1;
const uint SCENESNUM__AnalogSerialOutput__ = 0;
const uint ROOMBRIOUT__AnalogSerialOutput__ = 1;
const uint ROOMHUEOUT__AnalogSerialOutput__ = 2;
const uint ROOMSATOUT__AnalogSerialOutput__ = 3;
const uint ROOMSCENENAME__AnalogSerialOutput__ = 4;
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
