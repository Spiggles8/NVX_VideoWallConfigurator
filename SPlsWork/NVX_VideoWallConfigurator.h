namespace NVX_VideoWallConfigurator;
        // class declarations
         class Panel;
         class VideoWall;
         class PanelCoordinate;
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
        INTEGER CurrentWidth;
        INTEGER CurrentHeight;
        INTEGER CurrentX;
        INTEGER CurrentY;
        SIGNED_LONG_INTEGER PanelLayout;
    };

     class VideoWall 
    {
        // class delegates

        // class events

        // class functions
        INTEGER_FUNCTION GetIsHigh ( SIGNED_LONG_INTEGER x , SIGNED_LONG_INTEGER y );
        FUNCTION RouteSourceToHighPanels ( INTEGER sourceNumber );
        INTEGER_FUNCTION GetPanelSource ( SIGNED_LONG_INTEGER x , SIGNED_LONG_INTEGER y );
        SIGNED_LONG_INTEGER_FUNCTION GetPanelLayout ( SIGNED_LONG_INTEGER x , SIGNED_LONG_INTEGER y );
        FUNCTION SetPanel ( SIGNED_LONG_INTEGER number );
        FUNCTION CalculateHighPanelLayout ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();
        STRING_FUNCTION ToString ();

        // class variables
        INTEGER __class_id__;

        // class properties
    };

