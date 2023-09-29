namespace NVX_VideoWallConfigurator;
        // class declarations
         class Panel;
         class VideoWall;
         class HighPanelPosition;
         class PanelCoordinate;
         class HighPanelInfo;
     class Panel 
    {
        // class delegates

        // class events

        // class functions
        FUNCTION SetSource ( INTEGER source );
        FUNCTION SetHigh ();
        FUNCTION SetLow ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();
        STRING_FUNCTION ToString ();

        // class variables
        INTEGER __class_id__;

        // class properties
        INTEGER Source;
        INTEGER IsHigh;
    };

     class VideoWall 
    {
        // class delegates

        // class events

        // class functions
        FUNCTION GetCurrentHighPanelLayout ( BYREF SIGNED_LONG_INTEGER currentHeight , BYREF SIGNED_LONG_INTEGER currentWidth );
        FUNCTION VideoInputButtonPressed ( SIGNED_LONG_INTEGER videoInput );
        INTEGER_FUNCTION GetIsHigh ( SIGNED_LONG_INTEGER x , SIGNED_LONG_INTEGER y );
        FUNCTION RouteSourceToHighPanels ( INTEGER sourceNumber );
        INTEGER_FUNCTION GetPanelSource ( SIGNED_LONG_INTEGER x , SIGNED_LONG_INTEGER y );
        FUNCTION SetPanel ( SIGNED_LONG_INTEGER number );
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();
        STRING_FUNCTION ToString ();

        // class variables
        INTEGER __class_id__;

        // class properties
    };

     class HighPanelInfo 
    {
        // class delegates

        // class events

        // class functions
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();
        STRING_FUNCTION ToString ();

        // class variables
        INTEGER __class_id__;

        // class properties
        SIGNED_LONG_INTEGER W;
        SIGNED_LONG_INTEGER H;
        SIGNED_LONG_INTEGER X;
        SIGNED_LONG_INTEGER Y;
    };

