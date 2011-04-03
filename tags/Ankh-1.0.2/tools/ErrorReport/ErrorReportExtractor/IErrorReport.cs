﻿using System;
namespace ErrorReportExtractor
{
    public interface IErrorReport : IMailItem
    {
        int ErrorReportID { get; }
        int? MajorVersion { get; }
        int? MinorVersion { get; }
        int? PatchVersion { get; }
        int? Revision { get; }
        string ExceptionType { get; }
        string DteVersion { get; }
        IStackTrace StackTrace { get; }
        bool RepliedTo { get; set; }
        bool HasReplies { get; set; }
        string ExceptionMessage { get; set; }
    }
}