using GeoRecords.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GeoRecords.Services
{
    public class AppDbContext : DbContext, IDataStore<StoredLocation>
    {
        private string _dbPath;

        public AppDbContext(string dbPath)
        {
            _dbPath = dbPath;
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<StoredLocation> Locations { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            base.OnConfiguring(options);
            options.UseSqlite($"Data Source={_dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> AddItemAsync(StoredLocation item)
        {
            try
            {
                await Locations.AddAsync(item);
                await SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateItemAsync(StoredLocation item)
        {
            try
            {
                Locations.Update(item);
                await SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            try
            {
                var location = await Locations.FirstOrDefaultAsync(s => s.Id == id);
                if (location != null)
                {
                    Locations.Remove(location);
                }

                await SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<StoredLocation> GetItemAsync(int id)
        {
            var location = await Locations.FirstOrDefaultAsync(s => s.Id == id).ConfigureAwait(false);
            return location;
        }

        public async Task<IEnumerable<StoredLocation>> GetItemsAsync(bool forceRefresh = false)
        {
            var allLocations = await Locations.OrderBy(s => s.Date).ToListAsync();
            return allLocations;
        }

        public void ClearItems()
        {
            Locations.RemoveRange(Locations);
            SaveChanges();
        }
    }
}
