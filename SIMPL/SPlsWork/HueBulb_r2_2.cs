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

namespace UserModule_HUEBULB_R2_2
{
    public class UserModuleClass_HUEBULB_R2_2 : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        Crestron.Logos.SplusObjects.DigitalInput INIT;
        Crestron.Logos.SplusObjects.DigitalInput GETINFO;
        Crestron.Logos.SplusObjects.DigitalInput LIGHTON;
        Crestron.Logos.SplusObjects.DigitalInput LIGHTOFF;
        Crestron.Logos.SplusObjects.AnalogInput BRIIN;
        Crestron.Logos.SplusObjects.AnalogInput HUEIN;
        Crestron.Logos.SplusObjects.AnalogInput SATIN;
        Crestron.Logos.SplusObjects.StringInput LIGHTNAMELISTITEM;
        Crestron.Logos.SplusObjects.DigitalOutput LOADONLINE;
        Crestron.Logos.SplusObjects.DigitalOutput LOADISON;
        Crestron.Logos.SplusObjects.DigitalOutput LOADREACHABLE;
        Crestron.Logos.SplusObjects.DigitalOutput LOADISCOLOR;
        Crestron.Logos.SplusObjects.StringOutput LIGHTTYPE;
        Crestron.Logos.SplusObjects.AnalogOutput BRIOUT;
        Crestron.Logos.SplusObjects.AnalogOutput HUEOUT;
        Crestron.Logos.SplusObjects.AnalogOutput SATOUT;
        StringParameter LIGHTNAME;
        HueLights.HueLight MYLIGHT;
        object GETINFO_OnPush_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                
                __context__.SourceCodeLine = 34;
                MYLIGHT . GetBulb ( ) ; 
                
                
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
            
            __context__.SourceCodeLine = 39;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( Functions.Length( LIGHTNAMELISTITEM ) > 0 ))  ) ) 
                { 
                __context__.SourceCodeLine = 41;
                MYLIGHT . BulbName  =  ( LIGHTNAMELISTITEM  )  .ToString() ; 
                } 
            
            else 
                { 
                __context__.SourceCodeLine = 45;
                MYLIGHT . BulbName  =  ( LIGHTNAME  )  .ToString() ; 
                } 
            
            __context__.SourceCodeLine = 47;
            MYLIGHT . BulbInit ( ) ; 
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler( __SignalEventArg__ ); }
        return this;
        
    }
    
object LIGHTON_OnPush_2 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 53;
        MYLIGHT . LightsAction ( "on", "true", "none") ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object LIGHTOFF_OnPush_3 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 58;
        MYLIGHT . LightsAction ( "on", "false", "none") ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BRIIN_OnChange_4 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 64;
        MYLIGHT . LightsVal ( "bri", (ushort)( BRIIN  .UshortValue )) ; 
        __context__.SourceCodeLine = 65;
        BRIOUT  .Value = (ushort) ( MYLIGHT.BulbBri ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object HUEIN_OnChange_5 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 70;
        MYLIGHT . LightsVal ( "hue", (ushort)( HUEIN  .UshortValue )) ; 
        __context__.SourceCodeLine = 71;
        HUEOUT  .Value = (ushort) ( MYLIGHT.BulbHue ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object SATIN_OnChange_6 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 76;
        MYLIGHT . LightsVal ( "sat", (ushort)( SATIN  .UshortValue )) ; 
        __context__.SourceCodeLine = 77;
        SATOUT  .Value = (ushort) ( MYLIGHT.BulbSat ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

public void MYONOFFHANDLER ( object __sender__ /*HueLights.HueLight SENDER */, EventArgs ARGS ) 
    { 
    HueLight  SENDER  = (HueLight )__sender__;
    try
    {
        SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
        
        __context__.SourceCodeLine = 82;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SENDER == MYLIGHT))  ) ) 
            { 
            __context__.SourceCodeLine = 84;
            LOADISON  .Value = (ushort) ( MYLIGHT.BulbIsOn ) ; 
            } 
        
        
        
    }
    finally { ObjectFinallyHandler(); }
    }
    
