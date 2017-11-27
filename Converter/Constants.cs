namespace MbJsonToYaml
{
    class Constants
    {
        // tangram play can't read these, they are gzipped.
        public const string Osm2VectorUrl = "http://osm2vectortiles-3.tileserver.com/v2/{z}/{x}/{y}.pbf";
        public const string MapboxUrl = "https://a.tiles.mapbox.com/v4/mapbox.mapbox-streets-v7/{z}/{x}/{y}.vector.pbf?access_token=pk.eyJ1IjoiYmNhbXBlciIsImEiOiJWUmh3anY0In0.1fgSTNWpQV8-5sBjGbBzGg";

        public const string MapboxPolygons = "fill";
        public const string TangramPolygons = "polygons";
        public const string MapboxLines = "line";
        public const string TangramLines = "lines";
        public const string MapboxSymbols = "symbol";
        public const string TangramIcons = "icons";
        public const string TangramText = "text";

        public const string Background = "background";

        public const string CasingSuffix = "_casing";
        public const string CaseSuffix = "-case";

        public const string Motorway = "motorway";
        public const string Trunk = "trunk";
        public const string Primary = "primary";
        public const string Secondary = "secondary";
        public const string Tertiary = "tertiary";
        public const string Bridge = "bridge";
        public const string Link = "link";
        public const string Shield = "shield";

        public const string Road = "road";
        public const string Water = "water";
        public const string Building = "building";
        public const string Ferry = "ferry";

        public const string CustomLinesDash = "lines-dash";
        public const string RailLinesDash = "rail-dash";

        public static readonly string[] IgnoredLayers = { Background, "hillshade_" };
    }
}
