using System.Collections.Generic;

namespace CqrsRadio.Test.SongEngine
{
    public interface ISongEngine    
    {
        IEnumerable<string> GetRandomisedSongs(int length);
    }
}