public void MYBRIHANDLER ( object __sender__ /*HueLights.HueLight SENDER */, EventArgs ARGS ) 
    { 
    HueLight  SENDER  = (HueLight )__sender__;
    try
    {
        SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
        
        __context__.SourceCodeLine = 90;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SENDER == MYLIGHT))  ) ) 
            { 
            __context__.SourceCodeLine = 92;
            BRIOUT  .Value = (ushort) ( MYLIGHT.BulbBri ) ; 
            } 
        
        
        
    }
    finally { ObjectFinallyHandler(); }
    }
    
public void MYHUEHANDLER ( object __sender__ /*HueLights.HueLight SENDER */, EventArgs ARGS ) 
    { 
    HueLight  SENDER  = (HueLight )__sender__;
    try
    {
        SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
        
        __context__.SourceCodeLine = 98;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SENDER == MYLIGHT))  ) ) 
            { 
            __context__.SourceCodeLine = 100;
            HUEOUT  .Value = (ushort) ( MYLIGHT.BulbHue ) ; 
            } 
        
        
        
    }
    finally { ObjectFinallyHandler(); }
    }
    
public void MYSATHANDLER ( object __sender__ /*HueLights.HueLight SENDER */, EventArgs ARGS ) 
    { 
    HueLight  SENDER  = (HueLight )__sender__;
    try
    {
        SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
        
        __context__.SourceCodeLine = 106;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SENDER == MYLIGHT))  ) ) 
            { 
            __context__.SourceCodeLine = 108;
            SATOUT  .Value = (ushort) ( MYLIGHT.BulbSat ) ; 
            } 
        
        
        
    }
    finally { ObjectFinallyHandler(); }
    }
    
public void MYLIGHTHANDLER ( object __sender__ /*HueLights.HueLight SENDER */, EventArgs ARGS ) 
    { 
    HueLight  SENDER  = (HueLight )__sender__;
    try
    {
        SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
        
        __context__.SourceCodeLine = 114;
        if ( Functions.TestForTrue  ( ( MYLIGHT.Reachable)  ) ) 
            { 
            __context__.SourceCodeLine = 116;
            LOADONLINE  .Value = (ushort) ( MYLIGHT.BulbOnline ) ; 
            __context__.SourceCodeLine = 117;
            LOADISON  .Value = (ushort) ( MYLIGHT.BulbIsOn ) ; 
            __context__.SourceCodeLine = 118;
            LIGHTTYPE  .UpdateValue ( MYLIGHT . BulbType  ) ; 
            __context__.SourceCodeLine = 119;
            BRIOUT  .Value = (ushort) ( MYLIGHT.BulbBri ) ; 
            __context__.SourceCodeLine = 120;
            LOADREACHABLE  .Value = (ushort) ( MYLIGHT.Reachable ) ; 
            __context__.SourceCodeLine = 121;
            HUEOUT  .Value = (ushort) ( MYLIGHT.BulbHue ) ; 
            __context__.SourceCodeLine = 122;
            SATOUT  .Value = (ushort) ( MYLIGHT.BulbSat ) ; 
            } 
        
        
        
    }
    finally { ObjectFinallyHandler(); }
    }
    
public void MYONLINEHANDLER ( object __sender__ /*HueLights.HueLight SENDER */, EventArgs ARGS ) 
    { 
    HueLight  SENDER  = (HueLight )__sender__;
    try
    {
        SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
        
        __context__.SourceCodeLine = 128;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SENDER == MYLIGHT))  ) ) 
            { 
            __context__.SourceCodeLine = 130;
            LOADONLINE  .Value = (ushort) ( MYLIGHT.BulbOnline ) ; 
            __context__.SourceCodeLine = 131;
            LIGHTTYPE  .UpdateValue ( MYLIGHT . BulbType  ) ; 
            __context__.SourceCodeLine = 132;
            LOADREACHABLE  .Value = (ushort) ( MYLIGHT.Reachable ) ; 
            __context__.SourceCodeLine = 133;
            MYLIGHT . GetBulb ( ) ; 
            } 
        
        
        
    }
    finally { ObjectFinallyHandler(); }
    }
    
