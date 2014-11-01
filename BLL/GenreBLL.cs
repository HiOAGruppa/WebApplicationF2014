using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Data.Entity;
using Model;
using System.Diagnostics;

namespace BLL
{
    public class GenreBLL : BLL.IGenreBLL
    {
StoreContext db;

        private IStoreManagerRepository _repository;

        public GenreBLL() {
            db = new StoreContext();
            _repository = new StoreManagerRepository();
        }
        public GenreBLL(IStoreManagerRepository stub)
        {
            db = new StoreContext();
            _repository = stub;
        }

        public DbSet<Genre> getGenres()
        {
            return db.Genres;
        }
        
        public Genre getSelectedGenre(string genre)
        {
            var genreModel = db.getSelectedGenre(genre);
            return genreModel;
        }
    }

}
