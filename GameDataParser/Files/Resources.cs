﻿using Maple2.File.IO;
using Maple2Storage.Types;

namespace GameDataParser.Files;

public class MetadataResources
{
    public readonly M2dReader XmlReader;
    public readonly M2dReader ExportedReader;
    public readonly M2dReader TokReader;

    public MetadataResources()
    {
        string xmlPath = $"{Paths.RESOURCES_INPUT_DIR}/Xml.m2d";
        string exportedPath = $"{Paths.RESOURCES_INPUT_DIR}/Exported.m2d";
        string precomputedTerrainPath = $"{Paths.RESOURCES_INPUT_DIR}/PrecomputedTerrain.m2d";

        XmlReader = new(xmlPath);
        ExportedReader = new(exportedPath);
        TokReader = new(precomputedTerrainPath);
    }
}
