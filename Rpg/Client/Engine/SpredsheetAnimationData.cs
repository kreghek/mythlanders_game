using System.Collections.Generic;

namespace Client.Engine;
internal record SpredsheetAnimationData(SpredsheetAnimationDataTextureAtlas TextureAtlas, Dictionary<string, SpredsheetAnimationDataCycles> Cycles);
