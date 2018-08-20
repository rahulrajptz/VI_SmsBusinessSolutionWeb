using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Templateprj.Models
{
    public class IncidentModel
    {
        public string IncidentId { get; set; }

        public string AlertId { get; set; }

        public string SystemId { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Severity")]
        public string SelectedSeverity { get; set; }
        public SelectList SeverityList { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Owner")]
        public string Owner { get; set; }
        public SelectList OwnerList { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Staus { get; set; }
        public SelectList StatusList { get; set; }


        [Display(Name = "Customer Impacted")]
        [Required(ErrorMessage = "Required")]
        public int[] CustomerImpacted { get; set; }
        public MultiSelectList CustomerImpactedList { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Location")]
        public string Location { get; set; }
        public SelectList LocationList { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Problem")]
        public string Problem { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Impact Description")]
        public string ImpactDes { get; set; }

        [Required]
        [Display(Name = "Incident Raised By")]
        public string InsRaisedBy { get; set; }
        public SelectList InsRaisedByList { get; set; }

        [Display(Name = "Mail id")]
        [RegularExpression(@"^(,?[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)+$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string IncMailIds { get; set; }

        [Display(Name = "Resolution")]
        public string Resolution { get; set; }

        [Display(Name = "Root Cause")]
        public string Cause { get; set; }

        [Display(Name = "Corrective & Preventive Action")]
        public string Action { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "IncidentTime")]
        public string IncidentTime { get; set; }

        [Display(Name = "ResolutionTime")]
        public string ResolutionTime { get; set; }

        [Display(Name = "Pending Description")]
        public string Pending { get; set; }

        public HttpPostedFileBase Attachment { get; set; }
    }

    public class QrcModel
    {
        public string QrcId { get; set; }

        public string AlertId { get; set; }

        public string SystemId { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Complaint Source")]
        public string ComplaintType { get; set; }
        public SelectList ComplaintTypeList { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Type")]
        public string Type { get; set; }
        public SelectList TypeList { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Location")]
        public string Location { get; set; }
        public SelectList LocationList { get; set; }

        [Display(Name = "Complaint Status")]
        public string SelectedStatus { get; set; }
        public SelectList StatusList { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Customer Name")]
        public string CusName { get; set; }
        public SelectList CusNameList { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Complaint Date")]
        public string ComplaintDate { get; set; }

        [Display(Name = "Closing Date")]
        public string ClosingDate { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Problem Statement")]
        public string ProblemStmt { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "RDN")]
        public string Rdn { get; set; }

        [Display(Name = "RCA")]
        //[StringLength(10, ErrorMessage = "The Mobile must contains 10 characters", MinimumLength = 10)]
        public string Rca { get; set; }

        [Display(Name = "Sloution")]
        public string Solution { get; set; }

    }

    public class CusModel
    {
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Customer")]
        public string CusName { get; set; }

        [StringLength(11, MinimumLength = 11)]
        // [DataType(DataType.PhoneNumber)]
        [Display(Name = "TFN")]
        public string Tfn { get; set; }

        [Required(ErrorMessage = "Required")]
        //  [DataType(DataType.PhoneNumber)]
        [Display(Name = "RDN")]
        public string Rdn { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Location")]
        public string Location { get; set; }
        public SelectList LocationList { get; set; }

        [Required]
        public string OwnerName { get; set; }

    }

    public class ViewModel
    {

        [Display(Name = "Date Range")]
        public string DateRange { get; set; }
        public string Type { get; set; }
        public string TableNames { get; set; }
    }

    public class ProjectTracker
    {
        public bool cbx { get; set; }

        public string Id { get; set; }

        public string OtherRemarks { get; set; }

        [Required]
        public string Circle { get; set; }
        public SelectList CircleList { get; set; }

        [Required]
        public string Status { get; set; }
        public SelectList StatusList { get; set; }

        [Required]
        public string ProjectName { get; set; }

        [Required]
        public string ProjectRecipientDate { get; set; }

        [StringLength(11, MinimumLength = 10)]
        public string TFN { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10)]
        public string RDN { get; set; }

        public string CustomerSpoc { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string CustomerEmail { get; set; }

        [StringLength(10, MinimumLength = 10)]
        public string CustomerNumber { get; set; }

        public string IdeaSpoc { get; set; }

        public string RfpStatus { get; set; }
        public SelectList RfpStatusList { get; set; }

        public string AppConfigDate { get; set; }

        public string MSCConfigDate { get; set; }

        public string ProjectCompletionDate { get; set; }

        [StringLength(16, MinimumLength = 15)]
        public string IMSI { get; set; }

        public string Remarks { get; set; }

        public string RerecipientofRfp { get; set; }

        public string EIDCommitDate { get; set; }

        public string InternalUatresult { get; set; }
        public SelectList InternalUatresultList { get; set; }

        public string DateWelcomeEmail { get; set; }

        public string TypeOfSolution { get; set; }
        public SelectList TypeOfSolutionList { get; set; }

        public string InternalUat { get; set; }
        public SelectList InternalUatList { get; set; }

        public string RequestFrom { get; set; }

    }

    public class ProjectViewModel
    {
        public string id1 { get; set; }

        public string Daterange { get; set; }

        [StringLength(10, MinimumLength = 10)]
        public string RDNS { get; set; }

        [StringLength(11, MinimumLength = 10)]
        public string TFNS { get; set; }

        public string Circleall { get; set; }
        public SelectList CircleListall { get; set; }

        public string AutoComplt { get; set; }

    }
}