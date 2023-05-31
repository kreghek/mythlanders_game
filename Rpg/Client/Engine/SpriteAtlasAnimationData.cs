using System.Collections.Generic;

namespace Client.Engine;

/// <summary>
/// Dto to load animations from file.
/// </summary>
internal record SpriteAtlasAnimationData(SpriteAtlasAnimationDataTextureAtlas TextureAtlas,
    Dictionary<string, SpriteAtlasAnimationDataCycles> Cycles);