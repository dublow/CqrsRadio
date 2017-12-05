using System.Collections.Generic;

namespace CqrsRadio.Domain.Services
{
    public interface ISongEngine    
    {
        IEnumerable<string> GetRandomisedSongs(int length);
    }
}
