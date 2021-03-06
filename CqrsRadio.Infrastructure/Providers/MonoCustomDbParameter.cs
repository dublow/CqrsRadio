﻿using System.Data.Common;
using Mono.Data.Sqlite;

namespace CqrsRadio.Infrastructure.Providers
{
    public class MonoCustomDbParameter : IDbParameter
    {
        public DbParameter Create(string name, object value)
        {
            return new SqliteParameter(name, value);
        }
    }
}
