using System;
using CqrsRadio.Domain.Entities;

namespace CqrsRadio.Domain.Services
{
    public interface IRadioEngine   
    {
        RadioSong Parse(Uri url);
    }
}