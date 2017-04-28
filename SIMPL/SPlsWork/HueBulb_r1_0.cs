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

namespace UserModule_HUEBULB_R1_0
{
    public class UserModuleClass_HUEBULB_R1_0 : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        Crestron.Logos.SplusObjects.DigitalInput GETINFO;
        Crestron.Logos.SplusObjects.DigitalInput LIGHTON;
        Crestron.Logos.SplusObjects.DigitalInput LIGHTOFF;
        Crestron.Logos.SplusObjects.AnalogInput BRIIN;
        Crestron.Logos.SplusObjects.AnalogInput HUEIN;
        Crestron.Logos.SplusObjects.AnalogInput SATIN;
        Crestron.Logos.SplusObjects.DigitalOutput LOADISON;
        Crestron.Logos.SplusObjects.DigitalOutput LOADREACHABLE;
        Crestron.Logos.SplusObjects.DigitalOutput LOADISCOLOR;
        Crestron.Logos.SplusObjects.StringOutput LIGHTNAME;
        Crestron.Logos.SplusObjects.StringOutput LIGHTTYPE;
        Crestron.Logos.SplusObjects.AnalogOutput BRIOUT;
        Crestron.Logos.SplusObjects.AnalogOutput HUEOUT;
        Crestron.Logos.SplusObjects.AnalogOutput SATOUT;
        UShortParameter BULBID;
        HueLights.HueLight MYLIGHT;
        object GETINFO_OnPush_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                
                __context__.SourceCodeLine = 33;
                MYLIGHT . BulbID = (ushort) ( BULBID  .Value ) ; 
                __context__.SourceCodeLine = 34;
                MYLIGHT . GetBulb ( ) ; 
                __context__.SourceCodeLine = 35;
                LIGHTNAME  .UpdateValue ( MYLIGHT . BulbName  ) ; 
                __context__.SourceCodeLine = 36;
                LOADISON  .Value = (ushort) ( MYLIGHT.BulbIsOn ) ; 
                __context__.SourceCodeLine = 37;
                LIGHTTYPE  .UpdateValue ( MYLIGHT . BulbType  ) ; 
                __context__.SourceCodeLine = 38;
                BRIOUT  .Value = (ushort) ( MYLIGHT.BulbBri ) ; 
                __context__.SourceCodeLine = 39;
                LOADREACHABLE  .Value = (ushort) ( MYLIGHT.Reachable ) ; 
                __context__.SourceCodeLine = 40;
                HUEOUT  .Value = (ushort) ( MYLIGHT.BulbHue ) ; 
                __context__.SourceCodeLine = 41;
                SATOUT  .Value = (ushort) ( MYLIGHT.BulbSat ) ; 
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    object LIGHTON_OnPush_1 ( Object __EventInfo__ )
    
        { 
        Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
        try
        {
            SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
            
            __context__.SourceCodeLine = 46;
            MYLIGHT . LightsOn ( ) ; 
            __context__.SourceCodeLine = 47;
            LOADISON  .Value = (ushort) ( MYLIGHT.BulbIsOn ) ; 
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler( __SignalEventArg__ ); }
        return this;
        
    }
    
object LIGHTOFF_OnPush_2 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 52;
        MYLIGHT . LightsOff ( ) ; 
        __context__.SourceCodeLine = 53;
        LOADISON  .Value = (ushort) ( MYLIGHT.BulbIsOn ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BRIIN_OnChange_3 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 58;
        MYLIGHT . LightsVal ( "lights", "bri", (ushort)( BRIIN  .UshortValue )) ; 
        __context__.SourceCodeLine = 59;
        BRIOUT  .Value = (ushort) ( MYLIGHT.BulbBri ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object HUEIN_OnChange_4 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 64;
        MYLIGHT . LightsVal ( "lights", "hue", (ushort)( HUEIN  .UshortValue )) ; 
        __context__.SourceCodeLine = 65;
        HUEOUT  .Value = (ushort) ( MYLIGHT.BulbHue ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object SATIN_OnChange_5 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 70;
        MYLIGHT . LightsVal ( "lights", "sat", (ushort)( SATIN  .UshortValue )) ; 
        __context__.SourceCodeLine = 71;
        SATOUT  .Value = (ushort) ( MYLIGHT.BulbSat ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}


public override void LogosSplusInitialize()
{
    _SplusNVRAM = new SplusNVRAM( this );
    
    GETINFO = new Crestron.Logos.SplusObjects.DigitalInput( GETINFO__DigitalInput__, this );
    m_DigitalInputList.Add( GETINFO__DigitalInput__, GETINFO );
    
    LIGHTON = new Crestron.Logos.SplusObjects.DigitalInput( LIGHTON__DigitalInput__, this );
    m_DigitalInputList.Add( LIGHTON__DigitalInput__, LIGHTON );
    
    LIGHTOFF = new Crestron.Logos.SplusObjects.DigitalInput( LIGHTOFF__DigitalInput__, this );
    m_DigitalInputList.Add( LIGHTOFF__DigitalInput__, LIGHTOFF );
    
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
    
    LIGHTNAME = new Crestron.Logos.SplusObjects.StringOutput( LIGHTNAME__AnalogSerialOutput__, this );
    m_StringOutputList.Add( LIGHTNAME__AnalogSerialOutput__, LIGHTNAME );
    
    LIGHTTYPE = new Crestron.Logos.SplusObjects.StringOutput( LIGHTTYPE__AnalogSerialOutput__, this );
    m_StringOutputList.Add( LIGHTTYPE__AnalogSerialOutput__, LIGHTTYPE );
    
    BULBID = new UShortParameter( BULBID__Parameter__, this );
    m_ParameterList.Add( BULBID__Parameter__, BULBID );
    
    
    GETINFO.OnDigitalPush.Add( new InputChangeHandlerWrapper( GETINFO_OnPush_0, false ) );
    LIGHTON.OnDigitalPush.Add( new InputChangeHandlerWrapper( LIGHTON_OnPush_1, false ) );
    LIGHTOFF.OnDigitalPush.Add( new InputChangeHandlerWrapper( LIGHTOFF_OnPush_2, false ) );
    BRIIN.OnAnalogChange.Add( new InputChangeHandlerWrapper( BRIIN_OnChange_3, true ) );
    HUEIN.OnAnalogChange.Add( new InputChangeHandlerWrapper( HUEIN_OnChange_4, true ) );
    SATIN.OnAnalogChange.Add( new InputChangeHandlerWrapper( SATIN_OnChange_5, true ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    MYLIGHT  = new HueLights.HueLight();
    
    
}

public UserModuleClass_HUEBULB_R1_0 ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}




const uint GETINFO__DigitalInput__ = 0;
const uint LIGHTON__DigitalInput__ = 1;
const uint LIGHTOFF__DigitalInput__ = 2;
const uint BRIIN__AnalogSerialInput__ = 0;
const uint HUEIN__AnalogSerialInput__ = 1;
const uint SATIN__AnalogSerialInput__ = 2;
const uint LOADISON__DigitalOutput__ = 0;
const uint LOADREACHABLE__DigitalOutput__ = 1;
const uint LOADISCOLOR__DigitalOutput__ = 2;
const uint LIGHTNAME__AnalogSerialOutput__ = 0;
const uint LIGHTTYPE__AnalogSerialOutput__ = 1;
const uint BRIOUT__AnalogSerialOutput__ = 2;
const uint HUEOUT__AnalogSerialOutput__ = 3;
const uint SATOUT__AnalogSerialOutput__ = 4;
const uint BULBID__Parameter__ = 10;

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
