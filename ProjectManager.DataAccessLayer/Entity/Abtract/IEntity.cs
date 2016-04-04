using System;

namespace ProjectManager.DataAccessLayer.Entity.Abtract
{
    public interface IEntity
    {
        Guid Key { get; set; }
    }
}