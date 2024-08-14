using FamilyTree.Models.Master;
using Microsoft.EntityFrameworkCore;

namespace FamilyTree.Data;

public partial class DataContext : DbContext
{
    private readonly IConfiguration _configuration;
    //public DataContext(IConfiguration configuration)
    //{
    //    _configuration = configuration;
    //}

    public DataContext(DbContextOptions options)
        : base(options)
    {
    }

    public virtual DbSet<Person> Person { get; set; }
    public virtual DbSet<Occupation> Occupation { get; set; }
    public virtual DbSet<Qualification> Qualification { get; set; }

    public virtual DbSet<RelationMapping> RelationMapping { get; set; }

    public virtual DbSet<RelationType> RelationType { get; set; }
    public virtual DbSet<UserRole> UserRole { get; set; }
    public virtual DbSet<User> User { get; set; }

    //public virtual DbSet<AllParentMapping> ParentChildMapping { get; set; }

}
