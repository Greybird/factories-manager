//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FactoriesManager.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TestRunResult
    {
        public string SourceName { get; set; }
        public int ReportId { get; set; }
        public int Id { get; set; }
        public string Content { get; set; }
        public byte[] RowVersion { get; set; }
    
        public virtual TestReport TestReport { get; set; }
    }
}