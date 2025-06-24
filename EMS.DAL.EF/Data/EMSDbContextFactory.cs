using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EMS.DAL.EF.Data;

public class EMSDbContextFactory : IEMSDbContextFactory
{
    private readonly string _dbConnectionString;

    public EMSDbContextFactory(string dbConnectionString)
    {
        _dbConnectionString = dbConnectionString;
    }
    /*
    public EMSManagmentDbContext CreateDbContext()
    {
        DbContextOptionsBuilder<EMSManagmentDbContext> builder = new();
        builder.UseMySql(_dbConnectionString, ServerVersion.AutoDetect(_dbConnectionString));
        return new EMSManagmentDbContext(builder.Options);
    }*/
}