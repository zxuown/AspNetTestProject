using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace AspNetTest.Models;

public class SiteContext : IdentityDbContext<User, IdentityRole<int>, int>
{
	public SiteContext(DbContextOptions options) : base(options) { }
	public virtual DbSet<Abilities> Abilities { get; set; } = null!;
	public virtual DbSet<AboutMe> AboutMe { get; set; } = null!;

	public virtual DbSet<NewsItem> News { get; set; } = null!;

	public virtual DbSet<Marker> Markers { get; set; } = null!;

    public virtual DbSet<ToDoList> ToDoLists { get; set; } = null!;

    public override DbSet<User> Users { get; set; } = null!;
}
