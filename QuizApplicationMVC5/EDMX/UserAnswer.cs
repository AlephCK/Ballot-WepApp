//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuizApplicationMVC5.EDMX
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserAnswer
    {
        public int UserAnswerID { get; set; }
        public string UserAnswerText { get; set; }
        public bool Is_Answer { get; set; }
        public int QuestionID { get; set; }
    
        public virtual Question Question { get; set; }
    }
}
