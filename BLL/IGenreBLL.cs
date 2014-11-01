using System;
namespace BLL
{
    public interface IGenreBLL
    {
        System.Data.Entity.DbSet<Model.Genre> getGenres();
        Model.Genre getSelectedGenre(string genre);
    }
}
