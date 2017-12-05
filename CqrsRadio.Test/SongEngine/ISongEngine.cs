using System.Collections.Generic;
using CqrsRadio.Domain.Entities;

namespace CqrsRadio.Test.SongEngine
{
    public interface ISongEngine    
    {
        IEnumerable<DeezerSong> GetRandomisedSongs(int length);
    }
}
