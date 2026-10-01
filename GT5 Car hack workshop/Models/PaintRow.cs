using System;
using System.Collections.Generic;

namespace GT5_Car_hack_workshop.Models
{
    /// <summary>
    /// One row of the paint colour browser's grid: a handful of tiles.
    ///
    /// The grid is chunked into rows so a plain virtualising list can drive it. Avalonia 12.1.2 has
    /// no virtualising wrap/uniform panel, and laying out all 3408 tiles at once takes over a second
    /// (measured), whereas virtualising by row takes about 30 ms.
    /// </summary>
    public sealed class PaintRow
    {
        public IReadOnlyList<PaintTile> Tiles { get; init; } = Array.Empty<PaintTile>();
    }
}
