//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class TuesdayMatch
    {
        public int id { get; set; }
        public int GameDate { get; set; }
        public int Rink { get; set; }
        public int Team1 { get; set; }
        public int Team2 { get; set; }
    
        public virtual TuesdaySchedule TuesdaySchedule { get; set; }
        public virtual TuesdayTeam TuesdayTeam { get; set; }
        public virtual TuesdayTeam TuesdayTeam1 { get; set; }
    }
}
