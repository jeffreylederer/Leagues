﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Leagues.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=LeaguesEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<TuesdayMatch> TuesdayMatches { get; set; }
        public virtual DbSet<TuesdaySchedule> TuesdaySchedules { get; set; }
        public virtual DbSet<TuesdayTeam> TuesdayTeams { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<WednesdayMatch> WednesdayMatches { get; set; }
        public virtual DbSet<WednesdaySchedule> WednesdaySchedules { get; set; }
        public virtual DbSet<WednesdayTeam> WednesdayTeams { get; set; }
    }
}