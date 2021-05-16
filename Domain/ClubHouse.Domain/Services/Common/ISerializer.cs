using System;
using System.Collections.Generic;

namespace ClubHouse.Domain.Services.Common {
    public interface ISerializer {
        string Serialize<T>(T model, object options = null);
        T Deserialize<T>(string json, object options = null);
        T SafeDeserialize<T>(string json, object options = null);
    }
}