public override object FunctionMain (  object __obj__ ) 
    { 
    try
    {
        SplusExecutionContext __context__ = SplusFunctionMainStartCode();
        
        __context__.SourceCodeLine = 139;
        WaitForInitializationComplete ( ) ; 
        __context__.SourceCodeLine = 140;
        // RegisterEvent( MYLIGHT , BULBONOFFUPDATE , MYONOFFHANDLER ) 
        try { g_criticalSection.Enter(); MYLIGHT .BulbOnOffUpdate  += MYONOFFHANDLER; } finally { g_criticalSection.Leave(); }
        ; 
        __context__.SourceCodeLine = 141;
        // RegisterEvent( MYLIGHT , BULBBRIUPDATE , MYBRIHANDLER ) 
        try { g_criticalSection.Enter(); MYLIGHT .BulbBriUpdate  += MYBRIHANDLER; } finally { g_criticalSection.Leave(); }
        ; 
        __context__.SourceCodeLine = 142;
        // RegisterEvent( MYLIGHT , BULBHUEUPDATE , MYHUEHANDLER ) 
        try { g_criticalSection.Enter(); MYLIGHT .BulbHueUpdate  += MYHUEHANDLER; } finally { g_criticalSection.Leave(); }
        ; 
        __context__.SourceCodeLine = 143;
        // RegisterEvent( MYLIGHT , BULBSATUPDATE , MYSATHANDLER ) 
        try { g_criticalSection.Enter(); MYLIGHT .BulbSatUpdate  += MYSATHANDLER; } finally { g_criticalSection.Leave(); }
        ; 
        __context__.SourceCodeLine = 144;
        // RegisterEvent( MYLIGHT , BULBONLINEUPDATE , MYONLINEHANDLER ) 
        try { g_criticalSection.Enter(); MYLIGHT .BulbOnlineUpdate  += MYONLINEHANDLER; } finally { g_criticalSection.Leave(); }
        ; 
        __context__.SourceCodeLine = 145;
        // RegisterEvent( MYLIGHT , BULBUPDATE , MYLIGHTHANDLER ) 
        try { g_criticalSection.Enter(); MYLIGHT .BulbUpdate  += MYLIGHTHANDLER; } finally { g_criticalSection.Leave(); }
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
    
    LIGHTON = new Crestron.Logos.SplusObjects.DigitalInput( LIGHTON__DigitalInput__, this );
    m_DigitalInputList.Add( LIGHTON__DigitalInput__, LIGHTON );
    
    LIGHTOFF = new Crestron.Logos.SplusObjects.DigitalInput( LIGHTOFF__DigitalInput__, this );
    m_DigitalInputList.Add( LIGHTOFF__DigitalInput__, LIGHTOFF );
    
    LOADONLINE = new Crestron.Logos.SplusObjects.DigitalOutput( LOADONLINE__DigitalOutput__, this );
    m_DigitalOutputList.Add( LOADONLINE__DigitalOutput__, LOADONLINE );
    
    LOADISON = new Crestron.Logos.SplusObjects.DigitalOutput( LOADISON__DigitalOutput__, this );
    m_DigitalOutputList.Add( LOADISON__DigitalOutput__, LOADISON );
    
    LOADREACHABLE = new Crestron.Logos.SplusObjects.DigitalOutput( LOADREACHABLE__DigitalOutput__, this );
    m_DigitalOutputList.Add( LOADREACHABLE__DigitalOutput__, LOADREACHABLE );
    
    LOADISCOLOR = new Crestron.Logos.SplusObjects.DigitalOutput( LOADISCOLOR__DigitalOutput__, this );
    m_DigitalOutputList.Add( LOADISCOLOR__DigitalOutput__, LOADISCOLOR );
    
    BRIIN = new Crestron.Logos.SplusObjects.AnalogInput( BRIIN__AnalogSerialInput__, this );
    m_AnalogInputList.Add( BRIIN__AnalogSerialInput__, BRIIN );
    
    HUEIN = new Crestron.Logos.SplusObjects.AnalogInput( HUEIN__AnalogSerialInput__, this );
    m_AnalogInputList.Add( HUEIN__AnalogSerialInput__, HUEIN );
    
    SATIN = new Crestron.Logos.SplusObjects.AnalogInput( SATIN__AnalogSerialInput__, this );
    m_AnalogInputList.Add( SATIN__AnalogSerialInput__, SATIN );
    
    BRIOUT = new Crestron.Logos.SplusObjects.AnalogOutput( BRIOUT__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( BRIOUT__AnalogSerialOutput__, BRIOUT );
    
    HUEOUT = new Crestron.Logos.SplusObjects.AnalogOutput( HUEOUT__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( HUEOUT__AnalogSerialOutput__, HUEOUT );
    
    SATOUT = new Crestron.Logos.SplusObjects.AnalogOutput( SATOUT__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( SATOUT__AnalogSerialOutput__, SATOUT );
    
    LIGHTNAMELISTITEM = new Crestron.Logos.SplusObjects.StringInput( LIGHTNAMELISTITEM__AnalogSerialInput__, 50, this );
    m_StringInputList.Add( LIGHTNAMELISTITEM__AnalogSerialInput__, LIGHTNAMELISTITEM );
    
    LIGHTTYPE = new Crestron.Logos.SplusObjects.StringOutput( LIGHTTYPE__AnalogSerialOutput__, this );
    m_StringOutputList.Add( LIGHTTYPE__AnalogSerialOutput__, LIGHTTYPE );
    
    LIGHTNAME = new StringParameter( LIGHTNAME__Parameter__, this );
    m_ParameterList.Add( LIGHTNAME__Parameter__, LIGHTNAME );
    
    
    GETINFO.OnDigitalPush.Add( new InputChangeHandlerWrapper( GETINFO_OnPush_0, false ) );
    INIT.OnDigitalPush.Add( new InputChangeHandlerWrapper( INIT_OnPush_1, false ) );
    LIGHTON.OnDigitalPush.Add( new InputChangeHandlerWrapper( LIGHTON_OnPush_2, false ) );
    LIGHTOFF.OnDigitalPush.Add( new InputChangeHandlerWrapper( LIGHTOFF_OnPush_3, false ) );
    BRIIN.OnAnalogChange.Add( new InputChangeHandlerWrapper( BRIIN_OnChange_4, true ) );
    HUEIN.OnAnalogChange.Add( new InputChangeHandlerWrapper( HUEIN_OnChange_5, true ) );
    SATIN.OnAnalogChange.Add( new InputChangeHandlerWrapper( SATIN_OnChange_6, true ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    MYLIGHT  = new HueLights.HueLight();
    
    
}

public UserModuleClass_HUEBULB_R2_2 ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}




const uint INIT__DigitalInput__ = 0;
const uint GETINFO__DigitalInput__ = 1;
const uint LIGHTON__DigitalInput__ = 2;
const uint LIGHTOFF__DigitalInput__ = 3;
const uint BRIIN__AnalogSerialInput__ = 0;
const uint HUEIN__AnalogSerialInput__ = 1;
const uint SATIN__AnalogSerialInput__ = 2;
const uint LIGHTNAMELISTITEM__AnalogSerialInput__ = 3;
const uint LOADONLINE__DigitalOutput__ = 0;
const uint LOADISON__DigitalOutput__ = 1;
const uint LOADREACHABLE__DigitalOutput__ = 2;
const uint LOADISCOLOR__DigitalOutput__ = 3;
const uint LIGHTTYPE__AnalogSerialOutput__ = 0;
const uint BRIOUT__AnalogSerialOutput__ = 1;
const uint HUEOUT__AnalogSerialOutput__ = 2;
const uint SATOUT__AnalogSerialOutput__ = 3;
const uint LIGHTNAME__Parameter__ = 10;

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
