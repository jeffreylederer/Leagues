using System.ComponentModel.DataAnnotations;

namespace Leagues.Models
{
   
    [MetadataType(typeof(PlayerMetaData))]
    public partial class Player
    {
    }
    public class PlayerMetaData
    {

        [Display(Name="First Name")]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(30)]
        public string LastName { get; set; }

        [Display(Name = "In Tuesday League")]
        public bool TuesdayLeague { get; set; }
        [Display(Name = "In Wednesday League")]
        public bool WednesdayLeague { get; set; }

        [Display(Name = "Player")]
        public string FullName { get; set; }
    }

    [MetadataType(typeof(TuesdayMatchMetaData))]
    public partial class TuesdayMatch
    {
    }
    public partial class TuesdayMatchMetaData
    {
        [Display(Name = "Week")]
        public int GameDate { get; set; }

        [Display(Name = "Team 1")]
        public int Team1 { get; set; }

        [Display(Name = "Team 2")]
        public int Team2 { get; set; }


        [Display(Name = "Team 1 Score")]
        public int Team1Score { get; set; }

        [Display(Name = "Team 2 Score")]
        public int Team2Score { get; set; }
    }

    [MetadataType(typeof(WednesdayMatchMetaData))]
    public partial class WednesdayMatch
    {
    }
    public partial class WednesdayMatchMetaData
    {
        [Display(Name = "Week")]
        public int GameDate { get; set; }

        [Display(Name = "Team 1")]
        public int Team1 { get; set; }

        [Display(Name = "Team 2")]
        public int Team2 { get; set; }

        [Display(Name = "Team 1 Score")]
        public int Team1Score { get; set; }

        [Display(Name = "Team 2 Score")]
        public int Team2Score { get; set; }
    }

    [MetadataType(typeof(TuesdayScheduleMetaData))]
    public partial class TuesdaySchedule
    {
    }
    public partial class TuesdayScheduleMetaData
    {
        [Display(Name = "Week Number")]
        [Required]
        public int id { get; set; }

        [Display(Name = "Match Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public System.DateTime GameDate { get; set; }
    }

    [MetadataType(typeof(TuesdayScheduleMetaData))]
    public partial class WednesdaySchedule
    {
    }
    public partial class WednesdayScheduleMetaData
    {
        [Display(Name = "Week Number")]
        [Required]
        public int id { get; set; }

        [Display(Name = "Match Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public System.DateTime GameDate { get; set; }
    }

    [MetadataType(typeof(TuesdayTeamMetaData))]
    public partial class TuesdayTeam
    {
    }
    public partial class TuesdayTeamMetaData
    {
        [Display(Name = "Team Number")]
        [Required]
        public int id { get; set; }
    }

    [MetadataType(typeof(WednesdayTeamMetaData))]
    public partial class WednesdayTeam
    {
    }
    public partial class WednesdayTeamMetaData
    {
        [Display(Name = "Team Number")]
        [Required]
        public int id { get; set; }
    }
}


