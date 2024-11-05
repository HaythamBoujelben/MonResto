﻿using Microsoft.EntityFrameworkCore;
using MonRestoAPI.Models;

public class MonRestoAPIContext : DbContext
{
    public MonRestoAPIContext(DbContextOptions<MonRestoAPIContext> options) : base(options) { }

    public DbSet<Article> Articles { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
}